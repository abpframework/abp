# COMMON PATHS

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions

$solutionsPaths = (
    "framework",
    "modules/users",
    "modules/permission-management",
    "modules/setting-management",
    "modules/identity",
    "modules/tenant-management",
    "modules/account",
    "modules/docs",
    "modules/blogging"
)

# Build all solutions

foreach ($solutionsPath in $solutionsPaths) {    
    $solutionAbsPath = (Join-Path $rootFolder $solutionsPath)
    Set-Location $solutionAbsPath
    dotnet build
    if (-Not $?) {
        Write-Host ("Build failed for the solution: " + $solutionsPath)
        exit $LASTEXITCODE
    }
}

Set-Location $rootFolder
