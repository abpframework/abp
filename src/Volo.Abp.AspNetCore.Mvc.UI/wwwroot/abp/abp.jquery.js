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
                    if (data.__abp) {
                        abp.ajax.handleResponse(data, userOptions, $dfd, jqXHR);
                    } else {
                        $dfd.resolve(data);
                        userOptions.success && userOptions.success(data);
                    }
                }).fail(function (jqXHR) {
                    if (jqXHR.responseJSON && jqXHR.responseJSON.__abp) {
                        abp.ajax.handleResponse(jqXHR.responseJSON, userOptions, $dfd, jqXHR);
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

        handleNonAbpErrorResponse: function (jqXHR, userOptions, $dfd) {
            if (userOptions.abpHandleError !== false) {
                switch (jqXHR.status) {
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
            }

            $dfd.reject.apply(this, arguments);
            userOptions.error && userOptions.error.apply(this, arguments);
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

        handleResponse: function (data, userOptions, $dfd, jqXHR) {
            if (data) {
                if (data.success === true) {
                    $dfd && $dfd.resolve(data.result, data, jqXHR);
                    userOptions.success && userOptions.success(data.result, data, jqXHR);

                    if (data.targetUrl) {
                        abp.ajax.handleTargetUrl(data.targetUrl);
                    }
                } else if (data.success === false) {
                    var messagePromise = null;

                    if (data.error) {
                        if (userOptions.abpHandleError !== false) {
                            messagePromise = abp.ajax.showError(data.error);
                        }
                    } else {
                        data.error = abp.ajax.defaultError;
                    }

                    abp.ajax.logError(data.error);

                    $dfd && $dfd.reject(data.error, jqXHR);
                    userOptions.error && userOptions.error(data.error, jqXHR);

                    if (jqXHR.status === 401 && userOptions.abpHandleError !== false) {
                        abp.ajax.handleUnAuthorizedRequest(messagePromise, data.targetUrl);
                    }
                } else { //not wrapped result
                    $dfd && $dfd.resolve(data, null, jqXHR);
                    userOptions.success && userOptions.success(data, null, jqXHR);
                }
            } else { //no data sent to back
                $dfd && $dfd.resolve(jqXHR);
                userOptions.success && userOptions.success(jqXHR);
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
        },

        ajaxSendHandler: function (event, request, settings) {
            var token = abp.security.antiForgery.getToken();
            if (!token) {
                return;
            }

            if (!settings.headers || settings.headers[abp.security.antiForgery.tokenHeaderName] === undefined) {
                request.setRequestHeader(abp.security.antiForgery.tokenHeaderName, token);
            }
        }
    });

    $(document).ajaxSend(function (event, request, settings) {
        return abp.ajax.ajaxSendHandler(event, request, settings);
    });

    /* JQUERY PLUGIN ENHANCEMENTS ********************************************/

    /* jQuery Form Plugin 
     * http://www.malsup.com/jquery/form/
     */

    // abpAjaxForm -> uses ajaxForm ------------------------------------------

    if ($.fn.ajaxForm) {
        $.fn.abpAjaxForm = function (userOptions) {
            userOptions = userOptions || {};

            var options = $.extend({}, $.fn.abpAjaxForm.defaults, userOptions);

            options.beforeSubmit = function () {
                abp.ajax.blockUI(options);
                userOptions.beforeSubmit && userOptions.beforeSubmit.apply(this, arguments);
            };

            options.success = function (data) {
                abp.ajax.handleResponse(data, userOptions);
            };

            //TODO: Error?

            options.complete = function () {
                abp.ajax.unblockUI(options);
                userOptions.complete && userOptions.complete.apply(this, arguments);
            };

            return this.ajaxForm(options);
        };

        $.fn.abpAjaxForm.defaults = {
            method: 'POST'
        };
    }

    abp.event.on('abp.dynamicScriptsInitialized', function () {
        abp.ajax.defaultError.message = abp.localization.abpWeb('DefaultError');
        abp.ajax.defaultError.details = abp.localization.abpWeb('DefaultErrorDetail');
        abp.ajax.defaultError401.message = abp.localization.abpWeb('DefaultError401');
        abp.ajax.defaultError401.details = abp.localization.abpWeb('DefaultErrorDetail401');
        abp.ajax.defaultError403.message = abp.localization.abpWeb('DefaultError403');
        abp.ajax.defaultError403.details = abp.localization.abpWeb('DefaultErrorDetail403');
        abp.ajax.defaultError404.message = abp.localization.abpWeb('DefaultError404');
        abp.ajax.defaultError404.details = abp.localization.abpWeb('DefaultErrorDetail404');
    });

})(jQuery);