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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvcHJvZmlsZS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQWdCLE1BQU0sYUFBYSxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNyQyxPQUFPLEVBQUUsY0FBYyxFQUFFLFVBQVUsRUFBRSxhQUFhLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUV2RixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sNkJBQTZCLENBQUM7SUFNaEQsWUFBWSxTQUFaLFlBQVk7Ozs7SUFNdkIsWUFBb0IsY0FBOEI7UUFBOUIsbUJBQWMsR0FBZCxjQUFjLENBQWdCO0lBQUcsQ0FBQzs7Ozs7SUFKdEQsTUFBTSxDQUFDLFVBQVUsQ0FBQyxFQUFFLE9BQU8sRUFBaUI7UUFDMUMsT0FBTyxPQUFPLENBQUM7SUFDakIsQ0FBQzs7Ozs7SUFLRCxVQUFVLENBQUMsRUFBRSxVQUFVLEVBQStCO1FBQ3BELE9BQU8sSUFBSSxDQUFDLGNBQWMsQ0FBQyxHQUFHLEVBQUUsQ0FBQyxJQUFJLENBQ25DLEdBQUc7Ozs7UUFBQyxPQUFPLENBQUMsRUFBRSxDQUNaLFVBQVUsQ0FBQztZQUNULE9BQU87U0FDUixDQUFDLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QsYUFBYSxDQUFDLEVBQUUsVUFBVSxFQUErQixFQUFFLEVBQUUsT0FBTyxFQUFpQjtRQUNuRixPQUFPLElBQUksQ0FBQyxjQUFjLENBQUMsTUFBTSxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FDN0MsR0FBRzs7OztRQUFDLE9BQU8sQ0FBQyxFQUFFLENBQ1osVUFBVSxDQUFDO1lBQ1QsT0FBTztTQUNSLENBQUMsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCxjQUFjLENBQUMsQ0FBQyxFQUFFLEVBQUUsT0FBTyxFQUFrQjtRQUMzQyxPQUFPLElBQUksQ0FBQyxjQUFjLENBQUMsY0FBYyxDQUFDLE9BQU8sRUFBRSxJQUFJLENBQUMsQ0FBQztJQUMzRCxDQUFDO0NBQ0YsQ0FBQTs7WUE1QnFDLGNBQWM7O0FBR2xEO0lBREMsTUFBTSxDQUFDLFVBQVUsQ0FBQzs7Ozs4Q0FTbEI7QUFHRDtJQURDLE1BQU0sQ0FBQyxhQUFhLENBQUM7O3FEQUNrRCxhQUFhOztpREFRcEY7QUFHRDtJQURDLE1BQU0sQ0FBQyxjQUFjLENBQUM7O3FEQUNRLGNBQWM7O2tEQUU1QztBQS9CRDtJQURDLFFBQVEsRUFBRTs7OztvQ0FHVjtBQUpVLFlBQVk7SUFKeEIsS0FBSyxDQUFnQjtRQUNwQixJQUFJLEVBQUUsY0FBYztRQUNwQixRQUFRLEVBQUUsbUJBQUEsRUFBRSxFQUFpQjtLQUM5QixDQUFDOzZDQU9vQyxjQUFjO0dBTnZDLFlBQVksQ0FrQ3hCO1NBbENZLFlBQVk7Ozs7OztJQU1YLHNDQUFzQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcclxuaW1wb3J0IHsgQ2hhbmdlUGFzc3dvcmQsIEdldFByb2ZpbGUsIFVwZGF0ZVByb2ZpbGUgfSBmcm9tICcuLi9hY3Rpb25zL3Byb2ZpbGUuYWN0aW9ucyc7XHJcbmltcG9ydCB7IFByb2ZpbGUgfSBmcm9tICcuLi9tb2RlbHMvcHJvZmlsZSc7XHJcbmltcG9ydCB7IFByb2ZpbGVTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvcHJvZmlsZS5zZXJ2aWNlJztcclxuXHJcbkBTdGF0ZTxQcm9maWxlLlN0YXRlPih7XHJcbiAgbmFtZTogJ1Byb2ZpbGVTdGF0ZScsXHJcbiAgZGVmYXVsdHM6IHt9IGFzIFByb2ZpbGUuU3RhdGUsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBQcm9maWxlU3RhdGUge1xyXG4gIEBTZWxlY3RvcigpXHJcbiAgc3RhdGljIGdldFByb2ZpbGUoeyBwcm9maWxlIH06IFByb2ZpbGUuU3RhdGUpOiBQcm9maWxlLlJlc3BvbnNlIHtcclxuICAgIHJldHVybiBwcm9maWxlO1xyXG4gIH1cclxuXHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBwcm9maWxlU2VydmljZTogUHJvZmlsZVNlcnZpY2UpIHt9XHJcblxyXG4gIEBBY3Rpb24oR2V0UHJvZmlsZSlcclxuICBnZXRQcm9maWxlKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8UHJvZmlsZS5TdGF0ZT4pIHtcclxuICAgIHJldHVybiB0aGlzLnByb2ZpbGVTZXJ2aWNlLmdldCgpLnBpcGUoXHJcbiAgICAgIHRhcChwcm9maWxlID0+XHJcbiAgICAgICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgICAgICBwcm9maWxlLFxyXG4gICAgICAgIH0pLFxyXG4gICAgICApLFxyXG4gICAgKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oVXBkYXRlUHJvZmlsZSlcclxuICB1cGRhdGVQcm9maWxlKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8UHJvZmlsZS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBVcGRhdGVQcm9maWxlKSB7XHJcbiAgICByZXR1cm4gdGhpcy5wcm9maWxlU2VydmljZS51cGRhdGUocGF5bG9hZCkucGlwZShcclxuICAgICAgdGFwKHByb2ZpbGUgPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIHByb2ZpbGUsXHJcbiAgICAgICAgfSksXHJcbiAgICAgICksXHJcbiAgICApO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihDaGFuZ2VQYXNzd29yZClcclxuICBjaGFuZ2VQYXNzd29yZChfLCB7IHBheWxvYWQgfTogQ2hhbmdlUGFzc3dvcmQpIHtcclxuICAgIHJldHVybiB0aGlzLnByb2ZpbGVTZXJ2aWNlLmNoYW5nZVBhc3N3b3JkKHBheWxvYWQsIHRydWUpO1xyXG4gIH1cclxufVxyXG4iXX0=