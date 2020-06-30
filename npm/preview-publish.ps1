param(
  [string]$Version,
  [string]$User,
  [string]$Pass,
  [string]$Email,
  [string]$Registry
)

npm install

$NextVersion = $(node get-version.js)
$rootFolder = (Get-Item -Path "./" -Verbose).FullName

if(-Not $Version) {
$Version = $NextVersion;
}

if(-Not $Registry) {
$Registry = "https://www.myget.org/F/abp-nightly/npm";
}

$commands = (
  "cd ng-packs\scripts",
  "npm install",
  "npm run publish-packages -- --nextVersion $Version --preview",
  "cd ../../",
  "yarn lerna version $Version --yes --no-commit-hooks --skip-git --force-publish",
  "yarn replace-with-tilde",
  "yarn lerna exec 'npm publish --registry $Registry --tag preview'"
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