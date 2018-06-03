module.exports = {
    imports: [
        "../jquery/abp.resourcemapping.js"
    ],
    mappings: {
        //jQuery-Validation
        "@node_modules/jquery-validation/dist/jquery.validate.js": "@libs/jquery-validation/",
        "@node_modules/jquery-validation/dist/localization/*.*": "@libs/jquery-validation/localization/",

        //jQuery-Validation-Unobtrusive
        "@node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js": "@libs/jquery-validation-unobtrusive/"
    }
}