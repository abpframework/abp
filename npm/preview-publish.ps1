param(
  [string]$Version,
  [string]$Registry,
  [string]$LeptonXVersion
)

$commands = (
  ".\publish-mvc.ps1 $Version $Registry",
  ".\publish-ng.ps1 $Version $Registry $LeptonXVersion"
);

$NextVersion = $(node publish-utils.js --nextVersion)
$RootFolder = (Get-Item -Path "./" -Verbose).FullName

if(-Not $Version) {
$Version = $NextVersion;
}

if(-Not $Registry) {
exit
}

foreach ($command in $commands) { 
  Write-Host $command
  Invoke-Expression $command
  if($LASTEXITCODE -ne '0' -And $command -notlike '*cd *') {
    Write-Host ("Process failed! " + $command)
    Set-Location $RootFolder
    exit $LASTEXITCODE
  }
}
