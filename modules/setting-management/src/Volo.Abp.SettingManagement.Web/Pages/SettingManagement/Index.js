(function ($) {
    var l = abp.localization.getResource('AbpSettingManagement');

    $(document).on("AbpSettingSaved", function () {
        abp.notify.success(l("SuccessfullySaved"));
    });
})(jQuery);