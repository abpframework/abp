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
            var clearQueryString = function () {
                var uri = window.location.href.toString();

                if (uri.indexOf("?") > 0) {
                    uri = uri.substring(0, uri.indexOf("?"));
                }

                window.history.replaceState({}, document.title, uri);
            };

            var setQueryString = function () {
                clearQueryString();

                var uri = window.location.href.toString();

                var comboboxes = $(".doc-section-combobox");

                if (comboboxes.length < 1) {
                    return;
                }

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

            var getTenYearsLater = function () {
                var tenYearsLater = new Date();
                tenYearsLater.setTime(tenYearsLater.getTime() + (365 * 10 * 24 * 60 * 60 * 1000));
                return tenYearsLater;
            };

            var setCookies = function () {
                var cookie = abp.utils.getCookieValue("AbpDocsPreferences");

                if (!cookie || cookie == null || cookie === null) {
                    cookie = "";
                }
                var keyValues = cookie.split("|");

                var comboboxes = $(".doc-section-combobox");

                for (var i = 0; i < comboboxes.length; i++) {
                    var key = $(comboboxes[i]).data('key');
                    var value = comboboxes[i].value;

                    var changed = false;
                    var keyValueslength = keyValues.length;
                    for (var k = 0; k < keyValueslength; k++) {
                        var splitted = keyValues[k].split("=");

                        if (splitted.length > 0 && splitted[0] === key) {
                            keyValues[k] = key + "=" + value;
                            changed = true;
                        }
                    }

                    if (!changed) {
                        keyValues.push(key + "=" + value);
                    }
                }

                abp.utils.setCookieValue("AbpDocsPreferences", keyValues.join('|'), getTenYearsLater(), '/');
            };

            $(".doc-section-combobox").change(function () {
                setCookies();
                clearQueryString();
                location.reload();
            });

            setQueryString();
        };

        var initCrawlerLinks = function () {

            var isCrawler = function () {
                var crawlers = [
                    'Google',
                    'Googlebot',
                    'YandexBot',
                    'msnbot',
                    'Rambler',
                    'Yahoo', 
                    'AbachoBOT', 
                    'accoona', 
                    'AcoiRobot',
                    'ASPSeek',
                    'CrocCrawler', 
                    'Dumbot',
                    'FAST-WebCrawler', 
                    'GeonaBot', 
                    'Gigabot',
                    'Lycos', 
                    'MSRBOT', 
                    'Scooter',
                    'AltaVista',
                    'IDBot', 
                    'eStyle', 
                    'Scrubby', 
                    'Slurp', 
                    'DuckDuckBot', 
                    'Baiduspider', 
                    'VoilaBot', 
                    'ExaLead', 
                    'Search Dog', 
                    'MSN Bot' , 
                    'BingBot' 
                ];

                var agent = navigator.userAgent;

                for (var i = 0; i < crawlers.length; i++) {

                    if (agent.indexOf(crawlers[i]) >= 0) {
                        return true;
                    }
                }

                return false;
            };

            if (!isCrawler()) {
                return;
            }

            var comboboxes = $(".doc-section-combobox");

            if (comboboxes.length <= 0) {
                return;
            }

            $("#crawler_link").show();

            var html = '';

            var currentUrl = window.location.href.toString();

            if (currentUrl.indexOf("?") > 0) {
                currentUrl = currentUrl.substring(0, currentUrl.indexOf("?"));
            }

            var getQueryStringsFromComboboxes = function (x) {
                if (x >= comboboxes.length) {
                    return [];
                }

                var key = $(comboboxes[x]).data("key");

                var queryStrings = getQueryStringsFromComboboxes(x + 1);
                var returnList = [];

                $(comboboxes[x]).find("option").each(function () {

                    if (queryStrings.length <= 0) {
                        returnList.push(key + "=" + $(this).val());
                        
                    }
                    else {
                        for (var k = 0; k < queryStrings.length; k++) {
                            returnList.push(key + "=" + $(this).val() + "&" + queryStrings[k]);
                        }
                    }
                });

                return returnList;
            };

            var queryStrings = getQueryStringsFromComboboxes(0);

            for (var i = 0; i < queryStrings.length; i++) {
                html += "<a href=\"" + currentUrl + "?" + queryStrings[i] +"\">" + queryStrings[i]+"</a> "
            }

            $("#crawler_link").html(html);
        };

        initNavigationFilter("sidebar-scroll");

        initAnchorTags(".docs-page .docs-body");

        initSocialShareLinks();

        initSections();

        initCrawlerLinks();

    });

})(jQuery);

