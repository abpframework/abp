abp.widgets = abp.widgets || {}; //TODO: Remove later
(function () {

    abp.WidgetManager = function(opts) {

    };

    abp.DashboardManager = function (opts) {

        if (typeof opts === 'string') {
            opts = {
                wrapper: opts
            };
        }

        var $dashboardWrapper;
        if (typeof opts.wrapper === 'string') {
            $dashboardWrapper = $(opts.wrapper);
        } else {
            $dashboardWrapper = opts.wrapper;
        }

        var getFilters = function ($widgetWrapperDiv) {
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

        var initWidget = function($widgetWrapperDiv) {
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

        var callRefreshWidgetApi = function ($widgetWrapperDiv) {
            var widgetApi = $widgetWrapperDiv.data('abp-widget-api');
            if (widgetApi && widgetApi.refresh) {
                widgetApi.refresh(getFilters($widgetWrapperDiv));
            }
        }

        var refreshWidget = function ($widgetWrapperDiv) {
            var refreshUrl = $widgetWrapperDiv.attr('data-refresh-url');
            if (refreshUrl) {
                abp.ajax({
                    url: refreshUrl,
                    type: 'GET',
                    dataType: 'html',
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    data: getFilters($widgetWrapperDiv)
                }).then(function (result) {
                    var $newWidgetWrapperDiv = $(result);
                    $widgetWrapperDiv.replaceWith($newWidgetWrapperDiv);
                    $widgetWrapperDiv = $newWidgetWrapperDiv;
                    initWidget($widgetWrapperDiv);
                });
            } else {
                callRefreshWidgetApi($widgetWrapperDiv);
            }
        };

        var init = function () {
            $dashboardWrapper.find('.abp-widget-wrapper').each(function () {
                initWidget($(this));
            });
        };

        var refresh = function() {
            $dashboardWrapper.find('.abp-widget-wrapper').each(function() {
                refreshWidget($(this));
            });
        };
        
        return {
            init: init,
            refresh: refresh
        };
    };

    var filterForm = $('#MyDashboardFilterForm');

    var myDashboard = new abp.DashboardManager({
        wrapper: '#MyDashboardWidgetsArea',
        filterCallback: function() {
            return filterForm.serializeFormToObject();
        }
    });

    myDashboard.init();

    filterForm.submit(function(e) {
        e.preventDefault();
        myDashboard.refresh();
    });
})();