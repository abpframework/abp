$(function () {

    var l = abp.localization.getResource("CmsKit");

    var $selectBlog = $('#BlogSelectionSelect');
    var $formUpdate = $('#form-blog-post-update');
    var $coverImage = $('#ViewModel_CoverImageMediaId');
    var $slug = $('#ViewModel_Slug');
    var $buttonSubmit = $('#button-blog-post-update');
    var $blogPostIdInput = $('#Id');
    var $tagsInput = $('.tag-editor-form input[name=tags]');
    var $fileInput = $('#BlogPostCoverImage');

    var UPPY_FILE_ID = "uppy-upload-file";

    var isTagsEnabled = true;

    $formUpdate.data('validator').settings.ignore = ":hidden, [contenteditable='true']:not([name]), .tui-popup-wrapper";

    function initSelectBlog() {
        $selectBlog.data('autocompleteApiUrl', '/api/cms-kit-admin/blogs/blogs');
        $selectBlog.data('autocompleteDisplayProperty', 'name');
        $selectBlog.data('autocompleteValueProperty', 'id');
        $selectBlog.data('autocompleteItemsProperty', 'items');
        $selectBlog.data('autocompleteFilterParamName', 'filter');

        abp.dom.initializers.initializeAutocompleteSelects($selectBlog);
    }

    initSelectBlog();

    $formUpdate.on('submit', function (e) {
        e.preventDefault();

        if ($formUpdate.valid()) {

            abp.ui.setBusy();

            $formUpdate.ajaxSubmit({
                success: function (result) {
                    if (isTagsEnabled) {
                        submitEntityTags($blogPostIdInput.val());
                    }
                    else {
                        finishSaving(result);
                    }
                },
                error: function (result) {
                    abp.ui.clearBusy();
                    abp.notify.error(result.responseJSON.error.message);
                }
            });
        }
        else {
            abp.ui.clearBusy();
        }
    });

    $buttonSubmit.click(function (e) {
        e.preventDefault();
        submitCoverImage();
    });

    function submitEntityTags(blogPostId) {

        var tags = $tagsInput.val().split(',').map(x => x.trim()).filter(x => x);

        if (tags.length === 0) {
            finishSaving();
            return;
        }

        volo.cmsKit.admin.tags.entityTagAdmin
            .setEntityTags({
                entityType: 'BlogPost',
                entityId: blogPostId,
                tags: tags
            })
            .then(function (result) {
                finishSaving(result);
            });
    }

    function getUppyHeaders() {
        var headers = {};
        headers[abp.security.antiForgery.tokenHeaderName] = abp.security.antiForgery.getToken();

        return headers;
    }

    function submitCoverImage() {
        abp.ui.setBusy();

        var UPPY_OPTIONS = {
            endpoint: fileUploadUri,
            formData: true,
            fieldName: "file",
            method: "post",
            headers: getUppyHeaders()
        };

        var UPPY = Uppy.Core().use(Uppy.XHRUpload, UPPY_OPTIONS);

        UPPY.reset();

        var file = $fileInput[0].files[0];

        if (file) {

            UPPY.addFile({
                id: UPPY_FILE_ID,
                name: file.name, // file name
                type: file.type, // file type
                data: file, // file
            });

            UPPY.upload().then((result) => {
                if (result.failed.length > 0) {
                    abp.message.error(l("UploadFailedMessage"));
                } else {
                    $coverImage.val(result.successful[0].response.body.id);

                    $formUpdate.submit();
                }
            });
        }
        else {
            $formUpdate.submit();
        }
    }

    function finishSaving(result) {
        abp.notify.success(l('SuccessfullySaved'));
        abp.ui.clearBusy();
        location.href = "../../BlogPosts";
    }

    $slug.on('change', function () {
        reflectUrlChanges();
    });

    function reflectUrlChanges() {

        var title = $slug.val();

        var slugified = slugify(title);

        if (slugified != title) {
            $slug.val(slugified, {
                lower: true
            });
        }
    }


    // -----------------------------------
    var fileUploadUri = "/api/cms-kit-admin/media/blogpost";
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
            height: "95vh",
            minHeight: "25em",
            initialEditType: 'markdown',
            language: $editorContainer.data("language"),
            hooks: {
                addImageBlobHook: uploadFile,
            },
            events: {
                change: function (_val) {
                    $editorInput.val(editor.getMarkdown());
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