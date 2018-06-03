module.exports = {
    imports: [
        "../bootstrap/abp.resourcemapping.js",
        "../font-awesome/abp.resourcemapping.js",
        "../jquery-form/abp.resourcemapping.js",
        "../jquery-validation-unobtrusive/abp.resourcemapping.js",
        "../sweetalert/abp.resourcemapping.js",
        "../datatables.net-bs4/abp.resourcemapping.js"
    ],
    mappings: {
        //Toastr
        "@node_modules/toastr/build/*.*": "@libs/toastr/"
    }
}