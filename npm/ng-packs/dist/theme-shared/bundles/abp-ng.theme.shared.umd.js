(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@abp/ng.core'), require('@angular/core'), require('@ng-bootstrap/ng-bootstrap'), require('@ngx-validate/core'), require('primeng/components/common/messageservice'), require('primeng/toast'), require('rxjs'), require('rxjs/operators'), require('@ngxs/store'), require('@angular/router'), require('@ngxs/router-plugin')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.theme.shared', ['exports', '@abp/ng.core', '@angular/core', '@ng-bootstrap/ng-bootstrap', '@ngx-validate/core', 'primeng/components/common/messageservice', 'primeng/toast', 'rxjs', 'rxjs/operators', '@ngxs/store', '@angular/router', '@ngxs/router-plugin'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng.theme = global.abp.ng.theme || {}, global.abp.ng.theme.shared = {}), global.ng_core, global.ng.core, global.ngBootstrap, global.core$1, global.messageservice, global.toast, global.rxjs, global.rxjs.operators, global.store, global.ng.router, global.routerPlugin));
}(this, function (exports, ng_core, core, ngBootstrap, core$1, messageservice, toast, rxjs, operators, store, router, routerPlugin) { 'use strict';

    /*! *****************************************************************************
    Copyright (c) Microsoft Corporation. All rights reserved.
    Licensed under the Apache License, Version 2.0 (the "License"); you may not use
    this file except in compliance with the License. You may obtain a copy of the
    License at http://www.apache.org/licenses/LICENSE-2.0

    THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
    KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
    WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
    MERCHANTABLITY OR NON-INFRINGEMENT.

    See the Apache Version 2.0 License for specific language governing permissions
    and limitations under the License.
    ***************************************************************************** */
    /* global Reflect, Promise */

    var extendStatics = function(d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };

    function __extends(d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    }

    var __assign = function() {
        __assign = Object.assign || function __assign(t) {
            for (var s, i = 1, n = arguments.length; i < n; i++) {
                s = arguments[i];
                for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p)) t[p] = s[p];
            }
            return t;
        };
        return __assign.apply(this, arguments);
    };

    function __read(o, n) {
        var m = typeof Symbol === "function" && o[Symbol.iterator];
        if (!m) return o;
        var i = m.call(o), r, ar = [], e;
        try {
            while ((n === void 0 || n-- > 0) && !(r = i.next()).done) ar.push(r.value);
        }
        catch (error) { e = { error: error }; }
        finally {
            try {
                if (r && !r.done && (m = i["return"])) m.call(i);
            }
            finally { if (e) throw e.error; }
        }
        return ar;
    }

    function __spread() {
        for (var ar = [], i = 0; i < arguments.length; i++)
            ar = ar.concat(__read(arguments[i]));
        return ar;
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @template T
     */
    var   /**
     * @template T
     */
    AbstractToasterClass = /** @class */ (function () {
        function AbstractToasterClass(messageService) {
            this.messageService = messageService;
            this.key = 'abpToast';
            this.sticky = false;
        }
        /**
         * @param {?} message
         * @param {?} title
         * @param {?=} options
         * @return {?}
         */
        AbstractToasterClass.prototype.info = /**
         * @param {?} message
         * @param {?} title
         * @param {?=} options
         * @return {?}
         */
        function (message, title, options) {
            return this.show(message, title, 'info', options);
        };
        /**
         * @param {?} message
         * @param {?} title
         * @param {?=} options
         * @return {?}
         */
        AbstractToasterClass.prototype.success = /**
         * @param {?} message
         * @param {?} title
         * @param {?=} options
         * @return {?}
         */
        function (message, title, options) {
            return this.show(message, title, 'success', options);
        };
        /**
         * @param {?} message
         * @param {?} title
         * @param {?=} options
         * @return {?}
         */
        AbstractToasterClass.prototype.warn = /**
         * @param {?} message
         * @param {?} title
         * @param {?=} options
         * @return {?}
         */
        function (message, title, options) {
            return this.show(message, title, 'warn', options);
        };
        /**
         * @param {?} message
         * @param {?} title
         * @param {?=} options
         * @return {?}
         */
        AbstractToasterClass.prototype.error = /**
         * @param {?} message
         * @param {?} title
         * @param {?=} options
         * @return {?}
         */
        function (message, title, options) {
            return this.show(message, title, 'error', options);
        };
        /**
         * @protected
         * @param {?} message
         * @param {?} title
         * @param {?} severity
         * @param {?=} options
         * @return {?}
         */
        AbstractToasterClass.prototype.show = /**
         * @protected
         * @param {?} message
         * @param {?} title
         * @param {?} severity
         * @param {?=} options
         * @return {?}
         */
        function (message, title, severity, options) {
            this.messageService.clear(this.key);
            this.messageService.add(__assign({ severity: severity, detail: message, summary: title }, options, { key: this.key }, (typeof (options || ((/** @type {?} */ ({})))).sticky === 'undefined' && { sticky: this.sticky })));
            this.status$ = new rxjs.Subject();
            return this.status$;
        };
        /**
         * @param {?=} status
         * @return {?}
         */
        AbstractToasterClass.prototype.clear = /**
         * @param {?=} status
         * @return {?}
         */
        function (status) {
            this.messageService.clear(this.key);
            this.status$.next(status || "dismiss" /* dismiss */);
            this.status$.complete();
        };
        return AbstractToasterClass;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ConfirmationService = /** @class */ (function (_super) {
        __extends(ConfirmationService, _super);
        function ConfirmationService() {
            var _this = _super !== null && _super.apply(this, arguments) || this;
            _this.key = 'abpConfirmation';
            _this.sticky = true;
            return _this;
        }
        ConfirmationService.decorators = [
            { type: core.Injectable, args: [{ providedIn: 'root' },] }
        ];
        /** @nocollapse */ ConfirmationService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function ConfirmationService_Factory() { return new ConfirmationService(core.ɵɵinject(messageservice.MessageService)); }, token: ConfirmationService, providedIn: "root" });
        return ConfirmationService;
    }(AbstractToasterClass));

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ConfirmationComponent = /** @class */ (function () {
        function ConfirmationComponent(confirmationService) {
            this.confirmationService = confirmationService;
            this.confirm = "confirm" /* confirm */;
            this.reject = "reject" /* reject */;
            this.dismiss = "dismiss" /* dismiss */;
        }
        /**
         * @param {?} status
         * @return {?}
         */
        ConfirmationComponent.prototype.close = /**
         * @param {?} status
         * @return {?}
         */
        function (status) {
            this.confirmationService.clear(status);
        };
        ConfirmationComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-confirmation',
                        template: "\n    <p-toast\n      position=\"center\"\n      key=\"abpConfirmation\"\n      (onClose)=\"close(dismiss)\"\n      [modal]=\"true\"\n      [baseZIndex]=\"1000\"\n      styleClass=\"\"\n    >\n      <ng-template let-message pTemplate=\"message\">\n        <div *ngIf=\"message.summary\" class=\"modal-header\">\n          <h4 class=\"modal-title\">\n            {{ message.summary | abpLocalization: message.titleLocalizationParams }}\n          </h4>\n        </div>\n        <div class=\"modal-body\">\n          {{ message.detail | abpLocalization: message.messageLocalizationParams }}\n        </div>\n\n        <div class=\"modal-footer justify-content-center\">\n          <button *ngIf=\"!message.hideCancelBtn\" type=\"button\" class=\"btn btn-secondary\" (click)=\"close(reject)\">\n            {{ message.cancelCopy || 'AbpIdentity::Cancel' | abpLocalization }}\n          </button>\n          <button *ngIf=\"!message.hideYesBtn\" type=\"button\" class=\"btn btn-secondary\" (click)=\"close(confirm)\">\n            <span>{{ message.yesCopy || 'AbpIdentity::Yes' | abpLocalization }}</span>\n          </button>\n        </div>\n      </ng-template>\n    </p-toast>\n  "
                    }] }
        ];
        /** @nocollapse */
        ConfirmationComponent.ctorParameters = function () { return [
            { type: ConfirmationService }
        ]; };
        return ConfirmationComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ErrorComponent = /** @class */ (function () {
        function ErrorComponent() {
            this.title = 'Oops!';
            this.details = 'Sorry, an error has occured.';
        }
        /**
         * @return {?}
         */
        ErrorComponent.prototype.destroy = /**
         * @return {?}
         */
        function () {
            this.renderer.removeChild(this.host, this.elementRef.nativeElement);
        };
        ErrorComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-error',
                        template: "\n    <div class=\"error\">\n      <button id=\"abp-close-button mr-2\" type=\"button\" class=\"close\" (click)=\"destroy()\">\n        <span aria-hidden=\"true\">&times;</span>\n      </button>\n      <div class=\"row centered\">\n        <div class=\"col-md-12\">\n          <div class=\"error-template\">\n            <h1>\n              {{ title }}\n            </h1>\n            <div class=\"error-details\">\n              {{ details }}\n            </div>\n            <div class=\"error-actions\">\n              <a routerLink=\"/\" class=\"btn btn-primary btn-md mt-2\"\n                ><span class=\"glyphicon glyphicon-home\"></span> Take me home\n              </a>\n            </div>\n          </div>\n        </div>\n      </div>\n    </div>\n  ",
                        styles: [".error{position:fixed;top:0;background-color:#fff;width:100vw;height:100vh;z-index:999999}.centered{position:fixed;top:50%;left:50%;transform:translate(-50%,-50%)}"]
                    }] }
        ];
        return ErrorComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var LoaderBarComponent = /** @class */ (function () {
        function LoaderBarComponent(actions, router$1) {
            var _this = this;
            this.actions = actions;
            this.router = router$1;
            this.containerClass = 'abp-loader-bar';
            this.progressClass = 'abp-progress';
            this.isLoading = false;
            this.filter = (/**
             * @param {?} action
             * @return {?}
             */
            function (action) { return action.payload.url.indexOf('openid-configuration') < 0; });
            this.progressLevel = 0;
            actions
                .pipe(store.ofActionSuccessful(ng_core.LoaderStart, ng_core.LoaderStop), operators.filter(this.filter), core$1.takeUntilDestroy(this))
                .subscribe((/**
             * @param {?} action
             * @return {?}
             */
            function (action) {
                if (action instanceof ng_core.LoaderStart)
                    _this.startLoading();
                else
                    _this.stopLoading();
            }));
            router$1.events
                .pipe(operators.filter((/**
             * @param {?} event
             * @return {?}
             */
            function (event) { return event instanceof router.NavigationStart || event instanceof router.NavigationEnd; })), core$1.takeUntilDestroy(this))
                .subscribe((/**
             * @param {?} event
             * @return {?}
             */
            function (event) {
                if (event instanceof router.NavigationStart)
                    _this.startLoading();
                else
                    _this.stopLoading();
            }));
        }
        /**
         * @return {?}
         */
        LoaderBarComponent.prototype.ngOnDestroy = /**
         * @return {?}
         */
        function () { };
        /**
         * @return {?}
         */
        LoaderBarComponent.prototype.startLoading = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.isLoading = true;
            /** @type {?} */
            var interval = setInterval((/**
             * @return {?}
             */
            function () {
                if (_this.progressLevel < 75) {
                    _this.progressLevel += Math.random() * 10;
                }
                else if (_this.progressLevel < 90) {
                    _this.progressLevel += 0.4;
                }
                else if (_this.progressLevel < 100) {
                    _this.progressLevel += 0.1;
                }
                else {
                    clearInterval(interval);
                }
            }), 300);
            this.interval = interval;
        };
        /**
         * @return {?}
         */
        LoaderBarComponent.prototype.stopLoading = /**
         * @return {?}
         */
        function () {
            var _this = this;
            clearInterval(this.interval);
            this.progressLevel = 100;
            this.isLoading = false;
            setTimeout((/**
             * @return {?}
             */
            function () {
                _this.progressLevel = 0;
            }), 800);
        };
        LoaderBarComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-loader-bar',
                        template: "\n    <div id=\"abp-loader-bar\" [ngClass]=\"containerClass\" [class.is-loading]=\"isLoading\">\n      <div [ngClass]=\"progressClass\" [style.width.vw]=\"progressLevel\"></div>\n    </div>\n  ",
                        styles: [".abp-loader-bar{left:0;opacity:0;position:fixed;top:0;transition:opacity .4s linear .4s;z-index:99999}.abp-loader-bar.is-loading{opacity:1;transition:none}.abp-loader-bar .abp-progress{background:#77b6ff;box-shadow:0 0 10px rgba(119,182,255,.7);height:2px;left:0;position:fixed;top:0;transition:width .4s}"]
                    }] }
        ];
        /** @nocollapse */
        LoaderBarComponent.ctorParameters = function () { return [
            { type: store.Actions },
            { type: router.Router }
        ]; };
        LoaderBarComponent.propDecorators = {
            containerClass: [{ type: core.Input }],
            progressClass: [{ type: core.Input }],
            isLoading: [{ type: core.Input }],
            filter: [{ type: core.Input }]
        };
        return LoaderBarComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ModalComponent = /** @class */ (function () {
        function ModalComponent(renderer, confirmationService) {
            this.renderer = renderer;
            this.confirmationService = confirmationService;
            this.centered = true;
            this.modalClass = '';
            this.size = 'lg';
            this.visibleChange = new core.EventEmitter();
            this._visible = false;
            this.closable = false;
            this.isOpenConfirmation = false;
            this.destroy$ = new rxjs.Subject();
        }
        Object.defineProperty(ModalComponent.prototype, "visible", {
            get: /**
             * @return {?}
             */
            function () {
                return this._visible;
            },
            set: /**
             * @param {?} value
             * @return {?}
             */
            function (value) {
                var _this = this;
                if (!this.modalContent) {
                    setTimeout((/**
                     * @return {?}
                     */
                    function () { return (_this.visible = value); }), 0);
                    return;
                }
                if (value) {
                    this.setVisible(value);
                    this.listen();
                }
                else {
                    this.closable = false;
                    this.renderer.addClass(this.modalContent.nativeElement, 'fade-out-top');
                    setTimeout((/**
                     * @return {?}
                     */
                    function () {
                        _this.setVisible(value);
                        _this.renderer.removeClass(_this.modalContent.nativeElement, 'fade-out-top');
                        _this.ngOnDestroy();
                    }), 350);
                }
            },
            enumerable: true,
            configurable: true
        });
        /**
         * @return {?}
         */
        ModalComponent.prototype.ngOnDestroy = /**
         * @return {?}
         */
        function () {
            this.destroy$.next();
        };
        /**
         * @param {?} value
         * @return {?}
         */
        ModalComponent.prototype.setVisible = /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            var _this = this;
            this._visible = value;
            this.visibleChange.emit(value);
            value
                ? rxjs.timer(500)
                    .pipe(operators.take(1))
                    .subscribe((/**
                 * @param {?} _
                 * @return {?}
                 */
                function (_) { return (_this.closable = true); }))
                : (this.closable = false);
        };
        /**
         * @return {?}
         */
        ModalComponent.prototype.listen = /**
         * @return {?}
         */
        function () {
            var _this = this;
            rxjs.fromEvent(document, 'click')
                .pipe(operators.debounceTime(350), operators.takeUntil(this.destroy$), operators.filter((/**
             * @param {?} event
             * @return {?}
             */
            function (event) {
                return event &&
                    _this.closable &&
                    _this.modalContent &&
                    !_this.isOpenConfirmation &&
                    !_this.modalContent.nativeElement.contains(event.target);
            })))
                .subscribe((/**
             * @param {?} _
             * @return {?}
             */
            function (_) {
                _this.close();
            }));
            rxjs.fromEvent(document, 'keyup')
                .pipe(operators.takeUntil(this.destroy$), operators.filter((/**
             * @param {?} key
             * @return {?}
             */
            function (key) { return key && key.code === 'Escape' && _this.closable; })), operators.debounceTime(350))
                .subscribe((/**
             * @param {?} _
             * @return {?}
             */
            function (_) {
                _this.close();
            }));
            if (!this.abpClose)
                return;
            rxjs.fromEvent(this.abpClose.nativeElement, 'click')
                .pipe(operators.takeUntil(this.destroy$), operators.filter((/**
             * @return {?}
             */
            function () { return !!(_this.closable && _this.modalContent); })), operators.debounceTime(350))
                .subscribe((/**
             * @return {?}
             */
            function () { return _this.close(); }));
        };
        /**
         * @return {?}
         */
        ModalComponent.prototype.close = /**
         * @return {?}
         */
        function () {
            var _this = this;
            /** @type {?} */
            var nodes = getFlatNodes(((/** @type {?} */ (this.modalContent.nativeElement.querySelector('#abp-modal-body')))).childNodes);
            if (hasNgDirty(nodes)) {
                if (this.isOpenConfirmation)
                    return;
                this.isOpenConfirmation = true;
                this.confirmationService
                    .warn('AbpAccount::AreYouSureYouWantToCancelEditingWarningMessage', 'AbpAccount::AreYouSure')
                    .subscribe((/**
                 * @param {?} status
                 * @return {?}
                 */
                function (status) {
                    rxjs.timer(400).subscribe((/**
                     * @return {?}
                     */
                    function () {
                        _this.isOpenConfirmation = false;
                    }));
                    if (status === "confirm" /* confirm */) {
                        _this.visible = false;
                    }
                }));
            }
            else {
                this.visible = false;
            }
        };
        ModalComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-modal',
                        template: "<div\n  id=\"abp-modal\"\n  tabindex=\"-1\"\n  class=\"modal fade {{ modalClass }}\"\n  [class.show]=\"visible\"\n  [style.display]=\"visible ? 'block' : 'none'\"\n  [style.padding-right.px]=\"'15'\"\n>\n  <div\n    id=\"abp-modal-container\"\n    class=\"modal-dialog modal-{{ size }} fade-in-top\"\n    [class.modal-dialog-centered]=\"centered\"\n    #abpModalContent\n  >\n    <div #content id=\"abp-modal-content\" class=\"modal-content\">\n      <div id=\"abp-modal-header\" class=\"modal-header\">\n        <ng-container *ngTemplateOutlet=\"abpHeader\"></ng-container>\n\n        <button id=\"abp-modal-close-button\" type=\"button\" class=\"close\" (click)=\"close()\">\n          <span aria-hidden=\"true\">&times;</span>\n        </button>\n      </div>\n      <div id=\"abp-modal-body\" class=\"modal-body\">\n        <ng-container *ngTemplateOutlet=\"abpBody\"></ng-container>\n\n        <div id=\"abp-modal-footer\" class=\"modal-footer\">\n          <ng-container *ngTemplateOutlet=\"abpFooter\"></ng-container>\n        </div>\n      </div>\n    </div>\n  </div>\n\n  <ng-content></ng-content>\n</div>\n"
                    }] }
        ];
        /** @nocollapse */
        ModalComponent.ctorParameters = function () { return [
            { type: core.Renderer2 },
            { type: ConfirmationService }
        ]; };
        ModalComponent.propDecorators = {
            visible: [{ type: core.Input }],
            centered: [{ type: core.Input }],
            modalClass: [{ type: core.Input }],
            size: [{ type: core.Input }],
            visibleChange: [{ type: core.Output }],
            abpHeader: [{ type: core.ContentChild, args: ['abpHeader', { static: false },] }],
            abpBody: [{ type: core.ContentChild, args: ['abpBody', { static: false },] }],
            abpFooter: [{ type: core.ContentChild, args: ['abpFooter', { static: false },] }],
            abpClose: [{ type: core.ContentChild, args: ['abpClose', { static: false, read: core.ElementRef },] }],
            modalContent: [{ type: core.ViewChild, args: ['abpModalContent', { static: false },] }]
        };
        return ModalComponent;
    }());
    /**
     * @param {?} nodes
     * @return {?}
     */
    function getFlatNodes(nodes) {
        return Array.from(nodes).reduce((/**
         * @param {?} acc
         * @param {?} val
         * @return {?}
         */
        function (acc, val) { return __spread(acc, (val.childNodes && val.childNodes.length ? Array.from(val.childNodes) : [val])); }), []);
    }
    /**
     * @param {?} nodes
     * @return {?}
     */
    function hasNgDirty(nodes) {
        return nodes.findIndex((/**
         * @param {?} node
         * @return {?}
         */
        function (node) { return (node.className || '').indexOf('ng-dirty') > -1; })) > -1;
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ToastComponent = /** @class */ (function () {
        function ToastComponent() {
        }
        ToastComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-toast',
                        template: "\n    <p-toast position=\"bottom-right\" key=\"abpToast\" [baseZIndex]=\"1000\">\n      <ng-template let-message pTemplate=\"message\">\n        <span\n          class=\"ui-toast-icon pi\"\n          [ngClass]=\"{\n            'pi-info-circle': message.severity == 'info',\n            'pi-exclamation-triangle': message.severity == 'warn',\n            'pi-times': message.severity == 'error',\n            'pi-check': message.severity == 'success'\n          }\"\n        ></span>\n        <div class=\"ui-toast-message-text-content\">\n          <div class=\"ui-toast-summary\">{{ message.summary | abpLocalization: message.titleLocalizationParams }}</div>\n          <div class=\"ui-toast-detail\">{{ message.detail | abpLocalization: message.messageLocalizationParams }}</div>\n        </div>\n      </ng-template>\n    </p-toast>\n  "
                    }] }
        ];
        return ToastComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var styles = "\n.is-invalid .form-control {\n  border-color: #dc3545;\n  border-style: solid !important;\n}\n\n.is-invalid .invalid-feedback,\n.is-invalid + * .invalid-feedback {\n  display: block;\n}\n\n.data-tables-filter {\n  text-align: right;\n}\n\n.pointer {\n  cursor: pointer;\n}\n\n.navbar .dropdown-submenu a::after {\n  transform: rotate(-90deg);\n  position: absolute;\n  right: 16px;\n  top: 18px;\n}\n\n.modal {\n background-color: rgba(0, 0, 0, .6);\n}\n\n.abp-ellipsis {\n  display: inline-block;\n  overflow: hidden;\n  text-overflow: ellipsis;\n  white-space: nowrap;\n}\n\n/* <animations */\n\n.fade-in-top {\n  animation: fadeInTop 0.4s ease-in-out;\n}\n\n.fade-out-top {\n  animation: fadeOutTop 0.4s ease-in-out;\n}\n\n\n@keyframes fadeInTop {\n  from {\n    transform: translateY(-5px);\n    opacity: 0;\n  }\n\n  to {\n    transform: translateY(5px);\n    opacity: 1;\n  }\n}\n\n@keyframes fadeOutTop {\n  to {\n    transform: translateY(-5px);\n    opacity: 0;\n  }\n}\n\n/* </animations */\n\n";

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /** @type {?} */
    var DEFAULTS = {
        defaultError: {
            message: 'An error has occurred!',
            details: 'Error detail not sent by server.',
        },
        defaultError401: {
            message: 'You are not authenticated!',
            details: 'You should be authenticated (sign in) in order to perform this operation.',
        },
        defaultError403: {
            message: 'You are not authorized!',
            details: 'You are not allowed to perform this operation.',
        },
        defaultError404: {
            message: 'Resource not found!',
            details: 'The resource requested could not found on the server.',
        },
    };
    var ErrorHandler = /** @class */ (function () {
        function ErrorHandler(actions, store$1, confirmationService, appRef, cfRes, rendererFactory, injector) {
            var _this = this;
            this.actions = actions;
            this.store = store$1;
            this.confirmationService = confirmationService;
            this.appRef = appRef;
            this.cfRes = cfRes;
            this.rendererFactory = rendererFactory;
            this.injector = injector;
            actions.pipe(store.ofActionSuccessful(ng_core.RestOccurError)).subscribe((/**
             * @param {?} res
             * @return {?}
             */
            function (res) {
                var _a = res.payload, err = _a === void 0 ? (/** @type {?} */ ({})) : _a;
                /** @type {?} */
                var body = ((/** @type {?} */ (err))).error.error;
                if (err.headers.get('_AbpErrorFormat')) {
                    /** @type {?} */
                    var confirmation$ = _this.showError(null, null, body);
                    if (err.status === 401) {
                        confirmation$.subscribe((/**
                         * @return {?}
                         */
                        function () {
                            _this.navigateToLogin();
                        }));
                    }
                }
                else {
                    switch (((/** @type {?} */ (err))).status) {
                        case 401:
                            _this.showError(DEFAULTS.defaultError401.details, DEFAULTS.defaultError401.message).subscribe((/**
                             * @return {?}
                             */
                            function () {
                                return _this.navigateToLogin();
                            }));
                            break;
                        case 403:
                            _this.createErrorComponent({
                                title: DEFAULTS.defaultError403.message,
                                details: DEFAULTS.defaultError403.details,
                            });
                            break;
                        case 404:
                            _this.showError(DEFAULTS.defaultError404.details, DEFAULTS.defaultError404.message);
                            break;
                        case 500:
                            _this.createErrorComponent({
                                title: '500',
                                details: 'Sorry, an error has occured.',
                            });
                            break;
                        case 0:
                            if (((/** @type {?} */ (err))).statusText === 'Unknown Error') {
                                _this.createErrorComponent({
                                    title: 'Unknown Error',
                                    details: 'Sorry, an error has occured.',
                                });
                            }
                            break;
                        default:
                            _this.showError(DEFAULTS.defaultError.details, DEFAULTS.defaultError.message);
                            break;
                    }
                }
            }));
        }
        /**
         * @private
         * @param {?=} message
         * @param {?=} title
         * @param {?=} body
         * @return {?}
         */
        ErrorHandler.prototype.showError = /**
         * @private
         * @param {?=} message
         * @param {?=} title
         * @param {?=} body
         * @return {?}
         */
        function (message, title, body) {
            if (body) {
                if (body.details) {
                    message = body.details;
                    title = body.message;
                }
                else {
                    message = body.message || DEFAULTS.defaultError.message;
                }
            }
            return this.confirmationService.error(message, title, {
                hideCancelBtn: true,
                yesCopy: 'OK',
            });
        };
        /**
         * @private
         * @return {?}
         */
        ErrorHandler.prototype.navigateToLogin = /**
         * @private
         * @return {?}
         */
        function () {
            this.store.dispatch(new routerPlugin.Navigate(['/account/login'], null, {
                state: { redirectUrl: this.store.selectSnapshot(routerPlugin.RouterState).state.url },
            }));
        };
        /**
         * @param {?} instance
         * @return {?}
         */
        ErrorHandler.prototype.createErrorComponent = /**
         * @param {?} instance
         * @return {?}
         */
        function (instance) {
            /** @type {?} */
            var renderer = this.rendererFactory.createRenderer(null, null);
            /** @type {?} */
            var host = renderer.selectRootElement('app-root', true);
            /** @type {?} */
            var componentRef = this.cfRes.resolveComponentFactory(ErrorComponent).create(this.injector);
            for (var key in componentRef.instance) {
                if (componentRef.instance.hasOwnProperty(key)) {
                    componentRef.instance[key] = instance[key];
                }
            }
            this.appRef.attachView(componentRef.hostView);
            renderer.appendChild(host, ((/** @type {?} */ (componentRef.hostView))).rootNodes[0]);
            componentRef.instance.renderer = renderer;
            componentRef.instance.elementRef = componentRef.location;
            componentRef.instance.host = host;
        };
        ErrorHandler.decorators = [
            { type: core.Injectable, args: [{ providedIn: 'root' },] }
        ];
        /** @nocollapse */
        ErrorHandler.ctorParameters = function () { return [
            { type: store.Actions },
            { type: store.Store },
            { type: ConfirmationService },
            { type: core.ApplicationRef },
            { type: core.ComponentFactoryResolver },
            { type: core.RendererFactory2 },
            { type: core.Injector }
        ]; };
        /** @nocollapse */ ErrorHandler.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function ErrorHandler_Factory() { return new ErrorHandler(core.ɵɵinject(store.Actions), core.ɵɵinject(store.Store), core.ɵɵinject(ConfirmationService), core.ɵɵinject(core.ApplicationRef), core.ɵɵinject(core.ComponentFactoryResolver), core.ɵɵinject(core.RendererFactory2), core.ɵɵinject(core.INJECTOR)); }, token: ErrorHandler, providedIn: "root" });
        return ErrorHandler;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @param {?} injector
     * @return {?}
     */
    function appendScript(injector) {
        /** @type {?} */
        var fn = (/**
         * @return {?}
         */
        function () {
            /** @type {?} */
            var lazyLoadService = injector.get(ng_core.LazyLoadService);
            return rxjs.forkJoin(lazyLoadService.load(null, 'style', styles, 'head', 'afterbegin') /* lazyLoadService.load(null, 'script', scripts) */).pipe(operators.take(1));
        });
        return fn;
    }
    var ThemeSharedModule = /** @class */ (function () {
        function ThemeSharedModule() {
        }
        /**
         * @return {?}
         */
        ThemeSharedModule.forRoot = /**
         * @return {?}
         */
        function () {
            return {
                ngModule: ThemeSharedModule,
                providers: [
                    {
                        provide: core.APP_INITIALIZER,
                        multi: true,
                        deps: [core.Injector, ErrorHandler],
                        useFactory: appendScript,
                    },
                    { provide: messageservice.MessageService, useClass: messageservice.MessageService },
                ],
            };
        };
        ThemeSharedModule.decorators = [
            { type: core.NgModule, args: [{
                        imports: [
                            ng_core.CoreModule,
                            toast.ToastModule,
                            ngBootstrap.NgbModalModule,
                            core$1.NgxValidateCoreModule.forRoot({
                                targetSelector: '.form-group',
                            }),
                        ],
                        declarations: [ConfirmationComponent, ToastComponent, ModalComponent, ErrorComponent, LoaderBarComponent],
                        exports: [ngBootstrap.NgbModalModule, ConfirmationComponent, ToastComponent, ModalComponent, LoaderBarComponent],
                        entryComponents: [ErrorComponent],
                    },] }
        ];
        return ThemeSharedModule;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var Confirmation;
    (function (Confirmation) {
        /**
         * @record
         */
        function Options() { }
        Confirmation.Options = Options;
    })(Confirmation || (Confirmation = {}));

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    (function (Toaster) {
        /**
         * @record
         */
        function Options() { }
        Toaster.Options = Options;
    })(exports.Toaster || (exports.Toaster = {}));

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ToasterService = /** @class */ (function (_super) {
        __extends(ToasterService, _super);
        function ToasterService() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        /**
         * @param {?} messages
         * @return {?}
         */
        ToasterService.prototype.addAll = /**
         * @param {?} messages
         * @return {?}
         */
        function (messages) {
            var _this = this;
            this.messageService.addAll(messages.map((/**
             * @param {?} message
             * @return {?}
             */
            function (message) { return (__assign({ key: _this.key }, message)); })));
        };
        ToasterService.decorators = [
            { type: core.Injectable, args: [{ providedIn: 'root' },] }
        ];
        /** @nocollapse */ ToasterService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function ToasterService_Factory() { return new ToasterService(core.ɵɵinject(messageservice.MessageService)); }, token: ToasterService, providedIn: "root" });
        return ToasterService;
    }(AbstractToasterClass));

    exports.ConfirmationComponent = ConfirmationComponent;
    exports.ConfirmationService = ConfirmationService;
    exports.ModalComponent = ModalComponent;
    exports.ThemeSharedModule = ThemeSharedModule;
    exports.ToastComponent = ToastComponent;
    exports.ToasterService = ToasterService;
    exports.appendScript = appendScript;
    exports.ɵa = ConfirmationComponent;
    exports.ɵb = ConfirmationService;
    exports.ɵc = AbstractToasterClass;
    exports.ɵd = ToastComponent;
    exports.ɵe = ModalComponent;
    exports.ɵf = ErrorComponent;
    exports.ɵg = LoaderBarComponent;
    exports.ɵh = ErrorHandler;

    Object.defineProperty(exports, '__esModule', { value: true });

}));
//# sourceMappingURL=abp-ng.theme.shared.umd.js.map
