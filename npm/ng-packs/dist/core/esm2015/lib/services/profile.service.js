/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
     * @param {?=} throwErr
     * @return {?}
     */
    changePassword(body, throwErr = false) {
        /** @type {?} */
        const request = {
            method: 'POST',
            url: '/api/identity/my-profile/change-password',
            body,
        };
        return this.rest.request(request, { throwErr });
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3Byb2ZpbGUuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7OztBQU03QyxNQUFNLE9BQU8sY0FBYzs7OztJQUN6QixZQUFvQixJQUFpQjtRQUFqQixTQUFJLEdBQUosSUFBSSxDQUFhO0lBQUcsQ0FBQzs7OztJQUV6QyxHQUFHOztjQUNLLE9BQU8sR0FBdUI7WUFDbEMsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEVBQUUsMEJBQTBCO1NBQ2hDO1FBRUQsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBeUIsT0FBTyxDQUFDLENBQUM7SUFDNUQsQ0FBQzs7Ozs7SUFFRCxNQUFNLENBQUMsSUFBc0I7O2NBQ3JCLE9BQU8sR0FBbUM7WUFDOUMsTUFBTSxFQUFFLEtBQUs7WUFDYixHQUFHLEVBQUUsMEJBQTBCO1lBQy9CLElBQUk7U0FDTDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQXFDLE9BQU8sQ0FBQyxDQUFDO0lBQ3hFLENBQUM7Ozs7OztJQUVELGNBQWMsQ0FBQyxJQUFtQyxFQUFFLFdBQW9CLEtBQUs7O2NBQ3JFLE9BQU8sR0FBZ0Q7WUFDM0QsTUFBTSxFQUFFLE1BQU07WUFDZCxHQUFHLEVBQUUsMENBQTBDO1lBQy9DLElBQUk7U0FDTDtRQUVELE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQXNDLE9BQU8sRUFBRSxFQUFFLFFBQVEsRUFBRSxDQUFDLENBQUM7SUFDdkYsQ0FBQzs7O1lBakNGLFVBQVUsU0FBQztnQkFDVixVQUFVLEVBQUUsTUFBTTthQUNuQjs7OztZQUxRLFdBQVc7Ozs7Ozs7O0lBT04sOEJBQXlCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgUmVzdFNlcnZpY2UgfSBmcm9tICcuL3Jlc3Quc2VydmljZSc7XG5pbXBvcnQgeyBQcm9maWxlLCBSZXN0IH0gZnJvbSAnLi4vbW9kZWxzJztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIFByb2ZpbGVTZXJ2aWNlIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZXN0OiBSZXN0U2VydmljZSkge31cblxuICBnZXQoKTogT2JzZXJ2YWJsZTxQcm9maWxlLlJlc3BvbnNlPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PG51bGw+ID0ge1xuICAgICAgbWV0aG9kOiAnR0VUJyxcbiAgICAgIHVybDogJy9hcGkvaWRlbnRpdHkvbXktcHJvZmlsZScsXG4gICAgfTtcblxuICAgIHJldHVybiB0aGlzLnJlc3QucmVxdWVzdDxudWxsLCBQcm9maWxlLlJlc3BvbnNlPihyZXF1ZXN0KTtcbiAgfVxuXG4gIHVwZGF0ZShib2R5OiBQcm9maWxlLlJlc3BvbnNlKTogT2JzZXJ2YWJsZTxQcm9maWxlLlJlc3BvbnNlPiB7XG4gICAgY29uc3QgcmVxdWVzdDogUmVzdC5SZXF1ZXN0PFByb2ZpbGUuUmVzcG9uc2U+ID0ge1xuICAgICAgbWV0aG9kOiAnUFVUJyxcbiAgICAgIHVybDogJy9hcGkvaWRlbnRpdHkvbXktcHJvZmlsZScsXG4gICAgICBib2R5LFxuICAgIH07XG5cbiAgICByZXR1cm4gdGhpcy5yZXN0LnJlcXVlc3Q8UHJvZmlsZS5SZXNwb25zZSwgUHJvZmlsZS5SZXNwb25zZT4ocmVxdWVzdCk7XG4gIH1cblxuICBjaGFuZ2VQYXNzd29yZChib2R5OiBQcm9maWxlLkNoYW5nZVBhc3N3b3JkUmVxdWVzdCwgdGhyb3dFcnI6IGJvb2xlYW4gPSBmYWxzZSk6IE9ic2VydmFibGU8bnVsbD4ge1xuICAgIGNvbnN0IHJlcXVlc3Q6IFJlc3QuUmVxdWVzdDxQcm9maWxlLkNoYW5nZVBhc3N3b3JkUmVxdWVzdD4gPSB7XG4gICAgICBtZXRob2Q6ICdQT1NUJyxcbiAgICAgIHVybDogJy9hcGkvaWRlbnRpdHkvbXktcHJvZmlsZS9jaGFuZ2UtcGFzc3dvcmQnLFxuICAgICAgYm9keSxcbiAgICB9O1xuXG4gICAgcmV0dXJuIHRoaXMucmVzdC5yZXF1ZXN0PFByb2ZpbGUuQ2hhbmdlUGFzc3dvcmRSZXF1ZXN0LCBudWxsPihyZXF1ZXN0LCB7IHRocm93RXJyIH0pO1xuICB9XG59XG4iXX0=