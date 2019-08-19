/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmVhdHVyZS1tYW5hZ2VtZW50LnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmZlYXR1cmUtbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9mZWF0dXJlLW1hbmFnZW1lbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsV0FBVyxFQUFRLE1BQU0sY0FBYyxDQUFDO0FBQ2pELE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7Ozs7QUFJcEM7SUFJRSxrQ0FBb0IsSUFBaUIsRUFBVSxLQUFZO1FBQXZDLFNBQUksR0FBSixJQUFJLENBQWE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQzs7Ozs7SUFFL0QsOENBQVc7Ozs7SUFBWCxVQUFZLE1BQWtDOztZQUN0QyxPQUFPLEdBQXVCO1lBQ2xDLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRyxFQUFFLG1CQUFtQjtZQUN4QixNQUFNLFFBQUE7U0FDUDtRQUNELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQXlELE9BQU8sQ0FBQyxDQUFDO0lBQzVGLENBQUM7Ozs7O0lBRUQsaURBQWM7Ozs7SUFBZCxVQUFlLEVBSTJDO1lBSHhELHNCQUFRLEVBQ1IsNEJBQVcsRUFDWCw4QkFBWTs7WUFFTixPQUFPLEdBQTZDO1lBQ3hELE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRyxFQUFFLG1CQUFtQjtZQUN4QixJQUFJLEVBQUUsRUFBRSxRQUFRLFVBQUEsRUFBRTtZQUNsQixNQUFNLEVBQUUsRUFBRSxXQUFXLGFBQUEsRUFBRSxZQUFZLGNBQUEsRUFBRTtTQUN0QztRQUNELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQW1DLE9BQU8sQ0FBQyxDQUFDO0lBQ3RFLENBQUM7O2dCQTNCRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQVBRLFdBQVc7Z0JBQ1gsS0FBSzs7O21DQUZkO0NBa0NDLEFBNUJELElBNEJDO1NBekJZLHdCQUF3Qjs7Ozs7O0lBQ3ZCLHdDQUF5Qjs7Ozs7SUFBRSx5Q0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBSZXN0U2VydmljZSwgUmVzdCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IEZlYXR1cmVNYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzJztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIEZlYXR1cmVNYW5hZ2VtZW50U2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVzdDogUmVzdFNlcnZpY2UsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIGdldEZlYXR1cmVzKHBhcmFtczogRmVhdHVyZU1hbmFnZW1lbnQuUHJvdmlkZXIpOiBPYnNlcnZhYmxlPEZlYXR1cmVNYW5hZ2VtZW50LkZlYXR1cmVzPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PG51bGw+ID0ge1xuICAgICAgbWV0aG9kOiAnR0VUJyxcbiAgICAgIHVybDogJy9hcGkvYWJwL2ZlYXR1cmVzJyxcbiAgICAgIHBhcmFtcyxcbiAgICB9O1xuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxGZWF0dXJlTWFuYWdlbWVudC5Qcm92aWRlciwgRmVhdHVyZU1hbmFnZW1lbnQuRmVhdHVyZXM+KHJlcXVlc3QpO1xuICB9XG5cbiAgdXBkYXRlRmVhdHVyZXMoe1xuICAgIGZlYXR1cmVzLFxuICAgIHByb3ZpZGVyS2V5LFxuICAgIHByb3ZpZGVyTmFtZSxcbiAgfTogRmVhdHVyZU1hbmFnZW1lbnQuUHJvdmlkZXIgJiBGZWF0dXJlTWFuYWdlbWVudC5GZWF0dXJlcyk6IE9ic2VydmFibGU8bnVsbD4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxGZWF0dXJlTWFuYWdlbWVudC5GZWF0dXJlcz4gPSB7XG4gICAgICBtZXRob2Q6ICdQVVQnLFxuICAgICAgdXJsOiAnL2FwaS9hYnAvZmVhdHVyZXMnLFxuICAgICAgYm9keTogeyBmZWF0dXJlcyB9LFxuICAgICAgcGFyYW1zOiB7IHByb3ZpZGVyS2V5LCBwcm92aWRlck5hbWUgfSxcbiAgICB9O1xuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxGZWF0dXJlTWFuYWdlbWVudC5GZWF0dXJlcywgbnVsbD4ocmVxdWVzdCk7XG4gIH1cbn1cbiJdfQ==