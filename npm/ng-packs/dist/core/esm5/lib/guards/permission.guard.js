/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import { tap } from 'rxjs/operators';
import { RestOccurError } from '../actions';
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
        var _this = this;
        var data = _a.data;
        /** @type {?} */
        var resource = (/** @type {?} */ (data.requiredPolicy));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi5ndWFyZC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9ndWFyZHMvcGVybWlzc2lvbi5ndWFyZC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBRXBDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxXQUFXLENBQUM7QUFDeEMsT0FBTyxFQUFFLEdBQUcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3JDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxZQUFZLENBQUM7OztBQUU1QztJQUlFLHlCQUFvQixLQUFZO1FBQVosVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7O0lBRXBDLHFDQUFXOzs7O0lBQVgsVUFBWSxFQUFnQztRQUE1QyxpQkFTQztZQVRhLGNBQUk7O1lBQ1YsUUFBUSxHQUFHLG1CQUFBLElBQUksQ0FBQyxjQUFjLEVBQVU7UUFDOUMsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLE1BQU0sQ0FBQyxXQUFXLENBQUMsZ0JBQWdCLENBQUMsUUFBUSxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQ25FLEdBQUc7Ozs7UUFBQyxVQUFBLE1BQU07WUFDUixJQUFJLENBQUMsTUFBTSxFQUFFO2dCQUNYLEtBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksY0FBYyxDQUFDLEVBQUUsTUFBTSxFQUFFLEdBQUcsRUFBRSxDQUFDLENBQUMsQ0FBQzthQUMxRDtRQUNILENBQUMsRUFBQyxDQUNILENBQUM7SUFDSixDQUFDOztnQkFmRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQVJRLEtBQUs7OzswQkFGZDtDQXdCQyxBQWhCRCxJQWdCQztTQWJZLGVBQWU7Ozs7OztJQUNkLGdDQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEFjdGl2YXRlZFJvdXRlU25hcHNob3QsIENhbkFjdGl2YXRlIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMnO1xuaW1wb3J0IHsgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHsgUmVzdE9jY3VyRXJyb3IgfSBmcm9tICcuLi9hY3Rpb25zJztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIFBlcm1pc3Npb25HdWFyZCBpbXBsZW1lbnRzIENhbkFjdGl2YXRlIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgY2FuQWN0aXZhdGUoeyBkYXRhIH06IEFjdGl2YXRlZFJvdXRlU25hcHNob3QpOiBPYnNlcnZhYmxlPGJvb2xlYW4+IHtcbiAgICBjb25zdCByZXNvdXJjZSA9IGRhdGEucmVxdWlyZWRQb2xpY3kgYXMgc3RyaW5nO1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdChDb25maWdTdGF0ZS5nZXRHcmFudGVkUG9saWN5KHJlc291cmNlKSkucGlwZShcbiAgICAgIHRhcChhY2Nlc3MgPT4ge1xuICAgICAgICBpZiAoIWFjY2Vzcykge1xuICAgICAgICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFJlc3RPY2N1ckVycm9yKHsgc3RhdHVzOiA0MDMgfSkpO1xuICAgICAgICB9XG4gICAgICB9KSxcbiAgICApO1xuICB9XG59XG4iXX0=