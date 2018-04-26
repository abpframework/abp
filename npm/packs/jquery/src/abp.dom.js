var abp = abp || {};
(function ($) {

    if (!$) {
        return;
    }

    abp.dom = abp.dom || {};

    abp.dom.onNodeAdded = function (callback) {
        abp.event.on('abp.dom.nodeAdded', callback);
    };

    abp.dom.onNodeRemoved = function (callback) {
        abp.event.on('abp.dom.nodeRemoved', callback);
    };

    $.fn.findWithSelf = function (selector) {
        return this.filter(selector).add(this.find(selector));
    };

    var mutationObserverCallback = function(mutationsList) {
        for (var i = 0; i < mutationsList.length; i++) {
            var mutation = mutationsList[i];
            if (mutation.type === 'childList') {
                if (mutation.addedNodes && mutation.removedNodes.length) {
                    for (var k = 0; k < mutation.removedNodes.length; k++) {
                        abp.event.trigger(
                            'abp.dom.nodeRemoved',
                            {
                                $el: $(mutation.removedNodes[k])
                            }
                        );
                    }
                }

                if (mutation.addedNodes && mutation.addedNodes.length) {
                    for (var j = 0; j < mutation.addedNodes.length; j++) {
                        abp.event.trigger(
                            'abp.dom.nodeAdded',
                            {
                                 $el: $(mutation.addedNodes[j])
                            }
                        );
                    }
                }
            }
        }
    };

    new MutationObserver(mutationObserverCallback).observe(
        $('body')[0],
        {
            subtree: true,
            childList: true
        }
    );

})(jQuery);