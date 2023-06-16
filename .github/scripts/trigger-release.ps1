param(
  [string]$tag_name,
  [string]$prerelease,
  [string]$branchName
)

# GitHub credentials and repository info
$user = "skoc10"
$repo = "my_projects"
$workflow_file_name = "publish-release.yml"
$token = $env:RELEASE_TOKEN

# Create a header for the API request
$headers = @{
    Authorization = "Bearer $token"
    Accept = "application/vnd.github.v3+json"
}

# Create a body for the API request
$body = @{
    ref = $branchName
    inputs = @{
        tag_name = $tag_name
        prerelease = $prerelease
        branchName = $branchName
    }
} | ConvertTo-Json

# Send the API request to GitHub
Invoke-RestMethod -Method Post -Headers $headers -Body $body -Uri "https://api.github.com/repos/$user/$repo/actions/workflows/$workflow_file_name/dispatches"
