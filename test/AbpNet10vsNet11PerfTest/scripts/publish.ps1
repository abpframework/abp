# Release-publishes the web app and the DbMigrator of each target into publish/<target>/.
param([ValidateSet('abp10', 'abp11')][string[]]$Target = @('abp10', 'abp11'))
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent

foreach ($t in $Target) {
    # global.json is resolved from the working directory, so publish from inside the project folder.
    Push-Location "$root/$t"
    try {
        foreach ($p in @{ 'Bench.Web' = 'web'; 'Bench.DbMigrator' = 'migrator' }.GetEnumerator()) {
            $out = "$root/publish/$t/$($p.Value)"
            if (Test-Path $out) { Remove-Item $out -Recurse -Force }
            Write-Host "== $t / $($p.Key) (SDK $(dotnet --version))"
            dotnet publish "src/$($p.Key)" -c Release -o $out -v q
            if ($LASTEXITCODE) { throw "publish failed: $t/$($p.Key)" }
        }
    }
    finally { Pop-Location }
}
