(function ($) {
    var $container = $('#UserCountWidgetContainer');
    if ($container.length > 0) {
        var chart = {};

        var createChart = function () {
            dashboardDemo.demoStatistic.getMonthlyUserStatistic({}).then(function (result) {
                chart = new Chart($container.find('#UserStatistics'), {
                    type: 'bar',
                    data: {
                        labels: result.labels,
                        datasets: [{
                            label: 'Monthly user count',
                            data: result.data,
                            backgroundColor: 'rgba(255, 99, 132, 0.2)'
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
                    dashboardDemo.demoStatistic.getMonthlyUserStatistic({ startDate: filters.startDate, endDate: filters.endDate }).then(function (result) {
                        chart.data= {
                            labels: result.labels,
                                datasets: [{
                                label: 'Monthly user count',
                                data: result.data,
                                backgroundColor: 'rgba(255, 99, 132, 0.2)'
                            }]
                        },
                        chart.update();
                    });
                });

        createChart();
    }
})(jQuery);
