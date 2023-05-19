var abp = abp || {};
$(function () {
    abp.modals.addWidgetModal = function () {

        var initModal = function () {
            var activeEditor;
            var activeForm;

            let widgetName, widgetType;
            $("#ViewModel_Widget").change(function () {
                widgetName = $("#ViewModel_Widget").val();
                widgetType = $("#ViewModel_Widget").find(":selected").text();

                $('.widget-detail').attr('hidden', 'true');

                activeEditor = $('#editor-' + widgetName);
                activeEditor.removeAttr('hidden');

                activeForm = $('#editor-' + widgetName + ' form');
            });

            $(".save-changes").click(function () {

                let properties = activeForm.serializeFormToObject();          

                let widgetText = "[Widget Type=\"" + widgetType + "\" ";

                for (var propertyName in properties) {
                    if (!propertyName.includes(']') && !propertyName.includes('[')) {
                        widgetText += propertyName + "=\"" + properties[propertyName] + "\" ";
                    }
                }

                widgetText = widgetText.trim() + "]";

                $('#GeneratedWidgetText').val(widgetText);
                $("#GeneratedWidgetText").trigger("change");
                $('#addWidgetModal').modal('hide');
            });
        };

        return {
            initModal: initModal
        };
    };
});