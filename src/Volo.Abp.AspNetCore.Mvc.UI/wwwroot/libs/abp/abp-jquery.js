var abp = abp || {};
(function ($) {

    if (!$) {
        return;
    }

    /* JQUERY ENHANCEMENTS ***************************************************/

    // abp.ajax -> uses $.ajax ------------------------------------------------

    abp.ajax = function (userOptions) {
        userOptions = userOptions || {};

        var options = $.extend(true, {}, abp.ajax.defaultOpts, userOptions);

        options.success = undefined;
        options.error = undefined;

        return $.Deferred(function ($dfd) {
            $.ajax(options)
                .done(function (data, textStatus, jqXHR) {
                    $dfd.resolve(data);
                    userOptions.success && userOptions.success(data);
                }).fail(function (jqXHR) {
                    if (jqXHR.getResponseHeader('_AbpErrorFormat') === 'true') {
                        abp.ajax.handleAbpErrorResponse(jqXHR, userOptions, $dfd);
                    } else {
                        abp.ajax.handleNonAbpErrorResponse(jqXHR, userOptions, $dfd);
                    }
                });
        });
    };

    $.extend(abp.ajax, {
        defaultOpts: {
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        },

        defaultError: {
            message: 'An error has occurred!',
            details: 'Error detail not sent by server.'
        },

        defaultError401: {
            message: 'You are not authenticated!',
            details: 'You should be authenticated (sign in) in order to perform this operation.'
        },

        defaultError403: {
            message: 'You are not authorized!',
            details: 'You are not allowed to perform this operation.'
        },

        defaultError404: {
            message: 'Resource not found!',
            details: 'The resource requested could not found on the server.'
        },

        logError: function (error) {
            abp.log.error(error);
        },

        showError: function (error) {
            if (error.details) {
                return abp.message.error(error.details, error.message);
            } else {
                return abp.message.error(error.message || abp.ajax.defaultError.message);
            }
        },

        handleTargetUrl: function (targetUrl) {
            if (!targetUrl) {
                location.href = abp.appPath;
            } else {
                location.href = targetUrl;
            }
        },

        handleErrorStatusCode: function (status) {
            switch (status) {
                case 401:
                    abp.ajax.handleUnAuthorizedRequest(
                        abp.ajax.showError(abp.ajax.defaultError401),
                        abp.appPath
                    );
                    break;
                case 403:
                    abp.ajax.showError(abp.ajax.defaultError403);
                    break;
                case 404:
                    abp.ajax.showError(abp.ajax.defaultError404);
                    break;
                default:
                    abp.ajax.showError(abp.ajax.defaultError);
                    break;
            }
        },

        handleNonAbpErrorResponse: function (jqXHR, userOptions, $dfd) {
            if (userOptions.abpHandleError !== false) {
                abp.ajax.handleErrorStatusCode(jqXHR.status);
            }

            $dfd.reject.apply(this, arguments);
            userOptions.error && userOptions.error.apply(this, arguments);
        },

        handleAbpErrorResponse: function (jqXHR, userOptions, $dfd) {
            var messagePromise = null;

            if (userOptions.abpHandleError !== false) {
                messagePromise = abp.ajax.showError(jqXHR.responseJSON.error);
            }

            abp.ajax.logError(jqXHR.responseJSON.error);

            $dfd && $dfd.reject(jqXHR.responseJSON.error, jqXHR);
            userOptions.error && userOptions.error(jqXHR.responseJSON.error, jqXHR);

            if (jqXHR.status === 401 && userOptions.abpHandleError !== false) {
                abp.ajax.handleUnAuthorizedRequest(messagePromise);
            }
        },

        handleUnAuthorizedRequest: function (messagePromise, targetUrl) {
            if (messagePromise) {
                messagePromise.done(function () {
                    abp.ajax.handleTargetUrl(targetUrl);
                });
            } else {
                abp.ajax.handleTargetUrl(targetUrl);
            }
        },

        blockUI: function (options) {
            if (options.blockUI) {
                if (options.blockUI === true) { //block whole page
                    abp.ui.setBusy();
                } else { //block an element
                    abp.ui.setBusy(options.blockUI);
                }
            }
        },

        unblockUI: function (options) {
            if (options.blockUI) {
                if (options.blockUI === true) { //unblock whole page
                    abp.ui.clearBusy();
                } else { //unblock an element
                    abp.ui.clearBusy(options.blockUI);
                }
            }
        }//,

        //ajaxSendHandler: function (event, request, settings) {
        //    var token = abp.security.antiForgery.getToken();
        //    if (!token) {
        //        return;
        //    }

        //    if (!settings.headers || settings.headers[abp.security.antiForgery.tokenHeaderName] === undefined) {
        //        request.setRequestHeader(abp.security.antiForgery.tokenHeaderName, token);
        //    }
        //}
    });

    //$(document).ajaxSend(function (event, request, settings) {
    //    return abp.ajax.ajaxSendHandler(event, request, settings);
    //});

    //abp.event.on('abp.dynamicScriptsInitialized', function () {
    //    abp.ajax.defaultError.message = abp.localization.abpWeb('DefaultError');
    //    abp.ajax.defaultError.details = abp.localization.abpWeb('DefaultErrorDetail');
    //    abp.ajax.defaultError401.message = abp.localization.abpWeb('DefaultError401');
    //    abp.ajax.defaultError401.details = abp.localization.abpWeb('DefaultErrorDetail401');
    //    abp.ajax.defaultError403.message = abp.localization.abpWeb('DefaultError403');
    //    abp.ajax.defaultError403.details = abp.localization.abpWeb('DefaultErrorDetail403');
    //    abp.ajax.defaultError404.message = abp.localization.abpWeb('DefaultError404');
    //    abp.ajax.defaultError404.details = abp.localization.abpWeb('DefaultErrorDetail404');
    //});

})(jQuery);