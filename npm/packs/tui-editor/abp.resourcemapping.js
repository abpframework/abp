module.exports = {
    imports: [
        "../jquery/abp.resourcemapping.js"
    ],
    mappings: {
        "@node_modules/to-mark/dist/to-mark.min.js": "@libs/to-mark/",
        "@node_modules/tui-code-snippet/dist/tui-code-snippet.min.js": "@libs/tui-code-snippet/",
        "@node_modules/squire-rte/squire.js": "@libs/squire-rte/",
        "@node_modules/tui-editor/dist/*.*": "@libs/tui-editor/"
    }
}