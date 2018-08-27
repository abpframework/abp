(function ($) {
    $('button[type="button"]').click(function () {
        var data = $(this).attr('data-button');

        if (data == '') {
            return;
        }

        $('#repliedCommentId').val(data);
        $('#textBoxId').focus();
    });

})(jQuery);
