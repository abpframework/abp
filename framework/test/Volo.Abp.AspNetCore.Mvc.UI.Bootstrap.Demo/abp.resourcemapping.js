module.exports = {
    imports: [
        "node_modules/@abp/aspnetcore.mvc.ui.theme.shared/abp.resourcemapping.js"
    ],
    aliases: { //TODO: Make some aliases default: node_modules, libs
        "@node_modules": "./node_modules",
        "@libs": "./wwwroot/libs"
    },
    clean: [
        "@libs"
    ],
    mappings: {
        
    }
}