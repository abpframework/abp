module.exports = {
    aliases: {
        "@node_modules": "./node_modules",
        "@libs": "./wwwroot/libs"
    },
    clean: [
        "@libs/jszip",
        "@libs/devextreme",        
        "@libs/devextreme-aspnet-data"
    ],
    mappings: {
        "@node_modules/jszip/dist": "@libs/jszip",
        "@node_modules/devextreme": "@libs/devextreme",        
        "@node_modules/devextreme-aspnet-data/js": "@libs/devextreme-aspnet-data"
    }
}