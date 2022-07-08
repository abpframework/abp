var abp = abp || {};
$(function () {
    abp.modals.addWidgetModal = function () {

        var initModal = function () {

            let widgetName, widgetType;
            $("#ViewModel_Widget").change(function () {
                widgetName = $("#ViewModel_Widget").val();
                widgetType = $("#ViewModel_Widget").find(":selected").text();

                $('.widget-detail').attr('hidden', 'true');

                $('#editor-' + widgetName).removeAttr('hidden');
            });

            $("#save-changes").click(function () {
                var widgetKey = $("#WidgetCode").val();
                if (widgetKey != undefined) {
                    let html = " <input hidden class=\"properties form-control\" value=\"" + widgetKey + "\" id=\"Code\" type=\"text\" />"
                    $("#PropertySideId").append(html);
                }

                var keys = [];
                var values = [];
                $(".properties").each(function () {
                    if (($.trim($(this).val()).length > 0)) {
                        keys.push(this.id);
                        values.push($(this).val());
                    }
                });

                var contentEditorText = $("#ContentEditor")[0].innerText
                    .replace('WritePreview', '')
                    .replace('MarkdownWYSIWYG', '')
                    .replace('W', '');

                let updatedText = '';
                if (widgetType != undefined) {

                    updatedText = "[Widget Type=\"" + widgetType + "\" ";

                    for (var i = 0; i < keys.length; i++) {
                        updatedText += keys[i] + "=\"" + values[i];
                        updatedText += i == (keys.length - 1) ? "\"" : "\" ";
                    }

                    updatedText += "]";
                }

                if (contentEditorText == '\n\n\n') {
                    //TODO fails event
                    var fixedData = "<div>" + updatedText + "</div>";
                    var innerHtml = $("#ContentEditor")[0].innerHTML;
                    var replacedInnerHtml = innerHtml.replace('<div><br></div>', fixedData);
                    $("#ContentEditor")[0].innerHTML = replacedInnerHtml;
                }
                else {
                    $('.ProseMirror div').contents()[0].data = contentEditorText + updatedText;
                }

                $('#addWidgetModal').modal('hide');
            });
        };

        return {
            initModal: initModal
        };
    };
});