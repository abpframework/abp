(function ($) {

    var l = abp.localization.getResource('AbpIdentity');
    var _personalSettingsModal = new abp.ModalManager(abp.appPath + 'Identity/Shared/PersonalSettingsModal');
  
    $(function () {

        $("#abp-account-personal-settings").click(function (e) {
            e.preventDefault();
            _personalSettingsModal.open();
        });

        _personalSettingsModal.onResult(function () {
            //abp.message.success(l("PersonalSettingsSavedMessage"));
        });
    });

})(jQuery);
