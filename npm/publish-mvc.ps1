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

$PacksPublishCommand = "npm run lerna -- exec 'npm publish --registry $Registry'"

$IsPrerelase = $(node publish-utils.js --prerelase --customVersion $Version) -eq "true";

if ($IsPrerelase) {
  $PacksPublishCommand = $PacksPublishCommand.Substring(0, $PacksPublishCommand.Length - 1) + " --tag next'"
}

$commands = (
  "npm run lerna -- version $Version --yes --no-commit-hooks --skip-git --force-publish",
  "yarn replace-with-tilde",
  "cd scripts",
  "yarn install",
  "yarn validate-versions --compareVersion $Version --path ../packs",
  "cd ..",
  $PacksPublishCommand
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
