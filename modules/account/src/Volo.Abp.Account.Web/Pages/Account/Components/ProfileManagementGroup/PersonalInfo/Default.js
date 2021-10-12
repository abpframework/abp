(function ($) {
    $(function () {
        var l = abp.localization.getResource("AbpAccount");

        $('#PersonalSettingsForm').submit(function (e) {
            e.preventDefault();

            if (!$('#PersonalSettingsForm').valid()) {
                return false;
            }

            var input = $('#PersonalSettingsForm').serializeFormToObject();

            volo.abp.identity.profile.update(input).then(function (result) {
                abp.notify.success(l('PersonalSettingsSaved'));
            });
        });
    });
})(jQuery);
