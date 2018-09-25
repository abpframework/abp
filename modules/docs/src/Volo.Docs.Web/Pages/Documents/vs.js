$('.tree-toggle').click(function () {
    $(this).parent().children('ul.tree').toggle(100);
    $(this).toggleClass("opened");
});

$(document).ready(function () {
    var scrollTopBtn = $(".scroll-top-btn");
    var enoughHeight = $(".docs-sidebar-wrapper > .docs-top").height() + 60;

    $(window).scroll(function () {
        var topPos = $(window).scrollTop();
        if (topPos > enoughHeight) {
            $(scrollTopBtn).addClass("showup");
            $("body").addClass("scrolled");
        } else {
            $(scrollTopBtn).removeClass("showup");
            $("body").removeClass("scrolled");
        }
    });

    $(scrollTopBtn).click(function () {
        $('html, body').animate({
            scrollTop: 0
        }, 500);
        return false;
    });
});
 
 
    $(function () {
        var navSelector = '#docs-sticky-index';
        var $myNav = $(navSelector);
        Toc.init($myNav);
        $('body').scrollspy({
            target: navSelector
        });
    });
    $("#docs-sticky-index a").on('click', function (event) {
        if (this.hash !== "") {
            event.preventDefault();
            var hash = this.hash;
            $('html, body').animate({
                scrollTop: $(hash).offset().top
            }, 800, function () {
                window.location.hash = hash;
            });
        }
    });

$('.btn-toggle').on("click", function () {
    $(".toggle-row").slideToggle(400);
    $(this).toggleClass("less");
});

$('.close-mmenu').on("click", function () {
    $(".navbar-collapse").removeClass("show");
});

$('.open-dmenu').on("click", function () {
    $(".docs-tree-list").slideToggle();
});


(function ($) {

    $(window).on("load", function () {
        $("#sidebar-scroll").mCustomScrollbar({
            theme: "minimal"
        });
    });

    $(window).on("load", function () {
        $("#index-scroll").mCustomScrollbar({
            theme: "minimal-dark"
        });
    });

})(jQuery);

