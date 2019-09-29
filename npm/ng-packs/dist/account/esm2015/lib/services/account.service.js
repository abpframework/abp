/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import * as i0 from "@angular/core";
import * as i1 from "@abp/ng.core";
export class AccountService {
    /**
     * @param {?} rest
     */
    constructor(rest) {
        this.rest = rest;
    }
    /**
     * @param {?} tenantName
     * @return {?}
     */
    findTenant(tenantName) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: `/api/abp/multi-tenancy/tenants/by-name/${tenantName}`,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} body
     * @return {?}
     */
    register(body) {
        /** @type {?} */
        const request = {
            method: 'POST',
            url: `/api/account/register`,
            body,
        };
        return this.rest.request(request, { skipHandleError: true });
    }
}
AccountService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
AccountService.ctorParameters = () => [
    { type: RestService }
];
/** @nocollapse */ AccountService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function AccountService_Factory() { return new AccountService(i0.ɵɵinject(i1.RestService)); }, token: AccountService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    AccountService.prototype.rest;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL2FjY291bnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsV0FBVyxFQUFRLE1BQU0sY0FBYyxDQUFDOzs7QUFNakQsTUFBTSxPQUFPLGNBQWM7Ozs7SUFDekIsWUFBb0IsSUFBaUI7UUFBakIsU0FBSSxHQUFKLElBQUksQ0FBYTtJQUFHLENBQUM7Ozs7O0lBRXpDLFVBQVUsQ0FBQyxVQUFrQjs7Y0FDckIsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSwwQ0FBMEMsVUFBVSxFQUFFO1NBQzVEO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBeUIsT0FBTyxDQUFDLENBQUM7SUFDNUQsQ0FBQzs7Ozs7SUFFRCxRQUFRLENBQUMsSUFBcUI7O2NBQ3RCLE9BQU8sR0FBa0M7WUFDN0MsTUFBTSxFQUFFLE1BQU07WUFDZCxHQUFHLEVBQUUsdUJBQXVCO1lBQzVCLElBQUk7U0FDTDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQW9DLE9BQU8sRUFBRSxFQUFFLGVBQWUsRUFBRSxJQUFJLEVBQUUsQ0FBQyxDQUFDO0lBQ2xHLENBQUM7OztZQXZCRixVQUFVLFNBQUM7Z0JBQ1YsVUFBVSxFQUFFLE1BQU07YUFDbkI7Ozs7WUFMUSxXQUFXOzs7Ozs7OztJQU9OLDhCQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IFJlc3RTZXJ2aWNlLCBSZXN0IH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IFJlZ2lzdGVyUmVzcG9uc2UsIFJlZ2lzdGVyUmVxdWVzdCwgVGVuYW50SWRSZXNwb25zZSB9IGZyb20gJy4uL21vZGVscyc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBBY2NvdW50U2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVzdDogUmVzdFNlcnZpY2UpIHt9XG5cbiAgZmluZFRlbmFudCh0ZW5hbnROYW1lOiBzdHJpbmcpOiBPYnNlcnZhYmxlPFRlbmFudElkUmVzcG9uc2U+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8bnVsbD4gPSB7XG4gICAgICBtZXRob2Q6ICdHRVQnLFxuICAgICAgdXJsOiBgL2FwaS9hYnAvbXVsdGktdGVuYW5jeS90ZW5hbnRzL2J5LW5hbWUvJHt0ZW5hbnROYW1lfWAsXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxudWxsLCBUZW5hbnRJZFJlc3BvbnNlPihyZXF1ZXN0KTtcbiAgfVxuXG4gIHJlZ2lzdGVyKGJvZHk6IFJlZ2lzdGVyUmVxdWVzdCk6IE9ic2VydmFibGU8UmVnaXN0ZXJSZXNwb25zZT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxSZWdpc3RlclJlcXVlc3Q+ID0ge1xuICAgICAgbWV0aG9kOiAnUE9TVCcsXG4gICAgICB1cmw6IGAvYXBpL2FjY291bnQvcmVnaXN0ZXJgLFxuICAgICAgYm9keSxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFJlZ2lzdGVyUmVxdWVzdCwgUmVnaXN0ZXJSZXNwb25zZT4ocmVxdWVzdCwgeyBza2lwSGFuZGxlRXJyb3I6IHRydWUgfSk7XG4gIH1cbn1cbiJdfQ==