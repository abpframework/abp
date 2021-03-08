﻿$(function () {
    var l = abp.localization.getResource("CmsKit");

    var $createForm = $('#form-page-create');
    var $title = $('#ViewModel_Title');
    var $slug = $('#ViewModel_Slug');
    var $buttonSubmit = $('#button-page-create');

    $createForm.data('validator').settings.ignore = ":hidden, [contenteditable='true']:not([name]), .tui-popup-wrapper";
    
    $createForm.on('submit', function (e) {
        e.preventDefault();

        if ($createForm.valid()) {

            abp.ui.setBusy();

            $createForm.ajaxSubmit({
                success: function (result) {
                    abp.notify.success(l('SuccessfullySaved'));
                    abp.ui.clearBusy();
                    location.href = "../Pages";
                }
            });
        }
    });

    $buttonSubmit.click(function (e) {
        e.preventDefault();
        $createForm.submit();
    });

    var slugEdited = false;

    $title.on('change paste keyup', function () {
        var title = $title.val();

        if (slugEdited) {
            title = $slug.val();
        }

        var slugified = slugify(title, {
            lower: true
        });

        if (slugified != $slug.val()) {
            reflectedChange = true;
            $slug.val(slugified);
            reflectedChange = false;
        }
    });

    $slug.change(function () {
        slugEdited = true;
    });
    
    // -----------------------------------
    function getUppyHeaders() {
        var headers = {};
        headers[abp.security.antiForgery.tokenHeaderName] = abp.security.antiForgery.getToken();

        return headers;
    }
    
    var fileUploadUri = "/api/cms-kit-admin/media/page";
    var fileUriPrefix = "/api/cms-kit/media/";

    var editorDataKey = "tuiEditor";

    initAllEditors();

    function initAllEditors() {
        $('.content-editor').each(function (i, item) {
            initEditor(item);
        });
    }

    function initEditor(element) {
        var $editorContainer = $(element);
        var inputName = $editorContainer.data('input-id');
        var $editorInput = $('#' + inputName);
        var initialValue = $editorInput.val();

        var editor = $editorContainer.tuiEditor({
            usageStatistics: false,
            useCommandShortcut: true,
            initialValue: initialValue,
            previewStyle: 'tab',
            height: "25em",
            minHeight: "25em",
            initialEditType: initialValue ? 'wysiwyg' : 'markdown',
            language: $editorContainer.data("language"),
            hooks: {
                addImageBlobHook: uploadFile,
            },
            events: {
                change: function (_val) {
                    $editorInput.val(editor.getHtml());
                    $editorInput.trigger("change");
                }
            }
        }).data(editorDataKey);
    }

    function uploadFile(blob, callback, source) {
        var UPPY_OPTIONS = {
            endpoint: fileUploadUri,
            formData: true,
            fieldName: "file",
            method: "post",
            headers: getUppyHeaders()
        };

        var UPPY = Uppy.Core().use(Uppy.XHRUpload, UPPY_OPTIONS);

        UPPY.reset();

        UPPY.addFile({
            id: "content-file",
            name: blob.name,
            type: blob.type,
            data: blob,
        });

        UPPY.upload().then((result) => {
            if (result.failed.length > 0) {
                abp.message.error("File upload failed");
            } else {
                var mediaDto = result.successful[0].response.body;
                var fileUrl = (fileUriPrefix + mediaDto.id);

                callback(fileUrl, mediaDto.name);
            }
        });
    }
});