module.exports = {
    imports: [
        "./node_modules/@abp/aspnetcore.mvc.ui.theme.basic/abp.resourcemapping.js"
    ],
    aliases: {
        "@node_modules": "./node_modules",
        "@libs": "./wwwroot/libs"
    },
    clean: [
        "@libs"
    ],
    mappings: {

    }
}