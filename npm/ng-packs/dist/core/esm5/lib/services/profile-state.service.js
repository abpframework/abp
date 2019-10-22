/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS1zdGF0ZS5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3Byb2ZpbGUtc3RhdGUuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxXQUFXLENBQUM7OztBQUV6QztJQUlFLDZCQUFvQixLQUFZO1FBQVosVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7SUFFcEMsd0NBQVU7OztJQUFWO1FBQ0UsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxZQUFZLENBQUMsVUFBVSxDQUFDLENBQUM7SUFDNUQsQ0FBQzs7Z0JBUkYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFMUSxLQUFLOzs7OEJBRGQ7Q0FhQyxBQVRELElBU0M7U0FOWSxtQkFBbUI7Ozs7OztJQUNsQixvQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBQcm9maWxlU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMnO1xyXG5cclxuQEluamVjdGFibGUoe1xyXG4gIHByb3ZpZGVkSW46ICdyb290JyxcclxufSlcclxuZXhwb3J0IGNsYXNzIFByb2ZpbGVTdGF0ZVNlcnZpY2Uge1xyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxyXG5cclxuICBnZXRQcm9maWxlKCkge1xyXG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoUHJvZmlsZVN0YXRlLmdldFByb2ZpbGUpO1xyXG4gIH1cclxufVxyXG4iXX0=