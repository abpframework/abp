﻿(function ($) {
    $(function () {
        var l = abp.localization.getResource("AbpAccount");

        $('#ChangePasswordForm').submit(function (e) {
            e.preventDefault();

            if (!$('#ChangePasswordForm').valid()) {
                return false;
            }

            var input = $('#ChangePasswordForm').serializeFormToObject();

            if (
                input.newPassword != input.newPasswordConfirm ||
                input.newPassword == ''
            ) {
                abp.message.error(l('NewPasswordConfirmFailed'));
                return;
            }

            if (input.currentPassword && input.currentPassword == ''){
                return;
            }

            volo.abp.identity.profile.changePassword(input).then(function (result) {
                abp.message.success(l('PasswordChanged'));
            });
        });
    });
})(jQuery);
