param(
  [string]$Version
)

npm install

$NextVersion = $(node get-version.js) + '-preview' + (Get-Date).tostring(“yyyyMMdd”) + '-1'
$rootFolder = (Get-Item -Path "./" -Verbose).FullName

if(-Not $Version) {
$Version = $NextVersion;
}

$commands = (
  "cd ng-packs\scripts",
  "npm install",
  "npm run publish-packages -- --nextVersion $Version --preview",
  "cd ../../",
  "yarn lerna publish $Version --no-push --yes --no-git-reset --no-commit-hooks --no-git-tag-version --force-publish --dist-tag preview"
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