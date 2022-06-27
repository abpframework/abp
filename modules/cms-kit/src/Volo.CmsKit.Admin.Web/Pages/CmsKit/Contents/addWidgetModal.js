var abp = abp || {};
$(function () {
    abp.modals.addWidgetModal = function () {

        var initModal = function () {

            $('#PropertySideId').hide();

            $("#ViewModel_Widget").change(function () {
                $('#PropertySideId').show();
            });

            $("#save-changes").click(function () {
                var keys = [];
                var values = [];
                $(".properties").each(function () {
                    if (($.trim($(this).val()).length > 0)) {
                        keys.push(this.id);
                        values.push($(this).val());
                    }
                });

                var contentEditorText = $("#ContentEditor")[0].innerText.split("\n")[2];

                var updatedText = "[Widget ";

                for (var i = 0; i < keys.length; i++) {
                    updatedText += keys[i] + "=\"" + values[i] + "\" ";
                }

                updatedText += "]";
                $('.ProseMirror div').contents()[0].data = contentEditorText + updatedText;

                $('#addWidgetModal').modal('hide');
            });
        };

        return {
            initModal: initModal
        };
    };
});