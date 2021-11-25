var abp = abp || {};
$(function () {
    abp.modals.blogEdit = function () {
        var initModal = function (publicApi, args) {
            var $form = publicApi.getForm();
        };

        return {
            initModal: initModal,
        };
    };
});
