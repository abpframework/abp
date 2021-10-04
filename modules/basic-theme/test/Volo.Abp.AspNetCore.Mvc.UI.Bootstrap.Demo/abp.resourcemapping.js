module.exports = {
    aliases: { //TODO: Make some aliases default: node_modules, libs
        "@node_modules": "./node_modules",
        "@libs": "./wwwroot/libs"
    },
    clean: [
        "@libs"
    ],
    mappings: {
        "@node_modules/highlight.js/**/*.*": "@libs/highlight.js/"
    }
}