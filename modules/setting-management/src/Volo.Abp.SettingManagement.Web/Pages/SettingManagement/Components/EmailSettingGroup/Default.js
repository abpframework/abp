(function ($) {

    var _sendTestEmailModal = new abp.ModalManager(
        abp.appPath + 'SettingManagement/Components/EmailSettingGroup/SendTestEmailModal'
    );

    $(function () {

        var l = abp.localization.getResource('AbpSettingManagement');

        $("#EmailSettingsForm").on('submit', function (event) {
            event.preventDefault();

            if (!$(this).valid()) {
                return;
            }

            var form = $(this).serializeFormToObject();
            volo.abp.settingManagement.emailSettings.update(form).then(function (result) {
                $(document).trigger("AbpSettingSaved");
            });

        });

        $('#SmtpUseDefaultCredentials').change(function () {
            if (this.checked) {
                $('#HideSectionWhenUseDefaultCredentialsIsChecked').slideUp();
            } else {
                $('#HideSectionWhenUseDefaultCredentialsIsChecked').slideDown();
            }
        });

        _sendTestEmailModal.onOpen(function () {
            var $form = _sendTestEmailModal.getForm();
            _sendTestEmailModal.getForm().off('abp-ajax-success');

            $form.on('abp-ajax-success', function () {
                _sendTestEmailModal.setResult();
            });
        })
        
        _sendTestEmailModal.onResult(function () {
            abp.notify.success(l('SuccessfullySent'));
        });

        $("#SendTestEmailButton").click(function (e) {
            e.preventDefault();
            _sendTestEmailModal.open();
        });
    });

})(jQuery);
