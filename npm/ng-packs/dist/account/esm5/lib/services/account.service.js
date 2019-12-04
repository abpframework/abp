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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL2FjY291bnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFM0MsT0FBTyxFQUFFLFdBQVcsRUFBUSxNQUFNLGNBQWMsQ0FBQzs7O0FBR2pEO0lBSUUsd0JBQW9CLElBQWlCO1FBQWpCLFNBQUksR0FBSixJQUFJLENBQWE7SUFBRyxDQUFDOzs7OztJQUV6QyxtQ0FBVTs7OztJQUFWLFVBQVcsVUFBa0I7O1lBQ3JCLE9BQU8sR0FBdUI7WUFDbEMsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEVBQUUsNENBQTBDLFVBQVk7U0FDNUQ7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUF5QixPQUFPLENBQUMsQ0FBQztJQUM1RCxDQUFDOzs7OztJQUVELGlDQUFROzs7O0lBQVIsVUFBUyxJQUFxQjs7WUFDdEIsT0FBTyxHQUFrQztZQUM3QyxNQUFNLEVBQUUsTUFBTTtZQUNkLEdBQUcsRUFBRSx1QkFBdUI7WUFDNUIsSUFBSSxNQUFBO1NBQ0w7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFvQyxPQUFPLEVBQUUsRUFBRSxlQUFlLEVBQUUsSUFBSSxFQUFFLENBQUMsQ0FBQztJQUNsRyxDQUFDOztnQkF2QkYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFMUSxXQUFXOzs7eUJBRnBCO0NBNkJDLEFBeEJELElBd0JDO1NBckJZLGNBQWM7Ozs7OztJQUNiLDhCQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IFJlc3RTZXJ2aWNlLCBSZXN0IH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IFJlZ2lzdGVyUmVzcG9uc2UsIFJlZ2lzdGVyUmVxdWVzdCwgVGVuYW50SWRSZXNwb25zZSB9IGZyb20gJy4uL21vZGVscyc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBBY2NvdW50U2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVzdDogUmVzdFNlcnZpY2UpIHt9XG5cbiAgZmluZFRlbmFudCh0ZW5hbnROYW1lOiBzdHJpbmcpOiBPYnNlcnZhYmxlPFRlbmFudElkUmVzcG9uc2U+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8bnVsbD4gPSB7XG4gICAgICBtZXRob2Q6ICdHRVQnLFxuICAgICAgdXJsOiBgL2FwaS9hYnAvbXVsdGktdGVuYW5jeS90ZW5hbnRzL2J5LW5hbWUvJHt0ZW5hbnROYW1lfWAsXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxudWxsLCBUZW5hbnRJZFJlc3BvbnNlPihyZXF1ZXN0KTtcbiAgfVxuXG4gIHJlZ2lzdGVyKGJvZHk6IFJlZ2lzdGVyUmVxdWVzdCk6IE9ic2VydmFibGU8UmVnaXN0ZXJSZXNwb25zZT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxSZWdpc3RlclJlcXVlc3Q+ID0ge1xuICAgICAgbWV0aG9kOiAnUE9TVCcsXG4gICAgICB1cmw6ICcvYXBpL2FjY291bnQvcmVnaXN0ZXInLFxuICAgICAgYm9keSxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFJlZ2lzdGVyUmVxdWVzdCwgUmVnaXN0ZXJSZXNwb25zZT4ocmVxdWVzdCwgeyBza2lwSGFuZGxlRXJyb3I6IHRydWUgfSk7XG4gIH1cbn1cbiJdfQ==