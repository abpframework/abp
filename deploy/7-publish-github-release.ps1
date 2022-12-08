param(
  [string]$branchName,
  [string]$version,
  [string]$isRcVersion,
  [string]$isDraft,
  [string]$gitHubApiKey
) 

. ..\nupkg\common.ps1

Write-Info "Publishing GitHub Release..." ##  Further info see https://docs.github.com/en/rest/reference/releases

if ($isRcVersion -eq "")
{
	$isRcVersion = Read-Host "Is this a RC/Preview version? (y/n)"	
}

if ($gitHubApiKey -eq "")
{
	$gitHubApiKey = Read-File "github-api-key.txt"
	echo "GitHub API Key assigned from github-api-key.txt"
}

if(!$gitHubApiKey)
{
	$gitHubApiKey = Read-Host "Enter the GitHub API Key"
}

if ($version -eq "")
{
	$version = Get-Current-Version # The version number for this release
}

if ($branchName -eq "")
{
	$branchName = Get-Current-Branch # The branch name also the tag name
}

if ($isDraft -eq "")
{
	$draft = $FALSE
}
else
{
	$draft =  [boolean]::Parse($isDraft)
}

##############################################################################
$preRelease = ( ($isRcVersion -eq "true") -or ($isRcVersion -eq "y") -or ($isRcVersion -eq "rc") ) # Set to true to mark this as a pre-release version
$gitHubUsername = 'abpframework' # The github username
$gitHubRepository = 'abp' # The github repository name
$releaseNotes = '' # The notes to accompany this release, uses the commit message in this case 
##############################################################################

echo "Current version: $version"
echo "Current branch: $branchName"
echo "Preview version: $preRelease"
echo "Draft: $draft"

##############################################################################

$releaseData = @{
   tag_name = $version;
   target_commitish = $branchName; 
   name = $version;
   body = $releaseNotes;
   draft = $draft;
   prerelease = $preRelease;
 }

$releaseParams = @{
   Uri = "https://api.github.com/repos/$gitHubUsername/$gitHubRepository/releases";
   Method = 'POST';
   Headers = @{
     Authorization = 'Basic ' + [Convert]::ToBase64String(
     [Text.Encoding]::ASCII.GetBytes($gitHubApiKey + ":x-oauth-basic"));
   }
   ContentType = 'application/json';
   Body = (ConvertTo-Json $releaseData -Compress)
 }

$response = Invoke-RestMethod @releaseParams 

echo  "---------------------------------------------"
echo "$version has been successfully released."

