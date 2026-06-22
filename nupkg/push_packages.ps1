. ".\common.ps1"

$apiKey = $args[0]

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
$defaultQuotaRetryDelaysInSeconds = @(30, 60, 120)
$maxPushesPerHour = 250   # NuGet.org rate limit (conservative). Set 0 to disable.
$pushTimestamps = [System.Collections.Generic.List[datetime]]::new()
$failedPackagesFilePath = Join-Path $packFolder "failed-packages.txt"
Set-Location $packFolder
if (Test-Path $failedPackagesFilePath) { Remove-Item $failedPackagesFilePath -Force }

foreach($project in $projects) {
	$i += 1
	$projectFolder = Join-Path $rootFolder $project
	$projectName = ($project -split '/')[-1]
	$nugetPackageName = $projectName + "." + $version + ".nupkg"	
	$nugetPackageExists = Test-Path $nugetPackageName -PathType leaf
 
	Write-Info "[$i / $totalProjectsCount] - Pushing: $nugetPackageName"
	
	if ($nugetPackageExists)
	{
		# Sliding-window rate limiter: ensure we don't exceed $maxPushesPerHour pushes per hour
		if ($maxPushesPerHour -gt 0)
		{
			$windowStart = (Get-Date).AddHours(-1)
			$recentPushes = @($pushTimestamps | Where-Object { $_ -gt $windowStart })
			$trimmedPushTimestamps = [System.Collections.Generic.List[datetime]]::new()
			foreach ($timestamp in $recentPushes)
			{
				[void]$trimmedPushTimestamps.Add($timestamp)
			}
			$pushTimestamps = $trimmedPushTimestamps

			if ($pushTimestamps.Count -ge $maxPushesPerHour)
			{
				$waitUntil = $pushTimestamps[0].AddHours(1)
				$waitSeconds = [int]([math]::Ceiling(($waitUntil - (Get-Date)).TotalSeconds)) + 1
				if ($waitSeconds -gt 0)
				{
					Write-Warning "Rate limit: $($pushTimestamps.Count) pushes in the last hour (limit: $maxPushesPerHour). Pausing $waitSeconds seconds until the window resets..."
					Start-Sleep -Seconds $waitSeconds
				}
			}
		}

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

			# Parse the retry-after value from the NuGet response if present (e.g. "retry after: 2974s")
			$retryAfterMatch = ($pushOutput | Out-String) | Select-String -Pattern 'retry after:\s*(\d+)s' -AllMatches
			if ($retryAfterMatch -and $retryAfterMatch.Matches.Count -gt 0)
			{
				$retryDelay = [int]$retryAfterMatch.Matches[0].Groups[1].Value
			}
			else
			{
				$retryDelay = $defaultQuotaRetryDelaysInSeconds[$attempt - 1]
			}

			Write-Warning "NuGet push returned a 4xx response for $nugetPackageName. Retrying in $retryDelay seconds (retry $attempt/$maxQuotaRetryCount)..."
			Start-Sleep -Seconds $retryDelay
		}

		if (-not $pushSucceeded)
		{
			Write-Host ("********** ERROR PUSH FAILED: " + $nugetPackageName) -ForegroundColor red
			$errorCount += 1
			$failedPackages += $nugetPackageName
		}
		else
		{
			$pushTimestamps.Add((Get-Date))
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
	$failedPackages | Set-Content -Path $failedPackagesFilePath
	exit 1
}
