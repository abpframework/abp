# seed : runs the target's DbMigrator on an empty "Bench" database, adds the load-test data (seed-bench-data.sql)
#        and keeps the result as template "Bench_seed_<target>".
# reset: recreates "Bench" from that template, so every run starts from the same data set.
param(
    [Parameter(Mandatory)][ValidateSet('abp10', 'abp11', 'abp10-rt11')][string]$Target,
    [Parameter(Mandatory)][ValidateSet('seed', 'reset')][string]$Action
)
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$Target = $Target -replace '-rt11$', '' # same build and data set as abp10
$seedDb = "Bench_seed_$Target"

function Invoke-Sql([string]$Sql, [string]$Database = 'postgres') {
    # SQL goes through stdin to avoid quoting issues in native arguments.
    $Sql | docker exec -i bench-postgres psql -U bench -d $Database -v ON_ERROR_STOP=1 -q
    if ($LASTEXITCODE) { throw "psql failed: $Sql" }
}

# Dropping the database under a running app breaks its pooled connections (requests then fail with 500).
if (Get-NetTCPConnection -LocalPort 44377 -State Listen -ErrorAction SilentlyContinue) {
    throw 'The web app is running on port 44377; stop it before seeding/resetting the database.'
}

for ($i = 0; $i -lt 30; $i++) {
    docker exec bench-postgres pg_isready -U bench -q
    if (-not $LASTEXITCODE) { break }
    Start-Sleep -Seconds 1
}
if ($LASTEXITCODE) { throw 'PostgreSQL is not ready (docker compose up -d?)' }

if ($Action -eq 'seed') {
    Invoke-Sql 'DROP DATABASE IF EXISTS "Bench" WITH (FORCE);'
    . "$PSScriptRoot/env.ps1"
    Push-Location "$root/publish/$Target/migrator" # appsettings.json is read from the working directory
    try {
        dotnet Bench.DbMigrator.dll
        if ($LASTEXITCODE) { throw 'DbMigrator failed' }
    }
    finally { Pop-Location }
    Invoke-Sql (Get-Content "$PSScriptRoot/seed-bench-data.sql" -Raw) -Database 'Bench'
    Invoke-Sql "DROP DATABASE IF EXISTS `"$seedDb`";"
    Invoke-Sql "CREATE DATABASE `"$seedDb`" TEMPLATE `"Bench`";"
}

Invoke-Sql 'DROP DATABASE IF EXISTS "Bench" WITH (FORCE);'
Invoke-Sql "CREATE DATABASE `"Bench`" TEMPLATE `"$seedDb`";"
Write-Host "Bench database is ready ($Target)"
