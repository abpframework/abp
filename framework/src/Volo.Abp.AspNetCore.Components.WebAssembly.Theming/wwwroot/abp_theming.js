var abp = abp || {};
(function () {
    abp.utils = abp.utils || {};

    abp.utils.addClassToTag = function (tagName, className) {
        var tags = document.getElementsByTagName(tagName);
        for (var i = 0; i < tags.length; i++) {
            tags[i].classList.add(className);
        }
    };

    abp.utils.removeClassFromTag = function (tagName, className) {
        var tags = document.getElementsByTagName(tagName);
        for (var i = 0; i < tags.length; i++) {
            tags[i].classList.remove(className);
        }
    };

    abp.utils.hasClassOnTag = function (tagName, className) {
        var tags = document.getElementsByTagName(tagName);
        if (tags.length) {
            return tags[0].classList.contains(className);
        }

        return false;
    };

    abp.utils.replaceLinkHrefById = function (linkId, hrefValue) {
        var link = document.getElementById(linkId);

        if (link && link.href !== hrefValue) {
            link.href = hrefValue;
        }
    };
})();
