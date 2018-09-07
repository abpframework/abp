(function ($) {

    $('div .replyForm').hide();

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

        var id = $(this).attr('data-deleteid');

        if (id != '' && id !== undefined) {

            event.preventDefault();
            console.log(id);
            $.ajax({
                type: "POST",
                url: "/Blog/Comments/Delete",
                data: { id: id },
                success: function (response) {
                    console.log(linkElement);
                    linkElement.parent().parent().parent().remove();
                }
            });
        }

    });

})(jQuery);
