/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { GetUsers } from '../actions/identity.actions';
import { IdentityState } from '../states/identity.state';
var UserResolver = /** @class */ (function () {
    function UserResolver(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    UserResolver.prototype.resolve = /**
     * @return {?}
     */
    function () {
        /** @type {?} */
        var users = this.store.selectSnapshot(IdentityState.getUsers);
        return users && users.length ? null : this.store.dispatch(new GetUsers());
    };
    UserResolver.decorators = [
        { type: Injectable }
    ];
    /** @nocollapse */
    UserResolver.ctorParameters = function () { return [
        { type: Store }
    ]; };
    return UserResolver;
}());
export { UserResolver };
if (false) {
    /**
     * @type {?}
     * @private
     */
    UserResolver.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidXNlcnMucmVzb2x2ZXIuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL3Jlc29sdmVycy91c2Vycy5yZXNvbHZlci50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSw2QkFBNkIsQ0FBQztBQUV2RCxPQUFPLEVBQUUsYUFBYSxFQUFFLE1BQU0sMEJBQTBCLENBQUM7QUFFekQ7SUFFRSxzQkFBb0IsS0FBWTtRQUFaLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7O0lBRXBDLDhCQUFPOzs7SUFBUDs7WUFDUSxLQUFLLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsYUFBYSxDQUFDLFFBQVEsQ0FBQztRQUMvRCxPQUFPLEtBQUssSUFBSSxLQUFLLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksUUFBUSxFQUFFLENBQUMsQ0FBQztJQUM1RSxDQUFDOztnQkFQRixVQUFVOzs7O2dCQUxGLEtBQUs7O0lBYWQsbUJBQUM7Q0FBQSxBQVJELElBUUM7U0FQWSxZQUFZOzs7Ozs7SUFDWCw2QkFBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBSZXNvbHZlIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgR2V0VXNlcnMgfSBmcm9tICcuLi9hY3Rpb25zL2lkZW50aXR5LmFjdGlvbnMnO1xuaW1wb3J0IHsgSWRlbnRpdHkgfSBmcm9tICcuLi9tb2RlbHMvaWRlbnRpdHknO1xuaW1wb3J0IHsgSWRlbnRpdHlTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcy9pZGVudGl0eS5zdGF0ZSc7XG5cbkBJbmplY3RhYmxlKClcbmV4cG9ydCBjbGFzcyBVc2VyUmVzb2x2ZXIgaW1wbGVtZW50cyBSZXNvbHZlPElkZW50aXR5LlN0YXRlPiB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIHJlc29sdmUoKSB7XG4gICAgY29uc3QgdXNlcnMgPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KElkZW50aXR5U3RhdGUuZ2V0VXNlcnMpO1xuICAgIHJldHVybiB1c2VycyAmJiB1c2Vycy5sZW5ndGggPyBudWxsIDogdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgR2V0VXNlcnMoKSk7XG4gIH1cbn1cbiJdfQ==