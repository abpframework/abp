(function ($) {
    var $container = $('#UserCountWidgetContainer');
    if ($container.length > 0) {

        var $DateRangeGlobalFilterContainer = $("#DateRangeGlobalFilterContainer");


        var createChart = function () {
            dashboardDemo.demoStatistic.getNewUserPerDayStatistic().then(function (result) {

                var chart = new Chart($container.find('#UserStatistics'), {
                    type: 'bar',
                    data: {
                        labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', "Sun"],
                        datasets: [{
                            label: 'Avarage new user per day',
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

        if ($DateRangeGlobalFilterContainer.length > 0) {
            $DateRangeGlobalFilterContainer.find('#GlobalRefreshButton').on('click',
                function () {
                    createChart();
                });
        }

        createChart();
    }
})(jQuery);
