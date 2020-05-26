(function($) {

    var tenantSwitchModal = new abp.ModalManager();

    $(function() {
        $('#AbpTenantSwitchLink').click(function(e) {
            e.preventDefault();
            tenantSwitchModal.setOptions({
                viewUrl : abp.appPath + 'Abp/MultiTenancy/TenantSwitchModal'
            })
            tenantSwitchModal.open();
        });

        tenantSwitchModal.onResult(function() {
            location.reload();
        });
    });

})(jQuery);
