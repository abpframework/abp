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
    
    function initializeToolTip($tooltips){
        $tooltips.tooltip({
            container: 'body'
        });
    }

    function initializePopover($popovers){
        $popovers.popover({
            container: 'body'
        });
    }

    function initializeTimeAgo($timeagos){
        $timeagos.timeago();
    }

    abp.dom.onNodeAdded(function (args) {
        initializeToolTip(args.$el.findWithSelf('[data-toggle="tooltip"]'));
        initializePopover(args.$el.findWithSelf('[data-toggle="popover"]'));
        initializeTimeAgo(args.$el.findWithSelf('.timeago'));
        enableFormFeatures(args.$el.findWithSelf('form'), true);
        initializeScript(args.$el);
    });

    abp.dom.onNodeRemoved(function (args) {
        args.$el.findWithSelf('[data-toggle="tooltip"]').each(function () {
            $('#' + $(this).attr('aria-describedby')).remove();
        });
    });

    $(function () {
        initializeToolTip($('[data-toggle="tooltip"]'));
        initializePopover($('[data-toggle="popover"]'));
        initializeTimeAgo($('.timeago'));
        enableFormFeatures($('form'));
        $('[data-auto-focus="true"]').first().findWithSelf('input,select').focus();
    });

})(jQuery);