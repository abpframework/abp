$ErrorActionPreference = "Stop"
$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$solutionRoot = Join-Path $scriptRoot "../../"

function Write-StudioStep([string]$Message) {
    [Console]::Out.WriteLine("[STUDIO:STEP] $Message")
    [Console]::Out.Flush()
}

Write-StudioStep "Building solution"
Set-Location $solutionRoot
dotnet build

if ($LASTEXITCODE -ne 0) {
    [Console]::Error.WriteLine("dotnet build FAILED with exit code $LASTEXITCODE")
    exit -1
}

$jobs = @()

Write-StudioStep "Installing client-side libraries"
$jobs += Start-Job -Name "InstallLibs" -ScriptBlock {
    $ErrorActionPreference = "Stop"
    Set-Location (Join-Path $using:scriptRoot "../../")
    abp install-libs

    if ($LASTEXITCODE -ne 0) {
        throw "abp install-libs exited with code $LASTEXITCODE"
    }
}

$jobs += Start-Job -Name "DbMigrator" -ScriptBlock {
    $ErrorActionPreference = "Stop"
    $migratorDir = Join-Path $using:scriptRoot "../../src/Bench.DbMigrator"
    $efProjectDir = Join-Path $using:scriptRoot "../../src/Bench.EntityFrameworkCore"
    # The CLI creates the migration and then runs the migrator. Do not start another
    # dotnet run until that process exits; a parallel run can seed before AbpSettings exists.
    if (-not (Test-Path (Join-Path $efProjectDir "Migrations"))) {
        & abp create-migration-and-run-migrator $efProjectDir
    }
    else {
        Set-Location $migratorDir
        dotnet run
    }

    if ($LASTEXITCODE -ne 0) {
        throw "DbMigrator exited with code $LASTEXITCODE"
    }
}

Write-StudioStep "Creating development certificate"
$jobs += Start-Job -Name "DevCert" -ScriptBlock {
    $ErrorActionPreference = "Stop"
    Set-Location (Join-Path $using:scriptRoot "../../src/Bench.Web")
    dotnet dev-certs https -v -ep openiddict.pfx -p 9b3fcdc9-9e29-4862-96c7-18239226ee42

    if ($LASTEXITCODE -ne 0) {
        throw "dotnet dev-certs exited with code $LASTEXITCODE"
    }
}


Write-StudioStep "Waiting for initialization jobs"
while (($jobs | Where-Object { $_.State -eq 'Running' }).Count -gt 0) {
    $running = ($jobs | Where-Object { $_.State -eq 'Running' } | ForEach-Object { $_.Name }) -join ', '
    if (-not [string]::IsNullOrWhiteSpace($running)) {
        Write-StudioStep "Running: $running"
    }
    Start-Sleep -Seconds 2
}

Wait-Job $jobs | Out-Null
# Native tools can write warnings to stderr; keep them visible without failing completed jobs.
$jobs | Receive-Job -ErrorAction Continue

$failed = $jobs | Where-Object { $_.State -eq 'Failed' }
$hasError = $failed.Count -gt 0

if ($hasError) {
    foreach ($job in $failed) {
        [Console]::Error.WriteLine("Job '$($job.Name)' FAILED")
    }

    Remove-Job $jobs | Out-Null
    exit -1
}

Remove-Job $jobs | Out-Null
exit 0
