(function ($) {
    
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

        var init = function () {
            $dashboardWrapper.find('.abp-widget-wrapper').each(function () {
                var widgetManager = new abp.WidgetManager({
                    wrapper: $(this),
                    filterCallback: opts.filterCallback || function () {
                        if (opts.filterForm) {
                            return opts.filterForm.serializeFormToObject();
                        }

                        return {};
                    }
                });

                widgetManager.init();
            });
        };

        var refresh = function () {
            $dashboardWrapper.find('.abp-widget-wrapper').each(function () {
                var widgetManager = $(this).data('abp-widget-manager');
                widgetManager && widgetManager.refresh();
            });
        };

        if (opts.filterForm) {
            opts.filterForm.submit(function (e) {
                e.preventDefault();
                refresh();
            });
        }

        return {
            init: init,
            refresh: refresh
        };
    };
})(jQuery);