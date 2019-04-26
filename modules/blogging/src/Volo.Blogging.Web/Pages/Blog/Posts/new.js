$(function () {

    var $container = $("#qa-new-post-container");
    var $editorContainer = $container.find(".new-post-editor");
    var $submitButton = $container.find("button[type=submit]");
    var $form = $container.find("form#new-post-form");
    var editorDataKey = "tuiEditor";

    var setCoverImage = function (file) {
        $('#Post_CoverImage').val(file.fileUrl);
        $("#CoverImage").attr("src", file.fileUrl);
        $("#CoverImage").show();
    };

    var uploadCoverImage = function (file) {
        var formData = new FormData();
        formData.append('file', file);

        $.ajax({
            type: "POST",
            url: "/api/blogging/files/images/upload",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                setCoverImage(response);
            }
        });
    };

    $('#CoverImageFile').change(function () {
        if (!$('#CoverImageFile').prop('files').length) {
            return;
        }
        var file = $('#CoverImageFile').prop('files')[0];
        uploadCoverImage(file);
    });

    var uploadImage = function (file, callbackFn) {
        var formData = new FormData();
        formData.append('file', file);

        $.ajax({ 
            type: "POST",
            url: "/api/blogging/files/images/upload",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                callbackFn(response.fileUrl);
            }
        });
    };

    var newPostEditor = $editorContainer.tuiEditor({
        usageStatistics: false,
        initialEditType: 'markdown',
        previewStyle: 'tab',
        height: "auto",
        hooks: {
            addImageBlobHook: function (blob, callback, source) {
                var imageAltText = blob.name;

                uploadImage(blob,
                    function (fileUrl) {
                        callback(fileUrl, imageAltText);
                    });
            }
        },
        events: {
            load: function () {
                $editorContainer.find(".loading-cover").remove();
                $submitButton.prop("disabled", false);
                $form.data("validator").settings.ignore = '.ignore';
                $editorContainer.find(':input').addClass('ignore');
            }
        }
    }).data(editorDataKey);

    $container.find("form#new-post-form").submit(function (e) {
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
    });

    var urlEdited = false;
    var reflectedChange = false;

    $('#Post_Title').on("change paste keyup", function () {
        if (urlEdited) {
            return;
        }

        var title = $('#Post_Title').val();

        if (title.length > 64) {
            title = title.substring(0, 64);
        }

        title = title.replace(' ', '-');
        title = title.replace(new RegExp(' ', 'g'), '-');
        reflectedChange = true;
        $('#Post_Url').val(title);
        reflectedChange = false;
    });

    $('#Post_Url').change(function () {
        if (!reflectedChange) {
            urlEdited = true;
        }
    });

});
