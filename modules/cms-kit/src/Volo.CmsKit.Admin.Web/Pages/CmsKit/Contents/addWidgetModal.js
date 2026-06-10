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
                
                if(activeEditor){
                    activeEditor.attr('hidden', 'true');
                }
                
                activeEditor = $('#editor-' + widgetName);
                activeEditor.removeAttr('hidden');

                activeForm = $('#editor-' + widgetName + ' form');
            });

            $(".save-changes").click(function () {

                if (activeForm && activeForm.length > 0 && !activeForm.valid()) {
                    return;
                }

                let properties = activeForm ? activeForm.serializeFormToObject() : {};          

                let widgetText = "[Widget Type=\"" + widgetType + "\" ";

                for (var propertyName in properties) {
                    if (!propertyName.includes(']') && !propertyName.includes('[')) {
                        var propertyValue = properties[propertyName];
                        
                        //skip default/empty values
                        if (propertyValue === null || propertyValue === undefined || propertyValue === '') {
                            continue;
                        }

                        widgetText += propertyName + "=\"" + propertyValue + "\" ";
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