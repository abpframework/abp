abp.widgets = abp.widgets || {}; //TODO: Remove later
(function () {

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

        var getFilters = function() {
            if (!opts.filterCallback) {
                return {};
            }

            return opts.filterCallback();
        }
        
        var init = function () {
            $dashboardWrapper.find('.abp-widget-wrapper').each(function () {
                var $widgetWrapperDiv = $(this);
                var widgetName = $widgetWrapperDiv.attr('data-widget-name');
                var widgetApiClass = abp.widgets[widgetName];
                if (widgetApiClass) {
                    var widgetApi = new widgetApiClass($widgetWrapperDiv);
                    if (widgetApi.init) {
                        widgetApi.init(getFilters());
                    }
                    $widgetWrapperDiv.data('abp-widget-api', widgetApi);
                }
            });
        };

        var refresh = function() {
            $dashboardWrapper.find('.abp-widget-wrapper').each(function () {
                var $widgetWrapperDiv = $(this);
                var widgetApi = $widgetWrapperDiv.data('abp-widget-api');
                if (widgetApi && widgetApi.refresh) {
                    widgetApi.refresh(getFilters());
                }
            });
        }
        
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