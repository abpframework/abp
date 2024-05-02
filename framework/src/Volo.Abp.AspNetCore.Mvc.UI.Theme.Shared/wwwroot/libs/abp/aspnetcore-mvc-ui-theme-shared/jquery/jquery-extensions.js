(function ($) {
    if (!$) {
        return;
    }

    var localize = function (key) {
        return abp.localization.getResource('AbpUi')(key);
    };

    /* A simple jQuery plug-in to make a button busy. */
    $.fn.buttonBusy = function (isBusy) {
        return $(this).each(function () {
            var $button = $(this);
            var $icon = $button.find('i');
            var $buttonInnerSpan = $button.find('span');

            if (isBusy === undefined) {
                isBusy = true;
            }

            if (isBusy) {
                if ($button.hasClass('button-busy')) {
                    return;
                }

                $button.attr('disabled', 'disabled');

                //change icon
                if ($icon.length) {
                    $button.data('iconOriginalClasses', $icon.attr('class'));
                    $icon.removeClass();
                    $icon.addClass('fa fa-spin fa-spinner');
                }

                //change text
                if ($buttonInnerSpan.length && $button.attr('data-busy-text')) {
                    $button.data('buttonOriginalText', $buttonInnerSpan.html());

                    if ($button.data('busy-text-is-html')) {
                        $buttonInnerSpan.html($button.attr('data-busy-text'));
                    } else {
                        $buttonInnerSpan.text($button.attr('data-busy-text'));
                    }
                }

                $button.addClass('button-busy');
            } else {
                if (!$button.hasClass('button-busy')) {
                    return;
                }

                //enable button
                $button.removeAttr('disabled');

                //restore icon
                if ($icon.length && $button.data('iconOriginalClasses')) {
                    $icon.removeClass();
                    $icon.addClass($button.data('iconOriginalClasses'));
                }

                //restore text
                if ($buttonInnerSpan.length && $button.data('buttonOriginalText')) {
                    $buttonInnerSpan.html($button.data('buttonOriginalText'));
                }

                $button.removeClass('button-busy');
            }
        });
    };

    var toCamelCase = function (str) {
        var regexs = [
            /(^[A-Z])/, // first char of string
            /((\.)[A-Z])/ // first char after a dot (.)
        ];

        regexs.forEach(
            function (regex) {
                var infLoopAvoider = 0;

                while (regex.test(str)) {
                    str = str
                        .replace(regex, function ($1) { return $1.toLowerCase(); });

                    if (infLoopAvoider++ > 1000) {
                        break;
                    }
                }
            }
        );

        return str;
    };

    $.fn.serializeFormToObject = function (camelCase) {
        //serialize to array
        var data = $(this).serializeArray();

        // add unchecked checkboxes because serializeArray ignores them
        $(this).find("input[type=checkbox]").each(function () {
            if (!$(this).is(':checked')) {
                data.push({name: this.name, value: this.checked});
            }
        });

        //add also disabled items
        $(':disabled[name]', this)
            .each(function (item) {
                var value = '';

                if ($(this).is(":checkbox")) {
                    value = $(this).is(':checked');
                } else {
                    value = $(this).val();
                }

                data.push({ name: this.name, value: value });
            });

        //map to object

        var getVarName = function (v) {
            return v.toString().replace(/\(\)\s?=\>\s?/, '');
        }

        var getNames = function (index, variable) {
            var name = '';
            for (var i = 0; i <= index; i++) {
                if (i == 0) {
                    name = variable + '[' + i + ']'
                } else {
                    name += '][' + variable + '[' + i + ']'
                }
            }
            return name;
        }

        var obj = {};
        var objName = getVarName(() => obj);

        if (camelCase !== undefined ? camelCase : true) {
            data.forEach(function (d) {
                d.name = toCamelCase(d.name);
            });
        }

        data.map(function (x) {
            var names = x.name.split(".");
            var xName = getVarName(() => x);
            var namesName = getVarName(() => names);

            var i = obj ? 0 : 1;
            for (i = 0; i < names.length; i++) {
                if (eval('!' + objName + '[' + getNames(i, '' + namesName + '') + ']')) {
                    eval('' + objName + '[' + getNames(i, '' + namesName + '') + '] = {}');
                }
            }

            if ($.isEmptyObject(eval('' + objName + '[' + getNames(names.length - 1, '' + namesName + '') + ']'))) {
                eval('' + objName + '[' + getNames(names.length - 1, '' + namesName + '') + '] = ' + xName + '.value');
            }
        });

        return obj;
    };

    $.fn.focusEndOfText = function () {
        return this.each(function () {
            var $this = $(this);
            setTimeout(function () {
                $this.focus();
                $this[0].selectionStart = $this[0].selectionEnd = 10000;
            }, 0);
        });
    };

    $.fn.needConfirmationOnUnsavedClose = function ($modal) {
        var $form = $(this);
        var formSaved = false;
        var unEditedForm = JSON.stringify($form.serializeFormToObject());

        $modal.on("hide.bs.modal", function (e) {
            var currentForm = JSON.stringify($form.serializeFormToObject());
            var thereAreUnsavedChanges = currentForm !== unEditedForm;

            if (!formSaved && thereAreUnsavedChanges) {
                e.preventDefault();
                abp.message.confirm(localize('AreYouSureYouWantToCancelEditingWarningMessage'),
                    function (result) {
                        if (result) {
                            formSaved = true;
                            $modal.modal('hide');
                        }
                    }, false);
            }
        });

        $(this).on('abp-ajax-success', function () {
            formSaved = true;
        });
    };

})(jQuery);
