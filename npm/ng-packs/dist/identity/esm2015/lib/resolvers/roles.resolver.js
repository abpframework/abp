/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { GetRoles } from '../actions/identity.actions';
import { IdentityState } from '../states/identity.state';
export class RoleResolver {
    /**
     * @param {?} store
     */
    constructor(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    resolve() {
        /** @type {?} */
        const roles = this.store.selectSnapshot(IdentityState.getRoles);
        return roles && roles.length ? null : this.store.dispatch(new GetRoles());
    }
}
RoleResolver.decorators = [
    { type: Injectable }
];
/** @nocollapse */
RoleResolver.ctorParameters = () => [
    { type: Store }
];
if (false) {
    /**
     * @type {?}
     * @private
     */
    RoleResolver.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm9sZXMucmVzb2x2ZXIuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL3Jlc29sdmVycy9yb2xlcy5yZXNvbHZlci50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSw2QkFBNkIsQ0FBQztBQUV2RCxPQUFPLEVBQUUsYUFBYSxFQUFFLE1BQU0sMEJBQTBCLENBQUM7QUFHekQsTUFBTSxPQUFPLFlBQVk7Ozs7SUFDdkIsWUFBb0IsS0FBWTtRQUFaLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7O0lBRXBDLE9BQU87O2NBQ0MsS0FBSyxHQUFHLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLGFBQWEsQ0FBQyxRQUFRLENBQUM7UUFDL0QsT0FBTyxLQUFLLElBQUksS0FBSyxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLFFBQVEsRUFBRSxDQUFDLENBQUM7SUFDNUUsQ0FBQzs7O1lBUEYsVUFBVTs7OztZQUxGLEtBQUs7Ozs7Ozs7SUFPQSw2QkFBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBSZXNvbHZlIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgR2V0Um9sZXMgfSBmcm9tICcuLi9hY3Rpb25zL2lkZW50aXR5LmFjdGlvbnMnO1xuaW1wb3J0IHsgSWRlbnRpdHkgfSBmcm9tICcuLi9tb2RlbHMvaWRlbnRpdHknO1xuaW1wb3J0IHsgSWRlbnRpdHlTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcy9pZGVudGl0eS5zdGF0ZSc7XG5cbkBJbmplY3RhYmxlKClcbmV4cG9ydCBjbGFzcyBSb2xlUmVzb2x2ZXIgaW1wbGVtZW50cyBSZXNvbHZlPElkZW50aXR5LlN0YXRlPiB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIHJlc29sdmUoKSB7XG4gICAgY29uc3Qgcm9sZXMgPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KElkZW50aXR5U3RhdGUuZ2V0Um9sZXMpO1xuICAgIHJldHVybiByb2xlcyAmJiByb2xlcy5sZW5ndGggPyBudWxsIDogdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgR2V0Um9sZXMoKSk7XG4gIH1cbn1cbiJdfQ==