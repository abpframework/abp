param(
  [string]$nugetApiKey
) 

. ..\nupkg\common.ps1

if (!$nugetApiKey)
{	
	#reading password from file content
	$passwordFileName = "nuget-api-key.txt" 
	$pathExists = Test-Path -Path $passwordFileName -PathType Leaf
	if ($pathExists)
	{
		$nugetApiKey = Get-Content $passwordFileName
		echo "Using NuGet API Key from $passwordFileName ..." 
	}
}
 
if (!$nugetApiKey)
{
	$nugetApiKey = Read-Host "Enter the NuGet API KEY"
}

Write-Info "Pushing packages to NuGet"
echo "`n-----=====[ PUSHING PACKAGES TO NUGET ]=====-----`n"
cd ..\nupkg
powershell -File push_packages.ps1 $nugetApiKey
echo "`n-----=====[ PUSHING PACKAGES TO NUGET COMPLETED ]=====-----`n"
Write-Info "Pushing packages to NuGet Completed"

cd ..\deploy #always return to the deploy directory