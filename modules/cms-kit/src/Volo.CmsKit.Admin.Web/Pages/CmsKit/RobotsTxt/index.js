$(function () {
    var l = abp.localization.getResource("CmsKit");

    var service = volo.cmsKit.admin.robotsTxts.robotsTxtAdmin;

    var editor = CodeMirror.fromTextArea(document.getElementById("Content"), {
        lineNumbers: true
    });

    $('#SaveRobotsTxtButton').on('click', function () {
        service.update({
            content: editor.getValue()
        }).then(function () {
            abp.notify.success(l("SavedSuccessfully"));
            location.reload();
        });
    });

    $('#ResetRobotsTxtButton').on('click', function () {
        abp.message.confirm(l("RobotsTxtResetConfirmationMessage"))
            .then(function (confirmed) {
                if (!confirmed) {
                    return;
                }

                service.reset().then(function () {
                    abp.notify.success(l("SavedSuccessfully"));
                    location.reload();
                });
            });
    });
});
