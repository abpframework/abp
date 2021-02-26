$(function () {

    var l = abp.localization.getResource("CmsKit");

    var $selectBlog = $('#BlogSelectionSelect');
    var $formUpdate = $('#form-blog-post-update');
    var $title = $('#ViewModel_Title');
    var $titleClone = $('#title-clone');
    var $slug = $('#ViewModel_Slug');
    var $buttonSubmit = $('#button-blog-post-update');
    var $contentValueInput = $('#ViewModel_Value');
    var $blogPostIdInput = $('#Id');
    var $contentIdInput = $('#ViewModel_Id');
    var $tagsInput = $('.tag-editor-form input[name=tags]');
    var $fileInput = $('#BlogPostCoverImage');

    var UPPY_UPLOAD_ENDPOINT = "/api/cms-kit-admin/blogs/blog-posts/{0}/cover-image";
    var UPPY_FILE_ID = "uppy-upload-file";

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
                    submitEntityContent();
                },
                error: function (result) {
                    abp.ui.clearBusy(); abp.notify.error(result.responseJSON.error.message);
                }
            });
        }
    });

    $buttonSubmit.click(function (e) {
        e.preventDefault();
        $formUpdate.submit();
    });

    function submitEntityContent() {

        var contentId = $contentIdInput.val();
        var blogPostId = $blogPostIdInput.val();
        var contentValue = $contentValueInput.val();

        if (contentId) {
            volo.cmsKit.admin.contents.contentAdmin
                .update(contentId,
                    {
                        value: contentValue
                    })
                .then(function (result) {
                    entityContentCallback(blogPostId);
                });
        }
        else {
            volo.cmsKit.admin.contents.contentAdmin
                .create({
                    entityType: 'BlogPost',
                    entityId: blogPostId,
                    value: contentValue
                })
                .then(function (result) {
                    entityContentCallback(blogPostId);
                });
        }
    }

    function entityContentCallback(blogPostId) {
        if ($tagsInput.val()) {
            submitEntityTags(blogPostId);
        }
        else {
            submitCoverImage(blogPostId);
        }
    }

    function submitEntityTags(blogPostId) {      

        var tags = $tagsInput.val().split(",");

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

    function finishSaving(result) {
        abp.notify.success(l('SuccessfullySaved'));
        abp.ui.clearBusy();
        location.href = "../BlogPosts/";
    }

    $titleClone.on('change paste keyup', function () {
        $title.val($titleClone.val());
    });

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
});