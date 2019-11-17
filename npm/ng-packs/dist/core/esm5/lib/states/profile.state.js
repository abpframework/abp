/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/profile.state.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { tap } from 'rxjs/operators';
import { ChangePassword, GetProfile, UpdateProfile } from '../actions/profile.actions';
import { ProfileService } from '../services/profile.service';
var ProfileState = /** @class */ (function () {
    function ProfileState(profileService) {
        this.profileService = profileService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    ProfileState.getProfile = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var profile = _a.profile;
        return profile;
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    ProfileState.prototype.getProfile = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var patchState = _a.patchState;
        return this.profileService.get().pipe(tap((/**
         * @param {?} profile
         * @return {?}
         */
        function (profile) {
            return patchState({
                profile: profile,
            });
        })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    ProfileState.prototype.updateProfile = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState;
        var payload = _b.payload;
        return this.profileService.update(payload).pipe(tap((/**
         * @param {?} profile
         * @return {?}
         */
        function (profile) {
            return patchState({
                profile: profile,
            });
        })));
    };
    /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    ProfileState.prototype.changePassword = /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    function (_, _a) {
        var payload = _a.payload;
        return this.profileService.changePassword(payload, true);
    };
    ProfileState.ctorParameters = function () { return [
        { type: ProfileService }
    ]; };
    tslib_1.__decorate([
        Action(GetProfile),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object]),
        tslib_1.__metadata("design:returntype", void 0)
    ], ProfileState.prototype, "getProfile", null);
    tslib_1.__decorate([
        Action(UpdateProfile),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, UpdateProfile]),
        tslib_1.__metadata("design:returntype", void 0)
    ], ProfileState.prototype, "updateProfile", null);
    tslib_1.__decorate([
        Action(ChangePassword),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, ChangePassword]),
        tslib_1.__metadata("design:returntype", void 0)
    ], ProfileState.prototype, "changePassword", null);
    tslib_1.__decorate([
        Selector(),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object]),
        tslib_1.__metadata("design:returntype", Object)
    ], ProfileState, "getProfile", null);
    ProfileState = tslib_1.__decorate([
        State({
            name: 'ProfileState',
            defaults: (/** @type {?} */ ({})),
        }),
        tslib_1.__metadata("design:paramtypes", [ProfileService])
    ], ProfileState);
    return ProfileState;
}());
export { ProfileState };
if (false) {
    /**
     * @type {?}
     * @private
     */
    ProfileState.prototype.profileService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvcHJvZmlsZS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQWdCLE1BQU0sYUFBYSxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNyQyxPQUFPLEVBQUUsY0FBYyxFQUFFLFVBQVUsRUFBRSxhQUFhLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUV2RixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sNkJBQTZCLENBQUM7O0lBWTNELHNCQUFvQixjQUE4QjtRQUE5QixtQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7SUFBRyxDQUFDOzs7OztJQUovQyx1QkFBVTs7OztJQUFqQixVQUFrQixFQUEwQjtZQUF4QixvQkFBTztRQUN6QixPQUFPLE9BQU8sQ0FBQztJQUNqQixDQUFDOzs7OztJQUtELGlDQUFVOzs7O0lBQVYsVUFBVyxFQUEyQztZQUF6QywwQkFBVTtRQUNyQixPQUFPLElBQUksQ0FBQyxjQUFjLENBQUMsR0FBRyxFQUFFLENBQUMsSUFBSSxDQUNuQyxHQUFHOzs7O1FBQUMsVUFBQSxPQUFPO1lBQ1QsT0FBQSxVQUFVLENBQUM7Z0JBQ1QsT0FBTyxTQUFBO2FBQ1IsQ0FBQztRQUZGLENBRUUsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCxvQ0FBYTs7Ozs7SUFBYixVQUFjLEVBQTJDLEVBQUUsRUFBMEI7WUFBckUsMEJBQVU7WUFBbUMsb0JBQU87UUFDbEUsT0FBTyxJQUFJLENBQUMsY0FBYyxDQUFDLE1BQU0sQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQzdDLEdBQUc7Ozs7UUFBQyxVQUFBLE9BQU87WUFDVCxPQUFBLFVBQVUsQ0FBQztnQkFDVCxPQUFPLFNBQUE7YUFDUixDQUFDO1FBRkYsQ0FFRSxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELHFDQUFjOzs7OztJQUFkLFVBQWUsQ0FBQyxFQUFFLEVBQTJCO1lBQXpCLG9CQUFPO1FBQ3pCLE9BQU8sSUFBSSxDQUFDLGNBQWMsQ0FBQyxjQUFjLENBQUMsT0FBTyxFQUFFLElBQUksQ0FBQyxDQUFDO0lBQzNELENBQUM7O2dCQTNCbUMsY0FBYzs7SUFHbEQ7UUFEQyxNQUFNLENBQUMsVUFBVSxDQUFDOzs7O2tEQVNsQjtJQUdEO1FBREMsTUFBTSxDQUFDLGFBQWEsQ0FBQzs7eURBQ2tELGFBQWE7O3FEQVFwRjtJQUdEO1FBREMsTUFBTSxDQUFDLGNBQWMsQ0FBQzs7eURBQ1EsY0FBYzs7c0RBRTVDO0lBL0JEO1FBREMsUUFBUSxFQUFFOzs7O3dDQUdWO0lBSlUsWUFBWTtRQUp4QixLQUFLLENBQWdCO1lBQ3BCLElBQUksRUFBRSxjQUFjO1lBQ3BCLFFBQVEsRUFBRSxtQkFBQSxFQUFFLEVBQWlCO1NBQzlCLENBQUM7aURBT29DLGNBQWM7T0FOdkMsWUFBWSxDQWtDeEI7SUFBRCxtQkFBQztDQUFBLElBQUE7U0FsQ1ksWUFBWTs7Ozs7O0lBTVgsc0NBQXNDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBTZWxlY3RvciwgU3RhdGUsIFN0YXRlQ29udGV4dCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5pbXBvcnQgeyBDaGFuZ2VQYXNzd29yZCwgR2V0UHJvZmlsZSwgVXBkYXRlUHJvZmlsZSB9IGZyb20gJy4uL2FjdGlvbnMvcHJvZmlsZS5hY3Rpb25zJztcclxuaW1wb3J0IHsgUHJvZmlsZSB9IGZyb20gJy4uL21vZGVscy9wcm9maWxlJztcclxuaW1wb3J0IHsgUHJvZmlsZVNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9wcm9maWxlLnNlcnZpY2UnO1xyXG5cclxuQFN0YXRlPFByb2ZpbGUuU3RhdGU+KHtcclxuICBuYW1lOiAnUHJvZmlsZVN0YXRlJyxcclxuICBkZWZhdWx0czoge30gYXMgUHJvZmlsZS5TdGF0ZSxcclxufSlcclxuZXhwb3J0IGNsYXNzIFByb2ZpbGVTdGF0ZSB7XHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0UHJvZmlsZSh7IHByb2ZpbGUgfTogUHJvZmlsZS5TdGF0ZSk6IFByb2ZpbGUuUmVzcG9uc2Uge1xyXG4gICAgcmV0dXJuIHByb2ZpbGU7XHJcbiAgfVxyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHByb2ZpbGVTZXJ2aWNlOiBQcm9maWxlU2VydmljZSkge31cclxuXHJcbiAgQEFjdGlvbihHZXRQcm9maWxlKVxyXG4gIGdldFByb2ZpbGUoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxQcm9maWxlLlN0YXRlPikge1xyXG4gICAgcmV0dXJuIHRoaXMucHJvZmlsZVNlcnZpY2UuZ2V0KCkucGlwZShcclxuICAgICAgdGFwKHByb2ZpbGUgPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIHByb2ZpbGUsXHJcbiAgICAgICAgfSksXHJcbiAgICAgICksXHJcbiAgICApO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihVcGRhdGVQcm9maWxlKVxyXG4gIHVwZGF0ZVByb2ZpbGUoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxQcm9maWxlLlN0YXRlPiwgeyBwYXlsb2FkIH06IFVwZGF0ZVByb2ZpbGUpIHtcclxuICAgIHJldHVybiB0aGlzLnByb2ZpbGVTZXJ2aWNlLnVwZGF0ZShwYXlsb2FkKS5waXBlKFxyXG4gICAgICB0YXAocHJvZmlsZSA9PlxyXG4gICAgICAgIHBhdGNoU3RhdGUoe1xyXG4gICAgICAgICAgcHJvZmlsZSxcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKENoYW5nZVBhc3N3b3JkKVxyXG4gIGNoYW5nZVBhc3N3b3JkKF8sIHsgcGF5bG9hZCB9OiBDaGFuZ2VQYXNzd29yZCkge1xyXG4gICAgcmV0dXJuIHRoaXMucHJvZmlsZVNlcnZpY2UuY2hhbmdlUGFzc3dvcmQocGF5bG9hZCwgdHJ1ZSk7XHJcbiAgfVxyXG59XHJcbiJdfQ==