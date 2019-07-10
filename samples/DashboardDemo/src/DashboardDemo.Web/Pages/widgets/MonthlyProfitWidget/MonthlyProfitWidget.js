(function ($) {
    var $container = $('#MonthlyProfitWidgetContainer');
    if ($container.length > 0) {
        var chart = {};
        var $RefreshGlobalFilterContainer = $("#RefreshGlobalFilterContainer");

        var createChart = function () {
            dashboardDemo.demoStatistic.getMonthlyProfitStatistic().then(function (result) {

                chart = new Chart($container.find('#MonthlyProfitStatistics'), {
                    type: 'line',
                    data: {
                        labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', "Jul", "Aug", "Sept", "Oct", "Nov", "Dec"],
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

        if ($RefreshGlobalFilterContainer.length > 0) {
            $RefreshGlobalFilterContainer.find('#GlobalRefreshButton').on('click',
                function () {
                    dashboardDemo.demoStatistic.getMonthlyProfitStatistic().then(function (result) {
                        chart.data.datasets[0].data = result.data;
                        chart.update();
                    });
                });
        }

        createChart();
    }
})(jQuery);
