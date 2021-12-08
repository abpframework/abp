param(
  [string]$branch
) 

if (!$branch)
{
	$branch = Read-Host "Enter the branch name"
} 

echo "`n-----=====[ PULLING ABP REPO - BRANCH: $branch ]=====-----`n"

cd ..
git switch $branch
git pull origin

echo "`n-----=====[ BUILDING ALL PROJECTS ]=====-----`n"
cd build
.\build-all.ps1

echo "`n-----=====[ BUILDING ALL PROJECTS COMPLETED]=====-----`n"