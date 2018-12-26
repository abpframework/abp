(function ($) {

    $(function () {

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

            $(".docs-page .docs-filter input[type='search']").keyup(function (e) {
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

        initNavigationFilter("sidebar-scroll");

        initAnchorTags(".docs-page .docs-body");

        var getTitle = function() {
            var h1Tags = $(document).find('h1');
            if (h1Tags.length < 1) {
                return "";
            }
            return h1Tags[0].innerText;
        }

        $('#TwitterShareLink').attr(
            'href',
            'https://twitter.com/intent/tweet?text='
                      + encodeURI(getTitle() +
                " | " + $('#ProjectName')[0].innerText +
                " | " + window.location.href)
        );

        $('#LinkedinShareLink').attr(
            'href',
            'https://www.linkedin.com/shareArticle?'
            + 'url=' + encodeURI(window.location.href) + '&'
            + 'mini=true&'
            + "summary=" + encodeURI($('#ProjectName')[0].innerText) + '&'
            + "title=" + encodeURI(getTitle()) + '&'
            + "source=" + encodeURI($('#GoToMainWebSite').attr('href'))
        );

        $('#EmailShareLink').attr(
            'href',
            'mailto:?'
            + 'body=' + encodeURI('I want you to look at ' + window.location.href) + '&'
            + "subject=" + encodeURI(getTitle() + ' | ' + $('#ProjectName')[0].innerText) + '&'
        );

    });

})(jQuery);

