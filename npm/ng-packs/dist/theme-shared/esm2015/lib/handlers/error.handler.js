/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { RestOccurError } from '@abp/ng.core';
import { HttpErrorResponse } from '@angular/common/http';
import { ApplicationRef, ComponentFactoryResolver, Injectable, Injector, NgZone, RendererFactory2, } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, ofActionSuccessful, Store } from '@ngxs/store';
import snq from 'snq';
import { ErrorComponent } from '../components/error/error.component';
import { ConfirmationService } from '../services/confirmation.service';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
import * as i2 from "@angular/router";
import * as i3 from "../services/confirmation.service";
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
        title: '500',
        details: { key: 'AbpAccount::InternalServerErrorMessage', defaultValue: 'Error detail not sent by server.' },
    },
    defaultErrorUnknown: {
        title: 'Unknown Error',
        details: { key: 'AbpAccount::InternalServerErrorMessage', defaultValue: 'Error detail not sent by server.' },
    },
};
export class ErrorHandler {
    /**
     * @param {?} actions
     * @param {?} router
     * @param {?} ngZone
     * @param {?} store
     * @param {?} confirmationService
     * @param {?} appRef
     * @param {?} cfRes
     * @param {?} rendererFactory
     * @param {?} injector
     */
    constructor(actions, router, ngZone, store, confirmationService, appRef, cfRes, rendererFactory, injector) {
        this.actions = actions;
        this.router = router;
        this.ngZone = ngZone;
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
                        this.showError(DEFAULT_ERROR_MESSAGES.defaultError401.details, DEFAULT_ERROR_MESSAGES.defaultError401.title).subscribe((/**
                         * @return {?}
                         */
                        () => this.navigateToLogin()));
                        break;
                    case 403:
                        this.createErrorComponent({
                            title: DEFAULT_ERROR_MESSAGES.defaultError403.title,
                            details: DEFAULT_ERROR_MESSAGES.defaultError403.details,
                        });
                        break;
                    case 404:
                        this.showError(DEFAULT_ERROR_MESSAGES.defaultError404.details, DEFAULT_ERROR_MESSAGES.defaultError404.title);
                        break;
                    case 500:
                        this.createErrorComponent({
                            title: DEFAULT_ERROR_MESSAGES.defaultError500.title,
                            details: DEFAULT_ERROR_MESSAGES.defaultError500.details,
                        });
                        break;
                    case 0:
                        if (((/** @type {?} */ (err))).statusText === 'Unknown Error') {
                            this.createErrorComponent({
                                title: DEFAULT_ERROR_MESSAGES.defaultErrorUnknown.title,
                                details: DEFAULT_ERROR_MESSAGES.defaultErrorUnknown.details,
                            });
                        }
                        break;
                    default:
                        this.showError(DEFAULT_ERROR_MESSAGES.defaultError.details, DEFAULT_ERROR_MESSAGES.defaultError.title);
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
                message = body.message || DEFAULT_ERROR_MESSAGES.defaultError.title;
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
        this.ngZone.run((/**
         * @return {?}
         */
        () => {
            this.router.navigate(['/account/login'], {
                state: { redirectUrl: this.router.url },
            });
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
        const host = renderer.selectRootElement(document.body, true);
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
    { type: Router },
    { type: NgZone },
    { type: Store },
    { type: ConfirmationService },
    { type: ApplicationRef },
    { type: ComponentFactoryResolver },
    { type: RendererFactory2 },
    { type: Injector }
];
/** @nocollapse */ ErrorHandler.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ErrorHandler_Factory() { return new ErrorHandler(i0.ɵɵinject(i1.Actions), i0.ɵɵinject(i2.Router), i0.ɵɵinject(i0.NgZone), i0.ɵɵinject(i1.Store), i0.ɵɵinject(i3.ConfirmationService), i0.ɵɵinject(i0.ApplicationRef), i0.ɵɵinject(i0.ComponentFactoryResolver), i0.ɵɵinject(i0.RendererFactory2), i0.ɵɵinject(i0.INJECTOR)); }, token: ErrorHandler, providedIn: "root" });
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
    ErrorHandler.prototype.router;
    /**
     * @type {?}
     * @private
     */
    ErrorHandler.prototype.ngZone;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZXJyb3IuaGFuZGxlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2hhbmRsZXJzL2Vycm9yLmhhbmRsZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDOUMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDekQsT0FBTyxFQUNMLGNBQWMsRUFDZCx3QkFBd0IsRUFFeEIsVUFBVSxFQUNWLFFBQVEsRUFDUixNQUFNLEVBQ04sZ0JBQWdCLEdBQ2pCLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUN6QyxPQUFPLEVBQUUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUVqRSxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFDdEIsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHFDQUFxQyxDQUFDO0FBRXJFLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLGtDQUFrQyxDQUFDOzs7Ozs7QUFFdkUsTUFBTSxPQUFPLHNCQUFzQixHQUFHO0lBQ3BDLFlBQVksRUFBRTtRQUNaLEtBQUssRUFBRSx3QkFBd0I7UUFDL0IsT0FBTyxFQUFFLGtDQUFrQztLQUM1QztJQUNELGVBQWUsRUFBRTtRQUNmLEtBQUssRUFBRSw0QkFBNEI7UUFDbkMsT0FBTyxFQUFFLDJFQUEyRTtLQUNyRjtJQUNELGVBQWUsRUFBRTtRQUNmLEtBQUssRUFBRSx5QkFBeUI7UUFDaEMsT0FBTyxFQUFFLGdEQUFnRDtLQUMxRDtJQUNELGVBQWUsRUFBRTtRQUNmLEtBQUssRUFBRSxxQkFBcUI7UUFDNUIsT0FBTyxFQUFFLHVEQUF1RDtLQUNqRTtJQUNELGVBQWUsRUFBRTtRQUNmLEtBQUssRUFBRSxLQUFLO1FBQ1osT0FB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QVUsU0FBQyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUU7Ozs7WUFsQ3pCLE9BQU87WUFEUCxNQUFNO1lBSGIsTUFBTTtZQUk4QixLQUFLO1lBS2xDLG1CQUFtQjtZQWQxQixjQUFjO1lBQ2Qsd0JBQXdCO1lBS3hCLGdCQUFnQjtZQUZoQixRQUFROzs7Ozs7OztJQTBDTiwrQkFBd0I7Ozs7O0lBQ3hCLDhCQUFzQjs7Ozs7SUFDdEIsOEJBQXNCOzs7OztJQUN0Qiw2QkFBb0I7Ozs7O0lBQ3BCLDJDQUFnRDs7Ozs7SUFDaEQsOEJBQThCOzs7OztJQUM5Qiw2QkFBdUM7Ozs7O0lBQ3ZDLHVDQUF5Qzs7Ozs7SUFDekMsZ0NBQTBCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgUmVzdE9jY3VyRXJyb3IgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgSHR0cEVycm9yUmVzcG9uc2UgfSBmcm9tICdAYW5ndWxhci9jb21tb24vaHR0cCc7XG5pbXBvcnQge1xuICBBcHBsaWNhdGlvblJlZixcbiAgQ29tcG9uZW50RmFjdG9yeVJlc29sdmVyLFxuICBFbWJlZGRlZFZpZXdSZWYsXG4gIEluamVjdGFibGUsXG4gIEluamVjdG9yLFxuICBOZ1pvbmUsXG4gIFJlbmRlcmVyRmFjdG9yeTIsXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgUm91dGVyIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IEFjdGlvbnMsIG9mQWN0aW9uU3VjY2Vzc2Z1bCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQgeyBFcnJvckNvbXBvbmVudCB9IGZyb20gJy4uL2NvbXBvbmVudHMvZXJyb3IvZXJyb3IuY29tcG9uZW50JztcbmltcG9ydCB7IFRvYXN0ZXIgfSBmcm9tICcuLi9tb2RlbHMvdG9hc3Rlcic7XG5pbXBvcnQgeyBDb25maXJtYXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvY29uZmlybWF0aW9uLnNlcnZpY2UnO1xuXG5leHBvcnQgY29uc3QgREVGQVVMVF9FUlJPUl9NRVNTQUdFUyA9IHtcbiAgZGVmYXVsdEVycm9yOiB7XG4gICAgdGl0bGU6ICdBbiBlcnJvciBoYXMgb2NjdXJyZWQhJyxcbiAgICBkZXRhaWxzOiAnRXJyb3IgZGV0YWlsIG5vdCBzZW50IGJ5IHNlcnZlci4nLFxuICB9LFxuICBkZWZhdWx0RXJyb3I0MDE6IHtcbiAgICB0aXRsZTogJ1lvdSBhcmUgbm90IGF1dGhlbnRpY2F0ZWQhJyxcbiAgICBkZXRhaWxzOiAnWW91IHNob3VsZCBiZSBhdXRoZW50aWNhdGVkIChzaWduIGluKSBpbiBvcmRlciB0byBwZXJmb3JtIHRoaXMgb3BlcmF0aW9uLicsXG4gIH0sXG4gIGRlZmF1bHRFcnJvcjQwMzoge1xuICAgIHRpdGxlOiAnWW91IGFyZSBub3QgYXV0aG9yaXplZCEnLFxuICAgIGRldGFpbHM6ICdZb3UgYXJlIG5vdCBhbGxvd2VkIHRvIHBlcmZvcm0gdGhpcyBvcGVyYXRpb24uJyxcbiAgfSxcbiAgZGVmYXVsdEVycm9yNDA0OiB7XG4gICAgdGl0bGU6ICdSZXNvdXJjZSBub3QgZm91bmQhJyxcbiAgICBkZXRhaWxzOiAnVGhlIHJlc291cmNlIHJlcXVlc3RlZCBjb3VsZCBub3QgZm91bmQgb24gdGhlIHNlcnZlci4nLFxuICB9LFxuICBkZWZhdWx0RXJyb3I1MDA6IHtcbiAgICB0aXRsZTogJzUwMCcsXG4gICAgZGV0YWlsczogeyBrZXk6ICdBYnBBY2NvdW50OjpJbnRlcm5hbFNlcnZlckVycm9yTWVzc2FnZScsIGRlZmF1bHRWYWx1ZTogJ0Vycm9yIGRldGFpbCBub3Qgc2VudCBieSBzZXJ2ZXIuJyB9LFxuICB9LFxuICBkZWZhdWx0RXJyb3JVbmtub3duOiB7XG4gICAgdGl0bGU6ICdVbmtub3duIEVycm9yJyxcbiAgICBkZXRhaWxzOiB7IGtleTogJ0FicEFjY291bnQ6OkludGVybmFsU2VydmVyRXJyb3JNZXNzYWdlJywgZGVmYXVsdFZhbHVlOiAnRXJyb3IgZGV0YWlsIG5vdCBzZW50IGJ5IHNlcnZlci4nIH0sXG4gIH0sXG59O1xuXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxuZXhwb3J0IGNsYXNzIEVycm9ySGFuZGxlciB7XG4gIGNvbnN0cnVjdG9yKFxuICAgIHByaXZhdGUgYWN0aW9uczogQWN0aW9ucyxcbiAgICBwcml2YXRlIHJvdXRlcjogUm91dGVyLFxuICAgIHByaXZhdGUgbmdab25lOiBOZ1pvbmUsXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXG4gICAgcHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlLFxuICAgIHByaXZhdGUgYXBwUmVmOiBBcHBsaWNhdGlvblJlZixcbiAgICBwcml2YXRlIGNmUmVzOiBDb21wb25lbnRGYWN0b3J5UmVzb2x2ZXIsXG4gICAgcHJpdmF0ZSByZW5kZXJlckZhY3Rvcnk6IFJlbmRlcmVyRmFjdG9yeTIsXG4gICAgcHJpdmF0ZSBpbmplY3RvcjogSW5qZWN0b3IsXG4gICkge1xuICAgIGFjdGlvbnMucGlwZShvZkFjdGlvblN1Y2Nlc3NmdWwoUmVzdE9jY3VyRXJyb3IpKS5zdWJzY3JpYmUocmVzID0+IHtcbiAgICAgIGNvbnN0IHsgcGF5bG9hZDogZXJyID0ge30gYXMgSHR0cEVycm9yUmVzcG9uc2UgfCBhbnkgfSA9IHJlcztcbiAgICAgIGNvbnN0IGJvZHkgPSBzbnEoKCkgPT4gKGVyciBhcyBIdHRwRXJyb3JSZXNwb25zZSkuZXJyb3IuZXJyb3IsIERFRkFVTFRfRVJST1JfTUVTU0FHRVMuZGVmYXVsdEVycm9yLnRpdGxlKTtcblxuICAgICAgaWYgKGVyciBpbnN0YW5jZW9mIEh0dHBFcnJvclJlc3BvbnNlICYmIGVyci5oZWFkZXJzLmdldCgnX0FicEVycm9yRm9ybWF0JykpIHtcbiAgICAgICAgY29uc3QgY29uZmlybWF0aW9uJCA9IHRoaXMuc2hvd0Vycm9yKG51bGwsIG51bGwsIGJvZHkpO1xuXG4gICAgICAgIGlmIChlcnIuc3RhdHVzID09PSA0MDEpIHtcbiAgICAgICAgICBjb25maXJtYXRpb24kLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgICAgICB0aGlzLm5hdmlnYXRlVG9Mb2dpbigpO1xuICAgICAgICAgIH0pO1xuICAgICAgICB9XG4gICAgICB9IGVsc2Uge1xuICAgICAgICBzd2l0Y2ggKChlcnIgYXMgSHR0cEVycm9yUmVzcG9uc2UpLnN0YXR1cykge1xuICAgICAgICAgIGNhc2UgNDAxOlxuICAgICAgICAgICAgdGhpcy5zaG93RXJyb3IoXG4gICAgICAgICAgICAgIERFRkFVTFRfRVJST1JfTUVTU0FHRVMuZGVmYXVsdEVycm9yNDAxLmRldGFpbHMsXG4gICAgICAgICAgICAgIERFRkFVTFRfRVJST1JfTUVTU0FHRVMuZGVmYXVsdEVycm9yNDAxLnRpdGxlLFxuICAgICAgICAgICAgKS5zdWJzY3JpYmUoKCkgPT4gdGhpcy5uYXZpZ2F0ZVRvTG9naW4oKSk7XG4gICAgICAgICAgICBicmVhaztcbiAgICAgICAgICBjYXNlIDQwMzpcbiAgICAgICAgICAgIHRoaXMuY3JlYXRlRXJyb3JDb21wb25lbnQoe1xuICAgICAgICAgICAgICB0aXRsZTogREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3I0MDMudGl0bGUsXG4gICAgICAgICAgICAgIGRldGFpbHM6IERFRkFVTFRfRVJST1JfTUVTU0FHRVMuZGVmYXVsdEVycm9yNDAzLmRldGFpbHMsXG4gICAgICAgICAgICB9KTtcbiAgICAgICAgICAgIGJyZWFrO1xuICAgICAgICAgIGNhc2UgNDA0OlxuICAgICAgICAgICAgdGhpcy5zaG93RXJyb3IoXG4gICAgICAgICAgICAgIERFRkFVTFRfRVJST1JfTUVTU0FHRVMuZGVmYXVsdEVycm9yNDA0LmRldGFpbHMsXG4gICAgICAgICAgICAgIERFRkFVTFRfRVJST1JfTUVTU0FHRVMuZGVmYXVsdEVycm9yNDA0LnRpdGxlLFxuICAgICAgICAgICAgKTtcbiAgICAgICAgICAgIGJyZWFrO1xuICAgICAgICAgIGNhc2UgNTAwOlxuICAgICAgICAgICAgdGhpcy5jcmVhdGVFcnJvckNvbXBvbmVudCh7XG4gICAgICAgICAgICAgIHRpdGxlOiBERUZBVUxUX0VSUk9SX01FU1NBR0VTLmRlZmF1bHRFcnJvcjUwMC50aXRsZSxcbiAgICAgICAgICAgICAgZGV0YWlsczogREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3I1MDAuZGV0YWlscyxcbiAgICAgICAgICAgIH0pO1xuICAgICAgICAgICAgYnJlYWs7XG4gICAgICAgICAgY2FzZSAwOlxuICAgICAgICAgICAgaWYgKChlcnIgYXMgSHR0cEVycm9yUmVzcG9uc2UpLnN0YXR1c1RleHQgPT09ICdVbmtub3duIEVycm9yJykge1xuICAgICAgICAgICAgICB0aGlzLmNyZWF0ZUVycm9yQ29tcG9uZW50KHtcbiAgICAgICAgICAgICAgICB0aXRsZTogREVGQVVMVF9FUlJPUl9NRVNTQUdFUy5kZWZhdWx0RXJyb3JVbmtub3duLnRpdGxlLFxuICAgICAgICAgICAgICAgIGRldGFpbHM6IERFRkFVTFRfRVJST1JfTUVTU0FHRVMuZGVmYXVsdEVycm9yVW5rbm93bi5kZXRhaWxzLFxuICAgICAgICAgICAgICB9KTtcbiAgICAgICAgICAgIH1cbiAgICAgICAgICAgIGJyZWFrO1xuICAgICAgICAgIGRlZmF1bHQ6XG4gICAgICAgICAgICB0aGlzLnNob3dFcnJvcihERUZBVUxUX0VSUk9SX01FU1NBR0VTLmRlZmF1bHRFcnJvci5kZXRhaWxzLCBERUZBVUxUX0VSUk9SX01FU1NBR0VTLmRlZmF1bHRFcnJvci50aXRsZSk7XG4gICAgICAgICAgICBicmVhaztcbiAgICAgICAgfVxuICAgICAgfVxuICAgIH0pO1xuICB9XG5cbiAgcHJpdmF0ZSBzaG93RXJyb3IobWVzc2FnZT86IHN0cmluZywgdGl0bGU/OiBzdHJpbmcsIGJvZHk/OiBhbnkpOiBPYnNlcnZhYmxlPFRvYXN0ZXIuU3RhdHVzPiB7XG4gICAgaWYgKGJvZHkpIHtcbiAgICAgIGlmIChib2R5LmRldGFpbHMpIHtcbiAgICAgICAgbWVzc2FnZSA9IGJvZHkuZGV0YWlscztcbiAgICAgICAgdGl0bGUgPSBib2R5Lm1lc3NhZ2U7XG4gICAgICB9IGVsc2Uge1xuICAgICAgICBtZXNzYWdlID0gYm9keS5tZXNzYWdlIHx8IERFRkFVTFRfRVJST1JfTUVTU0FHRVMuZGVmYXVsdEVycm9yLnRpdGxlO1xuICAgICAgfVxuICAgIH1cblxuICAgIHJldHVybiB0aGlzLmNvbmZpcm1hdGlvblNlcnZpY2UuZXJyb3IobWVzc2FnZSwgdGl0bGUsIHtcbiAgICAgIGhpZGVDYW5jZWxCdG46IHRydWUsXG4gICAgICB5ZXNDb3B5OiAnT0snLFxuICAgIH0pO1xuICB9XG5cbiAgcHJpdmF0ZSBuYXZpZ2F0ZVRvTG9naW4oKSB7XG4gICAgdGhpcy5uZ1pvbmUucnVuKCgpID0+IHtcbiAgICAgIHRoaXMucm91dGVyLm5hdmlnYXRlKFsnL2FjY291bnQvbG9naW4nXSwge1xuICAgICAgICBzdGF0ZTogeyByZWRpcmVjdFVybDogdGhpcy5yb3V0ZXIudXJsIH0sXG4gICAgICB9KTtcbiAgICB9KTtcbiAgfVxuXG4gIGNyZWF0ZUVycm9yQ29tcG9uZW50KGluc3RhbmNlOiBQYXJ0aWFsPEVycm9yQ29tcG9uZW50Pikge1xuICAgIGNvbnN0IHJlbmRlcmVyID0gdGhpcy5yZW5kZXJlckZhY3RvcnkuY3JlYXRlUmVuZGVyZXIobnVsbCwgbnVsbCk7XG4gICAgY29uc3QgaG9zdCA9IHJlbmRlcmVyLnNlbGVjdFJvb3RFbGVtZW50KGRvY3VtZW50LmJvZHksIHRydWUpO1xuXG4gICAgY29uc3QgY29tcG9uZW50UmVmID0gdGhpcy5jZlJlcy5yZXNvbHZlQ29tcG9uZW50RmFjdG9yeShFcnJvckNvbXBvbmVudCkuY3JlYXRlKHRoaXMuaW5qZWN0b3IpO1xuXG4gICAgZm9yIChjb25zdCBrZXkgaW4gY29tcG9uZW50UmVmLmluc3RhbmNlKSB7XG4gICAgICBpZiAoY29tcG9uZW50UmVmLmluc3RhbmNlLmhhc093blByb3BlcnR5KGtleSkpIHtcbiAgICAgICAgY29tcG9uZW50UmVmLmluc3RhbmNlW2tleV0gPSBpbnN0YW5jZVtrZXldO1xuICAgICAgfVxuICAgIH1cblxuICAgIHRoaXMuYXBwUmVmLmF0dGFjaFZpZXcoY29tcG9uZW50UmVmLmhvc3RWaWV3KTtcbiAgICByZW5kZXJlci5hcHBlbmRDaGlsZChob3N0LCAoY29tcG9uZW50UmVmLmhvc3RWaWV3IGFzIEVtYmVkZGVkVmlld1JlZjxhbnk+KS5yb290Tm9kZXNbMF0pO1xuXG4gICAgY29tcG9uZW50UmVmLmluc3RhbmNlLnJlbmRlcmVyID0gcmVuZGVyZXI7XG4gICAgY29tcG9uZW50UmVmLmluc3RhbmNlLmVsZW1lbnRSZWYgPSBjb21wb25lbnRSZWYubG9jYXRpb247XG4gICAgY29tcG9uZW50UmVmLmluc3RhbmNlLmhvc3QgPSBob3N0O1xuICB9XG59XG4iXX0=