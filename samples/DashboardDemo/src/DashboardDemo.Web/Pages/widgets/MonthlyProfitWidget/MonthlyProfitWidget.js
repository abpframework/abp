(function ($) {
    var $container = $('#MonthlyProfitWidgetContainer');
    if ($container.length > 0) {
        var chart = {};

        var createChart = function () {
            dashboardDemo.demoStatistic.getMonthlyProfitStatistic({}).then(function (result) {
                chart = new Chart($container.find('#MonthlyProfitStatistics'), {
                    type: 'line',
                    data: {
                        labels: result.labels,
                        datasets: [{
                            label: 'Monthly Profit',
                            data: result.data,
                            backgroundColor: 'rgba(255, 255, 132, 0.2)'
                        }]
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
            });
        };

        $(document).on('RefreshWidgets',
            function (event, filters) {
                dashboardDemo.demoStatistic.getMonthlyProfitStatistic({ startDate: filters.startDate, endDate: filters.endDate }).then(function (result) {
                    chart.data = {
                        labels: result.labels,
                            datasets: [{
                            label: 'Monthly Profit',
                            data: result.data,
                            backgroundColor: 'rgba(255, 255, 132, 0.2)'
                        }]
                    },
                    chart.update();
                });
            });

        createChart();
    }
})(jQuery);
