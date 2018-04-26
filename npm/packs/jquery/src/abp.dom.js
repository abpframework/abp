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

    abp.dom.onElementUnloaded = function (callback) {
        abp.event.on('abp.dom.elementUnloaded', callback);
    };

    abp.dom.elementUnloaded = function (args) {
        abp.event.trigger('abp.dom.elementUnloaded', args);
    }

    $.fn.findWithSelf = function (selector) {
        return this.filter(selector).add(this.find(selector));
    };

})(jQuery);