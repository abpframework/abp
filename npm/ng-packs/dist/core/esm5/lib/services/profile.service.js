/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/profile.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3Byb2ZpbGUuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFM0MsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLGdCQUFnQixDQUFDOzs7QUFHN0M7SUFJRSx3QkFBb0IsSUFBaUI7UUFBakIsU0FBSSxHQUFKLElBQUksQ0FBYTtJQUFHLENBQUM7Ozs7SUFFekMsNEJBQUc7OztJQUFIOztZQUNRLE9BQU8sR0FBdUI7WUFDbEMsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEVBQUUsMEJBQTBCO1NBQ2hDO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBeUIsT0FBTyxDQUFDLENBQUM7SUFDNUQsQ0FBQzs7Ozs7SUFFRCwrQkFBTTs7OztJQUFOLFVBQU8sSUFBc0I7O1lBQ3JCLE9BQU8sR0FBbUM7WUFDOUMsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEVBQUUsMEJBQTBCO1lBQy9CLElBQUksTUFBQTtTQUNMO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBcUMsT0FBTyxDQUFDLENBQUM7SUFDeEUsQ0FBQzs7Ozs7O0lBRUQsdUNBQWM7Ozs7O0lBQWQsVUFBZSxJQUFtQyxFQUFFLGVBQWdDO1FBQWhDLGdDQUFBLEVBQUEsdUJBQWdDOztZQUM1RSxPQUFPLEdBQWdEO1lBQzNELE1BQU0sRUFBRSxNQUFNO1lBQ2QsR0FBRyxFQUFFLDBDQUEwQztZQUMvQyxJQUFJLE1BQUE7U0FDTDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQXNDLE9BQU8sRUFBRSxFQUFFLGVBQWUsaUJBQUEsRUFBRSxDQUFDLENBQUM7SUFDOUYsQ0FBQzs7Z0JBakNGLFVBQVUsU0FBQztvQkFDVixVQUFVLEVBQUUsTUFBTTtpQkFDbkI7Ozs7Z0JBTFEsV0FBVzs7O3lCQUZwQjtDQXVDQyxBQWxDRCxJQWtDQztTQS9CWSxjQUFjOzs7Ozs7SUFDYiw4QkFBeUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBSZXN0U2VydmljZSB9IGZyb20gJy4vcmVzdC5zZXJ2aWNlJztcbmltcG9ydCB7IFByb2ZpbGUsIFJlc3QgfSBmcm9tICcuLi9tb2RlbHMnO1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290Jyxcbn0pXG5leHBvcnQgY2xhc3MgUHJvZmlsZVNlcnZpY2Uge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJlc3Q6IFJlc3RTZXJ2aWNlKSB7fVxuXG4gIGdldCgpOiBPYnNlcnZhYmxlPFByb2ZpbGUuUmVzcG9uc2U+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8bnVsbD4gPSB7XG4gICAgICBtZXRob2Q6ICdHRVQnLFxuICAgICAgdXJsOiAnL2FwaS9pZGVudGl0eS9teS1wcm9maWxlJyxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PG51bGwsIFByb2ZpbGUuUmVzcG9uc2U+KHJlcXVlc3QpO1xuICB9XG5cbiAgdXBkYXRlKGJvZHk6IFByb2ZpbGUuUmVzcG9uc2UpOiBPYnNlcnZhYmxlPFByb2ZpbGUuUmVzcG9uc2U+IHtcbiAgICBjb25zdCByZXF1ZXN0OiBSZXN0LlJlcXVlc3Q8UHJvZmlsZS5SZXNwb25zZT4gPSB7XG4gICAgICBtZXRob2Q6ICdQVVQnLFxuICAgICAgdXJsOiAnL2FwaS9pZGVudGl0eS9teS1wcm9maWxlJyxcbiAgICAgIGJvZHksXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxQcm9maWxlLlJlc3BvbnNlLCBQcm9maWxlLlJlc3BvbnNlPihyZXF1ZXN0KTtcbiAgfVxuXG4gIGNoYW5nZVBhc3N3b3JkKGJvZHk6IFByb2ZpbGUuQ2hhbmdlUGFzc3dvcmRSZXF1ZXN0LCBza2lwSGFuZGxlRXJyb3I6IGJvb2xlYW4gPSBmYWxzZSk6IE9ic2VydmFibGU8bnVsbD4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxQcm9maWxlLkNoYW5nZVBhc3N3b3JkUmVxdWVzdD4gPSB7XG4gICAgICBtZXRob2Q6ICdQT1NUJyxcbiAgICAgIHVybDogJy9hcGkvaWRlbnRpdHkvbXktcHJvZmlsZS9jaGFuZ2UtcGFzc3dvcmQnLFxuICAgICAgYm9keSxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFByb2ZpbGUuQ2hhbmdlUGFzc3dvcmRSZXF1ZXN0LCBudWxsPihyZXF1ZXN0LCB7IHNraXBIYW5kbGVFcnJvciB9KTtcbiAgfVxufVxuIl19