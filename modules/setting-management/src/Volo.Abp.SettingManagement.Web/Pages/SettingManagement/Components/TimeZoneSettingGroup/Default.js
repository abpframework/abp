(function ($) {
    $(function () {

        var l = abp.localization.getResource('AbpSettingManagement');

        var select2 = null;
        volo.abp.settingManagement.timeZoneSettings.getTimezones().then(function (result) {
            var data = $.map(result, function (obj) {
                obj.id = obj.value;
                obj.text = obj.name;
                return obj;
            });

            select2 = $("#timezone-select").select2({
                data: data
            });

            volo.abp.settingManagement.timeZoneSettings.get().then(function (result) {
                select2.val(result).trigger("change");
            });
        });

        $("#TimeZoneSettingsForm").on('submit', function (event) {
            event.preventDefault();
            volo.abp.settingManagement.timeZoneSettings.update($("#timezone-select").val()).then(function (result) {
                $(document).trigger("AbpSettingSaved");
            });
        });
    });
})(jQuery);
