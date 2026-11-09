# ABP .NET 10 vs .NET 11 Performance Benchmark

End-to-end load test comparing an empty ABP MVC application on stable ABP 10.7.1 + .NET 10 (`net10`) against preview ABP 11.0.0-rc.1 + .NET 11 (`net11`) under identical hardware, database, data set, and workload conditions:

| Target | Application Build | Runtime | Role |
|---|---|---|---|
| `net10` | ABP `10.7.1` (`net10.0`) | .NET `10.0.13` | Primary — Stable baseline |
| `net11` | ABP `11.0.0-rc.1` (`net11.0`) | .NET `11.0.0-rc.3` | Primary — Preview stack |
| `net10-rt11` | ABP `10.7.1` (`net10.0`) | .NET `11.0.0-rc.3` (`DOTNET_ROLL_FORWARD=LatestMajor`) | Supplementary — Isolates the .NET runtime difference from ABP package changes |

![Three-target benchmark design](report/assets/01-three-target-design.png)

### Key Findings Summary

| Metric / Test | `net10` (ABP 10 + .NET 10) | `net10-rt11` (ABP 10 on .NET 11) | `net11` (ABP 11 + .NET 11) | .NET 11 Advantage |
|---|---|---|---|---|
| **6 it/s p95 Latency (27-run Marathon)** | 38.3 ms | 34.5 ms | **34.7 ms** | **~9.5% faster** |
| **6 it/s Web CPU Usage** | 55.0% | 50.3% | **50.6%** | **~4.4% CPU saving** |
| **6 it/s GC Heap Size** | 355 MB | 321 MB | **311 MB** | **~44 MB less heap** |
| **7 it/s Saturation Knee (p95)** | 39.5 ms | 40.3 ms | **40.2 ms** | Rock-solid at capacity edge |
| **ABP 11 Package Overhead** | — | Baseline | **+0.2 ms** (0.5%) | **Zero package regression** |

![Comparison of response time, CPU, and memory usage](report/assets/03-performance-resource-usage.png)

Full benchmark methodology, tool evaluation, scenario correlation analysis, and findings are documented in [`report/README.md`](report/README.md). Exact package, runtime, and hardware versions are recorded in [`report/versions.md`](report/versions.md).

---
## Repository Structure

```text
.
├── docker-compose.yml               # PostgreSQL 18.6 (digest-pinned) on port 5433
├── net10/                           # ABP 10.7.1 + .NET 10 MVC project (EF Core, PostgreSQL, LeptonX Lite)
├── net11/                           # ABP 11.0.0-rc.1 + .NET 11 MVC project (upgraded copy of net10)
├── k6/
│   ├── recordings/
│   │   └── abp-mvc.har              # Sanitized k6 Studio browser recording (passwords/cookies redacted)
│   ├── studio/
│   │   ├── abp-mvc-correlated.k6g   # k6 Studio generator with 7 correlation/verification rules
│   │   └── abp-mvc-correlated.js    # Correlated script exported from k6 Studio
│   ├── tools/
│   │   ├── sanitize-har.mjs         # Strips non-app requests, static bodies, and secrets from raw HAR
│   │   └── summarize.mjs            # Aggregates k6, dotnet-counters, and docker stats into Markdown tables
│   └── scenario.js                  # Parameterized multi-VU load-test scenario (smoke, probe, fixed modes)
├── scripts/
│   ├── env.ps1                      # Shared Production measurement environment variables
│   ├── publish.ps1                  # Release-publishes Web and DbMigrator for net10 and net11
│   ├── db.ps1                       # Seeds or resets the database from Bench_seed_<target> template DBs
│   ├── seed-bench-data.sql          # Seeds 1,000 test accounts (bench_vu_1..1000) and global email settings
│   ├── start-app.ps1                # Starts a target with optional CPU affinity mask and background logging
│   ├── run-bench.ps1                # Automated benchmark runner (DB reset, affinity, counters, docker stats, k6)
│   └── versions.ps1                 # Generates report/versions.md from published artifacts and system info
└── report/
    ├── README.md                    # Benchmark report (methodology, tool evaluation, results, limitations)
    ├── versions.md                  # Generated environment, SDK, runtime, and package version table
    ├── probe.md                     # Capacity probe results (2 to 32 it/s)
    ├── results-main.md              # Initial main comparison (net10 vs net11, 2, 4, 6 it/s)
    ├── results-main-validation.md   # Full 27-run marathon re-validation (net10 vs net10-rt11 vs net11)
    ├── results-runtime.md           # Runtime isolation at 4 it/s
    ├── results-runtime6.md          # Runtime isolation at 6 it/s
    ├── results-verify-knee.md       # Saturation knee validation at 7 it/s (9 runs)
    └── results-verify-marathon6.md  # Targeted 6 it/s marathon validation (9 runs)
```

---

## Prerequisites

* **OS:** Windows 11 (CPU affinity and power mode inspection in scripts use Windows commands)
* **.NET SDKs & Runtimes:**
  * .NET 10 SDK (`10.0.402`) and ASP.NET Core Runtime (`10.0.13`)
  * .NET 11 Preview/RC SDK and ASP.NET Core Runtime (`11.0.100-rc.3` / `11.0.0-rc.3`)
* **Diagnostics:** `dotnet-counters` (`dotnet tool install --global dotnet-counters`)
* **Database:** Docker Desktop (runs `postgres:18.6` via `docker compose`)
* **Load Generator:** [Grafana k6](https://grafana.com/docs/k6/latest/set-up/install-k6/) (`v2.3.0+`)
* **Node.js:** `v20+` (for `k6/tools/summarize.mjs` and `k6/tools/sanitize-har.mjs`)

---

## Setup & Preparation

Run all commands from the repository root in PowerShell.

### 1. Start PostgreSQL

```powershell
docker compose up -d
```

### 2. Publish Both Projects (Release)

Publishes `Bench.Web` and `Bench.DbMigrator` for both `net10` and `net11` into `publish/<target>/`:

```powershell
./scripts/publish.ps1
```

### 3. Seed Template Databases

Runs `Bench.DbMigrator`, applies `scripts/seed-bench-data.sql` (`bench_vu_1..1000` + global email setting rows), and snapshots the result into `Bench_seed_net10` and `Bench_seed_net11`. Subsequent runs use `-Action reset` to recreate `Bench` from the template in seconds:

```powershell
./scripts/db.ps1 -Target net10 -Action seed
./scripts/db.ps1 -Target net11 -Action seed
```

### 4. Generate the Versions Report

```powershell
./scripts/versions.ps1
```

---

## Running the Scenario Manually (Smoke / Validation)

Set the seeded admin/test user password in the environment:

```powershell
$env:BENCH_PASSWORD = '<seeded_admin_password>'
```

Reset the database and start the desired target (`net10`, `net11`, or `net10-rt11`):

```powershell
./scripts/db.ps1 -Target net10 -Action reset
./scripts/start-app.ps1 -Target net10
```

In a second terminal, run the load-test scenario with 1 VU (1 iteration) or 20 concurrent VUs:

```powershell
# Single-user verification (1 iteration = 92 HTTP requests, 107 checks)
k6 run -e BENCH_PASSWORD=$env:BENCH_PASSWORD --vus 1 --iterations 1 k6/scenario.js

# Multi-user concurrency verification (20 VUs, 60 iterations, no think time)
k6 run -e BENCH_PASSWORD=$env:BENCH_PASSWORD -e THINK_TIME=0 -e SESSION_ITERATIONS=2 --vus 20 --iterations 60 k6/scenario.js
```

To run the raw correlated script exported from k6 Studio (requires `bench.localtest.me` resolving to `127.0.0.1`, which works out of the box via public DNS):

```powershell
k6 run -e BENCH_PASSWORD=$env:BENCH_PASSWORD --insecure-skip-tls-verify --vus 1 --iterations 1 k6/studio/abp-mvc-correlated.js
```

---

## Running the Full Benchmark Suite

Before running measurements:
* Plug the machine into AC power and set Windows Power Mode to **Best performance**.
* Close browsers, IDEs, k6 Studio, and background apps; leave only Docker Desktop and the PowerShell terminal open.
* Ensure `$env:BENCH_PASSWORD` is set.

### Step 1: Capacity Probe (~32 minutes)

Steps through arrival rates (`2,4,6,8,10,12,14,16,20,24,28,32` iterations/s, 60 s per step) on `net10` and `net11` to find the saturation point:

```powershell
./scripts/run-bench.ps1 -Mode probe -Repeats 1 -Batch probe
node k6/tools/summarize.mjs results/probe report/probe.md
```

Inspect `report/probe.md` and pick three load levels (`a, b, c` iterations/s) corresponding to ~25%, ~50%, and ~75% of the lower saturation threshold before dropped iterations or error spikes occur.

### Step 2: Main Comparison — `net10` vs `net11` (~2.5 hours)

Runs 3 load levels × 2 targets × 3 repetitions (120 s warm-up + 300 s measured window per run, alternating target order between repetitions):

```powershell
./scripts/run-bench.ps1 -Mode fixed -Rates 2,4,6 -Targets net10,net11 -Repeats 3 -Batch main
node k6/tools/summarize.mjs results/main report/results-main.md
```

### Step 3: Runtime Isolation — `net10` vs `net10-rt11` vs `net11` (~1 hour 15 minutes)

Runs an arrival rate across all three targets × 3 repetitions to separate runtime gains/regressions from ABP package changes:

```powershell
# Medium load (4 it/s):
./scripts/run-bench.ps1 -Mode fixed -Rates 4 -Targets net10,net10-rt11,net11 -Repeats 3 -Batch runtime
node k6/tools/summarize.mjs results/runtime report/results-runtime.md

# High load (6 it/s):
./scripts/run-bench.ps1 -Mode fixed -Rates 6 -Targets net10,net10-rt11,net11 -Repeats 3 -Batch runtime6
node k6/tools/summarize.mjs results/runtime6 report/results-runtime6.md

### Step 4: Verification Batches — Knee & Marathon (~1 hour 15 minutes each)

Targeted validation runs addressing saturation threshold and sustained-load endurance:

```powershell
# Saturation Knee (7 it/s):
./scripts/run-bench.ps1 -Mode fixed -Rates 7 -Targets net10,net10-rt11,net11 -Repeats 3 -Batch verify-knee
node k6/tools/summarize.mjs results/verify-knee report/results-verify-knee.md

# Sustained Marathon (6 it/s):
./scripts/run-bench.ps1 -Mode fixed -Rates 6 -Targets net10,net10-rt11,net11 -Repeats 3 -Batch verify-marathon6
node k6/tools/summarize.mjs results/verify-marathon6 report/results-verify-marathon6.md

# Full 3-Target Re-Validation Marathon (Rates 2, 4, 6 it/s, 27 runs, ~3.5 hours):
./scripts/run-bench.ps1 -Mode fixed -Rates 2,4,6 -Targets net10,net10-rt11,net11 -Repeats 3 -Batch main-validation
node k6/tools/summarize.mjs results/main-validation report/results-main-validation.md
```
