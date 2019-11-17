/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/profile-state.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { ProfileState } from '../states';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
var ProfileStateService = /** @class */ (function () {
    function ProfileStateService(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    ProfileStateService.prototype.getProfile = /**
     * @return {?}
     */
    function () {
        return this.store.selectSnapshot(ProfileState.getProfile);
    };
    ProfileStateService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    ProfileStateService.ctorParameters = function () { return [
        { type: Store }
    ]; };
    /** @nocollapse */ ProfileStateService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ProfileStateService_Factory() { return new ProfileStateService(i0.ɵɵinject(i1.Store)); }, token: ProfileStateService, providedIn: "root" });
    return ProfileStateService;
}());
export { ProfileStateService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    ProfileStateService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS1zdGF0ZS5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3Byb2ZpbGUtc3RhdGUuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sV0FBVyxDQUFDOzs7QUFFekM7SUFJRSw2QkFBb0IsS0FBWTtRQUFaLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7O0lBRXBDLHdDQUFVOzs7SUFBVjtRQUNFLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsWUFBWSxDQUFDLFVBQVUsQ0FBQyxDQUFDO0lBQzVELENBQUM7O2dCQVJGLFVBQVUsU0FBQztvQkFDVixVQUFVLEVBQUUsTUFBTTtpQkFDbkI7Ozs7Z0JBTFEsS0FBSzs7OzhCQURkO0NBYUMsQUFURCxJQVNDO1NBTlksbUJBQW1COzs7Ozs7SUFDbEIsb0NBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgUHJvZmlsZVN0YXRlIH0gZnJvbSAnLi4vc3RhdGVzJztcclxuXHJcbkBJbmplY3RhYmxlKHtcclxuICBwcm92aWRlZEluOiAncm9vdCcsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBQcm9maWxlU3RhdGVTZXJ2aWNlIHtcclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cclxuXHJcbiAgZ2V0UHJvZmlsZSgpIHtcclxuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFByb2ZpbGVTdGF0ZS5nZXRQcm9maWxlKTtcclxuICB9XHJcbn1cclxuIl19