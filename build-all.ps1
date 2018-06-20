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
    "modules/blog"
)

# Build all solutions

foreach ($solutionsPath in $solutionsPaths) {    
    $solutionAbsPath = (Join-Path $rootFolder $solutionsPath)
    Set-Location $solutionAbsPath
    dotnet build
}

Set-Location $rootFolder