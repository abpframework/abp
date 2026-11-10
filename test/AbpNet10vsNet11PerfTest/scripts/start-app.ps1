# Runs the published web app of a target with the shared measurement settings.
#   abp10       ABP 10.7.1 on .NET 10
#   abp11       ABP 11.0.0-rc.1 on .NET 11
#   abp10-rt11  the abp10 build rolled forward onto the .NET 11 runtime (isolates the runtime difference)
# -Affinity <hex mask> pins the process from its start (Server GC sizes its heaps from the startup affinity).
# Without -LogFile it blocks until stopped; with -LogFile it starts in the background and returns the app's PID.
param(
    [Parameter(Mandatory)][ValidateSet('abp10', 'abp11', 'abp10-rt11')][string]$Target,
    [string]$Affinity,
    [string]$LogFile
)
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$port = 44377
. "$PSScriptRoot/env.ps1"

$build = $Target -replace '-rt11$', ''
if ($Target -eq 'abp10-rt11') {
    # 'Major' only rolls forward when .NET 10 is missing; 'LatestMajor' always picks the newest installed runtime.
    $env:DOTNET_ROLL_FORWARD = 'LatestMajor'
    $env:DOTNET_ROLL_FORWARD_TO_PRERELEASE = '1' # .NET 11 is still a release candidate
}
else {
    Remove-Item Env:DOTNET_ROLL_FORWARD, Env:DOTNET_ROLL_FORWARD_TO_PRERELEASE -ErrorAction SilentlyContinue
}

$web = "$root/publish/$build/web" # content root: wwwroot, openiddict.pfx, appsettings.json
$app = "dotnet Bench.Web.dll --urls https://localhost:$port"
$affinityArg = if ($Affinity) { "/affinity $Affinity" } else { '' }

Push-Location $web
try {
    if (-not $LogFile) {
        cmd /c "start `"`" /b /wait $affinityArg $app"
        return
    }
    if (Get-NetTCPConnection -LocalPort $port -State Listen -ErrorAction SilentlyContinue) { throw "port $port is already in use" }
    # The inner cmd gets the affinity and the dotnet child inherits it; output goes to the log file.
    # Start-Process keeps our console pipes away from the children, otherwise this script would wait for them.
    Start-Process cmd.exe -WindowStyle Hidden -ArgumentList "/c start `"`" /b $affinityArg cmd /c `"$app > `"$LogFile`" 2>&1`""
}
finally { Pop-Location }

for ($i = 0; $i -lt 120; $i++) {
    $listener = Get-NetTCPConnection -LocalPort $port -State Listen -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($listener) {
        try {
            $null = Invoke-WebRequest "https://localhost:$port/Account/Login" -SkipCertificateCheck -UseBasicParsing -TimeoutSec 5
            return $listener.OwningProcess
        }
        catch { }
    }
    Start-Sleep -Milliseconds 500
}
throw "the app did not start within 60 s; see $LogFile"
