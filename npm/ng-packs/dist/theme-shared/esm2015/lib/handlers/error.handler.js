/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { RestOccurError } from '@abp/ng.core';
import { HttpErrorResponse } from '@angular/common/http';
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZXJyb3IuaGFuZGxlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2hhbmRsZXJzL2Vycm9yLmhhbmRsZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDOUMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDekQsT0FBTyxFQUNMLGNBQWMsRUFDZCx3QkFBd0IsRUFFeEIsVUFBVSxFQUNWLFFBQVEsRUFDUixnQkFBZ0IsR0FDakIsTUFBTSxlQUFlLENBQUM7QUFDdkIsT0FBTyxFQUFFLFFBQVEsRUFBRSxXQUFXLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUM1RCxPQUFPLEVBQUUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUVqRSxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0NBQXNDLENBQUM7QUFFdEUsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0sa0NBQWtDLENBQUM7QUFDdkUsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDOzs7OztNQUVoQixRQUFRLEdBQUc7SUFDZixZQUFZLEVBQUU7UUFDWixPQUFPLEVBQUUsd0JBQXdCO1FBQ2pDLE9BQU8sRUFBRSxrQ0FBa0M7S0FDNUM7SUFFRCxlQUFlLEVBQUU7UUFDZixPQUFPLEVBQUUsNEJBQTRCO1FBQ3JDLE9BQU8sRUFBRSwyRUFBMkU7S0FDckY7SUFFRCxlQUFlLEVBQUU7UUFDZixPQUFPLEVBQUUseUJBQXlCO1FBQ2xDLE9BQU8sRUFBRSxnREFBZ0Q7S0FDMUQ7SUFFRCxlQUFlLEVBQUU7UUFDZixPQUFPLEVBQUUscUJBQXFCO1FBQzlCLE9BQU8sRUFBRSx1REFBdUQ7S0FDakU7Q0FDRjtBQUdELE1BQU0sT0FBTyxZQUFZOzs7Ozs7Ozs7O0lBQ3ZCLFlBQ1UsT0FBZ0IsRUFDaEIs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RUFBRTtZQUN2QyxJQUFJLFlBQVksQ0FBQyxRQUFRLENBQUMsY0FBYyxDQUFDLEdBQUcsQ0FBQyxFQUFFO2dCQUM3QyxZQUFZLENBQUMsUUFBUSxDQUFDLEdBQUcsQ0FBQyxHQUFHLFFBQVEsQ0FBQyxHQUFHLENBQUMsQ0FBQzthQUM1QztTQUNGO1FBRUQsSUFBSSxDQUFDLE1BQU0sQ0FBQyxVQUFVLENBQUMsWUFBWSxDQUFDLFFBQVEsQ0FBQyxDQUFDO1FBQzlDLFFBQVEsQ0FBQyxXQUFXLENBQUMsSUFBSSxFQUFFLENBQUMsbUJBQUEsWUFBWSxDQUFDLFFBQVEsRUFBd0IsQ0FBQyxDQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDO1FBRXpGLFlBQVksQ0FBQyxRQUFRLENBQUMsUUFBUSxHQUFHLFFBQVEsQ0FBQztRQUMxQyxZQUFZLENBQUMsUUFBUSxDQUFDLFVBQVUsR0FBRyxZQUFZLENBQUMsUUFBUSxDQUFDO1FBQ3pELFlBQVksQ0FBQyxRQUFRLENBQUMsSUFBSSxHQUFHLElBQUksQ0FBQztJQUNwQyxDQUFDOzs7WUF2R0YsVUFBVSxTQUFDLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRTs7OztZQTdCekIsT0FBTztZQUFzQixLQUFLO1lBSWxDLG1CQUFtQjtZQVoxQixjQUFjO1lBQ2Qsd0JBQXdCO1lBSXhCLGdCQUFnQjtZQURoQixRQUFROzs7Ozs7OztJQW9DTiwrQkFBd0I7Ozs7O0lBQ3hCLDZCQUFvQjs7Ozs7SUFDcEIsMkNBQWdEOzs7OztJQUNoRCw4QkFBOEI7Ozs7O0lBQzlCLDZCQUF1Qzs7Ozs7SUFDdkMsdUNBQXlDOzs7OztJQUN6QyxnQ0FBMEIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBSZXN0T2NjdXJFcnJvciB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBIdHRwRXJyb3JSZXNwb25zZSB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbi9odHRwJztcbmltcG9ydCB7XG4gIEFwcGxpY2F0aW9uUmVmLFxuICBDb21wb25lbnRGYWN0b3J5UmVzb2x2ZXIsXG4gIEVtYmVkZGVkVmlld1JlZixcbiAgSW5qZWN0YWJsZSxcbiAgSW5qZWN0b3IsXG4gIFJlbmRlcmVyRmFjdG9yeTIsXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmF2aWdhdGUsIFJvdXRlclN0YXRlIH0gZnJvbSAnQG5neHMvcm91dGVyLXBsdWdpbic7XG5pbXBvcnQgeyBBY3Rpb25zLCBvZkFjdGlvblN1Y2Nlc3NmdWwsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgRXJyb3JDb21wb25lbnQgfSBmcm9tICcuLi9jb21wb25lbnRzL2Vycm9ycy9lcnJvci5jb21wb25lbnQnO1xuaW1wb3J0IHsgVG9hc3RlciB9IGZyb20gJy4uL21vZGVscy90b2FzdGVyJztcbmltcG9ydCB7IENvbmZpcm1hdGlvblNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9jb25maXJtYXRpb24uc2VydmljZSc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5cbmNvbnN0IERFRkFVTFRTID0ge1xuICBkZWZhdWx0RXJyb3I6IHtcbiAgICBtZXNzYWdlOiAnQW4gZXJyb3IgaGFzIG9jY3VycmVkIScsXG4gICAgZGV0YWlsczogJ0Vycm9yIGRldGFpbCBub3Qgc2VudCBieSBzZXJ2ZXIuJyxcbiAgfSxcblxuICBkZWZhdWx0RXJyb3I0MDE6IHtcbiAgICBtZXNzYWdlOiAnWW91IGFyZSBub3QgYXV0aGVudGljYXRlZCEnLFxuICAgIGRldGFpbHM6ICdZb3Ugc2hvdWxkIGJlIGF1dGhlbnRpY2F0ZWQgKHNpZ24gaW4pIGluIG9yZGVyIHRvIHBlcmZvcm0gdGhpcyBvcGVyYXRpb24uJyxcbiAgfSxcblxuICBkZWZhdWx0RXJyb3I0MDM6IHtcbiAgICBtZXNzYWdlOiAnWW91IGFyZSBub3QgYXV0aG9yaXplZCEnLFxuICAgIGRldGFpbHM6ICdZb3UgYXJlIG5vdCBhbGxvd2VkIHRvIHBlcmZvcm0gdGhpcyBvcGVyYXRpb24uJyxcbiAgfSxcblxuICBkZWZhdWx0RXJyb3I0MDQ6IHtcbiAgICBtZXNzYWdlOiAnUmVzb3VyY2Ugbm90IGZvdW5kIScsXG4gICAgZGV0YWlsczogJ1RoZSByZXNvdXJjZSByZXF1ZXN0ZWQgY291bGQgbm90IGZvdW5kIG9uIHRoZSBzZXJ2ZXIuJyxcbiAgfSxcbn07XG5cbkBJbmplY3RhYmxlKHsgcHJvdmlkZWRJbjogJ3Jvb3QnIH0pXG5leHBvcnQgY2xhc3MgRXJyb3JIYW5kbGVyIHtcbiAgY29uc3RydWN0b3IoXG4gICAgcHJpdmF0ZSBhY3Rpb25zOiBBY3Rpb25zLFxuICAgIHByaXZhdGUgc3RvcmU6IFN0b3JlLFxuICAgIHByaXZhdGUgY29uZmlybWF0aW9uU2VydmljZTogQ29uZmlybWF0aW9uU2VydmljZSxcbiAgICBwcml2YXRlIGFwcFJlZjogQXBwbGljYXRpb25SZWYsXG4gICAgcHJpdmF0ZSBjZlJlczogQ29tcG9uZW50RmFjdG9yeVJlc29sdmVyLFxuICAgIHByaXZhdGUgcmVuZGVyZXJGYWN0b3J5OiBSZW5kZXJlckZhY3RvcnkyLFxuICAgIHByaXZhdGUgaW5qZWN0b3I6IEluamVjdG9yLFxuICApIHtcbiAgICBhY3Rpb25zLnBpcGUob2ZBY3Rpb25TdWNjZXNzZnVsKFJlc3RPY2N1ckVycm9yKSkuc3Vic2NyaWJlKHJlcyA9PiB7XG4gICAgICBjb25zdCB7IHBheWxvYWQ6IGVyciA9IHt9IGFzIEh0dHBFcnJvclJlc3BvbnNlIHwgYW55IH0gPSByZXM7XG4gICAgICBjb25zdCBib2R5ID0gc25xKCgpID0+IChlcnIgYXMgSHR0cEVycm9yUmVzcG9uc2UpLmVycm9yLmVycm9yLCBERUZBVUxUUy5kZWZhdWx0RXJyb3IubWVzc2FnZSk7XG5cbiAgICAgIGlmIChlcnIgaW5zdGFuY2VvZiBIdHRwRXJyb3JSZXNwb25zZSAmJiBlcnIuaGVhZGVycy5nZXQoJ19BYnBFcnJvckZvcm1hdCcpKSB7XG4gICAgICAgIGNvbnN0IGNvbmZpcm1hdGlvbiQgPSB0aGlzLnNob3dFcnJvcihudWxsLCBudWxsLCBib2R5KTtcblxuICAgICAgICBpZiAoZXJyLnN0YXR1cyA9PT0gNDAxKSB7XG4gICAgICAgICAgY29uZmlybWF0aW9uJC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICAgICAgdGhpcy5uYXZpZ2F0ZVRvTG9naW4oKTtcbiAgICAgICAgICB9KTtcbiAgICAgICAgfVxuICAgICAgfSBlbHNlIHtcbiAgICAgICAgc3dpdGNoICgoZXJyIGFzIEh0dHBFcnJvclJlc3BvbnNlKS5zdGF0dXMpIHtcbiAgICAgICAgICBjYXNlIDQwMTpcbiAgICAgICAgICAgIHRoaXMuc2hvd0Vycm9yKERFRkFVTFRTLmRlZmF1bHRFcnJvcjQwMS5kZXRhaWxzLCBERUZBVUxUUy5kZWZhdWx0RXJyb3I0MDEubWVzc2FnZSkuc3Vic2NyaWJlKCgpID0+XG4gICAgICAgICAgICAgIHRoaXMubmF2aWdhdGVUb0xvZ2luKCksXG4gICAgICAgICAgICApO1xuICAgICAgICAgICAgYnJlYWs7XG4gICAgICAgICAgY2FzZSA0MDM6XG4gICAgICAgICAgICB0aGlzLmNyZWF0ZUVycm9yQ29tcG9uZW50KHtcbiAgICAgICAgICAgICAgdGl0bGU6IERFRkFVTFRTLmRlZmF1bHRFcnJvcjQwMy5tZXNzYWdlLFxuICAgICAgICAgICAgICBkZXRhaWxzOiBERUZBVUxUUy5kZWZhdWx0RXJyb3I0MDMuZGV0YWlscyxcbiAgICAgICAgICAgIH0pO1xuICAgICAgICAgICAgYnJlYWs7XG4gICAgICAgICAgY2FzZSA0MDQ6XG4gICAgICAgICAgICB0aGlzLnNob3dFcnJvcihERUZBVUxUUy5kZWZhdWx0RXJyb3I0MDQuZGV0YWlscywgREVGQVVMVFMuZGVmYXVsdEVycm9yNDA0Lm1lc3NhZ2UpO1xuICAgICAgICAgICAgYnJlYWs7XG4gICAgICAgICAgY2FzZSA1MDA6XG4gICAgICAgICAgICB0aGlzLmNyZWF0ZUVycm9yQ29tcG9uZW50KHtcbiAgICAgICAgICAgICAgdGl0bGU6ICc1MDAnLFxuICAgICAgICAgICAgICBkZXRhaWxzOiAnQWJwQWNjb3VudDo6SW50ZXJuYWxTZXJ2ZXJFcnJvck1lc3NhZ2UnLFxuICAgICAgICAgICAgfSk7XG4gICAgICAgICAgICBicmVhaztcbiAgICAgICAgICBjYXNlIDA6XG4gICAgICAgICAgICBpZiAoKGVyciBhcyBIdHRwRXJyb3JSZXNwb25zZSkuc3RhdHVzVGV4dCA9PT0gJ1Vua25vd24gRXJyb3InKSB7XG4gICAgICAgICAgICAgIHRoaXMuY3JlYXRlRXJyb3JDb21wb25lbnQoe1xuICAgICAgICAgICAgICAgIHRpdGxlOiAnVW5rbm93biBFcnJvcicsXG4gICAgICAgICAgICAgICAgZGV0YWlsczogJ0FicEFjY291bnQ6OkludGVybmFsU2VydmVyRXJyb3JNZXNzYWdlJyxcbiAgICAgICAgICAgICAgfSk7XG4gICAgICAgICAgICB9XG4gICAgICAgICAgICBicmVhaztcbiAgICAgICAgICBkZWZhdWx0OlxuICAgICAgICAgICAgdGhpcy5zaG93RXJyb3IoREVGQVVMVFMuZGVmYXVsdEVycm9yLmRldGFpbHMsIERFRkFVTFRTLmRlZmF1bHRFcnJvci5tZXNzYWdlKTtcbiAgICAgICAgICAgIGJyZWFrO1xuICAgICAgICB9XG4gICAgICB9XG4gICAgfSk7XG4gIH1cblxuICBwcml2YXRlIHNob3dFcnJvcihtZXNzYWdlPzogc3RyaW5nLCB0aXRsZT86IHN0cmluZywgYm9keT86IGFueSk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcbiAgICBpZiAoYm9keSkge1xuICAgICAgaWYgKGJvZHkuZGV0YWlscykge1xuICAgICAgICBtZXNzYWdlID0gYm9keS5kZXRhaWxzO1xuICAgICAgICB0aXRsZSA9IGJvZHkubWVzc2FnZTtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIG1lc3NhZ2UgPSBib2R5Lm1lc3NhZ2UgfHwgREVGQVVMVFMuZGVmYXVsdEVycm9yLm1lc3NhZ2U7XG4gICAgICB9XG4gICAgfVxuXG4gICAgcmV0dXJuIHRoaXMuY29uZmlybWF0aW9uU2VydmljZS5lcnJvcihtZXNzYWdlLCB0aXRsZSwge1xuICAgICAgaGlkZUNhbmNlbEJ0bjogdHJ1ZSxcbiAgICAgIHllc0NvcHk6ICdPSycsXG4gICAgfSk7XG4gIH1cblxuICBwcml2YXRlIG5hdmlnYXRlVG9Mb2dpbigpIHtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKFxuICAgICAgbmV3IE5hdmlnYXRlKFsnL2FjY291bnQvbG9naW4nXSwgbnVsbCwge1xuICAgICAgICBzdGF0ZTogeyByZWRpcmVjdFVybDogdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChSb3V0ZXJTdGF0ZSkuc3RhdGUudXJsIH0sXG4gICAgICB9KSxcbiAgICApO1xuICB9XG5cbiAgY3JlYXRlRXJyb3JDb21wb25lbnQoaW5zdGFuY2U6IFBhcnRpYWw8RXJyb3JDb21wb25lbnQ+KSB7XG4gICAgY29uc3QgcmVuZGVyZXIgPSB0aGlzLnJlbmRlcmVyRmFjdG9yeS5jcmVhdGVSZW5kZXJlcihudWxsLCBudWxsKTtcbiAgICBjb25zdCBob3N0ID0gcmVuZGVyZXIuc2VsZWN0Um9vdEVsZW1lbnQoJ2FwcC1yb290JywgdHJ1ZSk7XG5cbiAgICBjb25zdCBjb21wb25lbnRSZWYgPSB0aGlzLmNmUmVzLnJlc29sdmVDb21wb25lbnRGYWN0b3J5KEVycm9yQ29tcG9uZW50KS5jcmVhdGUodGhpcy5pbmplY3Rvcik7XG5cbiAgICBmb3IgKGNvbnN0IGtleSBpbiBjb21wb25lbnRSZWYuaW5zdGFuY2UpIHtcbiAgICAgIGlmIChjb21wb25lbnRSZWYuaW5zdGFuY2UuaGFzT3duUHJvcGVydHkoa2V5KSkge1xuICAgICAgICBjb21wb25lbnRSZWYuaW5zdGFuY2Vba2V5XSA9IGluc3RhbmNlW2tleV07XG4gICAgICB9XG4gICAgfVxuXG4gICAgdGhpcy5hcHBSZWYuYXR0YWNoVmlldyhjb21wb25lbnRSZWYuaG9zdFZpZXcpO1xuICAgIHJlbmRlcmVyLmFwcGVuZENoaWxkKGhvc3QsIChjb21wb25lbnRSZWYuaG9zdFZpZXcgYXMgRW1iZWRkZWRWaWV3UmVmPGFueT4pLnJvb3ROb2Rlc1swXSk7XG5cbiAgICBjb21wb25lbnRSZWYuaW5zdGFuY2UucmVuZGVyZXIgPSByZW5kZXJlcjtcbiAgICBjb21wb25lbnRSZWYuaW5zdGFuY2UuZWxlbWVudFJlZiA9IGNvbXBvbmVudFJlZi5sb2NhdGlvbjtcbiAgICBjb21wb25lbnRSZWYuaW5zdGFuY2UuaG9zdCA9IGhvc3Q7XG4gIH1cbn1cbiJdfQ==