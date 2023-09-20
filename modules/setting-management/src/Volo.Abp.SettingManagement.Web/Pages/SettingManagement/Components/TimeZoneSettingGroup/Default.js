(function ($) {
    $(function () {

        var l = abp.localization.getResource('AbpSettingManagement');

        $("#TimeZoneSettingsForm").on('submit', function (event) {
            event.preventDefault();

            volo.abp.settingManagement.timeZoneSettings.update($("#Timezone").val()).then(function (result) {
                $(document).trigger("AbpSettingSaved");
            });

        });
    });
})(jQuery);
