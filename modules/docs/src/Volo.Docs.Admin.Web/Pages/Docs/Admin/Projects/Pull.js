var abp = abp || {};

$(function () {
    abp.modals.projectPull = function () {
        var initModal = function (publicApi, args) {
            var $form = publicApi.getForm();
            var fg = $form.find('#PullDocument_Name').parent();
            var nameInput = fg.html();

            $form.find('input:checkbox').change(function () {
                if ($(this).prop('checked')) {
                    fg.html('');
                } else {
                    fg.html(nameInput);
                }
            });
        };

        return {
            initModal: initModal,
        };
    };
});
