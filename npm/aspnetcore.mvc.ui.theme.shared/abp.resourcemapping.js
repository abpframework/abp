module.exports = {
    imports: [
        "../jquery/abp.resourcemapping.js"
    ],
    mappings: {
        //Bootstrap
        "@node_modules/bootstrap/dist/css/bootstrap.css": "@libs/bootstrap/css/",
        "@node_modules/bootstrap/dist/js/bootstrap.bundle.js": "@libs/bootstrap/js/",

        //Font-Awesome
        "@node_modules/font-awesome/css/font-awesome.css": "@libs/font-awesome/css/",
        "@node_modules/font-awesome/fonts/*.*": "@libs/font-awesome/fonts/",

        //jQuery-Form
        "@node_modules/jquery-form/dist/jquery.form.min.js": "@libs/jquery-form/",

        //jQuery-Validation
        "@node_modules/jquery-validation/dist/jquery.validate.js": "@libs/jquery-validation/dist/", //TODO: do not use dist on destination
        "@node_modules/jquery-validation/dist/localization/*.*": "@libs/jquery-validation/dist/localization/", //TODO: do not use dist on destination

        //jQuery-Validation-Unobtrusive
        "@node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js": "@libs/jquery-validation-unobtrusive/",

        //Datatables
        "@node_modules/datatables.net/js/jquery.dataTables.js": "@libs/datatables.net/js/",
        "@node_modules/datatables.net-bs4/css/dataTables.bootstrap4.css": "@libs/datatables.net-bs4/css/",
        "@node_modules/datatables.net-bs4/js/dataTables.bootstrap4.js": "@libs/datatables.net-bs4/js/"
    }
}