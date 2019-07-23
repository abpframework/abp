(function () {
    var chart;
    var serviceMethod = dashboardDemo.dashboard.getNewUserStatisticWidget;
    var latestEndDate;
    var latestStartDate;

    var getFrequencyVal = function() {
        return $("#FrequencyFilter option:selected").val();
    };

    var createChart = function (statistic) {
        chart = new Chart($('#NewUserStatisticChart'),
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
        chart.data = {
            labels: statistic.labels,
            datasets: [
                {
                    label: 'User count',
                    data: statistic.data,
                    backgroundColor: 'rgba(255, 132, 132, 1)'
                }
            ]
        };
        chart.update();
    };

    var init = function (args) {
        serviceMethod({
            startDate: args.filters.startDate,
            endDate: args.filters.endDate,
            frequency: getFrequencyVal()
        })
            .then(function (result) {
                createChart(result);
            });

        latestStartDate = args.filters.startDate;
        latestEndDate = args.filters.endDate;
    };

    var refresh = function (args) {

        serviceMethod({
            startDate: args.filters.startDate,
            endDate: args.filters.endDate,
            frequency: getFrequencyVal()
        })
            .then(function (result) {
                refreshChart(result);
            });

        latestStartDate = args.filters.startDate;
        latestEndDate = args.filters.endDate;
    };

    $('#FrequencyFilter').on('change',
        function () {
            refresh({
                filters: {
                    startDate: latestStartDate,
                    endDate: latestEndDate
                }
            });
        });

    abp.event.on('refresh-widgets', refresh);

    abp.event.on('init-widgets', init);
})();