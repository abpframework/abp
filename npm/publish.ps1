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
  "npm run lerna -- version $Version --yes --no-commit-hooks --skip-git --force-publish",
  "npm run replace-with-tilde",
  "npm run lerna -- exec 'npm publish --registry https://registry.npmjs.org'",
  "npm run update-gulp"
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