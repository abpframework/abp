/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { RestOccurError } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { Navigate, RouterState } from '@ngxs/router-plugin';
import { Actions, ofActionSuccessful, Store } from '@ngxs/store';
import { ConfirmationService } from '../services/confirmation.service';
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
     */
    constructor(actions, store, confirmationService) {
        this.actions = actions;
        this.store = store;
        this.confirmationService = confirmationService;
        actions.pipe(ofActionSuccessful(RestOccurError)).subscribe((/**
         * @param {?} res
         * @return {?}
         */
        res => {
            const { payload: err = (/** @type {?} */ ({})) } = res;
            /** @type {?} */
            const body = ((/** @type {?} */ (err))).error.error;
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
                        this.showError(DEFAULTS.defaultError403.details, DEFAULTS.defaultError403.message);
                        break;
                    case 404:
                        this.showError(DEFAULTS.defaultError404.details, DEFAULTS.defaultError404.message);
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
}
ErrorHandler.decorators = [
    { type: Injectable, args: [{ providedIn: 'root' },] }
];
/** @nocollapse */
ErrorHandler.ctorParameters = () => [
    { type: Actions },
    { type: Store },
    { type: ConfirmationService }
];
/** @nocollapse */ ErrorHandler.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ErrorHandler_Factory() { return new ErrorHandler(i0.ɵɵinject(i1.Actions), i0.ɵɵinject(i1.Store), i0.ɵɵinject(i2.ConfirmationService)); }, token: ErrorHandler, providedIn: "root" });
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
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZXJyb3IuaGFuZGxlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2hhbmRsZXJzL2Vycm9yLmhhbmRsZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFFOUMsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsUUFBUSxFQUFFLFdBQVcsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQzVELE9BQU8sRUFBRSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBR2pFLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLGtDQUFrQyxDQUFDOzs7OztNQUVqRSxRQUFRLEdBQUc7SUFDZixZQUFZLEVBQUU7UUFDWixPQUFPLEVBQUUsd0JBQXdCO1FBQ2pDLE9BQU8sRUFBRSxrQ0FBa0M7S0FDNUM7SUFFRCxlQUFlLEVBQUU7UUFDZixPQUFPLEVBQUUsNEJBQTRCO1FBQ3JDLE9BQU8sRUFBRSwyRUFBMkU7S0FDckY7SUFFRCxlQUFlLEVBQUU7UUFDZixPQUFPLEVBQUUseUJBQXlCO1FBQ2xDLE9BQU8sRUFBRSxnREFBZ0Q7S0FDMUQ7SUFFRCxlQUFlLEVBQUU7UUFDZixPQUFPLEVBQUUscUJBQXFCO1FBQzlCLE9BQU8sRUFBRSx1REFBdUQ7S0FDakU7Q0FDRjtBQUdELE1BQU0sT0FBTyxZQUFZOzs7Ozs7SUFDdkIsWUFBb0IsT0FBZ0IsRUFBVSxLQUFZLEVBQVUsbUJBQXdDO1FBQXhGLFlBQU8sR0FBUCxPQUFPLENBQVM7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQVUsd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQUMxRyxPQUFPLENBQUMsSUFBSSxDQUFDLGtCQUFrQixDQUFDLGNBQWMsQ0FBQyxDQUFDLENBQUMsU0FBUzs7OztRQUFD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Y2VzQ29udGVudCI6WyJpbXBvcnQgeyBSZXN0T2NjdXJFcnJvciB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBIdHRwRXJyb3JSZXNwb25zZSB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbi9odHRwJztcbmltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5hdmlnYXRlLCBSb3V0ZXJTdGF0ZSB9IGZyb20gJ0BuZ3hzL3JvdXRlci1wbHVnaW4nO1xuaW1wb3J0IHsgQWN0aW9ucywgb2ZBY3Rpb25TdWNjZXNzZnVsLCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IFRvYXN0ZXIgfSBmcm9tICcuLi9tb2RlbHMvdG9hc3Rlcic7XG5pbXBvcnQgeyBDb25maXJtYXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvY29uZmlybWF0aW9uLnNlcnZpY2UnO1xuXG5jb25zdCBERUZBVUxUUyA9IHtcbiAgZGVmYXVsdEVycm9yOiB7XG4gICAgbWVzc2FnZTogJ0FuIGVycm9yIGhhcyBvY2N1cnJlZCEnLFxuICAgIGRldGFpbHM6ICdFcnJvciBkZXRhaWwgbm90IHNlbnQgYnkgc2VydmVyLicsXG4gIH0sXG5cbiAgZGVmYXVsdEVycm9yNDAxOiB7XG4gICAgbWVzc2FnZTogJ1lvdSBhcmUgbm90IGF1dGhlbnRpY2F0ZWQhJyxcbiAgICBkZXRhaWxzOiAnWW91IHNob3VsZCBiZSBhdXRoZW50aWNhdGVkIChzaWduIGluKSBpbiBvcmRlciB0byBwZXJmb3JtIHRoaXMgb3BlcmF0aW9uLicsXG4gIH0sXG5cbiAgZGVmYXVsdEVycm9yNDAzOiB7XG4gICAgbWVzc2FnZTogJ1lvdSBhcmUgbm90IGF1dGhvcml6ZWQhJyxcbiAgICBkZXRhaWxzOiAnWW91IGFyZSBub3QgYWxsb3dlZCB0byBwZXJmb3JtIHRoaXMgb3BlcmF0aW9uLicsXG4gIH0sXG5cbiAgZGVmYXVsdEVycm9yNDA0OiB7XG4gICAgbWVzc2FnZTogJ1Jlc291cmNlIG5vdCBmb3VuZCEnLFxuICAgIGRldGFpbHM6ICdUaGUgcmVzb3VyY2UgcmVxdWVzdGVkIGNvdWxkIG5vdCBmb3VuZCBvbiB0aGUgc2VydmVyLicsXG4gIH0sXG59O1xuXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxuZXhwb3J0IGNsYXNzIEVycm9ySGFuZGxlciB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgYWN0aW9uczogQWN0aW9ucywgcHJpdmF0ZSBzdG9yZTogU3RvcmUsIHByaXZhdGUgY29uZmlybWF0aW9uU2VydmljZTogQ29uZmlybWF0aW9uU2VydmljZSkge1xuICAgIGFjdGlvbnMucGlwZShvZkFjdGlvblN1Y2Nlc3NmdWwoUmVzdE9jY3VyRXJyb3IpKS5zdWJzY3JpYmUocmVzID0+IHtcbiAgICAgIGNvbnN0IHsgcGF5bG9hZDogZXJyID0ge30gYXMgSHR0cEVycm9yUmVzcG9uc2UgfCBhbnkgfSA9IHJlcztcbiAgICAgIGNvbnN0IGJvZHkgPSAoZXJyIGFzIEh0dHBFcnJvclJlc3BvbnNlKS5lcnJvci5lcnJvcjtcblxuICAgICAgaWYgKGVyci5oZWFkZXJzLmdldCgnX0FicEVycm9yRm9ybWF0JykpIHtcbiAgICAgICAgY29uc3QgY29uZmlybWF0aW9uJCA9IHRoaXMuc2hvd0Vycm9yKG51bGwsIG51bGwsIGJvZHkpO1xuXG4gICAgICAgIGlmIChlcnIuc3RhdHVzID09PSA0MDEpIHtcbiAgICAgICAgICBjb25maXJtYXRpb24kLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgICAgICB0aGlzLm5hdmlnYXRlVG9Mb2dpbigpO1xuICAgICAgICAgIH0pO1xuICAgICAgICB9XG4gICAgICB9IGVsc2Uge1xuICAgICAgICBzd2l0Y2ggKChlcnIgYXMgSHR0cEVycm9yUmVzcG9uc2UpLnN0YXR1cykge1xuICAgICAgICAgIGNhc2UgNDAxOlxuICAgICAgICAgICAgdGhpcy5zaG93RXJyb3IoREVGQVVMVFMuZGVmYXVsdEVycm9yNDAxLmRldGFpbHMsIERFRkFVTFRTLmRlZmF1bHRFcnJvcjQwMS5tZXNzYWdlKS5zdWJzY3JpYmUoKCkgPT5cbiAgICAgICAgICAgICAgdGhpcy5uYXZpZ2F0ZVRvTG9naW4oKSxcbiAgICAgICAgICAgICk7XG4gICAgICAgICAgICBicmVhaztcbiAgICAgICAgICBjYXNlIDQwMzpcbiAgICAgICAgICAgIHRoaXMuc2hvd0Vycm9yKERFRkFVTFRTLmRlZmF1bHRFcnJvcjQwMy5kZXRhaWxzLCBERUZBVUxUUy5kZWZhdWx0RXJyb3I0MDMubWVzc2FnZSk7XG4gICAgICAgICAgICBicmVhaztcbiAgICAgICAgICBjYXNlIDQwNDpcbiAgICAgICAgICAgIHRoaXMuc2hvd0Vycm9yKERFRkFVTFRTLmRlZmF1bHRFcnJvcjQwNC5kZXRhaWxzLCBERUZBVUxUUy5kZWZhdWx0RXJyb3I0MDQubWVzc2FnZSk7XG4gICAgICAgICAgICBicmVhaztcbiAgICAgICAgICBkZWZhdWx0OlxuICAgICAgICAgICAgdGhpcy5zaG93RXJyb3IoREVGQVVMVFMuZGVmYXVsdEVycm9yLmRldGFpbHMsIERFRkFVTFRTLmRlZmF1bHRFcnJvci5tZXNzYWdlKTtcbiAgICAgICAgICAgIGJyZWFrO1xuICAgICAgICB9XG4gICAgICB9XG4gICAgfSk7XG4gIH1cblxuICBwcml2YXRlIHNob3dFcnJvcihtZXNzYWdlPzogc3RyaW5nLCB0aXRsZT86IHN0cmluZywgYm9keT86IGFueSk6IE9ic2VydmFibGU8VG9hc3Rlci5TdGF0dXM+IHtcbiAgICBpZiAoYm9keSkge1xuICAgICAgaWYgKGJvZHkuZGV0YWlscykge1xuICAgICAgICBtZXNzYWdlID0gYm9keS5kZXRhaWxzO1xuICAgICAgICB0aXRsZSA9IGJvZHkubWVzc2FnZTtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIG1lc3NhZ2UgPSBib2R5Lm1lc3NhZ2UgfHwgREVGQVVMVFMuZGVmYXVsdEVycm9yLm1lc3NhZ2U7XG4gICAgICB9XG4gICAgfVxuXG4gICAgcmV0dXJuIHRoaXMuY29uZmlybWF0aW9uU2VydmljZS5lcnJvcihtZXNzYWdlLCB0aXRsZSwge1xuICAgICAgaGlkZUNhbmNlbEJ0bjogdHJ1ZSxcbiAgICAgIHllc0NvcHk6ICdPSycsXG4gICAgfSk7XG4gIH1cblxuICBwcml2YXRlIG5hdmlnYXRlVG9Mb2dpbigpIHtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKFxuICAgICAgbmV3IE5hdmlnYXRlKFsnL2FjY291bnQvbG9naW4nXSwgbnVsbCwge1xuICAgICAgICBzdGF0ZTogeyByZWRpcmVjdFVybDogdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChSb3V0ZXJTdGF0ZSkuc3RhdGUudXJsIH0sXG4gICAgICB9KSxcbiAgICApO1xuICB9XG59XG4iXX0=