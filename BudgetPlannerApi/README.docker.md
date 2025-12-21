# Running BudgetPlannerApi with Docker (development)

Quick steps to run the API and Postgres with Docker Compose.

1. Copy `.env.example` to `.env` and edit if needed:

```bash
cp .env.example .env
# edit .env and set secure passwords before running in any shared environment
```

2. Start Docker Desktop (macOS) or ensure the Docker daemon is running.

3. Start the stack from repository root:

```bash
./BudgetPlannerApi/start-and-run.sh
```

The helper script now detects if the configured `API_HOST_PORT` is already in use and will pick a free host port automatically and run the compose stack with that port. The chosen port is printed by the script (so you can update `.env` if you prefer a permanent change).

Or run compose directly (useful for CI or manual runs):

```bash
docker compose -f BudgetPlannerApi/docker-compose.yml up --build
```

4. The API will be available on http://localhost:5000 by default (override `API_HOST_PORT` in `.env` if needed).

Notes and industry-grade choices made:

- Credentials live in an `.env` file (add to `.gitignore` in production) instead of being hard-coded in compose.
- The API image is built using a multi-stage Dockerfile to produce a small runtime image.
- The runtime image includes a tiny entrypoint script that waits for Postgres to be ready using `pg_isready` before starting the app.
- Compose uses `depends_on` with a Postgres healthcheck to ensure the DB container is healthy before starting the API container.
- For production, consider:
  - Using Docker secrets or a secret manager instead of `.env`.
  - Enabling HTTPS and adding a reverse proxy (nginx) or use a platform that terminates TLS.
  - Running the app in `ASPNETCORE_ENVIRONMENT=Production` and setting appropriate connection strings.
  - Adding automated migrations or migration pipelines (CI) rather than running migrations at container start.

Troubleshooting:

- If `docker compose` fails with "Cannot connect to the Docker daemon", start Docker Desktop and try again.
- To view container logs:

```bash
docker compose -f BudgetPlannerApi/docker-compose.yml logs -f api
```

- To rebuild images after code changes:

```bash
docker compose -f BudgetPlannerApi/docker-compose.yml up --build -d
```
