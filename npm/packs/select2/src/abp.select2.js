(function ($) {
    if (!$ || !$.fn.select2 || !$.fn.select2.amd) {
        return;
    }

    // Select2 4.1.x skips the direction lookup when the dir option is undefined and renders dir="undefined",
    // so its RTL styles never apply. Restore the 4.0.x lookup until an upstream release fixes it.
    var options = $.fn.select2.amd.require('select2/options').prototype;
    var fromElement = options.fromElement;

    options.fromElement = function ($element) {
        if (this.options.dir == null) {
            this.options.dir = $element.prop('dir') ||
                $element.closest('[dir]').prop('dir') ||
                'ltr';
        }

        return fromElement.call(this, $element);
    };
})(window.jQuery);
