param(
  [string]$branch,
  [string]$newVersion
) 

if (!$branch)
{
	$branch = Read-Host "Enter the branch name"
} 

# Read the current version from common.props
$commonPropsFilePath = resolve-path "../common.props"
$commonPropsXmlCurrent = [xml](Get-Content $commonPropsFilePath ) 
$currentVersion = $commonPropsXmlCurrent.Project.PropertyGroup.Version.Trim()

if (!$newVersion)
{
	$newVersion = Read-Host "Current version is '$currentVersion'. Enter the new version (empty for no change) "
	if($newVersion -eq "")
	{
		$newVersion = $currentVersion
	}
} 

if ($newVersion -ne $currentVersion){
	# Update common.props for version attribute
	$commonPropsXmlCurrent.Project.PropertyGroup.Version = $newVersion
	$commonPropsXmlCurrent.Save( $commonPropsFilePath )
	#check if it's updated...
	$commonPropsXmlNew = [xml](Get-Content $commonPropsFilePath ) 
	$newVersionAfterUpdate = $commonPropsXmlNew.Project.PropertyGroup.Version
	echo "`n`nNew version updated as '$newVersionAfterUpdate' in $commonPropsFilePath`n"
}


echo "`n-----=====[ PULLING ABP REPO - BRANCH: $branch ]=====-----`n"
cd ..
git switch $branch
git pull origin

echo "`n-----=====[ BUILDING ALL PROJECTS ]=====-----`n"
cd build
.\build-all.ps1 -f
echo "`n-----=====[ BUILDING ALL PROJECTS COMPLETED]=====-----`n"