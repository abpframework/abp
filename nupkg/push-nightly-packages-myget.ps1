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

dotnet nuget push '*.nupkg' -s $source --skip-duplicate --api-key $apikey