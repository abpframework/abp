(function () {

    function triggerWidgets(eventName) {
        abp.event.trigger(
            eventName,
            {
                container: $('#MyDashboardWidgetsArea'),
                filters: {
                    startDate: $('#StartDate').val(),
                    endDate: $('#EndDate').val()
                }
            }
        );
    }

    $(function () {

        $('#MyDashboardFilterForm').submit(function(e) {
            e.preventDefault();

            triggerWidgets('refresh-widgets');
        });

        triggerWidgets('init-widgets');
    });
})();