(function($) {
    
    abp.dom.onNodeAdded(function (args) {
        args.$el.findWithSelf('[data-toggle="tooltip"]').tooltip({
            container: 'body'
        });

        args.$el.findWithSelf('[data-toggle="popover"]').popover({
            container: 'body'
        });

        var $forms = args.$el.findWithSelf('form');
        if ($forms.length) {
            $forms.each(function() {
                $.validator.unobtrusive.parse($(this));
            });
        }
    });

    abp.dom.onNodeRemoved(function (args) {
        args.$el.findWithSelf('[data-toggle="tooltip"]').each(function () {
            $('#' + $(this).attr('aria-describedby')).remove();
        });
    });

    $(function () {
        $('[data-toggle="tooltip"]').tooltip({
            container: 'body'
        });

        $('[data-toggle="popover"]').popover({
            container: 'body'
        });
    });

})(jQuery);