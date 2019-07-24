(function () {
    var serviceMethod = dashboardDemo.dashboard.getNewUserStatisticWidget;

    function ChartManager($wrapper) {

        var _filters;
        var _chart;

        var getFrequencyVal = function () {
            return $wrapper.find('.frequency-filter option:selected').val();
        };

        var createChart = function (statistic) {
            _chart = new Chart($wrapper.find('.NewUserStatisticChart'),
                {
                    type: 'bar',
                    data: {
                        labels: statistic.labels,
                        datasets: [
                            {
                                label: 'User count',
                                data: statistic.data,
                                backgroundColor: 'rgba(255, 132, 132, 1)'
                            }
                        ]
                    },
                    options: {
                        scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                        }
                    }
                });
        };

        var refreshChart = function (statistic) {
            _chart.data = {
                labels: statistic.labels,
                datasets: [
                    {
                        label: 'User count',
                        data: statistic.data,
                        backgroundColor: 'rgba(255, 132, 132, 1)'
                    }
                ]
            };
            _chart.update();
        };

        var render = function (args, callback) {
            serviceMethod({
                    startDate: _filters.startDate,
                    endDate: _filters.endDate,
                    frequency: getFrequencyVal()
                })
                .then(function (result) {
                    callback(result);
                });
        };

        var init = function (filters) {
            _filters = filters;
            render(filters, createChart);
        };

        var refresh = function (filters) {
            _filters = filters;
            render(filters, refreshChart);
        };

        $wrapper.find('.frequency-filter').on('change',
            function () {
                refresh(_filters);
            });

        return {
            init: init,
            refresh: refresh
        };
    }

    var init = function (args) {
        args.container
            .find('.newUserStatistic-widget')
            .each(function() {
                var $this = $(this);
                var chartManager = new ChartManager($this);
                chartManager.init(args.filters);
                $this.data('chart-manager', chartManager);
            });

    };

    var refresh = function (args) {
        args.container
            .find('.newUserStatistic-widget')
            .each(function () {
                var $this = $(this);
                var chartManager = $this.data('chart-manager');
                chartManager.refresh(args.filters);
            });
    };

    abp.event.on('init-widgets', init);
    abp.event.on('refresh-widgets', refresh);
})();