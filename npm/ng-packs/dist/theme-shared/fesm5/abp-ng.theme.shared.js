import { StartLoader, StopLoader, RestOccurError, LazyLoadService, CoreModule } from '@abp/ng.core';
import { Injectable, ɵɵdefineInjectable, ɵɵinject, Component, Input, EventEmitter, Renderer2, Output, ContentChild, ElementRef, ViewChild, ViewChildren, ApplicationRef, ComponentFactoryResolver, RendererFactory2, Injector, INJECTOR, ChangeDetectionStrategy, ViewEncapsulation, APP_INITIALIZER, NgModule } from '@angular/core';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { takeUntilDestroy, ValidationErrorComponent as ValidationErrorComponent$1, NgxValidateCoreModule } from '@ngx-validate/core';
import { MessageService } from 'primeng/components/common/messageservice';
import { ToastModule } from 'primeng/toast';
import { Subject, timer, fromEvent, forkJoin } from 'rxjs';
import { filter, take, takeUntil, debounceTime } from 'rxjs/operators';
import { __assign, __extends, __spread } from 'tslib';
import { NavigationStart, NavigationEnd, Router } from '@angular/router';
import { ofActionSuccessful, Actions, Store } from '@ngxs/store';
import { Navigate, RouterState } from '@ngxs/router-plugin';
import snq from 'snq';
import { trigger, state, style, transition, animate } from '@angular/animations';

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/**
 * @template T
 */
var  /**
 * @template T
 */
AbstractToaster = /** @class */ (function () {
    function AbstractToaster(messageService) {
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
    AbstractToaster.prototype.info = /**
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
    AbstractToaster.prototype.success = /**
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
    AbstractToaster.prototype.warn = /**
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
    AbstractToaster.prototype.error = /**
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
    AbstractToaster.prototype.show = /**
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
        this.status$ = new Subject();
        return this.status$;
    };
    /**
     * @param {?=} status
     * @return {?}
     */
    AbstractToaster.prototype.clear = /**
     * @param {?=} status
     * @return {?}
     */
    function (status) {
        this.messageService.clear(this.key);
        this.status$.next(status || "dismiss" /* dismiss */);
        this.status$.complete();
    };
    return AbstractToaster;
}());
if (false) {
    /** @type {?} */
    AbstractToaster.prototype.status$;
    /** @type {?} */
    AbstractToaster.prototype.key;
    /** @type {?} */
    AbstractToaster.prototype.sticky;
    /**
     * @type {?}
     * @protected
     */
    AbstractToaster.prototype.messageService;
}

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
        { type: Injectable, args: [{ providedIn: 'root' },] }
    ];
    /** @nocollapse */ ConfirmationService.ngInjectableDef = ɵɵdefineInjectable({ factory: function ConfirmationService_Factory() { return new ConfirmationService(ɵɵinject(MessageService)); }, token: ConfirmationService, providedIn: "root" });
    return ConfirmationService;
}(AbstractToaster));
if (false) {
    /** @type {?} */
    ConfirmationService.prototype.key;
    /** @type {?} */
    ConfirmationService.prototype.sticky;
}

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
        { type: Component, args: [{
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
if (false) {
    /** @type {?} */
    ConfirmationComponent.prototype.confirm;
    /** @type {?} */
    ConfirmationComponent.prototype.reject;
    /** @type {?} */
    ConfirmationComponent.prototype.dismiss;
    /**
     * @type {?}
     * @private
     */
    ConfirmationComponent.prototype.confirmationService;
}

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
        { type: Component, args: [{
                    selector: 'abp-error',
                    template: "\n    <div class=\"error\">\n      <button id=\"abp-close-button mr-2\" type=\"button\" class=\"close\" (click)=\"destroy()\">\n        <span aria-hidden=\"true\">&times;</span>\n      </button>\n      <div class=\"row centered\">\n        <div class=\"col-md-12\">\n          <div class=\"error-template\">\n            <h1>\n              {{ title | abpLocalization }}\n            </h1>\n            <div class=\"error-details\">\n              {{ details | abpLocalization }}\n            </div>\n            <div class=\"error-actions\">\n              <a (click)=\"destroy()\" routerLink=\"/\" class=\"btn btn-primary btn-md mt-2\"\n                ><span class=\"glyphicon glyphicon-home\"></span> {{ '::Menu:Home' | abpLocalization }}\n              </a>\n            </div>\n          </div>\n        </div>\n      </div>\n    </div>\n  ",
                    styles: [".error{position:fixed;top:0;background-color:#fff;width:100vw;height:100vh;z-index:999999}.centered{position:fixed;top:50%;left:50%;transform:translate(-50%,-50%)}"]
                }] }
    ];
    return ErrorComponent;
}());
if (false) {
    /** @type {?} */
    ErrorComponent.prototype.title;
    /** @type {?} */
    ErrorComponent.prototype.details;
    /** @type {?} */
    ErrorComponent.prototype.renderer;
    /** @type {?} */
    ErrorComponent.prototype.elementRef;
    /** @type {?} */
    ErrorComponent.prototype.host;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var LoaderBarComponent = /** @class */ (function () {
    function LoaderBarComponent(actions, router) {
        var _this = this;
        this.actions = actions;
        this.router = router;
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
            .pipe(ofActionSuccessful(StartLoader, StopLoader), filter(this.filter), takeUntilDestroy(this))
            .subscribe((/**
         * @param {?} action
         * @return {?}
         */
        function (action) {
            if (action instanceof StartLoader)
                _this.startLoading();
            else
                _this.stopLoading();
        }));
        router.events
            .pipe(filter((/**
         * @param {?} event
         * @return {?}
         */
        function (event) { return event instanceof NavigationStart || event instanceof NavigationEnd; })), takeUntilDestroy(this))
            .subscribe((/**
         * @param {?} event
         * @return {?}
         */
        function (event) {
            if (event instanceof NavigationStart)
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
        { type: Component, args: [{
                    selector: 'abp-loader-bar',
                    template: "\n    <div id=\"abp-loader-bar\" [ngClass]=\"containerClass\" [class.is-loading]=\"isLoading\">\n      <div [ngClass]=\"progressClass\" [style.width.vw]=\"progressLevel\"></div>\n    </div>\n  ",
                    styles: [".abp-loader-bar{left:0;opacity:0;position:fixed;top:0;transition:opacity .4s linear .4s;z-index:99999}.abp-loader-bar.is-loading{opacity:1;transition:none}.abp-loader-bar .abp-progress{background:#77b6ff;box-shadow:0 0 10px rgba(119,182,255,.7);height:2px;left:0;position:fixed;top:0;transition:width .4s}"]
                }] }
    ];
    /** @nocollapse */
    LoaderBarComponent.ctorParameters = function () { return [
        { type: Actions },
        { type: Router }
    ]; };
    LoaderBarComponent.propDecorators = {
        containerClass: [{ type: Input }],
        progressClass: [{ type: Input }],
        isLoading: [{ type: Input }],
        filter: [{ type: Input }]
    };
    return LoaderBarComponent;
}());
if (false) {
    /** @type {?} */
    LoaderBarComponent.prototype.containerClass;
    /** @type {?} */
    LoaderBarComponent.prototype.progressClass;
    /** @type {?} */
    LoaderBarComponent.prototype.isLoading;
    /** @type {?} */
    LoaderBarComponent.prototype.filter;
    /** @type {?} */
    LoaderBarComponent.prototype.progressLevel;
    /** @type {?} */
    LoaderBarComponent.prototype.interval;
    /**
     * @type {?}
     * @private
     */
    LoaderBarComponent.prototype.actions;
    /**
     * @type {?}
     * @private
     */
    LoaderBarComponent.prototype.router;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var ButtonComponent = /** @class */ (function () {
    function ButtonComponent() {
        this.buttonClass = 'btn btn-primary';
        this.type = 'button';
        this.loading = false;
        this.disabled = false;
    }
    Object.defineProperty(ButtonComponent.prototype, "icon", {
        get: /**
         * @return {?}
         */
        function () {
            return "" + (this.loading ? 'fa fa-spin fa-spinner' : this.iconClass || 'd-none');
        },
        enumerable: true,
        configurable: true
    });
    ButtonComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-button',
                    template: "\n    <button [attr.type]=\"type\" [ngClass]=\"buttonClass\" [disabled]=\"loading || disabled\">\n      <i [ngClass]=\"icon\" class=\"mr-1\"></i><ng-content></ng-content>\n    </button>\n  "
                }] }
    ];
    ButtonComponent.propDecorators = {
        buttonClass: [{ type: Input }],
        type: [{ type: Input }],
        iconClass: [{ type: Input }],
        loading: [{ type: Input }],
        disabled: [{ type: Input }]
    };
    return ButtonComponent;
}());
if (false) {
    /** @type {?} */
    ButtonComponent.prototype.buttonClass;
    /** @type {?} */
    ButtonComponent.prototype.type;
    /** @type {?} */
    ButtonComponent.prototype.iconClass;
    /** @type {?} */
    ButtonComponent.prototype.loading;
    /** @type {?} */
    ButtonComponent.prototype.disabled;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
var ANIMATION_TIMEOUT = 200;
var ModalComponent = /** @class */ (function () {
    function ModalComponent(renderer, confirmationService) {
        this.renderer = renderer;
        this.confirmationService = confirmationService;
        this.centered = false;
        this.modalClass = '';
        this.size = 'lg';
        this.visibleChange = new EventEmitter();
        this.init = new EventEmitter();
        this._visible = false;
        this._busy = false;
        this.showModal = false;
        this.isOpenConfirmation = false;
        this.closable = false;
        this.destroy$ = new Subject();
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
            if (typeof value !== 'boolean')
                return;
            if (!this.modalContent) {
                if (value) {
                    setTimeout((/**
                     * @return {?}
                     */
                    function () {
                        _this.showModal = value;
                        _this.visible = value;
                    }), 0);
                }
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
                    _this.ngOnDestroy();
                }), ANIMATION_TIMEOUT - 10);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ModalComponent.prototype, "busy", {
        get: /**
         * @return {?}
         */
        function () {
            return this._busy;
        },
        set: /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            if (this.abpSubmit && this.abpSubmit instanceof ButtonComponent) {
                this.abpSubmit.loading = value;
            }
            this._busy = value;
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
        this.showModal = value;
        value
            ? timer(ANIMATION_TIMEOUT + 100)
                .pipe(take(1))
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
        fromEvent(document, 'keyup')
            .pipe(takeUntil(this.destroy$), debounceTime(150), filter((/**
         * @param {?} key
         * @return {?}
         */
        function (key) { return key && key.code === 'Escape' && _this.closable; })))
            .subscribe((/**
         * @param {?} _
         * @return {?}
         */
        function (_) {
            _this.close();
        }));
        setTimeout((/**
         * @return {?}
         */
        function () {
            if (!_this.abpClose)
                return;
            fromEvent(_this.abpClose.nativeElement, 'click')
                .pipe(takeUntil(_this.destroy$), filter((/**
             * @return {?}
             */
            function () { return !!(_this.closable && _this.modalContent); })))
                .subscribe((/**
             * @return {?}
             */
            function () { return _this.close(); }));
        }), 0);
        this.init.emit();
    };
    /**
     * @return {?}
     */
    ModalComponent.prototype.close = /**
     * @return {?}
     */
    function () {
        var _this = this;
        if (!this.closable || this.busy)
            return;
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
                timer(ANIMATION_TIMEOUT).subscribe((/**
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
        { type: Component, args: [{
                    selector: 'abp-modal',
                    template: "<div\n  *ngIf=\"showModal\"\n  (click)=\"close()\"\n  id=\"abp-modal\"\n  class=\"modal fade {{ modalClass }} d-block show\"\n  [style.padding-right.px]=\"'15'\"\n>\n  <div\n    id=\"abp-modal-container\"\n    class=\"modal-dialog modal-{{ size }} fade-in-top\"\n    tabindex=\"-1\"\n    [class.modal-dialog-centered]=\"centered\"\n    #abpModalContent\n  >\n    <div #content id=\"abp-modal-content\" class=\"modal-content\" (click)=\"$event.stopPropagation()\">\n      <div id=\"abp-modal-header\" class=\"modal-header\">\n        <ng-container *ngTemplateOutlet=\"abpHeader\"></ng-container>\n\n        <button id=\"abp-modal-close-button\" type=\"button\" class=\"close\" (click)=\"close()\">\n          <span aria-hidden=\"true\">&times;</span>\n        </button>\n      </div>\n      <div\n        id=\"abp-modal-body\"\n        class=\"modal-body\"\n        [style.height]=\"height || undefined\"\n        [style.minHeight]=\"minHeight || undefined\"\n      >\n        <ng-container *ngTemplateOutlet=\"abpBody\"></ng-container>\n\n        <div id=\"abp-modal-footer\" class=\"modal-footer\">\n          <ng-container *ngTemplateOutlet=\"abpFooter\"></ng-container>\n        </div>\n      </div>\n    </div>\n  </div>\n\n  <ng-content></ng-content>\n</div>\n"
                }] }
    ];
    /** @nocollapse */
    ModalComponent.ctorParameters = function () { return [
        { type: Renderer2 },
        { type: ConfirmationService }
    ]; };
    ModalComponent.propDecorators = {
        visible: [{ type: Input }],
        busy: [{ type: Input }],
        centered: [{ type: Input }],
        modalClass: [{ type: Input }],
        size: [{ type: Input }],
        height: [{ type: Input }],
        minHeight: [{ type: Input }],
        visibleChange: [{ type: Output }],
        init: [{ type: Output }],
        abpHeader: [{ type: ContentChild, args: ['abpHeader', { static: false },] }],
        abpBody: [{ type: ContentChild, args: ['abpBody', { static: false },] }],
        abpFooter: [{ type: ContentChild, args: ['abpFooter', { static: false },] }],
        abpClose: [{ type: ContentChild, args: ['abpClose', { static: false, read: ElementRef },] }],
        abpSubmit: [{ type: ContentChild, args: [ButtonComponent, { static: false, read: ButtonComponent },] }],
        modalContent: [{ type: ViewChild, args: ['abpModalContent', { static: false },] }],
        abpButtons: [{ type: ViewChildren, args: ['abp-button',] }]
    };
    return ModalComponent;
}());
if (false) {
    /** @type {?} */
    ModalComponent.prototype.centered;
    /** @type {?} */
    ModalComponent.prototype.modalClass;
    /** @type {?} */
    ModalComponent.prototype.size;
    /** @type {?} */
    ModalComponent.prototype.height;
    /** @type {?} */
    ModalComponent.prototype.minHeight;
    /** @type {?} */
    ModalComponent.prototype.visibleChange;
    /** @type {?} */
    ModalComponent.prototype.init;
    /** @type {?} */
    ModalComponent.prototype.abpHeader;
    /** @type {?} */
    ModalComponent.prototype.abpBody;
    /** @type {?} */
    ModalComponent.prototype.abpFooter;
    /** @type {?} */
    ModalComponent.prototype.abpClose;
    /** @type {?} */
    ModalComponent.prototype.abpSubmit;
    /** @type {?} */
    ModalComponent.prototype.modalContent;
    /** @type {?} */
    ModalComponent.prototype.abpButtons;
    /** @type {?} */
    ModalComponent.prototype._visible;
    /** @type {?} */
    ModalComponent.prototype._busy;
    /** @type {?} */
    ModalComponent.prototype.showModal;
    /** @type {?} */
    ModalComponent.prototype.isOpenConfirmation;
    /** @type {?} */
    ModalComponent.prototype.closable;
    /** @type {?} */
    ModalComponent.prototype.destroy$;
    /**
     * @type {?}
     * @private
     */
    ModalComponent.prototype.renderer;
    /**
     * @type {?}
     * @private
     */
    ModalComponent.prototype.confirmationService;
}
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
    function (acc, val) { return __spread(acc, (val.childNodes && val.childNodes.length ? getFlatNodes(val.childNodes) : [val])); }), []);
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
        { type: Component, args: [{
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
var styles = "\n.is-invalid .form-control {\n  border-color: #dc3545;\n  border-style: solid !important;\n}\n\n.is-invalid .invalid-feedback,\n.is-invalid + * .invalid-feedback {\n  display: block;\n}\n\n.data-tables-filter {\n  text-align: right;\n}\n\n.pointer {\n  cursor: pointer;\n}\n\n.navbar .dropdown-submenu a::after {\n  transform: rotate(-90deg);\n  position: absolute;\n  right: 16px;\n  top: 18px;\n}\n\n.navbar .dropdown-menu {\n  min-width: 215px;\n}\n\n.modal {\n background-color: rgba(0, 0, 0, .6);\n}\n\n.abp-ellipsis {\n  display: inline-block;\n  overflow: hidden;\n  text-overflow: ellipsis;\n  white-space: nowrap;\n}\n\n/* <animations */\n\n.fade-in-top {\n  animation: fadeInTop 0.2s ease-in-out;\n}\n\n.fade-out-top {\n  animation: fadeOutTop 0.2s ease-in-out;\n}\n\n\n@keyframes fadeInTop {\n  from {\n    transform: translateY(-5px);\n    opacity: 0;\n  }\n\n  to {\n    transform: translateY(5px);\n    opacity: 1;\n  }\n}\n\n@keyframes fadeOutTop {\n  to {\n    transform: translateY(-5px);\n    opacity: 0;\n  }\n}\n\n/* </animations */\n\n";

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
    function ErrorHandler(actions, store, confirmationService, appRef, cfRes, rendererFactory, injector) {
        var _this = this;
        this.actions = actions;
        this.store = store;
        this.confirmationService = confirmationService;
        this.appRef = appRef;
        this.cfRes = cfRes;
        this.rendererFactory = rendererFactory;
        this.injector = injector;
        actions.pipe(ofActionSuccessful(RestOccurError)).subscribe((/**
         * @param {?} res
         * @return {?}
         */
        function (res) {
            var _a = res.payload, err = _a === void 0 ? (/** @type {?} */ ({})) : _a;
            /** @type {?} */
            var body = snq((/**
             * @return {?}
             */
            function () { return ((/** @type {?} */ (err))).error.error; }), DEFAULTS.defaultError.message);
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
                            details: 'AbpAccount::InternalServerErrorMessage',
                        });
                        break;
                    case 0:
                        if (((/** @type {?} */ (err))).statusText === 'Unknown Error') {
                            _this.createErrorComponent({
                                title: 'Unknown Error',
                                details: 'AbpAccount::InternalServerErrorMessage',
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
        this.store.dispatch(new Navigate(['/account/login'], null, {
            state: { redirectUrl: this.store.selectSnapshot(RouterState).state.url },
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
        { type: Injectable, args: [{ providedIn: 'root' },] }
    ];
    /** @nocollapse */
    ErrorHandler.ctorParameters = function () { return [
        { type: Actions },
        { type: Store },
        { type: ConfirmationService },
        { type: ApplicationRef },
        { type: ComponentFactoryResolver },
        { type: RendererFactory2 },
        { type: Injector }
    ]; };
    /** @nocollapse */ ErrorHandler.ngInjectableDef = ɵɵdefineInjectable({ factory: function ErrorHandler_Factory() { return new ErrorHandler(ɵɵinject(Actions), ɵɵinject(Store), ɵɵinject(ConfirmationService), ɵɵinject(ApplicationRef), ɵɵinject(ComponentFactoryResolver), ɵɵinject(RendererFactory2), ɵɵinject(INJECTOR)); }, token: ErrorHandler, providedIn: "root" });
    return ErrorHandler;
}());
if (false) {
    /**
     * @type {?}
     * @private
     */
    ErrorHandler.prototype.actions;
    /**
     * @type {?}
     * @private
     */
    ErrorHandler.prototype.store;
    /**
     * @type {?}
     * @private
     */
    ErrorHandler.prototype.confirmationService;
    /**
     * @type {?}
     * @private
     */
    ErrorHandler.prototype.appRef;
    /**
     * @type {?}
     * @private
     */
    ErrorHandler.prototype.cfRes;
    /**
     * @type {?}
     * @private
     */
    ErrorHandler.prototype.rendererFactory;
    /**
     * @type {?}
     * @private
     */
    ErrorHandler.prototype.injector;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var ValidationErrorComponent = /** @class */ (function (_super) {
    __extends(ValidationErrorComponent, _super);
    function ValidationErrorComponent() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Object.defineProperty(ValidationErrorComponent.prototype, "abpErrors", {
        get: /**
         * @return {?}
         */
        function () {
            if (!this.errors || !this.errors.length)
                return [];
            return this.errors.map((/**
             * @param {?} error
             * @return {?}
             */
            function (error) {
                if (!error.message)
                    return error;
                /** @type {?} */
                var index = error.message.indexOf('[');
                if (index > -1) {
                    return __assign({}, error, { message: error.message.slice(0, index), interpoliteParams: error.message.slice(index + 1, error.message.length - 1).split(',') });
                }
                return error;
            }));
        },
        enumerable: true,
        configurable: true
    });
    ValidationErrorComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-validation-error',
                    template: "\n    <div class=\"invalid-feedback\" *ngFor=\"let error of abpErrors; trackBy: trackByFn\">\n      {{ error.message | abpLocalization: error.interpoliteParams }}\n    </div>\n  ",
                    changeDetection: ChangeDetectionStrategy.OnPush,
                    encapsulation: ViewEncapsulation.None
                }] }
    ];
    return ValidationErrorComponent;
}(ValidationErrorComponent$1));

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
        var lazyLoadService = injector.get(LazyLoadService);
        return forkJoin(lazyLoadService.load(null, 'style', styles, 'head', 'afterbegin') /* lazyLoadService.load(null, 'script', scripts) */).pipe(take(1));
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
                    provide: APP_INITIALIZER,
                    multi: true,
                    deps: [Injector, ErrorHandler],
                    useFactory: appendScript,
                },
                { provide: MessageService, useClass: MessageService },
            ],
        };
    };
    ThemeSharedModule.decorators = [
        { type: NgModule, args: [{
                    imports: [
                        CoreModule,
                        ToastModule,
                        NgbModalModule,
                        NgxValidateCoreModule.forRoot({
                            targetSelector: '.form-group',
                            blueprints: {
                                email: "AbpAccount::ThisFieldIsNotAValidEmailAddress.",
                                max: "AbpAccount::ThisFieldMustBeAStringWithAMaximumLengthOf{1}[{{ max }}]",
                                maxlength: "AbpAccount::ThisFieldMustBeAStringWithAMaximumLengthOf{1}[{{ requiredLength }}]",
                                min: "AbpAccount::ThisFieldMustBeAStringWithAMinimumLengthOf{1}AndAMaximumLengthOf{0}[{{ min }},{{ max }}]",
                                minlength: "AbpAccount::ThisFieldMustBeAStringWithAMinimumLengthOf{1}AndAMaximumLengthOf{0}[{{ min }},{{ max }}]",
                                required: "AbpAccount::ThisFieldIsRequired.",
                                passwordMismatch: "AbpIdentity::Identity.PasswordConfirmationFailed",
                            },
                            errorTemplate: ValidationErrorComponent,
                        }),
                    ],
                    declarations: [
                        ButtonComponent,
                        ConfirmationComponent,
                        ToastComponent,
                        ModalComponent,
                        ErrorComponent,
                        LoaderBarComponent,
                        ValidationErrorComponent,
                    ],
                    exports: [NgbModalModule, ButtonComponent, ConfirmationComponent, ToastComponent, ModalComponent, LoaderBarComponent],
                    entryComponents: [ErrorComponent, ValidationErrorComponent],
                },] }
    ];
    return ThemeSharedModule;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
var slideFromBottom = trigger('routeAnimations', [
    state('void', style({ 'margin-top': '20px', opacity: '0' })),
    state('*', style({ 'margin-top': '0px', opacity: '1' })),
    transition(':enter', [animate('0.2s ease-out', style({ opacity: '1', 'margin-top': '0px' }))]),
]);

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

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
    if (false) {
        /** @type {?|undefined} */
        Options.prototype.hideCancelBtn;
        /** @type {?|undefined} */
        Options.prototype.hideYesBtn;
        /** @type {?|undefined} */
        Options.prototype.cancelCopy;
        /** @type {?|undefined} */
        Options.prototype.yesCopy;
    }
})(Confirmation || (Confirmation = {}));

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var Toaster;
(function (Toaster) {
    /**
     * @record
     */
    function Options() { }
    Toaster.Options = Options;
    if (false) {
        /** @type {?|undefined} */
        Options.prototype.id;
        /** @type {?|undefined} */
        Options.prototype.closable;
        /** @type {?|undefined} */
        Options.prototype.life;
        /** @type {?|undefined} */
        Options.prototype.sticky;
        /** @type {?|undefined} */
        Options.prototype.data;
        /** @type {?|undefined} */
        Options.prototype.messageLocalizationParams;
        /** @type {?|undefined} */
        Options.prototype.titleLocalizationParams;
    }
})(Toaster || (Toaster = {}));

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

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
        { type: Injectable, args: [{ providedIn: 'root' },] }
    ];
    /** @nocollapse */ ToasterService.ngInjectableDef = ɵɵdefineInjectable({ factory: function ToasterService_Factory() { return new ToasterService(ɵɵinject(MessageService)); }, token: ToasterService, providedIn: "root" });
    return ToasterService;
}(AbstractToaster));

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

export { ButtonComponent, ConfirmationComponent, ConfirmationService, LoaderBarComponent, ModalComponent, ThemeSharedModule, ToastComponent, Toaster, ToasterService, appendScript, slideFromBottom, ValidationErrorComponent as ɵa, ButtonComponent as ɵb, ConfirmationComponent as ɵc, ConfirmationService as ɵd, AbstractToaster as ɵe, ToastComponent as ɵf, ModalComponent as ɵg, ErrorComponent as ɵh, LoaderBarComponent as ɵi, ErrorHandler as ɵj };
//# sourceMappingURL=abp-ng.theme.shared.js.map
