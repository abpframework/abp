module.exports = {
    aliases: {
        "@node_modules": "./node_modules",
        "@libs": "./wwwroot/libs"
    },
    clean: [
        "@libs"
    ],
    mappings: {
        "@node_modules/@microsoft/signalr/dist/browser/signalr.js": "@libs/signalr/",
        "@node_modules/@microsoft/signalr/dist/browser/signalr.js.map": "@libs/signalr/"
    }
};
