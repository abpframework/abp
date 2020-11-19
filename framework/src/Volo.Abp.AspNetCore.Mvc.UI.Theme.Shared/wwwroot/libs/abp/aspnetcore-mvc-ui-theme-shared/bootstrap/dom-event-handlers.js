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

    abp.dom.initializers.initializeAutocompleteSelects = function ($autocompleteSelects) {
        if ($autocompleteSelects.length) {
            $autocompleteSelects.each(function () {
                var $select = $(this);
                var url = $(this).data("autocompleteApiUrl");
                var displayName = $(this).data("autocompleteDisplayProperty");
                var displayValue = $(this).data("autocompleteValueProperty");
                var itemsPropertyName = $(this).data("autocompleteItemsProperty");
                var filterParamName = $(this).data("autocompleteFilterParamName");
                var selectedText = $(this).data("autocompleteSelectedItemName");
                var name = $(this).attr("name");
                var selectedTextInputName = name.substring(0, name.length - 1) + "_Text]";
                var selectedTextInput = $('<input>', {
                    type: 'hidden',
                    id: selectedTextInputName,
                    name: selectedTextInputName,
                });
                if (selectedText != "") {
                    selectedTextInput.val(selectedText);
                }
                selectedTextInput.insertAfter($select);
                $select.select2({
                    ajax: {
                        url: url,
                        dataType: "json",
                        data: function (params) {
                            var query = {};
                            query[filterParamName] = params.term;
                            return query;
                        },
                        processResults: function (data) {
                            var retVal = [];
                            var items = data;
                            if (itemsPropertyName) {
                                items = data[itemsPropertyName];
                            }

                            items.forEach(function (item, index) {
                                retVal.push({
                                    id: item[displayValue],
                                    text: item[displayName]
                                })
                            });
                            return {
                                results: retVal
                            };
                        }
                    },
                    width: '100%'
                });
                $select.on('select2:select', function (e) {
                    selectedTextInput.val(e.params.data.text);
                });
            });
        }
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
        getOptions: function ($input) { //$input may needed if developer wants to override this method
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
        abp.dom.initializers.initializeAutocompleteSelects(args.$el.findWithSelf('.auto-complete-select'));
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
        abp.dom.initializers.initializeAutocompleteSelects($('.auto-complete-select'));
        $('[data-auto-focus="true"]').first().findWithSelf('input,select').focus();

    });

})(jQuery);
