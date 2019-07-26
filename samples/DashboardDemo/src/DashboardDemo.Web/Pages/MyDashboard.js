(function () {

    var myDashboard = new abp.WidgetManager({
        wrapper: '#MyDashboardWidgetsArea',
        filterForm: $('#MyDashboardFilterForm')
    });

    myDashboard.init();
})();