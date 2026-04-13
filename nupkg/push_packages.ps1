. ".\common.ps1"

$apiKey = $args[0]

# Get the version
[xml]$commonPropsXml = Get-Content (Join-Path $rootFolder "common.props")
$version = $commonPropsXml.Project.PropertyGroup.Version

# Publish all packages
$i = 0
$errorCount = 0
$totalProjectsCount = $projects.length
$nugetUrl = "https://api.nuget.org/v3/index.json"
$maxQuotaRetryCount = 3
$quotaRetryDelaysInSeconds = @(30, 60, 120)
Set-Location $packFolder

foreach($project in $projects) {
	$i += 1
	$projectFolder = Join-Path $rootFolder $project
	$projectName = ($project -split '/')[-1]
	$nugetPackageName = $projectName + "." + $version + ".nupkg"	
	$nugetPackageExists = Test-Path $nugetPackageName -PathType leaf
 
	Write-Info "[$i / $totalProjectsCount] - Pushing: $nugetPackageName"
	
	if ($nugetPackageExists)
	{
		$attempt = 0
		while ($true)
		{
			$attempt += 1
			$pushOutput = dotnet nuget push $nugetPackageName --skip-duplicate -s $nugetUrl --api-key "$apiKey" 2>&1
			$pushOutput | ForEach-Object { Write-Host $_ }

			$pushSucceeded = $LASTEXITCODE -eq 0
			if ($pushSucceeded)
			{
				break
			}

			$quotaExceeded = ($pushOutput | Out-String) -match "403 \(Quota Exceeded\)"
			$canRetry = $quotaExceeded -and $attempt -le $maxQuotaRetryCount

			if (-not $canRetry)
			{
				break
			}

			$retryDelay = $quotaRetryDelaysInSeconds[$attempt - 1]
			Write-Warning "NuGet quota exceeded for $nugetPackageName. Retrying in $retryDelay seconds (retry $attempt/$maxQuotaRetryCount)..."
			Start-Sleep -Seconds $retryDelay
		}
		#Write-Host ("Deleting package from local: " + $nugetPackageName)
		#Remove-Item $nugetPackageName -Force
	}
	else
	{
		Write-Host ("********** ERROR PACKAGE NOT FOUND: " + $nugetPackageName) -ForegroundColor red
		$errorCount += 1
		#Exit
	}
	
	Write-Host "--------------------------------------------------------------`r`n"
}

if ($errorCount > 0)
{
	Write-Host ("******* $errorCount error(s) occured *******") -ForegroundColor red
}
