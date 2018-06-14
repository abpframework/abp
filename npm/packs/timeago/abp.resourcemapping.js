module.exports = {
    imports: [
        "../jquery/abp.resourcemapping.js"
    ],
    mappings: {
        "@node_modules/timeago/jquery.timeago.js": "@libs/timeago/",
        "@node_modules/timeago/locales/*.*": "@libs/timeago/locales/"
    }
}