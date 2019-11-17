/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/feature-management.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import { Store } from '@ngxs/store';
import * as i0 from "@angular/core";
import * as i1 from "@abp/ng.core";
import * as i2 from "@ngxs/store";
var FeatureManagementService = /** @class */ (function () {
    function FeatureManagementService(rest, store) {
        this.rest = rest;
        this.store = store;
    }
    /**
     * @param {?} params
     * @return {?}
     */
    FeatureManagementService.prototype.getFeatures = /**
     * @param {?} params
     * @return {?}
     */
    function (params) {
        /** @type {?} */
        var request = {
            method: 'GET',
            url: '/api/abp/features',
            params: params,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    FeatureManagementService.prototype.updateFeatures = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var features = _a.features, providerKey = _a.providerKey, providerName = _a.providerName;
        /** @type {?} */
        var request = {
            method: 'PUT',
            url: '/api/abp/features',
            body: { features: features },
            params: { providerKey: providerKey, providerName: providerName },
        };
        return this.rest.request(request);
    };
    FeatureManagementService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    FeatureManagementService.ctorParameters = function () { return [
        { type: RestService },
        { type: Store }
    ]; };
    /** @nocollapse */ FeatureManagementService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function FeatureManagementService_Factory() { return new FeatureManagementService(i0.ɵɵinject(i1.RestService), i0.ɵɵinject(i2.Store)); }, token: FeatureManagementService, providedIn: "root" });
    return FeatureManagementService;
}());
export { FeatureManagementService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    FeatureManagementService.prototype.rest;
    /**
     * @type {?}
     * @private
     */
    FeatureManagementService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmVhdHVyZS1tYW5hZ2VtZW50LnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmZlYXR1cmUtbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9mZWF0dXJlLW1hbmFnZW1lbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLFdBQVcsRUFBUSxNQUFNLGNBQWMsQ0FBQztBQUNqRCxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDOzs7O0FBSXBDO0lBSUUsa0NBQW9CLElBQWlCLEVBQVUsS0FBWTtRQUF2QyxTQUFJLEdBQUosSUFBSSxDQUFhO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7O0lBRS9ELDhDQUFXOzs7O0lBQVgsVUFBWSxNQUFrQzs7WUFDdEMsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSxtQkFBbUI7WUFDeEIsTUFBTSxRQUFBO1NBQ1A7UUFDRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUF5RCxPQUFPLENBQUMsQ0FBQztJQUM1RixDQUFDOzs7OztJQUVELGlEQUFjOzs7O0lBQWQsVUFBZSxFQUkyQztZQUh4RCxzQkFBUSxFQUNSLDRCQUFXLEVBQ1gsOEJBQVk7O1lBRU4sT0FBTyxHQUE2QztZQUN4RCxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSxtQkFBbUI7WUFDeEIsSUFBSSxFQUFFLEVBQUUsUUFBUSxVQUFBLEVBQUU7WUFDbEIsTUFBTSxFQUFFLEVBQUUsV0FBVyxhQUFBLEVBQUUsWUFBWSxjQUFBLEVBQUU7U0FDdEM7UUFDRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFtQyxPQUFPLENBQUMsQ0FBQztJQUN0RSxDQUFDOztnQkEzQkYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFQUSxXQUFXO2dCQUNYLEtBQUs7OzttQ0FGZDtDQWtDQyxBQTVCRCxJQTRCQztTQXpCWSx3QkFBd0I7Ozs7OztJQUN2Qix3Q0FBeUI7Ozs7O0lBQUUseUNBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBSZXN0U2VydmljZSwgUmVzdCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IEZlYXR1cmVNYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzJztcclxuXHJcbkBJbmplY3RhYmxlKHtcclxuICBwcm92aWRlZEluOiAncm9vdCcsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBGZWF0dXJlTWFuYWdlbWVudFNlcnZpY2Uge1xyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVzdDogUmVzdFNlcnZpY2UsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxyXG5cclxuICBnZXRGZWF0dXJlcyhwYXJhbXM6IEZlYXR1cmVNYW5hZ2VtZW50LlByb3ZpZGVyKTogT2JzZXJ2YWJsZTxGZWF0dXJlTWFuYWdlbWVudC5GZWF0dXJlcz4ge1xyXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PG51bGw+ID0ge1xyXG4gICAgICBtZXRob2Q6ICdHRVQnLFxyXG4gICAgICB1cmw6ICcvYXBpL2FicC9mZWF0dXJlcycsXHJcbiAgICAgIHBhcmFtcyxcclxuICAgIH07XHJcbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8RmVhdHVyZU1hbmFnZW1lbnQuUHJvdmlkZXIsIEZlYXR1cmVNYW5hZ2VtZW50LkZlYXR1cmVzPihyZXF1ZXN0KTtcclxuICB9XHJcblxyXG4gIHVwZGF0ZUZlYXR1cmVzKHtcclxuICAgIGZlYXR1cmVzLFxyXG4gICAgcHJvdmlkZXJLZXksXHJcbiAgICBwcm92aWRlck5hbWUsXHJcbiAgfTogRmVhdHVyZU1hbmFnZW1lbnQuUHJvdmlkZXIgJiBGZWF0dXJlTWFuYWdlbWVudC5GZWF0dXJlcyk6IE9ic2VydmFibGU8bnVsbD4ge1xyXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PEZlYXR1cmVNYW5hZ2VtZW50LkZlYXR1cmVzPiA9IHtcclxuICAgICAgbWV0aG9kOiAnUFVUJyxcclxuICAgICAgdXJsOiAnL2FwaS9hYnAvZmVhdHVyZXMnLFxyXG4gICAgICBib2R5OiB7IGZlYXR1cmVzIH0sXHJcbiAgICAgIHBhcmFtczogeyBwcm92aWRlcktleSwgcHJvdmlkZXJOYW1lIH0sXHJcbiAgICB9O1xyXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PEZlYXR1cmVNYW5hZ2VtZW50LkZlYXR1cmVzLCBudWxsPihyZXF1ZXN0KTtcclxuICB9XHJcbn1cclxuIl19