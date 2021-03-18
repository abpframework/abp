(function ($) {

    $(function () {

        var l = abp.localization.getResource('AbpSettingManagement');

        $("#EmailSettingsForm").on('submit', function (event) {
            event.preventDefault();
            var form = $(this).serializeFormToObject();

            volo.abp.settingManagement.emailSettings.update(form).then(function (result) {
                $(document).trigger("AbpSettingSaved");
            });

        });

        $('#SmtpUseDefaultCredentials').change(function () {
            if (this.checked) {
                $('#HideSectionWhenUseDefaultCredentialsIsChecked').slideUp();
            }
            else {
                $('#HideSectionWhenUseDefaultCredentialsIsChecked').slideDown();
            }
        });
    });

})(jQuery);
