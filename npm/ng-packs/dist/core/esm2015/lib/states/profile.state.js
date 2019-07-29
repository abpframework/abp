/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { State, Action, Selector } from '@ngxs/store';
import { ProfileGet, ProfileChangePassword, ProfileUpdate } from '../actions/profile.actions';
import { ProfileService } from '../services/profile.service';
import { tap } from 'rxjs/operators';
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
    profileGet({ patchState }) {
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
    profileUpdate({ patchState }, { payload }) {
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
        return this.profileService.changePassword(payload);
    }
};
tslib_1.__decorate([
    Action(ProfileGet),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object]),
    tslib_1.__metadata("design:returntype", void 0)
], ProfileState.prototype, "profileGet", null);
tslib_1.__decorate([
    Action(ProfileUpdate),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, ProfileUpdate]),
    tslib_1.__metadata("design:returntype", void 0)
], ProfileState.prototype, "profileUpdate", null);
tslib_1.__decorate([
    Action(ProfileChangePassword),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, ProfileChangePassword]),
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvcHJvZmlsZS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFnQixRQUFRLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFVBQVUsRUFBRSxxQkFBcUIsRUFBRSxhQUFhLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUU5RixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sNkJBQTZCLENBQUM7QUFDN0QsT0FBTyxFQUFFLEdBQUcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0lBTXhCLFlBQVksU0FBWixZQUFZOzs7O0lBTXZCLFlBQW9CLGNBQThCO1FBQTlCLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtJQUFHLENBQUM7Ozs7O0lBSnRELE1BQU0sQ0FBQyxVQUFVLENBQUMsRUFBRSxPQUFPLEVBQWlCO1FBQzFDLE9BQU8sT0FBTyxDQUFDO0lBQ2pCLENBQUM7Ozs7O0lBS0QsVUFBVSxDQUFDLEVBQUUsVUFBVSxFQUErQjtRQUNwRCxPQUFPLElBQUksQ0FBQyxjQUFjLENBQUMsR0FBRyxFQUFFLENBQUMsSUFBSSxDQUNuQyxHQUFHOzs7O1FBQUMsT0FBTyxDQUFDLEVBQUUsQ0FDWixVQUFVLENBQUM7WUFDVCxPQUFPO1NBQ1IsQ0FBQyxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELGFBQWEsQ0FBQyxFQUFFLFVBQVUsRUFBK0IsRUFBRSxFQUFFLE9BQU8sRUFBaUI7UUFDbkYsT0FBTyxJQUFJLENBQUMsY0FBYyxDQUFDLE1BQU0sQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQzdDLEdBQUc7Ozs7UUFBQyxPQUFPLENBQUMsRUFBRSxDQUNaLFVBQVUsQ0FBQztZQUNULE9BQU87U0FDUixDQUFDLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QsY0FBYyxDQUFDLENBQUMsRUFBRSxFQUFFLE9BQU8sRUFBeUI7UUFDbEQsT0FBTyxJQUFJLENBQUMsY0FBYyxDQUFDLGNBQWMsQ0FBQyxPQUFPLENBQUMsQ0FBQztJQUNyRCxDQUFDO0NBQ0YsQ0FBQTtBQXpCQztJQURDLE1BQU0sQ0FBQyxVQUFVLENBQUM7Ozs7OENBU2xCO0FBR0Q7SUFEQyxNQUFNLENBQUMsYUFBYSxDQUFDOztxREFDa0QsYUFBYTs7aURBUXBGO0FBR0Q7SUFEQyxNQUFNLENBQUMscUJBQXFCLENBQUM7O3FEQUNDLHFCQUFxQjs7a0RBRW5EO0FBL0JEO0lBREMsUUFBUSxFQUFFOzs7O29DQUdWO0FBSlUsWUFBWTtJQUp4QixLQUFLLENBQWdCO1FBQ3BCLElBQUksRUFBRSxjQUFjO1FBQ3BCLFFBQVEsRUFBRSxtQkFBQSxFQUFFLEVBQWlCO0tBQzlCLENBQUM7NkNBT29DLGNBQWM7R0FOdkMsWUFBWSxDQWtDeEI7U0FsQ1ksWUFBWTs7Ozs7O0lBTVgsc0NBQXNDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhdGUsIEFjdGlvbiwgU3RhdGVDb250ZXh0LCBTZWxlY3RvciB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IFByb2ZpbGVHZXQsIFByb2ZpbGVDaGFuZ2VQYXNzd29yZCwgUHJvZmlsZVVwZGF0ZSB9IGZyb20gJy4uL2FjdGlvbnMvcHJvZmlsZS5hY3Rpb25zJztcbmltcG9ydCB7IFByb2ZpbGUgfSBmcm9tICcuLi9tb2RlbHMvcHJvZmlsZSc7XG5pbXBvcnQgeyBQcm9maWxlU2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL3Byb2ZpbGUuc2VydmljZSc7XG5pbXBvcnQgeyB0YXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbkBTdGF0ZTxQcm9maWxlLlN0YXRlPih7XG4gIG5hbWU6ICdQcm9maWxlU3RhdGUnLFxuICBkZWZhdWx0czoge30gYXMgUHJvZmlsZS5TdGF0ZSxcbn0pXG5leHBvcnQgY2xhc3MgUHJvZmlsZVN0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldFByb2ZpbGUoeyBwcm9maWxlIH06IFByb2ZpbGUuU3RhdGUpOiBQcm9maWxlLlJlc3BvbnNlIHtcbiAgICByZXR1cm4gcHJvZmlsZTtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcHJvZmlsZVNlcnZpY2U6IFByb2ZpbGVTZXJ2aWNlKSB7fVxuXG4gIEBBY3Rpb24oUHJvZmlsZUdldClcbiAgcHJvZmlsZUdldCh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFByb2ZpbGUuU3RhdGU+KSB7XG4gICAgcmV0dXJuIHRoaXMucHJvZmlsZVNlcnZpY2UuZ2V0KCkucGlwZShcbiAgICAgIHRhcChwcm9maWxlID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHByb2ZpbGUsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihQcm9maWxlVXBkYXRlKVxuICBwcm9maWxlVXBkYXRlKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8UHJvZmlsZS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBQcm9maWxlVXBkYXRlKSB7XG4gICAgcmV0dXJuIHRoaXMucHJvZmlsZVNlcnZpY2UudXBkYXRlKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAocHJvZmlsZSA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICBwcm9maWxlLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oUHJvZmlsZUNoYW5nZVBhc3N3b3JkKVxuICBjaGFuZ2VQYXNzd29yZChfLCB7IHBheWxvYWQgfTogUHJvZmlsZUNoYW5nZVBhc3N3b3JkKSB7XG4gICAgcmV0dXJuIHRoaXMucHJvZmlsZVNlcnZpY2UuY2hhbmdlUGFzc3dvcmQocGF5bG9hZCk7XG4gIH1cbn1cbiJdfQ==