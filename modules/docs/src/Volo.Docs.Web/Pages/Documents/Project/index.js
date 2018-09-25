(function ($) {

    $(function () {

        /*<FILTERING DOCUMENT ITEMS (LEFT-SIDEBAR)>*/
        var navigationElementId = "sidebar-scroll";
        var $navigation = $("#" + navigationElementId);

        var getShownDocumentLinks = function () {
            return $navigation.find(".mCSB_container > li a:visible").not(".tree-toggle");
        };

        var gotoFilteredDocumentIfThereIsOnlyOne = function () {
            var $links = getShownDocumentLinks();
            if ($links.length === 1) {
                window.location = $links.first().attr("href");
            }
        };

        var filterDocumentItems = function (filterText) {

            $navigation.find(".mCSB_container .opened").removeClass("opened");
            $navigation.find(".mCSB_container > li, .mCSB_container > li ul").hide();

            if (!filterText) {
                $navigation.find(".mCSB_container > li").show();
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
                    if ($parent.attr("id") === navigationElementId) {
                        break;
                    }

                    $parent.show();
                    $parent.find("> li > label").not(".last-link").addClass("opened");

                    $parent = $parent.parent();
                    hasParent = $parent.length > 0;
                }
            });
        };

        $(".docs-page .docs-filter input[type='search']").keyup(function (e) {
            filterDocumentItems(e.target.value);

            if (e.key === "Enter") {
                gotoFilteredDocumentIfThereIsOnlyOne();
            }
        });

        /*</FILTERING DOCUMENT ITEMS (LEFT-SIDEBAR)>*/

     
        var addAncharTags = function () {
            anchors.options = {
                placement: 'left'
            };

            var anchorTags = ["h1", "h2", "h3", "h4", "h5", "h6"];
            var container = ".docs-page .docs-body";
            anchorTags.forEach(function (tag) {
                anchors.add(container + " " + tag);
            });
        };

        addAncharTags();
    });

})(jQuery);