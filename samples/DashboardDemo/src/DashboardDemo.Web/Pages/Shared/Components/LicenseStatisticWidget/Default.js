(function () {
    abp.widgets.LicenseStatisticWidget = function($wrapper) {

        var _chart;

        var refreshChart = function(statistic) {
            _chart.data = {
                labels: Object.keys(statistic.data),
                datasets: [
                    {
                        label: 'License ratios',
                        data: Object.values(statistic.data),
                        backgroundColor: [
                            'rgba(50, 150, 255, 1)',
                            'rgba(150, 255, 100, 1)',
                            'rgba(255, 100, 150, 1)'
                        ]
                    }
                ]
            };
            _chart.update();
        };

        var render = function(filters, callback) {
            dashboardDemo.dashboard.getLicenseStatisticWidget({
                    startDate: filters.startDate,
                    endDate: filters.endDate
                })
                .then(function(result) {
                    callback(result);
                });
        };

        var refresh = function(filters) {
            render(filters, refreshChart);
        };

        var init = function (filters) {
            _chart = new Chart($wrapper.find('.LicenseStatisticChart'),
                {
                    type: 'pie'
                });

            refresh(filters);
        };

        return {
            init: init,
            refresh: refresh
        };
    };
})();