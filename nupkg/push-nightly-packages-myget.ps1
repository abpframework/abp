param(
  [string]$source,
  [string]$apikey
)

if (!$source)
{
	$source = "https://nuget.org/"
}

if (!$apikey)
{
	$apikey = "dummy"
}

$maxRetryCount = 3
$retryDelaysInSeconds = @(30, 60, 120)
$failedPackages = @()
$failedPackagesFilePath = Join-Path (Get-Location) "failed-packages.txt"
if (Test-Path $failedPackagesFilePath) { Remove-Item $failedPackagesFilePath -Force }

$packages = Get-ChildItem -Filter *.nupkg | Sort-Object Name

foreach ($package in $packages)
{
	$packageName = $package.Name
	Write-Host "Pushing nightly package: $packageName"

	$attempt = 0
	$pushSucceeded = $false
	while ($true)
	{
		$attempt += 1
		$pushOutput = dotnet nuget push $packageName -s $source --skip-duplicate --api-key $apikey 2>&1
		$pushOutput | ForEach-Object { Write-Host $_ }

		$pushSucceeded = $LASTEXITCODE -eq 0
		if ($pushSucceeded)
		{
			break
		}

		$clientError = ($pushOutput | Out-String) -match "Response status code does not indicate success:\s*4\d\d"
		$canRetry = $clientError -and $attempt -le $maxRetryCount
		if (-not $canRetry)
		{
			break
		}

		$retryDelay = $retryDelaysInSeconds[$attempt - 1]
		Write-Warning "NuGet push returned a 4xx response for $packageName. Retrying in $retryDelay seconds (retry $attempt/$maxRetryCount)..."
		Start-Sleep -Seconds $retryDelay
	}

	if (-not $pushSucceeded)
	{
		Write-Host ("********** ERROR PUSH FAILED: " + $packageName) -ForegroundColor red
		$failedPackages += $packageName
	}
}

if ($failedPackages.Count -gt 0)
{
	$errorCount = $failedPackages.Count
	Write-Host ("******* $errorCount error(s) occured *******") -ForegroundColor red
	$failedPackages | Set-Content -Path $failedPackagesFilePath
	exit 1
}