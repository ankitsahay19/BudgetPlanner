#!/usr/bin/env bash
set -euo pipefail

# entrypoint that waits for Postgres to be ready before starting the app
DB_HOST=${DB_HOST:-postgres}
DB_PORT=${DB_PORT:-5432}
DB_USER=${DB_USER:-postgres}
MAX_WAIT_SECONDS=${MAX_WAIT_SECONDS:-60}

echo "[entrypoint] waiting for database at ${DB_HOST}:${DB_PORT} (user=${DB_USER})"

elapsed=0
until pg_isready -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" >/dev/null 2>&1; do
  if [ "$elapsed" -ge "$MAX_WAIT_SECONDS" ]; then
    echo "[entrypoint] timeout waiting for Postgres after ${MAX_WAIT_SECONDS}s"
    exit 1
  fi
  sleep 1
  elapsed=$((elapsed+1))
done

# Optionally run EF migrations here if desired (uncomment to enable)
# echo "[entrypoint] applying EF migrations"
# dotnet ef database update || true

echo "[entrypoint] database is ready, starting app"
exec dotnet BudgetPlannerApi.dll
