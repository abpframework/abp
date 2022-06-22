var abp = abp || {};
$(function () {
    alert("hello guys1");
    abp.modals.addWidgetModal = function () {

        var initModal = function () {
            alert("hello guys");
        };

        return {
            initModal: initModal
        };
    };
});