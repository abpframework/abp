(function ($) {

    abp.modals.TenantConnectionStringManagement = function() {

        var initModal = function (publicApi, args) {
            publicApi.getModal()
                .find('input[name="Tenant.UseSharedDatabase"]')
                .change(function () {
                    var $this = $(this);
                    $("#Tenant_DefaultConnectionString_Wrap").toggleClass("d-none");
                    $this.val($this.prop("checked"));
                });
        };

        return {
            initModal: initModal
        };
    };

})(jQuery);