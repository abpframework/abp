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
        for (var i = 0; i < $tooltips.length; i++) {
            new bootstrap.Tooltip($tooltips[i], {
                container: `body`
              });
        }
    }

    abp.dom.initializers.initializePopovers = function ($popovers) {
        for (var i = 0; i < $popovers.length; i++) {
            new bootstrap.Popover($popovers[i], {
                container: `body`
              });
        }
    }

    abp.dom.initializers.initializeTimeAgos = function ($timeagos) {
        $timeagos.timeago();
    }

    abp.dom.initializers.initializeAutocompleteSelects = function ($autocompleteSelects) {
        if ($autocompleteSelects.length) {
            $autocompleteSelects.each(function () {
                let $select = $(this);
                let url = $(this).data("autocompleteApiUrl");
                let displayName = $(this).data("autocompleteDisplayProperty");
                let displayValue = $(this).data("autocompleteValueProperty");
                let itemsPropertyName = $(this).data("autocompleteItemsProperty");
                let filterParamName = $(this).data("autocompleteFilterParamName");
                let selectedText = $(this).data("autocompleteSelectedItemName");
                let parentSelector = $(this).data("autocompleteParentSelector");
                let allowClear = $(this).data("autocompleteAllowClear");
                let placeholder = $(this).data("autocompletePlaceholder");
                if (allowClear && placeholder == undefined) {
                    placeholder = " ";
                }

                if (!parentSelector && $select.parents(".modal.fade").length === 1) {
                    parentSelector = ".modal.fade";
                }
                let name = $(this).attr("name");
                let selectedTextInputName = name + "_Text";
                if(name.indexOf(".ExtraProperties[") > 0) {
                    selectedTextInputName = name.substring(0, name.length - 1) + "_Text]"
                }
                let selectedTextInput = $('<input>', {
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
                        delay: 250,
                        dataType: "json",
                        data: function (params) {
                            let query = {};
                            query[filterParamName] = params.term;
                            return query;
                        },
                        processResults: function (data) {
                            let retVal = [];
                            let items = data;
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
                    width: '100%',
                    dropdownParent: parentSelector ? $(parentSelector) : $('body'),
                    allowClear: allowClear,
                    language: abp.localization.currentCulture.cultureName,
                    placeholder: {
                        id: '-1',
                        text: placeholder
                    }
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
            .findWithSelf('input.datepicker,input[type=date][abp-data-datepicker!=false]')
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

    abp.libs.bootstrapDateRangePicker = {
        packageName: "bootstrap-daterangepicker",

        createDateRangePicker: function (options) {
            options = options || {};
            options.singleDatePicker = false;
            return this.createDatePicker(options);
        },
        createSinglePicker: function (options) {
            options = options || {};
            options.singleDatePicker = true;
            return this.createDatePicker(options);
        },
        createDatePicker: function (options) {
            var $container = $('<div class="mb-3"></div>')
            var label = $('<label class="form-label"></label>')
            if (options.label) {
                label.text(options.label)
            }

            $container.append(label)
            var $datePicker = options.singleDatePicker ? $('<abp-date-picker></abp-date-picker>') : $('<abp-date-range-picker></abp-date-range-picker>');
            $container.append($datePicker)

            var $inputGroup = $('<div class="input-group"></div>');
            var $dateInput = $('<input type="text" autocomplete="off" class="form-control" />');

            if (options.placeholder) {
                $dateInput.attr('placeholder', options.placeholder)
            }

            if (options.value) {
                $dateInput.val(options.value)
            }

            if (options.name) {
                $dateInput.attr('name', options.name)
            }

            if (options.id) {
                $dateInput.attr('id', options.id)
            }

            if (options.required) {
                $dateInput.attr('required', true)
            }

            if (options.disabled) {
                $dateInput.attr('disabled', true)
            }

            if (options.readonly) {
                $dateInput.attr('readonly', true)
            }
            
            if(options.placeholder) {
                $dateInput.attr('placeholder', options.placeholder)
            }

            if (options.size) {
                switch (options.size) {
                    case 'Small':
                        $dateInput.addClass('form-control-sm')
                        break;
                    case 'Medium':
                        $dateInput.addClass('form-control-md')
                        break;
                    case 'Large':
                        $dateInput.addClass('form-control-lg')
                        break;
                    default:
                        break;
                }
            }

            $inputGroup.append($dateInput);

            if (options.openButton !== false) {
                var $openButton = $('<button type="button" class="btn btn-outline-secondary" tabindex="-1" data-type="open"><i class="fa fa-calendar"></i></button>');
                $inputGroup.append($openButton);
                if(options.disabled) {
                    $openButton.attr('disabled', 'disabled')
                }
            }

            if (options.clearButton !== false) {
                var $clearButton = $('<button type="button" class="btn btn-outline-secondary" tabindex="-1" data-type="clear"><i class="fa fa-times"></i></button>');
                $inputGroup.append($clearButton);
                if(options.disabled) {
                    $clearButton.attr('disabled', 'disabled')
                }
            }

            $datePicker.append($inputGroup);

            if (options.startDateName) {
                var $hiddenStartDateElement = $('<input type="hidden" data-start-date="true" name="' + options.startDateName + '" />');
                $datePicker.append($hiddenStartDateElement);
            }

            if (options.endDateName) {
                var $hiddenEndDateElement = $('<input type="hidden" data-end-date="true" name="' + options.endDateName + '" />');
                $datePicker.append($hiddenEndDateElement);
            }

            if (options.dateName) {
                var $hiddenDateElement = $('<input type="hidden" data-date="true" name="' + options.dateName + '" />');
                $datePicker.append($hiddenDateElement);
            }

            $datePicker.data('options', options);
            abp.dom.initializers.initializeDateRangePickers($datePicker);
            $container[0].datePicker = $datePicker[0];
            return $container;
        }
    };


    abp.dom.initializers.initializeDateRangePickers = function ($rootElement) {
        function setOptions (options, $datePickerElement, singleDatePicker) {

            options.singleDatePicker = singleDatePicker;

            var defaultOptions = {
                showDropdowns: true,
                opens: "center",
                drops: "down",
                autoApply: true,
                autoUpdateInput: false,
                showTodayButton: true,
                showClearButton: true,
                minYear: parseInt(moment().subtract(100, 'year').locale('en').format('YYYY')),
                maxYear: parseInt(moment().add(100, 'year').locale('en').format('YYYY')),
                locale: {
                    direction: abp.localization.currentCulture.isRightToLeft ? 'rtl' : 'ltr',
                    todayLabel: abp.localization.localize('Today', 'AbpUi'),
                    clearLabel: abp.localization.localize('Clear', 'AbpUi'),
                    applyLabel: abp.localization.localize('Apply', 'AbpUi'),
                },
                singleOpenAndClearButton: true
            };
            var locale = defaultOptions.locale;
            $.extend(options, defaultOptions);
            $.extend(options, $datePickerElement.data());
            if ($.isEmptyObject(options.locale)) {
                options.locale = locale;
            } else {
                locale = options.locale;
            }

            $.extend(options, $datePickerElement.data("options"));
            if ($.isEmptyObject(options.locale)) {
                options.locale = locale;
            }

            if (options.timePicker && options.timePicker24Hour === undefined) {
                options.timePicker24Hour = moment.localeData().longDateFormat('LT').indexOf('A') < 1;
            }

            if (options.dateFormat) {
                options.locale = options.locale || {};
                options.locale.format = options.dateFormat;
            }

            if (options.separator) {
                options.locale = options.locale || {};
                options.locale.separator = options.separator;
            }

            if (options.ranges) {
                $.each(options.ranges, function (key, value) {
                    let start = value[0];
                    let end;
                    if (value.length > 1) {
                        end = value[1];
                    } else {
                        end = value[0];
                    }
                    options.ranges[key] = [getMoment(start, options), getMoment(end, options)];
                });
            }

            if (typeof options.template !== 'string' && !(options.template instanceof $))
                options.template =
                    '<div class="daterangepicker">' +
                    '<div class="ranges"></div>' +
                    '<div class="drp-calendar left">' +
                    '<div class="calendar-table"></div>' +
                    '<div class="calendar-time"></div>' +
                    '</div>' +
                    '<div class="drp-calendar right">' +
                    '<div class="calendar-table"></div>' +
                    '<div class="calendar-time"></div>' +
                    '</div>' +
                    '<div class="drp-buttons">' +
                    '<button class="applyBtn" disabled="disabled" type="button"></button> ' +
                    '</div>' +
                    '</div>';
        }

        function replaceOpenButton (hasDate,singleOpenAndClearButton, $openButton, $clearButton) {
            var hiddenClass = 'd-none';

            if (singleOpenAndClearButton) {
                if (hasDate) {
                    $openButton.addClass(hiddenClass);
                    $clearButton.removeClass(hiddenClass);
                    $clearButton.insertAfter($openButton);
                } else {
                    $openButton.removeClass(hiddenClass);
                    $clearButton.addClass(hiddenClass);
                    $openButton.insertAfter($clearButton);
                }
            }
        }

        function getMoment (date, options, dateFormat) {
            var isUtc = options.isUtc;
            dateFormat = dateFormat || options.dateFormat;
            if (!date) {
                return isUtc ? moment.utc() : moment();
            }

            if (isUtc) {
                return moment.utc(date, dateFormat);
            } else {
                return moment(date, dateFormat);
            }
        }
        
        function getStartDate(options, startDate){
            startDate = startDate ? startDate : options.startDate;
            startDate = startDate ? getMoment(startDate, options) : null;
            if (options.singleDatePicker && !startDate) {
                startDate = options.date ? getMoment(options.date, options) : null;
            }
            
            if(startDate && startDate.isValid()){
                return startDate;
            }
            
            return null;
        }
        
        function getEndDate(options, endDate){
            if (options.singleDatePicker) {
                return null;
            }
            endDate = endDate ? endDate : options.endDate;
            endDate = endDate ? getMoment(endDate, options) : null;
            
            if(endDate && endDate.isValid()){
                return endDate;
            }
            
            return null;
        }
        
        function getTodayButton(options, $input){
            if (options.showTodayButton) {
                var $todayBtn = $('<button type="button" class="btn btn-sm today-btn" data-action="today">' + options.locale.todayLabel + '</button>');
                if(options.todayButtonClasses){
                    $todayBtn.addClass(options.todayButtonClasses);
                }else{
                    $todayBtn.addClass('btn-default');
                }

                $todayBtn.on('click', function () {
                    var today = getMoment(undefined, options);
                    $input.data('daterangepicker').setStartDate(today);
                    $input.data('daterangepicker').setEndDate(today);
                    $input.data('daterangepicker').clickApply();
                });
                
                return $todayBtn;
            }
            
            return null;
        }
        
        function getClearButton(options, $input, $dateRangePicker){
            if (options.showClearButton) {
                var $clearBtn = $('<button type="button" class="btn btn-sm clear-btn" data-action="clear">' + options.locale.clearLabel + '</button>');
                if(options.clearButtonClasses){
                    $clearBtn.addClass(options.clearButtonClasses);
                }else{
                    $clearBtn.addClass('btn-default');
                }
                $clearBtn.on('click', function () {
                    $input.val('').trigger('change');
                    $dateRangePicker.hide();
                });
                
                return $clearBtn;
            }
            return null;
        }
        
        function addExtraButtons(options, $dateRangePicker, $input) {
            var extraButtons = [getTodayButton(options, $input), getClearButton(options, $input, $dateRangePicker)];
            
            if ($dateRangePicker.container.hasClass('auto-apply')) {
                var $div = $('<div class="drp-buttons extra-buttons"></div>');
                $div.css('display', 'block');
                $.each(extraButtons, function (index, value) {
                    $div.prepend(value);
                });

                $div.insertAfter($dateRangePicker.container.find('.drp-buttons'));
            } else {
                $.each(extraButtons, function (index, value) {
                    $dateRangePicker.container.find('.drp-buttons').prepend(value);
                });
            }
        }
        
        function addOpenButtonClick($openButton, $dateRangePicker){
            if(!$openButton){
                return;
            }
            $dateRangePicker.outsideClick = function (e) {
                var target = $(e.target);
                // if the page is clicked anywhere except within the daterangerpicker/button itself then call this.hide()
                if (
                    // ie modal dialog fix
                    e.type == "focusin" ||
                    target.closest(this.element).length ||
                    target.closest(this.container).length ||
                    target.closest('.calendar-table').length ||
                    target.closest($openButton).length
                ) {
                    return;
                }

                this.hide();
                this.element.trigger('outsideClick.daterangepicker', this);
            };

            $openButton.on('click', function () {
                $dateRangePicker.toggle();
            });
        }

        function extendDateFormat (format,options) {
            if (options.timePicker) {
                if (options.timePicker24Hour) {
                    if (options.timePickerSeconds) {
                        format += " HH:mm:ss";
                    } else {
                        format += " HH:mm";
                    }
                } else {
                    if (options.timePickerSeconds) {
                        format += ' ' + " hh:mm:ss A"
                    } else {
                        format += " hh:mm A";
                    }
                }
            }

            return format;
        }
        
        function fillInput($input, startDate, endDate, options) {
            if (options.singleDatePicker) {
                if (startDate) {
                    $input.val(startDate.format(options.dateFormat));
                }
            } else {
                if (startDate && endDate) {
                    $input.val(startDate.format(options.dateFormat) + options.separator + endDate.format(options.dateFormat));
                }
            }
        }
        
        $rootElement
            .findWithSelf('abp-date-picker,abp-date-range-picker')
            .each(function () {
                var $this = $(this);
                var $input = $this.find('.input-group input[type="text"]')
                if ($input.data('daterangepicker')) {
                    return;
                }

                var $openButton = $this.find('button[data-type="open"]')
                var $clearButton = $this.find('button[data-type="clear"]')
                var singleDatePicker = $this.is('abp-date-picker')
                var options = {};
                
                setOptions(options, $this, singleDatePicker);

                var isIso = options.isIso;
                var dateFormat = options.dateFormat;
                var separator = options.separator;
                var defaultDateFormat  = extendDateFormat("YYYY-MM-DD", options);
                
                var singleOpenAndClearButton = options.singleOpenAndClearButton && $clearButton.length > 0 && $openButton.length > 0;
                
                var startDate = getStartDate(options);

                var endDate = getEndDate(options);
                if (startDate) {
                    options.startDate = startDate;
                }
                if (endDate) {
                    options.endDate = endDate;
                }

                $input.daterangepicker(options);

                var $dateRangePicker = $input.data('daterangepicker');

                addExtraButtons(options, $dateRangePicker, $input);

                addOpenButtonClick($openButton, $dateRangePicker);
                
                if (!dateFormat) {
                    dateFormat = extendDateFormat(moment.localeData().longDateFormat('L'), options);
                    options.dateFormat = dateFormat;
                }

                if (!separator) {
                    separator = $dateRangePicker.locale.separator;
                    options.separator = separator;
                }

                fillInput($input, startDate, endDate, options);
                
                $input.on('apply.daterangepicker', function (ev, picker) {
                    if (singleDatePicker) {
                        $(this).val(picker.startDate.format(dateFormat));
                    } else {
                        $(this).val(picker.startDate.format(dateFormat) + separator + picker.endDate.format(dateFormat));
                    }

                    $(this).trigger('change');
                });

                $input.on('cancel.daterangepicker', function (ev, picker) {
                    $(this).val('');
                    $(this).trigger('change');
                });

                $input.on('show.daterangepicker', function (ev, picker) {
                    var momentStartDate = getMoment(startDate, options);
                    var momentEndDate = getMoment(endDate, options);
                    if (momentStartDate.isValid()) {
                        picker.setStartDate(momentStartDate);
                        picker.setEndDate(momentEndDate);
                    }
                    if (momentEndDate.isValid() && !singleDatePicker) {
                        picker.setEndDate(momentEndDate);
                    }
                });

                $clearButton.on('click', function () {
                    $input.val('');
                    $input.trigger('change');
                });

                var firstTrigger = true;
                $input.on('change', function () {
                    if ($(this).val() !== '') {
                        replaceOpenButton(true, singleOpenAndClearButton, $openButton, $clearButton);
                    } else {
                        replaceOpenButton(false, singleOpenAndClearButton, $openButton, $clearButton);
                    }
                    var dates = $(this).val().split(separator);
                    if (dates.length === 2) {
                        startDate = formatDate(getStartDate(options, dates[0]));
                        endDate = formatDate(getEndDate(options, dates[1]));
                    } else {
                        if (dates[0]) {
                            startDate = formatDate(getStartDate(options, dates[0]));
                        }
                        else {
                            if(!firstTrigger){
                                startDate = null;
                            }
                        }

                        if(!firstTrigger){
                            endDate = null;
                        }
                    }

                    if (!startDate) {
                        replaceOpenButton(false, singleOpenAndClearButton, $openButton, $clearButton);
                        $(this).val('');
                    }

                    if (!singleDatePicker) {
                        var input1 = $this.find("input[data-start-date]")
                        input1.val(startDate);
                        var input2 = $this.find("input[data-end-date]")
                        input2.val(endDate);
                    } else {
                        var input = $this.find("input[data-date]")
                        input.val(startDate);
                    }

                    if (singleDatePicker) {
                        $this.data('date', startDate);
                        $input.data('date', startDate);
                    } else {
                        $this.data('startDate', startDate);
                        $this.data('endDate', endDate);
                        $input.data('startDate', startDate);
                        $input.data('endDate', endDate);
                    }
                    
                    firstTrigger = false;
                });

                function formatDate(date) {
                    if (date) {
                        if (isIso) {
                            return date.locale('en').format();
                        }
                        return date.locale('en').format(defaultDateFormat)
                    }

                    return null;
                }

                function getFormattedStartDate() {
                    if (startDate) {
                        return getMoment(startDate, options).format(dateFormat);
                    }

                    return null;
                }

                function getFormattedEndDate() {
                    if (endDate) {
                        return getMoment(endDate, options).format(dateFormat);
                    }

                    return null;
                }

                if (singleDatePicker) {
                    $this[0].getFormattedDate = getFormattedStartDate;
                    $input[0].getFormattedDate = getFormattedStartDate;
                    $dateRangePicker.getFormattedDate = getFormattedStartDate;
                } else {
                    $dateRangePicker.getFormattedStartDate = getFormattedStartDate;
                    $dateRangePicker.getFormattedEndDate = getFormattedEndDate;

                    $this[0].getFormattedStartDate = getFormattedStartDate;
                    $this[0].getFormattedEndDate = getFormattedEndDate;

                    $input[0].getFormattedStartDate = getFormattedStartDate;
                    $input[0].getFormattedEndDate = getFormattedEndDate;
                }

                $input.trigger('change');
            });
    }

    abp.dom.initializers.initializeAbpCspStyles =  function ($abpCspStyles){
        $abpCspStyles.attr("rel", "stylesheet");
    }

    abp.dom.onNodeAdded(function (args) {
        abp.dom.initializers.initializeToolTips(args.$el.findWithSelf('[data-bs-toggle="tooltip"]'));
        abp.dom.initializers.initializePopovers(args.$el.findWithSelf('[data-bs-toggle="popover"]'));
        abp.dom.initializers.initializeTimeAgos(args.$el.findWithSelf('.timeago'));
        abp.dom.initializers.initializeForms(args.$el.findWithSelf('form'), true);
        abp.dom.initializers.initializeScript(args.$el);
        abp.dom.initializers.initializeAutocompleteSelects(args.$el.findWithSelf('.auto-complete-select'));
        abp.dom.initializers.initializeAbpCspStyles($("link[abp-csp-style]"));
        abp.dom.initializers.initializeDateRangePickers(args.$el);
    });

    abp.dom.onNodeRemoved(function (args) {
        args.$el.findWithSelf('[data-bs-toggle="tooltip"]').each(function () {
            $('#' + $(this).attr('aria-describedby')).remove();
        });
    });

    abp.event.on('abp.configurationInitialized', function () {
        abp.libs.bootstrapDatepicker.normalizeLanguageConfig();
    });
    

    $(function () {
        abp.dom.initializers.initializeToolTips($('[data-bs-toggle="tooltip"]'));
        abp.dom.initializers.initializePopovers($('[data-bs-toggle="popover"]'));
        abp.dom.initializers.initializeTimeAgos($('.timeago'));
        abp.dom.initializers.initializeDatepickers($(document));
        abp.dom.initializers.initializeDateRangePickers($(document));
        abp.dom.initializers.initializeForms($('form'));
        abp.dom.initializers.initializeAutocompleteSelects($('.auto-complete-select'));
        $('[data-auto-focus="true"]').first().findWithSelf('input,select').focus();
        abp.dom.initializers.initializeAbpCspStyles($("link[abp-csp-style]"));
    });

})(jQuery);
