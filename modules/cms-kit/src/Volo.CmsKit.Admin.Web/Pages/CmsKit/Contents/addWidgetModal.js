var abp = abp || {};
$(function () {
    abp.modals.addWidgetModal = function () {

        var initModal = function () {

            $('head').append('<script type="text/javascript" src="/Pages/CmsKit/Polls/Polls.js"></script>');
            $('head').append('<script type="text/javascript" src="/client-proxies/cms-kit-pro-proxy.js"></script>');
            $('head').append('<script type="text/javascript" src="/client-proxies/cms-kit-pro-admin-proxy.js"></script>');

            let widgetType;
            $("#ViewModel_Widget").change(function () {
                widgetType = this.value;

                $("#PropertySideId").html('');
                volo.cmsKit.admin.contents.contentAdmin.getWidgets().then(function (data) {
                    var widgetTypes = data.items.filter(v => v.key === widgetType);
                    var firstWidgetType = widgetTypes[0];
                    if (firstWidgetType.key == "Poll") {
                        $("#polls").removeAttr("hidden");
                    }
                    else {
                        for (const property of firstWidgetType.properties) {
                            let html = "<div class=\"form-group\"> " +
                                " <label for=\"" + property.key + "\">" + property.name + "</label>" +
                                " <input class=\"properties form-control\" id=\"" + property.key + "\" type=\"text\" />" +
                                " </div>";
                            $("#PropertySideId").append(html);
                        }
                    }
                });
            });

            $("#save-changes").click(function () {
                var widgetKey = $('#WidgetCodes option').attr("data-key");
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
                    .replace('AW', '');

                let updatedText = '';
                if (widgetType != undefined) {

                    updatedText = "[Widget Type=\"" + widgetType + "\" ";

                    for (var i = 0; i < keys.length; i++) {
                        updatedText += keys[i] + "=\"" + values[i] + "\" ";
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