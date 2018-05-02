(function ($) {

    function enableAjaxForm($forms, validate) {
        if ($forms.length) {
            $forms.each(function () {
                var $form = $(this);

                if (validate === true) {
                    $.validator.unobtrusive.parse($form);
                }

                if ($form.attr('data-ajaxForm') === 'true') {
                    $form.abpAjaxForm();
                }
            });
        }
    }

    abp.dom.onNodeAdded(function (args) {
        args.$el.findWithSelf('[data-toggle="tooltip"]').tooltip({
            container: 'body'
        });

        args.$el.findWithSelf('[data-toggle="popover"]').popover({
            container: 'body'
        });

        enableAjaxForm(args.$el.findWithSelf('form'), true);
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

        enableAjaxForm($('form'));
    });

})(jQuery);