/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/profile.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { RestService } from './rest.service';
import * as i0 from "@angular/core";
import * as i1 from "./rest.service";
export class ProfileService {
    /**
     * @param {?} rest
     */
    constructor(rest) {
        this.rest = rest;
    }
    /**
     * @return {?}
     */
    get() {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: '/api/identity/my-profile',
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} body
     * @return {?}
     */
    update(body) {
        /** @type {?} */
        const request = {
            method: 'PUT',
            url: '/api/identity/my-profile',
            body,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} body
     * @param {?=} skipHandleError
     * @return {?}
     */
    changePassword(body, skipHandleError = false) {
        /** @type {?} */
        const request = {
            method: 'POST',
            url: '/api/identity/my-profile/change-password',
            body,
        };
        return this.rest.request(request, { skipHandleError });
    }
}
ProfileService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
ProfileService.ctorParameters = () => [
    { type: RestService }
];
/** @nocollapse */ ProfileService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ProfileService_Factory() { return new ProfileService(i0.ɵɵinject(i1.RestService)); }, token: ProfileService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    ProfileService.prototype.rest;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3Byb2ZpbGUuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFM0MsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLGdCQUFnQixDQUFDOzs7QUFNN0MsTUFBTSxPQUFPLGNBQWM7Ozs7SUFDekIsWUFBb0IsSUFBaUI7UUFBakIsU0FBSSxHQUFKLElBQUksQ0FBYTtJQUFHLENBQUM7Ozs7SUFFekMsR0FBRzs7Y0FDSyxPQUFPLEdBQXVCO1lBQ2xDLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRyxFQUFFLDBCQUEwQjtTQUNoQztRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQXlCLE9BQU8sQ0FBQyxDQUFDO0lBQzVELENBQUM7Ozs7O0lBRUQsTUFBTSxDQUFDLElBQXNCOztjQUNyQixPQUFPLEdBQW1DO1lBQzlDLE1BQU0sRUFBRSxLQUFLO1lBQ2IsR0FBRyxFQUFFLDBCQUEwQjtZQUMvQixJQUFJO1NBQ0w7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFxQyxPQUFPLENBQUMsQ0FBQztJQUN4RSxDQUFDOzs7Ozs7SUFFRCxjQUFjLENBQUMsSUFBbUMsRUFBRSxrQkFBMkIsS0FBSzs7Y0FDNUUsT0FBTyxHQUFnRDtZQUMzRCxNQUFNLEVBQUUsTUFBTTtZQUNkLEdBQUcsRUFBRSwwQ0FBMEM7WUFDL0MsSUFBSTtTQUNMO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBc0MsT0FBTyxFQUFFLEVBQUUsZUFBZSxFQUFFLENBQUMsQ0FBQztJQUM5RixDQUFDOzs7WUFqQ0YsVUFBVSxTQUFDO2dCQUNWLFVBQVUsRUFBRSxNQUFNO2FBQ25COzs7O1lBTFEsV0FBVzs7Ozs7Ozs7SUFPTiw4QkFBeUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgUmVzdFNlcnZpY2UgfSBmcm9tICcuL3Jlc3Quc2VydmljZSc7XHJcbmltcG9ydCB7IFByb2ZpbGUsIFJlc3QgfSBmcm9tICcuLi9tb2RlbHMnO1xyXG5cclxuQEluamVjdGFibGUoe1xyXG4gIHByb3ZpZGVkSW46ICdyb290JyxcclxufSlcclxuZXhwb3J0IGNsYXNzIFByb2ZpbGVTZXJ2aWNlIHtcclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJlc3Q6IFJlc3RTZXJ2aWNlKSB7fVxyXG5cclxuICBnZXQoKTogT2JzZXJ2YWJsZTxQcm9maWxlLlJlc3BvbnNlPiB7XHJcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8bnVsbD4gPSB7XHJcbiAgICAgIG1ldGhvZDogJ0dFVCcsXHJcbiAgICAgIHVybDogJy9hcGkvaWRlbnRpdHkvbXktcHJvZmlsZScsXHJcbiAgICB9O1xyXG5cclxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxudWxsLCBQcm9maWxlLlJlc3BvbnNlPihyZXF1ZXN0KTtcclxuICB9XHJcblxyXG4gIHVwZGF0ZShib2R5OiBQcm9maWxlLlJlc3BvbnNlKTogT2JzZXJ2YWJsZTxQcm9maWxlLlJlc3BvbnNlPiB7XHJcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8UHJvZmlsZS5SZXNwb25zZT4gPSB7XHJcbiAgICAgIG1ldGhvZDogJ1BVVCcsXHJcbiAgICAgIHVybDogJy9hcGkvaWRlbnRpdHkvbXktcHJvZmlsZScsXHJcbiAgICAgIGJvZHksXHJcbiAgICB9O1xyXG5cclxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxQcm9maWxlLlJlc3BvbnNlLCBQcm9maWxlLlJlc3BvbnNlPihyZXF1ZXN0KTtcclxuICB9XHJcblxyXG4gIGNoYW5nZVBhc3N3b3JkKGJvZHk6IFByb2ZpbGUuQ2hhbmdlUGFzc3dvcmRSZXF1ZXN0LCBza2lwSGFuZGxlRXJyb3I6IGJvb2xlYW4gPSBmYWxzZSk6IE9ic2VydmFibGU8bnVsbD4ge1xyXG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFByb2ZpbGUuQ2hhbmdlUGFzc3dvcmRSZXF1ZXN0PiA9IHtcclxuICAgICAgbWV0aG9kOiAnUE9TVCcsXHJcbiAgICAgIHVybDogJy9hcGkvaWRlbnRpdHkvbXktcHJvZmlsZS9jaGFuZ2UtcGFzc3dvcmQnLFxyXG4gICAgICBib2R5LFxyXG4gICAgfTtcclxuXHJcbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8UHJvZmlsZS5DaGFuZ2VQYXNzd29yZFJlcXVlc3QsIG51bGw+KHJlcXVlc3QsIHsgc2tpcEhhbmRsZUVycm9yIH0pO1xyXG4gIH1cclxufVxyXG4iXX0=