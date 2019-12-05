/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/application-configuration.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { RestService } from './rest.service';
import * as i0 from "@angular/core";
import * as i1 from "./rest.service";
export class ApplicationConfigurationService {
    /**
     * @param {?} rest
     */
    constructor(rest) {
        this.rest = rest;
    }
    /**
     * @return {?}
     */
    getConfiguration() {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: '/api/abp/application-configuration',
        };
        return this.rest.request(request);
    }
}
ApplicationConfigurationService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
ApplicationConfigurationService.ctorParameters = () => [
    { type: RestService }
];
/** @nocollapse */ ApplicationConfigurationService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ApplicationConfigurationService_Factory() { return new ApplicationConfigurationService(i0.ɵɵinject(i1.RestService)); }, token: ApplicationConfigurationService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    ApplicationConfigurationService.prototype.rest;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwbGljYXRpb24tY29uZmlndXJhdGlvbi5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL2FwcGxpY2F0aW9uLWNvbmZpZ3VyYXRpb24uc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFJM0MsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLGdCQUFnQixDQUFDOzs7QUFLN0MsTUFBTSxPQUFPLCtCQUErQjs7OztJQUMxQyxZQUFvQixJQUFpQjtRQUFqQixTQUFJLEdBQUosSUFBSSxDQUFhO0lBQUcsQ0FBQzs7OztJQUV6QyxnQkFBZ0I7O2NBQ1IsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSxvQ0FBb0M7U0FDMUM7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUEwQyxPQUFPLENBQUMsQ0FBQztJQUM3RSxDQUFDOzs7WUFiRixVQUFVLFNBQUM7Z0JBQ1YsVUFBVSxFQUFFLE1BQU07YUFDbkI7Ozs7WUFKUSxXQUFXOzs7Ozs7OztJQU1OLCtDQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IFJlc3QgfSBmcm9tICcuLi9tb2RlbHMvcmVzdCc7XG5pbXBvcnQgeyBBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24gfSBmcm9tICcuLi9tb2RlbHMvYXBwbGljYXRpb24tY29uZmlndXJhdGlvbic7XG5pbXBvcnQgeyBSZXN0U2VydmljZSB9IGZyb20gJy4vcmVzdC5zZXJ2aWNlJztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvblNlcnZpY2Uge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJlc3Q6IFJlc3RTZXJ2aWNlKSB7fVxuXG4gIGdldENvbmZpZ3VyYXRpb24oKTogT2JzZXJ2YWJsZTxBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24uUmVzcG9uc2U+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8bnVsbD4gPSB7XG4gICAgICBtZXRob2Q6ICdHRVQnLFxuICAgICAgdXJsOiAnL2FwaS9hYnAvYXBwbGljYXRpb24tY29uZmlndXJhdGlvbicsXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxudWxsLCBBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24uUmVzcG9uc2U+KHJlcXVlc3QpO1xuICB9XG59XG4iXX0=