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
        var data = linkElement.attr('data-relpyid');

        if (data != '' && data !== undefined) {

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

        data = $(this).attr('data-deleteid');

        if (data != '' && data !== undefined) {

            event.preventDefault();

            $.ajax({
                type: "POST",
                url: "/Blog/Comments/Delete",
                data: { id: data },
                success: function (response) {
                    linkElement.parent().parent().parent().remove();
                }
            });
        }

        data = $(this).attr('data-updateid');

        if (data != '' && data !== undefined) {

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
