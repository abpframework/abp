param(
  [string]$npmAuthToken
) 

. ..\nupkg\common.ps1

if (!$npmAuthToken)
{	
	#reading password from file content
	$passwordFileName = "npm-auth-token.txt" 
	$pathExists = Test-Path -Path $passwordFileName -PathType Leaf
	if ($pathExists)
	{
		$npmAuthToken = Get-Content $passwordFileName
		echo "Using NPM Auth Token from $passwordFileName ..." 
	}
}
 
if (!$npmAuthToken)
{
	$npmAuthToken = Read-Host "Enter the NPM Auth Token"
}

cd ..\npm

#----------------------------------------------------------------------------

echo "`nSetting npmjs.org auth-token token`n"
npm set //registry.npmjs.org/:_authToken $npmAuthToken

#----------------------------------------------------------------------------

Write-Info "Pushing MVC packages to NPM"
echo "`n-----=====[ PUSHING MVC PACKS TO NPM ]=====-----`n"
powershell -File publish-ng.ps1
echo "`n-----=====[ PUSHING MVC PACKS TO NPM COMPLETED ]=====-----`n"
Write-Info "Pushing MVC packages to NPM completed"

cd ..\deploy #always return to the deploy directory
