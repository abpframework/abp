/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvcHJvZmlsZS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLEdBQUcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3JDLE9BQU8sRUFBRSxjQUFjLEVBQUUsVUFBVSxFQUFFLGFBQWEsRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBRXZGLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSw2QkFBNkIsQ0FBQzs7SUFZM0Qsc0JBQW9CLGNBQThCO1FBQTlCLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtJQUFHLENBQUM7Ozs7O0lBSi9DLHVCQUFVOzs7O0lBQWpCLFVBQWtCLEVBQTBCO1lBQXhCLG9CQUFPO1FBQ3pCLE9BQU8sT0FBTyxDQUFDO0lBQ2pCLENBQUM7Ozs7O0lBS0QsaUNBQVU7Ozs7SUFBVixVQUFXLEVBQTJDO1lBQXpDLDBCQUFVO1FBQ3JCLE9BQU8sSUFBSSxDQUFDLGNBQWMsQ0FBQyxHQUFHLEVBQUUsQ0FBQyxJQUFJLENBQ25DLEdBQUc7Ozs7UUFBQyxVQUFBLE9BQU87WUFDVCxPQUFBLFVBQVUsQ0FBQztnQkFDVCxPQUFPLFNBQUE7YUFDUixDQUFDO1FBRkYsQ0FFRSxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELG9DQUFhOzs7OztJQUFiLFVBQWMsRUFBMkMsRUFBRSxFQUEwQjtZQUFyRSwwQkFBVTtZQUFtQyxvQkFBTztRQUNsRSxPQUFPLElBQUksQ0FBQyxjQUFjLENBQUMsTUFBTSxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FDN0MsR0FBRzs7OztRQUFDLFVBQUEsT0FBTztZQUNULE9BQUEsVUFBVSxDQUFDO2dCQUNULE9BQU8sU0FBQTthQUNSLENBQUM7UUFGRixDQUVFLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QscUNBQWM7Ozs7O0lBQWQsVUFBZSxDQUFDLEVBQUUsRUFBMkI7WUFBekIsb0JBQU87UUFDekIsT0FBTyxJQUFJLENBQUMsY0FBYyxDQUFDLGNBQWMsQ0FBQyxPQUFPLEVBQUUsSUFBSSxDQUFDLENBQUM7SUFDM0QsQ0FBQzs7Z0JBM0JtQyxjQUFjOztJQUdsRDtRQURDLE1BQU0sQ0FBQyxVQUFVLENBQUM7Ozs7a0RBU2xCO0lBR0Q7UUFEQyxNQUFNLENBQUMsYUFBYSxDQUFDOzt5REFDa0QsYUFBYTs7cURBUXBGO0lBR0Q7UUFEQyxNQUFNLENBQUMsY0FBYyxDQUFDOzt5REFDUSxjQUFjOztzREFFNUM7SUEvQkQ7UUFEQyxRQUFRLEVBQUU7Ozs7d0NBR1Y7SUFKVSxZQUFZO1FBSnhCLEtBQUssQ0FBZ0I7WUFDcEIsSUFBSSxFQUFFLGNBQWM7WUFDcEIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsRUFBaUI7U0FDOUIsQ0FBQztpREFPb0MsY0FBYztPQU52QyxZQUFZLENBa0N4QjtJQUFELG1CQUFDO0NBQUEsSUFBQTtTQWxDWSxZQUFZOzs7Ozs7SUFNWCxzQ0FBc0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBY3Rpb24sIFNlbGVjdG9yLCBTdGF0ZSwgU3RhdGVDb250ZXh0IH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyB0YXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XHJcbmltcG9ydCB7IENoYW5nZVBhc3N3b3JkLCBHZXRQcm9maWxlLCBVcGRhdGVQcm9maWxlIH0gZnJvbSAnLi4vYWN0aW9ucy9wcm9maWxlLmFjdGlvbnMnO1xyXG5pbXBvcnQgeyBQcm9maWxlIH0gZnJvbSAnLi4vbW9kZWxzL3Byb2ZpbGUnO1xyXG5pbXBvcnQgeyBQcm9maWxlU2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL3Byb2ZpbGUuc2VydmljZSc7XHJcblxyXG5AU3RhdGU8UHJvZmlsZS5TdGF0ZT4oe1xyXG4gIG5hbWU6ICdQcm9maWxlU3RhdGUnLFxyXG4gIGRlZmF1bHRzOiB7fSBhcyBQcm9maWxlLlN0YXRlLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgUHJvZmlsZVN0YXRlIHtcclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXRQcm9maWxlKHsgcHJvZmlsZSB9OiBQcm9maWxlLlN0YXRlKTogUHJvZmlsZS5SZXNwb25zZSB7XHJcbiAgICByZXR1cm4gcHJvZmlsZTtcclxuICB9XHJcblxyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcHJvZmlsZVNlcnZpY2U6IFByb2ZpbGVTZXJ2aWNlKSB7fVxyXG5cclxuICBAQWN0aW9uKEdldFByb2ZpbGUpXHJcbiAgZ2V0UHJvZmlsZSh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFByb2ZpbGUuU3RhdGU+KSB7XHJcbiAgICByZXR1cm4gdGhpcy5wcm9maWxlU2VydmljZS5nZXQoKS5waXBlKFxyXG4gICAgICB0YXAocHJvZmlsZSA9PlxyXG4gICAgICAgIHBhdGNoU3RhdGUoe1xyXG4gICAgICAgICAgcHJvZmlsZSxcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKFVwZGF0ZVByb2ZpbGUpXHJcbiAgdXBkYXRlUHJvZmlsZSh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFByb2ZpbGUuU3RhdGU+LCB7IHBheWxvYWQgfTogVXBkYXRlUHJvZmlsZSkge1xyXG4gICAgcmV0dXJuIHRoaXMucHJvZmlsZVNlcnZpY2UudXBkYXRlKHBheWxvYWQpLnBpcGUoXHJcbiAgICAgIHRhcChwcm9maWxlID0+XHJcbiAgICAgICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgICAgICBwcm9maWxlLFxyXG4gICAgICAgIH0pLFxyXG4gICAgICApLFxyXG4gICAgKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oQ2hhbmdlUGFzc3dvcmQpXHJcbiAgY2hhbmdlUGFzc3dvcmQoXywgeyBwYXlsb2FkIH06IENoYW5nZVBhc3N3b3JkKSB7XHJcbiAgICByZXR1cm4gdGhpcy5wcm9maWxlU2VydmljZS5jaGFuZ2VQYXNzd29yZChwYXlsb2FkLCB0cnVlKTtcclxuICB9XHJcbn1cclxuIl19