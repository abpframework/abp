/**
 * TODO: Document & prepare typescript definitions
 * TODO: Refactor & test more
 */
var abp = abp || {};

$.validator.defaults.ignore = ''; //TODO: Would be better if we can apply only for the form we are working on! Also this should be decided by the form itself!

(function ($) {

    abp.modals = abp.modals || {};
    
    abp.ModalManager = (function () {

        var CallbackList = function() { //TODO: To a seperated file
            var _callbacks = [];

            return {
                add: function(callback) {
                    _callbacks.push(callback);
                },

                triggerAll: function(thisObj, argumentList) {
                    for (var i = 0; i < _callbacks.length; i++) {
                        _callbacks[i].apply(thisObj, argumentList);
                    }
                }

            }
        };

        return function (options) {

            var _options = options;

            var _$modalContainer = null;
            var _$modal = null;
            var _$form = null;

            var _modalId = 'Modal_' + (Math.floor((Math.random() * 1000000))) + new Date().getTime();
            var _modalObject = null;

            var _publicApi = null;
            var _args = null;

            var _onCloseCallbacks = new CallbackList();
            var _onResultCallbacks = new CallbackList();

            function _removeContainer() {
                _$modalContainer && _$modalContainer.remove();
            };

            function _createContainer() {
                _removeContainer();
                _$modalContainer = $('<div id="' + _modalId + 'Container' + '"></div>').appendTo('body');
                return _$modalContainer;
            }

            function _initAndShowModal() {
                _$modal = _$modalContainer.find('.modal');

                _$form = _$modal.find('form');
                if (_$form.length) {
                    $.validator.unobtrusive.parse(_$form);
                    if (_$form.attr('data-ajaxForm') !== 'false') {
                        //TODO: Create abpAjaxForm to not repeat that code!
                        _$form.ajaxForm({
                            dataType: 'json',
                            beforeSubmit: function () {
                                if ($.validator && !_$form.valid()) {
                                    return false;
                                }

                                _setBusy(true);
                                return true;
                            },
                            success: function() {
                                _publicApi.setResult.apply(_publicApi, arguments);
                                _$modal.modal('hide');
                            },
                            error: function (jqXHR) {
                                //TODO: Better and central error handling!
                                if (jqXHR.getResponseHeader('_AbpErrorFormat') === 'true') {
                                    abp.ajax.logError(jqXHR.responseJSON.error);
                                    var messagePromise = abp.ajax.showError(jqXHR.responseJSON.error);
                                    if (jqXHR.status === 401) {
                                        abp.ajax.handleUnAuthorizedRequest(messagePromise);
                                    }
                                } else {
                                    abp.ajax.handleErrorStatusCode(jqXHR.status);
                                }
                            },
                            complete: function() {
                                _setBusy(false);
                            }
                        });
                    }
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
                    _$modal.find('input:not([type=hidden]):first').focus();
                });

                var modalClass = abp.modals[options.modalClass];
                if (modalClass) {
                    _modalObject = new modalClass();
                    if (_modalObject.init) {
                        _modalObject.init(_publicApi, _args);
                    }
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
                            abp.ResourceLoader.loadScript(options.scriptUrl, function() {
                                _initAndShowModal();
                            });
                        } else {
                            _initAndShowModal();
                        }
                    });
            };

            var _close = function() {
                if (!_$modal) {
                    return;
                }

                _$modal.modal('hide');
            };

            var _onClose = function (onCloseCallback) {
                _onCloseCallbacks.add(onCloseCallback);
            }

            var _onResult = function (callback) {
                _onResultCallbacks.add(callback);
            }

            function _setBusy(isBusy) {
                if (!_$modal) {
                    return;
                }

                _$modal.find('.modal-footer button').buttonBusy(isBusy);
            }

            _publicApi = {

                open: _open,

                reopen: function() {
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

                getOptions: function() {
                    return _options;
                },

                setBusy: _setBusy,

                setResult: function () {
                    _onResultCallbacks.triggerAll(_publicApi, arguments);
                },

                onClose: _onClose,

                onResult: _onResult
            }

            return _publicApi;

        };
    })();

})(jQuery);