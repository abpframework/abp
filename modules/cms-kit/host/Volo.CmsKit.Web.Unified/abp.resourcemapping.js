module.exports = {
    aliases: {
        "@node_modules": "./node_modules",
        "@libs": "./wwwroot/libs"
    },
    clean: [
        "@libs"
    ],
    mappings: {
        "@node_modules/codemirror/lib/*.*": "@libs/codemirror/",
        "@node_modules/codemirror/mode/**/*.*": "@libs/codemirror/mode/",
        "@node_modules/codemirror/theme/**/*.*": "@libs/codemirror/theme/",
        "@node_modules/codemirror/addon/**/*.*": "@libs/codemirror/addon/"
    }
}