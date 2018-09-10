(function ($) {

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

    $('a').click(function (event) {
        var linkElement = $(this);
        var replyCommentId = linkElement.attr('data-relpyid');

        if (replyCommentId != '' && replyCommentId !== undefined) {

            event.preventDefault();

            var div = $(this).parent().next();

            if (div.is(":hidden")) {
                $('div .replyForm').hide();
                div.show();
            } else {
                div.hide();
            }
            return;
        }

        var deleteCommentId = $(this).attr('data-deleteid');

        if (deleteCommentId != '' && deleteCommentId !== undefined) {

            console.log(deleteCommentId);
            event.preventDefault();
            abp.message.confirm(
                'User admin will be deleted.',
                'Are you sure?',
                function (isConfirmed) {
                    console.log(deleteCommentId);
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

        var updateCommentId = $(this).attr('data-updateid');

        if (updateCommentId != '' && updateCommentId !== undefined) {

            event.preventDefault();

            var div = $(this).parent().next().next();

            if (div.is(":hidden")) {
                $('div .editForm').hide();
                div.show();
            } else {
                div.hide();
            }
            return;
        }


    });

})(jQuery);
