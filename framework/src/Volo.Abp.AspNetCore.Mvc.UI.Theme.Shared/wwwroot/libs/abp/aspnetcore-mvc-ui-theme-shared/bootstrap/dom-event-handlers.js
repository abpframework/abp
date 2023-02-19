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
            }

            if (options.clearButton !== false) {
                var $clearButton = $('<button type="button" class="btn btn-outline-secondary" tabindex="-1" data-type="clear"><i class="fa fa-times"></i></button>');
                $inputGroup.append($clearButton);
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

            $container.data('options', options);
            if($container.data('options', options)){
                debugger;
            }
            abp.dom.initializers.initializeDateRangePickers($container);
            return $container;
        }
    };
            
    
    abp.dom.initializers.initializeDateRangePickers = function ($rootElement) {
        $rootElement
            .findWithSelf('abp-date-picker,abp-date-range-picker')
            .each(function () {
                var $this = $(this);
                var $input = $(this).find('.input-group input[type="text"]')
                if($input.data('daterangepicker')) {
                    return;
                }
                var $openButton = $(this).find('button[data-type="open"]')
                var $clearButton = $(this).find('button[data-type="clear"]')
                var singleDatePicker = $this.is('abp-date-picker')
                var options = {};

                var defaultOptions = {
                    showDropdowns: true,
                    opens: "center",
                    drops: "down",
                    autoApply: true,
                    autoUpdateInput: false
                };
                
                $.extend(options, defaultOptions);
                $.extend(options, $this.data());
                if($this.data("options")){
                    debugger;
                    $.extend(options, $this.data("options"));
                }
                $.extend(options, $this.data("options"));

                var isUtc = options.isUtc;
                var isIso = options.isIso;
                var timePicker = options.timePicker;
                var timePicker24Hour = options.timePicker24Hour;
                var timePickerSeconds = options.timePickerSeconds;
                var dateFormat = options.dateFormat;
                var separator = options.separator;
                const getMoment = function (date) {
                    if(!date){
                        return isUtc ? moment.utc() : moment();
                    }
                    if(isUtc) {
                        return moment.utc(date,dateFormat);
                    }else {
                        return moment(date,dateFormat);
                    }
                }
                
                if (dateFormat) {
                    options.locale = options.locale || {};
                    options.locale.format = dateFormat;
                }

                if (separator) {
                    options.locale = options.locale || {};
                    options.locale.separator = separator;
                }

                var startDate = options.startDate ? getMoment(options.startDate) : null;
                if(singleDatePicker && !startDate){
                    startDate = options.date ? getMoment(options.date) : null;
                }
                var endDate = options.endDate ? getMoment(options.endDate) : null;
                
                if (startDate) {
                    options.startDate = startDate;
                }
                if (endDate) {
                    options.endDate = endDate;
                }
                
                if(options.ranges){
                    $.each(options.ranges, function (key, value) {
                        let start = value[0];
                        let end;
                        if(value.length > 1){
                            end = value[1];
                        }else{
                            end = value[0];
                        }
                        options.ranges[key] = [getMoment(start), getMoment(end)];
                    });
                }

                $input.daterangepicker(options);
                var $dateRangePicker = $input.data('daterangepicker');
                
                $dateRangePicker.outsideClick = function(e) {
                    var target = $(e.target);
                    // if the page is clicked anywhere except within the daterangerpicker/button
                    // itself then call this.hide()
                    if (
                        // ie modal dialog fix
                        e.type == "focusin" ||
                        target.closest(this.element).length ||
                        target.closest(this.container).length ||
                        target.closest('.calendar-table').length ||
                        target.closest($openButton).length
                    ) return;
                    this.hide();
                    this.element.trigger('outsideClick.daterangepicker', this);
                };
                
                $openButton.on('click', function () {
                    $dateRangePicker.toggle();
                });
                
                
                if(!dateFormat) {
                    if(timePicker){
                        if(timePicker24Hour){
                            if(timePickerSeconds){
                                dateFormat = moment.localeData().longDateFormat('L') + " HH:mm:ss";
                            }else{
                                dateFormat = moment.localeData().longDateFormat('L') + " HH:mm";
                            }
                        }else {
                            if (timePickerSeconds) {
                                dateFormat = moment.localeData().longDateFormat('L') + ' ' + " hh:mm:ss A"
                            } else {
                                dateFormat = moment.localeData().longDateFormat('L') + " hh:mm A";
                            }
                        }
                    }else{
                        dateFormat = moment.localeData().longDateFormat('L');
                    }
                }
                
                
                if(!separator) {
                    separator = $dateRangePicker.locale.separator;
                }
                
                if(singleDatePicker){
                    if(startDate){
                        $input.val(startDate.format(dateFormat));
                    }
                }else{
                    if(startDate && endDate){
                        $input.val(startDate.format(dateFormat) + separator + endDate.format(dateFormat));
                    }
                }
                
                $input.on('apply.daterangepicker', function (ev, picker) {
                    if(singleDatePicker){
                        $(this).val(picker.startDate.format(dateFormat));
                    }else{
                        $(this).val(picker.startDate.format(dateFormat) + separator + picker.endDate.format(dateFormat));
                    }

                    $(this).trigger('change');
                });

                $input.on('cancel.daterangepicker', function (ev, picker) {
                    $(this).val('');
                    $(this).trigger('change');
                });
                $input.on('show.daterangepicker', function (ev, picker) {
                    var momentStartDate = getMoment(startDate);
                    var momentEndDate = getMoment(endDate);
                    if (momentStartDate.isValid()) {
                        picker.setStartDate(momentStartDate);
                    }
                    if (momentEndDate.isValid()) {
                        picker.setEndDate(momentEndDate);
                    }
                });
                

                $clearButton.on('click', function () {
                    $input.val('');
                    $input.trigger('change');
                });

                $input.on('change', function () {
                    var dates = $(this).val().split(separator);
                    if (dates.length === 2) {
                        startDate = getMoment(dates[0]);
                        if (!startDate.isValid()) {
                            startDate = null;
                        }else{
                            startDate = formatDate(startDate);
                        }
                        endDate = getMoment(dates[1]);
                        if (!endDate.isValid()) {
                            endDate = null;
                        }else{
                            endDate = formatDate(endDate);
                        }
                    } else {
                        startDate = getMoment(dates[0]);
                        if (!startDate.isValid()) {
                            startDate = null;
                        }else{
                            startDate = formatDate(startDate);
                        }
                        endDate = null;
                    }
                    
                    if(!startDate){
                        $dateRangePicker.setStartDate(getMoment());
                        $dateRangePicker.setEndDate(getMoment());
                    }

                    if(!singleDatePicker){
                        var input1 = $this.find("input[data-start-date]")
                        input1.val(startDate);
                        var input2 = $this.find("input[data-end-date]")
                        input2.val(endDate);
                    }else{
                        var input = $this.find("input[data-date]")
                        input.val(startDate);
                    }

                    if(singleDatePicker){
                        $this.data('date', startDate);
                        $input.data('date', startDate);
                    }else{
                        $this.data('startDate', startDate);
                        $this.data('endDate', endDate);
                        $input.data('startDate', startDate);
                        $input.data('endDate', endDate);
                    }
                });
                
                function formatDate(date){
                    if(date){
                        debugger;
                        if(isIso){
                            return date.format();
                        }
                        return date.format(dateFormat)
                    }
                    return null;
                }
                function getFormattedStartDate (){
                    if(startDate){
                        return getMoment(startDate).format(dateFormat);
                    }
                    return null;
                }
                
                function getFormattedEndDate (){
                    if(endDate){
                        return getMoment(endDate).format(dateFormat);
                    }
                    return null;
                }
                
                if(singleDatePicker){
                    $this[0].getFormattedDate = getFormattedStartDate;
                    $input[0].getFormattedDate = getFormattedStartDate;
                    $dateRangePicker.getFormattedDate = getFormattedStartDate;
                }else{
                    $dateRangePicker.getFormattedStartDate = getFormattedStartDate;
                    $dateRangePicker.getFormattedEndDate = getFormattedEndDate;
                    
                    $this[0].getFormattedStartDate = getFormattedStartDate;
                    $this[0].getFormattedEndDate = getFormattedEndDate;
                    
                    $input[0].getFormattedStartDate = getFormattedStartDate;
                    $input[0].getFormattedEndDate = getFormattedEndDate;
                }
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
        abp.dom.initializers.initializeDateRangePickers($(document));
        abp.dom.initializers.initializeForms($('form'));
        abp.dom.initializers.initializeAutocompleteSelects($('.auto-complete-select'));
        $('[data-auto-focus="true"]').first().findWithSelf('input,select').focus();

    });

})(jQuery);
