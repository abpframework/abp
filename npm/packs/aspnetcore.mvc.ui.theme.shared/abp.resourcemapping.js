module.exports = {
    imports: [
        "../bootstrap/abp.resourcemapping.js",
        "../font-awesome/abp.resourcemapping.js",
        "../jquery-form/abp.resourcemapping.js",
        "../jquery-validation-unobtrusive/abp.resourcemapping.js",
        "../datatables.net-bs4/abp.resourcemapping.js"
    ],
    mappings: {
        //Sweetalert
        "@node_modules/sweetalert/dist/*.*": "@libs/sweetalert/",
        
        //Toastr
        "@node_modules/toastr/build/*.*": "@libs/toastr/"
    }
}