# Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# restore
# Dockerfile is located inside the `BudgetPlannerApi` folder, so COPY paths are relative to that
COPY ["BudgetPlannerApi.csproj", "./"]
RUN dotnet restore "./BudgetPlannerApi.csproj"
# copy everything and build
COPY . .
WORKDIR /src
RUN  dotnet build "BudgetPlannerApi.csproj" -c Release -o /app/build

# Stage 2: Publish Stage
FROM build AS publish
RUN dotnet publish "BudgetPlannerApi.csproj" -c Release -o /app/publish /p:PublishTrimmed=false

# Stage 3: Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
ENV ASPNETCORE_HTTP_PORTS=5001
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetPlannerApi.dll"]