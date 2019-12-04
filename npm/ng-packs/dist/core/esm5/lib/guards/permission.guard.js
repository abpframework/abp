/**
 * @fileoverview added by tsickle
 * Generated from: lib/guards/permission.guard.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { tap } from 'rxjs/operators';
import snq from 'snq';
import { RestOccurError } from '../actions';
import { ConfigState } from '../states';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
var PermissionGuard = /** @class */ (function () {
    function PermissionGuard(store) {
        this.store = store;
    }
    /**
     * @param {?} route
     * @param {?} state
     * @return {?}
     */
    PermissionGuard.prototype.canActivate = /**
     * @param {?} route
     * @param {?} state
     * @return {?}
     */
    function (route, state) {
        var _this = this;
        /** @type {?} */
        var resource = snq((/**
         * @return {?}
         */
        function () { return route.data.routes.requiredPolicy; })) || snq((/**
         * @return {?}
         */
        function () { return (/** @type {?} */ (route.data.requiredPolicy)); }));
        if (!resource) {
            resource = snq((/**
             * @return {?}
             */
            function () { return route.routeConfig.children.find((/**
             * @param {?} child
             * @return {?}
             */
            function (child) { return state.url.indexOf(child.path) > -1; })).data.requiredPolicy; }));
        }
        return this.store.select(ConfigState.getGrantedPolicy(resource)).pipe(tap((/**
         * @param {?} access
         * @return {?}
         */
        function (access) {
            if (!access) {
                _this.store.dispatch(new RestOccurError({ status: 403 }));
            }
        })));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi5ndWFyZC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9ndWFyZHMvcGVybWlzc2lvbi5ndWFyZC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFM0MsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUVwQyxPQUFPLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDckMsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxZQUFZLENBQUM7QUFDNUMsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLFdBQVcsQ0FBQzs7O0FBRXhDO0lBSUUseUJBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQzs7Ozs7O0lBRXBDLHFDQUFXOzs7OztJQUFYLFVBQVksS0FBNkIsRUFBRSxLQUEwQjtRQUFyRSxpQkFlQzs7WUFkSyxRQUFRLEdBQUcsR0FBRzs7O1FBQUMsY0FBTSxPQUFBLEtBQUssQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLGNBQWMsRUFBaEMsQ0FBZ0MsRUFBQyxJQUFJLEdBQUc7OztRQUFDLHFCQUFNLG1CQUFBLEtBQUssQ0FBQyxJQUFJLENBQUMsY0FBYyxFQUFVLEdBQUEsRUFBQztRQUM1RyxJQUFJLENBQUMsUUFBUSxFQUFFO1lBQ2IsUUFBUSxHQUFHLEdBQUc7OztZQUNaLGNBQU0sT0FBQSxLQUFLLENBQUMsV0FBVyxDQUFDLFFBQVEsQ0FBQyxJQUFJOzs7O1lBQUMsVUFBQSxLQUFLLElBQUksT0FBQSxLQUFLLENBQUMsR0FBRyxDQUFDLE9BQU8sQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxDQUFDLEVBQWxDLENBQWtDLEVBQUMsQ0FBQyxJQUFJLENBQUMsY0FBYyxFQUFoRyxDQUFnRyxFQUN2RyxDQUFDO1NBQ0g7UUFFRCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsTUFBTSxDQUFDLFdBQVcsQ0FBQyxnQkFBZ0IsQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FDbkUsR0FBRzs7OztRQUFDLFVBQUEsTUFBTTtZQUNSLElBQUksQ0FBQyxNQUFNLEVBQUU7Z0JBQ1gsS0FBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxjQUFjLENBQUMsRUFBRSxNQUFNLEVBQUUsR0FBRyxFQUFFLENBQUMsQ0FBQyxDQUFDO2FBQzFEO1FBQ0gsQ0FBQyxFQUFDLENBQ0gsQ0FBQztJQUNKLENBQUM7O2dCQXJCRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQVRRLEtBQUs7OzswQkFGZDtDQStCQyxBQXRCRCxJQXNCQztTQW5CWSxlQUFlOzs7Ozs7SUFDZCxnQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90LCBDYW5BY3RpdmF0ZSwgUm91dGVyU3RhdGVTbmFwc2hvdCB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcbmltcG9ydCB7IFJlc3RPY2N1ckVycm9yIH0gZnJvbSAnLi4vYWN0aW9ucyc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcyc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBQZXJtaXNzaW9uR3VhcmQgaW1wbGVtZW50cyBDYW5BY3RpdmF0ZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIGNhbkFjdGl2YXRlKHJvdXRlOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90LCBzdGF0ZTogUm91dGVyU3RhdGVTbmFwc2hvdCk6IE9ic2VydmFibGU8Ym9vbGVhbj4ge1xuICAgIGxldCByZXNvdXJjZSA9IHNucSgoKSA9PiByb3V0ZS5kYXRhLnJvdXRlcy5yZXF1aXJlZFBvbGljeSkgfHwgc25xKCgpID0+IHJvdXRlLmRhdGEucmVxdWlyZWRQb2xpY3kgYXMgc3RyaW5nKTtcbiAgICBpZiAoIXJlc291cmNlKSB7XG4gICAgICByZXNvdXJjZSA9IHNucShcbiAgICAgICAgKCkgPT4gcm91dGUucm91dGVDb25maWcuY2hpbGRyZW4uZmluZChjaGlsZCA9PiBzdGF0ZS51cmwuaW5kZXhPZihjaGlsZC5wYXRoKSA+IC0xKS5kYXRhLnJlcXVpcmVkUG9saWN5LFxuICAgICAgKTtcbiAgICB9XG5cbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3QoQ29uZmlnU3RhdGUuZ2V0R3JhbnRlZFBvbGljeShyZXNvdXJjZSkpLnBpcGUoXG4gICAgICB0YXAoYWNjZXNzID0+IHtcbiAgICAgICAgaWYgKCFhY2Nlc3MpIHtcbiAgICAgICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBSZXN0T2NjdXJFcnJvcih7IHN0YXR1czogNDAzIH0pKTtcbiAgICAgICAgfVxuICAgICAgfSksXG4gICAgKTtcbiAgfVxufVxuIl19