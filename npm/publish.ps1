param(
  [string]$Version
)

npm install

$NextVersion = $(node get-version.js)
$rootFolder = (Get-Item -Path "./" -Verbose).FullName

if(-Not $Version) {
  $Version = $NextVersion;
}

$commands = (
  "cd ng-packs\scripts",
  "npm install",
  "npm run publish-packages -- --nextVersion $Version",
  "cd ../../",
  "yarn lerna publish $Version --no-push --yes --no-git-reset --no-commit-hooks --no-git-tag-version --force-publish",
  "yarn update:templates",
  "yarn gulp:app",
  "yarn gulp:module"
)

foreach ($command in $commands) { 
  Write-Host $command
  Invoke-Expression $command
  if($LASTEXITCODE -ne '0' -And $command -notlike '*cd *'){
    Write-Host ("Process failed! " + $command)
    Set-Location $rootFolder
    exit $LASTEXITCODE
  }
}