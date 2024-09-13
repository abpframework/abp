(function ($) {
    abp.dom = abp.dom || {};

    abp.dom.initializers = abp.dom.initializers || {};
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

            if (options.value) {
                $dateInput.val(options.value)
            }

            const setAttribute = attr => {
                if (options[attr]) {
                    $dateInput.attr(attr, options[attr])
                }
            }

            setAttribute('placeholder');
            setAttribute('name');
            setAttribute('id');
            setAttribute('required');
            setAttribute('disabled');
            setAttribute('readonly');

            const sizeMap = {
                'Small': 'form-control-sm',
                'Medium': 'form-control-md',
                'Large': 'form-control-lg'
            };

            if (options.size && sizeMap[options.size]) {
                $dateInput.addClass(sizeMap[options.size])
            }

            $inputGroup.append($dateInput);

            const addButton = (button, buttonHTML) => {
                if (options[button] !== false) {
                    var $button = $(buttonHTML);
                    $inputGroup.append($button);
                    if (options.disabled) {
                        $button.attr('disabled', 'disabled')
                    }
                }
            }

            addButton('openButton', '<button type="button" class="btn btn-outline-secondary" tabindex="-1" data-type="open"><i class="fa fa-calendar"></i></button>');
            addButton('clearButton', '<button type="button" class="btn btn-outline-secondary" tabindex="-1" data-type="clear"><i class="fa fa-times"></i></button>');

            $datePicker.append($inputGroup);

            const addHiddenInput = (name, attributeName) => {
                if (name) {
                    var $hiddenElement = $(`<input type="hidden" name="${name}" ${attributeName}="true" />`);
                    $datePicker.append($hiddenElement);
                }
            }

            addHiddenInput(options.startDateName, 'data-start-date');
            addHiddenInput(options.endDateName, 'data-end-date');
            addHiddenInput(options.dateName, 'data-date');

            $datePicker.data('options', options);
            abp.dom.initializers.initializeDateRangePickers($datePicker);
            $container[0].datePicker = $datePicker[0];
            return $container;
        }
    };

    function isEmptyDate(date, options) {
        if(!date) {
            return true;
        }

        if(typeof date === 'string') {
            return date.trim() === '';
        }

        return !AbpDate(date, options).isValid();
    }

    function AbpDate(originalDate, options, format) {

        var _moment = convertToMoment(originalDate, options, format);
        function _clone() {
            return AbpDate(_moment, options, format);
        }

        function _isSame(date) {
            return _format(options.inputDateFormat) === AbpDate(date, options).format(options.inputDateFormat);
        }

        function _isValid() {
            return _moment && _moment.isValid();
        }

        function _format(format) {
            if (_isValid()) {
                return _moment.format(format);
            }
            return '';
        }

        function _valueOf() {
            return _isValid() ? _moment.valueOf() : undefined;
        }

        function _toString() {
            return _moment.toString();
        }

        return {
            _moment: _moment,
            clone: _clone,
            isSame: _isSame,
            isValid: _isValid,
            format: _format,
            valueOf: _valueOf,
            toString: _toString,
            date: _moment.toISOString(),
            isAbpDate: true
        };
    }

    function replaceOpenButton(hasDate, singleOpenAndClearButton, $openButton, $clearButton) {
        const hideButtonClass = 'd-none';
        if (!singleOpenAndClearButton) {
            return;
        }

        if (hasDate) {
            $openButton.addClass(hideButtonClass);
            $clearButton.removeClass(hideButtonClass);
            $clearButton.insertAfter($openButton);
        } else {
            $openButton.removeClass(hideButtonClass);
            $clearButton.addClass(hideButtonClass);
            $openButton.insertAfter($clearButton);
        }
    }

    function convertToMoment(value, options, dateFormat, isUtc) {
        if(!value) {
            // invalid date
            return moment.invalid();
        }

        options = options || {};
        var formats = new Set([dateFormat, options.visibleDateFormat, options.inputDateFormat, undefined]);

        if (typeof value === 'string') {
            for(var format of formats) {
                var date = isUtc ? moment.utc(value, format) : moment(value, format);
                if(date.isValid()) {
                    return date;
                }
            }
        }

        if (value.isAbpDate) {
            return value._moment.clone();
        }

        if(value.isLuxonDateTime) {
            return isUtc ? moment.utc(value.toISO()) : moment(value.toISO());
        }

        return isUtc ? moment.utc(value) : moment(value);
    }

    function getTodayButton(options, $dateRangePicker) {
        if (options.showTodayButton) {
            var $todayBtn = $('<button type="button" class="btn btn-sm today-btn" data-action="today">' + options.locale.todayLabel + '</button>');
            if (options.todayButtonClasses) {
                $todayBtn.addClass(options.todayButtonClasses);
            } else {
                $todayBtn.addClass('btn-default');
            }

            $todayBtn.on('click', function () {
                var today = moment();
                $dateRangePicker.setStartDate(today);
                $dateRangePicker.setEndDate(today);

                if(options.singleDatePicker && options.autoApply){
                    $dateRangePicker.clickApply();
                }else{
                    $dateRangePicker.updateView();
                }
            });

            return $todayBtn;
        }

        return null;
    }

    function getClearButton(options, $input, $dateRangePicker) {
        if (options.showClearButton && options.autoUpdateInput !== true) {
            var $clearBtn = $('<button type="button" class="btn btn-sm clear-btn" data-action="clear">' + options.locale.clearLabel + '</button>');
            if (options.clearButtonClasses) {
                $clearBtn.addClass(options.clearButtonClasses);
            } else {
                $clearBtn.addClass('btn-default');
            }
            $clearBtn.on('click', function () {
                $input.trigger('cancel.daterangepicker', $dateRangePicker);
                $dateRangePicker.hide();
            });

            return $clearBtn;
        }
        return null;
    }

    function addExtraButtons(options, $dateRangePicker, $input) {
        var extraButtons = [getTodayButton(options, $dateRangePicker), getClearButton(options, $input, $dateRangePicker)];

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

    function addOpenButtonClick($openButton, $dateRangePicker) {
        if (!$openButton) {
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

    function extendDateFormat(format, options) {
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

    function setOptions(options, $datePickerElement, singleDatePicker) {
        options.singleDatePicker = singleDatePicker;

        var $modal = $datePickerElement.closest('.modal.fade');

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
            singleOpenAndClearButton: true,
            parentEl: $modal.length > 0 ? $modal : 'body'
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

        if(options.timePickerSeconds === true && options.timePicker !== false) {
            options.timePicker = true;
        }

        if (options.timePicker && options.timePicker24Hour === undefined) {
            options.timePicker24Hour = moment.localeData().longDateFormat('LT').indexOf('A') < 1;
        }

        options.visibleDateFormat = options.visibleDateFormat || options.dateFormat || extendDateFormat(moment.localeData().longDateFormat('L'), options);
        options.locale = options.locale || {};
        options.locale.format = options.visibleDateFormat;

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
                options.ranges[key] = [AbpDate(start, options), AbpDate(end, options)];
            });
        }

        if (options.maxDate){
            options.maxDate = convertToMoment(options.maxDate, options);
        }

        if (options.minDate){
            options.minDate = convertToMoment(options.minDate, options);
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

    function getAccessorDescriptor(obj, prop) {
        if (!obj) {
            return;
        }
        var descriptor = Object.getOwnPropertyDescriptor(obj, prop);
        if (descriptor) {
            return descriptor;
        }

        return getAccessorDescriptor(Object.getPrototypeOf(obj), prop);
    }

    function addHiddenDateInputSetListener(obj, prop, listener, options) {
        if (!obj) {
            return;
        }

        var descriptor = getAccessorDescriptor(obj, prop);
        if (!descriptor) {
            return;
        }
        var originalSet = descriptor.set;
        descriptor.set = function (value) {
            originalSet.call(this, formatHiddenDate(value, options));
            if (value.isAbpDate) {
                return;
            }
            listener.call(this, value);
        };

        Object.defineProperty(obj, prop, descriptor);
    }

    function formatHiddenDate(date, options) {
        date = convertToMoment(date, options, options.inputDateFormat);
        if (date.isValid()) {
            if (options.isUtc) {
                date = date.utc();
            }else{
                date = date.local();
            }
            if (options.isIso) {
                return date.toISOString();
            }
            return date.locale('en').format(options.inputDateFormat)
        }

        return '';
    }

    function createInitialAbpDate(date, options) {
        date = convertToMoment(date, options, undefined, options.isUtc);
        if (options.isUtc) {
            date = date.local();
        }

        return AbpDate(date, options);
    }

    abp.dom.initializers.initializeDateRangePickers = function ($rootElement) {
        $rootElement
            .findWithSelf('abp-date-picker,abp-date-range-picker')
            .each(function () {
                var $this = $(this);
                var $input = $this.find('.input-group input[type="text"]')
                var $startDateInput = $this.find('input[type="hidden"][data-start-date]');
                var $endDateInput = $this.find('input[type="hidden"][data-end-date]');
                var $dateInput = $this.find('input[type="hidden"][data-date]');
                if ($input.data('daterangepicker')) {
                    return;
                }

                var $openButton = $this.find('button[data-type="open"]')
                var $clearButton = $this.find('button[data-type="clear"]')
                var singleDatePicker = $this.is('abp-date-picker')
                var options = {};

                setOptions(options, $this, singleDatePicker);

                options.inputDateFormat = options.inputDateFormat || extendDateFormat("YYYY-MM-DD", options);

                var singleOpenAndClearButton = options.singleOpenAndClearButton && $clearButton.length > 0 && $openButton.length > 0;

                var startDate = createInitialAbpDate(options.startDate || options.date || (options.autoUpdateInput ? new Date() : undefined), options);

                var oldStartDate = AbpDate(undefined, options);

                var endDate = createInitialAbpDate(options.endDate || (options.autoUpdateInput ? new Date() : undefined), options);
                var oldEndDate = AbpDate(undefined, options);

                options.startDate = convertToMoment(startDate, options);
                options.endDate = convertToMoment(endDate, options);
                $input.daterangepicker(options);

                var $dateRangePicker = $input.data('daterangepicker');

                $input.data('format', options.visibleDateFormat);
                $this.data('daterangepicker', $dateRangePicker);
                $startDateInput.data('daterangepicker', $dateRangePicker);
                $startDateInput.data('format', options.inputDateFormat);
                $endDateInput.data('daterangepicker', $dateRangePicker);
                $endDateInput.data('format', options.inputDateFormat);
                $dateInput.data('daterangepicker', $dateRangePicker);
                $dateInput.data('format', options.inputDateFormat);

                addExtraButtons(options, $dateRangePicker, $input);

                addOpenButtonClick($openButton, $dateRangePicker);

                if (!options.separator) {
                    options.separator = $dateRangePicker.locale.separator;
                }

                function fillInput() {
                    if (options.singleDatePicker) {
                        if (!isEmptyDate(startDate, options)) {
                            $input.val(startDate.format(options.visibleDateFormat));
                        }
                    } else {
                        if (!isEmptyDate(startDate, options) && !isEmptyDate(endDate, options)) {
                            $input.val(startDate.format(options.visibleDateFormat) + options.separator + endDate.format(options.visibleDateFormat));
                        }
                    }
                }

                fillInput();

                function changeDate(isTrigger = true, isInputTrigger = true, isDateRangePickerTrigger = true) {
                    if (singleDatePicker) {
                        if (oldStartDate.isSame(startDate)) {
                            return;
                        }
                        setDataDates(startDate, $dateInput, '');
                        $dateInput.val(startDate);
                        if (isTrigger) {
                            triggerDateChange($dateInput, startDate);
                        }
                    } else {
                        if (!oldStartDate.isSame(startDate)) {
                            setDataDates(startDate, $startDateInput, 'start');
                            $startDateInput.val(startDate);
                            if (isTrigger) {
                                triggerDateChange($startDateInput, startDate);
                            }
                        }

                        if (!oldEndDate.isSame(endDate)) {
                            setDataDates(endDate, $endDateInput, 'end');
                            $endDateInput.val(endDate);
                            if (isTrigger) {
                                triggerDateChange($endDateInput, endDate);
                            }
                        }
                    }

                    if (isTrigger && isInputTrigger) {
                        if (!oldStartDate.isSame(startDate) || !oldEndDate.isSame(endDate)) {
                            triggerDateChange($this, startDate, isDateRangePickerTrigger);
                            triggerDateChange($input, startDate, isDateRangePickerTrigger);
                        }
                    }

                    if(isTrigger || isInputTrigger || isDateRangePickerTrigger){
                        triggerValidation();
                    }
                }

                function setDataDates(date, $selfInput, prefix){
                    date = AbpDate(date, options).date;
                    $selfInput.data('date', date);
                    $this.data(prefix + 'date', date);
                    $input.data(prefix + 'date', date);
                }

                function triggerValidation() {
                    checkValidity($startDateInput);
                    checkValidity($endDateInput);
                    checkValidity($dateInput);
                    checkValidity($input);
                }

                function checkValidity($selfInput) {
                    if (!$selfInput) {
                        return;
                    }

                    if($selfInput.closest('form').length === 0) {
                        return;
                    }

                    $selfInput.valid();
                }

                function triggerDateChange($selfInput, value, isDateRangePickerTrigger) {
                    $selfInput.trigger('change');
                    if(isDateRangePickerTrigger !== false){
                        if(isEmptyDate(value, options)) {
                            $selfInput.trigger('cancel.daterangepicker', $dateRangePicker);
                        }else {
                            $selfInput.trigger('apply.daterangepicker', $dateRangePicker);
                        }
                    }
                }

                $input.on('apply.daterangepicker', function (ev, picker) {
                    if(startDate.isSame(picker.startDate) && (singleDatePicker || endDate.isSame(picker.endDate))) {
                        fillInput();
                        picker.hide();
                        return;
                    }
                    setStartDateByMomentDate(picker.startDate);
                    setEndDateByMomentEndDate(picker.endDate);
                    fillInput();
                    inputChangeHandler(true, true, false);
                });

                $input.on('show.daterangepicker', function (ev, picker) {
                    const today = moment().startOf('day');
                    if(isEmptyDate(startDate, options)){
                        picker.setStartDate(today);
                    }else{
                        picker.setStartDate(convertToMoment(startDate, options, options.inputDateFormat));
                    }

                    if(singleDatePicker){
                        picker.setEndDate(picker.startDate);
                    }else if(isEmptyDate(endDate, options)){
                        picker.setEndDate(today);
                    }
                    else{
                        picker.setEndDate(convertToMoment(endDate, options, options.inputDateFormat));
                    }

                    picker.updateView();
                });

                $input.on('cancel.daterangepicker', function (ev, picker) {
                    if(!$input.val()){
                        picker.hide();
                        return;
                    }
                    $input.val('');
                    setStartDateByMomentDate();
                    setEndDateByMomentEndDate();
                    inputChangeHandler(true, true, false);
                });

                $clearButton.on('click', function (e) {
                    $input.trigger('cancel.daterangepicker', $dateRangePicker);
                });

                $input.on('change', function (e) {
                    var dates = $input.val().split(options.separator);
                    var newStartDate = convertToMoment(dates[0], options, options.visibleDateFormat);
                    var newEndDate = undefined;
                    if (dates.length === 2) {
                        newEndDate = convertToMoment(dates[1], options, options.visibleDateFormat);
                    }else{
                        newEndDate = convertToMoment(undefined, options, options.visibleDateFormat);
                    }

                    if (startDate.isSame(newStartDate) && (singleDatePicker || endDate.isSame(newEndDate))) {
                        fillInput();
                        return;
                    }

                    setStartDateByMomentDate(newStartDate);
                    setEndDateByMomentEndDate(newEndDate);

                    if(newStartDate.isValid()){
                        $dateRangePicker.setStartDate(newStartDate);
                    }else{
                        $dateRangePicker.setStartDate();
                    }

                    if(newEndDate.isValid()){
                        $dateRangePicker.setEndDate(newEndDate);
                    }else{
                        $dateRangePicker.setEndDate();
                    }

                    fillInput();
                    inputChangeHandler(undefined, false);
                });

                function inputChangeHandler(isTrigger, isInputTrigger, isDateRangePickerTrigger) {
                    var value = $input.val();
                    if (value !== '') {
                        replaceOpenButton(true, singleOpenAndClearButton, $openButton, $clearButton);
                    } else {
                        replaceOpenButton(false, singleOpenAndClearButton, $openButton, $clearButton);
                    }

                    if (isEmptyDate(startDate, options)) {
                        replaceOpenButton(false, singleOpenAndClearButton, $openButton, $clearButton);
                        $this.val('');
                    }

                    changeDate(isTrigger, isInputTrigger, isDateRangePickerTrigger);
                }

                function setStartDateByMomentDate (momentDate) {
                    oldStartDate = startDate;
                    startDate = AbpDate(momentDate, options);
                }

                function setEndDateByMomentEndDate(momentDate){
                    if(singleDatePicker){
                        return;
                    }
                    oldEndDate = endDate;
                    endDate = AbpDate(momentDate, options);
                }

                function startDateInputHandler(value) {
                    value = convertToMoment(value, options);

                    if (startDate.isSame(value)) {
                        return;
                    }

                    setStartDateByMomentDate(value);

                    $dateRangePicker.setStartDate(value);
                    fillInput();
                    inputChangeHandler(false);
                }

                function endDateInputHandler(value) {
                    value = convertToMoment(value, options);

                    if (endDate.isSame(value)) {
                        return;
                    }

                    setEndDateByMomentEndDate(value);
                    $dateRangePicker.setEndDate(value);
                    fillInput();
                    inputChangeHandler(false);
                }

                addHiddenDateInputSetListener($startDateInput[0], 'value', startDateInputHandler, options);
                addHiddenDateInputSetListener($endDateInput[0], 'value', endDateInputHandler, options);
                addHiddenDateInputSetListener($dateInput[0], 'value', startDateInputHandler, options);

                inputChangeHandler(false, false, false);
            });
    }

    abp.dom.onNodeAdded(function (args) {
        abp.dom.initializers.initializeDateRangePickers(args.$el);
    });

    $(function () {
        abp.dom.initializers.initializeDateRangePickers($('body'));
    });
})(jQuery);