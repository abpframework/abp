(function ($) {

    var l = abp.localization.getResource('AbpAccount');

    var _profileService = volo.abp.identity.profile;

    $("#ChangePasswordForm").submit(function (e) {
        e.preventDefault();

        if (!$("#ChangePasswordForm").valid()) {
            return false;
        }

        var input = $("#ChangePasswordForm").serializeFormToObject().changePasswordInfoModel;

        if (input.newPassword != input.newPasswordConfirm || input.currentPassword == '') {
            abp.message.error(l("NewPasswordConfirmFailed"));
            return;
        }

        if (input.currentPassword == '') {
            return;
        }

        _profileService.changePassword(
            input
        ).then(function (result) {
            abp.message.success(l("PasswordChanged"));
        });

    });

    $("#PersonalSettingsForm").submit(function (e) {
        e.preventDefault();

        if (!$("#PersonalSettingsForm").valid()) {
            return false;
        }

        var input = $("#PersonalSettingsForm").serializeFormToObject().personalSettingsInfoModel;

        _profileService.update(
            input
        ).then(function (result) {
            abp.notify.success(l("PersonalSettingsSaved"));
        });

    });

})(jQuery);