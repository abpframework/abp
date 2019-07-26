(function ($) {
    abp.widgets = abp.widgets || {};

    abp.WidgetManager = function (opts) {
        if (!opts) {
            opts = {};
        } else if (typeof opts === 'string') {
            opts = {
                wrapper: opts
            };
        }

        if (!opts.wrapper) {
            opts.wrapper = $('body');
        } else if (typeof opts.wrapper === 'string') {
            opts.wrapper = $(opts.wrapper);
        }

        var getFilters = function ($widgetWrapperDiv) {
            var filters = {};
            
            if (opts.filterForm) {
                filters = opts.filterForm.serializeFormToObject();
            }

            if (opts.filterCallback) {
                filters = $.extend(filters, opts.filterCallback());
            }

            var widgetApi = $widgetWrapperDiv.data('abp-widget-api');
            if (widgetApi && widgetApi.getFilters) {
                filters = $.extend(filters, widgetApi.getFilters());
            }

            return filters;
        };

        var init = function () {
            opts.wrapper.find('.abp-widget-wrapper').each(function () {
                var $widgetWrapperDiv = $(this);
                var widgetName = $widgetWrapperDiv.attr('data-widget-name');
                var widgetApiClass = abp.widgets[widgetName];
                if (widgetApiClass) {
                    var widgetApi = new widgetApiClass($widgetWrapperDiv);
                    $widgetWrapperDiv.data('abp-widget-api', widgetApi);
                    if (widgetApi.init) {
                        widgetApi.init(getFilters($widgetWrapperDiv));
                    }
                }
            });
        };

        var refresh = function () {
            opts.wrapper.find('.abp-widget-wrapper').each(function () {
                var $widgetWrapperDiv = $(this);

                var refreshUrl = $widgetWrapperDiv.attr('data-refresh-url');
                if (refreshUrl) {
                    abp.ajax({
                        url: refreshUrl,
                        type: 'GET',
                        dataType: 'html',
                        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                        data: getFilters($widgetWrapperDiv)
                    }).then(function (result) {
                        $widgetWrapperDiv.replaceWith($(result));
                    });
                } else {
                    var widgetApi = $widgetWrapperDiv.data('abp-widget-api');
                    if (widgetApi && widgetApi.refresh) {
                        widgetApi.refresh(getFilters($widgetWrapperDiv));
                    }
                }
            });
        };

        if (opts.filterForm) {
            opts.filterForm.submit(function (e) {
                e.preventDefault();
                refresh();
            });
        }

        return  {
            init: init,
            refresh: refresh
        };
    };
})(jQuery);