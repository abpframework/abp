(function ($) {

    function enableFormFeatures($forms, validate) {
        if ($forms.length) {
            $forms.each(function () {
                var $form = $(this);

                if (validate === true) {
                    $.validator.unobtrusive.parse($form);
                }

                var confirmText = $form.attr('data-confirm');
                if (confirmText) {
                    $form.submit(function(e) {
                        if (!$form.data('abp-confirmed')) {
                            e.preventDefault();
                            abp.message.confirm(confirmText).done(function(accepted) {
                                if (accepted) {
                                    $form.data('abp-confirmed', true);
                                    $form.submit();
                                    $form.data('abp-confirmed', undefined);
                                }
                            });
                        }
                    });
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

        enableFormFeatures(args.$el.findWithSelf('form'), true);
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

        enableFormFeatures($('form'));
    });

})(jQuery);