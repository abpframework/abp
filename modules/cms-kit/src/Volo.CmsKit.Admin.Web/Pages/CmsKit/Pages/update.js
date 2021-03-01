$(function () {

    var l = abp.localization.getResource("CmsKit");

    var $formUpdate = $('#form-page-update');
    var $buttonSubmit = $('#button-page-update');
    var $pageContentInput = $('#ViewModel_Value');
    var $pageIdInput = $('#Id');
    var $contentIdInput = $('#ViewModel_Id');

    $formUpdate.on('submit', function (e) {
        e.preventDefault();

        if ($formUpdate.valid()) {

            abp.ui.setBusy();

            $formUpdate.ajaxSubmit({
                success: function (result) {
                    submitEntityContent();
                }
            });
        }
    });

    $buttonSubmit.click(function (e) {
        e.preventDefault();
        $formUpdate.submit();
    });

    function submitEntityContent() {

        var contentId = $contentIdInput.val();
        var pageId = $pageIdInput.val();
        var contentValue = $pageContentInput.val();

        if (contentId) {
            volo.cmsKit.admin.contents.contentAdmin
                .update(contentId,
                    {
                        value: contentValue
                    })
                .then(function (result) {
                    finishSaving(result);
                });
        }
        else {
            volo.cmsKit.admin.contents.contentAdmin
                .create({
                    entityType: 'Page',
                    entityId: pageId,
                    value: contentValue
                })
                .then(function (result) {
                    finishSaving(result);
                });
        }
    }

    function finishSaving(result) {
        abp.notify.success(l('SuccessfullySaved'));
        abp.ui.clearBusy();
        location.href = "/CmsKit/Pages/";
    }
});