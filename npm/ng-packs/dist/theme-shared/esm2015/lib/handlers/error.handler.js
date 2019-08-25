/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { RestOccurError } from '@abp/ng.core';
import { ApplicationRef, ComponentFactoryResolver, Injectable, Injector, RendererFactory2, } from '@angular/core';
import { Navigate, RouterState } from '@ngxs/router-plugin';
import { Actions, ofActionSuccessful, Store } from '@ngxs/store';
import { ErrorComponent } from '../components/errors/error.component';
import { ConfirmationService } from '../services/confirmation.service';
import snq from 'snq';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
import * as i2 from "../services/confirmation.service";
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
export class ErrorHandler {
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
/** @nocollapse */ ErrorHandler.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ErrorHandler_Factory() { return new ErrorHandler(i0.ɵɵinject(i1.Actions), i0.ɵɵinject(i1.Store), i0.ɵɵinject(i2.ConfirmationService), i0.ɵɵinject(i0.ApplicationRef), i0.ɵɵinject(i0.ComponentFactoryResolver), i0.ɵɵinject(i0.RendererFactory2), i0.ɵɵinject(i0.INJECTOR)); }, token: ErrorHandler, providedIn: "root" });
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZXJyb3IuaGFuZGxlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2hhbmRsZXJzL2Vycm9yLmhhbmRsZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFFOUMsT0FBTyxFQUNMLGNBQWMsRUFDZCx3QkFBd0IsRUFFeEIsVUFBVSxFQUNWLFFBQVEsRUFDUixnQkFBZ0IsR0FDakIsTUFBTSxlQUFlLENBQUM7QUFDdkIsT0FBTyxFQUFFLFFBQVEsRUFBRSxXQUFXLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUM1RCxPQUFPLEVBQUUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUVqRSxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0NBQXNDLENBQUM7QUFFdEUsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0sa0NBQWtDLENBQUM7QUFDdkUsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDOzs7OztNQUVoQixRQUFRLEdBQUc7SUFDZixZQUFZLEVBQUU7UUFDWixPQUFPLEVBQUUsd0JBQXdCO1FBQ2pDLE9BQU8sRUFBRSxrQ0FBa0M7S0FDNUM7SUFFRCxlQUFlLEVBQUU7UUFDZixPQUFPLEVBQUUsNEJBQTRCO1FBQ3JDLE9BQU8sRUFBRSwyRUFBMkU7S0FDckY7SUFFRCxlQUFlLEVBQUU7UUFDZixPQUFPLEVBQUUseUJBQXlCO1FBQ2xDLE9BQU8sRUFBRSxnREFBZ0Q7S0FDMUQ7SUFFRCxlQUFlLEVBQUU7UUFDZixPQUFPLEVBQUUscUJBQXFCO1FBQzlCLE9BQU8sRUFBRSx1REFBdUQ7S0FDakU7Q0FDRjtBQUdELE1BQU0sT0FBTyxZQUFZOzs7Ozs7Ozs7O0lBQ3ZCLFlBQ1UsT0FBZ0IsRUFDaEIsS0FBWSxFQUNaLG1CQUF3QyxFQUN4QyxNQUFzQixFQUN0QixLQUErQixFQUMv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LFlBQVksQ0FBQyxRQUFRLENBQUMsR0FBRyxDQUFDLEdBQUcsUUFBUSxDQUFDLEdBQUcsQ0FBQyxDQUFDO2FBQzVDO1NBQ0Y7UUFFRCxJQUFJLENBQUMsTUFBTSxDQUFDLFVBQVUsQ0FBQyxZQUFZLENBQUMsUUFBUSxDQUFDLENBQUM7UUFDOUMsUUFBUSxDQUFDLFdBQVcsQ0FBQyxJQUFJLEVBQUUsQ0FBQyxtQkFBQSxZQUFZLENBQUMsUUFBUSxFQUF3QixDQUFDLENBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7UUFFekYsWUFBWSxDQUFDLFFBQVEsQ0FBQyxRQUFRLEdBQUcsUUFBUSxDQUFDO1FBQzFDLFlBQVksQ0FBQyxRQUFRLENBQUMsVUFBVSxHQUFHLFlBQVksQ0FBQyxRQUFRLENBQUM7UUFDekQsWUFBWSxDQUFDLFFBQVEsQ0FBQyxJQUFJLEdBQUcsSUFBSSxDQUFDO0lBQ3BDLENBQUM7OztZQXZHRixVQUFVLFNBQUMsRUFBRSxVQUFVLEVBQUUsTUFBTSxFQUFFOzs7O1lBN0J6QixPQUFPO1lBQXNCLEtBQUs7WUFJbEMsbUJBQW1CO1lBWjFCLGNBQWM7WUFDZCx3QkFBd0I7WUFJeEIsZ0JBQWdCO1lBRGhCLFFBQVE7Ozs7Ozs7O0lBb0NOLCtCQUF3Qjs7Ozs7SUFDeEIsNkJBQW9COzs7OztJQUNwQiwyQ0FBZ0Q7Ozs7O0lBQ2hELDhCQUE4Qjs7Ozs7SUFDOUIsNkJBQXVDOzs7OztJQUN2Qyx1Q0FBeUM7Ozs7O0lBQ3pDLGdDQUEwQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFJlc3RPY2N1ckVycm9yIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IEh0dHBFcnJvclJlc3BvbnNlIH0gZnJvbSAnQGFuZ3VsYXIvY29tbW9uL2h0dHAnO1xuaW1wb3J0IHtcbiAgQXBwbGljYXRpb25SZWYsXG4gIENvbXBvbmVudEZhY3RvcnlSZXNvbHZlcixcbiAgRW1iZWRkZWRWaWV3UmVmLFxuICBJbmplY3RhYmxlLFxuICBJbmplY3RvcixcbiAgUmVuZGVyZXJGYWN0b3J5Mixcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBOYXZpZ2F0ZSwgUm91dGVyU3RhdGUgfSBmcm9tICdAbmd4cy9yb3V0ZXItcGx1Z2luJztcbmltcG9ydCB7IEFjdGlvbnMsIG9mQWN0aW9uU3VjY2Vzc2Z1bCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBFcnJvckNvbXBvbmVudCB9IGZyb20gJy4uL2NvbXBvbmVudHMvZXJyb3JzL2Vycm9yLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBUb2FzdGVyIH0gZnJvbSAnLi4vbW9kZWxzL3RvYXN0ZXInO1xuaW1wb3J0IHsgQ29uZmlybWF0aW9uU2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL2NvbmZpcm1hdGlvbi5zZXJ2aWNlJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcblxuY29uc3QgREVGQVVMVFMgPSB7XG4gIGRlZmF1bHRFcnJvcjoge1xuICAgIG1lc3NhZ2U6ICdBbiBlcnJvciBoYXMgb2NjdXJyZWQhJyxcbiAgICBkZXRhaWxzOiAnRXJyb3IgZGV0YWlsIG5vdCBzZW50IGJ5IHNlcnZlci4nLFxuICB9LFxuXG4gIGRlZmF1bHRFcnJvcjQwMToge1xuICAgIG1lc3NhZ2U6ICdZb3UgYXJlIG5vdCBhdXRoZW50aWNhdGVkIScsXG4gICAgZGV0YWlsczogJ1lvdSBzaG91bGQgYmUgYXV0aGVudGljYXRlZCAoc2lnbiBpbikgaW4gb3JkZXIgdG8gcGVyZm9ybSB0aGlzIG9wZXJhdGlvbi4nLFxuICB9LFxuXG4gIGRlZmF1bHRFcnJvcjQwMzoge1xuICAgIG1lc3NhZ2U6ICdZb3UgYXJlIG5vdCBhdXRob3JpemVkIScsXG4gICAgZGV0YWlsczogJ1lvdSBhcmUgbm90IGFsbG93ZWQgdG8gcGVyZm9ybSB0aGlzIG9wZXJhdGlvbi4nLFxuICB9LFxuXG4gIGRlZmF1bHRFcnJvcjQwNDoge1xuICAgIG1lc3NhZ2U6ICdSZXNvdXJjZSBub3QgZm91bmQhJyxcbiAgICBkZXRhaWxzOiAnVGhlIHJlc291cmNlIHJlcXVlc3RlZCBjb3VsZCBub3QgZm91bmQgb24gdGhlIHNlcnZlci4nLFxuICB9LFxufTtcblxuQEluamVjdGFibGUoeyBwcm92aWRlZEluOiAncm9vdCcgfSlcbmV4cG9ydCBjbGFzcyBFcnJvckhhbmRsZXIge1xuICBjb25zdHJ1Y3RvcihcbiAgICBwcml2YXRlIGFjdGlvbnM6IEFjdGlvbnMsXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXG4gICAgcHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlLFxuICAgIHByaXZhdGUgYXBwUmVmOiBBcHBsaWNhdGlvblJlZixcbiAgICBwcml2YXRlIGNmUmVzOiBDb21wb25lbnRGYWN0b3J5UmVzb2x2ZXIsXG4gICAgcHJpdmF0ZSByZW5kZXJlckZhY3Rvcnk6IFJlbmRlcmVyRmFjdG9yeTIsXG4gICAgcHJpdmF0ZSBpbmplY3RvcjogSW5qZWN0b3IsXG4gICkge1xuICAgIGFjdGlvbnMucGlwZShvZkFjdGlvblN1Y2Nlc3NmdWwoUmVzdE9jY3VyRXJyb3IpKS5zdWJzY3JpYmUocmVzID0+IHtcbiAgICAgIGNvbnN0IHsgcGF5bG9hZDogZXJyID0ge30gYXMgSHR0cEVycm9yUmVzcG9uc2UgfCBhbnkgfSA9IHJlcztcbiAgICAgIGNvbnN0IGJvZHkgPSBzbnEoKCkgPT4gKGVyciBhcyBIdHRwRXJyb3JSZXNwb25zZSkuZXJyb3IuZXJyb3IsIERFRkFVTFRTLmRlZmF1bHRFcnJvci5tZXNzYWdlKTtcblxuICAgICAgaWYgKGVyci5oZWFkZXJzLmdldCgnX0FicEVycm9yRm9ybWF0JykpIHtcbiAgICAgICAgY29uc3QgY29uZmlybWF0aW9uJCA9IHRoaXMuc2hvd0Vycm9yKG51bGwsIG51bGwsIGJvZHkpO1xuXG4gICAgICAgIGlmIChlcnIuc3RhdHVzID09PSA0MDEpIHtcbiAgICAgICAgICBjb25maXJtYXRpb24kLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgICAgICB0aGlzLm5hdmlnYXRlVG9Mb2dpbigpO1xuICAgICAgICAgIH0pO1xuICAgICAgICB9XG4gICAgICB9IGVsc2Uge1xuICAgICAgICBzd2l0Y2ggKChlcnIgYXMgSHR0cEVycm9yUmVzcG9uc2UpLnN0YXR1cykge1xuICAgICAgICAgIGNhc2UgNDAxOlxuICAgICAgICAgICAgdGhpcy5zaG93RXJyb3IoREVGQVVMVFMuZGVmYXVsdEVycm9yNDAxLmRldGFpbHMsIERFRkFVTFRTLmRlZmF1bHRFcnJvcjQwMS5tZXNzYWdlKS5zdWJzY3JpYmUoKCkgPT5cbiAgICAgICAgICAgICAgdGhpcy5uYXZpZ2F0ZVRvTG9naW4oKSxcbiAgICAgICAgICAgICk7XG4gICAgICAgICAgICBicmVhaztcbiAgICAgICAgICBjYXNlIDQwMzpcbiAgICAgICAgICAgIHRoaXMuY3JlYXRlRXJyb3JDb21wb25lbnQoe1xuICAgICAgICAgICAgICB0aXRsZTogREVGQVVMVFMuZGVmYXVsdEVycm9yNDAzLm1lc3NhZ2UsXG4gICAgICAgICAgICAgIGRldGFpbHM6IERFRkFVTFRTLmRlZmF1bHRFcnJvcjQwMy5kZXRhaWxzLFxuICAgICAgICAgICAgfSk7XG4gICAgICAgICAgICBicmVhaztcbiAgICAgICAgICBjYXNlIDQwNDpcbiAgICAgICAgICAgIHRoaXMuc2hvd0Vycm9yKERFRkFVTFRTLmRlZmF1bHRFcnJvcjQwNC5kZXRhaWxzLCBERUZBVUxUUy5kZWZhdWx0RXJyb3I0MDQubWVzc2FnZSk7XG4gICAgICAgICAgICBicmVhaztcbiAgICAgICAgICBjYXNlIDUwMDpcbiAgICAgICAgICAgIHRoaXMuY3JlYXRlRXJyb3JDb21wb25lbnQoe1xuICAgICAgICAgICAgICB0aXRsZTogJzUwMCcsXG4gICAgICAgICAgICAgIGRldGFpbHM6ICdBYnBBY2NvdW50OjpJbnRlcm5hbFNlcnZlckVycm9yTWVzc2FnZScsXG4gICAgICAgICAgICB9KTtcbiAgICAgICAgICAgIGJyZWFrO1xuICAgICAgICAgIGNhc2UgMDpcbiAgICAgICAgICAgIGlmICgoZXJyIGFzIEh0dHBFcnJvclJlc3BvbnNlKS5zdGF0dXNUZXh0ID09PSAnVW5rbm93biBFcnJvcicpIHtcbiAgICAgICAgICAgICAgdGhpcy5jcmVhdGVFcnJvckNvbXBvbmVudCh7XG4gICAgICAgICAgICAgICAgdGl0bGU6ICdVbmtub3duIEVycm9yJyxcbiAgICAgICAgICAgICAgICBkZXRhaWxzOiAnQWJwQWNjb3VudDo6SW50ZXJuYWxTZXJ2ZXJFcnJvck1lc3NhZ2UnLFxuICAgICAgICAgICAgICB9KTtcbiAgICAgICAgICAgIH1cbiAgICAgICAgICAgIGJyZWFrO1xuICAgICAgICAgIGRlZmF1bHQ6XG4gICAgICAgICAgICB0aGlzLnNob3dFcnJvcihERUZBVUxUUy5kZWZhdWx0RXJyb3IuZGV0YWlscywgREVGQVVMVFMuZGVmYXVsdEVycm9yLm1lc3NhZ2UpO1xuICAgICAgICAgICAgYnJlYWs7XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICB9KTtcbiAgfVxuXG4gIHByaXZhdGUgc2hvd0Vycm9yKG1lc3NhZ2U/OiBzdHJpbmcsIHRpdGxlPzogc3RyaW5nLCBib2R5PzogYW55KTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xuICAgIGlmIChib2R5KSB7XG4gICAgICBpZiAoYm9keS5kZXRhaWxzKSB7XG4gICAgICAgIG1lc3NhZ2UgPSBib2R5LmRldGFpbHM7XG4gICAgICAgIHRpdGxlID0gYm9keS5tZXNzYWdlO1xuICAgICAgfSBlbHNlIHtcbiAgICAgICAgbWVzc2FnZSA9IGJvZHkubWVzc2FnZSB8fCBERUZBVUxUUy5kZWZhdWx0RXJyb3IubWVzc2FnZTtcbiAgICAgIH1cbiAgICB9XG5cbiAgICByZXR1cm4gdGhpcy5jb25maXJtYXRpb25TZXJ2aWNlLmVycm9yKG1lc3NhZ2UsIHRpdGxlLCB7XG4gICAgICBoaWRlQ2FuY2VsQnRuOiB0cnVlLFxuICAgICAgeWVzQ29weTogJ09LJyxcbiAgICB9KTtcbiAgfVxuXG4gIHByaXZhdGUgbmF2aWdhdGVUb0xvZ2luKCkge1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2goXG4gICAgICBuZXcgTmF2aWdhdGUoWycvYWNjb3VudC9sb2dpbiddLCBudWxsLCB7XG4gICAgICAgIHN0YXRlOiB7IHJlZGlyZWN0VXJsOiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFJvdXRlclN0YXRlKS5zdGF0ZS51cmwgfSxcbiAgICAgIH0pLFxuICAgICk7XG4gIH1cblxuICBjcmVhdGVFcnJvckNvbXBvbmVudChpbnN0YW5jZTogUGFydGlhbDxFcnJvckNvbXBvbmVudD4pIHtcbiAgICBjb25zdCByZW5kZXJlciA9IHRoaXMucmVuZGVyZXJGYWN0b3J5LmNyZWF0ZVJlbmRlcmVyKG51bGwsIG51bGwpO1xuICAgIGNvbnN0IGhvc3QgPSByZW5kZXJlci5zZWxlY3RSb290RWxlbWVudCgnYXBwLXJvb3QnLCB0cnVlKTtcblxuICAgIGNvbnN0IGNvbXBvbmVudFJlZiA9IHRoaXMuY2ZSZXMucmVzb2x2ZUNvbXBvbmVudEZhY3RvcnkoRXJyb3JDb21wb25lbnQpLmNyZWF0ZSh0aGlzLmluamVjdG9yKTtcblxuICAgIGZvciAoY29uc3Qga2V5IGluIGNvbXBvbmVudFJlZi5pbnN0YW5jZSkge1xuICAgICAgaWYgKGNvbXBvbmVudFJlZi5pbnN0YW5jZS5oYXNPd25Qcm9wZXJ0eShrZXkpKSB7XG4gICAgICAgIGNvbXBvbmVudFJlZi5pbnN0YW5jZVtrZXldID0gaW5zdGFuY2Vba2V5XTtcbiAgICAgIH1cbiAgICB9XG5cbiAgICB0aGlzLmFwcFJlZi5hdHRhY2hWaWV3KGNvbXBvbmVudFJlZi5ob3N0Vmlldyk7XG4gICAgcmVuZGVyZXIuYXBwZW5kQ2hpbGQoaG9zdCwgKGNvbXBvbmVudFJlZi5ob3N0VmlldyBhcyBFbWJlZGRlZFZpZXdSZWY8YW55Pikucm9vdE5vZGVzWzBdKTtcblxuICAgIGNvbXBvbmVudFJlZi5pbnN0YW5jZS5yZW5kZXJlciA9IHJlbmRlcmVyO1xuICAgIGNvbXBvbmVudFJlZi5pbnN0YW5jZS5lbGVtZW50UmVmID0gY29tcG9uZW50UmVmLmxvY2F0aW9uO1xuICAgIGNvbXBvbmVudFJlZi5pbnN0YW5jZS5ob3N0ID0gaG9zdDtcbiAgfVxufVxuIl19