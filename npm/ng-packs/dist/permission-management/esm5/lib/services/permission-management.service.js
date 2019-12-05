/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/permission-management.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import * as i0 from "@angular/core";
import * as i1 from "@abp/ng.core";
var PermissionManagementService = /** @class */ (function () {
    function PermissionManagementService(rest) {
        this.rest = rest;
    }
    /**
     * @param {?} params
     * @return {?}
     */
    PermissionManagementService.prototype.getPermissions = /**
     * @param {?} params
     * @return {?}
     */
    function (params) {
        /** @type {?} */
        var request = {
            method: 'GET',
            url: '/api/abp/permissions',
            params: params,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    PermissionManagementService.prototype.updatePermissions = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var permissions = _a.permissions, providerKey = _a.providerKey, providerName = _a.providerName;
        /** @type {?} */
        var request = {
            method: 'PUT',
            url: '/api/abp/permissions',
            body: { permissions: permissions },
            params: { providerKey: providerKey, providerName: providerName },
        };
        return this.rest.request(request);
    };
    PermissionManagementService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    PermissionManagementService.ctorParameters = function () { return [
        { type: RestService }
    ]; };
    /** @nocollapse */ PermissionManagementService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function PermissionManagementService_Factory() { return new PermissionManagementService(i0.ɵɵinject(i1.RestService)); }, token: PermissionManagementService, providedIn: "root" });
    return PermissionManagementService;
}());
export { PermissionManagementService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    PermissionManagementService.prototype.rest;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnBlcm1pc3Npb24tbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLFdBQVcsRUFBUSxNQUFNLGNBQWMsQ0FBQzs7O0FBSWpEO0lBSUUscUNBQW9CLElBQWlCO1FBQWpCLFNBQUksR0FBSixJQUFJLENBQWE7SUFBRyxDQUFDOzs7OztJQUV6QyxvREFBYzs7OztJQUFkLFVBQWUsTUFBNEM7O1lBQ25ELE9BQU8sR0FBdUQ7WUFDbEUsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEVBQUUsc0JBQXNCO1lBQzNCLE1BQU0sUUFBQTtTQUNQO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBc0UsT0FBTyxDQUFDLENBQUM7SUFDekcsQ0FBQzs7Ozs7SUFFRCx1REFBaUI7Ozs7SUFBakIsVUFBa0IsRUFJMEQ7WUFIMUUsNEJBQVcsRUFDWCw0QkFBVyxFQUNYLDhCQUFZOztZQUVOLE9BQU8sR0FBcUQ7WUFDaEUsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEVBQUUsc0JBQXNCO1lBQzNCLElBQUksRUFBRSxFQUFFLFdBQVcsYUFBQSxFQUFFO1lBQ3JCLE1BQU0sRUFBRSxFQUFFLFdBQVcsYUFBQSxFQUFFLFlBQVksY0FBQSxFQUFFO1NBQ3RDO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBMkMsT0FBTyxDQUFDLENBQUM7SUFDOUUsQ0FBQzs7Z0JBN0JGLFVBQVUsU0FBQztvQkFDVixVQUFVLEVBQUUsTUFBTTtpQkFDbkI7Ozs7Z0JBTlEsV0FBVzs7O3NDQURwQjtDQW1DQyxBQTlCRCxJQThCQztTQTNCWSwyQkFBMkI7Ozs7OztJQUMxQiwyQ0FBeUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBSZXN0U2VydmljZSwgUmVzdCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudCB9IGZyb20gJy4uL21vZGVscy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQnO1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290Jyxcbn0pXG5leHBvcnQgY2xhc3MgUGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZXN0OiBSZXN0U2VydmljZSkge31cblxuICBnZXRQZXJtaXNzaW9ucyhwYXJhbXM6IFBlcm1pc3Npb25NYW5hZ2VtZW50LkdyYW50ZWRQcm92aWRlcik6IE9ic2VydmFibGU8UGVybWlzc2lvbk1hbmFnZW1lbnQuUmVzcG9uc2U+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8UGVybWlzc2lvbk1hbmFnZW1lbnQuR3JhbnRlZFByb3ZpZGVyPiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICB1cmw6ICcvYXBpL2FicC9wZXJtaXNzaW9ucycsXG4gICAgICBwYXJhbXMsXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxQZXJtaXNzaW9uTWFuYWdlbWVudC5HcmFudGVkUHJvdmlkZXIsIFBlcm1pc3Npb25NYW5hZ2VtZW50LlJlc3BvbnNlPihyZXF1ZXN0KTtcbiAgfVxuXG4gIHVwZGF0ZVBlcm1pc3Npb25zKHtcbiAgICBwZXJtaXNzaW9ucyxcbiAgICBwcm92aWRlcktleSxcbiAgICBwcm92aWRlck5hbWUsXG4gIH06IFBlcm1pc3Npb25NYW5hZ2VtZW50LkdyYW50ZWRQcm92aWRlciAmIFBlcm1pc3Npb25NYW5hZ2VtZW50LlVwZGF0ZVJlcXVlc3QpOiBPYnNlcnZhYmxlPG51bGw+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8UGVybWlzc2lvbk1hbmFnZW1lbnQuVXBkYXRlUmVxdWVzdD4gPSB7XG4gICAgICBtZXRob2Q6ICdQVVQnLFxuICAgICAgdXJsOiAnL2FwaS9hYnAvcGVybWlzc2lvbnMnLFxuICAgICAgYm9keTogeyBwZXJtaXNzaW9ucyB9LFxuICAgICAgcGFyYW1zOiB7IHByb3ZpZGVyS2V5LCBwcm92aWRlck5hbWUgfSxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFBlcm1pc3Npb25NYW5hZ2VtZW50LlVwZGF0ZVJlcXVlc3QsIG51bGw+KHJlcXVlc3QpO1xuICB9XG59XG4iXX0=