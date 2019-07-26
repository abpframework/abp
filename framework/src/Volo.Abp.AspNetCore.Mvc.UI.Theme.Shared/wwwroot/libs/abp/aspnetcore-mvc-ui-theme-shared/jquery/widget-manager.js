(function ($) {
    abp.widgets = abp.widgets || {};

    abp.WidgetManager = function (opts) {
        if (typeof opts === 'string') {
            opts = {
                wrapper: opts
            };
        }

        var $widgetWrapperDiv;
        if (typeof opts.wrapper === 'string') {
            $widgetWrapperDiv = $(opts.wrapper);
        } else {
            $widgetWrapperDiv = opts.wrapper;
        }

        if (!$widgetWrapperDiv.hasClass('abp-widget-wrapper')) {
            $widgetWrapperDiv = $widgetWrapperDiv.closest('.abp-widget-wrapper');
        }

        var getFilters = function () {
            var filters = {};

            if (opts.filterCallback) {
                filters = opts.filterCallback();
            }

            var widgetApi = $widgetWrapperDiv.data('abp-widget-api');
            if (widgetApi && widgetApi.getFilters) {
                filters = $.extend(filters, widgetApi.getFilters());
            }

            return filters;
        };

        var init = function () {
            var widgetName = $widgetWrapperDiv.attr('data-widget-name');
            var widgetApiClass = abp.widgets[widgetName];
            if (widgetApiClass) {
                var widgetApi = new widgetApiClass($widgetWrapperDiv);
                $widgetWrapperDiv.data('abp-widget-api', widgetApi);
                if (widgetApi.init) {
                    widgetApi.init(getFilters($widgetWrapperDiv));
                }
            }
        };

        var callRefreshWidgetApi = function () {
            var widgetApi = $widgetWrapperDiv.data('abp-widget-api');
            if (widgetApi && widgetApi.refresh) {
                widgetApi.refresh(getFilters($widgetWrapperDiv));
            }
        }

        var refresh = function () {
            var refreshUrl = $widgetWrapperDiv.attr('data-refresh-url');
            if (refreshUrl) {
                abp.ajax({
                    url: refreshUrl,
                    type: 'GET',
                    dataType: 'html',
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    data: getFilters()
                }).then(function (result) {
                    var $newWidgetWrapperDiv = $(result);
                    $widgetWrapperDiv.replaceWith($newWidgetWrapperDiv);
                    $widgetWrapperDiv = $newWidgetWrapperDiv;
                    $widgetWrapperDiv.data('abp-widget-manager', publicApi);
                    init();
                });
            } else {
                callRefreshWidgetApi();
            }
        };

        var publicApi = {
            init: init,
            refresh: refresh
        };

        $widgetWrapperDiv.data('abp-widget-manager', publicApi);

        return publicApi;
    };
})(jQuery);