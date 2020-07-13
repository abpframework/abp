var abp = abp || {};
$(function () {
    abp.modals.projectEdit = function () {
        var initModal = function (publicApi, args) {
            var $form = publicApi.getForm();
        };

        return {
            initModal: initModal,
        };
    };
});
