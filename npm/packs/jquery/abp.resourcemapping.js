module.exports = {
    imports: [
        "../core/abp.resourcemapping.js"
    ],
    mappings: {
        "@node_modules/jquery/dist/jquery.js": "@libs/jquery/",

        "@node_modules/@abp/jquery/src/*.*": "@libs/abp/jquery/"
    }
}