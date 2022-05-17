#Requires -Version 3.0

function New-GitHubRelease
{
<#
	.SYNOPSIS
	Creates a new Release for the given GitHub repository.

	.DESCRIPTION
	Uses the GitHub API to create a new Release for a given repository.
	Allows you to specify all of the Release properties, such as the Tag, Name, Assets, and if it's a Draft or Prerelease or not.

	.PARAMETER GitHubUsername
	The username that the GitHub repository exists under.
	e.g. For the repository https://github.com/deadlydog/New-GitHubRelease, the username is 'deadlydog'.

	.PARAMETER GitHubRepositoryName
	The name of the repository to create the Release for.
	e.g. For the repository https://github.com/deadlydog/New-GitHubRelease, the repository name is 'New-GitHubRelease'.

	.PARAMETER GitHubAccessToken
	The Access Token to use as credentials for GitHub.
	Access tokens can be generated at https://github.com/settings/tokens.
	The access token will need to have the repo/public_repo permission on it for it to be allowed to create a new Release.

	.PARAMETER TagName
	The name of the tag to create at the Commitish.

	.PARAMETER ReleaseName
	The name to use for the new release.
	If blank, the TagName will be used.

	.PARAMETER ReleaseNotes
	The text describing the contents of the release.

	.PARAMETER AssetFilePaths
	The full paths of the files to include in the release.

	.PARAMETER Commitish
	Specifies the commitish value that determines where the Git tag is created from.
	Can be any branch or commit SHA. Unused if the Git tag already exists.
	Default: the repository's default branch (usually master).

	.PARAMETER IsDraft
	True to create a draft (unpublished) release, false to create a published one.
	Default: false

	.PARAMETER IsPreRelease
	True to identify the release as a prerelease. false to identify the release as a full release.
	Default: false

	.OUTPUTS
	A hash table with the following properties is returned:

	Succeeded = $true if the Release was created successfully and all assets were uploaded to it, $false if some part of the process failed.
	ReleaseCreationSucceeded = $true if the Release was created successfully (does not include asset uploads), $false if the Release was not created.
	AllAssetUploadsSucceeded = $true if all assets were uploaded to the Release successfully, $false if one of them failed, $null if there were no assets to upload.
	ReleaseUrl = The URL of the new Release that was created.
	ErrorMessage = A message describing what went wrong in the case that Succeeded is $false.

	.EXAMPLE
	# Import the module dynamically from the PowerShell Gallery. Use CurrentUser scope to avoid having to run as admin.
	Import-Module -Name New-GitHubRelease -Scope CurrentUser

	# Specify the parameters required to create the release. Do it as a hash table for easier readability.
	$newGitHubReleaseParameters =
	@{
		GitHubUsername = 'deadlydog'
		GitHubRepositoryName = 'New-GitHubRelease'
		GitHubAccessToken = 'SomeLongHexidecimalString'
		ReleaseName = "New-GitHubRelease v1.0.0"
		TagName = "v1.0.0"
		ReleaseNotes = "This release contains the following changes: ..."
		AssetFilePaths = @('C:\MyProject\Installer.exe','C:\MyProject\Documentation.md')
		IsPreRelease = $false
		IsDraft = $true	# Set to true when testing so we don't publish a real release (visible to everyone) by accident.
	}

	# Try to create the Release on GitHub and save the results.
	$result = New-GitHubRelease @newGitHubReleaseParameters

	# Provide some feedback to the user based on the results.
	if ($result.Succeeded -eq $true)
	{
		Write-Output "Release published successfully! View it at $($result.ReleaseUrl)"
	}
	elseif ($result.ReleaseCreationSucceeded -eq $false)
	{
		Write-Error "The release was not created. Error message is: $($result.ErrorMessage)"
	}
	elseif ($result.AllAssetUploadsSucceeded -eq $false)
	{
		Write-Error "The release was created, but not all of the assets were uploaded to it. View it at $($result.ReleaseUrl). Error message is: $($result.ErrorMessage)"
	}

	Attempt to create a new Release on GitHub, and provide feedback to the user indicating if it succeeded or not.

	.LINK
	Project home: https://github.com/deadlydog/New-GitHubRelease

	.NOTES
	Name:   New-GitHubRelease
	Author: Daniel Schroeder (originally based on the script at https://github.com/majkinetor/au/blob/master/scripts/Github-CreateRelease.ps1)
	GitHub Release API Documentation: https://developer.github.com/v3/repos/releases/#create-a-release
	Version: 1.0.2
#>
	[CmdletBinding()]
	param
	(
		[Parameter(Mandatory=$true,HelpMessage="The username the repository is under (e.g. deadlydog).")]
		[string] $GitHubUsername,

		[Parameter(Mandatory=$true,HelpMessage="The repository name to create the release in (e.g. Invoke-MsBuild).")]
		[string] $GitHubRepositoryName,

		[Parameter(Mandatory=$true,HelpMessage="The Acess Token to use as credentials for GitHub.")]
		[string] $GitHubAccessToken,

		[Parameter(Mandatory=$true,HelpMessage="The name of the tag to create at the the Commitish.")]
		[string] $TagName,

		[Parameter(Mandatory=$false,HelpMessage="The name of the release. If blank, the TagName will be used.")]
		[string] $ReleaseName,

		[Parameter(Mandatory=$false,HelpMessage="Text describing the contents of the tag.")]
		[string] $ReleaseNotes,

		[Parameter(Mandatory=$false,HelpMessage="The full paths of the files to include in the release.")]
		[string[]] $AssetFilePaths,

		[Parameter(Mandatory=$false, HelpMessage="Specifies the commitish value that determines where the Git tag is created from. Can be any branch or commit SHA. Unused if the Git tag already exists. Default: the repository's default branch (usually master).")]
		[string] $Commitish,

		[Parameter(Mandatory=$false,HelpMessage="True to create a draft (unpublished) release, false to create a published one. Default: false")]
		[bool] $IsDraft = $false,

		[Parameter(Mandatory=$false,HelpMessage="True to identify the release as a prerelease. false to identify the release as a full release. Default: false")]
		[bool] $IsPreRelease = $false
	)

	BEGIN
	{
		# Turn on Strict Mode to help catch syntax-related errors.
		# 	This must come after a script's/function's param section.
		# 	Forces a function to be the first non-comment code to appear in a PowerShell Script/Module.
		Set-StrictMode -Version Latest

		Set-SecurityProtocolForThread

		[string] $NewLine = [Environment]::NewLine

		if ([string]::IsNullOrEmpty($ReleaseName))
		{
			$ReleaseName = $TagName
		}

		# Ensure that all of the given asset file paths to upload are valid.
		Test-AllFilePathsAndThrowErrorIfOneIsNotValid $AssetFilePaths
	}

	END { }

	PROCESS
	{
		# Create the hash table to return, with default values.
		$result = @{}
		$result.Succeeded = $false
		$result.ReleaseCreationSucceeded = $false
		$result.AllAssetUploadsSucceeded = $false
		$result.ReleaseUrl = $null
		$result.ErrorMessage = $null

		[bool] $thereAreNoAssetsToIncludeInTheRelease = ($AssetFilePaths -eq $null) -or ($AssetFilePaths.Count -le 0)
		if ($thereAreNoAssetsToIncludeInTheRelease)
		{
			$result.AllAssetUploadsSucceeded = $null
		}

		$authHeader =
		@{
			Authorization = 'Basic ' + [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes($GitHubAccessToken + ":x-oauth-basic"))
		}

		$releaseData =
		@{
			tag_name         = $TagName
			target_commitish = $Commitish
			name             = $ReleaseName
			body             = $ReleaseNotes
			draft            = $IsDraft
			prerelease       = $IsPreRelease
		}

		$createReleaseWebRequestParameters =
		@{
			Uri         = "https://api.github.com/repos/$GitHubUsername/$GitHubRepositoryName/releases"
			Method      = 'POST'
			Headers     = $authHeader
			ContentType = 'application/json'
			Body        = (ConvertTo-Json $releaseData -Compress)
		}

		try
		{
			Write-Verbose "Sending web request to create the new Release..."
			$createReleaseWebRequestResults = Invoke-RestMethodAndThrowDescriptiveErrorOnFailure $createReleaseWebRequestParameters
		}
		catch
		{
			$result.ReleaseCreationSucceeded = $false
			$result.ErrorMessage = $_.Exception.Message
			return $result
		}

		$result.ReleaseCreationSucceeded = $true
		$result.ReleaseUrl = $createReleaseWebRequestResults.html_url

		if ($thereAreNoAssetsToIncludeInTheRelease)
		{
			$result.Succeeded = $true
			return $result
		}

		# Upload Url has template parameters on the end (e.g. ".../assets{?name,label}"), so remove them.
		[string] $urlToUploadFilesTo = $createReleaseWebRequestResults.upload_url -replace '{.+}'

		try
		{
			Write-Verbose "Uploading asset files to the new release..."
			Send-FilesToGitHubRelease -filePathsToUpload $AssetFilePaths -urlToUploadFilesTo $urlToUploadFilesTo -authHeader $authHeader
		}
		catch
		{
			$result.AllAssetUploadsSucceeded = $false
			$result.ErrorMessage = $_.Exception.Message
			return $result
		}

		$result.AllAssetUploadsSucceeded = $true
		$result.Succeeded = $true
		return $result
	}
}

function Send-FilesToGitHubRelease([string[]] $filePathsToUpload, [string] $urlToUploadFilesTo, $authHeader)
{
	[int] $numberOfFilesToUpload = $filePathsToUpload.Count
	[int] $numberOfFilesUploaded = 0
	$filePathsToUpload | ForEach-Object `
	{
		$filePath = $_
		$fileName = Get-Item $filePath | Select-Object -ExpandProperty Name

		$uploadAssetWebRequestParameters =
		@{
			# Append the name of the file to the upload url.
			Uri         = $urlToUploadFilesTo + "?name=$fileName"
			Method      = 'POST'
			Headers     = $authHeader
			ContentType = 'application/zip'
			InFile      = $filePath
		}

		$numberOfFilesUploaded = $numberOfFilesUploaded + 1
		Write-Verbose "Uploading asset $numberOfFilesUploaded of $numberOfFilesToUpload, '$filePath'."
		Invoke-RestMethodAndThrowDescriptiveErrorOnFailure $uploadAssetWebRequestParameters > $null
	}
}

function Test-AllFilePathsAndThrowErrorIfOneIsNotValid([string[]] $filePaths)
{
	foreach ($filePath in $filePaths)
	{
		[bool] $fileWasNotFoundAtPath = [string]::IsNullOrEmpty($filePath) -or !(Test-Path -Path $filePath -PathType Leaf)
		if ($fileWasNotFoundAtPath)
		{
			throw "There is no file at the specified path, '$filePath'."
		}
	}
}

function Invoke-RestMethodAndThrowDescriptiveErrorOnFailure($requestParametersHashTable)
{
	$requestDetailsAsNicelyFormattedString = Convert-HashTableToNicelyFormattedString $requestParametersHashTable
	Write-Verbose "Making web request with the following parameters:$NewLine$requestDetailsAsNicelyFormattedString"

	try
	{
		$webRequestResult = Invoke-RestMethod @requestParametersHashTable
	}
	catch
	{
		[Exception] $exception = $_.Exception

		[string] $errorMessage = Get-RestMethodExceptionDetailsOrNull -restMethodException $exception
		if ([string]::IsNullOrWhiteSpace($errorMessage))
		{
			$errorMessage = $exception.ToString()
		}

		throw "An unexpected error occurred while making web request:$NewLine$errorMessage"
	}

	Write-Verbose "Web request returned the following result:$NewLine$webRequestResult"
	return $webRequestResult
}

function Get-RestMethodExceptionDetailsOrNull([Exception] $restMethodException)
{
	try
	{
		$responseDetails = @{
			ResponseUri = $exception.Response.ResponseUri
			StatusCode = $exception.Response.StatusCode
			StatusDescription = $exception.Response.StatusDescription
			ErrorMessage = $exception.Message
		}
		[string] $responseDetailsAsNicelyFormattedString = Convert-HashTableToNicelyFormattedString $responseDetails

		[string] $errorInfo = "Request Details:" + $NewLine + $requestDetailsAsNicelyFormattedString
		$errorInfo += $NewLine
		$errorInfo += "Response Details:" + $NewLine + $responseDetailsAsNicelyFormattedString
		return $errorInfo
	}
	catch
	{
		return $null
	}
}

function Convert-HashTableToNicelyFormattedString($hashTable)
{
	[string] $nicelyFormattedString = $hashTable.Keys | ForEach-Object `
	{
		$key = $_
		$value = $hashTable.$key
		"  $key = $value$NewLine"
	}
	return $nicelyFormattedString
}

function Set-SecurityProtocolForThread
{
	[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12 -bor [System.Net.SecurityProtocolType]::Tls11 -bor [System.Net.SecurityProtocolType]::Tls
}

Export-ModuleMember -Function New-GitHubRelease