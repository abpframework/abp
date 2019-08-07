/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
var PermissionGuard = /** @class */ (function () {
    function PermissionGuard(store) {
        this.store = store;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    PermissionGuard.prototype.canActivate = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var data = _a.data;
        /** @type {?} */
        var resource = (/** @type {?} */ (data.requiredPolicy));
        return this.store.select(ConfigState.getGrantedPolicy(resource));
    };
    PermissionGuard.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    PermissionGuard.ctorParameters = function () { return [
        { type: Store }
    ]; };
    /** @nocollapse */ PermissionGuard.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function PermissionGuard_Factory() { return new PermissionGuard(i0.ɵɵinject(i1.Store)); }, token: PermissionGuard, providedIn: "root" });
    return PermissionGuard;
}());
export { PermissionGuard };
if (false) {
    /**
     * @type {?}
     * @private
     */
    PermissionGuard.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi5ndWFyZC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9ndWFyZHMvcGVybWlzc2lvbi5ndWFyZC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBRXBDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxXQUFXLENBQUM7OztBQUV4QztJQUlFLHlCQUFvQixLQUFZO1FBQVosVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7O0lBRXBDLHFDQUFXOzs7O0lBQVgsVUFBWSxFQUFnQztZQUE5QixjQUFJOztZQUNWLFFBQVEsR0FBRyxtQkFBQSxJQUFJLENBQUMsY0FBYyxFQUFVO1FBQzlDLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxNQUFNLENBQUMsV0FBVyxDQUFDLGdCQUFnQixDQUFDLFFBQVEsQ0FBQyxDQUFDLENBQUM7SUFDbkUsQ0FBQzs7Z0JBVEYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFOUSxLQUFLOzs7MEJBRmQ7Q0FnQkMsQUFWRCxJQVVDO1NBUFksZUFBZTs7Ozs7O0lBQ2QsZ0NBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQWN0aXZhdGVkUm91dGVTbmFwc2hvdCwgQ2FuQWN0aXZhdGUgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcyc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBQZXJtaXNzaW9uR3VhcmQgaW1wbGVtZW50cyBDYW5BY3RpdmF0ZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIGNhbkFjdGl2YXRlKHsgZGF0YSB9OiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90KTogT2JzZXJ2YWJsZTxib29sZWFuPiB7XG4gICAgY29uc3QgcmVzb3VyY2UgPSBkYXRhLnJlcXVpcmVkUG9saWN5IGFzIHN0cmluZztcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3QoQ29uZmlnU3RhdGUuZ2V0R3JhbnRlZFBvbGljeShyZXNvdXJjZSkpO1xuICB9XG59XG4iXX0=