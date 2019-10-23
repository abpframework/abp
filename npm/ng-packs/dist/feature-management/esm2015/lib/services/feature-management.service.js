/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmVhdHVyZS1tYW5hZ2VtZW50LnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmZlYXR1cmUtbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9mZWF0dXJlLW1hbmFnZW1lbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsV0FBVyxFQUFRLE1BQU0sY0FBYyxDQUFDO0FBQ2pELE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7Ozs7QUFPcEMsTUFBTSxPQUFPLHdCQUF3Qjs7Ozs7SUFDbkMsWUFBb0IsSUFBaUIsRUFBVSxLQUFZO1FBQXZDLFNBQUksR0FBSixJQUFJLENBQWE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQzs7Ozs7SUFFL0QsV0FBVyxDQUFDLE1BQWtDOztjQUN0QyxPQUFPLEdBQXVCO1lBQ2xDLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRyxFQUFFLG1CQUFtQjtZQUN4QixNQUFNO1NBQ1A7UUFDRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUF5RCxPQUFPLENBQUMsQ0FBQztJQUM1RixDQUFDOzs7OztJQUVELGNBQWMsQ0FBQyxFQUNiLFFBQVEsRUFDUixXQUFXLEVBQ1gsWUFBWSxHQUM0Qzs7Y0FDbEQsT0FBTyxHQUE2QztZQUN4RCxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSxtQkFBbUI7WUFDeEIsSUFBSSxFQUFFLEVBQUUsUUFBUSxFQUFFO1lBQ2xCLE1BQU0sRUFBRSxFQUFFLFdBQVcsRUFBRSxZQUFZLEVBQUU7U0FDdEM7UUFDRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFtQyxPQUFPLENBQUMsQ0FBQztJQUN0RSxDQUFDOzs7WUEzQkYsVUFBVSxTQUFDO2dCQUNWLFVBQVUsRUFBRSxNQUFNO2FBQ25COzs7O1lBUFEsV0FBVztZQUNYLEtBQUs7Ozs7Ozs7O0lBUUEsd0NBQXlCOzs7OztJQUFFLHlDQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgUmVzdFNlcnZpY2UsIFJlc3QgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xyXG5pbXBvcnQgeyBGZWF0dXJlTWFuYWdlbWVudCB9IGZyb20gJy4uL21vZGVscyc7XHJcblxyXG5ASW5qZWN0YWJsZSh7XHJcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgRmVhdHVyZU1hbmFnZW1lbnRTZXJ2aWNlIHtcclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJlc3Q6IFJlc3RTZXJ2aWNlLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cclxuXHJcbiAgZ2V0RmVhdHVyZXMocGFyYW1zOiBGZWF0dXJlTWFuYWdlbWVudC5Qcm92aWRlcik6IE9ic2VydmFibGU8RmVhdHVyZU1hbmFnZW1lbnQuRmVhdHVyZXM+IHtcclxuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcclxuICAgICAgbWV0aG9kOiAnR0VUJyxcclxuICAgICAgdXJsOiAnL2FwaS9hYnAvZmVhdHVyZXMnLFxyXG4gICAgICBwYXJhbXMsXHJcbiAgICB9O1xyXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PEZlYXR1cmVNYW5hZ2VtZW50LlByb3ZpZGVyLCBGZWF0dXJlTWFuYWdlbWVudC5GZWF0dXJlcz4ocmVxdWVzdCk7XHJcbiAgfVxyXG5cclxuICB1cGRhdGVGZWF0dXJlcyh7XHJcbiAgICBmZWF0dXJlcyxcclxuICAgIHByb3ZpZGVyS2V5LFxyXG4gICAgcHJvdmlkZXJOYW1lLFxyXG4gIH06IEZlYXR1cmVNYW5hZ2VtZW50LlByb3ZpZGVyICYgRmVhdHVyZU1hbmFnZW1lbnQuRmVhdHVyZXMpOiBPYnNlcnZhYmxlPG51bGw+IHtcclxuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxGZWF0dXJlTWFuYWdlbWVudC5GZWF0dXJlcz4gPSB7XHJcbiAgICAgIG1ldGhvZDogJ1BVVCcsXHJcbiAgICAgIHVybDogJy9hcGkvYWJwL2ZlYXR1cmVzJyxcclxuICAgICAgYm9keTogeyBmZWF0dXJlcyB9LFxyXG4gICAgICBwYXJhbXM6IHsgcHJvdmlkZXJLZXksIHByb3ZpZGVyTmFtZSB9LFxyXG4gICAgfTtcclxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxGZWF0dXJlTWFuYWdlbWVudC5GZWF0dXJlcywgbnVsbD4ocmVxdWVzdCk7XHJcbiAgfVxyXG59XHJcbiJdfQ==