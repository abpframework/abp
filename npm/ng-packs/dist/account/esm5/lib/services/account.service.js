/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL2FjY291bnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsV0FBVyxFQUFRLE1BQU0sY0FBYyxDQUFDOzs7QUFHakQ7SUFJRSx3QkFBb0IsSUFBaUI7UUFBakIsU0FBSSxHQUFKLElBQUksQ0FBYTtJQUFHLENBQUM7Ozs7O0lBRXpDLG1DQUFVOzs7O0lBQVYsVUFBVyxVQUFrQjs7WUFDckIsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSw0Q0FBMEMsVUFBWTtTQUM1RDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQXlCLE9BQU8sQ0FBQyxDQUFDO0lBQzVELENBQUM7Ozs7O0lBRUQsaUNBQVE7Ozs7SUFBUixVQUFTLElBQXFCOztZQUN0QixPQUFPLEdBQWtDO1lBQzdDLE1BQU0sRUFBRSxNQUFNO1lBQ2QsR0FBRyxFQUFFLHVCQUF1QjtZQUM1QixJQUFJLE1BQUE7U0FDTDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQW9DLE9BQU8sRUFBRSxFQUFFLGVBQWUsRUFBRSxJQUFJLEVBQUUsQ0FBQyxDQUFDO0lBQ2xHLENBQUM7O2dCQXZCRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQUxRLFdBQVc7Ozt5QkFGcEI7Q0E2QkMsQUF4QkQsSUF3QkM7U0FyQlksY0FBYzs7Ozs7O0lBQ2IsOEJBQXlCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IFJlc3RTZXJ2aWNlLCBSZXN0IH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgUmVnaXN0ZXJSZXNwb25zZSwgUmVnaXN0ZXJSZXF1ZXN0LCBUZW5hbnRJZFJlc3BvbnNlIH0gZnJvbSAnLi4vbW9kZWxzJztcclxuXHJcbkBJbmplY3RhYmxlKHtcclxuICBwcm92aWRlZEluOiAncm9vdCcsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBBY2NvdW50U2VydmljZSB7XHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZXN0OiBSZXN0U2VydmljZSkge31cclxuXHJcbiAgZmluZFRlbmFudCh0ZW5hbnROYW1lOiBzdHJpbmcpOiBPYnNlcnZhYmxlPFRlbmFudElkUmVzcG9uc2U+IHtcclxuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcclxuICAgICAgbWV0aG9kOiAnR0VUJyxcclxuICAgICAgdXJsOiBgL2FwaS9hYnAvbXVsdGktdGVuYW5jeS90ZW5hbnRzL2J5LW5hbWUvJHt0ZW5hbnROYW1lfWAsXHJcbiAgICB9O1xyXG5cclxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxudWxsLCBUZW5hbnRJZFJlc3BvbnNlPihyZXF1ZXN0KTtcclxuICB9XHJcblxyXG4gIHJlZ2lzdGVyKGJvZHk6IFJlZ2lzdGVyUmVxdWVzdCk6IE9ic2VydmFibGU8UmVnaXN0ZXJSZXNwb25zZT4ge1xyXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFJlZ2lzdGVyUmVxdWVzdD4gPSB7XHJcbiAgICAgIG1ldGhvZDogJ1BPU1QnLFxyXG4gICAgICB1cmw6ICcvYXBpL2FjY291bnQvcmVnaXN0ZXInLFxyXG4gICAgICBib2R5LFxyXG4gICAgfTtcclxuXHJcbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8UmVnaXN0ZXJSZXF1ZXN0LCBSZWdpc3RlclJlc3BvbnNlPihyZXF1ZXN0LCB7IHNraXBIYW5kbGVFcnJvcjogdHJ1ZSB9KTtcclxuICB9XHJcbn1cclxuIl19