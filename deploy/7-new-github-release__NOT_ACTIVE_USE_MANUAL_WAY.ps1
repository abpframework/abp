#THIS IS NOT BEING USED CURRENTLY. RELEASE GITHUB MANUALLY

# Import the module dynamically from the PowerShell Gallery. Use CurrentUser scope to avoid having to run as admin.
Import-Module -Name new-github-release-function.psm1

# Specify the parameters required to create the release. Do it as a hash table for easier readability.
$newGitHubReleaseParameters =
@{
    GitHubUsername = 'abpframework'
    GitHubRepositoryName = 'abp'
    GitHubAccessToken = '*******************'
    ReleaseName = "5.0.0-rc.2"
    TagName = "5.0.0-rc.2"
    ReleaseNotes = "N/A"
    #AssetFilePaths = @('C:\MyProject\Installer.exe','C:\MyProject\Documentation.md')
    IsPreRelease = $true
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
