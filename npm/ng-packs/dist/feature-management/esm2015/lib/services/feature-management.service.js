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
export class FeatureManagementService {
    /**
     * @param {?} rest
     * @param {?} store
     */
    constructor(rest, store) {
        this.rest = rest;
        this.store = store;
    }
    /**
     * @param {?} params
     * @return {?}
     */
    getFeatures(params) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: '/api/abp/features',
            params,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    updateFeatures({ features, providerKey, providerName, }) {
        /** @type {?} */
        const request = {
            method: 'PUT',
            url: '/api/abp/features',
            body: { features },
            params: { providerKey, providerName },
        };
        return this.rest.request(request);
    }
}
FeatureManagementService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
FeatureManagementService.ctorParameters = () => [
    { type: RestService },
    { type: Store }
];
/** @nocollapse */ FeatureManagementService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function FeatureManagementService_Factory() { return new FeatureManagementService(i0.ɵɵinject(i1.RestService), i0.ɵɵinject(i2.Store)); }, token: FeatureManagementService, providedIn: "root" });
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmVhdHVyZS1tYW5hZ2VtZW50LnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmZlYXR1cmUtbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9mZWF0dXJlLW1hbmFnZW1lbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLFdBQVcsRUFBUSxNQUFNLGNBQWMsQ0FBQztBQUNqRCxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDOzs7O0FBT3BDLE1BQU0sT0FBTyx3QkFBd0I7Ozs7O0lBQ25DLFlBQW9CLElBQWlCLEVBQVUsS0FBWTtRQUF2QyxTQUFJLEdBQUosSUFBSSxDQUFhO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7O0lBRS9ELFdBQVcsQ0FBQyxNQUFrQzs7Y0FDdEMsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSxtQkFBbUI7WUFDeEIsTUFBTTtTQUNQO1FBQ0QsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBeUQsT0FBTyxDQUFDLENBQUM7SUFDNUYsQ0FBQzs7Ozs7SUFFRCxjQUFjLENBQUMsRUFDYixRQUFRLEVBQ1IsV0FBVyxFQUNYLFlBQVksR0FDNEM7O2NBQ2xELE9BQU8sR0FBNkM7WUFDeEQsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEVBQUUsbUJBQW1CO1lBQ3hCLElBQUksRUFBRSxFQUFFLFFBQVEsRUFBRTtZQUNsQixNQUFNLEVBQUUsRUFBRSxXQUFXLEVBQUUsWUFBWSxFQUFFO1NBQ3RDO1FBQ0QsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBbUMsT0FBTyxDQUFDLENBQUM7SUFDdEUsQ0FBQzs7O1lBM0JGLFVBQVUsU0FBQztnQkFDVixVQUFVLEVBQUUsTUFBTTthQUNuQjs7OztZQVBRLFdBQVc7WUFDWCxLQUFLOzs7Ozs7OztJQVFBLHdDQUF5Qjs7Ozs7SUFBRSx5Q0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFJlc3RTZXJ2aWNlLCBSZXN0IH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgRmVhdHVyZU1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMnO1xyXG5cclxuQEluamVjdGFibGUoe1xyXG4gIHByb3ZpZGVkSW46ICdyb290JyxcclxufSlcclxuZXhwb3J0IGNsYXNzIEZlYXR1cmVNYW5hZ2VtZW50U2VydmljZSB7XHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZXN0OiBSZXN0U2VydmljZSwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XHJcblxyXG4gIGdldEZlYXR1cmVzKHBhcmFtczogRmVhdHVyZU1hbmFnZW1lbnQuUHJvdmlkZXIpOiBPYnNlcnZhYmxlPEZlYXR1cmVNYW5hZ2VtZW50LkZlYXR1cmVzPiB7XHJcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8bnVsbD4gPSB7XHJcbiAgICAgIG1ldGhvZDogJ0dFVCcsXHJcbiAgICAgIHVybDogJy9hcGkvYWJwL2ZlYXR1cmVzJyxcclxuICAgICAgcGFyYW1zLFxyXG4gICAgfTtcclxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxGZWF0dXJlTWFuYWdlbWVudC5Qcm92aWRlciwgRmVhdHVyZU1hbmFnZW1lbnQuRmVhdHVyZXM+KHJlcXVlc3QpO1xyXG4gIH1cclxuXHJcbiAgdXBkYXRlRmVhdHVyZXMoe1xyXG4gICAgZmVhdHVyZXMsXHJcbiAgICBwcm92aWRlcktleSxcclxuICAgIHByb3ZpZGVyTmFtZSxcclxuICB9OiBGZWF0dXJlTWFuYWdlbWVudC5Qcm92aWRlciAmIEZlYXR1cmVNYW5hZ2VtZW50LkZlYXR1cmVzKTogT2JzZXJ2YWJsZTxudWxsPiB7XHJcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8RmVhdHVyZU1hbmFnZW1lbnQuRmVhdHVyZXM+ID0ge1xyXG4gICAgICBtZXRob2Q6ICdQVVQnLFxyXG4gICAgICB1cmw6ICcvYXBpL2FicC9mZWF0dXJlcycsXHJcbiAgICAgIGJvZHk6IHsgZmVhdHVyZXMgfSxcclxuICAgICAgcGFyYW1zOiB7IHByb3ZpZGVyS2V5LCBwcm92aWRlck5hbWUgfSxcclxuICAgIH07XHJcbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8RmVhdHVyZU1hbmFnZW1lbnQuRmVhdHVyZXMsIG51bGw+KHJlcXVlc3QpO1xyXG4gIH1cclxufVxyXG4iXX0=