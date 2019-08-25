import { StartLoader, StopLoader, RestOccurError, LazyLoadService, CoreModule } from '@abp/ng.core';
import { Injectable, ɵɵdefineInjectable, ɵɵinject, Component, Input, EventEmitter, Renderer2, Output, ContentChild, ElementRef, ViewChild, ViewChildren, ApplicationRef, ComponentFactoryResolver, RendererFactory2, Injector, INJECTOR, ChangeDetectionStrategy, ViewEncapsulation, APP_INITIALIZER, NgModule } from '@angular/core';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { takeUntilDestroy, ValidationErrorComponent as ValidationErrorComponent$1, NgxValidateCoreModule } from '@ngx-validate/core';
import { MessageService } from 'primeng/components/common/messageservice';
import { ToastModule } from 'primeng/toast';
import { Subject, timer, fromEvent, forkJoin } from 'rxjs';
import { filter, take, takeUntil, debounceTime } from 'rxjs/operators';
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
class AbstractToaster {
    /**
     * @param {?} messageService
     */
    constructor(messageService) {
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
    info(message, title, options) {
        return this.show(message, title, 'info', options);
    }
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    success(message, title, options) {
        return this.show(message, title, 'success', options);
    }
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    warn(message, title, options) {
        return this.show(message, title, 'warn', options);
    }
    /**
     * @param {?} message
     * @param {?} title
     * @param {?=} options
     * @return {?}
     */
    error(message, title, options) {
        return this.show(message, title, 'error', options);
    }
    /**
     * @protected
     * @param {?} message
     * @param {?} title
     * @param {?} severity
     * @param {?=} options
     * @return {?}
     */
    show(message, title, severity, options) {
        this.messageService.clear(this.key);
        this.messageService.add(Object.assign({ severity, detail: message, summary: title }, options, { key: this.key }, (typeof (options || ((/** @type {?} */ ({})))).sticky === 'undefined' && { sticky: this.sticky })));
        this.status$ = new Subject();
        return this.status$;
    }
    /**
     * @param {?=} status
     * @return {?}
     */
    clear(status) {
        this.messageService.clear(this.key);
        this.status$.next(status || "dismiss" /* dismiss */);
        this.status$.complete();
    }
}
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
class ConfirmationService extends AbstractToaster {
    constructor() {
        super(...arguments);
        this.key = 'abpConfirmation';
        this.sticky = true;
    }
}
ConfirmationService.decorators = [
    { type: Injectable, args: [{ providedIn: 'root' },] }
];
/** @nocollapse */ ConfirmationService.ngInjectableDef = ɵɵdefineInjectable({ factory: function ConfirmationService_Factory() { return new ConfirmationService(ɵɵinject(MessageService)); }, token: ConfirmationService, providedIn: "root" });
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
class ConfirmationComponent {
    /**
     * @param {?} confirmationService
     */
    constructor(confirmationService) {
        this.confirmationService = confirmationService;
        this.confirm = "confirm" /* confirm */;
        this.reject = "reject" /* reject */;
        this.dismiss = "dismiss" /* dismiss */;
    }
    /**
     * @param {?} status
     * @return {?}
     */
    close(status) {
        this.confirmationService.clear(status);
    }
}
ConfirmationComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-confirmation',
                template: `
    <p-toast
      position="center"
      key="abpConfirmation"
      (onClose)="close(dismiss)"
      [modal]="true"
      [baseZIndex]="1000"
      styleClass=""
    >
      <ng-template let-message pTemplate="message">
        <div *ngIf="message.summary" class="modal-header">
          <h4 class="modal-title">
            {{ message.summary | abpLocalization: message.titleLocalizationParams }}
          </h4>
        </div>
        <div class="modal-body">
          {{ message.detail | abpLocalization: message.messageLocalizationParams }}
        </div>

        <div class="modal-footer justify-content-center">
          <button *ngIf="!message.hideCancelBtn" type="button" class="btn btn-secondary" (click)="close(reject)">
            {{ message.cancelCopy || 'AbpIdentity::Cancel' | abpLocalization }}
          </button>
          <button *ngIf="!message.hideYesBtn" type="button" class="btn btn-secondary" (click)="close(confirm)">
            <span>{{ message.yesCopy || 'AbpIdentity::Yes' | abpLocalization }}</span>
          </button>
        </div>
      </ng-template>
    </p-toast>
  `
            }] }
];
/** @nocollapse */
ConfirmationComponent.ctorParameters = () => [
    { type: ConfirmationService }
];
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
class ErrorComponent {
    constructor() {
        this.title = 'Oops!';
        this.details = 'Sorry, an error has occured.';
    }
    /**
     * @return {?}
     */
    destroy() {
        this.renderer.removeChild(this.host, this.elementRef.nativeElement);
    }
}
ErrorComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-error',
                template: `
    <div class="error">
      <button id="abp-close-button mr-2" type="button" class="close" (click)="destroy()">
        <span aria-hidden="true">&times;</span>
      </button>
      <div class="row centered">
        <div class="col-md-12">
          <div class="error-template">
            <h1>
              {{ title | abpLocalization }}
            </h1>
            <div class="error-details">
              {{ details | abpLocalization }}
            </div>
            <div class="error-actions">
              <a (click)="destroy()" routerLink="/" class="btn btn-primary btn-md mt-2"
                ><span class="glyphicon glyphicon-home"></span> {{ '::Menu:Home' | abpLocalization }}
              </a>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
                styles: [".error{position:fixed;top:0;background-color:#fff;width:100vw;height:100vh;z-index:999999}.centered{position:fixed;top:50%;left:50%;transform:translate(-50%,-50%)}"]
            }] }
];
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
class LoaderBarComponent {
    /**
     * @param {?} actions
     * @param {?} router
     */
    constructor(actions, router) {
        this.actions = actions;
        this.router = router;
        this.containerClass = 'abp-loader-bar';
        this.progressClass = 'abp-progress';
        this.isLoading = false;
        this.filter = (/**
         * @param {?} action
         * @return {?}
         */
        (action) => action.payload.url.indexOf('openid-configuration') < 0);
        this.progressLevel = 0;
        actions
            .pipe(ofActionSuccessful(StartLoader, StopLoader), filter(this.filter), takeUntilDestroy(this))
            .subscribe((/**
         * @param {?} action
         * @return {?}
         */
        action => {
            if (action instanceof StartLoader)
                this.startLoading();
            else
                this.stopLoading();
        }));
        router.events
            .pipe(filter((/**
         * @param {?} event
         * @return {?}
         */
        event => event instanceof NavigationStart || event instanceof NavigationEnd)), takeUntilDestroy(this))
            .subscribe((/**
         * @param {?} event
         * @return {?}
         */
        event => {
            if (event instanceof NavigationStart)
                this.startLoading();
            else
                this.stopLoading();
        }));
    }
    /**
     * @return {?}
     */
    ngOnDestroy() { }
    /**
     * @return {?}
     */
    startLoading() {
        this.isLoading = true;
        /** @type {?} */
        const interval = setInterval((/**
         * @return {?}
         */
        () => {
            if (this.progressLevel < 75) {
                this.progressLevel += Math.random() * 10;
            }
            else if (this.progressLevel < 90) {
                this.progressLevel += 0.4;
            }
            else if (this.progressLevel < 100) {
                this.progressLevel += 0.1;
            }
            else {
                clearInterval(interval);
            }
        }), 300);
        this.interval = interval;
    }
    /**
     * @return {?}
     */
    stopLoading() {
        clearInterval(this.interval);
        this.progressLevel = 100;
        this.isLoading = false;
        setTimeout((/**
         * @return {?}
         */
        () => {
            this.progressLevel = 0;
        }), 800);
    }
}
LoaderBarComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-loader-bar',
                template: `
    <div id="abp-loader-bar" [ngClass]="containerClass" [class.is-loading]="isLoading">
      <div [ngClass]="progressClass" [style.width.vw]="progressLevel"></div>
    </div>
  `,
                styles: [".abp-loader-bar{left:0;opacity:0;position:fixed;top:0;transition:opacity .4s linear .4s;z-index:99999}.abp-loader-bar.is-loading{opacity:1;transition:none}.abp-loader-bar .abp-progress{background:#77b6ff;box-shadow:0 0 10px rgba(119,182,255,.7);height:2px;left:0;position:fixed;top:0;transition:width .4s}"]
            }] }
];
/** @nocollapse */
LoaderBarComponent.ctorParameters = () => [
    { type: Actions },
    { type: Router }
];
LoaderBarComponent.propDecorators = {
    containerClass: [{ type: Input }],
    progressClass: [{ type: Input }],
    isLoading: [{ type: Input }],
    filter: [{ type: Input }]
};
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
class ButtonComponent {
    constructor() {
        this.buttonClass = 'btn btn-primary';
        this.type = 'button';
        this.loading = false;
        this.disabled = false;
    }
    /**
     * @return {?}
     */
    get icon() {
        return `${this.loading ? 'fa fa-spin fa-spinner' : this.iconClass || 'd-none'}`;
    }
}
ButtonComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-button',
                template: `
    <button [attr.type]="type" [ngClass]="buttonClass" [disabled]="loading || disabled">
      <i [ngClass]="icon" class="mr-1"></i><ng-content></ng-content>
    </button>
  `
            }] }
];
ButtonComponent.propDecorators = {
    buttonClass: [{ type: Input }],
    type: [{ type: Input }],
    iconClass: [{ type: Input }],
    loading: [{ type: Input }],
    disabled: [{ type: Input }]
};
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
const ANIMATION_TIMEOUT = 200;
class ModalComponent {
    /**
     * @param {?} renderer
     * @param {?} confirmationService
     */
    constructor(renderer, confirmationService) {
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
    /**
     * @return {?}
     */
    get visible() {
        return this._visible;
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set visible(value) {
        if (typeof value !== 'boolean')
            return;
        if (!this.modalContent) {
            if (value) {
                setTimeout((/**
                 * @return {?}
                 */
                () => {
                    this.showModal = value;
                    this.visible = value;
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
            () => {
                this.setVisible(value);
                this.ngOnDestroy();
            }), ANIMATION_TIMEOUT - 10);
        }
    }
    /**
     * @return {?}
     */
    get busy() {
        return this._busy;
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set busy(value) {
        if (this.abpSubmit && this.abpSubmit instanceof ButtonComponent) {
            this.abpSubmit.loading = value;
        }
        this._busy = value;
    }
    /**
     * @return {?}
     */
    ngOnDestroy() {
        this.destroy$.next();
    }
    /**
     * @param {?} value
     * @return {?}
     */
    setVisible(value) {
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
            _ => (this.closable = true)))
            : (this.closable = false);
    }
    /**
     * @return {?}
     */
    listen() {
        fromEvent(document, 'keyup')
            .pipe(takeUntil(this.destroy$), debounceTime(150), filter((/**
         * @param {?} key
         * @return {?}
         */
        (key) => key && key.code === 'Escape' && this.closable)))
            .subscribe((/**
         * @param {?} _
         * @return {?}
         */
        _ => {
            this.close();
        }));
        setTimeout((/**
         * @return {?}
         */
        () => {
            if (!this.abpClose)
                return;
            fromEvent(this.abpClose.nativeElement, 'click')
                .pipe(takeUntil(this.destroy$), filter((/**
             * @return {?}
             */
            () => !!(this.closable && this.modalContent))))
                .subscribe((/**
             * @return {?}
             */
            () => this.close()));
        }), 0);
        this.init.emit();
    }
    /**
     * @return {?}
     */
    close() {
        if (!this.closable || this.busy)
            return;
        /** @type {?} */
        const nodes = getFlatNodes(((/** @type {?} */ (this.modalContent.nativeElement.querySelector('#abp-modal-body')))).childNodes);
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
            (status) => {
                timer(ANIMATION_TIMEOUT).subscribe((/**
                 * @return {?}
                 */
                () => {
                    this.isOpenConfirmation = false;
                }));
                if (status === "confirm" /* confirm */) {
                    this.visible = false;
                }
            }));
        }
        else {
            this.visible = false;
        }
    }
}
ModalComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-modal',
                template: "<div\n  *ngIf=\"showModal\"\n  (click)=\"close()\"\n  id=\"abp-modal\"\n  class=\"modal fade {{ modalClass }} d-block show\"\n  [style.padding-right.px]=\"'15'\"\n>\n  <div\n    id=\"abp-modal-container\"\n    class=\"modal-dialog modal-{{ size }} fade-in-top\"\n    tabindex=\"-1\"\n    [class.modal-dialog-centered]=\"centered\"\n    #abpModalContent\n  >\n    <div #content id=\"abp-modal-content\" class=\"modal-content\" (click)=\"$event.stopPropagation()\">\n      <div id=\"abp-modal-header\" class=\"modal-header\">\n        <ng-container *ngTemplateOutlet=\"abpHeader\"></ng-container>\n\n        <button id=\"abp-modal-close-button\" type=\"button\" class=\"close\" (click)=\"close()\">\n          <span aria-hidden=\"true\">&times;</span>\n        </button>\n      </div>\n      <div\n        id=\"abp-modal-body\"\n        class=\"modal-body\"\n        [style.height]=\"height || undefined\"\n        [style.minHeight]=\"minHeight || undefined\"\n      >\n        <ng-container *ngTemplateOutlet=\"abpBody\"></ng-container>\n\n        <div id=\"abp-modal-footer\" class=\"modal-footer\">\n          <ng-container *ngTemplateOutlet=\"abpFooter\"></ng-container>\n        </div>\n      </div>\n    </div>\n  </div>\n\n  <ng-content></ng-content>\n</div>\n"
            }] }
];
/** @nocollapse */
ModalComponent.ctorParameters = () => [
    { type: Renderer2 },
    { type: ConfirmationService }
];
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
    (acc, val) => [...acc, ...(val.childNodes && val.childNodes.length ? getFlatNodes(val.childNodes) : [val])]), []);
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
    node => (node.className || '').indexOf('ng-dirty') > -1)) > -1;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class ToastComponent {
}
ToastComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-toast',
                template: `
    <p-toast position="bottom-right" key="abpToast" [baseZIndex]="1000">
      <ng-template let-message pTemplate="message">
        <span
          class="ui-toast-icon pi"
          [ngClass]="{
            'pi-info-circle': message.severity == 'info',
            'pi-exclamation-triangle': message.severity == 'warn',
            'pi-times': message.severity == 'error',
            'pi-check': message.severity == 'success'
          }"
        ></span>
        <div class="ui-toast-message-text-content">
          <div class="ui-toast-summary">{{ message.summary | abpLocalization: message.titleLocalizationParams }}</div>
          <div class="ui-toast-detail">{{ message.detail | abpLocalization: message.messageLocalizationParams }}</div>
        </div>
      </ng-template>
    </p-toast>
  `
            }] }
];

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var styles = `
.is-invalid .form-control {
  border-color: #dc3545;
  border-style: solid !important;
}

.is-invalid .invalid-feedback,
.is-invalid + * .invalid-feedback {
  display: block;
}

.data-tables-filter {
  text-align: right;
}

.pointer {
  cursor: pointer;
}

.navbar .dropdown-submenu a::after {
  transform: rotate(-90deg);
  position: absolute;
  right: 16px;
  top: 18px;
}

.navbar .dropdown-menu {
  min-width: 215px;
}

.modal {
 background-color: rgba(0, 0, 0, .6);
}

.abp-ellipsis {
  display: inline-block;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

/* <animations */

.fade-in-top {
  animation: fadeInTop 0.2s ease-in-out;
}

.fade-out-top {
  animation: fadeOutTop 0.2s ease-in-out;
}


@keyframes fadeInTop {
  from {
    transform: translateY(-5px);
    opacity: 0;
  }

  to {
    transform: translateY(5px);
    opacity: 1;
  }
}

@keyframes fadeOutTop {
  to {
    transform: translateY(-5px);
    opacity: 0;
  }
}

/* </animations */

`;

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
const DEFAULTS = {
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
class ErrorHandler {
    /**
     * @param {?} actions
     * @param {?} store
     * @param {?} confirmationService
     * @param {?} appRef
     * @param {?} cfRes
     * @param {?} rendererFactory
     * @param {?} injector
     */
    constructor(actions, store, confirmationService, appRef, cfRes, rendererFactory, injector) {
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
        res => {
            const { payload: err = (/** @type {?} */ ({})) } = res;
            /** @type {?} */
            const body = snq((/**
             * @return {?}
             */
            () => ((/** @type {?} */ (err))).error.error), DEFAULTS.defaultError.message);
            if (err.headers.get('_AbpErrorFormat')) {
                /** @type {?} */
                const confirmation$ = this.showError(null, null, body);
                if (err.status === 401) {
                    confirmation$.subscribe((/**
                     * @return {?}
                     */
                    () => {
                        this.navigateToLogin();
                    }));
                }
            }
            else {
                switch (((/** @type {?} */ (err))).status) {
                    case 401:
                        this.showError(DEFAULTS.defaultError401.details, DEFAULTS.defaultError401.message).subscribe((/**
                         * @return {?}
                         */
                        () => this.navigateToLogin()));
                        break;
                    case 403:
                        this.createErrorComponent({
                            title: DEFAULTS.defaultError403.message,
                            details: DEFAULTS.defaultError403.details,
                        });
                        break;
                    case 404:
                        this.showError(DEFAULTS.defaultError404.details, DEFAULTS.defaultError404.message);
                        break;
                    case 500:
                        this.createErrorComponent({
                            title: '500',
                            details: 'AbpAccount::InternalServerErrorMessage',
                        });
                        break;
                    case 0:
                        if (((/** @type {?} */ (err))).statusText === 'Unknown Error') {
                            this.createErrorComponent({
                                title: 'Unknown Error',
                                details: 'AbpAccount::InternalServerErrorMessage',
                            });
                        }
                        break;
                    default:
                        this.showError(DEFAULTS.defaultError.details, DEFAULTS.defaultError.message);
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
    showError(message, title, body) {
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
    }
    /**
     * @private
     * @return {?}
     */
    navigateToLogin() {
        this.store.dispatch(new Navigate(['/account/login'], null, {
            state: { redirectUrl: this.store.selectSnapshot(RouterState).state.url },
        }));
    }
    /**
     * @param {?} instance
     * @return {?}
     */
    createErrorComponent(instance) {
        /** @type {?} */
        const renderer = this.rendererFactory.createRenderer(null, null);
        /** @type {?} */
        const host = renderer.selectRootElement('app-root', true);
        /** @type {?} */
        const componentRef = this.cfRes.resolveComponentFactory(ErrorComponent).create(this.injector);
        for (const key in componentRef.instance) {
            if (componentRef.instance.hasOwnProperty(key)) {
                componentRef.instance[key] = instance[key];
            }
        }
        this.appRef.attachView(componentRef.hostView);
        renderer.appendChild(host, ((/** @type {?} */ (componentRef.hostView))).rootNodes[0]);
        componentRef.instance.renderer = renderer;
        componentRef.instance.elementRef = componentRef.location;
        componentRef.instance.host = host;
    }
}
ErrorHandler.decorators = [
    { type: Injectable, args: [{ providedIn: 'root' },] }
];
/** @nocollapse */
ErrorHandler.ctorParameters = () => [
    { type: Actions },
    { type: Store },
    { type: ConfirmationService },
    { type: ApplicationRef },
    { type: ComponentFactoryResolver },
    { type: RendererFactory2 },
    { type: Injector }
];
/** @nocollapse */ ErrorHandler.ngInjectableDef = ɵɵdefineInjectable({ factory: function ErrorHandler_Factory() { return new ErrorHandler(ɵɵinject(Actions), ɵɵinject(Store), ɵɵinject(ConfirmationService), ɵɵinject(ApplicationRef), ɵɵinject(ComponentFactoryResolver), ɵɵinject(RendererFactory2), ɵɵinject(INJECTOR)); }, token: ErrorHandler, providedIn: "root" });
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
class ValidationErrorComponent extends ValidationErrorComponent$1 {
    /**
     * @return {?}
     */
    get abpErrors() {
        if (!this.errors || !this.errors.length)
            return [];
        return this.errors.map((/**
         * @param {?} error
         * @return {?}
         */
        error => {
            if (!error.message)
                return error;
            /** @type {?} */
            const index = error.message.indexOf('[');
            if (index > -1) {
                return Object.assign({}, error, { message: error.message.slice(0, index), interpoliteParams: error.message.slice(index + 1, error.message.length - 1).split(',') });
            }
            return error;
        }));
    }
}
ValidationErrorComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-validation-error',
                template: `
    <div class="invalid-feedback" *ngFor="let error of abpErrors; trackBy: trackByFn">
      {{ error.message | abpLocalization: error.interpoliteParams }}
    </div>
  `,
                changeDetection: ChangeDetectionStrategy.OnPush,
                encapsulation: ViewEncapsulation.None
            }] }
];

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
    const fn = (/**
     * @return {?}
     */
    function () {
        /** @type {?} */
        const lazyLoadService = injector.get(LazyLoadService);
        return forkJoin(lazyLoadService.load(null, 'style', styles, 'head', 'afterbegin') /* lazyLoadService.load(null, 'script', scripts) */).pipe(take(1));
    });
    return fn;
}
class ThemeSharedModule {
    /**
     * @return {?}
     */
    static forRoot() {
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
    }
}
ThemeSharedModule.decorators = [
    { type: NgModule, args: [{
                imports: [
                    CoreModule,
                    ToastModule,
                    NgbModalModule,
                    NgxValidateCoreModule.forRoot({
                        targetSelector: '.form-group',
                        blueprints: {
                            email: `AbpAccount::ThisFieldIsNotAValidEmailAddress.`,
                            max: `AbpAccount::ThisFieldMustBeAStringWithAMaximumLengthOf{1}[{{ max }}]`,
                            maxlength: `AbpAccount::ThisFieldMustBeAStringWithAMaximumLengthOf{1}[{{ requiredLength }}]`,
                            min: `AbpAccount::ThisFieldMustBeAStringWithAMinimumLengthOf{1}AndAMaximumLengthOf{0}[{{ min }},{{ max }}]`,
                            minlength: `AbpAccount::ThisFieldMustBeAStringWithAMinimumLengthOf{1}AndAMaximumLengthOf{0}[{{ min }},{{ max }}]`,
                            required: `AbpAccount::ThisFieldIsRequired.`,
                            passwordMismatch: `AbpIdentity::Identity.PasswordConfirmationFailed`,
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

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
const slideFromBottom = trigger('routeAnimations', [
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
class ToasterService extends AbstractToaster {
    /**
     * @param {?} messages
     * @return {?}
     */
    addAll(messages) {
        this.messageService.addAll(messages.map((/**
         * @param {?} message
         * @return {?}
         */
        message => (Object.assign({ key: this.key }, message)))));
    }
}
ToasterService.decorators = [
    { type: Injectable, args: [{ providedIn: 'root' },] }
];
/** @nocollapse */ ToasterService.ngInjectableDef = ɵɵdefineInjectable({ factory: function ToasterService_Factory() { return new ToasterService(ɵɵinject(MessageService)); }, token: ToasterService, providedIn: "root" });

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
