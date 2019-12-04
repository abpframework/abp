/**
 * @fileoverview added by tsickle
 * Generated from: lib/handlers/error.handler.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { RestOccurError } from '@abp/ng.core';
import { HttpErrorResponse } from '@angular/common/http';
import { ApplicationRef, ComponentFactoryResolver, Inject, Injectable, Injector, RendererFactory2, } from '@angular/core';
import { Navigate, RouterError, RouterState, RouterDataResolved } from '@ngxs/router-plugin';
import { Actions, ofActionSuccessful, Store } from '@ngxs/store';
import { Subject } from 'rxjs';
import snq from 'snq';
import { HttpErrorWrapperComponent } from '../components/http-error-wrapper/http-error-wrapper.component';
import { ConfirmationService } from '../services/confirmation.service';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
import * as i2 from "../services/confirmation.service";
/** @type {?} */
export const DEFAULT_ERROR_MESSAGES = {
    defaultError: {
        title: 'An error has occurred!',
        details: 'Error detail not sent by server.',
    },
    defaultError401: {
        title: 'You are not authenticated!',
        details: 'You should be authenticated (sign in) in order to perform this operation.',
    },
    defaultError403: {
        title: 'You are not authorized!',
        details: 'You are not allowed to perform this operation.',
    },
    defaultError404: {
        title: 'Resource not found!',
        details: 'The resource requested could not found on the server.',
    },
    defaultError500: {
        title: 'Internal server error',
        details: 'Error detail not sent by server.',
    },
};
export class ErrorHandler {
    /**
     * @param {?} actions
     * @param {?} store
     * @param {?} confirmationService
     * @param {?} appRef
     * @param {?} cfRes
     * @param {?} rendererFactory
     * @param {?} injector
     * @param {?} httpErrorConfig
     */
    constructor(actions, store, confirmationService, appRef, cfRes, rendererFactory, injector, httpErrorConfig) {
        this.actions = actions;
        this.store = store;
        this.confirmationService = confirmationService;
        this.appRef = appRef;
        this.cfRes = cfRes;
        this.rendererFactory = rendererFactory;
        this.injector = injector;
        this.httpErrorConfig = httpErrorConfig;
        this.actions.pipe(ofActionSuccessful(RestOccurError, RouterError, RouterDataResolved)).subscribe((/**
         * @param {?} res
         * @return {?}
         */
        res => {
            if (res instanceof RestOccurError) {
                const { payload: err = (/** @type {?} */ ({})) } = res;
                /** @type {?} */
                const body = snq((/**
                 * @return {?}
                 */
                () => ((/** @type {?} */ (err))).error.error), DEFAULT_ERROR_MESSAGES.defaultError.title);
                if (err instanceof HttpErrorResponse && err.headers.get('_AbpErrorFormat')) {
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
                            this.canCreateCustomError(401)
                                ? this.show401Page()
                                : this.showError({
                                    key: 'AbpAccount::DefaultErrorMessage401',
                                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.title,
                                }, {
                                    key: 'AbpAccount::DefaultErrorMessage401Detail',
                                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.details,
                                }).subscribe((/**
                                 * @return {?}
                                 */
                                () => this.navigateToLogin()));
                            break;
                        case 403:
                            this.createErrorComponent({
                                title: {
                                    key: 'AbpAccount::DefaultErrorMessage403',
                                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError403.title,
                                },
                                details: {
                                    key: 'AbpAccount::DefaultErrorMessage403Detail',
                                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError403.details,
                                },
                                status: 403,
                            });
                            break;
                        case 404:
                            this.canCreateCustomError(404)
                                ? this.show404Page()
                                : this.showError({
                                    key: 'AbpAccount::DefaultErrorMessage404',
                                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.details,
                                }, {
                                    key: 'AbpAccount::DefaultErrorMessage404Detail',
                                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.title,
                                });
                            break;
                        case 500:
                            this.createErrorComponent({
                                title: {
                                    key: 'AbpAccount::500Message',
                                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError500.title,
                                },
                                details: {
                                    key: 'AbpAccount::InternalServerErrorMessage',
                                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError500.details,
                                },
                                status: 500,
                            });
                            break;
                        case 0:
                            if (((/** @type {?} */ (err))).statusText === 'Unknown Error') {
                                this.createErrorComponent({
                                    title: {
                                        key: 'AbpAccount::DefaultErrorMessage',
                                        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
                                    },
                                });
                            }
                            break;
                        default:
                            this.showError(DEFAULT_ERROR_MESSAGES.defaultError.details, DEFAULT_ERROR_MESSAGES.defaultError.title);
                            break;
                    }
                }
            }
            else if (res instanceof RouterError && snq((/**
             * @return {?}
             */
            () => res.event.error.indexOf('Cannot match') > -1), false)) {
                this.show404Page();
            }
            else if (res instanceof RouterDataResolved && this.componentRef) {
                this.componentRef.destroy();
                this.componentRef = null;
            }
        }));
    }
    /**
     * @private
     * @return {?}
     */
    show401Page() {
        this.createErrorComponent({
            title: {
                key: 'AbpAccount::401Message',
                defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.title,
            },
            status: 401,
        });
    }
    /**
     * @private
     * @return {?}
     */
    show404Page() {
        this.createErrorComponent({
            title: {
                key: 'AbpAccount::404Message',
                defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.title,
            },
            status: 404,
        });
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
                message = body.message || DEFAULT_ERROR_MESSAGES.defaultError.title;
            }
        }
        return this.confirmationService.error(message, title, {
            hideCancelBtn: true,
            yesText: 'AbpAccount::Close',
        });
    }
    /**
     * @private
     * @return {?}
     */
    navigateToLogin() {
        this.store.dispatch(new Navigate(['/account/login'], null, { state: { redirectUrl: this.store.selectSnapshot(RouterState.url) } }));
    }
    /**
     * @param {?} instance
     * @return {?}
     */
    createErrorComponent(instance) {
        /** @type {?} */
        const renderer = this.rendererFactory.createRenderer(null, null);
        /** @type {?} */
        const host = renderer.selectRootElement(document.body, true);
        this.componentRef = this.cfRes.resolveComponentFactory(HttpErrorWrapperComponent).create(this.injector);
        for (const key in this.componentRef.instance) {
            if (this.componentRef.instance.hasOwnProperty(key)) {
                this.componentRef.instance[key] = instance[key];
            }
        }
        this.componentRef.instance.hideCloseIcon = this.httpErrorConfig.errorScreen.hideCloseIcon;
        if (this.canCreateCustomError((/** @type {?} */ (instance.status)))) {
            this.componentRef.instance.cfRes = this.cfRes;
            this.componentRef.instance.appRef = this.appRef;
            this.componentRef.instance.injector = this.injector;
            this.componentRef.instance.customComponent = this.httpErrorConfig.errorScreen.component;
        }
        this.appRef.attachView(this.componentRef.hostView);
        renderer.appendChild(host, ((/** @type {?} */ (this.componentRef.hostView))).rootNodes[0]);
        /** @type {?} */
        const destroy$ = new Subject();
        this.componentRef.instance.destroy$ = destroy$;
        destroy$.subscribe((/**
         * @return {?}
         */
        () => {
            this.componentRef.destroy();
            this.componentRef = null;
        }));
    }
    /**
     * @param {?} status
     * @return {?}
     */
    canCreateCustomError(status) {
        return snq((/**
         * @return {?}
         */
        () => this.httpErrorConfig.errorScreen.component &&
            this.httpErrorConfig.errorScreen.forWhichErrors.indexOf(status) > -1));
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
    { type: Injector },
    { type: undefined, decorators: [{ type: Inject, args: ['HTTP_ERROR_CONFIG',] }] }
];
/** @nocollapse */ ErrorHandler.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ErrorHandler_Factory() { return new ErrorHandler(i0.ɵɵinject(i1.Actions), i0.ɵɵinject(i1.Store), i0.ɵɵinject(i2.ConfirmationService), i0.ɵɵinject(i0.ApplicationRef), i0.ɵɵinject(i0.ComponentFactoryResolver), i0.ɵɵinject(i0.RendererFactory2), i0.ɵɵinject(i0.INJECTOR), i0.ɵɵinject("HTTP_ERROR_CONFIG")); }, token: ErrorHandler, providedIn: "root" });
if (false) {
    /** @type {?} */
    ErrorHandler.prototype.componentRef;
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
    /**
     * @type {?}
     * @private
     */
    ErrorHandler.prototype.httpErrorConfig;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZXJyb3IuaGFuZGxlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2hhbmRsZXJzL2Vycm9yLmhhbmRsZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQVUsY0FBYyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3RELE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFDTCxjQUFjLEVBQ2Qsd0JBQXdCLEVBRXhCLE1BQU0sRUFDTixVQUFVLEVBQ1YsUUFBUSxFQUNSLGdCQUFnQixHQUdqQixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsUUFBUSxFQUFFLFdBQVcsRUFBRSxXQUFXLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUM3RixPQUFPLEVBQUUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNqRSxPQUFPLEVBQWMsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzNDLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUN0QixPQUFPLEVBQUUseUJBQXlCLEVBQUUsTUFBTSwrREFBK0QsQ0FBQztBQUcxRyxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSxrQ0FBa0MsQ0FBQzs7Ozs7QUFFdkUsTUFBTSxPQUFPLHNCQUFzQixHQUFHO0lBQ3BDLFlBQVksRUFBRTtRQUNaLEtBQUssRUFBRSx3QkFBd0I7UUFDL0IsT0FBTyxFQUFFLGtDQUFrQztLQUM1QztJQUNELGVBQWUsRUFBRTtRQUNmLEtBQUssRUFBRSw0QkFBNEI7UUFDbkMsT0FBTyxFQUFFLDJFQUEyRTtLQUNyRjtJQUNELGVBQWUsRUFBRTtRQUNmLEtBQUssRUFBRSx5QkFBeUI7UUFDaEMsT0FBTyxFQUFFLGdEQUFnRDtLQUMxRDtJQUNELGVBQWUsRUFBRTtRQUNmLEtBQUssRUFBRSxxQkFB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Q0FBQyxNQUE2QjtRQUNoRCxPQUFPLEdBQUc7OztRQUNSLEdBQUcsRUFBRSxDQUNILElBQUksQ0FBQyxlQUFlLENBQUMsV0FBVyxDQUFDLFNBQVM7WUFDMUMsSUFBSSxDQUFDLGVBQWUsQ0FBQyxXQUFXLENBQUMsY0FBYyxDQUFDLE9BQU8sQ0FBQyxNQUFNLENBQUMsR0FBRyxDQUFDLENBQUMsRUFDdkUsQ0FBQztJQUNKLENBQUM7OztZQTdMRixVQUFVLFNBQUMsRUFBRSxVQUFVLEVBQUUsTUFBTSxFQUFFOzs7O1lBL0J6QixPQUFPO1lBQXNCLEtBQUs7WUFNbEMsbUJBQW1CO1lBakIxQixjQUFjO1lBQ2Qsd0JBQXdCO1lBS3hCLGdCQUFnQjtZQURoQixRQUFROzRDQWlETCxNQUFNLFNBQUMsbUJBQW1COzs7OztJQVY3QixvQ0FBc0Q7Ozs7O0lBR3BELCtCQUF3Qjs7Ozs7SUFDeEIsNkJBQW9COzs7OztJQUNwQiwyQ0FBZ0Q7Ozs7O0lBQ2hELDhCQUE4Qjs7Ozs7SUFDOUIsNkJBQXVDOzs7OztJQUN2Qyx1Q0FBeUM7Ozs7O0lBQ3pDLGdDQUEwQjs7Ozs7SUFDMUIsdUNBQXFFIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29uZmlnLCBSZXN0T2NjdXJFcnJvciB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBIdHRwRXJyb3JSZXNwb25zZSB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbi9odHRwJztcbmltcG9ydCB7XG4gIEFwcGxpY2F0aW9uUmVmLFxuICBDb21wb25lbnRGYWN0b3J5UmVzb2x2ZXIsXG4gIEVtYmVkZGVkVmlld1JlZixcbiAgSW5qZWN0LFxuICBJbmplY3RhYmxlLFxuICBJbmplY3RvcixcbiAgUmVuZGVyZXJGYWN0b3J5MixcbiAgVHlwZSxcbiAgQ29tcG9uZW50UmVmLFxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5hdmlnYXRlLCBSb3V0ZXJFcnJvciwgUm91dGVyU3RhdGUsIFJvdXRlckRhdGFSZXNvbHZlZCB9IGZyb20gJ0BuZ3hzL3JvdXRlci1wbHVnaW4nO1xuaW1wb3J0IHsgQWN0aW9ucywgb2ZBY3Rpb25TdWNjZXNzZnVsLCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUsIFN1YmplY3QgfSBmcm9tICdyeGpzJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcbmltcG9ydCB7IEh0dHBFcnJvcldyYXBwZXJDb21wb25lbnQgfSBmcm9tICcuLi9jb21wb25lbnRzL2h0dHAtZXJyb3Itd3JhcHBlci9odHRwLWVycm9yLXdyYXBwZXIuY29tcG9uZW50JztcbmltcG9ydCB7IEh0dHBFcnJvckNvbmZpZywgRXJyb3JTY3JlZW5FcnJvckNvZGVzIH0gZnJvbSAnLi4vbW9kZWxzL2NvbW1vbic7XG5pbXBvcnQgeyBUb2FzdGVyIH0gZnJvbSAnLi4vbW9kZWxzL3RvYXN0ZXInO1xuaW1wb3J0IHsgQ29uZmlybWF0aW9uU2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL2NvbmZpcm1hdGlvbi5zZXJ2aWNlJztcblxuZXhwb3J0IGNvbnN0IERFRkFVTFRfRVJST1JfTUVTU0FHRVMgPSB7XG4gIGRlZmF1bHRFcnJvcjoge1xuICAgIHRpdGxlOiAnQW4gZXJyb3IgaGFzIG9jY3VycmVkIScsXG4gICAgZGV0YWlsczogJ0Vycm9yIGRldGFpbCBub3Qgc2VudCBieSBzZXJ2ZXIuJyxcbiAgfSxcbiAgZGVmYXVsdEVycm9yNDAxOiB7XG4gICAgdGl0bGU6ICdZb3UgYXJlIG5vdCBhdXRoZW50aWNhdGVkIScsXG4gICAgZGV0YWlsczogJ1lvdSBzaG91bGQgYmUgYXV0aGVudGljYXRlZCAoc2lnbiBpbikgaW4gb3JkZXIgdG8gcGVyZm9ybSB0aGlzIG9wZXJhdGlvbi4nLFxuICB9LFxuICBkZWZhdWx0RXJyb3I0MDM6IHtcbiAgICB0aXRsZTogJ1lvdSBhcmUgbm90IGF1dGhvcml6ZWQhJyxcbiAgICBkZXRhaWxzOiAnWW91IGFyZSBub3QgYWxsb3dlZCB0byBwZXJmb3JtIHRoaXMgb3BlcmF0aW9uLicsXG4gIH0sXG4gIGRlZmF1bHRFcnJvcjQwNDoge1xuICAgIHRpdGxlOiAnUmVzb3VyY2Ugbm90IGZvdW5kIScsXG4gICAgZGV0YWlsczogJ1RoZSByZXNvdXJjZSByZXF1ZXN0ZWQgY291bGQgbm90IGZvdW5kIG9uIHRoZSBzZXJ2ZXIuJyxcbiAgfSxcbiAgZGVmYXVsdEVycm9yNTAwOiB7XG4gICAgdGl0bGU6ICdJbnRlcm5hbCBzZXJ2ZXIgZXJyb3InLFxuICAgIGRldGFpbHM6ICdFcnJvciBkZXRhaWwgbm90IHNlbnQgYnkgc2VydmVyLicsXG4gIH0sXG59O1xuXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxuZXhwb3J0IGNsYXNzIEVycm9ySGFuZGxlciB7XG4gIGNvbXBvbmVudFJlZjogQ29tcG9uZW50UmVmPEh0dHBFcnJvcldyYXBwZXJDb21wb25lbnQ+O1xuXG4gIGNvbnN0cnVjdG9yKFxuICAgIHByaXZhdGUgYWN0aW9uczogQWN0aW9ucyxcbiAgICBwcml2YXRlIHN0b3JlOiBTdG9yZSxcbiAgICBwcml2YXRlIGNvbmZpcm1hdGlvblNlcnZpY2U6IENvbmZpcm1hdGlvblNlcnZpY2UsXG4gICAgcHJpdmF0ZSBhcHBSZWY6IEFwcGxpY2F0aW9uUmVmLFxuICAgIHByaXZhdGUgY2ZSZXM6IENvbXBvbmVudEZhY3RvcnlSZXNvbHZlcixcbiAgICBwcml2YXRlIHJlbmRlcmVyRmFjdG9yeTogUmVuZGVyZXJGYWN0b3J5MixcbiAgICBwcml2YXRlIGluamVjdG9yOiBJbmplY3RvcixcbiAgICBASW5qZWN0KCdIVFRQX0VSUk9SX0NPTkZJRycpIHByaXZhdGUgaHR0cEVycm9yQ29uZmlnOiBIdHRwRXJyb3JDb25maWcsXG4gICkge1xuICAgIHRoaXMuYWN0aW9ucy5waXBlKG9mQWN0aW9uU3VjY2Vzc2Z1bChSZXN0T2NjdXJFcnJvciwgUm91dGVyRXJyb3IsIFJvdXRlckRhdGFSZXNvbHZlZCkpLnN1YnNjcmliZShyZXMgPT4ge1xuICAgICAgaWYgKHJlcyBpbnN0YW5jZW9mIFJlc3RPY2N1ckVycm9yKSB7XG4gICAgICAgIGNvbnN0IHsgcGF5bG9hZDogZXJyID0ge30gYXMgSHR0cEVycm9yUmVzcG9uc2UgfCBhbnkgfSA9IHJlcztcbiAgICAgICAgY29uc3QgYm9keSA9IHNucSgoKSA9PiAoZXJyIGFzIEh0dHBFcnJvclJlc3BvbnNlKS5lcnJvci5lcnJvciwgREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3IudGl0bGUpO1xuXG4gICAgICAgIGlmIChlcnIgaW5zdGFuY2VvZiBIdHRwRXJyb3JSZXNwb25zZSAmJiBlcnIuaGVhZGVycy5nZXQoJ19BYnBFcnJvckZvcm1hdCcpKSB7XG4gICAgICAgICAgY29uc3QgY29uZmlybWF0aW9uJCA9IHRoaXMuc2hvd0Vycm9yKG51bGwsIG51bGwsIGJvZHkpO1xuXG4gICAgICAgICAgaWYgKGVyci5zdGF0dXMgPT09IDQwMSkge1xuICAgICAgICAgICAgY29uZmlybWF0aW9uJC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICAgICAgICB0aGlzLm5hdmlnYXRlVG9Mb2dpbigpO1xuICAgICAgICAgICAgfSk7XG4gICAgICAgICAgfVxuICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgIHN3aXRjaCAoKGVyciBhcyBIdHRwRXJyb3JSZXNwb25zZSkuc3RhdHVzKSB7XG4gICAgICAgICAgICBjYXNlIDQwMTpcbiAgICAgICAgICAgICAgdGhpcy5jYW5DcmVhdGVDdXN0b21FcnJvcig0MDEpXG4gICAgICAgICAgICAgICAgPyB0aGlzLnNob3c0MDFQYWdlKClcbiAgICAgICAgICAgICAgICA6IHRoaXMuc2hvd0Vycm9yKFxuICAgICAgICAgICAgICAgICAgICB7XG4gICAgICAgICAgICAgICAgICAgICAga2V5OiAnQWJwQWNjb3VudDo6RGVmYXVsdEVycm9yTWVzc2FnZTQwMScsXG4gICAgICAgICAgICAgICAgICAgICAgZGVmYXVsdFZhbHVlOiBERUZBVUxUX0VSUk9SX01FU1NBR0VTLmRlZmF1bHRFcnJvcjQwMS50aXRsZSxcbiAgICAgICAgICAgICAgICAgICAgfSxcbiAgICAgICAgICAgICAgICAgICAge1xuICAgICAgICAgICAgICAgICAgICAgIGtleTogJ0FicEFjY291bnQ6OkRlZmF1bHRFcnJvck1lc3NhZ2U0MDFEZXRhaWwnLFxuICAgICAgICAgICAgICAgICAgICAgIGRlZmF1bHRWYWx1ZTogREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3I0MDEuZGV0YWlscyxcbiAgICAgICAgICAgICAgICAgICAgfSxcbiAgICAgICAgICAgICAgICAgICkuc3Vic2NyaWJlKCgpID0+IHRoaXMubmF2aWdhdGVUb0xvZ2luKCkpO1xuICAgICAgICAgICAgICBicmVhaztcbiAgICAgICAgICAgIGNhc2UgNDAzOlxuICAgICAgICAgICAgICB0aGlzLmNyZWF0ZUVycm9yQ29tcG9uZW50KHtcbiAgICAgICAgICAgICAgICB0aXRsZToge1xuICAgICAgICAgICAgICAgICAga2V5OiAnQWJwQWNjb3VudDo6RGVmYXVsdEVycm9yTWVzc2FnZTQwMycsXG4gICAgICAgICAgICAgICAgICBkZWZhdWx0VmFsdWU6IERFRkFVTFRfRVJST1JfTUVTU0FHRVMuZGVmYXVsdEVycm9yNDAzLnRpdGxlLFxuICAgICAgICAgICAgICAgIH0sXG4gICAgICAgICAgICAgICAgZGV0YWlsczoge1xuICAgICAgICAgICAgICAgICAga2V5OiAnQWJwQWNjb3VudDo6RGVmYXVsdEVycm9yTWVzc2FnZTQwM0RldGFpbCcsXG4gICAgICAgICAgICAgICAgICBkZWZhdWx0VmFsdWU6IERFRkFVTFRfRVJST1JfTUVTU0FHRVMuZGVmYXVsdEVycm9yNDAzLmRldGFpbHMsXG4gICAgICAgICAgICAgICAgfSxcbiAgICAgICAgICAgICAgICBzdGF0dXM6IDQwMyxcbiAgICAgICAgICAgICAgfSk7XG4gICAgICAgICAgICAgIGJyZWFrO1xuICAgICAgICAgICAgY2FzZSA0MDQ6XG4gICAgICAgICAgICAgIHRoaXMuY2FuQ3JlYXRlQ3VzdG9tRXJyb3IoNDA0KVxuICAgICAgICAgICAgICAgID8gdGhpcy5zaG93NDA0UGFnZSgpXG4gICAgICAgICAgICAgICAgOiB0aGlzLnNob3dFcnJvcihcbiAgICAgICAgICAgICAgICAgICAge1xuICAgICAgICAgICAgICAgICAgICAgIGtleTogJ0FicEFjY291bnQ6OkRlZmF1bHRFcnJvck1lc3NhZ2U0MDQnLFxuICAgICAgICAgICAgICAgICAgICAgIGRlZmF1bHRWYWx1ZTogREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3I0MDQuZGV0YWlscyxcbiAgICAgICAgICAgICAgICAgICAgfSxcbiAgICAgICAgICAgICAgICAgICAge1xuICAgICAgICAgICAgICAgICAgICAgIGtleTogJ0FicEFjY291bnQ6OkRlZmF1bHRFcnJvck1lc3NhZ2U0MDREZXRhaWwnLFxuICAgICAgICAgICAgICAgICAgICAgIGRlZmF1bHRWYWx1ZTogREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3I0MDQudGl0bGUsXG4gICAgICAgICAgICAgICAgICAgIH0sXG4gICAgICAgICAgICAgICAgICApO1xuICAgICAgICAgICAgICBicmVhaztcbiAgICAgICAgICAgIGNhc2UgNTAwOlxuICAgICAgICAgICAgICB0aGlzLmNyZWF0ZUVycm9yQ29tcG9uZW50KHtcbiAgICAgICAgICAgICAgICB0aXRsZToge1xuICAgICAgICAgICAgICAgICAga2V5OiAnQWJwQWNjb3VudDo6NTAwTWVzc2FnZScsXG4gICAgICAgICAgICAgICAgICBkZWZhdWx0VmFsdWU6IERFRkFVTFRfRVJST1JfTUVTU0FHRVMuZGVmYXVsdEVycm9yNTAwLnRpdGxlLFxuICAgICAgICAgICAgICAgIH0sXG4gICAgICAgICAgICAgICAgZGV0YWlsczoge1xuICAgICAgICAgICAgICAgICAga2V5OiAnQWJwQWNjb3VudDo6SW50ZXJuYWxTZXJ2ZXJFcnJvck1lc3NhZ2UnLFxuICAgICAgICAgICAgICAgICAgZGVmYXVsdFZhbHVlOiBERUZBVUxUX0VSUk9SX01FU1NBR0VTLmRlZmF1bHRFcnJvcjUwMC5kZXRhaWxzLFxuICAgICAgICAgICAgICAgIH0sXG4gICAgICAgICAgICAgICAgc3RhdHVzOiA1MDAsXG4gICAgICAgICAgICAgIH0pO1xuICAgICAgICAgICAgICBicmVhaztcbiAgICAgICAgICAgIGNhc2UgMDpcbiAgICAgICAgICAgICAgaWYgKChlcnIgYXMgSHR0cEVycm9yUmVzcG9uc2UpLnN0YXR1c1RleHQgPT09ICdVbmtub3duIEVycm9yJykge1xuICAgICAgICAgICAgICAgIHRoaXMuY3JlYXRlRXJyb3JDb21wb25lbnQoe1xuICAgICAgICAgICAgICAgICAgdGl0bGU6IHtcbiAgICAgICAgICAgICAgICAgICAga2V5OiAnQWJwQWNjb3VudDo6RGVmYXVsdEVycm9yTWVzc2FnZScsXG4gICAgICAgICAgICAgICAgICAgIGRlZmF1bHRWYWx1ZTogREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3IudGl0bGUsXG4gICAgICAgICAgICAgICAgICB9LFxuICAgICAgICAgICAgICAgIH0pO1xuICAgICAgICAgICAgICB9XG4gICAgICAgICAgICAgIGJyZWFrO1xuICAgICAgICAgICAgZGVmYXVsdDpcbiAgICAgICAgICAgICAgdGhpcy5zaG93RXJyb3IoREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3IuZGV0YWlscywgREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3IudGl0bGUpO1xuICAgICAgICAgICAgICBicmVhaztcbiAgICAgICAgICB9XG4gICAgICAgIH1cbiAgICAgIH0gZWxzZSBpZiAocmVzIGluc3RhbmNlb2YgUm91dGVyRXJyb3IgJiYgc25xKCgpID0+IHJlcy5ldmVudC5lcnJvci5pbmRleE9mKCdDYW5ub3QgbWF0Y2gnKSA+IC0xLCBmYWxzZSkpIHtcbiAgICAgICAgdGhpcy5zaG93NDA0UGFnZSgpO1xuICAgICAgfSBlbHNlIGlmIChyZXMgaW5zdGFuY2VvZiBSb3V0ZXJEYXRhUmVzb2x2ZWQgJiYgdGhpcy5jb21wb25lbnRSZWYpIHtcbiAgICAgICAgdGhpcy5jb21wb25lbnRSZWYuZGVzdHJveSgpO1xuICAgICAgICB0aGlzLmNvbXBvbmVudFJlZiA9IG51bGw7XG4gICAgICB9XG4gICAgfSk7XG4gIH1cblxuICBwcml2YXRlIHNob3c0MDFQYWdlKCkge1xuICAgIHRoaXMuY3JlYXRlRXJyb3JDb21wb25lbnQoe1xuICAgICAgdGl0bGU6IHtcbiAgICAgICAga2V5OiAnQWJwQWNjb3VudDo6NDAxTWVzc2FnZScsXG4gICAgICAgIGRlZmF1bHRWYWx1ZTogREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3I0MDEudGl0bGUsXG4gICAgICB9LFxuICAgICAgc3RhdHVzOiA0MDEsXG4gICAgfSk7XG4gIH1cblxuICBwcml2YXRlIHNob3c0MDRQYWdlKCkge1xuICAgIHRoaXMuY3JlYXRlRXJyb3JDb21wb25lbnQoe1xuICAgICAgdGl0bGU6IHtcbiAgICAgICAga2V5OiAnQWJwQWNjb3VudDo6NDA0TWVzc2FnZScsXG4gICAgICAgIGRlZmF1bHRWYWx1ZTogREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3I0MDQudGl0bGUsXG4gICAgICB9LFxuICAgICAgc3RhdHVzOiA0MDQsXG4gICAgfSk7XG4gIH1cblxuICBwcml2YXRlIHNob3dFcnJvcihcbiAgICBtZXNzYWdlPzogQ29uZmlnLkxvY2FsaXphdGlvblBhcmFtLFxuICAgIHRpdGxlPzogQ29uZmlnLkxvY2FsaXphdGlvblBhcmFtLFxuICAgIGJvZHk/OiBhbnksXG4gICk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcbiAgICBpZiAoYm9keSkge1xuICAgICAgaWYgKGJvZHkuZGV0YWlscykge1xuICAgICAgICBtZXNzYWdlID0gYm9keS5kZXRhaWxzO1xuICAgICAgICB0aXRsZSA9IGJvZHkubWVzc2FnZTtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIG1lc3NhZ2UgPSBib2R5Lm1lc3NhZ2UgfHwgREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3IudGl0bGU7XG4gICAgICB9XG4gICAgfVxuXG4gICAgcmV0dXJuIHRoaXMuY29uZmlybWF0aW9uU2VydmljZS5lcnJvcihtZXNzYWdlLCB0aXRsZSwge1xuICAgICAgaGlkZUNhbmNlbEJ0bjogdHJ1ZSxcbiAgICAgIHllc1RleHQ6ICdBYnBBY2NvdW50OjpDbG9zZScsXG4gICAgfSk7XG4gIH1cblxuICBwcml2YXRlIG5hdmlnYXRlVG9Mb2dpbigpIHtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKFxuICAgICAgbmV3IE5hdmlnYXRlKFsnL2FjY291bnQvbG9naW4nXSwgbnVsbCwgeyBzdGF0ZTogeyByZWRpcmVjdFVybDogdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChSb3V0ZXJTdGF0ZS51cmwpIH0gfSksXG4gICAgKTtcbiAgfVxuXG4gIGNyZWF0ZUVycm9yQ29tcG9uZW50KGluc3RhbmNlOiBQYXJ0aWFsPEh0dHBFcnJvcldyYXBwZXJDb21wb25lbnQ+KSB7XG4gICAgY29uc3QgcmVuZGVyZXIgPSB0aGlzLnJlbmRlcmVyRmFjdG9yeS5jcmVhdGVSZW5kZXJlcihudWxsLCBudWxsKTtcbiAgICBjb25zdCBob3N0ID0gcmVuZGVyZXIuc2VsZWN0Um9vdEVsZW1lbnQoZG9jdW1lbnQuYm9keSwgdHJ1ZSk7XG5cbiAgICB0aGlzLmNvbXBvbmVudFJlZiA9IHRoaXMuY2ZSZXMucmVzb2x2ZUNvbXBvbmVudEZhY3RvcnkoSHR0cEVycm9yV3JhcHBlckNvbXBvbmVudCkuY3JlYXRlKHRoaXMuaW5qZWN0b3IpO1xuXG4gICAgZm9yIChjb25zdCBrZXkgaW4gdGhpcy5jb21wb25lbnRSZWYuaW5zdGFuY2UpIHtcbiAgICAgIGlmICh0aGlzLmNvbXBvbmVudFJlZi5pbnN0YW5jZS5oYXNPd25Qcm9wZXJ0eShrZXkpKSB7XG4gICAgICAgIHRoaXMuY29tcG9uZW50UmVmLmluc3RhbmNlW2tleV0gPSBpbnN0YW5jZVtrZXldO1xuICAgICAgfVxuICAgIH1cbiAgICB0aGlzLmNvbXBvbmVudFJlZi5pbnN0YW5jZS5oaWRlQ2xvc2VJY29uID0gdGhpcy5odHRwRXJyb3JDb25maWcuZXJyb3JTY3JlZW4uaGlkZUNsb3NlSWNvbjtcbiAgICBpZiAodGhpcy5jYW5DcmVhdGVDdXN0b21FcnJvcihpbnN0YW5jZS5zdGF0dXMgYXMgRXJyb3JTY3JlZW5FcnJvckNvZGVzKSkge1xuICAgICAgdGhpcy5jb21wb25lbnRSZWYuaW5zdGFuY2UuY2ZSZXMgPSB0aGlzLmNmUmVzO1xuICAgICAgdGhpcy5jb21wb25lbnRSZWYuaW5zdGFuY2UuYXBwUmVmID0gdGhpcy5hcHBSZWY7XG4gICAgICB0aGlzLmNvbXBvbmVudFJlZi5pbnN0YW5jZS5pbmplY3RvciA9IHRoaXMuaW5qZWN0b3I7XG4gICAgICB0aGlzLmNvbXBvbmVudFJlZi5pbnN0YW5jZS5jdXN0b21Db21wb25lbnQgPSB0aGlzLmh0dHBFcnJvckNvbmZpZy5lcnJvclNjcmVlbi5jb21wb25lbnQ7XG4gICAgfVxuXG4gICAgdGhpcy5hcHBSZWYuYXR0YWNoVmlldyh0aGlzLmNvbXBvbmVudFJlZi5ob3N0Vmlldyk7XG4gICAgcmVuZGVyZXIuYXBwZW5kQ2hpbGQoaG9zdCwgKHRoaXMuY29tcG9uZW50UmVmLmhvc3RWaWV3IGFzIEVtYmVkZGVkVmlld1JlZjxhbnk+KS5yb290Tm9kZXNbMF0pO1xuXG4gICAgY29uc3QgZGVzdHJveSQgPSBuZXcgU3ViamVjdDx2b2lkPigpO1xuICAgIHRoaXMuY29tcG9uZW50UmVmLmluc3RhbmNlLmRlc3Ryb3kkID0gZGVzdHJveSQ7XG4gICAgZGVzdHJveSQuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgIHRoaXMuY29tcG9uZW50UmVmLmRlc3Ryb3koKTtcbiAgICAgIHRoaXMuY29tcG9uZW50UmVmID0gbnVsbDtcbiAgICB9KTtcbiAgfVxuXG4gIGNhbkNyZWF0ZUN1c3RvbUVycm9yKHN0YXR1czogRXJyb3JTY3JlZW5FcnJvckNvZGVzKTogYm9vbGVhbiB7XG4gICAgcmV0dXJuIHNucShcbiAgICAgICgpID0+XG4gICAgICAgIHRoaXMuaHR0cEVycm9yQ29uZmlnLmVycm9yU2NyZWVuLmNvbXBvbmVudCAmJlxuICAgICAgICB0aGlzLmh0dHBFcnJvckNvbmZpZy5lcnJvclNjcmVlbi5mb3JXaGljaEVycm9ycy5pbmRleE9mKHN0YXR1cykgPiAtMSxcbiAgICApO1xuICB9XG59XG4iXX0=