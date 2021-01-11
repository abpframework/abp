var abp = abp || {};

$.validator.defaults.ignore = ''; //TODO: Would be better if we can apply only for the form we are working on! Also this should be decided by the form itself!

(function ($) {

    abp.modals = abp.modals || {};

    abp.ModalManager = (function () {

        var CallbackList = function () {
            var _callbacks = [];

            return {
                add: function (callback) {
                    _callbacks.push(callback);
                },

                triggerAll: function (thisObj, argumentList) {
                    for (var i = 0; i < _callbacks.length; i++) {
                        _callbacks[i].apply(thisObj, argumentList);
                    }
                }

            };
        };

        return function (options) {

            if (typeof options === 'string') {
                options = {
                    viewUrl: options
                };
            }

            var _options = options;

            var _$modalContainer = null;
            var _$modal = null;
            var _$form = null;

            var _modalId = 'Modal_' + (Math.floor((Math.random() * 1000000))) + new Date().getTime();
            var _modalObject = null;

            var _publicApi = null;
            var _args = null;

            var _onOpenCallbacks = new CallbackList();
            var _onCloseCallbacks = new CallbackList();
            var _onResultCallbacks = new CallbackList();

            function _removeContainer() {
                _$modalContainer && _$modalContainer.remove();
            }

            function _createContainer() {
                _removeContainer();
                _$modalContainer = $('<div id="' + _modalId + 'Container' + '"></div>');
                $('body').prepend(_$modalContainer);
                return _$modalContainer;
            }

            function _initAndShowModal() {
                _$modal = _$modalContainer.find('.modal');
                _$form = _$modalContainer.find('form');
                if (_$form.length) {
                    if (_$form.attr('data-ajaxForm') !== 'false') {
                        _$form.abpAjaxForm();
                    }

                    if (_$form.attr('data-check-form-on-close') !== 'false') {
                        _$form.needConfirmationOnUnsavedClose(_$modal);
                    }

                    _$form.on('abp-ajax-success',
                        function () {
                            _publicApi.setResult.apply(_publicApi, arguments);
                            _$modal.modal('hide');
                        });
                } else {
                    _$form = null;
                }

                _$modal.modal({
                    backdrop: 'static'
                });

                _$modal.on('hidden.bs.modal', function () {
                    _removeContainer();
                    _onCloseCallbacks.triggerAll(_publicApi);
                });

                _$modal.on('shown.bs.modal', function () {
                    //focuses first element if it's a typeable input.
                    var $firstVisibleInput = _$modal.find('input:not([type=hidden]):first');

                    _onOpenCallbacks.triggerAll(_publicApi);

                    if ($firstVisibleInput.hasClass("datepicker")) {
                        return; //don't pop-up date pickers...
                    }

                    var focusableInputs = ["text", "password", "email", "number", "search", "tel", "url"];
                    if (!focusableInputs.includes($firstVisibleInput.prop("type"))) {
                        return;
                    }

                    $firstVisibleInput.focus();
                });

                var modalClass = abp.modals[options.modalClass];
                if (modalClass) {
                    _modalObject = new modalClass();
                    _modalObject.init && _modalObject.init(_publicApi, _args); //TODO: Remove later
                    _modalObject.initModal && _modalObject.initModal(_publicApi, _args);
                }

                _$modal.modal('show');
            };

            var _open = function (args) {

                _args = args || {};

                _createContainer(_modalId)
                    .load(options.viewUrl, $.param(_args), function (response, status, xhr) {
                        if (status === "error") {
                            //TODO: Handle!
                            return;
                        };

                        if (options.scriptUrl) {
                            abp.ResourceLoader.loadScript(options.scriptUrl, function () {
                                _initAndShowModal();
                            });
                        } else {
                            _initAndShowModal();
                        }
                    });
            };

            var _close = function () {
                if (!_$modal) {
                    return;
                }

                _$modal.modal('hide');
            };

            var _onOpen = function (onOpenCallback) {
                _onOpenCallbacks.add(onOpenCallback);
            };

            var _onClose = function (onCloseCallback) {
                _onCloseCallbacks.add(onCloseCallback);
            };

            var _onResult = function (callback) {
                _onResultCallbacks.add(callback);
            };

            _publicApi = {
                open: _open,

                reopen: function () {
                    _open(_args);
                },

                close: _close,

                getModalId: function () {
                    return _modalId;
                },

                getModal: function () {
                    return _$modal;
                },

                getForm: function () {
                    return _$form;
                },

                getArgs: function () {
                    return _args;
                },

                getOptions: function () {
                    return _options;
                },

                setResult: function () {
                    _onResultCallbacks.triggerAll(_publicApi, arguments);
                },

                onOpen: _onOpen,

                onClose: _onClose,

                onResult: _onResult
            };

            return _publicApi;

        };
    })();

})(jQuery);
