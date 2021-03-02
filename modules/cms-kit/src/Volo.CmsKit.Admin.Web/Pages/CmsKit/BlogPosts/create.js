$(function () {

    var l = abp.localization.getResource("CmsKit");

    var $selectBlog = $('#BlogSelectionSelect');
    var $formCreate = $('#form-blog-post-create');
    var $title = $('#ViewModel_Title');
    var $shortDescription = $('#ViewModel_ShortDescription');
    var $url = $('#ViewModel_Slug');
    var $buttonSubmit = $('#button-blog-post-create');
    var $pageContentInput = $('#ViewModel_Value');
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
                    submitEntityContent(result.id);
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

    function submitEntityContent(blogPostId) {
        volo.cmsKit.admin.contents.contentAdmin
            .create(
                {
                    entityType: 'BlogPost',
                    entityId: blogPostId,
                    value: $pageContentInput.val()
                })
            .then(function (result) {
                if (isTagsEnabled) {
                    submitEntityTags(blogPostId)
                }
                else {
                    submitCoverImage(blogPostId);
                }
            });
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
});