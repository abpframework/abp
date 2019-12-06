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
export class PermissionGuard {
    /**
     * @param {?} store
     */
    constructor(store) {
        this.store = store;
    }
    /**
     * @param {?} route
     * @param {?} state
     * @return {?}
     */
    canActivate(route, state) {
        /** @type {?} */
        let resource = snq((/**
         * @return {?}
         */
        () => route.data.routes.requiredPolicy)) || snq((/**
         * @return {?}
         */
        () => (/** @type {?} */ (route.data.requiredPolicy))));
        if (!resource) {
            resource = snq((/**
             * @return {?}
             */
            () => route.routeConfig.children.find((/**
             * @param {?} child
             * @return {?}
             */
            child => state.url.indexOf(child.path) > -1)).data.requiredPolicy));
        }
        return this.store.select(ConfigState.getGrantedPolicy(resource)).pipe(tap((/**
         * @param {?} access
         * @return {?}
         */
        access => {
            if (!access) {
                this.store.dispatch(new RestOccurError({ status: 403 }));
            }
        })));
    }
}
PermissionGuard.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
PermissionGuard.ctorParameters = () => [
    { type: Store }
];
/** @nocollapse */ PermissionGuard.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function PermissionGuard_Factory() { return new PermissionGuard(i0.ɵɵinject(i1.Store)); }, token: PermissionGuard, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    PermissionGuard.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi5ndWFyZC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9ndWFyZHMvcGVybWlzc2lvbi5ndWFyZC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFM0MsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUVwQyxPQUFPLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDckMsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxZQUFZLENBQUM7QUFDNUMsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLFdBQVcsQ0FBQzs7O0FBS3hDLE1BQU0sT0FBTyxlQUFlOzs7O0lBQzFCLFlBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQzs7Ozs7O0lBRXBDLFdBQVcsQ0FBQyxLQUE2QixFQUFFLEtBQTBCOztZQUMvRCxRQUFRLEdBQUcsR0FBRzs7O1FBQUMsR0FBRyxFQUFFLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsY0FBYyxFQUFDLElBQUksR0FBRzs7O1FBQUMsR0FBRyxFQUFFLENBQUMsbUJBQUEsS0FBSyxDQUFDLElBQUksQ0FBQyxjQUFjLEVBQVUsRUFBQztRQUM1RyxJQUFJLENBQUMsUUFBUSxFQUFFO1lBQ2IsUUFBUSxHQUFHLEdBQUc7OztZQUNaLEdBQUcsRUFBRSxDQUFDLEtBQUssQ0FBQyxXQUFXLENBQUMsUUFBUSxDQUFDLElBQUk7Ozs7WUFBQyxLQUFLLENBQUMsRUFBRSxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsT0FBTyxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLENBQUMsRUFBQyxDQUFDLElBQUksQ0FBQyxjQUFjLEVBQ3ZHLENBQUM7U0FDSDtRQUVELE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxNQUFNLENBQUMsV0FBVyxDQUFDLGdCQUFnQixDQUFDLFFBQVEsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUNuRSxHQUFHOzs7O1FBQUMsTUFBTSxDQUFDLEVBQUU7WUFDWCxJQUFJLENBQUMsTUFBTSxFQUFFO2dCQUNYLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksY0FBYyxDQUFDLEVBQUUsTUFBTSxFQUFFLEdBQUcsRUFBRSxDQUFDLENBQUMsQ0FBQzthQUMxRDtRQUNILENBQUMsRUFBQyxDQUNILENBQUM7SUFDSixDQUFDOzs7WUFyQkYsVUFBVSxTQUFDO2dCQUNWLFVBQVUsRUFBRSxNQUFNO2FBQ25COzs7O1lBVFEsS0FBSzs7Ozs7Ozs7SUFXQSxnQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90LCBDYW5BY3RpdmF0ZSwgUm91dGVyU3RhdGVTbmFwc2hvdCB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcbmltcG9ydCB7IFJlc3RPY2N1ckVycm9yIH0gZnJvbSAnLi4vYWN0aW9ucyc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcyc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBQZXJtaXNzaW9uR3VhcmQgaW1wbGVtZW50cyBDYW5BY3RpdmF0ZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIGNhbkFjdGl2YXRlKHJvdXRlOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90LCBzdGF0ZTogUm91dGVyU3RhdGVTbmFwc2hvdCk6IE9ic2VydmFibGU8Ym9vbGVhbj4ge1xuICAgIGxldCByZXNvdXJjZSA9IHNucSgoKSA9PiByb3V0ZS5kYXRhLnJvdXRlcy5yZXF1aXJlZFBvbGljeSkgfHwgc25xKCgpID0+IHJvdXRlLmRhdGEucmVxdWlyZWRQb2xpY3kgYXMgc3RyaW5nKTtcbiAgICBpZiAoIXJlc291cmNlKSB7XG4gICAgICByZXNvdXJjZSA9IHNucShcbiAgICAgICAgKCkgPT4gcm91dGUucm91dGVDb25maWcuY2hpbGRyZW4uZmluZChjaGlsZCA9PiBzdGF0ZS51cmwuaW5kZXhPZihjaGlsZC5wYXRoKSA+IC0xKS5kYXRhLnJlcXVpcmVkUG9saWN5LFxuICAgICAgKTtcbiAgICB9XG5cbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3QoQ29uZmlnU3RhdGUuZ2V0R3JhbnRlZFBvbGljeShyZXNvdXJjZSkpLnBpcGUoXG4gICAgICB0YXAoYWNjZXNzID0+IHtcbiAgICAgICAgaWYgKCFhY2Nlc3MpIHtcbiAgICAgICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBSZXN0T2NjdXJFcnJvcih7IHN0YXR1czogNDAzIH0pKTtcbiAgICAgICAgfVxuICAgICAgfSksXG4gICAgKTtcbiAgfVxufVxuIl19