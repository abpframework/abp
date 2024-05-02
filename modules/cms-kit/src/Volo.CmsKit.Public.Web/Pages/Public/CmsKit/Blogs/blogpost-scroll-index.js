$(function () {
    var $myNav = $('#blog-post-sticky-index');
    var $scrollToTopBtn = $('.scroll-top-btn');

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

    Toc.init($myNav);

    $('body').scrollspy({
        target: $myNav,
    });

    $scrollToTopBtn.click(function () {
        $('html, body').animate({scrollTop: 0}, 'fast');
    });

    // When the user scrolls down 20px from the top of the document, show the button
    window.onscroll = function () {
        scrollFunction()
    };

    function scrollFunction() {
        if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
            $scrollToTopBtn.addClass('showup');
        } else {
            $scrollToTopBtn.removeClass('showup');
        }
    }
});
