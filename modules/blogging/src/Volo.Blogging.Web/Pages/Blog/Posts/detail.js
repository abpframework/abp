(function ($) {

    $('div .replyForm').hide();

    $('a').click(function (event) {
        var data = $(this).attr('data-relpyid');

        if (data == '' || data === undefined) {
            return;
        }

        event.preventDefault();

        var div = $(this).parent().next();

        if (div.is(":hidden")) {
            $('div .replyForm').hide();
            div.show();
        } else {
            div.hide();
        }

    });

})(jQuery);
