var abp = abp || {};
$(function () {
    abp.modals.addWidgetModal = function () {

        var initModal = function () {
            $("#save-changes").click(function () {
                alert("works");
            });
        };

        return {
            initModal: initModal
        };
    };
});