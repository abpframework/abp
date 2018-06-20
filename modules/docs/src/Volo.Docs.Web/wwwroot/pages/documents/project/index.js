(function ($) {

    $(function () {
        // Build table of contents
        $('#docsContainer .toc-navigaton').each(function () {
            Toc.init({
                $nav: $(this),
                $scope: $("#docsContainer main.bd-content")
            });
        });


        $("#DocumentVersion").change(function () {
           document.location.href = $(this).val();
        });

        /*<GOOGLE CUSTOM SEARCH MODIFICATION>*/
        var getUrlParameter = function (sParam) {
            var sPageURL = decodeURIComponent(window.location.search.substring(1)),
                sURLVariables = sPageURL.split('&'),
                sParameterName,
                i;

            for (i = 0; i < sURLVariables.length; i++) {
                sParameterName = sURLVariables[i].split('=');

                if (sParameterName[0] === sParam) {
                    return sParameterName[1] === undefined ? true : sParameterName[1];
                }
            }
        };

        //function is being executed in window.onload
        var normalizeGoogleSearch = function () {
            var searchKeyName = "searchKey";
            var searchKeyToHighlight = getUrlParameter(searchKeyName);
            if (searchKeyToHighlight) {
                $("#content > .container").mark(searchKeyToHighlight);
            }

            var addSearchKeyword = function (targetLink) {
                var searchKey = $("input.gsc-input").val();
                return targetLink + "?" + searchKeyName + "=" + encodeURIComponent(searchKey);
            };

            $('body').on('click',
                'a.gs-title',
                function (e) {
                    e.preventDefault();

                    var $anchor = $(this);
                    var targetLink = $anchor.attr("data-ctorig");
                    var urlWithKeyword = addSearchKeyword(targetLink);
                    var tmp = urlWithKeyword.replace("https://aspnetboilerplate.com", "");
                    window.open(tmp);
                });

        };
        /*</GOOGLE CUSTOM SEARCH MODIFICATION>*/

        /*<FILTERING DOCUMENT ITEMS (LEFT-SIDEBAR)>*/
        var hideDocElement = function ($item) {
            $item.addClass("doc-hidden");
        };

        var showDocElement = function ($item) {
            $item.removeClass("doc-hidden");
        };

        var showHideAllDocumentItems = function (isShown) {
            if (isShown) {
                showDocElement($("#bd-docs-nav .doc-hidden"));
            } else {
                hideDocElement($("#bd-docs-nav").children());
            }
        };

        var showAllDocumentItems = function () {
            showHideAllDocumentItems(true);
        };

        var hideAllDocumentItems = function () {
            showHideAllDocumentItems(false);
        };

        var getShownDocumentLinks = function () {
            return $("#bd-docs-nav li > a:visible");
        };

        var gotoFilteredDocumentIfThereIsOnlyOne = function () {
            var $links = getShownDocumentLinks();
            if ($links.length === 1) {
                window.location = $links.first().attr("href");
            }
        };

        var filterDocumentItems = function(filterText) {
            var navigationElementId = "bd-docs-nav";
            var $navigation = $("#" + navigationElementId);
           
             if (!filterText) {
                $navigation.find("*").show();
                return;
            }

            var filterTextUpper = filterText.toUpperCase();
            var parentTagsToShow = ["p", "h3"];
            $navigation.find("*").hide();

            $navigation.find("li > a").filter(function () {
                    return $(this).text().toUpperCase().indexOf(filterTextUpper) > -1;
                }).each(function () {


                var $el = $(this);
                $el.show();
                var $parent = $el.parent();

                var hasParent = true;
                while (hasParent) {
                    if ($parent.attr("id") === navigationElementId) {
                        break;
                    }

                    $parent.show();

                    parentTagsToShow.forEach(function (parentTag) {
                        $parent.closest("ul").prevAll(parentTag + ":first").show();
                    });

                    $parent = $parent.parent();
                    hasParent = $parent.length > 0;
                }
            });
        };

        $("#filterInput").keyup(function (e) {
            filterDocumentItems(e.target.value);
            if (e.key === "Enter") {
                gotoFilteredDocumentIfThereIsOnlyOne();
            }
        });

        //fix for after collapsing navigation menu height of nav stays 30px
        $('#bd-docs-nav').on('hidden.bs.collapse', function () {
            $("#bd-docs-nav").css("height", "");
        });

        /*</FILTERING DOCUMENT ITEMS (LEFT-SIDEBAR)>*/


        $('pre').each(function (i, element) {
            $("<div/>")
                .addClass("code-header")
                .append($("<button>").addClass("copy").html("Copy"))
                .insertBefore($(element));
        });

        var setCopyToClipboardButtonClicks = function () {
            /*clipboard.js v1.7.1 https://zenorocha.github.io/clipboard.js*/
            var clipboard = new Clipboard("button.copy",
                {
                    target: function (trigger) {
                        return $(trigger).parent().next()[0];
                    }
                });

            clipboard.on('success', function (e) {
                var $btn = $(e.trigger);
                $btn.text("Copied").fadeTo("fast", 0.5);
                setTimeout(function () {
                    $btn.fadeTo("slow", 1,
                        function () {
                            $btn.text("Copy");
                        });
                }, 200);
            });

        };

        //var addAncharTags = function () {
        //     AnchorJS: https://www.bryanbraun.com/anchorjs
        //    var anchorTags = ["h1", "h2", "h3", "h4", "h5", "h6"];
        //    var container = ".bd-content";
        //    anchorTags.forEach(function (tag) {
        //        anchors.add(container + " " + tag);
        //    });
        //};
        //addAncharTags();

        setCopyToClipboardButtonClicks();

        $(window).on("load",
            function () {
                // Google fix
                normalizeGoogleSearch();
            });

        $(document).ready(function () {
            /*http://luis-almeida.github.io/unveil/*/
            $("img").unveil(0, function () {
                $(this).load(function () {
                    this.style.opacity = 1;
                });
            });
        });

    });

})(jQuery);