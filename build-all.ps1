# COMMON PATHS

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions

$solutionPaths = (
    "framework",
    "modules/users",
    "modules/permission-management",
    "modules/setting-management",
    "modules/identity",
    "modules/tenant-management",
    "modules/account",
    "modules/docs",
    "modules/blogging",
    "modules/audit-logging",
    "modules/background-jobs"
)

# Build all solutions

foreach ($solutionPath in $solutionPaths) {    
    $solutionAbsPath = (Join-Path $rootFolder $solutionPath)
    Set-Location $solutionAbsPath
    dotnet build
    if (-Not $?) {
        Write-Host ("Build failed for the solution: " + $solutionPath)
        exit $LASTEXITCODE
    }
}

Set-Location $rootFolder
