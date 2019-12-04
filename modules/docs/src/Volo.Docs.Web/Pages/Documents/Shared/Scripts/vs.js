(function ($) {
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