(function($) {

    var tenantSwitchModal = new abp.ModalManager(abp.appPath + 'Abp/MultiTenancy/TenantSwitchModal');

    $(function() {
        $('#TenantSwitchToolbarLink').click(function(e) {
            e.preventDefault();
            tenantSwitchModal.open();
        });

        tenantSwitchModal.onResult(function() {
            location.reload();
        });
    });

})(jQuery);