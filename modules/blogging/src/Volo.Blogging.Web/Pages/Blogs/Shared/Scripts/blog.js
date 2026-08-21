(function () {
    function handleImages() {
        //if ($(window).width() > 767) {
        //    $(".box-articles .img-container").each(function () {
        //        var squareWidth = $(this).width();
        //        $(this).css("height", squareWidth);
        //    });
        //}
        //else {
        //    $(".box-articles .img-container").css("height", "auto");
        //}
    }

    $(function () {
        handleImages();

        $('.nav-link').on('click', function () {
            $(this).parent().parent();
        });

        $(window).resize(function () {
            setTimeout(function () {
                handleImages();
            }, 500);
        });
    });
})();
