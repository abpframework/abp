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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvcHJvZmlsZS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLEdBQUcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3JDLE9BQU8sRUFBRSxjQUFjLEVBQUUsVUFBVSxFQUFFLGFBQWEsRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBRXZGLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSw2QkFBNkIsQ0FBQztJQU1oRCxZQUFZLFNBQVosWUFBWTs7OztJQU12QixZQUFvQixjQUE4QjtRQUE5QixtQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7SUFBRyxDQUFDOzs7OztJQUp0RCxNQUFNLENBQUMsVUFBVSxDQUFDLEVBQUUsT0FBTyxFQUFpQjtRQUMxQyxPQUFPLE9BQU8sQ0FBQztJQUNqQixDQUFDOzs7OztJQUtELFVBQVUsQ0FBQyxFQUFFLFVBQVUsRUFBK0I7UUFDcEQsT0FBTyxJQUFJLENBQUMsY0FBYyxDQUFDLEdBQUcsRUFBRSxDQUFDLElBQUksQ0FDbkMsR0FBRzs7OztRQUFDLE9BQU8sQ0FBQyxFQUFFLENBQ1osVUFBVSxDQUFDO1lBQ1QsT0FBTztTQUNSLENBQUMsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCxhQUFhLENBQUMsRUFBRSxVQUFVLEVBQStCLEVBQUUsRUFBRSxPQUFPLEVBQWlCO1FBQ25GLE9BQU8sSUFBSSxDQUFDLGNBQWMsQ0FBQyxNQUFNLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUM3QyxHQUFHOzs7O1FBQUMsT0FBTyxDQUFDLEVBQUUsQ0FDWixVQUFVLENBQUM7WUFDVCxPQUFPO1NBQ1IsQ0FBQyxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELGNBQWMsQ0FBQyxDQUFDLEVBQUUsRUFBRSxPQUFPLEVBQWtCO1FBQzNDLE9BQU8sSUFBSSxDQUFDLGNBQWMsQ0FBQyxjQUFjLENBQUMsT0FBTyxFQUFFLElBQUksQ0FBQyxDQUFDO0lBQzNELENBQUM7Q0FDRixDQUFBOztZQTVCcUMsY0FBYzs7QUFHbEQ7SUFEQyxNQUFNLENBQUMsVUFBVSxDQUFDOzs7OzhDQVNsQjtBQUdEO0lBREMsTUFBTSxDQUFDLGFBQWEsQ0FBQzs7cURBQ2tELGFBQWE7O2lEQVFwRjtBQUdEO0lBREMsTUFBTSxDQUFDLGNBQWMsQ0FBQzs7cURBQ1EsY0FBYzs7a0RBRTVDO0FBL0JEO0lBREMsUUFBUSxFQUFFOzs7O29DQUdWO0FBSlUsWUFBWTtJQUp4QixLQUFLLENBQWdCO1FBQ3BCLElBQUksRUFBRSxjQUFjO1FBQ3BCLFFBQVEsRUFBRSxtQkFBQSxFQUFFLEVBQWlCO0tBQzlCLENBQUM7NkNBT29DLGNBQWM7R0FOdkMsWUFBWSxDQWtDeEI7U0FsQ1ksWUFBWTs7Ozs7O0lBTVgsc0NBQXNDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBTZWxlY3RvciwgU3RhdGUsIFN0YXRlQ29udGV4dCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7IENoYW5nZVBhc3N3b3JkLCBHZXRQcm9maWxlLCBVcGRhdGVQcm9maWxlIH0gZnJvbSAnLi4vYWN0aW9ucy9wcm9maWxlLmFjdGlvbnMnO1xuaW1wb3J0IHsgUHJvZmlsZSB9IGZyb20gJy4uL21vZGVscy9wcm9maWxlJztcbmltcG9ydCB7IFByb2ZpbGVTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvcHJvZmlsZS5zZXJ2aWNlJztcblxuQFN0YXRlPFByb2ZpbGUuU3RhdGU+KHtcbiAgbmFtZTogJ1Byb2ZpbGVTdGF0ZScsXG4gIGRlZmF1bHRzOiB7fSBhcyBQcm9maWxlLlN0YXRlLFxufSlcbmV4cG9ydCBjbGFzcyBQcm9maWxlU3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0UHJvZmlsZSh7IHByb2ZpbGUgfTogUHJvZmlsZS5TdGF0ZSk6IFByb2ZpbGUuUmVzcG9uc2Uge1xuICAgIHJldHVybiBwcm9maWxlO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBwcm9maWxlU2VydmljZTogUHJvZmlsZVNlcnZpY2UpIHt9XG5cbiAgQEFjdGlvbihHZXRQcm9maWxlKVxuICBnZXRQcm9maWxlKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8UHJvZmlsZS5TdGF0ZT4pIHtcbiAgICByZXR1cm4gdGhpcy5wcm9maWxlU2VydmljZS5nZXQoKS5waXBlKFxuICAgICAgdGFwKHByb2ZpbGUgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgcHJvZmlsZSxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKFVwZGF0ZVByb2ZpbGUpXG4gIHVwZGF0ZVByb2ZpbGUoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxQcm9maWxlLlN0YXRlPiwgeyBwYXlsb2FkIH06IFVwZGF0ZVByb2ZpbGUpIHtcbiAgICByZXR1cm4gdGhpcy5wcm9maWxlU2VydmljZS51cGRhdGUocGF5bG9hZCkucGlwZShcbiAgICAgIHRhcChwcm9maWxlID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHByb2ZpbGUsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihDaGFuZ2VQYXNzd29yZClcbiAgY2hhbmdlUGFzc3dvcmQoXywgeyBwYXlsb2FkIH06IENoYW5nZVBhc3N3b3JkKSB7XG4gICAgcmV0dXJuIHRoaXMucHJvZmlsZVNlcnZpY2UuY2hhbmdlUGFzc3dvcmQocGF5bG9hZCwgdHJ1ZSk7XG4gIH1cbn1cbiJdfQ==