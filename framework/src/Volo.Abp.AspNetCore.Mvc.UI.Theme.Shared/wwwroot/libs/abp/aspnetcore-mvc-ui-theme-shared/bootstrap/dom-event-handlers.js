(function ($) {

    abp.dom = abp.dom || {};

    abp.dom.initializers = abp.dom.initializers || {};

    abp.dom.initializers.initializeForms = function ($forms, validate) {
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
    };

    abp.dom.initializers.initializeScript = function ($el) {
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

    abp.dom.initializers.initializeToolTips = function ($tooltips) {
        $tooltips.tooltip({
            container: 'body'
        });
    }

    abp.dom.initializers.initializePopovers = function ($popovers) {
        $popovers.popover({
            container: 'body'
        });
    }

    abp.dom.initializers.initializeTimeAgos = function ($timeagos) {
        $timeagos.timeago();
    }

    abp.libs = abp.libs = abp.libs || {};
    abp.libs.bootstrapDatepicker = {
        packageName: "bootstrap-datepicker",
        normalizeLanguageConfig: function () {
            var language = abp.localization.getLanguagesMap(this.packageName);
            var languageConfig = $.fn.datepicker.dates[language];
            if (languageConfig && (!languageConfig.format || language !== abp.localization.currentCulture.name)) {
                languageConfig.format = abp.localization.currentCulture.dateTimeFormat.shortDatePattern.toLowerCase();
            }
        },
        getFormattedValue: function (isoFormattedValue) {
            if (!isoFormattedValue) {
                return isoFormattedValue;
            }
            return luxon
                .DateTime
                .fromISO(isoFormattedValue, {
                    locale: abp.localization.currentCulture.name
                }).toLocaleString();
        },
        getOptions: function($input) { //$input may needed if developer wants to override this method
            return {
                todayBtn: "linked",
                autoclose: true,
                language: abp.localization.getLanguagesMap(this.packageName)
            };
        }
    };

    abp.dom.initializers.initializeDatepickers = function ($rootElement) {
        $rootElement
            .findWithSelf('input.datepicker,input[type=date]')
            .each(function () {
                var $input = $(this);
                $input
                    .attr('type', 'text')
                    .val(abp.libs.bootstrapDatepicker.getFormattedValue($input.val()))
                    .datepicker(abp.libs.bootstrapDatepicker.getOptions($input))
                    .on('hide', function (e) {
                        e.stopPropagation();
                    });
            });
    }

    abp.dom.onNodeAdded(function (args) {
        abp.dom.initializers.initializeToolTips(args.$el.findWithSelf('[data-toggle="tooltip"]'));
        abp.dom.initializers.initializePopovers(args.$el.findWithSelf('[data-toggle="popover"]'));
        abp.dom.initializers.initializeTimeAgos(args.$el.findWithSelf('.timeago'));
        abp.dom.initializers.initializeForms(args.$el.findWithSelf('form'), true);
        abp.dom.initializers.initializeScript(args.$el);
    });

    abp.dom.onNodeRemoved(function (args) {
        args.$el.findWithSelf('[data-toggle="tooltip"]').each(function () {
            $('#' + $(this).attr('aria-describedby')).remove();
        });
    });

    abp.event.on('abp.configurationInitialized', function () {
        abp.libs.bootstrapDatepicker.normalizeLanguageConfig();
    });

    $(function () {
        abp.dom.initializers.initializeToolTips($('[data-toggle="tooltip"]'));
        abp.dom.initializers.initializePopovers($('[data-toggle="popover"]'));
        abp.dom.initializers.initializeTimeAgos($('.timeago'));
        abp.dom.initializers.initializeDatepickers($(document));
        abp.dom.initializers.initializeForms($('form'));
        $('[data-auto-focus="true"]').first().findWithSelf('input,select').focus();
    });

})(jQuery);
