var abp = abp || {};
(function ($) {

    if (!$) {
        return;
    }

    abp.dom = abp.dom || {};

    abp.dom.onElementLoaded = function (callback) {
        abp.event.on('abp.dom.elementLoaded', callback);
    };

    abp.dom.elementLoaded = function(args) {
        abp.event.trigger('abp.dom.elementLoaded', args);
    }

})(jQuery);