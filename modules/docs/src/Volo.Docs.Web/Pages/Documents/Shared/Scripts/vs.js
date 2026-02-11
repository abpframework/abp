(function ($) {
    $(function () {

        var scrollTopBtn = $('.scroll-top-btn');
        var enoughHeight = $('.docs-sidebar-wrapper > .docs-top').height();
        var enoughHeightPlus = 500;

        $(window).scroll(function () {
            var topPos = $(window).scrollTop();
            if (topPos > enoughHeight) {
                $(scrollTopBtn).addClass('showup');
                $('body').addClass('scrolled');
            } else {
                $(scrollTopBtn).removeClass('showup');
                $('body').removeClass('scrolled');
            }
            if (topPos > enoughHeightPlus) {
                $('body').addClass('scrolledMore');
            } else {
                $('body').removeClass('scrolledMore');
            }
        });

        $(scrollTopBtn).click(function () {
            $('html, body').animate(
                {
                    scrollTop: 0,
                },
                500
            );
            return false;
        });

        var scrollToHashLink = function () {
            var hash = window.location.hash;

            if (!hash || hash === '#' || hash === '#gsc.tab=0') {
                return;
            }

            hash = hash.split('&')[0];

            var $targetElement = $(decodeURIComponent(hash));

            $targetElement = $targetElement.length
                ? $targetElement
                : $('[name=' + hash.slice(1) + ']');

            if (!$targetElement.length) {
                return;
            }

            $('html,body').stop().animate(
                {
                    scrollTop: $targetElement.offset().top,
                },
                200
            );

            return;
        };

        $(document).ready(function () {
            handleCustomScrolls();

            var $myNav = $('#docs-sticky-index');

            if ($myNav.length === 0) {
                return;
            }

            $('body').scrollspy({
                target: $myNav,
                offset:100
            });

            $('#docs-sticky-index a').on('click', function (event) {
                if (this.hash !== '') {
                    event.preventDefault();
                    var hash = this.hash;
                    $('html, body').animate(
                        {
                            scrollTop: $(decodeURIComponent(hash)).offset().top,
                        },
                        500,
                        function () {
                            window.location.hash = hash;
                        }
                    );
                }
            });

            $("body").on('activate.bs.scrollspy', function (e) {
                var $activeLink = $('.nav-link.active', $('#docs-sticky-index'));

                var $activeLi = $activeLink.parent('li.nav-item');

                $myNav.find('li.toc-item-has-children.open').each(function () {
                    if ($(this).has($activeLi).length === 0) {
                        $(this).removeClass('open');
                    }
                });

                var $parentToOpen = $activeLi.closest('li.toc-item-has-children');
                if ($parentToOpen.length > 0) {
                    $parentToOpen.addClass('open');
                }
            });

            $('.btn-toggle').on('click', function () {
                $('.toggle-row').slideToggle(400);
                $(this).toggleClass('less');
            });

            $('.close-mmenu').on('click', function () {
                $('.navbar-collapse').removeClass('show');
            });

            $('.open-dmenu').on('click', function () {
                $('.docs-tree-list').slideToggle();
            });

            initMenuToggle();
            scrollToHashLink();
        });

        $(window).resize(function () {
            handleCustomScrolls();
        });
    });

    function handleCustomScrolls() {
        $('#sidebar-scroll').mCustomScrollbar({
            theme: 'minimal',
            alwaysShowScrollbar: 0,
        });

        $('#scroll-index').mCustomScrollbar({
            theme: 'minimal-dark',
            alwaysShowScrollbar: 0,
        });

        $('.mCustomScrollbar-1').mCustomScrollbar({
            theme: 'minimal-dark',
            alwaysShowScrollbar: 0,
            horizontalScroll: true,
        });
    }

     function initMenuToggle() {
        $('li:not(.last-link) a.tree-toggle').off('click');
        $('li:not(.last-link) span.plus-icon i.fa-chevron-right').off('click');
        
        $('li:not(.last-link) a.tree-toggle').click(function () {
            $(this).parent().children('ul.tree').toggle(100);
            $(this).closest('li').toggleClass('selected-tree');
        });

        $('li:not(.last-link) span.plus-icon i.fa-chevron-right').click(
            function () {
                var $element = $(this).parent();
                var $filter = $('.docs-version #filter');

                if ($filter && $filter.val() != ''){
                    return;
                }

                $element.parent().children('ul.tree').toggle(100);
                $element.closest('li').toggleClass('selected-tree');
            }
        );
    }

    function docsCriteria() {
        var docsContentWidth = $('.docs-content').width() - 74;
        $('.alert-criteria').width(docsContentWidth);
    }
    $(document).ready(function () {
        docsCriteria();
    });
    $(window).resize(function () {
        docsCriteria();
    });
})(jQuery);
