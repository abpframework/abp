(function ($) {
        $('#GlobalRefreshButton').on('click',
            function () {
                $(document).trigger('RefreshWidgets', $('#DashboardGlobalFiltersForm').serializeFormToObject());
            });
})(jQuery);