// Summarizes a results/<batch> folder written by scripts/run-bench.ps1 into Markdown tables.
//   probe batch: one row per target and load step (to find the saturation point)
//   fixed batch: one row per target and rate, median over the repetitions (min-max in brackets)
// Usage: node k6/tools/summarize.mjs results/<batch> [out.md]
import { readFileSync, readdirSync, existsSync, writeFileSync } from 'node:fs';
import { join } from 'node:path';

const [batchDir, outFile] = process.argv.slice(2);
if (!batchDir) throw new Error('usage: summarize.mjs results/<batch> [out.md]');
const batch = JSON.parse(readFileSync(join(batchDir, 'batch.json'), 'utf8'));
const readJson = (f) => JSON.parse(readFileSync(f, 'utf8'));
const avg = (xs) => (xs.length ? xs.reduce((a, b) => a + b, 0) / xs.length : NaN);
const median = (xs) => { const s = [...xs].sort((a, b) => a - b); const m = s.length >> 1; return s.length % 2 ? s[m] : (s[m - 1] + s[m]) / 2; };

// dotnet-counters CSV: Timestamp(local),Provider,Counter Name,Counter Type,Mean/Increment
function readCounters(file) {
    return readFileSync(file, 'utf8').split(/\r?\n/).slice(1).filter(Boolean).map((line) => {
        const parts = line.split(',');
        return { t: new Date(parts[0]).getTime(), name: parts.slice(2, -2).join(','), value: Number(parts.at(-1)) };
    });
}

// docker stats: <UTC ISO>\t{json}
function readDocker(file) {
    if (!existsSync(file)) return [];
    return readFileSync(file, 'utf8').split(/\r?\n/).filter((l) => l.includes('\t{')).map((l) => {
        const [t, json] = l.split('\t');
        const s = JSON.parse(json);
        const mem = /([\d.]+)(\w+)/.exec(s.MemUsage);
        const mib = mem ? Number(mem[1]) * ({ KiB: 1 / 1024, MiB: 1, GiB: 1024 }[mem[2]] ?? 1) : NaN;
        return { t: new Date(t).getTime(), cpu: parseFloat(s.CPUPerc), memMiB: mib };
    });
}

// Server-side figures for one time window [from, to) in ms.
function serverStats(counters, docker, from, to) {
    const inWin = counters.filter((c) => c.t >= from && c.t < to);
    const series = (prefix) => inWin.filter((c) => c.name.startsWith(prefix));
    const perSecond = (prefix) => { // sum the tagged series per timestamp, e.g. cpu.time user + system
        const byT = new Map();
        for (const c of series(prefix)) byT.set(c.t, (byT.get(c.t) ?? 0) + c.value);
        return [...byT.values()];
    };
    const cpus = series('dotnet.process.cpu.count').map((c) => c.value).at(-1) || 1;
    const cpu = perSecond('dotnet.process.cpu.time').map((v) => (v / cpus) * 100);
    const ws = series('dotnet.process.memory.working_set').map((c) => c.value / 2 ** 20);
    const heap = series('dotnet.gc.last_collection.heap.size').length ? perSecond('dotnet.gc.last_collection.heap.size').map((v) => v / 2 ** 20) : [];
    const gcPause = perSecond('dotnet.gc.pause.time').map((v) => v * 1000);
    const gen2 = series('dotnet.gc.collections ({collection} / 1 sec)[gc.heap.generation=gen2]').reduce((a, c) => a + c.value, 0);
    const tpQueue = series('dotnet.thread_pool.queue.length').map((c) => c.value);
    const db = docker.filter((d) => d.t >= from && d.t < to);
    return {
        cpuAvg: avg(cpu), cpuMax: Math.max(...cpu), wsAvg: avg(ws), wsMax: Math.max(...ws), heapAvg: avg(heap),
        gcPauseMsPerS: avg(gcPause), gen2, tpQueueMax: Math.max(0, ...tpQueue),
        dbCpuAvg: avg(db.map((d) => d.cpu)), dbMemAvg: avg(db.map((d) => d.memMiB)),
    };
}

function k6Stats(metrics, scenario, seconds) {
    const m = (name) => metrics[`${name}{scenario:${scenario}}`] ?? {};
    const d = m('http_req_duration');
    return {
        rps: (m('http_reqs').count ?? 0) / seconds, itps: (m('iterations').count ?? 0) / seconds,
        avg: d.avg, med: d.med, p95: d['p(95)'], p99: d['p(99)'],
        failed: m('http_req_failed').value ?? 0, checks: m('checks').value ?? 0, dropped: m('dropped_iterations').count ?? 0,
        groups: Object.fromEntries(Object.entries(metrics)
            .filter(([k]) => k.startsWith(`http_req_duration{scenario:${scenario},group:::`))
            .map(([k, v]) => [k.match(/group:::(\w+)/)[1], v.max > 0 ? v['p(95)'] : NaN])), // max 0 = no requests
    };
}

function runs() {
    const out = [];
    for (const target of readdirSync(batchDir, { withFileTypes: true }).filter((d) => d.isDirectory()).map((d) => d.name)) {
        for (const label of readdirSync(join(batchDir, target))) {
            for (const rep of readdirSync(join(batchDir, target, label))) {
                const dir = join(batchDir, target, label, rep);
                if (!existsSync(join(dir, 'k6-summary.json'))) { console.error(`skipping ${dir}: no k6 summary`); continue; }
                out.push({ target, label, rep, dir, run: readJson(join(dir, 'run.json')), metrics: readJson(join(dir, 'k6-summary.json')).metrics });
            }
        }
    }
    return out;
}

const f = (x, d = 1) => (Number.isFinite(x) ? x.toFixed(d) : '-');
const pct = (x) => (Number.isFinite(x) ? `${(x * 100).toFixed(2)}%` : '-');
const lines = [`# Results: ${batchDir.split(/[\\/]/).at(-1)}`, '', '```json', JSON.stringify(batch), '```', ''];

if (batch.mode === 'probe') {
    const rates = String(batch.probeRates).split(',').map(Number);
    lines.push('| target | rate it/s | req/s | p95 ms | p99 ms | failed | dropped it | app CPU % (avg/max) | working set MB | GC pause ms/s | TP queue max | DB CPU % |',
        '|---|---|---|---|---|---|---|---|---|---|---|---|');
    for (const r of runs()) {
        const counters = readCounters(join(r.dir, 'counters.csv'));
        const docker = readDocker(join(r.dir, 'docker-stats.tsv'));
        const t0 = new Date(r.run.startedUtc).getTime();
        rates.forEach((rate, i) => {
            const s = `step_${String(rate).padStart(3, '0')}`;
            const from = t0 + (i + 1) * (batch.stepS + batch.graceS) * 1000;
            const k = k6Stats(r.metrics, s, batch.stepS);
            const srv = serverStats(counters, docker, from + 5000, from + batch.stepS * 1000); // skip the ramp-in seconds
            lines.push(`| ${r.target} | ${rate} | ${f(k.rps)} | ${f(k.p95)} | ${f(k.p99)} | ${pct(k.failed)} | ${k.dropped} | ${f(srv.cpuAvg)} / ${f(srv.cpuMax)} | ${f(srv.wsAvg, 0)} | ${f(srv.gcPauseMsPerS)} | ${srv.tpQueueMax} | ${f(srv.dbCpuAvg)} |`);
        });
    }
} else {
    const groups = new Map();
    for (const r of runs()) {
        const counters = readCounters(join(r.dir, 'counters.csv'));
        const docker = readDocker(join(r.dir, 'docker-stats.tsv'));
        const from = new Date(r.run.startedUtc).getTime() + r.run.measureStartOffsetS * 1000;
        const row = { ...k6Stats(r.metrics, 'measure', batch.durationS), ...serverStats(counters, docker, from, from + batch.durationS * 1000) };
        const key = `${r.label}|${r.target}`;
        groups.set(key, [...(groups.get(key) ?? []), row]);
    }
    const cell = (rows, k, d = 1) => {
        const xs = rows.map((r) => r[k]).filter(Number.isFinite);
        return xs.length > 1 ? `${f(median(xs), d)} (${f(Math.min(...xs), d)}–${f(Math.max(...xs), d)})` : f(xs[0], d);
    };
    const sorted = [...groups.entries()].sort(([a], [b]) => a.localeCompare(b, 'en', { numeric: true }));
    lines.push('Median of repetitions (min–max).', '',
        '| rate | target | reps | req/s | avg ms | p95 ms | p99 ms | failed | app CPU % | CPU max % | working set MB | GC heap MB | GC pause ms/s | gen2 GCs | DB CPU % |',
        '|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|');
    for (const [key, rows] of sorted) {
        const [label, target] = key.split('|');
        lines.push(`| ${label.replace('rate', '')} it/s | ${target} | ${rows.length} | ${cell(rows, 'rps')} | ${cell(rows, 'avg')} | ${cell(rows, 'p95')} | ${cell(rows, 'p99')} | ${pct(median(rows.map((r) => r.failed)))} | ${cell(rows, 'cpuAvg')} | ${cell(rows, 'cpuMax')} | ${cell(rows, 'wsAvg', 0)} | ${cell(rows, 'heapAvg', 0)} | ${cell(rows, 'gcPauseMsPerS')} | ${cell(rows, 'gen2', 0)} | ${cell(rows, 'dbCpuAvg')} |`);
    }
    lines.push('', '## p95 per page group (ms, median of repetitions)', '');
    const groupNames = [...new Set(sorted.flatMap(([, rows]) => Object.keys(rows[0].groups)))].sort();
    lines.push(`| rate | target | ${groupNames.join(' | ')} |`, `|---|---|${groupNames.map(() => '---').join('|')}|`);
    for (const [key, rows] of sorted) {
        const [label, target] = key.split('|');
        lines.push(`| ${label.replace('rate', '')} | ${target} | ${groupNames.map((g) => f(median(rows.map((r) => r.groups[g]).filter(Number.isFinite)))).join(' | ')} |`);
    }
}

const md = lines.join('\n') + '\n';
if (outFile) writeFileSync(outFile, md);
process.stdout.write(md);
