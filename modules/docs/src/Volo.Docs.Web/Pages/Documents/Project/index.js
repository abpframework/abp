(function ($) {

    $(function () {
       
        $("#DocumentVersion").change(function () {
           document.location.href = $(this).val();
        });
 
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
 

    });

})(jQuery);