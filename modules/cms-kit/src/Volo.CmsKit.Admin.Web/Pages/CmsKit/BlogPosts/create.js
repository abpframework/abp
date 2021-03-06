$(function () {

    var l = abp.localization.getResource("CmsKit");

    var $selectBlog = $('#BlogSelectionSelect');
    var $formCreate = $('#form-blog-post-create');
    var $title = $('#ViewModel_Title');
    var $shortDescription = $('#ViewModel_ShortDescription');
    var $url = $('#ViewModel_Slug');
    var $buttonSubmit = $('#button-blog-post-create');
    var $pageContentInput = $('#ViewModel_Content');
    var $tagsInput = $('.tag-editor-form input[name=tags]');
    var $fileInput = $('#BlogPostCoverImage');
    var $tagsWrapper = $('#blog-post-tags-wrapper');

    var UPPY_UPLOAD_ENDPOINT = "/api/cms-kit-admin/blogs/blog-posts/{0}/cover-image";
    var UPPY_FILE_ID = "uppy-upload-file";

    var isTagsEnabled = true;

    $formCreate.data('validator').settings.ignore = ":hidden, [contenteditable='true']:not([name]), .tui-popup-wrapper";

    function initSelectBlog() {
        $selectBlog.data('autocompleteApiUrl', '/api/cms-kit-admin/blogs');
        $selectBlog.data('autocompleteDisplayProperty', 'name');
        $selectBlog.data('autocompleteValueProperty', 'id');
        $selectBlog.data('autocompleteItemsProperty', 'items');
        $selectBlog.data('autocompleteFilterParamName', 'filter');

        abp.dom.initializers.initializeAutocompleteSelects($selectBlog);
    }

    initSelectBlog();

    $formCreate.on('submit', function (e) {
        e.preventDefault();

        if ($formCreate.valid()) {

            abp.ui.setBusy();

            $formCreate.ajaxSubmit({
                success: function (result) {
                    if (isTagsEnabled) {
                        submitEntityTags(result.id);
                    }
                    else {
                        submitCoverImage(result.id);
                    }
                },
                error: function (result) {
                    abp.notify.error(result.responseJSON.error.message);
                    abp.ui.clearBusy();
                }
            });
        }
    });

    $buttonSubmit.click(function (e) {
        e.preventDefault();
        $formCreate.submit();
    });
    
    function submitEntityTags(blogPostId) {

        var tags = $tagsInput.val().split(',').map(x => x.trim()).filter(x => x);
                
        if(tags.length === 0){
            submitCoverImage(blogPostId);
            return;
        }
        
        volo.cmsKit.admin.tags.entityTagAdmin
            .setEntityTags({
                entityType: 'BlogPost',
                entityId: blogPostId,
                tags: tags
            })
            .then(function (result) {
                submitCoverImage(blogPostId);
            });
    }

    function getUppyHeaders() {
        var headers = {};
        headers[abp.security.antiForgery.tokenHeaderName] = abp.security.antiForgery.getToken();

        return headers;
    }

    function submitCoverImage(blogPostId) {
        var UPPY_OPTIONS = {
            endpoint: UPPY_UPLOAD_ENDPOINT.replace("{0}", blogPostId),
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
                    finishSaving();
                }
            });
        }
        else {
            finishSaving();
        }
    }

    function finishSaving() {
        abp.notify.success(l('SuccessfullySaved'));
        abp.ui.clearBusy();
        location.href = "../BlogPosts";
    }

    var urlEdited = false;
    var reflectedChange = false;

    $url.on('change', function () {
        reflectUrlChanges();
    });

    $title.on('keyup paste', function () {
        reflectUrlChanges();
    });

    function reflectUrlChanges() {
        var title = $title.val().toLocaleLowerCase();

        if (urlEdited) {
            title = $url.val();
        }

        var slugified = slugify(title, {
            lower: true
        });

        if (slugified != $url.val()) {
            reflectedChange = true;

            $url.val(slugified);

            reflectedChange = false;
        }
    }

    $url.change(function () {
        if (!reflectedChange) {
            urlEdited = true;
        }
    });

    var shorDescriptionEdited = false;
    $pageContentInput.on('change', function () {
        if (shorDescriptionEdited) {
            return;
        }

        var htmlValue = $pageContentInput.val();

        var plainValue = jQuery('<div>').html(htmlValue).text().replace(/\n/g, ' ').substring(0, 120);

        $shortDescription.val(plainValue);
    });

    $shortDescription.on('change', function () {
        shorDescriptionEdited = true;
    });

    $selectBlog.on('change', function () {
        var blogId = $selectBlog.val();
        volo.cmsKit.blogs.blogFeature
            .getOrDefault(blogId, 'CmsKit.Tags')
            .then(function (result) {
                if (result) {
                    isTagsEnabled = result.isEnabled == true
                    if (isTagsEnabled === true) {
                        $tagsWrapper.removeClass('d-none');
                    }
                    else {
                        $tagsWrapper.addClass('d-none');
                    }
                }
            });
    });
    
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