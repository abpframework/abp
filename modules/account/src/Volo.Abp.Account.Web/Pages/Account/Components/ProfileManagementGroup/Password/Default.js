(function ($) {
    $(".password-visibility-button").click(function (e) {
        let button = $(this);
        let passwordInput = button.parent().find("input");
        if (!passwordInput) {
            return;
        }

        if (passwordInput.attr("type") === "password") {
            passwordInput.attr("type", "text");
        }
        else {
            passwordInput.attr("type", "password");
        }

        let icon = button.find("i");
        if (icon) {
            icon.toggleClass("fa-eye-slash").toggleClass("fa-eye");
        }
    });

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

            if (input.currentPassword && input.currentPassword == '') {
                return;
            }

            if (input.currentPassword == input.newPassword) {
                abp.message.error(l('NewPasswordSameAsOld'));
                return;
            }

            volo.abp.account.profile.changePassword(input).then(function (result) {
                abp.message.success(l('PasswordChanged'));
                abp.event.trigger('passwordChanged');
                $('#ChangePasswordForm').trigger("reset");
            });
        });
    });
})(jQuery);
