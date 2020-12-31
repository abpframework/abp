param(
  [string]$Version
)

npm install

$NextVersion = $(node publish-utils.js --nextVersion)
$RootFolder = (Get-Item -Path "./" -Verbose).FullName

if (-Not $Version) {
  $Version = $NextVersion;
}

$NgPacksPublishCommand = "npm run publish-packages -- --nextVersion $Version --skipGit"
$PacksPublishCommand = "npm run lerna -- exec 'npm publish --registry https://registry.npmjs.org'"
$UpdateGulpCommand = "npm run update-gulp"

$IsRc = $(node publish-utils.js --rc) -eq "true";

if ($IsRc) { 
  $NgPacksPublishCommand += " --rc"
  $UpdateGulpCommand += " -- --rc"
  $PacksPublishCommand = $PacksPublishCommand.Substring(0, $PacksPublishCommand.Length - 1) + " --tag next'"
}

$commands = (
  "cd ng-packs\scripts",
  "npm install",
  $NgPacksPublishCommand,
  "cd ../../",
  "npm run lerna -- version $Version --yes --no-commit-hooks --skip-git --force-publish",
  "npm run replace-with-tilde",
  $PacksPublishCommand,
  "cd scripts",
  "yarn",
  "yarn remove-lock-files",
  "cd ..",
  $UpdateGulpCommand
)

foreach ($command in $commands) { 
  Write-Host $command
  Invoke-Expression $command
  if ($LASTEXITCODE -ne '0' -And $command -notlike '*cd *') {
    Write-Host ("Process failed! " + $command)
    Set-Location $RootFolder
    exit $LASTEXITCODE
  }
}