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
                    $form.submit(function (e) {
                        if (!$form.data('abp-confirmed')) {
                            e.preventDefault();
                            abp.message.confirm(confirmText).done(function (accepted) {
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

    function initializeScript($el) {
        $el.findWithSelf('[data-script-class]').each(function () {
            var scriptClassName = $(this).attr('data-script-class');
            if (!scriptClassName) {
                return;
            }

            var scriptClass = eval(scriptClassName);
            if (!scriptClass) {
                return;
            }

            var scriptObject = new scriptClass();
            $el.data('abp-script-object', scriptObject);

            scriptObject.initDom && scriptObject.initDom($el);
        });
    }

    abp.dom.onNodeAdded(function (args) {
        args.$el.findWithSelf('[data-toggle="tooltip"]').tooltip({
            container: 'body'
        });

        args.$el.findWithSelf('[data-toggle="popover"]').popover({
            container: 'body'
        });

        enableFormFeatures(args.$el.findWithSelf('form'), true);

        initializeScript(args.$el);
    });

    abp.dom.onNodeRemoved(function (args) {
        args.$el.findWithSelf('[data-toggle="tooltip"]').each(function () {
            $('#' + $(this).attr('aria-describedby')).remove();
        });
    });

    $(function () {
        enableFormFeatures($('form'));

        $('[data-toggle="tooltip"]').tooltip({
            container: 'body'
        });

        $('[data-toggle="popover"]').popover({
            container: 'body'
        });

        $('[data-auto-focus="true"]').first().findWithSelf('input').focus();
    });

})(jQuery);