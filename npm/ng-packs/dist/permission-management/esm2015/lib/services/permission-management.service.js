/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnBlcm1pc3Npb24tbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsV0FBVyxFQUFRLE1BQU0sY0FBYyxDQUFDOzs7QUFPakQsTUFBTSxPQUFPLDJCQUEyQjs7OztJQUN0QyxZQUFvQixJQUFpQjtRQUFqQixTQUFJLEdBQUosSUFBSSxDQUFhO0lBQUcsQ0FBQzs7Ozs7SUFFekMsY0FBYyxDQUFDLE1BQTRDOztjQUNuRCxPQUFPLEdBQXVEO1lBQ2xFLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRyxFQUFFLHNCQUFzQjtZQUMzQixNQUFNO1NBQ1A7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFzRSxPQUFPLENBQUMsQ0FBQztJQUN6RyxDQUFDOzs7OztJQUVELGlCQUFpQixDQUFDLEVBQ2hCLFdBQVcsRUFDWCxXQUFXLEVBQ1gsWUFBWSxHQUM4RDs7Y0FDcEUsT0FBTyxHQUFxRDtZQUNoRSxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSxzQkFBc0I7WUFDM0IsSUFBSSxFQUFFLEVBQUUsV0FBVyxFQUFFO1lBQ3JCLE1BQU0sRUFBRSxFQUFFLFdBQVcsRUFBRSxZQUFZLEVBQUU7U0FDdEM7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUEyQyxPQUFPLENBQUMsQ0FBQztJQUM5RSxDQUFDOzs7WUE3QkYsVUFBVSxTQUFDO2dCQUNWLFVBQVUsRUFBRSxNQUFNO2FBQ25COzs7O1lBTlEsV0FBVzs7Ozs7Ozs7SUFRTiwyQ0FBeUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFJlc3RTZXJ2aWNlLCBSZXN0IH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xyXG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudCB9IGZyb20gJy4uL21vZGVscy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQnO1xyXG5cclxuQEluamVjdGFibGUoe1xyXG4gIHByb3ZpZGVkSW46ICdyb290JyxcclxufSlcclxuZXhwb3J0IGNsYXNzIFBlcm1pc3Npb25NYW5hZ2VtZW50U2VydmljZSB7XHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZXN0OiBSZXN0U2VydmljZSkge31cclxuXHJcbiAgZ2V0UGVybWlzc2lvbnMocGFyYW1zOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5HcmFudGVkUHJvdmlkZXIpOiBPYnNlcnZhYmxlPFBlcm1pc3Npb25NYW5hZ2VtZW50LlJlc3BvbnNlPiB7XHJcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8UGVybWlzc2lvbk1hbmFnZW1lbnQuR3JhbnRlZFByb3ZpZGVyPiA9IHtcclxuICAgICAgbWV0aG9kOiAnR0VUJyxcclxuICAgICAgdXJsOiAnL2FwaS9hYnAvcGVybWlzc2lvbnMnLFxyXG4gICAgICBwYXJhbXMsXHJcbiAgICB9O1xyXG5cclxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxQZXJtaXNzaW9uTWFuYWdlbWVudC5HcmFudGVkUHJvdmlkZXIsIFBlcm1pc3Npb25NYW5hZ2VtZW50LlJlc3BvbnNlPihyZXF1ZXN0KTtcclxuICB9XHJcblxyXG4gIHVwZGF0ZVBlcm1pc3Npb25zKHtcclxuICAgIHBlcm1pc3Npb25zLFxyXG4gICAgcHJvdmlkZXJLZXksXHJcbiAgICBwcm92aWRlck5hbWUsXHJcbiAgfTogUGVybWlzc2lvbk1hbmFnZW1lbnQuR3JhbnRlZFByb3ZpZGVyICYgUGVybWlzc2lvbk1hbmFnZW1lbnQuVXBkYXRlUmVxdWVzdCk6IE9ic2VydmFibGU8bnVsbD4ge1xyXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFBlcm1pc3Npb25NYW5hZ2VtZW50LlVwZGF0ZVJlcXVlc3Q+ID0ge1xyXG4gICAgICBtZXRob2Q6ICdQVVQnLFxyXG4gICAgICB1cmw6ICcvYXBpL2FicC9wZXJtaXNzaW9ucycsXHJcbiAgICAgIGJvZHk6IHsgcGVybWlzc2lvbnMgfSxcclxuICAgICAgcGFyYW1zOiB7IHByb3ZpZGVyS2V5LCBwcm92aWRlck5hbWUgfSxcclxuICAgIH07XHJcblxyXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFBlcm1pc3Npb25NYW5hZ2VtZW50LlVwZGF0ZVJlcXVlc3QsIG51bGw+KHJlcXVlc3QpO1xyXG4gIH1cclxufVxyXG4iXX0=