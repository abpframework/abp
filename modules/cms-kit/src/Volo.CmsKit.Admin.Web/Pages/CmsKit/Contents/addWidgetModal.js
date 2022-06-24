var abp = abp || {};
$(function () {
    abp.modals.addWidgetModal = function () {

        var initModal = function () {
            $("#save-changes").click(function () {
                var updatedText = $("#textId").val();

                var contentEditorText = $("#ContentEditor")[0].innerText.split("\n")[2];

                $('.ProseMirror div').contents()[0].data = contentEditorText + updatedText;

                $('#addWidgetModal').modal('hide');
            });
        };

        return {
            initModal: initModal
        };
    };
});