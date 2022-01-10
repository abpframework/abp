param(
  [string]$Version,
  [string]$Registry
)

yarn install

$NextVersion = $(node publish-utils.js --nextVersion)
$RootFolder = (Get-Item -Path "./" -Verbose).FullName

if(-Not $Version) {
$Version = $NextVersion;
}

if(-Not $Registry) {
exit
}

$commands = (
  "cd ng-packs\scripts",
  "yarn",
  "npm run publish-packages -- --nextVersion $Version --preview --registry $Registry --skipVersionValidation",
  "cd ../../",
  "npm run lerna -- version $Version --yes --no-commit-hooks --skip-git --force-publish",
  "npm run replace-with-tilde",
  "npm run lerna -- exec 'npm publish --registry $Registry --tag preview'"
)

foreach ($command in $commands) { 
  Write-Host $command
  Invoke-Expression $command
  if($LASTEXITCODE -ne '0' -And $command -notlike '*cd *'){
    Write-Host ("Process failed! " + $command)
    Set-Location $RootFolder
    exit $LASTEXITCODE
  }
}