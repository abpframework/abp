(function() {
    function refresh(args) {
        args.container
            .find('.counters-widget')
            .each(function () {
                var $this = $(this);
                dashboardDemo.dashboard
                    .getCountersWidget({
                        startDate: args.filters.startDate,
                        endDate: args.filters.endDate
                    }).then(function (result) {
                        $this.find('.new-user-count').text(result.newUsers);
                        $this.find('.active-user-count').text(result.activeUsers);
                        $this.find('.total-income').text('$' + result.totalIncome.toFixed(2));
                        $this.find('.total-profit').text('$' + result.totalProfit.toFixed(2));
                    });
            });
    }

    abp.event.on('refresh-widgets', refresh);
})();