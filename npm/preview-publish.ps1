param(
  [string]$Version,
  [string]$User,
  [string]$Pass,
  [string]$Email,
  [string]$Registry
)

npm install

$NextVersion = $(node publish-utils.js --nextVersion)
$RootFolder = (Get-Item -Path "./" -Verbose).FullName

if(-Not $Version) {
$Version = $NextVersion;
}

if(-Not $Registry) {
$Registry = "https://www.myget.org/F/abp-nightly/auth/8f2a5234-1bce-4dc7-b976-2983078590a9/npm/";
}

$commands = (
  "cd ng-packs\scripts",
  "npm install",
  "npm run publish-packages -- --nextVersion $Version --preview",
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