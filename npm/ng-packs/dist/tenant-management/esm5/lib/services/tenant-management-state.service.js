/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { TenantManagementState } from '../states/tenant-management.state';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
var TenantManagementStateService = /** @class */ (function () {
    function TenantManagementStateService(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    TenantManagementStateService.prototype.getTenants = /**
     * @return {?}
     */
    function () {
        return this.store.selectSnapshot(TenantManagementState.get);
    };
    /**
     * @return {?}
     */
    TenantManagementStateService.prototype.getTenantsTotalCount = /**
     * @return {?}
     */
    function () {
        return this.store.selectSnapshot(TenantManagementState.getTenantsTotalCount);
    };
    TenantManagementStateService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    TenantManagementStateService.ctorParameters = function () { return [
        { type: Store }
    ]; };
    /** @nocollapse */ TenantManagementStateService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function TenantManagementStateService_Factory() { return new TenantManagementStateService(i0.ɵɵinject(i1.Store)); }, token: TenantManagementStateService, providedIn: "root" });
    return TenantManagementStateService;
}());
export { TenantManagementStateService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    TenantManagementStateService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQtc3RhdGUuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGVuYW50LW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQtc3RhdGUuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLG1DQUFtQyxDQUFDOzs7QUFFMUU7SUFJRSxzQ0FBb0IsS0FBWTtRQUFaLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7O0lBRXBDLGlEQUFVOzs7SUFBVjtRQUNFLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMscUJBQXFCLENBQUMsR0FBRyxDQUFDLENBQUM7SUFDOUQsQ0FBQzs7OztJQUVELDJEQUFvQjs7O0lBQXBCO1FBQ0UsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxxQkFBcUIsQ0FBQyxvQkFBb0IsQ0FBQyxDQUFDO0lBQy9FLENBQUM7O2dCQVpGLFVBQVUsU0FBQztvQkFDVixVQUFVLEVBQUUsTUFBTTtpQkFDbkI7Ozs7Z0JBTFEsS0FBSzs7O3VDQURkO0NBaUJDLEFBYkQsSUFhQztTQVZZLDRCQUE0Qjs7Ozs7O0lBQzNCLDZDQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnRTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcy90ZW5hbnQtbWFuYWdlbWVudC5zdGF0ZSc7XHJcblxyXG5ASW5qZWN0YWJsZSh7XHJcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgVGVuYW50TWFuYWdlbWVudFN0YXRlU2VydmljZSB7XHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XHJcblxyXG4gIGdldFRlbmFudHMoKSB7XHJcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChUZW5hbnRNYW5hZ2VtZW50U3RhdGUuZ2V0KTtcclxuICB9XHJcblxyXG4gIGdldFRlbmFudHNUb3RhbENvdW50KCkge1xyXG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoVGVuYW50TWFuYWdlbWVudFN0YXRlLmdldFRlbmFudHNUb3RhbENvdW50KTtcclxuICB9XHJcbn1cclxuIl19