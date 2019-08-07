/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
export class LocalizationService {
    /**
     * @param {?} store
     */
    constructor(store) {
        this.store = store;
    }
    /**
     * @param {?} keys
     * @param {...?} interpolateParams
     * @return {?}
     */
    get(keys, ...interpolateParams) {
        return this.store.select(ConfigState.getCopy(keys, ...interpolateParams));
    }
    /**
     * @param {?} keys
     * @param {...?} interpolateParams
     * @return {?}
     */
    instant(keys, ...interpolateParams) {
        return this.store.selectSnapshot(ConfigState.getCopy(keys, ...interpolateParams));
    }
}
LocalizationService.decorators = [
    { type: Injectable, args: [{ providedIn: 'root' },] }
];
/** @nocollapse */
LocalizationService.ctorParameters = () => [
    { type: Store }
];
/** @nocollapse */ LocalizationService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function LocalizationService_Factory() { return new LocalizationService(i0.ɵɵinject(i1.Store)); }, token: LocalizationService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    LocalizationService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvbG9jYWxpemF0aW9uLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sV0FBVyxDQUFDOzs7QUFJeEMsTUFBTSxPQUFPLG1CQUFtQjs7OztJQUM5QixZQUFvQixLQUFZO1FBQVosVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7OztJQUVwQyxHQUFHLENBQUMsSUFBWSxFQUFFLEdBQUcsaUJBQTJCO1FBQzlDLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxNQUFNLENBQUMsV0FBVyxDQUFDLE9BQU8sQ0FBQyxJQUFJLEVBQUUsR0FBRyxpQkFBaUIsQ0FBQyxDQUFDLENBQUM7SUFDNUUsQ0FBQzs7Ozs7O0lBRUQsT0FBTyxDQUFDLElBQVksRUFBRSxHQUFHLGlCQUEyQjtRQUNsRCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxPQUFPLENBQUMsSUFBSSxFQUFFLEdBQUcsaUJBQWlCLENBQUMsQ0FBQyxDQUFDO0lBQ3BGLENBQUM7OztZQVZGLFVBQVUsU0FBQyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUU7Ozs7WUFKekIsS0FBSzs7Ozs7Ozs7SUFNQSxvQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcblxuQEluamVjdGFibGUoeyBwcm92aWRlZEluOiAncm9vdCcgfSlcbmV4cG9ydCBjbGFzcyBMb2NhbGl6YXRpb25TZXJ2aWNlIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgZ2V0KGtleXM6IHN0cmluZywgLi4uaW50ZXJwb2xhdGVQYXJhbXM6IHN0cmluZ1tdKTogT2JzZXJ2YWJsZTxzdHJpbmc+IHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3QoQ29uZmlnU3RhdGUuZ2V0Q29weShrZXlzLCAuLi5pbnRlcnBvbGF0ZVBhcmFtcykpO1xuICB9XG5cbiAgaW5zdGFudChrZXlzOiBzdHJpbmcsIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXSk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0Q29weShrZXlzLCAuLi5pbnRlcnBvbGF0ZVBhcmFtcykpO1xuICB9XG59XG4iXX0=