var abp = abp || {};
$(function () {
    abp.modals.projectCreate = function () {
        var initModal = function (publicApi, args) {
            var $form = publicApi.getForm();
        };

        return {
            initModal: initModal,
        };
    };
});
