(function () {

    function triggerWidgets(eventName, startDate, endDate) {
        abp.event.trigger(
            eventName,
            {
                container: $('#MyDashboardWidgetsArea'),
                filters: {
                    startDate: startDate,
                    endDate: endDate
                }
            }
        );
    }

    $(function () {

        $('#DateRangePickerGlobalFilter').daterangepicker({
            "showDropdowns": true,
            opens: 'left'
        }, function (start, end, label) {
                triggerWidgets('refresh-widgets', start, end);
            });

        triggerWidgets('init-widgets',
            $('#DateRangePickerGlobalFilter').data('daterangepicker').startDate,
            $('#DateRangePickerGlobalFilter').data('daterangepicker').endDate);
    });
})();