(function() {
    function handleArrows() {
        var herosWidth = $(".hero-articles").width();
        var arrowsPosition = (herosWidth / 2 - 90);
        $(".owl-next").css("right", arrowsPosition);
        $(".owl-prev").css("left", arrowsPosition);
    }

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

    function handleOwlCarousel() {
        $('.hero-section .owl-carousel').owlCarousel({
            loop: true,
            margin: 0,
            nav: false,
            dots: false,
            //center: true, 
            autoplay: true,
            autoHeight: true,
            autoplaySpeed: 1000,
            items: 1,
        });
        $('.card-article-container .owl-carousel').owlCarousel({
            loop: true,
            margin: 0,
            nav: false,
            dots: false,
            autoplay: true,
            autoplaySpeed: 1000,
            responsive: {
                0: {
                    items: 1
                }
            }
        });
    }

    $(function () {
        handleImages();
        handleOwlCarousel();

        $(".nav-link").on("click", function () {
            $(this).parent().parent();
        });

        $(window).resize(function () {
            setTimeout(function () {
                handleArrows();
                handleImages();
            }, 500);
        });

        setTimeout(function () {
            handleArrows();
        }, 500);
    });
})();