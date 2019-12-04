(function ($) {

    $(function () {

        var initToc = function () {

            function handleCustomScrolls() {
                var wWidth = $(window).width();
                if (wWidth > 766) {
                    $("#sidebar-scroll").mCustomScrollbar({
                        theme: "minimal"
                    });

                    $("#scroll-index").mCustomScrollbar({
                        theme: "minimal-dark"
                    });
                }
            }

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

            var scrollToHashLink = function () {
                var hash = window.location.hash;

                if (!hash || hash === "#") {
                    return;
                }

                var $targetElement = $(hash);

                $targetElement = $targetElement.length ? $targetElement : $('[name=' + this.hash.slice(1) + ']');

                if (!$targetElement.length) {
                    return;
                }

                $('html,body').stop().animate({
                    scrollTop: $targetElement.offset().top
                }, 200);

                return;
            };

            var createToc = function () {
                handleCustomScrolls();

                $("#docs-sticky-index").empty();

                var $myNav = $("#docs-sticky-index");
                Toc.init($myNav);

                $("body").scrollspy({
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

                $(".btn-toggle").on("click", function () {
                    $(".toggle-row").slideToggle(400);
                    $(this).toggleClass("less");
                });

                $(".close-mmenu").on("click", function () {
                    $(".navbar-collapse").removeClass("show");
                });

                $(".open-dmenu").on("click", function () {
                    $(".docs-tree-list").slideToggle();
                });

                scrollToHashLink();
            };

            $(window).resize(function () {
                handleCustomScrolls();
            });


            createToc();
        };

        var initNavigationFilter = function (navigationContainerId) {

            var $navigation = $("#" + navigationContainerId);

            var getShownDocumentLinks = function () {
                return $navigation.find(".mCSB_container > li a:visible").not(".tree-toggle");
            };

            var gotoFilteredDocumentIfThereIsOnlyOne = function () {
                var $links = getShownDocumentLinks();
                if ($links.length === 1) {
                    var url = $links.first().attr("href");
                    if (url === "javascript:;") {
                        return;
                    }

                    window.location = url;
                }
            };

            var filterDocumentItems = function (filterText) {
                $navigation.find(".mCSB_container .opened").removeClass("opened");
                $navigation.find(".mCSB_container > li, .mCSB_container > li ul").hide();

                if (!filterText) {
                    $navigation.find(".mCSB_container > li").show();
                    $navigation.find(".mCSB_container .selected-tree > ul").show();
                    return;
                }

                var filteredItems = $navigation.find("li > a").filter(function () {
                    return $(this).text().toUpperCase().indexOf(filterText.toUpperCase()) > -1;
                });

                filteredItems.each(function () {

                    var $el = $(this);
                    $el.show();
                    var $parent = $el.parent();

                    var hasParent = true;
                    while (hasParent) {
                        if ($parent.attr("id") === navigationContainerId) {
                            break;
                        }

                        $parent.show();
                        $parent.find("> li > label").not(".last-link").addClass("opened");

                        $parent = $parent.parent();
                        hasParent = $parent.length > 0;
                    }
                });
            };

            $(".docs-page .docs-tree-list input[type='search']").keyup(function (e) {
                filterDocumentItems(e.target.value);

                if (e.key === "Enter") {
                    gotoFilteredDocumentIfThereIsOnlyOne();
                }
            });
        };

        var initAnchorTags = function (container) {
            anchors.options = {
                placement: 'left'
            };

            var anchorTags = ["h1", "h2", "h3", "h4", "h5", "h6"];
            anchorTags.forEach(function (tag) {
                anchors.add(container + " " + tag);
            });
        };

        var initSocialShareLinks = function () {
            var pageHeader = $(".docs-body").find("h1, h2").first().text();

            var projectName = $('#ProjectName')[0].innerText;

            $('#TwitterShareLink').attr('href',
                'https://twitter.com/intent/tweet?text=' + encodeURI(pageHeader + " | " + projectName + " | " + window.location.href)
            );

            $('#LinkedinShareLink').attr('href',
                'https://www.linkedin.com/shareArticle?'
                + 'url=' + encodeURI(window.location.href) + '&'
                + 'mini=true&'
                + "summary=" + encodeURI(projectName) + '&'
                + "title=" + encodeURI(pageHeader) + '&'
                + "source=" + encodeURI($('#GoToMainWebSite').attr('href'))
            );

            $('#EmailShareLink').attr('href',
                'mailto:?'
                + 'body=' + encodeURI('I want you to look at ' + window.location.href) + '&'
                + "subject=" + encodeURI(pageHeader + ' | ' + projectName) + '&'
            );
        };

        var initSections = function () {
            var getQueryStringParameterByName = function (name) {
                var url = window.location.href;
                name = name.replace(/[\[\]]/g, '\\$&');
                var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                    results = regex.exec(url);
                if (!results) return null;
                if (!results[2]) return '';
                return decodeURIComponent(results[2].replace(/\+/g, ' '));
            };

            var setQueryString = function () {
                var uri = window.location.href.toString();

                if (uri.indexOf("?") > 0) {
                    uri = uri.substring(0, uri.indexOf("?"));
                }

                var comboboxes = $(".doc-section-combobox");

                var new_uri = uri + "?";

                for (var i = 0; i < comboboxes.length; i++) {
                    var key = $(comboboxes[i]).data('key');
                    var value = comboboxes[i].value;

                    new_uri += key + "=" + value;

                    if (i !== comboboxes.length - 1) {
                        new_uri += "&";
                    }
                }

                window.history.replaceState({}, document.title, new_uri);
            };

            var setSections = function () {

                var sections = $(".doc-section");

                var comboboxes = $(".doc-section-combobox");
                var comboboxKeys = [];
                var comboboxSelectedValues = [];

                for (var i = 0; i < comboboxes.length; i++) {
                    comboboxKeys.push($(comboboxes[i]).data("key"));
                    comboboxSelectedValues.push(comboboxes[i].value);
                }

                for (i = 0; i < sections.length; i++) {
                    var keys = $(sections[i]).data("keys");
                    var values = $(sections[i]).data("values");

                    var show = false;
                    var keysSplitted;
                    var valuesSplitted;

                    if (keys.indexOf("&") >= 0) {
                        keysSplitted = keys.split("&");
                        valuesSplitted = values.split("&");

                        var hide = false;

                        for (var k = 0; k < keysSplitted.length; k++) {
                            if (valuesSplitted[k] !== comboboxSelectedValues[comboboxKeys.indexOf(keysSplitted[k])]) {
                                hide = true;
                                break;
                            }
                        }

                        show = !hide;
                    }
                    else if (keys.indexOf("|") >= 0) {
                        keysSplitted = keys.split("|");
                        valuesSplitted = values.split("|");

                        for (k = 0; k < keysSplitted.length; k++) {
                            if (valuesSplitted[k] === comboboxSelectedValues[comboboxKeys.indexOf(keysSplitted[k])]) {
                                show = true;
                                break;
                            }
                        }
                    }
                    else {
                        if (values === comboboxSelectedValues[comboboxKeys.indexOf(keys)]) {
                            show = true;
                        }
                    }

                    var headers = $(sections[i]).find(':header');

                    if (show) {
                        headers.removeAttr('data-toc-skip');
                        $(sections[i]).show();
                    }
                    else {
                        headers.attr('data-toc-skip', 'true');
                        $(sections[i]).hide();
                    }
                }

                setQueryString();

                initToc();
            };

            $(".doc-section-combobox").change(function () {
                localStorage["abp-doc-section-" + $(this).data("key")] = this.value;
                setSections();
            });

            var comboboxes = $(".doc-section-combobox");

            for (var i = 0; i < comboboxes.length; i++) {
                var key = $(comboboxes[i]).data("key");

                var cacheValue = localStorage["abp-doc-section-" + key];

                if (cacheValue) {
                    comboboxes[i].value = cacheValue;
                }
                var queryValue = getQueryStringParameterByName(key);

                if (queryValue) {
                    comboboxes[i].value = queryValue;
                }
            }

            setSections();
        };


        initNavigationFilter("sidebar-scroll");

        initAnchorTags(".docs-page .docs-body");

        initSocialShareLinks();

        initSections();

        initToc();
    });

})(jQuery);

