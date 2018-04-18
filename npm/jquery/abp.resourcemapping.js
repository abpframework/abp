module.exports = {
    imports: [
        "../core/abp.resourcemapping.js"
    ],
    mappings: {
        //jQuery
        "@node_modules/jquery/dist/jquery.js": "@libs/jquery/",

        //@abp/jquery
        "@node_modules/@abp/jquery/src/*.*": "@libs/abp/jquery/"
    }
}