/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { State, Action, Selector } from '@ngxs/store';
import { GetProfile, ChangePassword, UpdateProfile } from '../actions/profile.actions';
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
        return this.profileService.changePassword(payload, true);
    }
};
tslib_1.__decorate([
    Action(GetProfile),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object]),
    tslib_1.__metadata("design:returntype", void 0)
], ProfileState.prototype, "profileGet", null);
tslib_1.__decorate([
    Action(UpdateProfile),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, UpdateProfile]),
    tslib_1.__metadata("design:returntype", void 0)
], ProfileState.prototype, "profileUpdate", null);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvcHJvZmlsZS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFnQixRQUFRLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFVBQVUsRUFBRSxjQUFjLEVBQUUsYUFBYSxFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFFdkYsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLDZCQUE2QixDQUFDO0FBQzdELE9BQU8sRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztJQU14QixZQUFZLFNBQVosWUFBWTs7OztJQU12QixZQUFvQixjQUE4QjtRQUE5QixtQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7SUFBRyxDQUFDOzs7OztJQUp0RCxNQUFNLENBQUMsVUFBVSxDQUFDLEVBQUUsT0FBTyxFQUFpQjtRQUMxQyxPQUFPLE9BQU8sQ0FBQztJQUNqQixDQUFDOzs7OztJQUtELFVBQVUsQ0FBQyxFQUFFLFVBQVUsRUFBK0I7UUFDcEQsT0FBTyxJQUFJLENBQUMsY0FBYyxDQUFDLEdBQUcsRUFBRSxDQUFDLElBQUksQ0FDbkMsR0FBRzs7OztRQUFDLE9BQU8sQ0FBQyxFQUFFLENBQ1osVUFBVSxDQUFDO1lBQ1QsT0FBTztTQUNSLENBQUMsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCxhQUFhLENBQUMsRUFBRSxVQUFVLEVBQStCLEVBQUUsRUFBRSxPQUFPLEVBQWlCO1FBQ25GLE9BQU8sSUFBSSxDQUFDLGNBQWMsQ0FBQyxNQUFNLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUM3QyxHQUFHOzs7O1FBQUMsT0FBTyxDQUFDLEVBQUUsQ0FDWixVQUFVLENBQUM7WUFDVCxPQUFPO1NBQ1IsQ0FBQyxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELGNBQWMsQ0FBQyxDQUFDLEVBQUUsRUFBRSxPQUFPLEVBQWtCO1FBQzNDLE9BQU8sSUFBSSxDQUFDLGNBQWMsQ0FBQyxjQUFjLENBQUMsT0FBTyxFQUFFLElBQUksQ0FBQyxDQUFDO0lBQzNELENBQUM7Q0FDRixDQUFBO0FBekJDO0lBREMsTUFBTSxDQUFDLFVBQVUsQ0FBQzs7Ozs4Q0FTbEI7QUFHRDtJQURDLE1BQU0sQ0FBQyxhQUFhLENBQUM7O3FEQUNrRCxhQUFhOztpREFRcEY7QUFHRDtJQURDLE1BQU0sQ0FBQyxjQUFjLENBQUM7O3FEQUNRLGNBQWM7O2tEQUU1QztBQS9CRDtJQURDLFFBQVEsRUFBRTs7OztvQ0FHVjtBQUpVLFlBQVk7SUFKeEIsS0FBSyxDQUFnQjtRQUNwQixJQUFJLEVBQUUsY0FBYztRQUNwQixRQUFRLEVBQUUsbUJBQUEsRUFBRSxFQUFpQjtLQUM5QixDQUFDOzZDQU9vQyxjQUFjO0dBTnZDLFlBQVksQ0FrQ3hCO1NBbENZLFlBQVk7Ozs7OztJQU1YLHNDQUFzQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFN0YXRlLCBBY3Rpb24sIFN0YXRlQ29udGV4dCwgU2VsZWN0b3IgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBHZXRQcm9maWxlLCBDaGFuZ2VQYXNzd29yZCwgVXBkYXRlUHJvZmlsZSB9IGZyb20gJy4uL2FjdGlvbnMvcHJvZmlsZS5hY3Rpb25zJztcbmltcG9ydCB7IFByb2ZpbGUgfSBmcm9tICcuLi9tb2RlbHMvcHJvZmlsZSc7XG5pbXBvcnQgeyBQcm9maWxlU2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL3Byb2ZpbGUuc2VydmljZSc7XG5pbXBvcnQgeyB0YXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbkBTdGF0ZTxQcm9maWxlLlN0YXRlPih7XG4gIG5hbWU6ICdQcm9maWxlU3RhdGUnLFxuICBkZWZhdWx0czoge30gYXMgUHJvZmlsZS5TdGF0ZSxcbn0pXG5leHBvcnQgY2xhc3MgUHJvZmlsZVN0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldFByb2ZpbGUoeyBwcm9maWxlIH06IFByb2ZpbGUuU3RhdGUpOiBQcm9maWxlLlJlc3BvbnNlIHtcbiAgICByZXR1cm4gcHJvZmlsZTtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcHJvZmlsZVNlcnZpY2U6IFByb2ZpbGVTZXJ2aWNlKSB7fVxuXG4gIEBBY3Rpb24oR2V0UHJvZmlsZSlcbiAgcHJvZmlsZUdldCh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFByb2ZpbGUuU3RhdGU+KSB7XG4gICAgcmV0dXJuIHRoaXMucHJvZmlsZVNlcnZpY2UuZ2V0KCkucGlwZShcbiAgICAgIHRhcChwcm9maWxlID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHByb2ZpbGUsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihVcGRhdGVQcm9maWxlKVxuICBwcm9maWxlVXBkYXRlKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8UHJvZmlsZS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBVcGRhdGVQcm9maWxlKSB7XG4gICAgcmV0dXJuIHRoaXMucHJvZmlsZVNlcnZpY2UudXBkYXRlKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAocHJvZmlsZSA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICBwcm9maWxlLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oQ2hhbmdlUGFzc3dvcmQpXG4gIGNoYW5nZVBhc3N3b3JkKF8sIHsgcGF5bG9hZCB9OiBDaGFuZ2VQYXNzd29yZCkge1xuICAgIHJldHVybiB0aGlzLnByb2ZpbGVTZXJ2aWNlLmNoYW5nZVBhc3N3b3JkKHBheWxvYWQsIHRydWUpO1xuICB9XG59XG4iXX0=