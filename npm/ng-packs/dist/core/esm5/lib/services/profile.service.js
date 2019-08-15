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
     * @param {?=} throwErr
     * @return {?}
     */
    ProfileService.prototype.changePassword = /**
     * @param {?} body
     * @param {?=} throwErr
     * @return {?}
     */
    function (body, throwErr) {
        if (throwErr === void 0) { throwErr = false; }
        /** @type {?} */
        var request = {
            method: 'POST',
            url: '/api/identity/my-profile/change-password',
            body: body,
        };
        return this.rest.request(request, { throwErr: throwErr });
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3Byb2ZpbGUuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7OztBQUc3QztJQUlFLHdCQUFvQixJQUFpQjtRQUFqQixTQUFJLEdBQUosSUFBSSxDQUFhO0lBQUcsQ0FBQzs7OztJQUV6Qyw0QkFBRzs7O0lBQUg7O1lBQ1EsT0FBTyxHQUF1QjtZQUNsQyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSwwQkFBMEI7U0FDaEM7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUF5QixPQUFPLENBQUMsQ0FBQztJQUM1RCxDQUFDOzs7OztJQUVELCtCQUFNOzs7O0lBQU4sVUFBTyxJQUFzQjs7WUFDckIsT0FBTyxHQUFtQztZQUM5QyxNQUFNLEVBQUUsS0FBSztZQUNiLEdBQUcsRUFBRSwwQkFBMEI7WUFDL0IsSUFBSSxNQUFBO1NBQ0w7UUFFRCxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFxQyxPQUFPLENBQUMsQ0FBQztJQUN4RSxDQUFDOzs7Ozs7SUFFRCx1Q0FBYzs7Ozs7SUFBZCxVQUFlLElBQW1DLEVBQUUsUUFBeUI7UUFBekIseUJBQUEsRUFBQSxnQkFBeUI7O1lBQ3JFLE9BQU8sR0FBZ0Q7WUFDM0QsTUFBTSxFQUFFLE1BQU07WUFDZCxHQUFHLEVBQUUsMENBQTBDO1lBQy9DLElBQUksTUFBQTtTQUNMO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBc0MsT0FBTyxFQUFFLEVBQUUsUUFBUSxVQUFBLEVBQUUsQ0FBQyxDQUFDO0lBQ3ZGLENBQUM7O2dCQWpDRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQUxRLFdBQVc7Ozt5QkFGcEI7Q0F1Q0MsQUFsQ0QsSUFrQ0M7U0EvQlksY0FBYzs7Ozs7O0lBQ2IsOEJBQXlCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgUmVzdFNlcnZpY2UgfSBmcm9tICcuL3Jlc3Quc2VydmljZSc7XG5pbXBvcnQgeyBQcm9maWxlLCBSZXN0IH0gZnJvbSAnLi4vbW9kZWxzJztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIFByb2ZpbGVTZXJ2aWNlIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZXN0OiBSZXN0U2VydmljZSkge31cblxuICBnZXQoKTogT2JzZXJ2YWJsZTxQcm9maWxlLlJlc3BvbnNlPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PG51bGw+ID0ge1xuICAgICAgbWV0aG9kOiAnR0VUJyxcbiAgICAgIHVybDogJy9hcGkvaWRlbnRpdHkvbXktcHJvZmlsZScsXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxudWxsLCBQcm9maWxlLlJlc3BvbnNlPihyZXF1ZXN0KTtcbiAgfVxuXG4gIHVwZGF0ZShib2R5OiBQcm9maWxlLlJlc3BvbnNlKTogT2JzZXJ2YWJsZTxQcm9maWxlLlJlc3BvbnNlPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFByb2ZpbGUuUmVzcG9uc2U+ID0ge1xuICAgICAgbWV0aG9kOiAnUFVUJyxcbiAgICAgIHVybDogJy9hcGkvaWRlbnRpdHkvbXktcHJvZmlsZScsXG4gICAgICBib2R5LFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8UHJvZmlsZS5SZXNwb25zZSwgUHJvZmlsZS5SZXNwb25zZT4ocmVxdWVzdCk7XG4gIH1cblxuICBjaGFuZ2VQYXNzd29yZChib2R5OiBQcm9maWxlLkNoYW5nZVBhc3N3b3JkUmVxdWVzdCwgdGhyb3dFcnI6IGJvb2xlYW4gPSBmYWxzZSk6IE9ic2VydmFibGU8bnVsbD4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxQcm9maWxlLkNoYW5nZVBhc3N3b3JkUmVxdWVzdD4gPSB7XG4gICAgICBtZXRob2Q6ICdQT1NUJyxcbiAgICAgIHVybDogJy9hcGkvaWRlbnRpdHkvbXktcHJvZmlsZS9jaGFuZ2UtcGFzc3dvcmQnLFxuICAgICAgYm9keSxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFByb2ZpbGUuQ2hhbmdlUGFzc3dvcmRSZXF1ZXN0LCBudWxsPihyZXF1ZXN0LCB7IHRocm93RXJyIH0pO1xuICB9XG59XG4iXX0=