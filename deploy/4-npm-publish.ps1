param(
  [string]$npmAuthToken
) 

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

echo "`n-----=====[ SETTING NPM AUTH TOKEN ]=====-----`n"
npm set //registry.npmjs.org/:_authToken $npmAuthToken

echo "`n-----=====[ PUSHING MVC PACKS TO NPM ]=====-----`n"
powershell -File publish-mvc.ps1

echo "`n-----=====[ PUSHING ANGULAR PACKS TO NPM ]=====-----`n"
powershell -File publish-ng.ps1

echo "`n-----=====[ PUSHING PACKS TO NPM COMPLETED ]=====-----`n"
