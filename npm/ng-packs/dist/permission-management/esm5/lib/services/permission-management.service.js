/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnBlcm1pc3Npb24tbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsV0FBVyxFQUFRLE1BQU0sY0FBYyxDQUFDOzs7QUFJakQ7SUFJRSxxQ0FBb0IsSUFBaUI7UUFBakIsU0FBSSxHQUFKLElBQUksQ0FBYTtJQUFHLENBQUM7Ozs7O0lBRXpDLG9EQUFjOzs7O0lBQWQsVUFBZSxNQUE0Qzs7WUFDbkQsT0FBTyxHQUF1RDtZQUNsRSxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSxzQkFBc0I7WUFDM0IsTUFBTSxRQUFBO1NBQ1A7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFzRSxPQUFPLENBQUMsQ0FBQztJQUN6RyxDQUFDOzs7OztJQUVELHVEQUFpQjs7OztJQUFqQixVQUFrQixFQUkwRDtZQUgxRSw0QkFBVyxFQUNYLDRCQUFXLEVBQ1gsOEJBQVk7O1lBRU4sT0FBTyxHQUFxRDtZQUNoRSxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSxzQkFBc0I7WUFDM0IsSUFBSSxFQUFFLEVBQUUsV0FBVyxhQUFBLEVBQUU7WUFDckIsTUFBTSxFQUFFLEVBQUUsV0FBVyxhQUFBLEVBQUUsWUFBWSxjQUFBLEVBQUU7U0FDdEM7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUEyQyxPQUFPLENBQUMsQ0FBQztJQUM5RSxDQUFDOztnQkE3QkYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFOUSxXQUFXOzs7c0NBRHBCO0NBbUNDLEFBOUJELElBOEJDO1NBM0JZLDJCQUEyQjs7Ozs7O0lBQzFCLDJDQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgUmVzdFNlcnZpY2UsIFJlc3QgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IFBlcm1pc3Npb25NYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3Blcm1pc3Npb24tbWFuYWdlbWVudCc7XHJcblxyXG5ASW5qZWN0YWJsZSh7XHJcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgUGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlIHtcclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJlc3Q6IFJlc3RTZXJ2aWNlKSB7fVxyXG5cclxuICBnZXRQZXJtaXNzaW9ucyhwYXJhbXM6IFBlcm1pc3Npb25NYW5hZ2VtZW50LkdyYW50ZWRQcm92aWRlcik6IE9ic2VydmFibGU8UGVybWlzc2lvbk1hbmFnZW1lbnQuUmVzcG9uc2U+IHtcclxuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxQZXJtaXNzaW9uTWFuYWdlbWVudC5HcmFudGVkUHJvdmlkZXI+ID0ge1xyXG4gICAgICBtZXRob2Q6ICdHRVQnLFxyXG4gICAgICB1cmw6ICcvYXBpL2FicC9wZXJtaXNzaW9ucycsXHJcbiAgICAgIHBhcmFtcyxcclxuICAgIH07XHJcblxyXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFBlcm1pc3Npb25NYW5hZ2VtZW50LkdyYW50ZWRQcm92aWRlciwgUGVybWlzc2lvbk1hbmFnZW1lbnQuUmVzcG9uc2U+KHJlcXVlc3QpO1xyXG4gIH1cclxuXHJcbiAgdXBkYXRlUGVybWlzc2lvbnMoe1xyXG4gICAgcGVybWlzc2lvbnMsXHJcbiAgICBwcm92aWRlcktleSxcclxuICAgIHByb3ZpZGVyTmFtZSxcclxuICB9OiBQZXJtaXNzaW9uTWFuYWdlbWVudC5HcmFudGVkUHJvdmlkZXIgJiBQZXJtaXNzaW9uTWFuYWdlbWVudC5VcGRhdGVSZXF1ZXN0KTogT2JzZXJ2YWJsZTxudWxsPiB7XHJcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8UGVybWlzc2lvbk1hbmFnZW1lbnQuVXBkYXRlUmVxdWVzdD4gPSB7XHJcbiAgICAgIG1ldGhvZDogJ1BVVCcsXHJcbiAgICAgIHVybDogJy9hcGkvYWJwL3Blcm1pc3Npb25zJyxcclxuICAgICAgYm9keTogeyBwZXJtaXNzaW9ucyB9LFxyXG4gICAgICBwYXJhbXM6IHsgcHJvdmlkZXJLZXksIHByb3ZpZGVyTmFtZSB9LFxyXG4gICAgfTtcclxuXHJcbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8UGVybWlzc2lvbk1hbmFnZW1lbnQuVXBkYXRlUmVxdWVzdCwgbnVsbD4ocmVxdWVzdCk7XHJcbiAgfVxyXG59XHJcbiJdfQ==