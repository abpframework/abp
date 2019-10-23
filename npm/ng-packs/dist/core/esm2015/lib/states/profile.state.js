/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { tap } from 'rxjs/operators';
import { ChangePassword, GetProfile, UpdateProfile } from '../actions/profile.actions';
import { ProfileService } from '../services/profile.service';
let ProfileState = class ProfileState {
    /**
     * @param {?} profileService
     */
    constructor(profileService) {
        this.profileService = profileService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getProfile({ profile }) {
        return profile;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    getProfile({ patchState }) {
        return this.profileService.get().pipe(tap((/**
         * @param {?} profile
         * @return {?}
         */
        profile => patchState({
            profile,
        }))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    updateProfile({ patchState }, { payload }) {
        return this.profileService.update(payload).pipe(tap((/**
         * @param {?} profile
         * @return {?}
         */
        profile => patchState({
            profile,
        }))));
    }
    /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    changePassword(_, { payload }) {
        return this.profileService.changePassword(payload, true);
    }
};
ProfileState.ctorParameters = () => [
    { type: ProfileService }
];
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
export { ProfileState };
if (false) {
    /**
     * @type {?}
     * @private
     */
    ProfileState.prototype.profileService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvcHJvZmlsZS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLEdBQUcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3JDLE9BQU8sRUFBRSxjQUFjLEVBQUUsVUFBVSxFQUFFLGFBQWEsRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBRXZGLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSw2QkFBNkIsQ0FBQztJQU1oRCxZQUFZLFNBQVosWUFBWTs7OztJQU12QixZQUFvQixjQUE4QjtRQUE5QixtQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7SUFBRyxDQUFDOzs7OztJQUp0RCxNQUFNLENBQUMsVUFBVSxDQUFDLEVBQUUsT0FBTyxFQUFpQjtRQUMxQyxPQUFPLE9BQU8sQ0FBQztJQUNqQixDQUFDOzs7OztJQUtELFVBQVUsQ0FBQyxFQUFFLFVBQVUsRUFBK0I7UUFDcEQsT0FBTyxJQUFJLENBQUMsY0FBYyxDQUFDLEdBQUcsRUFBRSxDQUFDLElBQUksQ0FDbkMsR0FBRzs7OztRQUFDLE9BQU8sQ0FBQyxFQUFFLENBQ1osVUFBVSxDQUFDO1lBQ1QsT0FBTztTQUNSLENBQUMsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCxhQUFhLENBQUMsRUFBRSxVQUFVLEVBQStCLEVBQUUsRUFBRSxPQUFPLEVBQWlCO1FBQ25GLE9BQU8sSUFBSSxDQUFDLGNBQWMsQ0FBQyxNQUFNLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUM3QyxHQUFHOzs7O1FBQUMsT0FBTyxDQUFDLEVBQUUsQ0FDWixVQUFVLENBQUM7WUFDVCxPQUFPO1NBQ1IsQ0FBQyxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELGNBQWMsQ0FBQyxDQUFDLEVBQUUsRUFBRSxPQUFPLEVBQWtCO1FBQzNDLE9BQU8sSUFBSSxDQUFDLGNBQWMsQ0FBQyxjQUFjLENBQUMsT0FBTyxFQUFFLElBQUksQ0FBQyxDQUFDO0lBQzNELENBQUM7Q0FDRixDQUFBOztZQTVCcUMsY0FBYzs7QUFHbEQ7SUFEQyxNQUFNLENBQUMsVUFBVSxDQUFDOzs7OzhDQVNsQjtBQUdEO0lBREMsTUFBTSxDQUFDLGFBQWEsQ0FBQzs7cURBQ2tELGFBQWE7O2lEQVFwRjtBQUdEO0lBREMsTUFBTSxDQUFDLGNBQWMsQ0FBQzs7cURBQ1EsY0FBYzs7a0RBRTVDO0FBL0JEO0lBREMsUUFBUSxFQUFFOzs7O29DQUdWO0FBSlUsWUFBWTtJQUp4QixLQUFLLENBQWdCO1FBQ3BCLElBQUksRUFBRSxjQUFjO1FBQ3BCLFFBQVEsRUFBRSxtQkFBQSxFQUFFLEVBQWlCO0tBQzlCLENBQUM7NkNBT29DLGNBQWM7R0FOdkMsWUFBWSxDQWtDeEI7U0FsQ1ksWUFBWTs7Ozs7O0lBTVgsc0NBQXNDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBTZWxlY3RvciwgU3RhdGUsIFN0YXRlQ29udGV4dCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5pbXBvcnQgeyBDaGFuZ2VQYXNzd29yZCwgR2V0UHJvZmlsZSwgVXBkYXRlUHJvZmlsZSB9IGZyb20gJy4uL2FjdGlvbnMvcHJvZmlsZS5hY3Rpb25zJztcclxuaW1wb3J0IHsgUHJvZmlsZSB9IGZyb20gJy4uL21vZGVscy9wcm9maWxlJztcclxuaW1wb3J0IHsgUHJvZmlsZVNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9wcm9maWxlLnNlcnZpY2UnO1xyXG5cclxuQFN0YXRlPFByb2ZpbGUuU3RhdGU+KHtcclxuICBuYW1lOiAnUHJvZmlsZVN0YXRlJyxcclxuICBkZWZhdWx0czoge30gYXMgUHJvZmlsZS5TdGF0ZSxcclxufSlcclxuZXhwb3J0IGNsYXNzIFByb2ZpbGVTdGF0ZSB7XHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0UHJvZmlsZSh7IHByb2ZpbGUgfTogUHJvZmlsZS5TdGF0ZSk6IFByb2ZpbGUuUmVzcG9uc2Uge1xyXG4gICAgcmV0dXJuIHByb2ZpbGU7XHJcbiAgfVxyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHByb2ZpbGVTZXJ2aWNlOiBQcm9maWxlU2VydmljZSkge31cclxuXHJcbiAgQEFjdGlvbihHZXRQcm9maWxlKVxyXG4gIGdldFByb2ZpbGUoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxQcm9maWxlLlN0YXRlPikge1xyXG4gICAgcmV0dXJuIHRoaXMucHJvZmlsZVNlcnZpY2UuZ2V0KCkucGlwZShcclxuICAgICAgdGFwKHByb2ZpbGUgPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIHByb2ZpbGUsXHJcbiAgICAgICAgfSksXHJcbiAgICAgICksXHJcbiAgICApO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihVcGRhdGVQcm9maWxlKVxyXG4gIHVwZGF0ZVByb2ZpbGUoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxQcm9maWxlLlN0YXRlPiwgeyBwYXlsb2FkIH06IFVwZGF0ZVByb2ZpbGUpIHtcclxuICAgIHJldHVybiB0aGlzLnByb2ZpbGVTZXJ2aWNlLnVwZGF0ZShwYXlsb2FkKS5waXBlKFxyXG4gICAgICB0YXAocHJvZmlsZSA9PlxyXG4gICAgICAgIHBhdGNoU3RhdGUoe1xyXG4gICAgICAgICAgcHJvZmlsZSxcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKENoYW5nZVBhc3N3b3JkKVxyXG4gIGNoYW5nZVBhc3N3b3JkKF8sIHsgcGF5bG9hZCB9OiBDaGFuZ2VQYXNzd29yZCkge1xyXG4gICAgcmV0dXJuIHRoaXMucHJvZmlsZVNlcnZpY2UuY2hhbmdlUGFzc3dvcmQocGF5bG9hZCwgdHJ1ZSk7XHJcbiAgfVxyXG59XHJcbiJdfQ==