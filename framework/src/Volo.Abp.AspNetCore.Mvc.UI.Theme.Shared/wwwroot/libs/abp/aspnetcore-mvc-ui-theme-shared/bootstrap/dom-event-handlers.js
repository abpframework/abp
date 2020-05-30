(function ($) {

    abp.dom = abp.dom || {};
    
    function initializeForms($forms, validate) {
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
    
    function initializeToolTips($tooltips){
        $tooltips.tooltip({
            container: 'body'
        });
    }

    function initializePopovers($popovers){
        $popovers.popover({
            container: 'body'
        });
    }

    function initializeTimeAgos($timeagos){
        $timeagos.timeago();
    }

    abp.dom.onNodeAdded(function (args) {
        initializeToolTips(args.$el.findWithSelf('[data-toggle="tooltip"]'));
        initializePopovers(args.$el.findWithSelf('[data-toggle="popover"]'));
        initializeTimeAgos(args.$el.findWithSelf('.timeago'));
        initializeForms(args.$el.findWithSelf('form'), true);
        initializeScript(args.$el);
    });

    abp.dom.onNodeRemoved(function (args) {
        args.$el.findWithSelf('[data-toggle="tooltip"]').each(function () {
            $('#' + $(this).attr('aria-describedby')).remove();
        });
    });

    $(function () {
        initializeToolTips($('[data-toggle="tooltip"]'));
        initializePopovers($('[data-toggle="popover"]'));
        initializeTimeAgos($('.timeago'));
        initializeForms($('form'));
        $('[data-auto-focus="true"]').first().findWithSelf('input,select').focus();
    });

})(jQuery);