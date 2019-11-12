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
import { ErrorComponent } from '../components/error/error.component';
import { ConfirmationService } from '../services/confirmation.service';
import { HTTP_ERROR_CONFIG } from '../tokens/error-pages.token';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
import * as i2 from "../services/confirmation.service";
import * as i3 from "../tokens/error-pages.token";
/** @type {?} */
export var DEFAULT_ERROR_MESSAGES = {
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
var ErrorHandler = /** @class */ (function () {
    function ErrorHandler(actions, store, confirmationService, appRef, cfRes, rendererFactory, injector, httpErrorConfig) {
        var _this = this;
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
        function (res) {
            if (res instanceof RestOccurError) {
                var _a = res.payload, err_1 = _a === void 0 ? (/** @type {?} */ ({})) : _a;
                /** @type {?} */
                var body = snq((/**
                 * @return {?}
                 */
                function () { return ((/** @type {?} */ (err_1))).error.error; }), DEFAULT_ERROR_MESSAGES.defaultError.title);
                if (err_1 instanceof HttpErrorResponse && err_1.headers.get('_AbpErrorFormat')) {
                    /** @type {?} */
                    var confirmation$ = _this.showError(null, null, body);
                    if (err_1.status === 401) {
                        confirmation$.subscribe((/**
                         * @return {?}
                         */
                        function () {
                            _this.navigateToLogin();
                        }));
                    }
                }
                else {
                    switch (((/** @type {?} */ (err_1))).status) {
                        case 401:
                            _this.canCreateCustomError(401)
                                ? _this.show401Page()
                                : _this.showError({
                                    key: 'AbpAccount::DefaultErrorMessage401',
                                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.title,
                                }, {
                                    key: 'AbpAccount::DefaultErrorMessage401Detail',
                                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.details,
                                }).subscribe((/**
                                 * @return {?}
                                 */
                                function () { return _this.navigateToLogin(); }));
                            break;
                        case 403:
                            _this.createErrorComponent({
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
                            _this.canCreateCustomError(404)
                                ? _this.show404Page()
                                : _this.showError({
                                    key: 'AbpAccount::DefaultErrorMessage404',
                                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.details,
                                }, {
                                    key: 'AbpAccount::DefaultErrorMessage404Detail',
                                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.title,
                                });
                            break;
                        case 500:
                            _this.createErrorComponent({
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
                            if (((/** @type {?} */ (err_1))).statusText === 'Unknown Error') {
                                _this.createErrorComponent({
                                    title: {
                                        key: 'AbpAccount::DefaultErrorMessage',
                                        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
                                    },
                                });
                            }
                            break;
                        default:
                            _this.showError(DEFAULT_ERROR_MESSAGES.defaultError.details, DEFAULT_ERROR_MESSAGES.defaultError.title);
                            break;
                    }
                }
            }
            else if (res instanceof RouterError && snq((/**
             * @return {?}
             */
            function () { return res.event.error.indexOf('Cannot match') > -1; }), false)) {
                _this.show404Page();
            }
            else if (res instanceof RouterDataResolved && _this.componentRef) {
                _this.componentRef.destroy();
                _this.componentRef = null;
            }
        }));
    }
    /**
     * @private
     * @return {?}
     */
    ErrorHandler.prototype.show401Page = /**
     * @private
     * @return {?}
     */
    function () {
        this.createErrorComponent({
            title: {
                key: 'AbpAccount::401Message',
                defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.title,
            },
            status: 401,
        });
    };
    /**
     * @private
     * @return {?}
     */
    ErrorHandler.prototype.show404Page = /**
     * @private
     * @return {?}
     */
    function () {
        this.createErrorComponent({
            title: {
                key: 'AbpAccount::404Message',
                defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.title,
            },
            status: 404,
        });
    };
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
                message = body.message || DEFAULT_ERROR_MESSAGES.defaultError.title;
            }
        }
        return this.confirmationService.error(message, title, {
            hideCancelBtn: true,
            yesText: 'AbpAccount::Close',
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
        console.warn(this.store.selectSnapshot(RouterState.url));
        this.store.dispatch(new Navigate(['/account/login'], null, { state: { redirectUrl: this.store.selectSnapshot(RouterState.url) } }));
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
        var _this = this;
        /** @type {?} */
        var renderer = this.rendererFactory.createRenderer(null, null);
        /** @type {?} */
        var host = renderer.selectRootElement(document.body, true);
        this.componentRef = this.cfRes.resolveComponentFactory(ErrorComponent).create(this.injector);
        for (var key in this.componentRef.instance) {
            if (this.componentRef.instance.hasOwnProperty(key)) {
                this.componentRef.instance[key] = instance[key];
            }
        }
        if (this.canCreateCustomError((/** @type {?} */ (instance.status)))) {
            this.componentRef.instance.cfRes = this.cfRes;
            this.componentRef.instance.customComponent = this.httpErrorConfig.errorScreen.component;
        }
        this.appRef.attachView(this.componentRef.hostView);
        renderer.appendChild(host, ((/** @type {?} */ (this.componentRef.hostView))).rootNodes[0]);
        /** @type {?} */
        var destroy$ = new Subject();
        this.componentRef.instance.destroy$ = destroy$;
        destroy$.subscribe((/**
         * @return {?}
         */
        function () {
            _this.componentRef.destroy();
            _this.componentRef = null;
        }));
    };
    /**
     * @param {?} status
     * @return {?}
     */
    ErrorHandler.prototype.canCreateCustomError = /**
     * @param {?} status
     * @return {?}
     */
    function (status) {
        var _this = this;
        return snq((/**
         * @return {?}
         */
        function () {
            return _this.httpErrorConfig.errorScreen.component &&
                _this.httpErrorConfig.errorScreen.forWhichErrors.indexOf(status) > -1;
        }));
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
        { type: Injector },
        { type: undefined, decorators: [{ type: Inject, args: [HTTP_ERROR_CONFIG,] }] }
    ]; };
    /** @nocollapse */ ErrorHandler.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ErrorHandler_Factory() { return new ErrorHandler(i0.ɵɵinject(i1.Actions), i0.ɵɵinject(i1.Store), i0.ɵɵinject(i2.ConfirmationService), i0.ɵɵinject(i0.ApplicationRef), i0.ɵɵinject(i0.ComponentFactoryResolver), i0.ɵɵinject(i0.RendererFactory2), i0.ɵɵinject(i0.INJECTOR), i0.ɵɵinject(i3.HTTP_ERROR_CONFIG)); }, token: ErrorHandler, providedIn: "root" });
    return ErrorHandler;
}());
export { ErrorHandler };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZXJyb3IuaGFuZGxlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2hhbmRsZXJzL2Vycm9yLmhhbmRsZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQVUsY0FBYyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3RELE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFDTCxjQUFjLEVBQ2Qsd0JBQXdCLEVBRXhCLE1BQU0sRUFDTixVQUFVLEVBQ1YsUUFBUSxFQUNSLGdCQUFnQixHQUdqQixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsUUFBUSxFQUFFLFdBQVcsRUFBRSxXQUFXLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUM3RixPQUFPLEVBQUUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNqRSxPQUFPLEVBQWMsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzNDLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUN0QixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0scUNBQXFDLENBQUM7QUFHckUsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0sa0NBQWtDLENBQUM7QUFDdkUsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sNkJBQTZCLENBQUM7Ozs7OztBQUVoRSxNQUFNLEtBQU8sc0JBQXNCLEdBQUc7SUFDcEMsWUFBWSxFQUFFO1FBQ1osS0FBSyxFQUFFLHdCQUF3QjtRQUMvQixPQUFPLEVBQUUsa0NBQWtDO0tBQzVDO0lBQ0QsZUFBZSxFQUFFO1FBQ2YsS0FBSyxFQUFFLDRCQUE0QjtRQUNuQyxPQUFPLEVBQUUsMkVBQTJFO0tBQ3JGO0lBQ0QsZUFBZSxFQUFFO1FBQ2YsS0FBSyxFQUFFLHlCQUF5QjtRQUNoQyxPQUFPLEVBQUUsZ0RB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ZUFBZSxDQUFDLFdBQVcsQ0FBQyxTQUFTO2dCQUMxQyxLQUFJLENBQUMsZUFBZSxDQUFDLFdBQVcsQ0FBQyxjQUFjLENBQUMsT0FBTyxDQUFDLE1BQU0sQ0FBQyxHQUFHLENBQUMsQ0FBQztRQURwRSxDQUNvRSxFQUN2RSxDQUFDO0lBQ0osQ0FBQzs7Z0JBNUxGLFVBQVUsU0FBQyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUU7Ozs7Z0JBaEN6QixPQUFPO2dCQUFzQixLQUFLO2dCQU1sQyxtQkFBbUI7Z0JBakIxQixjQUFjO2dCQUNkLHdCQUF3QjtnQkFLeEIsZ0JBQWdCO2dCQURoQixRQUFRO2dEQWtETCxNQUFNLFNBQUMsaUJBQWlCOzs7dUJBMUQ3QjtDQTJPQyxBQTdMRCxJQTZMQztTQTVMWSxZQUFZOzs7SUFDdkIsb0NBQTJDOzs7OztJQUd6QywrQkFBd0I7Ozs7O0lBQ3hCLDZCQUFvQjs7Ozs7SUFDcEIsMkNBQWdEOzs7OztJQUNoRCw4QkFBOEI7Ozs7O0lBQzlCLDZCQUF1Qzs7Ozs7SUFDdkMsdUNBQXlDOzs7OztJQUN6QyxnQ0FBMEI7Ozs7O0lBQzFCLHVDQUFtRSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbmZpZywgUmVzdE9jY3VyRXJyb3IgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5pbXBvcnQgeyBIdHRwRXJyb3JSZXNwb25zZSB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbi9odHRwJztcclxuaW1wb3J0IHtcclxuICBBcHBsaWNhdGlvblJlZixcclxuICBDb21wb25lbnRGYWN0b3J5UmVzb2x2ZXIsXHJcbiAgRW1iZWRkZWRWaWV3UmVmLFxyXG4gIEluamVjdCxcclxuICBJbmplY3RhYmxlLFxyXG4gIEluamVjdG9yLFxyXG4gIFJlbmRlcmVyRmFjdG9yeTIsXHJcbiAgVHlwZSxcclxuICBDb21wb25lbnRSZWYsXHJcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IE5hdmlnYXRlLCBSb3V0ZXJFcnJvciwgUm91dGVyU3RhdGUsIFJvdXRlckRhdGFSZXNvbHZlZCB9IGZyb20gJ0BuZ3hzL3JvdXRlci1wbHVnaW4nO1xyXG5pbXBvcnQgeyBBY3Rpb25zLCBvZkFjdGlvblN1Y2Nlc3NmdWwsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBPYnNlcnZhYmxlLCBTdWJqZWN0IH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcclxuaW1wb3J0IHsgRXJyb3JDb21wb25lbnQgfSBmcm9tICcuLi9jb21wb25lbnRzL2Vycm9yL2Vycm9yLmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IEh0dHBFcnJvckNvbmZpZywgRXJyb3JTY3JlZW5FcnJvckNvZGVzIH0gZnJvbSAnLi4vbW9kZWxzL2NvbW1vbic7XHJcbmltcG9ydCB7IFRvYXN0ZXIgfSBmcm9tICcuLi9tb2RlbHMvdG9hc3Rlcic7XHJcbmltcG9ydCB7IENvbmZpcm1hdGlvblNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9jb25maXJtYXRpb24uc2VydmljZSc7XHJcbmltcG9ydCB7IEhUVFBfRVJST1JfQ09ORklHIH0gZnJvbSAnLi4vdG9rZW5zL2Vycm9yLXBhZ2VzLnRva2VuJztcclxuXHJcbmV4cG9ydCBjb25zdCBERUZBVUxUX0VSUk9SX01FU1NBR0VTID0ge1xyXG4gIGRlZmF1bHRFcnJvcjoge1xyXG4gICAgdGl0bGU6ICdBbiBlcnJvciBoYXMgb2NjdXJyZWQhJyxcclxuICAgIGRldGFpbHM6ICdFcnJvciBkZXRhaWwgbm90IHNlbnQgYnkgc2VydmVyLicsXHJcbiAgfSxcclxuICBkZWZhdWx0RXJyb3I0MDE6IHtcclxuICAgIHRpdGxlOiAnWW91IGFyZSBub3QgYXV0aGVudGljYXRlZCEnLFxyXG4gICAgZGV0YWlsczogJ1lvdSBzaG91bGQgYmUgYXV0aGVudGljYXRlZCAoc2lnbiBpbikgaW4gb3JkZXIgdG8gcGVyZm9ybSB0aGlzIG9wZXJhdGlvbi4nLFxyXG4gIH0sXHJcbiAgZGVmYXVsdEVycm9yNDAzOiB7XHJcbiAgICB0aXRsZTogJ1lvdSBhcmUgbm90IGF1dGhvcml6ZWQhJyxcclxuICAgIGRldGFpbHM6ICdZb3UgYXJlIG5vdCBhbGxvd2VkIHRvIHBlcmZvcm0gdGhpcyBvcGVyYXRpb24uJyxcclxuICB9LFxyXG4gIGRlZmF1bHRFcnJvcjQwNDoge1xyXG4gICAgdGl0bGU6ICdSZXNvdXJjZSBub3QgZm91bmQhJyxcclxuICAgIGRldGFpbHM6ICdUaGUgcmVzb3VyY2UgcmVxdWVzdGVkIGNvdWxkIG5vdCBmb3VuZCBvbiB0aGUgc2VydmVyLicsXHJcbiAgfSxcclxuICBkZWZhdWx0RXJyb3I1MDA6IHtcclxuICAgIHRpdGxlOiAnSW50ZXJuYWwgc2VydmVyIGVycm9yJyxcclxuICAgIGRldGFpbHM6ICdFcnJvciBkZXRhaWwgbm90IHNlbnQgYnkgc2VydmVyLicsXHJcbiAgfSxcclxufTtcclxuXHJcbkBJbmplY3RhYmxlKHsgcHJvdmlkZWRJbjogJ3Jvb3QnIH0pXHJcbmV4cG9ydCBjbGFzcyBFcnJvckhhbmRsZXIge1xyXG4gIGNvbXBvbmVudFJlZjogQ29tcG9uZW50UmVmPEVycm9yQ29tcG9uZW50PjtcclxuXHJcbiAgY29uc3RydWN0b3IoXHJcbiAgICBwcml2YXRlIGFjdGlvbnM6IEFjdGlvbnMsXHJcbiAgICBwcml2YXRlIHN0b3JlOiBTdG9yZSxcclxuICAgIHByaXZhdGUgY29uZmlybWF0aW9uU2VydmljZTogQ29uZmlybWF0aW9uU2VydmljZSxcclxuICAgIHByaXZhdGUgYXBwUmVmOiBBcHBsaWNhdGlvblJlZixcclxuICAgIHByaXZhdGUgY2ZSZXM6IENvbXBvbmVudEZhY3RvcnlSZXNvbHZlcixcclxuICAgIHByaXZhdGUgcmVuZGVyZXJGYWN0b3J5OiBSZW5kZXJlckZhY3RvcnkyLFxyXG4gICAgcHJpdmF0ZSBpbmplY3RvcjogSW5qZWN0b3IsXHJcbiAgICBASW5qZWN0KEhUVFBfRVJST1JfQ09ORklHKSBwcml2YXRlIGh0dHBFcnJvckNvbmZpZzogSHR0cEVycm9yQ29uZmlnLFxyXG4gICkge1xyXG4gICAgdGhpcy5hY3Rpb25zLnBpcGUob2ZBY3Rpb25TdWNjZXNzZnVsKFJlc3RPY2N1ckVycm9yLCBSb3V0ZXJFcnJvciwgUm91dGVyRGF0YVJlc29sdmVkKSkuc3Vic2NyaWJlKHJlcyA9PiB7XHJcbiAgICAgIGlmIChyZXMgaW5zdGFuY2VvZiBSZXN0T2NjdXJFcnJvcikge1xyXG4gICAgICAgIGNvbnN0IHsgcGF5bG9hZDogZXJyID0ge30gYXMgSHR0cEVycm9yUmVzcG9uc2UgfCBhbnkgfSA9IHJlcztcclxuICAgICAgICBjb25zdCBib2R5ID0gc25xKCgpID0+IChlcnIgYXMgSHR0cEVycm9yUmVzcG9uc2UpLmVycm9yLmVycm9yLCBERUZBVUxUX0VSUk9SX01FU1NBR0VTLmRlZmF1bHRFcnJvci50aXRsZSk7XHJcblxyXG4gICAgICAgIGlmIChlcnIgaW5zdGFuY2VvZiBIdHRwRXJyb3JSZXNwb25zZSAmJiBlcnIuaGVhZGVycy5nZXQoJ19BYnBFcnJvckZvcm1hdCcpKSB7XHJcbiAgICAgICAgICBjb25zdCBjb25maXJtYXRpb24kID0gdGhpcy5zaG93RXJyb3IobnVsbCwgbnVsbCwgYm9keSk7XHJcblxyXG4gICAgICAgICAgaWYgKGVyci5zdGF0dXMgPT09IDQwMSkge1xyXG4gICAgICAgICAgICBjb25maXJtYXRpb24kLnN1YnNjcmliZSgoKSA9PiB7XHJcbiAgICAgICAgICAgICAgdGhpcy5uYXZpZ2F0ZVRvTG9naW4oKTtcclxuICAgICAgICAgICAgfSk7XHJcbiAgICAgICAgICB9XHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgIHN3aXRjaCAoKGVyciBhcyBIdHRwRXJyb3JSZXNwb25zZSkuc3RhdHVzKSB7XHJcbiAgICAgICAgICAgIGNhc2UgNDAxOlxyXG4gICAgICAgICAgICAgIHRoaXMuY2FuQ3JlYXRlQ3VzdG9tRXJyb3IoNDAxKVxyXG4gICAgICAgICAgICAgICAgPyB0aGlzLnNob3c0MDFQYWdlKClcclxuICAgICAgICAgICAgICAgIDogdGhpcy5zaG93RXJyb3IoXHJcbiAgICAgICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgICAga2V5OiAnQWJwQWNjb3VudDo6RGVmYXVsdEVycm9yTWVzc2FnZTQwMScsXHJcbiAgICAgICAgICAgICAgICAgICAgICBkZWZhdWx0VmFsdWU6IERFRkFVTFRfRVJST1JfTUVTU0FHRVMuZGVmYXVsdEVycm9yNDAxLnRpdGxlLFxyXG4gICAgICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgICAga2V5OiAnQWJwQWNjb3VudDo6RGVmYXVsdEVycm9yTWVzc2FnZTQwMURldGFpbCcsXHJcbiAgICAgICAgICAgICAgICAgICAgICBkZWZhdWx0VmFsdWU6IERFRkFVTFRfRVJST1JfTUVTU0FHRVMuZGVmYXVsdEVycm9yNDAxLmRldGFpbHMsXHJcbiAgICAgICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgICAgKS5zdWJzY3JpYmUoKCkgPT4gdGhpcy5uYXZpZ2F0ZVRvTG9naW4oKSk7XHJcbiAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgIGNhc2UgNDAzOlxyXG4gICAgICAgICAgICAgIHRoaXMuY3JlYXRlRXJyb3JDb21wb25lbnQoe1xyXG4gICAgICAgICAgICAgICAgdGl0bGU6IHtcclxuICAgICAgICAgICAgICAgICAga2V5OiAnQWJwQWNjb3VudDo6RGVmYXVsdEVycm9yTWVzc2FnZTQwMycsXHJcbiAgICAgICAgICAgICAgICAgIGRlZmF1bHRWYWx1ZTogREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3I0MDMudGl0bGUsXHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAgZGV0YWlsczoge1xyXG4gICAgICAgICAgICAgICAgICBrZXk6ICdBYnBBY2NvdW50OjpEZWZhdWx0RXJyb3JNZXNzYWdlNDAzRGV0YWlsJyxcclxuICAgICAgICAgICAgICAgICAgZGVmYXVsdFZhbHVlOiBERUZBVUxUX0VSUk9SX01FU1NBR0VTLmRlZmF1bHRFcnJvcjQwMy5kZXRhaWxzLFxyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHN0YXR1czogNDAzLFxyXG4gICAgICAgICAgICAgIH0pO1xyXG4gICAgICAgICAgICAgIGJyZWFrO1xyXG4gICAgICAgICAgICBjYXNlIDQwNDpcclxuICAgICAgICAgICAgICB0aGlzLmNhbkNyZWF0ZUN1c3RvbUVycm9yKDQwNClcclxuICAgICAgICAgICAgICAgID8gdGhpcy5zaG93NDA0UGFnZSgpXHJcbiAgICAgICAgICAgICAgICA6IHRoaXMuc2hvd0Vycm9yKFxyXG4gICAgICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICAgIGtleTogJ0FicEFjY291bnQ6OkRlZmF1bHRFcnJvck1lc3NhZ2U0MDQnLFxyXG4gICAgICAgICAgICAgICAgICAgICAgZGVmYXVsdFZhbHVlOiBERUZBVUxUX0VSUk9SX01FU1NBR0VTLmRlZmF1bHRFcnJvcjQwNC5kZXRhaWxzLFxyXG4gICAgICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgICAga2V5OiAnQWJwQWNjb3VudDo6RGVmYXVsdEVycm9yTWVzc2FnZTQwNERldGFpbCcsXHJcbiAgICAgICAgICAgICAgICAgICAgICBkZWZhdWx0VmFsdWU6IERFRkFVTFRfRVJST1JfTUVTU0FHRVMuZGVmYXVsdEVycm9yNDA0LnRpdGxlLFxyXG4gICAgICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICAgICk7XHJcbiAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgIGNhc2UgNTAwOlxyXG4gICAgICAgICAgICAgIHRoaXMuY3JlYXRlRXJyb3JDb21wb25lbnQoe1xyXG4gICAgICAgICAgICAgICAgdGl0bGU6IHtcclxuICAgICAgICAgICAgICAgICAga2V5OiAnQWJwQWNjb3VudDo6NTAwTWVzc2FnZScsXHJcbiAgICAgICAgICAgICAgICAgIGRlZmF1bHRWYWx1ZTogREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3I1MDAudGl0bGUsXHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAgZGV0YWlsczoge1xyXG4gICAgICAgICAgICAgICAgICBrZXk6ICdBYnBBY2NvdW50OjpJbnRlcm5hbFNlcnZlckVycm9yTWVzc2FnZScsXHJcbiAgICAgICAgICAgICAgICAgIGRlZmF1bHRWYWx1ZTogREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3I1MDAuZGV0YWlscyxcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICBzdGF0dXM6IDUwMCxcclxuICAgICAgICAgICAgICB9KTtcclxuICAgICAgICAgICAgICBicmVhaztcclxuICAgICAgICAgICAgY2FzZSAwOlxyXG4gICAgICAgICAgICAgIGlmICgoZXJyIGFzIEh0dHBFcnJvclJlc3BvbnNlKS5zdGF0dXNUZXh0ID09PSAnVW5rbm93biBFcnJvcicpIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuY3JlYXRlRXJyb3JDb21wb25lbnQoe1xyXG4gICAgICAgICAgICAgICAgICB0aXRsZToge1xyXG4gICAgICAgICAgICAgICAgICAgIGtleTogJ0FicEFjY291bnQ6OkRlZmF1bHRFcnJvck1lc3NhZ2UnLFxyXG4gICAgICAgICAgICAgICAgICAgIGRlZmF1bHRWYWx1ZTogREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3IudGl0bGUsXHJcbiAgICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB9KTtcclxuICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgIGRlZmF1bHQ6XHJcbiAgICAgICAgICAgICAgdGhpcy5zaG93RXJyb3IoREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3IuZGV0YWlscywgREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3IudGl0bGUpO1xyXG4gICAgICAgICAgICAgIGJyZWFrO1xyXG4gICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgICAgfSBlbHNlIGlmIChyZXMgaW5zdGFuY2VvZiBSb3V0ZXJFcnJvciAmJiBzbnEoKCkgPT4gcmVzLmV2ZW50LmVycm9yLmluZGV4T2YoJ0Nhbm5vdCBtYXRjaCcpID4gLTEsIGZhbHNlKSkge1xyXG4gICAgICAgIHRoaXMuc2hvdzQwNFBhZ2UoKTtcclxuICAgICAgfSBlbHNlIGlmIChyZXMgaW5zdGFuY2VvZiBSb3V0ZXJEYXRhUmVzb2x2ZWQgJiYgdGhpcy5jb21wb25lbnRSZWYpIHtcclxuICAgICAgICB0aGlzLmNvbXBvbmVudFJlZi5kZXN0cm95KCk7XHJcbiAgICAgICAgdGhpcy5jb21wb25lbnRSZWYgPSBudWxsO1xyXG4gICAgICB9XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIHByaXZhdGUgc2hvdzQwMVBhZ2UoKSB7XHJcbiAgICB0aGlzLmNyZWF0ZUVycm9yQ29tcG9uZW50KHtcclxuICAgICAgdGl0bGU6IHtcclxuICAgICAgICBrZXk6ICdBYnBBY2NvdW50Ojo0MDFNZXNzYWdlJyxcclxuICAgICAgICBkZWZhdWx0VmFsdWU6IERFRkFVTFRfRVJST1JfTUVTU0FHRVMuZGVmYXVsdEVycm9yNDAxLnRpdGxlLFxyXG4gICAgICB9LFxyXG4gICAgICBzdGF0dXM6IDQwMSxcclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgcHJpdmF0ZSBzaG93NDA0UGFnZSgpIHtcclxuICAgIHRoaXMuY3JlYXRlRXJyb3JDb21wb25lbnQoe1xyXG4gICAgICB0aXRsZToge1xyXG4gICAgICAgIGtleTogJ0FicEFjY291bnQ6OjQwNE1lc3NhZ2UnLFxyXG4gICAgICAgIGRlZmF1bHRWYWx1ZTogREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3I0MDQudGl0bGUsXHJcbiAgICAgIH0sXHJcbiAgICAgIHN0YXR1czogNDA0LFxyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBwcml2YXRlIHNob3dFcnJvcihcclxuICAgIG1lc3NhZ2U/OiBDb25maWcuTG9jYWxpemF0aW9uUGFyYW0sXHJcbiAgICB0aXRsZT86IENvbmZpZy5Mb2NhbGl6YXRpb25QYXJhbSxcclxuICAgIGJvZHk/OiBhbnksXHJcbiAgKTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xyXG4gICAgaWYgKGJvZHkpIHtcclxuICAgICAgaWYgKGJvZHkuZGV0YWlscykge1xyXG4gICAgICAgIG1lc3NhZ2UgPSBib2R5LmRldGFpbHM7XHJcbiAgICAgICAgdGl0bGUgPSBib2R5Lm1lc3NhZ2U7XHJcbiAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgbWVzc2FnZSA9IGJvZHkubWVzc2FnZSB8fCBERUZBVUxUX0VSUk9SX01FU1NBR0VTLmRlZmF1bHRFcnJvci50aXRsZTtcclxuICAgICAgfVxyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiB0aGlzLmNvbmZpcm1hdGlvblNlcnZpY2UuZXJyb3IobWVzc2FnZSwgdGl0bGUsIHtcclxuICAgICAgaGlkZUNhbmNlbEJ0bjogdHJ1ZSxcclxuICAgICAgeWVzVGV4dDogJ0FicEFjY291bnQ6OkNsb3NlJyxcclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgcHJpdmF0ZSBuYXZpZ2F0ZVRvTG9naW4oKSB7XHJcbiAgICBjb25zb2xlLndhcm4odGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChSb3V0ZXJTdGF0ZS51cmwpKTtcclxuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2goXHJcbiAgICAgIG5ldyBOYXZpZ2F0ZShbJy9hY2NvdW50L2xvZ2luJ10sIG51bGwsIHsgc3RhdGU6IHsgcmVkaXJlY3RVcmw6IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoUm91dGVyU3RhdGUudXJsKSB9IH0pLFxyXG4gICAgKTtcclxuICB9XHJcblxyXG4gIGNyZWF0ZUVycm9yQ29tcG9uZW50KGluc3RhbmNlOiBQYXJ0aWFsPEVycm9yQ29tcG9uZW50Pikge1xyXG4gICAgY29uc3QgcmVuZGVyZXIgPSB0aGlzLnJlbmRlcmVyRmFjdG9yeS5jcmVhdGVSZW5kZXJlcihudWxsLCBudWxsKTtcclxuICAgIGNvbnN0IGhvc3QgPSByZW5kZXJlci5zZWxlY3RSb290RWxlbWVudChkb2N1bWVudC5ib2R5LCB0cnVlKTtcclxuXHJcbiAgICB0aGlzLmNvbXBvbmVudFJlZiA9IHRoaXMuY2ZSZXMucmVzb2x2ZUNvbXBvbmVudEZhY3RvcnkoRXJyb3JDb21wb25lbnQpLmNyZWF0ZSh0aGlzLmluamVjdG9yKTtcclxuXHJcbiAgICBmb3IgKGNvbnN0IGtleSBpbiB0aGlzLmNvbXBvbmVudFJlZi5pbnN0YW5jZSkge1xyXG4gICAgICBpZiAodGhpcy5jb21wb25lbnRSZWYuaW5zdGFuY2UuaGFzT3duUHJvcGVydHkoa2V5KSkge1xyXG4gICAgICAgIHRoaXMuY29tcG9uZW50UmVmLmluc3RhbmNlW2tleV0gPSBpbnN0YW5jZVtrZXldO1xyXG4gICAgICB9XHJcbiAgICB9XHJcblxyXG4gICAgaWYgKHRoaXMuY2FuQ3JlYXRlQ3VzdG9tRXJyb3IoaW5zdGFuY2Uuc3RhdHVzIGFzIEVycm9yU2NyZWVuRXJyb3JDb2RlcykpIHtcclxuICAgICAgdGhpcy5jb21wb25lbnRSZWYuaW5zdGFuY2UuY2ZSZXMgPSB0aGlzLmNmUmVzO1xyXG4gICAgICB0aGlzLmNvbXBvbmVudFJlZi5pbnN0YW5jZS5jdXN0b21Db21wb25lbnQgPSB0aGlzLmh0dHBFcnJvckNvbmZpZy5lcnJvclNjcmVlbi5jb21wb25lbnQ7XHJcbiAgICB9XHJcblxyXG4gICAgdGhpcy5hcHBSZWYuYXR0YWNoVmlldyh0aGlzLmNvbXBvbmVudFJlZi5ob3N0Vmlldyk7XHJcbiAgICByZW5kZXJlci5hcHBlbmRDaGlsZChob3N0LCAodGhpcy5jb21wb25lbnRSZWYuaG9zdFZpZXcgYXMgRW1iZWRkZWRWaWV3UmVmPGFueT4pLnJvb3ROb2Rlc1swXSk7XHJcblxyXG4gICAgY29uc3QgZGVzdHJveSQgPSBuZXcgU3ViamVjdDx2b2lkPigpO1xyXG4gICAgdGhpcy5jb21wb25lbnRSZWYuaW5zdGFuY2UuZGVzdHJveSQgPSBkZXN0cm95JDtcclxuICAgIGRlc3Ryb3kkLnN1YnNjcmliZSgoKSA9PiB7XHJcbiAgICAgIHRoaXMuY29tcG9uZW50UmVmLmRlc3Ryb3koKTtcclxuICAgICAgdGhpcy5jb21wb25lbnRSZWYgPSBudWxsO1xyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBjYW5DcmVhdGVDdXN0b21FcnJvcihzdGF0dXM6IEVycm9yU2NyZWVuRXJyb3JDb2Rlcyk6IGJvb2xlYW4ge1xyXG4gICAgcmV0dXJuIHNucShcclxuICAgICAgKCkgPT5cclxuICAgICAgICB0aGlzLmh0dHBFcnJvckNvbmZpZy5lcnJvclNjcmVlbi5jb21wb25lbnQgJiZcclxuICAgICAgICB0aGlzLmh0dHBFcnJvckNvbmZpZy5lcnJvclNjcmVlbi5mb3JXaGljaEVycm9ycy5pbmRleE9mKHN0YXR1cykgPiAtMSxcclxuICAgICk7XHJcbiAgfVxyXG59XHJcbiJdfQ==