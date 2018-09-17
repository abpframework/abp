(function ($) {

    var l = abp.localization.getResource('AbpIdentity');
    var _changePasswordModal = new abp.ModalManager(abp.appPath + 'Identity/Shared/ChangePasswordModal');
  
    $(function () {

        $("#abp-account-change-password").click(function (e) {
            e.preventDefault();
            _changePasswordModal.open();
        });

        _changePasswordModal.onResult(function () {
            abp.message.success(l("PasswordChangedMessage"));
        });
    });

})(jQuery);
