var abp = abp || {};
(function ($) {
    if (!Swal || !$) {
        return;
    }

    /* DEFAULTS *************************************************/

    abp.libs = abp.libs || {};
    abp.libs.sweetAlert = {
        config: {
            'default': {

            },
            info: {
                icon: 'info'
            },
            success: {
                icon: 'success'
            },
            warn: {
                icon: 'warning'
            },
            error: {
                icon: 'error'
            },
            confirm: {
                icon: 'warning',
                title: 'Are you sure?',
                showCancelButton: true,
                reverseButtons: true
            }
        }
    };

    /* MESSAGE **************************************************/

    var showMessage = function (type, message, title) {
        var opts = $.extend(
            {},
            abp.libs.sweetAlert.config['default'],
            abp.libs.sweetAlert.config[type],
            {
                title: title,
                text: message
            }
        );

        return $.Deferred(function ($dfd) {
            Swal.fire(opts).then(function () {
                $dfd.resolve();
            });
        });
    };

    abp.message.info = function (message, title) {
        return showMessage('info', message, title);
    };

    abp.message.success = function (message, title) {
        return showMessage('success', message, title);
    };

    abp.message.warn = function (message, title) {
        return showMessage('warn', message, title);
    };

    abp.message.error = function (message, title) {
        return showMessage('error', message, title);
    };

    abp.message.confirm = function (message, titleOrCallback, callback) {

        var userOpts = {
            text: message
        };

        if ($.isFunction(titleOrCallback)) {
            closeOnEsc = callback;
            callback = titleOrCallback;
        } else if (titleOrCallback) {
            userOpts.title = titleOrCallback;
        };

        var opts = $.extend(
            {},
            abp.libs.sweetAlert.config['default'],
            abp.libs.sweetAlert.config.confirm,
            userOpts
        );

        return $.Deferred(function ($dfd) {
            Swal.fire(opts).then(result  => {
                callback && callback(result.value);
                $dfd.resolve(result.value);
            })
        });
    };

    abp.event.on('abp.configurationInitialized', function () {
        var l = abp.localization.getResource('AbpUi');

        abp.libs.sweetAlert.config.confirm.title = l('AreYouSure');
        abp.libs.sweetAlert.config.confirm.confirmButtonText = l('Yes');
        abp.libs.sweetAlert.config.confirm.denyButtonText = l('No');
        abp.libs.sweetAlert.config.confirm.cancelButtonText = l('Cancel');
        abp.libs.sweetAlert.config.confirm.showCancelButton = true;
        abp.libs.sweetAlert.config.confirm.reverseButtons = true;
    });

})(jQuery);
