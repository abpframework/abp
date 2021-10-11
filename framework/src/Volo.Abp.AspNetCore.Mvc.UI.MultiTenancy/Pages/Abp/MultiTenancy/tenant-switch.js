(function($) {

    var tenantSwitchModal = new abp.ModalManager(abp.appPath + 'Abp/MultiTenancy/TenantSwitchModal');

    $(function() {
        $('#AbpTenantSwitchLink').click(function(e) {
            e.preventDefault();
            tenantSwitchModal.open();
        });

        tenantSwitchModal.onResult(function() {
            location.assign(location.href);
        });
    });

})(jQuery);