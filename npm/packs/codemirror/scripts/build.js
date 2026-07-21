const { build } = require('esbuild');
const path = require('path');

async function run() {
    await build({
        entryPoints: [path.join(__dirname, 'entry.js')],
        outfile: path.join(__dirname, '..', 'src', 'codemirror.js'),
        bundle: true,
        format: 'iife',
        platform: 'browser',
        target: ['es2019'],
        sourcemap: false,
        minify: false,
        legalComments: 'eof'
    });
}

run().catch(error => {
    console.error(error);
    process.exit(1);
});