# Runs the load tests under identical conditions and stores every run under results/<batch>/<target>/<label>/rep<n>/.
# For every repetition and rate it runs each target (the order alternates between repetitions to cancel drift):
#   stop app -> reset database -> start app pinned to the app cores -> start dotnet-counters + docker stats
#   -> k6 (pinned to the load-generator cores) -> stop collectors and app.
#
# Examples (BENCH_PASSWORD must be set to the seed password; k6 reads it from the environment):
#   ./scripts/run-bench.ps1 -Mode probe -Repeats 1                                  # capacity probe, abp10 + abp11
#   ./scripts/run-bench.ps1 -Mode fixed -Rates 6,12,18                              # measured levels
#   ./scripts/run-bench.ps1 -Mode fixed -Rates 12 -Targets abp10,abp10-rt11,abp11   # runtime-only comparison
#   ./scripts/run-bench.ps1 -Mode fixed -Rates 4 -Repeats 1 -WarmupS 20 -DurationS 30   # dry run
param(
    [ValidateSet('fixed', 'probe')][string]$Mode = 'fixed',
    [ValidateSet('abp10', 'abp11', 'abp10-rt11')][string[]]$Targets = @('abp10', 'abp11'),
    [int[]]$Rates = @(),
    [int]$Repeats = 3,
    [int]$WarmupS = 120,
    [int]$DurationS = 300,
    [string]$ProbeRates = '2,4,6,8,10,12,14,16,20,24,28,32',
    [int]$StepS = 60,
    [string]$AppAffinity = 'FF',      # logical CPUs 0-7 = 4 performance cores
    [string]$K6Affinity = 'FF0000',   # logical CPUs 16-23 = efficiency cores
    [int]$CooldownS = 15,
    [string]$Batch = (Get-Date -Format 'yyyyMMdd-HHmmss')
)
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$graceS = 10 # must match GRACE_S in k6/scenario.js
$env:Path = [Environment]::GetEnvironmentVariable('Path', 'Machine') + ';' + [Environment]::GetEnvironmentVariable('Path', 'User')

if (-not $env:BENCH_PASSWORD) { throw 'Set $env:BENCH_PASSWORD to the seed password first.' }
if ($Mode -eq 'fixed' -and -not $Rates) { throw 'MODE fixed needs -Rates (iterations per second).' }
foreach ($tool in 'k6', 'dotnet-counters', 'docker') { if (-not (Get-Command $tool -ErrorAction SilentlyContinue)) { throw "$tool not found" } }

$labels = if ($Mode -eq 'fixed') { $Rates | ForEach-Object { "rate$_" } } else { @('probe') }
$k6Seconds = if ($Mode -eq 'fixed') { $WarmupS + $graceS + $DurationS + $graceS } else { (($ProbeRates -split ',').Count + 1) * ($StepS + $graceS) }
$batchDir = Join-Path $root "results/$Batch"
New-Item -ItemType Directory -Force $batchDir | Out-Null
@{ mode = $Mode; targets = $Targets; rates = $Rates; repeats = $Repeats; warmupS = $WarmupS; durationS = $DurationS
   probeRates = $ProbeRates; stepS = $StepS; graceS = $graceS; appAffinity = $AppAffinity; k6Affinity = $K6Affinity
   thinkTime = $env:THINK_TIME ?? '1'; sessionIterations = $env:SESSION_ITERATIONS ?? '10' } |
    ConvertTo-Json | Set-Content "$batchDir/batch.json"

$total = $Repeats * $labels.Count * $Targets.Count
$minutes = [math]::Ceiling($total * ($k6Seconds + $CooldownS + 20) / 60)
Write-Host "batch $Batch : $total runs, about $minutes min -> $batchDir"

function Stop-App {
    $l = Get-NetTCPConnection -LocalPort 44377 -State Listen -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($l) { Stop-Process -Id $l.OwningProcess -Force; Start-Sleep 2 }
}

$n = 0
for ($rep = 1; $rep -le $Repeats; $rep++) {
    foreach ($label in $labels) {
        $order = if ($rep % 2) { $Targets } else { $Targets[($Targets.Count - 1)..0] }
        foreach ($target in $order) {
            $n++
            $dir = Join-Path $batchDir "$target/$label/rep$rep"
            New-Item -ItemType Directory -Force $dir | Out-Null
            Write-Host "[$n/$total] $target $label rep$rep" -NoNewline

            Stop-App
            & "$PSScriptRoot/db.ps1" -Target $target -Action reset *> $null
            $appPid = & "$PSScriptRoot/start-app.ps1" -Target $target -Affinity $AppAffinity -LogFile "$dir/app.log"

            $counters = Start-Process dotnet-counters -PassThru -WindowStyle Hidden -ArgumentList @(
                'collect', '-p', $appPid, '--refresh-interval', '1', '--format', 'csv', '-o', "`"$dir/counters.csv`"",
                '--counters', 'System.Runtime,Microsoft.AspNetCore.Hosting,Microsoft.AspNetCore.Server.Kestrel',
                '--duration', [TimeSpan]::FromSeconds($k6Seconds + 60).ToString('dd\:hh\:mm\:ss'))
            $dockerStats = Start-Job -ArgumentList "$dir/docker-stats.tsv" -ScriptBlock {
                param($file)
                while ($true) {
                    $line = docker stats --no-stream --format '{{json .}}' bench-postgres
                    "$((Get-Date).ToUniversalTime().ToString('o'))`t$line" | Add-Content $file
                }
            }

            $k6Args = "run --quiet --summary-export `"$dir/k6-summary.json`" -e MODE=$Mode " +
                $(if ($Mode -eq 'fixed') { "-e RATE=$($label -replace 'rate', '') -e WARMUP_S=$WarmupS -e DURATION_S=$DurationS " }
                  else { "-e PROBE_RATES=$ProbeRates -e STEP_S=$StepS " }) + "`"$root/k6/scenario.js`""
            $startedUtc = (Get-Date).ToUniversalTime()
            Start-Process cmd.exe -Wait -WindowStyle Hidden -ArgumentList "/c start `"`" /b /wait /affinity $K6Affinity k6 $k6Args > `"$dir/k6.log`" 2>&1"
            $endedUtc = (Get-Date).ToUniversalTime()

            Stop-Job $dockerStats; Remove-Job $dockerStats
            if (-not $counters.HasExited) { Stop-Process -Id $counters.Id -Force }
            Stop-App

            @{ target = $target; label = $label; rep = $rep; appPid = $appPid; startedUtc = $startedUtc.ToString('o'); endedUtc = $endedUtc.ToString('o')
               measureStartOffsetS = if ($Mode -eq 'fixed') { $WarmupS + $graceS } else { $StepS + $graceS }
               measureDurationS = if ($Mode -eq 'fixed') { $DurationS } else { $null } } |
                ConvertTo-Json | Set-Content "$dir/run.json"

            if (Test-Path "$dir/k6-summary.json") {
                $m = (Get-Content "$dir/k6-summary.json" -Raw | ConvertFrom-Json).metrics
                $s = if ($Mode -eq 'fixed') { 'measure' } else { 'step_' + ($ProbeRates -split ',')[-1].PadLeft(3, '0') }
                $d = $m."http_req_duration{scenario:$s}"; $f = $m."http_req_failed{scenario:$s}"; $c = $m."http_reqs{scenario:$s}"
                $rps = if ($Mode -eq 'fixed' -and $c) { [math]::Round($c.count / $DurationS, 1) } else { '-' }
                Write-Host ("  req/s {0}  p95 {1:N1} ms  p99 {2:N1} ms  failed {3:P2}" -f $rps, $d.'p(95)', $d.'p(99)', $f.value)
            }
            else { Write-Host '  k6 produced no summary, see k6.log' -ForegroundColor Red }
            Start-Sleep $CooldownS
        }
    }
}
Write-Host "done: $batchDir"
