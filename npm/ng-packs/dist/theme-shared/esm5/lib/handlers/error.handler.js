/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { RestOccurError } from '@abp/ng.core';
import { Injectable, ApplicationRef, ComponentFactoryResolver, RendererFactory2, Injector, } from '@angular/core';
import { Navigate, RouterState } from '@ngxs/router-plugin';
import { Actions, ofActionSuccessful, Store } from '@ngxs/store';
import { ConfirmationService } from '../services/confirmation.service';
import { Error500Component } from '../components/errors/error-500.component';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
import * as i2 from "../services/confirmation.service";
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
                        _this.showError(DEFAULTS.defaultError403.details, DEFAULTS.defaultError403.message);
                        break;
                    case 404:
                        _this.showError(DEFAULTS.defaultError404.details, DEFAULTS.defaultError404.message);
                        break;
                    case 500:
                        _this.show500Component();
                        break;
                    case 0:
                        if (((/** @type {?} */ (err))).statusText === 'Unknown Error') {
                            _this.show500Component();
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
     * @private
     * @return {?}
     */
    ErrorHandler.prototype.show500Component = /**
     * @private
     * @return {?}
     */
    function () {
        /** @type {?} */
        var renderer = this.rendererFactory.createRenderer(null, null);
        /** @type {?} */
        var host = renderer.selectRootElement('app-root', true);
        /** @type {?} */
        var componentRef = this.cfRes.resolveComponentFactory(Error500Component).create(this.injector);
        this.appRef.attachView(componentRef.hostView);
        renderer.appendChild(host, ((/** @type {?} */ (componentRef.hostView))).rootNodes[0]);
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
    /** @nocollapse */ ErrorHandler.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ErrorHandler_Factory() { return new ErrorHandler(i0.ɵɵinject(i1.Actions), i0.ɵɵinject(i1.Store), i0.ɵɵinject(i2.ConfirmationService), i0.ɵɵinject(i0.ApplicationRef), i0.ɵɵinject(i0.ComponentFactoryResolver), i0.ɵɵinject(i0.RendererFactory2), i0.ɵɵinject(i0.INJECTOR)); }, token: ErrorHandler, providedIn: "root" });
    return ErrorHandler;
}());
export { ErrorHandler };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZXJyb3IuaGFuZGxlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2hhbmRsZXJzL2Vycm9yLmhhbmRsZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFFOUMsT0FBTyxFQUNMLFVBQVUsRUFDVixjQUFjLEVBQ2Qsd0JBQXdCLEVBQ3hCLGdCQUFnQixFQUdoQixRQUFRLEdBRVQsTUFBTSxlQUFlLENBQUM7QUFDdkIsT0FBTyxFQUFFLFFBQVEsRUFBRSxXQUFXLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUM1RCxPQUFPLEVBQUUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUdqRSxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSxrQ0FBa0MsQ0FBQztBQUV2RSxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSwwQ0FBMEMsQ0FBQzs7Ozs7SUFFdkUsUUFBUSxHQUFHO0lBQ2YsWUFBWSxFQUFFO1FBQ1osT0FBTyxFQUFFLHdCQUF3QjtRQUNqQyxPQUFPLEVBQUUsa0NBQWtDO0tBQzVDO0lBRUQsZUFBZSxFQUFFO1FBQ2YsT0FBTyxFQUFFLDRCQUE0QjtRQUNyQyxPQUFPLEVBQUUsMkVBQTJFO0tBQ3JGO0lBRUQsZUFBZSxFQUFFO1FBQ2YsT0FBTyxFQUFFLHlCQUF5QjtRQUNsQyxPQUFPLEVBQUUsZ0RBQWdEO0tBQzFEO0lBRUQsZUFBZSxFQUFFO1FBQ2YsT0FBTyxFQUFFLHFCQUFxQjtRQUM5QixPQUFPLEVBQUUsdURBQXVEO0tBQ2pFO0NBQ0Y7QUFFRDtJQUVFLHNCQUNVLE9BQWdCLEVBQ2hCLEtBQVksRUFDWixtQkFBd0MsRUFDeEMsTUFBc0IsRUFDdEIsS0FBK0IsRUFDL0IsZUFBaUMsRUFDakMsUUFBa0I7UUFQNUIsaUJBZ0RDO1FBL0NTLFlBQU8sR0FBUCxPQUFP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Q3hCLDZCQUFvQjs7Ozs7SUFDcEIsMkNBQWdEOzs7OztJQUNoRCw4QkFBOEI7Ozs7O0lBQzlCLDZCQUF1Qzs7Ozs7SUFDdkMsdUNBQXlDOzs7OztJQUN6QyxnQ0FBMEIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBSZXN0T2NjdXJFcnJvciB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBIdHRwRXJyb3JSZXNwb25zZSB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbi9odHRwJztcbmltcG9ydCB7XG4gIEluamVjdGFibGUsXG4gIEFwcGxpY2F0aW9uUmVmLFxuICBDb21wb25lbnRGYWN0b3J5UmVzb2x2ZXIsXG4gIFJlbmRlcmVyRmFjdG9yeTIsXG4gIEluamVjdCxcbiAgUmVmbGVjdGl2ZUluamVjdG9yLFxuICBJbmplY3RvcixcbiAgRW1iZWRkZWRWaWV3UmVmLFxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5hdmlnYXRlLCBSb3V0ZXJTdGF0ZSB9IGZyb20gJ0BuZ3hzL3JvdXRlci1wbHVnaW4nO1xuaW1wb3J0IHsgQWN0aW9ucywgb2ZBY3Rpb25TdWNjZXNzZnVsLCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IFRvYXN0ZXIgfSBmcm9tICcuLi9tb2RlbHMvdG9hc3Rlcic7XG5pbXBvcnQgeyBDb25maXJtYXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvY29uZmlybWF0aW9uLnNlcnZpY2UnO1xuaW1wb3J0IHsgRE9DVU1FTlQgfSBmcm9tICdAYW5ndWxhci9jb21tb24nO1xuaW1wb3J0IHsgRXJyb3I1MDBDb21wb25lbnQgfSBmcm9tICcuLi9jb21wb25lbnRzL2Vycm9ycy9lcnJvci01MDAuY29tcG9uZW50JztcblxuY29uc3QgREVGQVVMVFMgPSB7XG4gIGRlZmF1bHRFcnJvcjoge1xuICAgIG1lc3NhZ2U6ICdBbiBlcnJvciBoYXMgb2NjdXJyZWQhJyxcbiAgICBkZXRhaWxzOiAnRXJyb3IgZGV0YWlsIG5vdCBzZW50IGJ5IHNlcnZlci4nLFxuICB9LFxuXG4gIGRlZmF1bHRFcnJvcjQwMToge1xuICAgIG1lc3NhZ2U6ICdZb3UgYXJlIG5vdCBhdXRoZW50aWNhdGVkIScsXG4gICAgZGV0YWlsczogJ1lvdSBzaG91bGQgYmUgYXV0aGVudGljYXRlZCAoc2lnbiBpbikgaW4gb3JkZXIgdG8gcGVyZm9ybSB0aGlzIG9wZXJhdGlvbi4nLFxuICB9LFxuXG4gIGRlZmF1bHRFcnJvcjQwMzoge1xuICAgIG1lc3NhZ2U6ICdZb3UgYXJlIG5vdCBhdXRob3JpemVkIScsXG4gICAgZGV0YWlsczogJ1lvdSBhcmUgbm90IGFsbG93ZWQgdG8gcGVyZm9ybSB0aGlzIG9wZXJhdGlvbi4nLFxuICB9LFxuXG4gIGRlZmF1bHRFcnJvcjQwNDoge1xuICAgIG1lc3NhZ2U6ICdSZXNvdXJjZSBub3QgZm91bmQhJyxcbiAgICBkZXRhaWxzOiAnVGhlIHJlc291cmNlIHJlcXVlc3RlZCBjb3VsZCBub3QgZm91bmQgb24gdGhlIHNlcnZlci4nLFxuICB9LFxufTtcblxuQEluamVjdGFibGUoeyBwcm92aWRlZEluOiAncm9vdCcgfSlcbmV4cG9ydCBjbGFzcyBFcnJvckhhbmRsZXIge1xuICBjb25zdHJ1Y3RvcihcbiAgICBwcml2YXRlIGFjdGlvbnM6IEFjdGlvbnMsXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXG4gICAgcHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlLFxuICAgIHByaXZhdGUgYXBwUmVmOiBBcHBsaWNhdGlvblJlZixcbiAgICBwcml2YXRlIGNmUmVzOiBDb21wb25lbnRGYWN0b3J5UmVzb2x2ZXIsXG4gICAgcHJpdmF0ZSByZW5kZXJlckZhY3Rvcnk6IFJlbmRlcmVyRmFjdG9yeTIsXG4gICAgcHJpdmF0ZSBpbmplY3RvcjogSW5qZWN0b3IsXG4gICkge1xuICAgIGFjdGlvbnMucGlwZShvZkFjdGlvblN1Y2Nlc3NmdWwoUmVzdE9jY3VyRXJyb3IpKS5zdWJzY3JpYmUocmVzID0+IHtcbiAgICAgIGNvbnN0IHsgcGF5bG9hZDogZXJyID0ge30gYXMgSHR0cEVycm9yUmVzcG9uc2UgfCBhbnkgfSA9IHJlcztcbiAgICAgIGNvbnN0IGJvZHkgPSAoZXJyIGFzIEh0dHBFcnJvclJlc3BvbnNlKS5lcnJvci5lcnJvcjtcblxuICAgICAgaWYgKGVyci5oZWFkZXJzLmdldCgnX0FicEVycm9yRm9ybWF0JykpIHtcbiAgICAgICAgY29uc3QgY29uZmlybWF0aW9uJCA9IHRoaXMuc2hvd0Vycm9yKG51bGwsIG51bGwsIGJvZHkpO1xuXG4gICAgICAgIGlmIChlcnIuc3RhdHVzID09PSA0MDEpIHtcbiAgICAgICAgICBjb25maXJtYXRpb24kLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgICAgICB0aGlzLm5hdmlnYXRlVG9Mb2dpbigpO1xuICAgICAgICAgIH0pO1xuICAgICAgICB9XG4gICAgICB9IGVsc2Uge1xuICAgICAgICBzd2l0Y2ggKChlcnIgYXMgSHR0cEVycm9yUmVzcG9uc2UpLnN0YXR1cykge1xuICAgICAgICAgIGNhc2UgNDAxOlxuICAgICAgICAgICAgdGhpcy5zaG93RXJyb3IoREVGQVVMVFMuZGVmYXVsdEVycm9yNDAxLmRldGFpbHMsIERFRkFVTFRTLmRlZmF1bHRFcnJvcjQwMS5tZXNzYWdlKS5zdWJzY3JpYmUoKCkgPT5cbiAgICAgICAgICAgICAgdGhpcy5uYXZpZ2F0ZVRvTG9naW4oKSxcbiAgICAgICAgICAgICk7XG4gICAgICAgICAgICBicmVhaztcbiAgICAgICAgICBjYXNlIDQwMzpcbiAgICAgICAgICAgIHRoaXMuc2hvd0Vycm9yKERFRkFVTFRTLmRlZmF1bHRFcnJvcjQwMy5kZXRhaWxzLCBERUZBVUxUUy5kZWZhdWx0RXJyb3I0MDMubWVzc2FnZSk7XG4gICAgICAgICAgICBicmVhaztcbiAgICAgICAgICBjYXNlIDQwNDpcbiAgICAgICAgICAgIHRoaXMuc2hvd0Vycm9yKERFRkFVTFRTLmRlZmF1bHRFcnJvcjQwNC5kZXRhaWxzLCBERUZBVUxUUy5kZWZhdWx0RXJyb3I0MDQubWVzc2FnZSk7XG4gICAgICAgICAgICBicmVhaztcbiAgICAgICAgICBjYXNlIDUwMDpcbiAgICAgICAgICAgIHRoaXMuc2hvdzUwMENvbXBvbmVudCgpO1xuICAgICAgICAgICAgYnJlYWs7XG4gICAgICAgICAgY2FzZSAwOlxuICAgICAgICAgICAgaWYgKChlcnIgYXMgSHR0cEVycm9yUmVzcG9uc2UpLnN0YXR1c1RleHQgPT09ICdVbmtub3duIEVycm9yJykge1xuICAgICAgICAgICAgICB0aGlzLnNob3c1MDBDb21wb25lbnQoKTtcbiAgICAgICAgICAgIH1cbiAgICAgICAgICAgIGJyZWFrO1xuICAgICAgICAgIGRlZmF1bHQ6XG4gICAgICAgICAgICB0aGlzLnNob3dFcnJvcihERUZBVUxUUy5kZWZhdWx0RXJyb3IuZGV0YWlscywgREVGQVVMVFMuZGVmYXVsdEVycm9yLm1lc3NhZ2UpO1xuICAgICAgICAgICAgYnJlYWs7XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICB9KTtcbiAgfVxuXG4gIHByaXZhdGUgc2hvd0Vycm9yKG1lc3NhZ2U/OiBzdHJpbmcsIHRpdGxlPzogc3RyaW5nLCBib2R5PzogYW55KTogT2JzZXJ2YWJsZTxUb2FzdGVyLlN0YXR1cz4ge1xuICAgIGlmIChib2R5KSB7XG4gICAgICBpZiAoYm9keS5kZXRhaWxzKSB7XG4gICAgICAgIG1lc3NhZ2UgPSBib2R5LmRldGFpbHM7XG4gICAgICAgIHRpdGxlID0gYm9keS5tZXNzYWdlO1xuICAgICAgfSBlbHNlIHtcbiAgICAgICAgbWVzc2FnZSA9IGJvZHkubWVzc2FnZSB8fCBERUZBVUxUUy5kZWZhdWx0RXJyb3IubWVzc2FnZTtcbiAgICAgIH1cbiAgICB9XG5cbiAgICByZXR1cm4gdGhpcy5jb25maXJtYXRpb25TZXJ2aWNlLmVycm9yKG1lc3NhZ2UsIHRpdGxlLCB7XG4gICAgICBoaWRlQ2FuY2VsQnRuOiB0cnVlLFxuICAgICAgeWVzQ29weTogJ09LJyxcbiAgICB9KTtcbiAgfVxuXG4gIHByaXZhdGUgbmF2aWdhdGVUb0xvZ2luKCkge1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2goXG4gICAgICBuZXcgTmF2aWdhdGUoWycvYWNjb3VudC9sb2dpbiddLCBudWxsLCB7XG4gICAgICAgIHN0YXRlOiB7IHJlZGlyZWN0VXJsOiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFJvdXRlclN0YXRlKS5zdGF0ZS51cmwgfSxcbiAgICAgIH0pLFxuICAgICk7XG4gIH1cblxuICBwcml2YXRlIHNob3c1MDBDb21wb25lbnQoKSB7XG4gICAgY29uc3QgcmVuZGVyZXIgPSB0aGlzLnJlbmRlcmVyRmFjdG9yeS5jcmVhdGVSZW5kZXJlcihudWxsLCBudWxsKTtcbiAgICBjb25zdCBob3N0ID0gcmVuZGVyZXIuc2VsZWN0Um9vdEVsZW1lbnQoJ2FwcC1yb290JywgdHJ1ZSk7XG4gICAgY29uc3QgY29tcG9uZW50UmVmID0gdGhpcy5jZlJlcy5yZXNvbHZlQ29tcG9uZW50RmFjdG9yeShFcnJvcjUwMENvbXBvbmVudCkuY3JlYXRlKHRoaXMuaW5qZWN0b3IpO1xuICAgIHRoaXMuYXBwUmVmLmF0dGFjaFZpZXcoY29tcG9uZW50UmVmLmhvc3RWaWV3KTtcbiAgICByZW5kZXJlci5hcHBlbmRDaGlsZChob3N0LCAoY29tcG9uZW50UmVmLmhvc3RWaWV3IGFzIEVtYmVkZGVkVmlld1JlZjxhbnk+KS5yb290Tm9kZXNbMF0pO1xuICB9XG59XG4iXX0=