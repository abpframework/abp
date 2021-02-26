$(function () {

    $entityContentEditors = $('.form-entity-content-editor');

    var contentAdminService = volo.cmsKit.admin.contents.contentAdmin;

    $entityContentEditors.on('submit', function (e, obj) {
        e.preventDefault();

        var $form = $(e.currentTarget);

        if ($form.valid()) {

            var data = form.serializeFormToObject().viewModel;

            abp.ui.setBusy(e);
            if (data.id) {
                contentAdminService
                    .update(data.id, data)
                    .then(function (result) {
                        abp.ui.clearBusy();
                    });

            }
            else {
                contentAdminService
                    .create(data)
                    .then(function (result) {
                        abp.ui.clearBusy();
                    });
            }
        }
    })
})