/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/permission-management.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import * as i0 from "@angular/core";
import * as i1 from "@abp/ng.core";
export class PermissionManagementService {
    /**
     * @param {?} rest
     */
    constructor(rest) {
        this.rest = rest;
    }
    /**
     * @param {?} params
     * @return {?}
     */
    getPermissions(params) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: '/api/abp/permissions',
            params,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    updatePermissions({ permissions, providerKey, providerName, }) {
        /** @type {?} */
        const request = {
            method: 'PUT',
            url: '/api/abp/permissions',
            body: { permissions },
            params: { providerKey, providerName },
        };
        return this.rest.request(request);
    }
}
PermissionManagementService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
PermissionManagementService.ctorParameters = () => [
    { type: RestService }
];
/** @nocollapse */ PermissionManagementService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function PermissionManagementService_Factory() { return new PermissionManagementService(i0.ɵɵinject(i1.RestService)); }, token: PermissionManagementService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    PermissionManagementService.prototype.rest;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnBlcm1pc3Npb24tbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLFdBQVcsRUFBUSxNQUFNLGNBQWMsQ0FBQzs7O0FBT2pELE1BQU0sT0FBTywyQkFBMkI7Ozs7SUFDdEMsWUFBb0IsSUFBaUI7UUFBakIsU0FBSSxHQUFKLElBQUksQ0FBYTtJQUFHLENBQUM7Ozs7O0lBRXpDLGNBQWMsQ0FBQyxNQUE0Qzs7Y0FDbkQsT0FBTyxHQUF1RDtZQUNsRSxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSxzQkFBc0I7WUFDM0IsTUFBTTtTQUNQO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBc0UsT0FBTyxDQUFDLENBQUM7SUFDekcsQ0FBQzs7Ozs7SUFFRCxpQkFBaUIsQ0FBQyxFQUNoQixXQUFXLEVBQ1gsV0FBVyxFQUNYLFlBQVksR0FDOEQ7O2NBQ3BFLE9BQU8sR0FBcUQ7WUFDaEUsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEVBQUUsc0JBQXNCO1lBQzNCLElBQUksRUFBRSxFQUFFLFdBQVcsRUFBRTtZQUNyQixNQUFNLEVBQUUsRUFBRSxXQUFXLEVBQUUsWUFBWSxFQUFFO1NBQ3RDO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBMkMsT0FBTyxDQUFDLENBQUM7SUFDOUUsQ0FBQzs7O1lBN0JGLFVBQVUsU0FBQztnQkFDVixVQUFVLEVBQUUsTUFBTTthQUNuQjs7OztZQU5RLFdBQVc7Ozs7Ozs7O0lBUU4sMkNBQXlCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgUmVzdFNlcnZpY2UsIFJlc3QgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgUGVybWlzc2lvbk1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvcGVybWlzc2lvbi1tYW5hZ2VtZW50JztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIFBlcm1pc3Npb25NYW5hZ2VtZW50U2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVzdDogUmVzdFNlcnZpY2UpIHt9XG5cbiAgZ2V0UGVybWlzc2lvbnMocGFyYW1zOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5HcmFudGVkUHJvdmlkZXIpOiBPYnNlcnZhYmxlPFBlcm1pc3Npb25NYW5hZ2VtZW50LlJlc3BvbnNlPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFBlcm1pc3Npb25NYW5hZ2VtZW50LkdyYW50ZWRQcm92aWRlcj4gPSB7XG4gICAgICBtZXRob2Q6ICdHRVQnLFxuICAgICAgdXJsOiAnL2FwaS9hYnAvcGVybWlzc2lvbnMnLFxuICAgICAgcGFyYW1zLFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8UGVybWlzc2lvbk1hbmFnZW1lbnQuR3JhbnRlZFByb3ZpZGVyLCBQZXJtaXNzaW9uTWFuYWdlbWVudC5SZXNwb25zZT4ocmVxdWVzdCk7XG4gIH1cblxuICB1cGRhdGVQZXJtaXNzaW9ucyh7XG4gICAgcGVybWlzc2lvbnMsXG4gICAgcHJvdmlkZXJLZXksXG4gICAgcHJvdmlkZXJOYW1lLFxuICB9OiBQZXJtaXNzaW9uTWFuYWdlbWVudC5HcmFudGVkUHJvdmlkZXIgJiBQZXJtaXNzaW9uTWFuYWdlbWVudC5VcGRhdGVSZXF1ZXN0KTogT2JzZXJ2YWJsZTxudWxsPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFBlcm1pc3Npb25NYW5hZ2VtZW50LlVwZGF0ZVJlcXVlc3Q+ID0ge1xuICAgICAgbWV0aG9kOiAnUFVUJyxcbiAgICAgIHVybDogJy9hcGkvYWJwL3Blcm1pc3Npb25zJyxcbiAgICAgIGJvZHk6IHsgcGVybWlzc2lvbnMgfSxcbiAgICAgIHBhcmFtczogeyBwcm92aWRlcktleSwgcHJvdmlkZXJOYW1lIH0sXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxQZXJtaXNzaW9uTWFuYWdlbWVudC5VcGRhdGVSZXF1ZXN0LCBudWxsPihyZXF1ZXN0KTtcbiAgfVxufVxuIl19