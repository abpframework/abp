param(
  [string]$branch,
  [string]$newVersion,
  [string]$isRcVersion
) 

. ..\nupkg\common.ps1

if (!$branch)
{
	$branch = Read-Host "Enter the branch name"
} 

if (!$newVersion)
{
	$currentVersion = Get-Current-Version
	$newVersion = Read-Host "Current version is '$currentVersion'. Enter the new version (empty for no change) "
	if($newVersion -eq "")
	{
		$newVersion = $currentVersion
	}
} 
	 
if ($isRcVersion -eq "")
{
	$isRcVersion = Read-Host "Is this a RC/Preview version? (y/n)"	
}

$publishGithubReleaseParams = @{
    branchName=$branch 
	isRcVersion=$isRcVersion 
}
  

./1-fetch-and-build.ps1 $branch $newVersion
./2-nuget-pack.ps1
./3-nuget-push.ps1
./4-npm-publish-mvc.ps1
./5-npm-publish-angular.ps1
./6-git-commit.ps1
./7-publish-github-release.ps1 @publishGithubReleaseParams
./8-download-release-zip.ps1

