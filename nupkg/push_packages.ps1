. ".\common.ps1"

$apiKey = $args[0]
$discordWebhookUrl = if ($args.Length -gt 1) { $args[1] } else { $env:DISCORD_WEBHOOK_URL }

# Get the version
[xml]$commonPropsXml = Get-Content (Join-Path $rootFolder "common.props")
$version = $commonPropsXml.Project.PropertyGroup.Version

# Publish all packages
$i = 0
$errorCount = 0
$failedPackages = @()
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
		$pushSucceeded = $false
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

			$clientError = ($pushOutput | Out-String) -match "Response status code does not indicate success:\s*4\d\d"
			$canRetry = $clientError -and $attempt -le $maxQuotaRetryCount

			if (-not $canRetry)
			{
				break
			}

			$retryDelay = $quotaRetryDelaysInSeconds[$attempt - 1]
			Write-Warning "NuGet push returned a 4xx response for $nugetPackageName. Retrying in $retryDelay seconds (retry $attempt/$maxQuotaRetryCount)..."
			Start-Sleep -Seconds $retryDelay
		}

		if (-not $pushSucceeded)
		{
			Write-Host ("********** ERROR PUSH FAILED: " + $nugetPackageName) -ForegroundColor red
			$errorCount += 1
			$failedPackages += $nugetPackageName
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

	if (-not [string]::IsNullOrWhiteSpace($discordWebhookUrl))
	{
		try
		{
			$messageLines = @(
				"NuGet push completed with failures (ABP).",
				"Failed package count: $errorCount",
				"Failed packages:"
			) + ($failedPackages | ForEach-Object { "- $_" })

			$payload = @{ content = ($messageLines -join "`n") } | ConvertTo-Json -Compress
			Invoke-RestMethod -Uri $discordWebhookUrl -Method Post -ContentType "application/json" -Body $payload | Out-Null
			Write-Info "Discord notification sent for failed packages."
		}
		catch
		{
			Write-Warning "Failed to send Discord webhook notification: $($_.Exception.Message)"
		}
	}
}
