param(
  [string]$Version
)

if(-Not $Version) {
echo 'Please pass a semantic version like this: ./publish.ps1 -Version patch'
exit
}

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

$commands = (
  "cd ng-packs\scripts",
  "npm install",
  "npm run publish-packages -- $Version",
  "cd ../../",
  "yarn",
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