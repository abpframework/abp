(function ($) {
    $('#Post_Title').on("change paste keyup", function() {
        var title = $('#Post_Title').val();

        if (title.length > 64) {
            title = title.substring(0, 64);
        }

        title = title.replace(' ','-');

        $('#Post_Url').val(title);
    });

})(jQuery);
