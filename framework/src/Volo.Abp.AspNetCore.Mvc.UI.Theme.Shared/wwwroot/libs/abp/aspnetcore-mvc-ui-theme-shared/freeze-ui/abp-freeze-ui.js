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
        var opt = { text : text ? text : ' ' };

        if (element) {
            opt.element = element;
        }

        if (freezeDelay) {
            opt.freezeDelay = freezeDelay;
        }
         
        window.FreezeUI(opt);
    };

    abp.ui.clearBusy = function () {
        window.UnFreezeUI();
    };

})(jQuery);