(function ($) {

    var l = abp.localization.getResource('Blogging');

    $('div .replyForm').hide();

    $('div .editForm').hide();

    $('form[class="editFormClass"]').submit(function (event) {
        event.preventDefault();
        var form = $(this).serializeFormToObject();

        $.ajax({
            type: "POST",
            url: "/Blog/Comments/Update",
            data: {
                id: form.commentId,
                commentDto: {
                    text: form.text
                }
            },
            success: function (response) {
                $('div .editForm').hide();
                $('#' + form.commentId).text(form.text);
            }
        });
    });

    $('.editCancelButton').click(function (event) {
        event.preventDefault();
        $('div .editForm').hide();
    });

    $('.replyCancelButton').click(function (event) {
        event.preventDefault();
        $('div .replyForm').hide();
    });

    $('.replyLink').click(function (event) {
        event.preventDefault();
        $('div .editForm').hide();
        var linkElement = $(this);
        var replyCommentId = linkElement.attr('data-relpyid');

        if (replyCommentId != '' && replyCommentId !== undefined) {
            var div = linkElement.parent().next();

            if (div.is(":hidden")) {
                $('div .replyForm').hide();
                div.show();
            } else {
                div.hide();
            }
            return;
        }
    });

    $('.deleteLink').click(function (event) {
        event.preventDefault();
        var linkElement = $(this);
        var deleteCommentId = linkElement.attr('data-deleteid');

        if (deleteCommentId != '' && deleteCommentId !== undefined) {
            abp.message.confirm(
                l('CommentDeletionWarningMessage'), // TODO: localize
                l('Are you sure?'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        $.ajax({
                            type: "POST",
                            url: "/Blog/Comments/Delete",
                            data: { id: deleteCommentId },
                            success: function (response) {
                                linkElement.parent().parent().parent().remove();
                            }
                        });
                    }
                }
            );
        }
    });

    $('#DeletePostLink').click(function (event) {
        event.preventDefault();
        var linkElement = $(this);
        var deleteCommentId = linkElement.attr('data-postid');
        var blogShortName = linkElement.attr('data-blogShortName');

        if (deleteCommentId != '' && deleteCommentId !== undefined) {
            abp.message.confirm(
                l('PostDeletionWarningMessage'),
                l('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        $.ajax({
                            type: "POST",
                            url: "/Blog/Posts/Delete",
                            data: { id: deleteCommentId },
                            success: function () {
                                var url = window.location.pathname;
                                var postNameBeginning = url.lastIndexOf('/');
                                window.location.replace(url.substring(0, postNameBeginning));
                            }
                        });
                    }
                }
            );
        }
    });

    $('.updateLink').click(function (event) {
        event.preventDefault();
        $('div .replyForm').hide();

        var linkElement = $(this);
        var updateCommentId = $(this).attr('data-updateid');

        if (updateCommentId != '' && updateCommentId !== undefined) {

            var div = linkElement.parent().next().next();

            if (div.is(":hidden")) {
                $('div .editForm').hide();
                div.show();
            } else {
                div.hide();
            }
            return;
        }
    });

    if ($('#FocusCommentId').val() != '00000000-0000-0000-0000-000000000000') {
        $('html, body').animate({
            scrollTop: ($('#' + $('#FocusCommentId').val()).offset().top - 150)
        }, 500);
    }

})(jQuery);
