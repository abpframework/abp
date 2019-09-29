/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { SetLanguage, SetTenant } from '../actions/session.actions';
import { GetAppConfiguration } from '../actions/config.actions';
import { LocalizationService } from '../services/localization.service';
import { from, combineLatest } from 'rxjs';
let SessionState = class SessionState {
    /**
     * @param {?} localizationService
     */
    constructor(localizationService) {
        this.localizationService = localizationService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getLanguage({ language }) {
        return language;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getTenant({ tenant }) {
        return tenant;
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    setLanguage({ patchState, dispatch }, { payload }) {
        patchState({
            language: payload,
        });
        return combineLatest([dispatch(new GetAppConfiguration()), from(this.localizationService.registerLocale(payload))]);
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    setTenantId({ patchState }, { payload }) {
        patchState({
            tenant: payload,
        });
    }
};
tslib_1.__decorate([
    Action(SetLanguage),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, SetLanguage]),
    tslib_1.__metadata("design:returntype", void 0)
], SessionState.prototype, "setLanguage", null);
tslib_1.__decorate([
    Action(SetTenant),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, SetTenant]),
    tslib_1.__metadata("design:returntype", void 0)
], SessionState.prototype, "setTenantId", null);
tslib_1.__decorate([
    Selector(),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object]),
    tslib_1.__metadata("design:returntype", String)
], SessionState, "getLanguage", null);
tslib_1.__decorate([
    Selector(),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object]),
    tslib_1.__metadata("design:returntype", Object)
], SessionState, "getTenant", null);
SessionState = tslib_1.__decorate([
    State({
        name: 'SessionState',
        defaults: (/** @type {?} */ ({})),
    }),
    tslib_1.__metadata("design:paramtypes", [LocalizationService])
], SessionState);
export { SessionState };
if (false) {
    /**
     * @type {?}
     * @private
     */
    SessionState.prototype.localizationService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2Vzc2lvbi5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvc2Vzc2lvbi5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFdBQVcsRUFBRSxTQUFTLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUVwRSxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSwyQkFBMkIsQ0FBQztBQUNoRSxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSxrQ0FBa0MsQ0FBQztBQUN2RSxPQUFPLEVBQUUsSUFBSSxFQUFFLGFBQWEsRUFBRSxNQUFNLE1BQU0sQ0FBQztJQU05QixZQUFZLFNBQVosWUFBWTs7OztJQVd2QixZQUFvQixtQkFBd0M7UUFBeEMsd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtJQUFHLENBQUM7Ozs7O0lBVGhFLE1BQU0sQ0FBQyxXQUFXLENBQUMsRUFBRSxRQUFRLEVBQWlCO1FBQzVDLE9BQU8sUUFBUSxDQUFDO0lBQ2xCLENBQUM7Ozs7O0lBR0QsTUFBTSxDQUFDLFNBQVMsQ0FBQyxFQUFFLE1BQU0sRUFBaUI7UUFDeEMsT0FBTyxNQUFNLENBQUM7SUFDaEIsQ0FBQzs7Ozs7O0lBS0QsV0FBVyxDQUFDLEVBQUUsVUFBVSxFQUFFLFFBQVEsRUFBK0IsRUFBRSxFQUFFLE9BQU8sRUFBZTtRQUN6RixVQUFVLENBQUM7WUFDVCxRQUFRLEVBQUUsT0FBTztTQUNsQixDQUFDLENBQUM7UUFFSCxPQUFPLGFBQWEsQ0FBQyxDQUFDLFFBQVEsQ0FBQyxJQUFJLG1CQUFtQixFQUFFLENBQUMsRUFBRSxJQUFJLENBQUMsSUFBSSxDQUFDLG1CQUFtQixDQUFDLGNBQWMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQztJQUN0SCxDQUFDOzs7Ozs7SUFHRCxXQUFXLENBQUMsRUFBRSxVQUFVLEVBQStCLEVBQUUsRUFBRSxPQUFPLEVBQWE7UUFDN0UsVUFBVSxDQUFDO1lBQ1QsTUFBTSxFQUFFLE9BQU87U0FDaEIsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztDQUNGLENBQUE7QUFkQztJQURDLE1BQU0sQ0FBQyxXQUFXLENBQUM7O3FEQUM0RCxXQUFXOzsrQ0FNMUY7QUFHRDtJQURDLE1BQU0sQ0FBQyxTQUFTLENBQUM7O3FEQUNvRCxTQUFTOzsrQ0FJOUU7QUF6QkQ7SUFEQyxRQUFRLEVBQUU7Ozs7cUNBR1Y7QUFHRDtJQURDLFFBQVEsRUFBRTs7OzttQ0FHVjtBQVRVLFlBQVk7SUFKeEIsS0FBSyxDQUFnQjtRQUNwQixJQUFJLEVBQUUsY0FBYztRQUNwQixRQUFRLEVBQUUsbUJBQUEsRUFBRSxFQUFpQjtLQUM5QixDQUFDOzZDQVl5QyxtQkFBbUI7R0FYakQsWUFBWSxDQTRCeEI7U0E1QlksWUFBWTs7Ozs7O0lBV1gsMkNBQWdEIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBTZWxlY3RvciwgU3RhdGUsIFN0YXRlQ29udGV4dCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IFNldExhbmd1YWdlLCBTZXRUZW5hbnQgfSBmcm9tICcuLi9hY3Rpb25zL3Nlc3Npb24uYWN0aW9ucyc7XG5pbXBvcnQgeyBBQlAsIFNlc3Npb24gfSBmcm9tICcuLi9tb2RlbHMnO1xuaW1wb3J0IHsgR2V0QXBwQ29uZmlndXJhdGlvbiB9IGZyb20gJy4uL2FjdGlvbnMvY29uZmlnLmFjdGlvbnMnO1xuaW1wb3J0IHsgTG9jYWxpemF0aW9uU2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL2xvY2FsaXphdGlvbi5zZXJ2aWNlJztcbmltcG9ydCB7IGZyb20sIGNvbWJpbmVMYXRlc3QgfSBmcm9tICdyeGpzJztcblxuQFN0YXRlPFNlc3Npb24uU3RhdGU+KHtcbiAgbmFtZTogJ1Nlc3Npb25TdGF0ZScsXG4gIGRlZmF1bHRzOiB7fSBhcyBTZXNzaW9uLlN0YXRlLFxufSlcbmV4cG9ydCBjbGFzcyBTZXNzaW9uU3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0TGFuZ3VhZ2UoeyBsYW5ndWFnZSB9OiBTZXNzaW9uLlN0YXRlKTogc3RyaW5nIHtcbiAgICByZXR1cm4gbGFuZ3VhZ2U7XG4gIH1cblxuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0VGVuYW50KHsgdGVuYW50IH06IFNlc3Npb24uU3RhdGUpOiBBQlAuQmFzaWNJdGVtIHtcbiAgICByZXR1cm4gdGVuYW50O1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBsb2NhbGl6YXRpb25TZXJ2aWNlOiBMb2NhbGl6YXRpb25TZXJ2aWNlKSB7fVxuXG4gIEBBY3Rpb24oU2V0TGFuZ3VhZ2UpXG4gIHNldExhbmd1YWdlKHsgcGF0Y2hTdGF0ZSwgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PFNlc3Npb24uU3RhdGU+LCB7IHBheWxvYWQgfTogU2V0TGFuZ3VhZ2UpIHtcbiAgICBwYXRjaFN0YXRlKHtcbiAgICAgIGxhbmd1YWdlOiBwYXlsb2FkLFxuICAgIH0pO1xuXG4gICAgcmV0dXJuIGNvbWJpbmVMYXRlc3QoW2Rpc3BhdGNoKG5ldyBHZXRBcHBDb25maWd1cmF0aW9uKCkpLCBmcm9tKHRoaXMubG9jYWxpemF0aW9uU2VydmljZS5yZWdpc3RlckxvY2FsZShwYXlsb2FkKSldKTtcbiAgfVxuXG4gIEBBY3Rpb24oU2V0VGVuYW50KVxuICBzZXRUZW5hbnRJZCh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFNlc3Npb24uU3RhdGU+LCB7IHBheWxvYWQgfTogU2V0VGVuYW50KSB7XG4gICAgcGF0Y2hTdGF0ZSh7XG4gICAgICB0ZW5hbnQ6IHBheWxvYWQsXG4gICAgfSk7XG4gIH1cbn1cbiJdfQ==