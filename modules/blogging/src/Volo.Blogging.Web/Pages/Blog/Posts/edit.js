(function ($) {
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
