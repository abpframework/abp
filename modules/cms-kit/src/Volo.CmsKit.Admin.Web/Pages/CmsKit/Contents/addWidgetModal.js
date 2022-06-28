var abp = abp || {};
$(function () {
    abp.modals.addWidgetModal = function () {

        var initModal = function () {

            $('#PropertySideIdPoll').hide();
            $('#PropertySideIdComment').hide();

            //var getRelatedProperties = function () {
            //    var html = "";

            //    for (var i = 0; i < 2; i++) {
            //        html += "<div class=\"form-group\"> " +
            //            " <label for=\"" + i + "\">" + i + "</label>" +
            //            " <input class=\"properties form-control\" id=\"" + i + " type=\"text\" />" +
            //            " </div>";
            //    }

            //    $("#ye").each(function () {
            //        debugger
            //        alert("a");
            //    });

            //    return html;
            //}

            let widgetType;

            $("#ViewModel_Widget").change(function () {
                widgetType = this.value;
                debugger
                if (widgetType == "Poll") {
                    $('#PropertySideIdPoll').show();
                    $('#PropertySideIdComment').hide();
                }
                else {
                    $('#PropertySideIdPoll').hide();
                    $('#PropertySideIdComment').show();
                }

                //var newHtml = getRelatedProperties();

                //$("#PropertySideId").append(newHtml);

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

                var contentEditorText = $("#ContentEditor")[0].innerText.split("\n")[1];

                var updatedText = "[Widget Type=\"" + widgetType + "\" ";

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