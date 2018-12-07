module.exports = {
    aliases: {
        "@node_modules": "./node_modules",
        "@libs": "./wwwroot/libs"
    },
    clean: [
        "@libs"
    ],
    mappings: {
        "@node_modules/prismjs/plugins/toolbar/prism-toolbar.css": "@libs/prismjs/",
        "@node_modules/prismjs/plugins/line-highlight/prism-line-highlight.css": "@libs/prismjs/",
        "@node_modules/prismjs/themes/prism-okaidia.css": "@libs/prismjs/",
        "@node_modules/prismjs/components/prism-csharp.js": "@libs/prismjs/",
        "@node_modules/prismjs/plugins/line-highlight/prism-line-highlight.js": "@libs/prismjs/",
        "@node_modules/prismjs/plugins/copy-to-clipboard/prism-copy-to-clipboard.js": "@libs/prismjs/",
        "@node_modules/prismjs/plugins/show-language/prism-show-language.js": "@libs/prismjs/",
        "@node_modules/prismjs/prism.js": "@libs/prismjs/",
        "@node_modules/prismjs/plugins/toolbar/prism-toolbar.js": "@libs/prismjs/",
        "@node_modules/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.concat.min.js": "@libs/mCustomScrollbar/",
        "@node_modules/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.css": "@libs/mCustomScrollbar/",
        "@node_modules/anchor-js/anchor.js": "@libs/anchor-js/",
        "@node_modules/clipboard/dist/clipboard.js": "@libs/clipboard/",
        "@node_modules/popper.js/dist/umd/popper.min.js": "@libs/popper.js/"
    }
}
