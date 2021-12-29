param(
  [string]$password
) 
 
if (!$password)
{	
	#reading password from file content
	$sshPasswordFileName = "ssh-password.txt" 
	$password = Get-Content $sshPasswordFileName
	echo "Using SSH password from $sshPasswordFileName ..."
}
 
if (!$password)
{
	$password = Read-Host "Enter the Natro jenkins user password"
}

# Get the current version
[xml]$commonPropsXml = Get-Content "../common.props"
$version = $commonPropsXml.Project.PropertyGroup.Version
echo "`n---===[ Current version: $version ]===---"



echo "`n---===[ Running SSH commands on NATRO ]===---"

echo "`nDownloading release file into Natro..."
plink.exe -ssh jenkins@94.73.164.234 -pw $password -P 22 -batch "powershell -File c:\ci\scripts\download-latest-abp-release.ps1 ${version}" 

echo "`n---===[ COMPLETED ]===---"