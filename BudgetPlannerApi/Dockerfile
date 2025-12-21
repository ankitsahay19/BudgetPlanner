# multi-stage Dockerfile for BudgetPlannerApi
# Build with the SDK, produce a trimmed runtime image for production/development

ARG BASE_IMAGE_SDK="mcr.microsoft.com/dotnet/sdk:9.0"
ARG BASE_IMAGE_RUNTIME="mcr.microsoft.com/dotnet/aspnet:9.0"

FROM ${BASE_IMAGE_SDK} AS build
WORKDIR /src

# copy csproj and restore first (leverage layer caching)
COPY *.sln .
COPY BudgetPlannerApi.csproj ./
# copy any Directory.Build.* or nuget config if present
RUN dotnet restore --no-cache

# copy everything and build
COPY . .
RUN dotnet publish -c Release -o /app/publish /p:PublishTrimmed=false

# runtime image
FROM ${BASE_IMAGE_RUNTIME} AS final
WORKDIR /app

# install small client tool (pg_isready) so the entrypoint script can wait for Postgres
USER root
RUN apt-get update \
    && apt-get install -y --no-install-recommends postgresql-client curl \
    && rm -rf /var/lib/apt/lists/*

# create non-root user
RUN useradd -m -u 1000 appuser || true

COPY --from=build /app/publish .
COPY docker-entrypoint.sh /usr/local/bin/docker-entrypoint.sh

RUN chmod +x /usr/local/bin/docker-entrypoint.sh \
    && chown -R appuser:appuser /app

USER appuser

ENV ASPNETCORE_URLS="http://+:80"
EXPOSE 80

ENTRYPOINT ["/usr/local/bin/docker-entrypoint.sh"]
