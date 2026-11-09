// Makes a k6 Studio recording safe to commit:
// keeps only the app's requests, drops static response bodies, and redacts passwords and cookie values.
// Usage: node k6/tools/sanitize-har.mjs <raw.har> <out.har> [host]
import { readFileSync, writeFileSync } from 'node:fs';

const [input, output, host = 'bench.localtest.me:44377'] = process.argv.slice(2);
if (!input || !output) throw new Error('usage: sanitize-har.mjs <raw.har> <out.har> [host]');

const REDACTED = '<redacted>';
const isStatic = (path) => /^\/(libs|__bundles|Themes|images|fonts|favicon)|\.(js|css|woff2?|png|svg|ico|map)$/i.test(path);
const isPasswordField = (name) => /password/i.test(name);

const redactCookies = (list = []) => list.forEach((c) => { c.value = REDACTED; });
const redactHeaders = (list = []) => list.forEach((h) => {
    if (/^(cookie|set-cookie|requestverificationtoken)$/i.test(h.name)) h.value = REDACTED;
});
const redactText = (text) => text
    // JSON bodies: "somePassword":"value"
    .replace(/("[^"]*password[^"]*"\s*:\s*")[^"]*/gi, `$1${REDACTED}`)
    // multipart bodies: name="X.Password"\r\n\r\nvalue
    .replace(/(name="[^"]*password[^"]*"\r?\n\r?\n)[^\r\n]*/gi, `$1${REDACTED}`);

const har = JSON.parse(readFileSync(input, 'utf8'));
const log = har.log;

log.entries = log.entries.filter((e) => new URL(e.request.url).host === host);
for (const e of log.entries) {
    const { pathname } = new URL(e.request.url);
    redactHeaders(e.request.headers);
    redactHeaders(e.response.headers);
    redactCookies(e.request.cookies);
    redactCookies(e.response.cookies);
    const post = e.request.postData;
    if (post?.params) post.params.forEach((p) => { if (isPasswordField(p.name)) p.value = REDACTED; });
    if (post?.text) post.text = redactText(post.text);
    if (isStatic(pathname) && e.response.content) {
        delete e.response.content.text;
        delete e.response.content.encoding;
    }
}

const events = log._browserEvents?.events ?? [];
log._browserEvents.events = events.filter((ev) => !ev.url || new URL(ev.url).host === host);
for (const ev of log._browserEvents.events) {
    if (ev.sensitive || isPasswordField(JSON.stringify(ev.target ?? {}))) ev.value = REDACTED;
}

writeFileSync(output, JSON.stringify(har, null, 1));
console.log(`${log.entries.length} entries, ${log._browserEvents.events.length} browser events -> ${output}`);
