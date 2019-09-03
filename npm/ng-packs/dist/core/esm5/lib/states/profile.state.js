/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { State, Action, Selector } from '@ngxs/store';
import { GetProfile, ChangePassword, UpdateProfile } from '../actions/profile.actions';
import { ProfileService } from '../services/profile.service';
import { tap } from 'rxjs/operators';
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
    ProfileState.prototype.profileGet = /**
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
    ProfileState.prototype.profileUpdate = /**
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvcHJvZmlsZS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFnQixRQUFRLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFVBQVUsRUFBRSxjQUFjLEVBQUUsYUFBYSxFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFFdkYsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLDZCQUE2QixDQUFDO0FBQzdELE9BQU8sRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQzs7SUFZbkMsc0JBQW9CLGNBQThCO1FBQTlCLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtJQUFHLENBQUM7Ozs7O0lBSi9DLHVCQUFVOzs7O0lBQWpCLFVBQWtCLEVBQTBCO1lBQXhCLG9CQUFPO1FBQ3pCLE9BQU8sT0FBTyxDQUFDO0lBQ2pCLENBQUM7Ozs7O0lBS0QsaUNBQVU7Ozs7SUFBVixVQUFXLEVBQTJDO1lBQXpDLDBCQUFVO1FBQ3JCLE9BQU8sSUFBSSxDQUFDLGNBQWMsQ0FBQyxHQUFHLEVBQUUsQ0FBQyxJQUFJLENBQ25DLEdBQUc7Ozs7UUFBQyxVQUFBLE9BQU87WUFDVCxPQUFBLFVBQVUsQ0FBQztnQkFDVCxPQUFPLFNBQUE7YUFDUixDQUFDO1FBRkYsQ0FFRSxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELG9DQUFhOzs7OztJQUFiLFVBQWMsRUFBMkMsRUFBRSxFQUEwQjtZQUFyRSwwQkFBVTtZQUFtQyxvQkFBTztRQUNsRSxPQUFPLElBQUksQ0FBQyxjQUFjLENBQUMsTUFBTSxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FDN0MsR0FBRzs7OztRQUFDLFVBQUEsT0FBTztZQUNULE9BQUEsVUFBVSxDQUFDO2dCQUNULE9BQU8sU0FBQTthQUNSLENBQUM7UUFGRixDQUVFLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QscUNBQWM7Ozs7O0lBQWQsVUFBZSxDQUFDLEVBQUUsRUFBMkI7WUFBekIsb0JBQU87UUFDekIsT0FBTyxJQUFJLENBQUMsY0FBYyxDQUFDLGNBQWMsQ0FBQyxPQUFPLEVBQUUsSUFBSSxDQUFDLENBQUM7SUFDM0QsQ0FBQztJQXhCRDtRQURDLE1BQU0sQ0FBQyxVQUFVLENBQUM7Ozs7a0RBU2xCO0lBR0Q7UUFEQyxNQUFNLENBQUMsYUFBYSxDQUFDOzt5REFDa0QsYUFBYTs7cURBUXBGO0lBR0Q7UUFEQyxNQUFNLENBQUMsY0FBYyxDQUFDOzt5REFDUSxjQUFjOztzREFFNUM7SUEvQkQ7UUFEQyxRQUFRLEVBQUU7Ozs7d0NBR1Y7SUFKVSxZQUFZO1FBSnhCLEtBQUssQ0FBZ0I7WUFDcEIsSUFBSSxFQUFFLGNBQWM7WUFDcEIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsRUFBaUI7U0FDOUIsQ0FBQztpREFPb0MsY0FBYztPQU52QyxZQUFZLENBa0N4QjtJQUFELG1CQUFDO0NBQUEsSUFBQTtTQWxDWSxZQUFZOzs7Ozs7SUFNWCxzQ0FBc0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBTdGF0ZSwgQWN0aW9uLCBTdGF0ZUNvbnRleHQsIFNlbGVjdG9yIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgR2V0UHJvZmlsZSwgQ2hhbmdlUGFzc3dvcmQsIFVwZGF0ZVByb2ZpbGUgfSBmcm9tICcuLi9hY3Rpb25zL3Byb2ZpbGUuYWN0aW9ucyc7XG5pbXBvcnQgeyBQcm9maWxlIH0gZnJvbSAnLi4vbW9kZWxzL3Byb2ZpbGUnO1xuaW1wb3J0IHsgUHJvZmlsZVNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9wcm9maWxlLnNlcnZpY2UnO1xuaW1wb3J0IHsgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuXG5AU3RhdGU8UHJvZmlsZS5TdGF0ZT4oe1xuICBuYW1lOiAnUHJvZmlsZVN0YXRlJyxcbiAgZGVmYXVsdHM6IHt9IGFzIFByb2ZpbGUuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIFByb2ZpbGVTdGF0ZSB7XG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRQcm9maWxlKHsgcHJvZmlsZSB9OiBQcm9maWxlLlN0YXRlKTogUHJvZmlsZS5SZXNwb25zZSB7XG4gICAgcmV0dXJuIHByb2ZpbGU7XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHByb2ZpbGVTZXJ2aWNlOiBQcm9maWxlU2VydmljZSkge31cblxuICBAQWN0aW9uKEdldFByb2ZpbGUpXG4gIHByb2ZpbGVHZXQoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxQcm9maWxlLlN0YXRlPikge1xuICAgIHJldHVybiB0aGlzLnByb2ZpbGVTZXJ2aWNlLmdldCgpLnBpcGUoXG4gICAgICB0YXAocHJvZmlsZSA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICBwcm9maWxlLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oVXBkYXRlUHJvZmlsZSlcbiAgcHJvZmlsZVVwZGF0ZSh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFByb2ZpbGUuU3RhdGU+LCB7IHBheWxvYWQgfTogVXBkYXRlUHJvZmlsZSkge1xuICAgIHJldHVybiB0aGlzLnByb2ZpbGVTZXJ2aWNlLnVwZGF0ZShwYXlsb2FkKS5waXBlKFxuICAgICAgdGFwKHByb2ZpbGUgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgcHJvZmlsZSxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKENoYW5nZVBhc3N3b3JkKVxuICBjaGFuZ2VQYXNzd29yZChfLCB7IHBheWxvYWQgfTogQ2hhbmdlUGFzc3dvcmQpIHtcbiAgICByZXR1cm4gdGhpcy5wcm9maWxlU2VydmljZS5jaGFuZ2VQYXNzd29yZChwYXlsb2FkLCB0cnVlKTtcbiAgfVxufVxuIl19