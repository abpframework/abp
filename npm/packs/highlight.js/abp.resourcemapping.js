module.exports = {
    imports: [
        "../core/abp.resourcemapping.js"
    ],
    mappings: {
        "@node_modules/highlight.js/lib/highlight.js": "@libs/highlight.js/",
        "@node_modules/highlight.js/lib/languages/*.*": "@libs/highlight.js/languages/",
        "@node_modules/highlight.js/styles/*.*": "@libs/highlight.js/styles/"
    }
}