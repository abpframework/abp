/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { from } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { GetAppConfiguration } from '../actions/config.actions';
import { SetLanguage, SetTenant } from '../actions/session.actions';
import { LocalizationService } from '../services/localization.service';
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
        return dispatch(new GetAppConfiguration()).pipe(switchMap((/**
         * @return {?}
         */
        () => from(this.localizationService.registerLocale(payload)))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    setTenant({ patchState }, { payload }) {
        patchState({
            tenant: payload,
        });
    }
};
SessionState.ctorParameters = () => [
    { type: LocalizationService }
];
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
], SessionState.prototype, "setTenant", null);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2Vzc2lvbi5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvc2Vzc2lvbi5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLElBQUksRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUM1QixPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDM0MsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0sMkJBQTJCLENBQUM7QUFDaEUsT0FBTyxFQUFFLFdBQVcsRUFBRSxTQUFTLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUVwRSxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSxrQ0FBa0MsQ0FBQztJQU0xRCxZQUFZLFNBQVosWUFBWTs7OztJQVd2QixZQUFvQixtQkFBd0M7UUFBeEMsd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtJQUFHLENBQUM7Ozs7O0lBVGhFLE1BQU0sQ0FBQyxXQUFXLENBQUMsRUFBRSxRQUFRLEVBQWlCO1FBQzVDLE9BQU8sUUFBUSxDQUFDO0lBQ2xCLENBQUM7Ozs7O0lBR0QsTUFBTSxDQUFDLFNBQVMsQ0FBQyxFQUFFLE1BQU0sRUFBaUI7UUFDeEMsT0FBTyxNQUFNLENBQUM7SUFDaEIsQ0FBQzs7Ozs7O0lBS0QsV0FBVyxDQUFDLEVBQUUsVUFBVSxFQUFFLFFBQVEsRUFBK0IsRUFBRSxFQUFFLE9BQU8sRUFBZTtRQUN6RixVQUFVLENBQUM7WUFDVCxRQUFRLEVBQUUsT0FBTztTQUNsQixDQUFDLENBQUM7UUFFSCxPQUFPLFFBQVEsQ0FBQyxJQUFJLG1CQUFtQixFQUFFLENBQUMsQ0FBQyxJQUFJLENBQzdDLFNBQVM7OztRQUFDLEdBQUcsRUFBRSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsbUJBQW1CLENBQUMsY0FBYyxDQUFDLE9BQU8sQ0FBQyxDQUFDLEVBQUMsQ0FDeEUsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELFNBQVMsQ0FBQyxFQUFFLFVBQVUsRUFBK0IsRUFBRSxFQUFFLE9BQU8sRUFBYTtRQUMzRSxVQUFVLENBQUM7WUFDVCxNQUFNLEVBQUUsT0FBTztTQUNoQixDQUFDLENBQUM7SUFDTCxDQUFDO0NBQ0YsQ0FBQTs7WUFuQjBDLG1CQUFtQjs7QUFHNUQ7SUFEQyxNQUFNLENBQUMsV0FBVyxDQUFDOztxREFDNEQsV0FBVzs7K0NBUTFGO0FBR0Q7SUFEQyxNQUFNLENBQUMsU0FBUyxDQUFDOztxREFDa0QsU0FBUzs7NkNBSTVFO0FBM0JEO0lBREMsUUFBUSxFQUFFOzs7O3FDQUdWO0FBR0Q7SUFEQyxRQUFRLEVBQUU7Ozs7bUNBR1Y7QUFUVSxZQUFZO0lBSnhCLEtBQUssQ0FBZ0I7UUFDcEIsSUFBSSxFQUFFLGNBQWM7UUFDcEIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsRUFBaUI7S0FDOUIsQ0FBQzs2Q0FZeUMsbUJBQW1CO0dBWGpELFlBQVksQ0E4QnhCO1NBOUJZLFlBQVk7Ozs7OztJQVdYLDJDQUFnRCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IGZyb20gfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgc3dpdGNoTWFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5pbXBvcnQgeyBHZXRBcHBDb25maWd1cmF0aW9uIH0gZnJvbSAnLi4vYWN0aW9ucy9jb25maWcuYWN0aW9ucyc7XHJcbmltcG9ydCB7IFNldExhbmd1YWdlLCBTZXRUZW5hbnQgfSBmcm9tICcuLi9hY3Rpb25zL3Nlc3Npb24uYWN0aW9ucyc7XHJcbmltcG9ydCB7IEFCUCwgU2Vzc2lvbiB9IGZyb20gJy4uL21vZGVscyc7XHJcbmltcG9ydCB7IExvY2FsaXphdGlvblNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9sb2NhbGl6YXRpb24uc2VydmljZSc7XHJcblxyXG5AU3RhdGU8U2Vzc2lvbi5TdGF0ZT4oe1xyXG4gIG5hbWU6ICdTZXNzaW9uU3RhdGUnLFxyXG4gIGRlZmF1bHRzOiB7fSBhcyBTZXNzaW9uLlN0YXRlLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgU2Vzc2lvblN0YXRlIHtcclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXRMYW5ndWFnZSh7IGxhbmd1YWdlIH06IFNlc3Npb24uU3RhdGUpOiBzdHJpbmcge1xyXG4gICAgcmV0dXJuIGxhbmd1YWdlO1xyXG4gIH1cclxuXHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0VGVuYW50KHsgdGVuYW50IH06IFNlc3Npb24uU3RhdGUpOiBBQlAuQmFzaWNJdGVtIHtcclxuICAgIHJldHVybiB0ZW5hbnQ7XHJcbiAgfVxyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGxvY2FsaXphdGlvblNlcnZpY2U6IExvY2FsaXphdGlvblNlcnZpY2UpIHt9XHJcblxyXG4gIEBBY3Rpb24oU2V0TGFuZ3VhZ2UpXHJcbiAgc2V0TGFuZ3VhZ2UoeyBwYXRjaFN0YXRlLCBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8U2Vzc2lvbi5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBTZXRMYW5ndWFnZSkge1xyXG4gICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgIGxhbmd1YWdlOiBwYXlsb2FkLFxyXG4gICAgfSk7XHJcblxyXG4gICAgcmV0dXJuIGRpc3BhdGNoKG5ldyBHZXRBcHBDb25maWd1cmF0aW9uKCkpLnBpcGUoXHJcbiAgICAgIHN3aXRjaE1hcCgoKSA9PiBmcm9tKHRoaXMubG9jYWxpemF0aW9uU2VydmljZS5yZWdpc3RlckxvY2FsZShwYXlsb2FkKSkpLFxyXG4gICAgKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oU2V0VGVuYW50KVxyXG4gIHNldFRlbmFudCh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFNlc3Npb24uU3RhdGU+LCB7IHBheWxvYWQgfTogU2V0VGVuYW50KSB7XHJcbiAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgdGVuYW50OiBwYXlsb2FkLFxyXG4gICAgfSk7XHJcbiAgfVxyXG59XHJcbiJdfQ==