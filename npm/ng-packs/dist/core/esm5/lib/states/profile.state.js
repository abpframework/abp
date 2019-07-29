/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { State, Action, Selector } from '@ngxs/store';
import { ProfileGet, ProfileChangePassword, ProfileUpdate } from '../actions/profile.actions';
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
        return this.profileService.changePassword(payload);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicHJvZmlsZS5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvcHJvZmlsZS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFnQixRQUFRLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFVBQVUsRUFBRSxxQkFBcUIsRUFBRSxhQUFhLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUU5RixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sNkJBQTZCLENBQUM7QUFDN0QsT0FBTyxFQUFFLEdBQUcsRUFBRSxNQUFNLGdCQUFnQixDQUFDOztJQVluQyxzQkFBb0IsY0FBOEI7UUFBOUIsbUJBQWMsR0FBZCxjQUFjLENBQWdCO0lBQUcsQ0FBQzs7Ozs7SUFKL0MsdUJBQVU7Ozs7SUFBakIsVUFBa0IsRUFBMEI7WUFBeEIsb0JBQU87UUFDekIsT0FBTyxPQUFPLENBQUM7SUFDakIsQ0FBQzs7Ozs7SUFLRCxpQ0FBVTs7OztJQUFWLFVBQVcsRUFBMkM7WUFBekMsMEJBQVU7UUFDckIsT0FBTyxJQUFJLENBQUMsY0FBYyxDQUFDLEdBQUcsRUFBRSxDQUFDLElBQUksQ0FDbkMsR0FBRzs7OztRQUFDLFVBQUEsT0FBTztZQUNULE9BQUEsVUFBVSxDQUFDO2dCQUNULE9BQU8sU0FBQTthQUNSLENBQUM7UUFGRixDQUVFLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0Qsb0NBQWE7Ozs7O0lBQWIsVUFBYyxFQUEyQyxFQUFFLEVBQTBCO1lBQXJFLDBCQUFVO1lBQW1DLG9CQUFPO1FBQ2xFLE9BQU8sSUFBSSxDQUFDLGNBQWMsQ0FBQyxNQUFNLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUM3QyxHQUFHOzs7O1FBQUMsVUFBQSxPQUFPO1lBQ1QsT0FBQSxVQUFVLENBQUM7Z0JBQ1QsT0FBTyxTQUFBO2FBQ1IsQ0FBQztRQUZGLENBRUUsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCxxQ0FBYzs7Ozs7SUFBZCxVQUFlLENBQUMsRUFBRSxFQUFrQztZQUFoQyxvQkFBTztRQUN6QixPQUFPLElBQUksQ0FBQyxjQUFjLENBQUMsY0FBYyxDQUFDLE9BQU8sQ0FBQyxDQUFDO0lBQ3JELENBQUM7SUF4QkQ7UUFEQyxNQUFNLENBQUMsVUFBVSxDQUFDOzs7O2tEQVNsQjtJQUdEO1FBREMsTUFBTSxDQUFDLGFBQWEsQ0FBQzs7eURBQ2tELGFBQWE7O3FEQVFwRjtJQUdEO1FBREMsTUFBTSxDQUFDLHFCQUFxQixDQUFDOzt5REFDQyxxQkFBcUI7O3NEQUVuRDtJQS9CRDtRQURDLFFBQVEsRUFBRTs7Ozt3Q0FHVjtJQUpVLFlBQVk7UUFKeEIsS0FBSyxDQUFnQjtZQUNwQixJQUFJLEVBQUUsY0FBYztZQUNwQixRQUFRLEVBQUUsbUJBQUEsRUFBRSxFQUFpQjtTQUM5QixDQUFDO2lEQU9vQyxjQUFjO09BTnZDLFlBQVksQ0FrQ3hCO0lBQUQsbUJBQUM7Q0FBQSxJQUFBO1NBbENZLFlBQVk7Ozs7OztJQU1YLHNDQUFzQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFN0YXRlLCBBY3Rpb24sIFN0YXRlQ29udGV4dCwgU2VsZWN0b3IgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBQcm9maWxlR2V0LCBQcm9maWxlQ2hhbmdlUGFzc3dvcmQsIFByb2ZpbGVVcGRhdGUgfSBmcm9tICcuLi9hY3Rpb25zL3Byb2ZpbGUuYWN0aW9ucyc7XG5pbXBvcnQgeyBQcm9maWxlIH0gZnJvbSAnLi4vbW9kZWxzL3Byb2ZpbGUnO1xuaW1wb3J0IHsgUHJvZmlsZVNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9wcm9maWxlLnNlcnZpY2UnO1xuaW1wb3J0IHsgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuXG5AU3RhdGU8UHJvZmlsZS5TdGF0ZT4oe1xuICBuYW1lOiAnUHJvZmlsZVN0YXRlJyxcbiAgZGVmYXVsdHM6IHt9IGFzIFByb2ZpbGUuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIFByb2ZpbGVTdGF0ZSB7XG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRQcm9maWxlKHsgcHJvZmlsZSB9OiBQcm9maWxlLlN0YXRlKTogUHJvZmlsZS5SZXNwb25zZSB7XG4gICAgcmV0dXJuIHByb2ZpbGU7XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHByb2ZpbGVTZXJ2aWNlOiBQcm9maWxlU2VydmljZSkge31cblxuICBAQWN0aW9uKFByb2ZpbGVHZXQpXG4gIHByb2ZpbGVHZXQoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxQcm9maWxlLlN0YXRlPikge1xuICAgIHJldHVybiB0aGlzLnByb2ZpbGVTZXJ2aWNlLmdldCgpLnBpcGUoXG4gICAgICB0YXAocHJvZmlsZSA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICBwcm9maWxlLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oUHJvZmlsZVVwZGF0ZSlcbiAgcHJvZmlsZVVwZGF0ZSh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFByb2ZpbGUuU3RhdGU+LCB7IHBheWxvYWQgfTogUHJvZmlsZVVwZGF0ZSkge1xuICAgIHJldHVybiB0aGlzLnByb2ZpbGVTZXJ2aWNlLnVwZGF0ZShwYXlsb2FkKS5waXBlKFxuICAgICAgdGFwKHByb2ZpbGUgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgcHJvZmlsZSxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKFByb2ZpbGVDaGFuZ2VQYXNzd29yZClcbiAgY2hhbmdlUGFzc3dvcmQoXywgeyBwYXlsb2FkIH06IFByb2ZpbGVDaGFuZ2VQYXNzd29yZCkge1xuICAgIHJldHVybiB0aGlzLnByb2ZpbGVTZXJ2aWNlLmNoYW5nZVBhc3N3b3JkKHBheWxvYWQpO1xuICB9XG59XG4iXX0=