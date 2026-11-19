// Load-test scenario for the empty ABP MVC app, built from a k6 Studio recording of the browser session (the recording itself is not committed).
// One iteration visits every page and runs its functions; each virtual user (VU) works only on its own data:
//  - VU n logs in as bench_vu_n (seeded by scripts/seed-bench-data.sql) and keeps its session across iterations,
//    logging out and in again every SESSION_ITERATIONS iterations (password hashing would dominate otherwise).
//  - users/roles/tenants it creates get unique names per run, VU and iteration, so VUs never collide.
//  - every page also loads ABP's dynamic scripts (/Abp/...Script) like the browser does; static files are skipped.
//
// Run: k6 run -e BENCH_PASSWORD=<seed password> --vus 1 --iterations 1 k6/scenario.js
// Env: BASE_URL (default https://localhost:44377), BENCH_PASSWORD (required), THINK_TIME seconds between groups
//      (default 1), SESSION_ITERATIONS (default 10), MAX_VUS (default and cap: seeded accounts).
// Load profiles (open model: iterations started per second, so every target gets exactly the same load):
//  MODE=smoke (default)  VUs/iterations from the CLI.
//  MODE=fixed  RATE=<it/s> WARMUP_S=120 DURATION_S=300   warm-up, then the measured scenario "measure".
//  MODE=probe  PROBE_RATES=2,4,... STEP_S=60             warm-up, then one scenario "step_NNN" per rate.
// Measured scenarios get always-passing thresholds so their (and their groups') metrics appear in --summary-export.
import http from 'k6/http';
import exec from 'k6/execution';
import { check, group, sleep } from 'k6';

const BASE = __ENV.BASE_URL || 'https://localhost:44377';
const PASSWORD = __ENV.BENCH_PASSWORD;
const THINK_TIME = Number(__ENV.THINK_TIME ?? 1);
const SESSION_ITERATIONS = Number(__ENV.SESSION_ITERATIONS ?? 10);
const SEEDED_USERS = 1000;
// Unique per k6 process and VU, so names never repeat across runs on the same database either.
const RUN = Date.now().toString(36);

if (!PASSWORD) throw new Error('BENCH_PASSWORD is required (password of the seeded admin / bench_vu_n users)');

const MODE = __ENV.MODE || 'smoke';
const MAX_VUS = Math.min(Number(__ENV.MAX_VUS ?? SEEDED_USERS), SEEDED_USERS);
const GROUPS = ['01_login', '02_home', '03_users', '04_roles', '05_tenants', '06_settings', '07_profile', '08_language', '09_logout'];
// Scenarios run back to back with this gap, so they never overlap and reuse the same (already logged-in) VUs.
const GRACE_S = 10;

function arrival(rate, durationS, startS) {
    return {
        executor: 'constant-arrival-rate', rate, timeUnit: '1s', duration: `${durationS}s`, startTime: `${startS}s`,
        gracefulStop: `${GRACE_S}s`,
        // One iteration takes ~10 s with THINK_TIME=1; keep headroom so slow responses don't drop iterations early.
        preAllocatedVUs: Math.min(MAX_VUS, Math.ceil(rate * 12)), maxVUs: MAX_VUS,
    };
}

function buildScenarios() {
    if (MODE === 'fixed') {
        const rate = Number(__ENV.RATE);
        if (!(rate > 0)) throw new Error('MODE=fixed needs RATE (iterations per second)');
        const warmup = Number(__ENV.WARMUP_S ?? 120);
        return { warmup: arrival(rate, warmup, 0), measure: arrival(rate, Number(__ENV.DURATION_S ?? 300), warmup + GRACE_S) };
    }
    if (MODE === 'probe') {
        const rates = (__ENV.PROBE_RATES || '2,4,6,8,10,12,14,16,20,24,28,32').split(',').map(Number);
        const step = Number(__ENV.STEP_S ?? 60);
        const scenarios = { warmup: arrival(rates[0], step, 0) };
        rates.forEach((r, i) => { scenarios[`step_${String(r).padStart(3, '0')}`] = arrival(r, step, (i + 1) * (step + GRACE_S)); });
        return scenarios;
    }
    if (MODE !== 'smoke') throw new Error(`unknown MODE ${MODE}`);
    return undefined;
}

function summaryThresholds(scenarioNames) {
    const thresholds = {};
    for (const s of scenarioNames) {
        const add = (metric, expr) => { thresholds[`${metric}{scenario:${s}}`] = [expr]; };
        add('http_req_duration', 'max>=0');
        add('http_req_failed', 'rate>=0');
        add('http_reqs', 'count>=0');
        add('iterations', 'count>=0');
        add('dropped_iterations', 'count>=0');
        add('checks', 'rate>=0');
        for (const g of GROUPS) {
            thresholds[`http_req_duration{scenario:${s},group:::${g}}`] = ['max>=0'];
            thresholds[`group_duration{scenario:${s},group:::${g}}`] = ['max>=0'];
        }
    }
    return thresholds;
}

const scenarios = buildScenarios();

export const options = {
    insecureSkipTLSVerify: true, // ASP.NET Core development certificate
    noCookiesReset: true, // k6 clears cookies every iteration by default, which would drop the session
    summaryTrendStats: ['avg', 'med', 'p(95)', 'p(99)', 'max'],
    ...(scenarios && { scenarios, thresholds: summaryThresholds(Object.keys(scenarios).filter((s) => s !== 'warmup')) }),
};

const HTML = { accept: 'text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8' };
const TOKEN_RE = /name="__RequestVerificationToken" type="hidden" value="([^"]+)"/;

// Per-VU state (module scope lives as long as the VU).
let loggedIn = false;
let culture = 'en';
let formToken = ''; // latest anti-forgery token from a page or modal form

function remember(res) {
    const m = TOKEN_RE.exec(res.body || '');
    if (m) formToken = m[1];
    return res;
}

function stampOf(res) {
    const m = /name="(?:\w+\.)?ConcurrencyStamp" value="([^"]+)"/.exec(res.body || '');
    return m ? m[1] : '';
}

// ABP's JS sends the XSRF-TOKEN cookie (set by ApplicationConfigurationScript) as the RequestVerificationToken header.
function xsrf() {
    const values = http.cookieJar().cookiesForURL(BASE)['XSRF-TOKEN'];
    return values ? values[0] : '';
}

function ok(res, name, expectedStatus = 200, extra) {
    const statusOk = res.status === expectedStatus;
    return check(res, {
        [`${name} -> ${expectedStatus}`]: () => statusOk,
        // Content is only inspected on the expected status; a login/error page would not even be JSON.
        ...(extra ? { [`${name} content`]: (r) => statusOk && extra(r) } : {}),
    });
}

const jsonOr = (res, path, fallback) => (res.status === 200 ? res.json(path) : fallback);

// Full page: the HTML plus the three dynamic ABP scripts the layout loads on every page (in parallel, like a browser).
function page(path, name = path, { anonymous = false } = {}) {
    const res = remember(http.get(`${BASE}${path}`, { headers: HTML, tags: { name: `GET ${name}` } }));
    ok(res, `GET ${name}`, 200, anonymous ? undefined : (r) => !r.url.includes('/Account/Login'));
    const scripts = http.batch([
        ['GET', `${BASE}/Abp/ApplicationConfigurationScript`, null, { tags: { name: 'GET /Abp/ApplicationConfigurationScript' } }],
        ['GET', `${BASE}/Abp/ApplicationLocalizationScript?cultureName=${culture}`, null, { tags: { name: 'GET /Abp/ApplicationLocalizationScript' } }],
        ['GET', `${BASE}/Abp/ServiceProxyScript`, null, { tags: { name: 'GET /Abp/ServiceProxyScript' } }],
    ]);
    scripts.forEach((r) => ok(r, r.request.url.includes('Configuration') ? 'GET /Abp/ApplicationConfigurationScript'
        : r.request.url.includes('Localization') ? 'GET /Abp/ApplicationLocalizationScript' : 'GET /Abp/ServiceProxyScript'));
    return res;
}

// Requests ABP's JS makes from a page (jQuery AJAX): JSON API calls, modal HTML and modal form posts.
function ajax(method, path, { name = path, json, form, accept } = {}) {
    const headers = {
        'x-requested-with': 'XMLHttpRequest',
        requestverificationtoken: xsrf(),
        accept: accept || (form || method === 'GET' && path.includes('Modal') ? 'text/html, */*; q=0.01' : 'application/json, text/javascript, */*; q=0.01'),
    };
    let body = null;
    if (json !== undefined) {
        headers['content-type'] = 'application/json';
        body = JSON.stringify(json);
    } else if (form) {
        // Modal forms are posted as FormData (multipart), including the form's own anti-forgery field.
        const boundary = `----k6Boundary${Math.random().toString(16).slice(2)}`;
        headers['content-type'] = `multipart/form-data; boundary=${boundary}`;
        body = Object.entries({ ...form, __RequestVerificationToken: formToken })
            .map(([k, v]) => `--${boundary}\r\nContent-Disposition: form-data; name="${k}"\r\n\r\n${v}\r\n`)
            .join('') + `--${boundary}--\r\n`;
    }
    return remember(http.request(method, `${BASE}${path}`, body, { headers, tags: { name: `${method} ${name}` } }));
}

function switchLanguage(to, returnUrl) {
    const res = http.get(`${BASE}/Abp/Languages/Switch?culture=${to}&uiCulture=${to}&returnUrl=${encodeURIComponent(returnUrl)}`,
        { headers: HTML, redirects: 0, tags: { name: 'GET /Abp/Languages/Switch' } });
    ok(res, 'GET /Abp/Languages/Switch', 302);
    culture = to;
}

function login(userName) {
    page('/Account/Login', '/Account/Login', { anonymous: true });
    switchLanguage('en', '/Account/Login');
    page('/Account/Login', '/Account/Login', { anonymous: true });
    const res = http.post(`${BASE}/Account/Login`, {
        'LoginInput.UserNameOrEmailAddress': userName,
        'LoginInput.Password': PASSWORD,
        Action: 'Login',
        __RequestVerificationToken: formToken,
        'LoginInput.RememberMe': 'false',
    }, { headers: HTML, redirects: 0, tags: { name: 'POST /Account/Login' } });
    ok(res, 'POST /Account/Login', 302, (r) => r.headers.Location === '/');
    page('/', '/', { anonymous: true });
    loggedIn = true;
}

function users(n) {
    const name = `u_${n}`;
    const search = (filter, label) => ajax('GET', `/api/identity/users?filter=${filter}&skipCount=0&maxResultCount=10`,
        { name: `/api/identity/users?filter=${label}` });

    page('/Identity/Users');
    ok(search('', ''), 'GET /api/identity/users', 200, (r) => r.json('totalCount') >= SEEDED_USERS);
    ok(search('bench', 'bench'), 'GET /api/identity/users?filter=bench');
    ok(ajax('GET', '/Identity/Users/CreateModal'), 'GET /Identity/Users/CreateModal');
    const userInfo = {
        'UserInfo.UserName': name, 'UserInfo.Name': '', 'UserInfo.Surname': '', 'UserInfo.Password': PASSWORD,
        'UserInfo.Email': `${name}@bench.local`, 'UserInfo.PhoneNumber': '', 'UserInfo.IsActive': 'true',
        'UserInfo.LockoutEnabled': 'true', 'Roles[0].Name': 'admin',
    };
    ok(ajax('POST', '/Identity/Users/CreateModal', { form: userInfo }), 'POST /Identity/Users/CreateModal', 204);
    search('bench', 'bench');
    const found = search(name, '{name}');
    ok(found, 'GET /api/identity/users?filter={name}', 200, (r) => r.json('totalCount') === 1);
    const id = jsonOr(found, 'items.0.id');

    const edit = ajax('GET', `/Identity/Users/EditModal?id=${id}`, { name: '/Identity/Users/EditModal?id={id}' });
    ok(edit, 'GET /Identity/Users/EditModal', 200, (r) => r.body.includes(name));
    ok(ajax('POST', '/Identity/Users/EditModal', {
        form: { 'UserInfo.Id': id, 'UserInfo.ConcurrencyStamp': stampOf(edit), ...userInfo, 'UserInfo.Name': 'Rec', 'UserInfo.Password': '' },
    }), 'POST /Identity/Users/EditModal', 204);
    search(name, '{name}');

    ok(ajax('GET', `/AbpPermissionManagement/PermissionManagementModal?providerName=U&providerKey=${id}&providerKeyDisplayName=${name}`,
        { name: '/AbpPermissionManagement/PermissionManagementModal?providerName=U' }), 'GET PermissionManagementModal (user)');
    ok(ajax('POST', '/AbpPermissionManagement/PermissionManagementModal', {
        form: { ProviderKey: id, ProviderName: 'U', SelectAllInAllTabs: 'false', OnlyChangedPermissions: 'true',
            GrantedPermissionNames: 'FeatureManagement.ManageHostFeatures', RevokedPermissionNames: '' },
    }), 'POST PermissionManagementModal (user)', 204);

    ok(ajax('DELETE', `/api/identity/users/${id}`, { name: '/api/identity/users/{id}' }), 'DELETE /api/identity/users/{id}', 204);
    ok(search(name, '{name}'), 'GET /api/identity/users?filter={name} (deleted)', 200, (r) => r.json('totalCount') === 0);
}

function roles(n) {
    const name = `r_${n}`;
    const list = () => ajax('GET', '/api/identity/roles?skipCount=0&maxResultCount=10');
    // The page lists 10 roles; with many VUs ours may not be among them, so look it up by name.
    const find = () => ajax('GET', `/api/identity/roles?filter=${name}&skipCount=0&maxResultCount=10`, { name: '/api/identity/roles?filter={name}' });

    page('/Identity/Roles');
    ok(list(), 'GET /api/identity/roles');
    ok(ajax('GET', '/Identity/Roles/CreateModal'), 'GET /Identity/Roles/CreateModal');
    const role = { 'Role.Name': name, 'Role.IsDefault': 'false', 'Role.IsPublic': 'false' };
    ok(ajax('POST', '/Identity/Roles/CreateModal', { form: role }), 'POST /Identity/Roles/CreateModal', 204);
    list();
    const found = find();
    const id = jsonOr(found, 'items', []).find((x) => x.name === name)?.id;
    ok(found, 'GET /api/identity/roles?filter={name}', 200, () => id !== undefined);

    const edit = ajax('GET', `/Identity/Roles/EditModal?id=${id}`, { name: '/Identity/Roles/EditModal?id={id}' });
    ok(edit, 'GET /Identity/Roles/EditModal', 200, (r) => r.body.includes(name));
    ok(ajax('POST', '/Identity/Roles/EditModal', { form: { 'Role.Id': id, 'Role.ConcurrencyStamp': stampOf(edit), ...role } }),
        'POST /Identity/Roles/EditModal', 204);
    list();

    ok(ajax('GET', `/AbpPermissionManagement/PermissionManagementModal?providerName=R&providerKey=${name}&providerKeyDisplayName=${name}`,
        { name: '/AbpPermissionManagement/PermissionManagementModal?providerName=R' }), 'GET PermissionManagementModal (role)');
    ok(ajax('POST', '/AbpPermissionManagement/PermissionManagementModal', {
        form: { ProviderKey: name, ProviderName: 'R', SelectAllInAllTabs: 'false', OnlyChangedPermissions: 'true',
            GrantedPermissionNames: 'FeatureManagement.ManageHostFeatures', RevokedPermissionNames: '' },
    }), 'POST PermissionManagementModal (role)', 204);

    ok(ajax('DELETE', `/api/identity/roles/${id}`, { name: '/api/identity/roles/{id}' }), 'DELETE /api/identity/roles/{id}', 204);
    ok(find(), 'GET /api/identity/roles?filter={name} (deleted)', 200, (r) => r.json('totalCount') === 0);
}

function tenants(n) {
    const name = `t_${n}`;
    const list = () => ajax('GET', '/api/multi-tenancy/tenants?filter=&sorting=name%20asc&skipCount=0&maxResultCount=10');
    const find = () => ajax('GET', `/api/multi-tenancy/tenants?filter=${name}&sorting=name%20asc&skipCount=0&maxResultCount=10`,
        { name: '/api/multi-tenancy/tenants?filter={name}' });

    page('/TenantManagement/Tenants');
    ok(list(), 'GET /api/multi-tenancy/tenants');
    ok(ajax('GET', '/TenantManagement/Tenants/CreateModal'), 'GET /TenantManagement/Tenants/CreateModal');
    ok(ajax('POST', '/TenantManagement/Tenants/CreateModal', {
        form: { 'Tenant.Name': name, 'Tenant.AdminEmailAddress': `admin@${name}.local`, 'Tenant.AdminPassword': PASSWORD },
    }), 'POST /TenantManagement/Tenants/CreateModal', 204);
    list();
    const found = find();
    ok(found, 'GET /api/multi-tenancy/tenants?filter={name}', 200, (r) => r.json('totalCount') === 1);
    const id = jsonOr(found, 'items.0.id');

    const edit = ajax('GET', `/TenantManagement/Tenants/EditModal?id=${id}`, { name: '/TenantManagement/Tenants/EditModal?id={id}' });
    ok(edit, 'GET /TenantManagement/Tenants/EditModal', 200, (r) => r.body.includes(name));
    ok(ajax('POST', '/TenantManagement/Tenants/EditModal', {
        form: { 'Tenant.Id': id, 'Tenant.ConcurrencyStamp': stampOf(edit), 'Tenant.Name': name },
    }), 'POST /TenantManagement/Tenants/EditModal', 204);
    list();

    ok(ajax('GET', `/FeatureManagement/FeatureManagementModal?providerName=T&providerKey=${id}&providerKeyDisplayName=${name}`,
        { name: '/FeatureManagement/FeatureManagementModal?providerName=T&providerKey={id}' }), 'GET FeatureManagementModal (tenant)');
    ok(ajax('POST', '/FeatureManagement/FeatureManagementModal', {
        form: {
            ProviderKey: id, ProviderName: 'T',
            'FeatureGroups[0].Features[0].IsDisabled': 'False', 'FeatureGroups[0].Features[0].Type': 'ToggleStringValueType',
            'FeatureGroups[0].Features[0].BoolValue': 'true', 'FeatureGroups[0].Features[0].Name': 'SettingManagement.Enable',
            'FeatureGroups[0].Features[1].IsDisabled': 'False', 'FeatureGroups[0].Features[1].Type': 'ToggleStringValueType',
            'FeatureGroups[0].Features[1].Name': 'SettingManagement.AllowChangingEmailSettings',
        },
    }), 'POST FeatureManagementModal (tenant)', 204);

    ok(ajax('DELETE', `/api/multi-tenancy/tenants/${id}`, { name: '/api/multi-tenancy/tenants/{id}' }), 'DELETE /api/multi-tenancy/tenants/{id}', 204);
    ok(find(), 'GET /api/multi-tenancy/tenants?filter={name} (deleted)', 200, (r) => r.json('totalCount') === 0);
}

function settings() {
    page('/SettingManagement');
    ok(ajax('POST', '/SettingManagement?handler=RenderView&id=Volo.Abp.EmailSetting', { accept: 'text/html, */*; q=0.01' }),
        'POST /SettingManagement?handler=RenderView (email)', 200, (r) => TOKEN_RE.test(r.body));
    ok(ajax('POST', '/api/setting-management/emailing', {
        json: {
            defaultFromDisplayName: 'ABP application', defaultFromAddress: 'noreply@abp.io', smtpHost: '127.0.0.1', smtpPort: '25',
            __Invariant: 'SmtpPort', smtpUseDefaultCredentials: 'true', smtpDomain: '', smtpUserName: '', smtpPassword: '',
            __RequestVerificationToken: formToken, smtpEnableSsl: 'false',
        },
    }), 'POST /api/setting-management/emailing', 204);
    ok(ajax('POST', '/SettingManagement?handler=RefreshConfiguration', { json: undefined }), 'POST /SettingManagement?handler=RefreshConfiguration', 204);
    ok(ajax('POST', '/SettingManagement?handler=RenderView&id=Volo.Abp.FeatureManagement', { accept: 'text/html, */*; q=0.01' }),
        'POST /SettingManagement?handler=RenderView (features)');
    ok(ajax('GET', '/FeatureManagement/FeatureManagementModal?providerName=T', { name: '/FeatureManagement/FeatureManagementModal?providerName=T' }),
        'GET FeatureManagementModal (host)');
}

function profile(userName, vu) {
    const manage = page('/Account/Manage');
    ok(ajax('PUT', '/api/account/my-profile', {
        json: {
            ConcurrencyStamp: stampOf(manage), UserName: userName, Name: 'Bench Rec', Surname: `VU ${vu}`,
            Email: `${userName}@bench.local`, PhoneNumber: '', __RequestVerificationToken: formToken,
        },
    }), 'PUT /api/account/my-profile', 200, (r) => r.json('userName') === userName);
    ok(ajax('GET', '/api/account/my-profile'), 'GET /api/account/my-profile');
}

function language() {
    switchLanguage('tr', '/Account/Manage');
    page('/Account/Manage');
    switchLanguage('en', '/Account/Manage');
    page('/Account/Manage');
}

function logout() {
    const res = http.get(`${BASE}/Account/Logout`, { headers: HTML, redirects: 0, tags: { name: 'GET /Account/Logout' } });
    ok(res, 'GET /Account/Logout', 302);
    page('/Account/Login', '/Account/Login', { anonymous: true });
    loggedIn = false;
}

export default function () {
    const vu = exec.vu.idInTest;
    if (vu > SEEDED_USERS) exec.test.abort(`VU ${vu} has no seeded account (bench_vu_1..${SEEDED_USERS})`);
    const userName = `bench_vu_${vu}`;
    const iteration = exec.vu.iterationInInstance;
    const n = `${RUN}_${vu}_${iteration}`;

    if (!loggedIn) {
        group('01_login', () => login(userName));
        sleep(THINK_TIME);
    }
    group('02_home', () => page('/', '/', {}));
    sleep(THINK_TIME);
    group('03_users', () => users(n));
    sleep(THINK_TIME);
    group('04_roles', () => roles(n));
    sleep(THINK_TIME);
    group('05_tenants', () => tenants(n));
    sleep(THINK_TIME);
    group('06_settings', () => settings());
    sleep(THINK_TIME);
    group('07_profile', () => profile(userName, vu));
    sleep(THINK_TIME);
    group('08_language', () => language());
    sleep(THINK_TIME);
    if ((iteration + 1) % SESSION_ITERATIONS === 0) {
        group('09_logout', () => logout());
        sleep(THINK_TIME);
    }
}
