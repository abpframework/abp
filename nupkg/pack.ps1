. ".\common.ps1"

# Rebuild all solutions
foreach($solution in $solutions) {
    $solutionFolder = Join-Path $rootFolder $solution
    Set-Location $solutionFolder
    & dotnet restore
}

# Create all packages
$i = 0
foreach($project in $projects) {
    $i += 1
    $projectFolder = Join-Path $rootFolder $project
	$projectName = ($project -split '/')[-1]
	
	# Create nuget pack
	Write-Host ("-----===[ $i / " + $projects.length  + " - " + $projectName + " ]===-----")
    Set-Location $projectFolder
    Remove-Item -Force -Recurse (Join-Path $projectFolder "bin/Release")
    & dotnet pack -c Release

    if (-Not $?) {
        Write-Host ("Packaging failed for the project: " + $projectFolder)
        exit $LASTEXITCODE
    }
    
    # Copy nuget package
    $projectName = $project.Substring($project.LastIndexOf("/") + 1)
    $projectPackPath = Join-Path $projectFolder ("/bin/Release/" + $projectName + ".*.nupkg")
    Move-Item -Force $projectPackPath $packFolder
}

# Go back to the pack folder
Set-Location $packFolder