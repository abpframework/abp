param(
  [string]$Version,
  [string]$Registry,
  [string]$LeptonXVersion
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

$UpdateNgPacksCommand = "yarn update-version $Version"
$NgPacksPublishCommand = "npm run publish-packages -- --nextVersion $Version --skipGit --registry $Registry --skipVersionValidation"
$UpdateGulpCommand = "yarn update-gulp --registry $Registry"
$UpdateLeptonXCommand = "yarn update-lepton-x-versions -v $LeptonXVersion";

$IsPrerelease = $(node publish-utils.js --prerelease --customVersion $Version) -eq "true";

if ($IsPrerelease) {
  $UpdateGulpCommand += " --prerelease"
  $UpdateNgPacksCommand += " --prerelease"
}

$commands = (
  "cd ng-packs",
  "yarn install",
  $UpdateNgPacksCommand,
  "cd scripts",
  "yarn install",
  $NgPacksPublishCommand,
  "cd ../../",
  "cd scripts",
  "yarn remove-lock-files",
  "cd ..",
  $UpdateGulpCommand,
  "cd scripts",
  $UpdateLeptonXCommand
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
