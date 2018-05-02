(function ($) {
    if (!$ || !$.fn.ajaxForm) {
        return;
    }

    $.fn.abpAjaxForm = function (userOptions) {
        userOptions = userOptions || {};

        var options = $.extend({}, $.fn.abpAjaxForm.defaults, userOptions);

        options.beforeSubmit = function (arr, $form) {
            if (userOptions.beforeSubmit && userOptions.beforeSubmit.apply(this, arguments) === false) {
                return false;
            }

            if (!$form.valid()) {
                return false;
            }

            $form.find("button[type='submit']").buttonBusy(true);
            //TODO: Disable other buttons..?
            return true;
        };

        options.error = function (jqXhr) {
            if (jqXhr.getResponseHeader('_AbpErrorFormat') === 'true') {
                abp.ajax.logError(jqXhr.responseJSON.error);
                var messagePromise = abp.ajax.showError(jqXhr.responseJSON.error);
                if (jqXhr.status === 401) {
                    abp.ajax.handleUnAuthorizedRequest(messagePromise);
                }
            } else {
                abp.ajax.handleErrorStatusCode(jqXhr.status);
            }
        };

        //TODO: Error?

        options.complete = function (jqXhr, status, $form) {
            if ($.contains(document, $form[0])) {
                $form.find("button[type='submit']").buttonBusy(false);
                //TODO: Re-enable other buttons..?
            }

            userOptions.complete && userOptions.complete.apply(this, arguments);
        };

        return this.ajaxForm(options);
    };

    $.fn.abpAjaxForm.defaults = {
        method: 'POST'
    };

})(jQuery);