(function () {
    'use strict';

    // Workaround for MudBlazor 9.x ripple cleanup not always running in Blazor SSR /
    // WebApp / InteractiveAuto render modes. mud's own Ripple.js relies on the
    // pointerdown / pointerup pair to schedule `.mud-ripple-effect-expanding` removal
    // (see Q/J/K in MudBlazor.min.js). When Blazor re-renders the click target before
    // pointerup fires, mud loses its `_mudRipples` Map and the ripple span is never
    // cleaned up - stale spans accumulate as gray splotches on tabs / nav-links /
    // buttons, looking as if several items are "selected".
    //
    // Tracking upstream at https://github.com/MudBlazor/MudBlazor/issues/12128 (open
    // since 2025-11, reproduces on 9.1+ with Blazor Server). Until that ships we
    // observe DOM mutations and schedule the same fade-then-remove sequence mud's
    // own Q() would. If mud's cleanup runs first the span is already gone and the
    // setTimeout callbacks are no-ops.

    var CLEANUP_DELAY = 600;
    var FADE_DELAY = 400;

    function scheduleCleanup(ripple) {
        setTimeout(function () {
            if (!ripple.parentNode) {
                return;
            }
            if (!ripple.classList.contains('mud-ripple-effect-fading')) {
                ripple.classList.add('mud-ripple-effect-fading');
            }
            setTimeout(function () {
                if (ripple.parentNode) {
                    ripple.remove();
                }
            }, FADE_DELAY);
        }, CLEANUP_DELAY);
    }

    function startObserving() {
        new MutationObserver(function (mutations) {
            for (var i = 0; i < mutations.length; i++) {
                var added = mutations[i].addedNodes;
                for (var j = 0; j < added.length; j++) {
                    var n = added[j];
                    if (n.nodeType !== 1 || !n.classList) {
                        continue;
                    }
                    // mud appends the ripple span with className `mud-ripple-effect`
                    // first, and only adds `-expanding` on the next tick. Match by
                    // the base class so the MutationObserver `childList` callback
                    // catches the node on the very first mutation.
                    if (n.classList.contains('mud-ripple-effect')) {
                        scheduleCleanup(n);
                    }
                }
            }
        }).observe(document.body, { subtree: true, childList: true });
    }

    if (document.body) {
        startObserving();
    } else {
        document.addEventListener('DOMContentLoaded', startObserving);
    }
})();
