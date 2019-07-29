/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { IdentityGetRoles } from '../actions/identity.actions';
import { IdentityState } from '../states/identity.state';
var RoleResolver = /** @class */ (function () {
    function RoleResolver(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    RoleResolver.prototype.resolve = /**
     * @return {?}
     */
    function () {
        /** @type {?} */
        var roles = this.store.selectSnapshot(IdentityState.getRoles);
        return roles && roles.length ? null : this.store.dispatch(new IdentityGetRoles());
    };
    RoleResolver.decorators = [
        { type: Injectable }
    ];
    /** @nocollapse */
    RoleResolver.ctorParameters = function () { return [
        { type: Store }
    ]; };
    return RoleResolver;
}());
export { RoleResolver };
if (false) {
    /**
     * @type {?}
     * @private
     */
    RoleResolver.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm9sZXMucmVzb2x2ZXIuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL3Jlc29sdmVycy9yb2xlcy5yZXNvbHZlci50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLDZCQUE2QixDQUFDO0FBRS9ELE9BQU8sRUFBRSxhQUFhLEVBQUUsTUFBTSwwQkFBMEIsQ0FBQztBQUV6RDtJQUVFLHNCQUFvQixLQUFZO1FBQVosVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7SUFFcEMsOEJBQU87OztJQUFQOztZQUNRLEtBQUssR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxhQUFhLENBQUMsUUFBUSxDQUFDO1FBQy9ELE9BQU8sS0FBSyxJQUFJLEtBQUssQ0FBQyxNQUFNLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxnQkFBZ0IsRUFBRSxDQUFDLENBQUM7SUFDcEYsQ0FBQzs7Z0JBUEYsVUFBVTs7OztnQkFMRixLQUFLOztJQWFkLG1CQUFDO0NBQUEsQUFSRCxJQVFDO1NBUFksWUFBWTs7Ozs7O0lBQ1gsNkJBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgUmVzb2x2ZSB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IElkZW50aXR5R2V0Um9sZXMgfSBmcm9tICcuLi9hY3Rpb25zL2lkZW50aXR5LmFjdGlvbnMnO1xuaW1wb3J0IHsgSWRlbnRpdHkgfSBmcm9tICcuLi9tb2RlbHMvaWRlbnRpdHknO1xuaW1wb3J0IHsgSWRlbnRpdHlTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcy9pZGVudGl0eS5zdGF0ZSc7XG5cbkBJbmplY3RhYmxlKClcbmV4cG9ydCBjbGFzcyBSb2xlUmVzb2x2ZXIgaW1wbGVtZW50cyBSZXNvbHZlPElkZW50aXR5LlN0YXRlPiB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIHJlc29sdmUoKSB7XG4gICAgY29uc3Qgcm9sZXMgPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KElkZW50aXR5U3RhdGUuZ2V0Um9sZXMpO1xuICAgIHJldHVybiByb2xlcyAmJiByb2xlcy5sZW5ndGggPyBudWxsIDogdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgSWRlbnRpdHlHZXRSb2xlcygpKTtcbiAgfVxufVxuIl19