(function ($) {
    var $container = $('#UserCountWidgetContainer');
    if ($container.length > 0) {

        var _identityUserAppService = volo.abp.identity.identityUser;
        _identityUserAppService.getList({}).then(function (result) {
            $container.find('#UserCount').text(result.items.length);
        });

        var createChart = function() {
            dashboardDemo.userStatistic.getNewUserPerDayStatistic().then(function (result) {
                console.log(result.data);

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

        createChart();
    }
})(jQuery);
