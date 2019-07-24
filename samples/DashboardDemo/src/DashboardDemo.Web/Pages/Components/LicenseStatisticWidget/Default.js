(function () {
    var serviceMethod = dashboardDemo.dashboard.getLicenseStatisticWidget;

    function ChartManager($wrapper) {

        var _chart;

        var createChart = function (statistic) {
            _chart = new Chart($wrapper.find('.LicenseStatisticChart'),
                {
                    type: 'pie',
                    data: {
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
                    }
                });
        };

        var refreshChart = function (statistic) {
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

        var render = function (filters, callback) {
            serviceMethod({
                startDate: filters.startDate,
                endDate: filters.endDate
                })
                .then(function (result) {
                    callback(result);
                });
        };

        var init = function (filters) {
            render(filters, createChart);
        };

        var refresh = function (filters) {
            render(filters, refreshChart);
        };

        return {
            init: init,
            refresh: refresh
        };
    }

    abp.event.on('init-widgets', function (args) {
        args.container
            .find('.license-statistic-widget')
            .each(function () {
                var $this = $(this);
                var chartManager = new ChartManager($this);
                chartManager.init(args.filters);
                $this.data('chart-manager', chartManager);
            });

    });

    abp.event.on('refresh-widgets', function (args) {
        args.container
            .find('.license-statistic-widget')
            .each(function () {
                var $this = $(this);
                var chartManager = $this.data('chart-manager');
                chartManager.refresh(args.filters);
            });
    });
})();