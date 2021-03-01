$(function () {
    var l = abp.localization.getResource("CmsKit");

    var $formCreate = $('#form-page-create');
    var $title = $('#ViewModel_Title');
    var $slug = $('#ViewModel_Slug');
    var $buttonSubmit = $('#button-page-create');
    var $pageContentInput = $('#ViewModel_Value');

    $formCreate.on('submit', function (e) {
        e.preventDefault();

        if ($formCreate.valid()) {

            abp.ui.setBusy();

            $formCreate.ajaxSubmit({
                success: function (result) {
                    submitEntityContent(result.id);
                }
            });
        }
    });

    $buttonSubmit.click(function (e) {
        e.preventDefault();
        $formCreate.submit();
    });

    function submitEntityContent(pageId) {
        volo.cmsKit.admin.contents.contentAdmin
            .create(
                {
                    entityType: 'Page',
                    entityId: pageId,
                    value: $pageContentInput.val()
                })
            .then(function (result) {
                finishSaving();
            });
    }

    function finishSaving() {
        abp.notify.success(l('SuccessfullySaved'));
        abp.ui.clearBusy();
        location.href = "/CmsKit/Pages/";
    }

    var slugEdited = false;

    $title.on('change paste keyup', function () {
        var title = $title.val();

        if (slugEdited) {
            title = $slug.val();
        }

        var slugified = slugify(title, {
            lower: true
        });

        if (slugified != $slug.val()) {
            reflectedChange = true;
            $slug.val(slugified);
            reflectedChange = false;
        }
    });

    $slug.change(function () {
        slugEdited = true;
    });
});