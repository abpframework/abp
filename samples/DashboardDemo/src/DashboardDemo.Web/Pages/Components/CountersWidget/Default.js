abp.widgets = abp.widgets || {}; //TODO: Remove later
(function () {
    abp.widgets.CountersWidget = function ($wrapper) {

        var refresh = function (filters) {
            dashboardDemo.dashboard.getCountersWidget({
                startDate: filters.startDate,
                endDate: filters.endDate
            }).then(function (result) {
                $wrapper.find('.new-user-count').text(result.newUsers);
                $wrapper.find('.active-user-count').text(result.activeUsers);
                $wrapper.find('.total-income').text('$' + result.totalIncome.toFixed(2));
                $wrapper.find('.total-profit').text('$' + result.totalProfit.toFixed(2));
            });
        };

        var init = function (filters) {
            // there is no initial process for this widget.
        };

        return {
            init: init,
            refresh: refresh
        };
    };
})();