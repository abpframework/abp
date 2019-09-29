/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
            url: "/api/account/register",
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL2FjY291bnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsV0FBVyxFQUFRLE1BQU0sY0FBYyxDQUFDOzs7QUFHakQ7SUFJRSx3QkFBb0IsSUFBaUI7UUFBakIsU0FBSSxHQUFKLElBQUksQ0FBYTtJQUFHLENBQUM7Ozs7O0lBRXpDLG1DQUFVOzs7O0lBQVYsVUFBVyxVQUFrQjs7WUFDckIsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSw0Q0FBMEMsVUFBWTtTQUM1RDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQXlCLE9BQU8sQ0FBQyxDQUFDO0lBQzVELENBQUM7Ozs7O0lBRUQsaUNBQVE7Ozs7SUFBUixVQUFTLElBQXFCOztZQUN0QixPQUFPLEdBQWtDO1lBQzdDLE1BQU0sRUFBRSxNQUFNO1lBQ2QsR0FBRyxFQUFFLHVCQUF1QjtZQUM1QixJQUFJLE1BQUE7U0FDTDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQW9DLE9BQU8sRUFBRSxFQUFFLGVBQWUsRUFBRSxJQUFJLEVBQUUsQ0FBQyxDQUFDO0lBQ2xHLENBQUM7O2dCQXZCRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQUxRLFdBQVc7Ozt5QkFGcEI7Q0E2QkMsQUF4QkQsSUF3QkM7U0FyQlksY0FBYzs7Ozs7O0lBQ2IsOEJBQXlCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgUmVzdFNlcnZpY2UsIFJlc3QgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgUmVnaXN0ZXJSZXNwb25zZSwgUmVnaXN0ZXJSZXF1ZXN0LCBUZW5hbnRJZFJlc3BvbnNlIH0gZnJvbSAnLi4vbW9kZWxzJztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIEFjY291bnRTZXJ2aWNlIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZXN0OiBSZXN0U2VydmljZSkge31cblxuICBmaW5kVGVuYW50KHRlbmFudE5hbWU6IHN0cmluZyk6IE9ic2VydmFibGU8VGVuYW50SWRSZXNwb25zZT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICB1cmw6IGAvYXBpL2FicC9tdWx0aS10ZW5hbmN5L3RlbmFudHMvYnktbmFtZS8ke3RlbmFudE5hbWV9YCxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIFRlbmFudElkUmVzcG9uc2U+KHJlcXVlc3QpO1xuICB9XG5cbiAgcmVnaXN0ZXIoYm9keTogUmVnaXN0ZXJSZXF1ZXN0KTogT2JzZXJ2YWJsZTxSZWdpc3RlclJlc3BvbnNlPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFJlZ2lzdGVyUmVxdWVzdD4gPSB7XG4gICAgICBtZXRob2Q6ICdQT1NUJyxcbiAgICAgIHVybDogYC9hcGkvYWNjb3VudC9yZWdpc3RlcmAsXG4gICAgICBib2R5LFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8UmVnaXN0ZXJSZXF1ZXN0LCBSZWdpc3RlclJlc3BvbnNlPihyZXF1ZXN0LCB7IHNraXBIYW5kbGVFcnJvcjogdHJ1ZSB9KTtcbiAgfVxufVxuIl19