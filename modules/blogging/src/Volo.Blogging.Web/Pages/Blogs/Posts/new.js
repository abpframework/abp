$(function () {
    var $container = $('#qa-new-post-container');
    var $editorContainer = $container.find('.new-post-editor');
    var $submitButton = $container.find('button[type=submit]');
    var $form = $container.find('form#new-post-form');
    var $titleLengthWarning = $('#title-length-warning');
    var maxTitleLength = parseInt($titleLengthWarning.data('max-length'));
    var $title = $('#Post_Title');
    var $url = $('#Post_Url');
    var $coverImage = $('#CoverImage');
    var $postCoverImage = $('#Post_CoverImage');
    var $coverImageFile = $('#CoverImageFile');
    var $postFormSubmitButton = $('#PostFormSubmitButton');

    var setCoverImage = function (file) {
        $postCoverImage.val(file.fileUrl);
        $coverImage.attr('src', file.fileUrl);
        $coverImage.show();
        $postFormSubmitButton.removeAttr('disabled');
    };

    var uploadCoverImage = function (file) {
        var formData = new FormData();
        formData.append('file', file);

        $.ajax({
            type: 'POST',
            url: '/api/blogging/files/images/upload',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                setCoverImage(response);
            },
        });
    };

    $coverImageFile.change(function () {
        if (!$coverImageFile.prop('files').length) {
            return;
        }
        $postFormSubmitButton.attr('disabled', true);
        var file = $coverImageFile.prop('files')[0];
        uploadCoverImage(file);
    });

    var uploadImage = function (file, callbackFn) {
        var formData = new FormData();
        formData.append('file', file);

        $.ajax({
            type: 'POST',
            url: '/api/blogging/files/images/upload',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                callbackFn(response.fileUrl);
            },
        });
    };

    var newPostEditor = new toastui.Editor({
        el: $editorContainer[0],
        usageStatistics: false,
        initialEditType: 'markdown',
        previewStyle: 'tab',
        height: 'auto',
        hooks: {
            addImageBlobHook: function (blob, callback, source) {
                var imageAltText = blob.name;

                uploadImage(blob, function (fileUrl) {
                    callback(fileUrl, imageAltText);
                });
            },
        },
        events: {
            load: function () {
                $editorContainer.find('.loading-cover').remove();
                $submitButton.prop('disabled', false);
                $form.data('validator').settings.ignore = '.ignore';
                $editorContainer.find(':input').addClass('ignore');
            },
        },
    });

    $container.find('form#new-post-form').submit(function (e) {
        var $postTextInput = $form.find("input[name='Post.Content']");

        var postText = newPostEditor.getMarkdown();
        $postTextInput.val(postText);

        if (!$form.valid()) {
            var validationResult = $form.validate();
            abp.message.warn(validationResult.errorList[0].message); //TODO: errors can be merged into lines. make sweetalert accept HTML.
            e.preventDefault();
            return false; //for old browsers
        }

        $submitButton.buttonBusy();
        $(this).off('submit').submit();
        return true;
    });

    var urlEdited = false;
    var reflectedChange = false;

    $title.on('change paste keyup', function () {
        if (urlEdited) {
            return;
        }

        var title = $title.val();

        if (title.length > maxTitleLength) {
            $titleLengthWarning.show();
        } else {
            $titleLengthWarning.hide();
        }

        title = title.replace(' &', ' ');
        title = title.replace('& ', ' ');
        title = title.replace('&', '');
        title = title.replace(' ', '-');
        title = title.replace('/', '-');
        title = title.replace(new RegExp(' ', 'g'), '-');
        reflectedChange = true;
        $url.val(title);
        reflectedChange = false;
    });

    $url.change(function () {
        if (!reflectedChange) {
            urlEdited = true;
        }
    });
});
