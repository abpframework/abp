/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/account.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import * as i0 from "@angular/core";
import * as i1 from "@abp/ng.core";
var AccountService = /** @class */ (function () {
    function AccountService(rest) {
        this.rest = rest;
    }
    /**
     * @param {?} tenantName
     * @return {?}
     */
    AccountService.prototype.findTenant = /**
     * @param {?} tenantName
     * @return {?}
     */
    function (tenantName) {
        /** @type {?} */
        var request = {
            method: 'GET',
            url: "/api/abp/multi-tenancy/tenants/by-name/" + tenantName,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} body
     * @return {?}
     */
    AccountService.prototype.register = /**
     * @param {?} body
     * @return {?}
     */
    function (body) {
        /** @type {?} */
        var request = {
            method: 'POST',
            url: '/api/account/register',
            body: body,
        };
        return this.rest.request(request, { skipHandleError: true });
    };
    AccountService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    AccountService.ctorParameters = function () { return [
        { type: RestService }
    ]; };
    /** @nocollapse */ AccountService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function AccountService_Factory() { return new AccountService(i0.ɵɵinject(i1.RestService)); }, token: AccountService, providedIn: "root" });
    return AccountService;
}());
export { AccountService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    AccountService.prototype.rest;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL2FjY291bnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFM0MsT0FBTyxFQUFFLFdBQVcsRUFBUSxNQUFNLGNBQWMsQ0FBQzs7O0FBR2pEO0lBSUUsd0JBQW9CLElBQWlCO1FBQWpCLFNBQUksR0FBSixJQUFJLENBQWE7SUFBRyxDQUFDOzs7OztJQUV6QyxtQ0FBVTs7OztJQUFWLFVBQVcsVUFBa0I7O1lBQ3JCLE9BQU8sR0FBdUI7WUFDbEMsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEVBQUUsNENBQTBDLFVBQVk7U0FDNUQ7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUF5QixPQUFPLENBQUMsQ0FBQztJQUM1RCxDQUFDOzs7OztJQUVELGlDQUFROzs7O0lBQVIsVUFBUyxJQUFxQjs7WUFDdEIsT0FBTyxHQUFrQztZQUM3QyxNQUFNLEVBQUUsTUFBTTtZQUNkLEdBQUcsRUFBRSx1QkFBdUI7WUFDNUIsSUFBSSxNQUFBO1NBQ0w7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFvQyxPQUFPLEVBQUUsRUFBRSxlQUFlLEVBQUUsSUFBSSxFQUFFLENBQUMsQ0FBQztJQUNsRyxDQUFDOztnQkF2QkYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFMUSxXQUFXOzs7eUJBRnBCO0NBNkJDLEFBeEJELElBd0JDO1NBckJZLGNBQWM7Ozs7OztJQUNiLDhCQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xyXG5pbXBvcnQgeyBSZXN0U2VydmljZSwgUmVzdCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcbmltcG9ydCB7IFJlZ2lzdGVyUmVzcG9uc2UsIFJlZ2lzdGVyUmVxdWVzdCwgVGVuYW50SWRSZXNwb25zZSB9IGZyb20gJy4uL21vZGVscyc7XHJcblxyXG5ASW5qZWN0YWJsZSh7XHJcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgQWNjb3VudFNlcnZpY2Uge1xyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVzdDogUmVzdFNlcnZpY2UpIHt9XHJcblxyXG4gIGZpbmRUZW5hbnQodGVuYW50TmFtZTogc3RyaW5nKTogT2JzZXJ2YWJsZTxUZW5hbnRJZFJlc3BvbnNlPiB7XHJcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8bnVsbD4gPSB7XHJcbiAgICAgIG1ldGhvZDogJ0dFVCcsXHJcbiAgICAgIHVybDogYC9hcGkvYWJwL211bHRpLXRlbmFuY3kvdGVuYW50cy9ieS1uYW1lLyR7dGVuYW50TmFtZX1gLFxyXG4gICAgfTtcclxuXHJcbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8bnVsbCwgVGVuYW50SWRSZXNwb25zZT4ocmVxdWVzdCk7XHJcbiAgfVxyXG5cclxuICByZWdpc3Rlcihib2R5OiBSZWdpc3RlclJlcXVlc3QpOiBPYnNlcnZhYmxlPFJlZ2lzdGVyUmVzcG9uc2U+IHtcclxuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxSZWdpc3RlclJlcXVlc3Q+ID0ge1xyXG4gICAgICBtZXRob2Q6ICdQT1NUJyxcclxuICAgICAgdXJsOiAnL2FwaS9hY2NvdW50L3JlZ2lzdGVyJyxcclxuICAgICAgYm9keSxcclxuICAgIH07XHJcblxyXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFJlZ2lzdGVyUmVxdWVzdCwgUmVnaXN0ZXJSZXNwb25zZT4ocmVxdWVzdCwgeyBza2lwSGFuZGxlRXJyb3I6IHRydWUgfSk7XHJcbiAgfVxyXG59XHJcbiJdfQ==