/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { RestService } from './rest.service';
import * as i0 from "@angular/core";
import * as i1 from "./rest.service";
var ProfileService = /** @class */ (function () {
    function ProfileService(rest) {
        this.rest = rest;
    }
    /**
     * @return {?}
     */
    ProfileService.prototype.get = /**
     * @return {?}
     */
    function () {
        /** @type {?} */
        var request = {
            method: 'GET',
            url: '/api/identity/my-profile',
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} body
     * @return {?}
     */
    ProfileService.prototype.update = /**
     * @param {?} body
     * @return {?}
     */
    function (body) {
        /** @type {?} */
        var request = {
            method: 'PUT',
            url: '/api/identity/my-profile',
            body: body,
        };
        return this.rest.request(request);
    };
    /**
     * @param {?} body
     * @param {?=} skipHandleError
     * @return {?}
     */
    ProfileService.prototype.changePassword = /**
     * @param {?} body
     * @param {?=} skipHandleError
     * @return {?}
     */
    function (body, skipHandleError) {
        if (skipHandleError === void 0) { skipHandleError = false; }
        /** @type {?} */
        var request = {
            method: 'POST',
            url: '/api/identity/my-profile/change-password',
            body: body,
        };
        return this.rest.request(request, { skipHandleError: skipHandleError });
    };
    ProfileService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    ProfileService.ctorParameters = function () { return [
        { type: RestService }
    ]; };
    /** @nocollapse */ ProfileService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ProfileService_Factory() { return new ProfileService(i0.ɵɵinject(i1.RestService)); }, token: ProfileService, providedIn: "root" });
    return ProfileService;
}());
export { ProfileService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    ProfileService.prototype.rest;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3Byb2ZpbGUuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7OztBQUc3QztJQUlFLHdCQUFvQixJQUFpQjtRQUFqQixTQUFJLEdBQUosSUFBSSxDQUFhO0lBQUcsQ0FBQzs7OztJQUV6Qyw0QkFBRzs7O0lBQUg7O1lBQ1EsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSwwQkFBMEI7U0FDaEM7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUF5QixPQUFPLENBQUMsQ0FBQztJQUM1RCxDQUFDOzs7OztJQUVELCtCQUFNOzs7O0lBQU4sVUFBTyxJQUFzQjs7WUFDckIsT0FBTyxHQUFtQztZQUM5QyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSwwQkFBMEI7WUFDL0IsSUFBSSxNQUFBO1NBQ0w7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFxQyxPQUFPLENBQUMsQ0FBQztJQUN4RSxDQUFDOzs7Ozs7SUFFRCx1Q0FBYzs7Ozs7SUFBZCxVQUFlLElBQW1DLEVBQUUsZUFBZ0M7UUFBaEMsZ0NBQUEsRUFBQSx1QkFBZ0M7O1lBQzVFLE9BQU8sR0FBZ0Q7WUFDM0QsTUFBTSxFQUFFLE1BQU07WUFDZCxHQUFHLEVBQUUsMENBQTBDO1lBQy9DLElBQUksTUFBQTtTQUNMO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBc0MsT0FBTyxFQUFFLEVBQUUsZUFBZSxpQkFBQSxFQUFFLENBQUMsQ0FBQztJQUM5RixDQUFDOztnQkFqQ0YsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFMUSxXQUFXOzs7eUJBRnBCO0NBdUNDLEFBbENELElBa0NDO1NBL0JZLGNBQWM7Ozs7OztJQUNiLDhCQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IFJlc3RTZXJ2aWNlIH0gZnJvbSAnLi9yZXN0LnNlcnZpY2UnO1xuaW1wb3J0IHsgUHJvZmlsZSwgUmVzdCB9IGZyb20gJy4uL21vZGVscyc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBQcm9maWxlU2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVzdDogUmVzdFNlcnZpY2UpIHt9XG5cbiAgZ2V0KCk6IE9ic2VydmFibGU8UHJvZmlsZS5SZXNwb25zZT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxudWxsPiA9IHtcbiAgICAgIG1ldGhvZDogJ0dFVCcsXG4gICAgICB1cmw6ICcvYXBpL2lkZW50aXR5L215LXByb2ZpbGUnLFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8bnVsbCwgUHJvZmlsZS5SZXNwb25zZT4ocmVxdWVzdCk7XG4gIH1cblxuICB1cGRhdGUoYm9keTogUHJvZmlsZS5SZXNwb25zZSk6IE9ic2VydmFibGU8UHJvZmlsZS5SZXNwb25zZT4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxQcm9maWxlLlJlc3BvbnNlPiA9IHtcbiAgICAgIG1ldGhvZDogJ1BVVCcsXG4gICAgICB1cmw6ICcvYXBpL2lkZW50aXR5L215LXByb2ZpbGUnLFxuICAgICAgYm9keSxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFByb2ZpbGUuUmVzcG9uc2UsIFByb2ZpbGUuUmVzcG9uc2U+KHJlcXVlc3QpO1xuICB9XG5cbiAgY2hhbmdlUGFzc3dvcmQoYm9keTogUHJvZmlsZS5DaGFuZ2VQYXNzd29yZFJlcXVlc3QsIHNraXBIYW5kbGVFcnJvcjogYm9vbGVhbiA9IGZhbHNlKTogT2JzZXJ2YWJsZTxudWxsPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFByb2ZpbGUuQ2hhbmdlUGFzc3dvcmRSZXF1ZXN0PiA9IHtcbiAgICAgIG1ldGhvZDogJ1BPU1QnLFxuICAgICAgdXJsOiAnL2FwaS9pZGVudGl0eS9teS1wcm9maWxlL2NoYW5nZS1wYXNzd29yZCcsXG4gICAgICBib2R5LFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8UHJvZmlsZS5DaGFuZ2VQYXNzd29yZFJlcXVlc3QsIG51bGw+KHJlcXVlc3QsIHsgc2tpcEhhbmRsZUVycm9yIH0pO1xuICB9XG59XG4iXX0=