param(
  [string]$nugetApiKey
) 

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

echo "`n-----=====[ PUSHING PACKS TO NUGET ]=====-----`n"

cd ..\nupkg
powershell -File push_packages.ps1 $nugetApiKey

echo "`n-----=====[ PUSHING PACKS TO NUGET COMPLETED ]=====-----`n"
