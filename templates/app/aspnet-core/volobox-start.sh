#!/usr/bin/env bash
# Volobox preview startup script for the ABP "app" (aspnet-core, MVC + EF Core) template.
# Waits for SQL Server + Redis, applies EF Core migrations/seed data, then starts the Web host.
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

wait_for_port() {
  local host="$1" port="$2" retries="${3:-90}"
  for ((i = 1; i <= retries; i++)); do
    if (exec 3<>"/dev/tcp/${host}/${port}") 2>/dev/null; then
      exec 3>&- 3<&- || true
      return 0
    fi
    sleep 2
  done
  echo "Timed out waiting for ${host}:${port}" >&2
  return 1
}

echo "==> Waiting for SQL Server (127.0.0.1:1433)..."
wait_for_port 127.0.0.1 1433

echo "==> Waiting for Redis (127.0.0.1:6379)..."
wait_for_port 127.0.0.1 6379

export ConnectionStrings__Default="Server=127.0.0.1,1433;Database=MyProjectName;User Id=sa;Password=${MSSQL_SA_PASSWORD};TrustServerCertificate=True"

echo "==> Applying database migrations and seed data..."
dotnet run --project src/MyCompanyName.MyProjectName.DbMigrator/MyCompanyName.MyProjectName.DbMigrator.csproj

echo "==> Starting the Web host..."
exec dotnet run --project src/MyCompanyName.MyProjectName.Web/MyCompanyName.MyProjectName.Web.csproj
