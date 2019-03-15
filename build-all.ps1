# COMMON PATHS

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions

$solutionPaths = (
    "framework",
    "modules/users",
    "modules/permission-management",
    "modules/setting-management",
    "modules/identity",
    "modules/identityserver",
    "modules/tenant-management",
    "modules/account",
    "modules/docs",
    "modules/blogging",
    "modules/audit-logging",
    "modules/background-jobs",
    "abp_io",
    "templates/module",
    "templates/service",
    "templates/mvc",
    "samples/MicroserviceDemo"
)

# Specific Dictionary when the path contains more than one sln/proj
$solutionPathsSpecificNames = @{
	"samples/MicroserviceDemo" = "MicroserviceDemo.sln"
}

# Build all solutions

foreach ($solutionPath in $solutionPaths) {    
    $solutionAbsPath = (Join-Path $rootFolder $solutionPath)
    Set-Location $solutionAbsPath

	# Added exceptional scenario when we have more than one sln in the directory
	if ( $solutionPathsSpecificNames.ContainsKey(($solutionPath))) {
		$sln = $solutionPathsSpecificNames[$solutionPath]
		dotnet build $sln
	}
	else
	{
		dotnet build
	}
    
    if (-Not $?) {
        Write-Host ("Build failed for the solution: " + $solutionPath)
        Set-Location $rootFolder
        exit $LASTEXITCODE
    }
}

Set-Location $rootFolder
