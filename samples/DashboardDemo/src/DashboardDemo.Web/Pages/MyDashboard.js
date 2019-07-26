(function () {

    var myDashboard = new abp.DashboardManager({
        wrapper: '#MyDashboardWidgetsArea',
        filterForm: $('#MyDashboardFilterForm')
    });

    myDashboard.init();
})();