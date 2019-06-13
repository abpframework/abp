var abp = abp || {};
(function ($) {
    if (!window.FreezeUI || !$) {
        return;
    }

    /* MESSAGE **************************************************/
    /*Package from https://alexradulescu.github.io/freeze-ui/*/

    abp.ui = abp.ui || {};

    /* UI BLOCK */

    abp.ui.block = function (elm) {
        if (elm) {
            window.FreezeUI({ selector: elm });
        } else {
            window.FreezeUI();
        }
    };

    abp.ui.unblock = function () {
        window.UnFreezeUI();
    };

    /* UI BUSY */

    abp.ui.setBusy = function (element, text, freezeDelay) {
        var opt = { text: text ? text : ' ' };

        if (element) {
            opt.element = element;
        }

        if (freezeDelay) {
            opt.freezeDelay = freezeDelay;
        }

        window.FreezeUI(opt);
    };

    abp.ui.setBusy = function (options) {
        options = $.extend({}, options || {
            freezeDelay: 500
        });
        
        if (!options.text) {
            options.text = " ";
        }
        
        window.FreezeUI(options);

        if (options.promise) { 
            if (options.promise.always) {
                options.promise.always(function () {
                    abp.ui.clearBusy(options.element);
                });
            } else if (options.promise['finally']) {
                options.promise['finally'](function () {
                    abp.ui.clearBusy(options.element);
                });
            }
        }
    };


    abp.ui.clearBusy = function () {
        window.UnFreezeUI();
    };

})(jQuery);