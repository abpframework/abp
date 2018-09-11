(function ($) {

    var $container = $("#edit-post-container");
    var $editorContainer = $container.find(".edit-post-editor");
    var $submitButton = $container.find("button[type=submit]");
    var $form = $container.find("form#edit-post-form");
    var editorDataKey = "tuiEditor";

    var setCoverImage = function (file) {
        console.log(file.fileUrl);
        $('#Post_CoverImage').val(file.fileUrl);
        $("#CoverImage").attr("src", file.fileUrl);
    };

    var uploadCoverImage = function (file) {
        var formData = new FormData();
        formData.append('file', file);

        $.ajax({
            type: "POST",
            url: "/Blog/Files/UploadImage",
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
            url: "/Blog/Files/UploadImage",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                callbackFn(response.fileUrl);
            }
        });
    };

    console.log($form.find("input[name='Post.Content']").val() + "asda");
    var newPostEditor = $editorContainer.tuiEditor({
        usageStatistics: false,
        initialEditType: 'markdown',
        previewStyle: 'tab',
        height: "auto",
        initialValue: $form.find("input[name='Post.Content']").val(),
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
            }
        }
    }).data(editorDataKey);

    $container.find("form#edit-post-form").submit(function (e) {
        var $postTextInput = $form.find("input[name='Post.Content']");

        var postText = newPostEditor.getMarkdown();
        $postTextInput.val(postText);
        console.log(postText);

        $submitButton.buttonBusy();
        $(this).off('submit').submit();
    });

    $('#Post_Title').on("change paste keyup", function() {
        var title = $('#Post_Title').val();

        if (title.length > 64) {
            title = title.substring(0, 64);
        }

        title = title.replace(' ','-');
        title = title.replace(new RegExp(' ', 'g'), '-');
        $('#Post_Url').val(title);
    });

})(jQuery);
