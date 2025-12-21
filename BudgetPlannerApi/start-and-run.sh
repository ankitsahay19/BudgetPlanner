#!/usr/bin/env bash
# Helper to start Docker Desktop on macOS (if needed) and bring up the compose stack
# Usage: ./start-and-run.sh

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
COMPOSE_FILE="$SCRIPT_DIR/docker-compose.yml"

echo "Checking for Docker daemon..."
if ! docker info >/dev/null 2>&1; then
  echo "Docker daemon not running. Attempting to start Docker Desktop (macOS)..."
  if [[ "$(uname)" == "Darwin" ]]; then
    open -a Docker || true
    echo "Waiting for Docker to start (this can take a while)..."
    until docker info >/dev/null 2>&1; do
      sleep 2
      printf "."
    done
    echo "\nDocker is running"
  else
    echo "Docker is not running. Please start Docker and rerun this script."
    exit 1
  fi
else
  echo "Docker daemon is available"
fi

# Load .env from the script dir if present so default port can be read
if [ -f "$SCRIPT_DIR/.env" ]; then
  # shellcheck disable=SC1090
  set -o allexport
  # shellcheck source=/dev/null
  source "$SCRIPT_DIR/.env"
  set +o allexport
fi

# Determine desired host port; default to 5000
DESIRED_PORT=${API_HOST_PORT:-5000}

port_in_use() {
  local port=$1
  if command -v lsof >/dev/null 2>&1; then
    sudo lsof -nP -iTCP:"$port" -sTCP:LISTEN >/dev/null 2>&1
    return $?
  else
    # fallback: use netstat if lsof isn't available
    if command -v netstat >/dev/null 2>&1; then
      netstat -an | grep "\.${port} .*LISTEN" >/dev/null 2>&1
      return $?
    fi
    return 1
  fi
}

find_free_port() {
  # Use python to pick an available port
  if command -v python3 >/dev/null 2>&1; then
    python3 - <<'PY'
import socket
s=socket.socket()
s.bind(("",0))
print(s.getsockname()[1])
s.close()
PY
  else
    # fallback simple scan
    for p in $(seq 5001 6000); do
      if ! port_in_use "$p"; then
        echo "$p"
        return 0
      fi
    done
    return 1
  fi
}

SELECTED_PORT=$DESIRED_PORT
if port_in_use "$DESIRED_PORT"; then
  echo "Port $DESIRED_PORT is in use on the host. Finding a free port..."
  FREE_PORT=$(find_free_port)
  if [ -n "$FREE_PORT" ]; then
    echo "Using fallback host port $FREE_PORT"
    SELECTED_PORT=$FREE_PORT
  else
    echo "Could not find a free host port to map the API. Please free port $DESIRED_PORT or set API_HOST_PORT in .env"
    exit 1
  fi
fi

echo "Bringing up services with compose file: $COMPOSE_FILE (API host port -> $SELECTED_PORT)"
API_HOST_PORT=$SELECTED_PORT docker compose -f "$COMPOSE_FILE" up --build -d

echo "Services started (or are starting). Use 'docker compose -f $COMPOSE_FILE ps' to view status."

echo "API should be reachable at http://localhost:${SELECTED_PORT}"

# If desired, open the browser to the API swagger page (unless NO_OPEN_BROWSER=1)
if [ "${NO_OPEN_BROWSER:-0}" != "1" ]; then
  SWAGGER_PATH="/swagger"
  URL="http://localhost:${SELECTED_PORT}${SWAGGER_PATH}"

  echo "Waiting for API to respond at ${URL} (will wait up to ${MAX_WAIT_SECONDS:-60}s)"
  elapsed=0
  until curl -sSf "$URL" >/dev/null 2>&1; do
    if [ "$elapsed" -ge "${MAX_WAIT_SECONDS:-60}" ]; then
      echo "Timed out waiting for API to respond at ${URL}. You can open it manually later."
      break
    fi
    sleep 1
    elapsed=$((elapsed+1))
  done

  if curl -sSf "$URL" >/dev/null 2>&1; then
    echo "Opening browser to ${URL}"
    if command -v open >/dev/null 2>&1; then
      open "$URL"
    elif command -v xdg-open >/dev/null 2>&1; then
      xdg-open "$URL" >/dev/null 2>&1 || true
    else
      echo "No known 'open' command found. Please open ${URL} in your browser."
    fi
  fi
else
  echo "Skipping automatic browser open because NO_OPEN_BROWSER=${NO_OPEN_BROWSER:-0}"
fi
