(function ($) {
    $(function () {
        $('li:not(.last-link) a.tree-toggle').click(function () {
            $(this).parent().children('ul.tree').toggle(100);
            $(this).closest("li").toggleClass("selected-tree");
        });
        $('li:not(.last-link) span.plus-icon i.fa-chevron-right').click(function () {

            var $element = $(this).parent();

            $element.parent().children('ul.tree').toggle(100);
            $element.closest("li").toggleClass("selected-tree");
        });

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

        var navSelector = '#docs-sticky-index';
        var $myNav = $(navSelector);
        Toc.init($myNav);
        $('body').scrollspy({
            target: $myNav
        });
        $("#docs-sticky-index a").on('click', function (event) {
            if (this.hash !== "") {
                event.preventDefault();
                var hash = this.hash;
                $('html, body').animate({
                    scrollTop: $(hash).offset().top
                }, 500, function () {
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

        $(window).on("load", function () {
            $("#sidebar-scroll").mCustomScrollbar({
                theme: "minimal"
            });
        });

        $(window).on("load", function () {
            $("#scroll-index").mCustomScrollbar({
                theme: "minimal-dark"
            });
        });
    });

    window.Toc.helpers.createNavList = function () {
        return $('<ul class="nav nav-pills flex-column"></ul>');
    };

    window.Toc.helpers.createChildNavList = function ($parent) {
        var $childList = this.createNavList();
        $parent.append($childList);
        return $childList;
    };

    window.Toc.helpers.generateNavEl = function (anchor, text) {
        var $a = $('<a class="nav-link"></a>');
        $a.attr('href', '#' + anchor);
        $a.text(text);
        var $li = $('<li class="nav-item"></li>');
        $li.append($a);
        return $li;
    };
})(jQuery);