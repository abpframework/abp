param(
  [string]$Version,
  [string]$Registry
)

yarn install

$NextVersion = $(node publish-utils.js --nextVersion)
$RootFolder = (Get-Item -Path "./" -Verbose).FullName

if (-Not $Version) {
  $Version = $NextVersion;
}

if (-Not $Registry) {
  $Registry = "https://registry.npmjs.org";
}

$NgPacksPublishCommand = "npm run publish-packages -- --nextVersion $Version --skipGit --registry $Registry"
$UpdateGulpCommand = "npm run update-gulp"
$UpdateNgPacksCommand = "yarn update --registry $Registry"

$IsPrerelase = $(node publish-utils.js --prerelase --customVersion $Version) -eq "true";

if ($IsPrerelase) {
  $UpdateGulpCommand += " -- --prerelase"
  $UpdateNgPacksCommand += " --prerelase"
}

$commands = (
  $UpdateNgPacksCommand,
  "cd ng-packs\scripts",
  "yarn install",
  $NgPacksPublishCommand,
  "cd ../../",
  "cd scripts",
  "yarn",
  "yarn remove-lock-files",
  "cd ..",
  $UpdateGulpCommand
)

foreach ($command in $commands) { 
  $timer = [System.Diagnostics.Stopwatch]::StartNew()
  Write-Host $command
  Invoke-Expression $command
  if ($LASTEXITCODE -ne '0' -And $command -notlike '*cd *') {
    Write-Host ("Process failed! " + $command)
    Set-Location $RootFolder
    exit $LASTEXITCODE
  }
  $timer.Stop()
  $total = $timer.Elapsed
  Write-Output "-------------------------"
  Write-Output "$command command took $total (Hours:Minutes:Seconds.Milliseconds)"
  Write-Output "-------------------------"
}