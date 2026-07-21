(function () {
    'use strict';

    var popover = window.mudPopover;
    if (!popover || typeof popover.createObservers !== 'function') {
        return;
    }

    var original = popover.createObservers;
    popover.createObservers = function (id) {
        if (!this.map[id]) {
            return;
        }
        return original.call(this, id);
    };
})();
