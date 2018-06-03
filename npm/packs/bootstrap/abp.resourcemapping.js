module.exports = {
    imports: [
        "../core/abp.resourcemapping.js"
    ],
    mappings: {
		//Bootstrap
        "@node_modules/bootstrap/dist/css/bootstrap.css": "@libs/bootstrap/css/",
        "@node_modules/bootstrap/dist/js/bootstrap.bundle.js": "@libs/bootstrap/js/"
    }
}