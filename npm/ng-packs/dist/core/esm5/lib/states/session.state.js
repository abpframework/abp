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
var SessionState = /** @class */ (function () {
    function SessionState(localizationService) {
        this.localizationService = localizationService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    SessionState.getLanguage = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var language = _a.language;
        return language;
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    SessionState.getTenant = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var tenant = _a.tenant;
        return tenant;
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    SessionState.prototype.setLanguage = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var _this = this;
        var patchState = _a.patchState, dispatch = _a.dispatch;
        var payload = _b.payload;
        patchState({
            language: payload,
        });
        return dispatch(new GetAppConfiguration()).pipe(switchMap((/**
         * @return {?}
         */
        function () { return from(_this.localizationService.registerLocale(payload)); })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    SessionState.prototype.setTenant = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState;
        var payload = _b.payload;
        patchState({
            tenant: payload,
        });
    };
    SessionState.ctorParameters = function () { return [
        { type: LocalizationService }
    ]; };
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
    return SessionState;
}());
export { SessionState };
if (false) {
    /**
     * @type {?}
     * @private
     */
    SessionState.prototype.localizationService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2Vzc2lvbi5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvc2Vzc2lvbi5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLElBQUksRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUM1QixPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDM0MsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0sMkJBQTJCLENBQUM7QUFDaEUsT0FBTyxFQUFFLFdBQVcsRUFBRSxTQUFTLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUVwRSxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSxrQ0FBa0MsQ0FBQzs7SUFpQnJFLHNCQUFvQixtQkFBd0M7UUFBeEMsd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtJQUFHLENBQUM7Ozs7O0lBVHpELHdCQUFXOzs7O0lBQWxCLFVBQW1CLEVBQTJCO1lBQXpCLHNCQUFRO1FBQzNCLE9BQU8sUUFBUSxDQUFDO0lBQ2xCLENBQUM7Ozs7O0lBR00sc0JBQVM7Ozs7SUFBaEIsVUFBaUIsRUFBeUI7WUFBdkIsa0JBQU07UUFDdkIsT0FBTyxNQUFNLENBQUM7SUFDaEIsQ0FBQzs7Ozs7O0lBS0Qsa0NBQVc7Ozs7O0lBQVgsVUFBWSxFQUFxRCxFQUFFLEVBQXdCO1FBRDNGLGlCQVNDO1lBUmEsMEJBQVUsRUFBRSxzQkFBUTtZQUFtQyxvQkFBTztRQUMxRSxVQUFVLENBQUM7WUFDVCxRQUFRLEVBQUUsT0FBTztTQUNsQixDQUFDLENBQUM7UUFFSCxPQUFPLFFBQVEsQ0FBQyxJQUFJLG1CQUFtQixFQUFFLENBQUMsQ0FBQyxJQUFJLENBQzdDLFNBQVM7OztRQUFDLGNBQU0sT0FBQSxJQUFJLENBQUMsS0FBSSxDQUFDLG1CQUFtQixDQUFDLGNBQWMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxFQUF0RCxDQUFzRCxFQUFDLENBQ3hFLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCxnQ0FBUzs7Ozs7SUFBVCxVQUFVLEVBQTJDLEVBQUUsRUFBc0I7WUFBakUsMEJBQVU7WUFBbUMsb0JBQU87UUFDOUQsVUFBVSxDQUFDO1lBQ1QsTUFBTSxFQUFFLE9BQU87U0FDaEIsQ0FBQyxDQUFDO0lBQ0wsQ0FBQzs7Z0JBbEJ3QyxtQkFBbUI7O0lBRzVEO1FBREMsTUFBTSxDQUFDLFdBQVcsQ0FBQzs7eURBQzRELFdBQVc7O21EQVExRjtJQUdEO1FBREMsTUFBTSxDQUFDLFNBQVMsQ0FBQzs7eURBQ2tELFNBQVM7O2lEQUk1RTtJQTNCRDtRQURDLFFBQVEsRUFBRTs7Ozt5Q0FHVjtJQUdEO1FBREMsUUFBUSxFQUFFOzs7O3VDQUdWO0lBVFUsWUFBWTtRQUp4QixLQUFLLENBQWdCO1lBQ3BCLElBQUksRUFBRSxjQUFjO1lBQ3BCLFFBQVEsRUFBRSxtQkFBQSxFQUFFLEVBQWlCO1NBQzlCLENBQUM7aURBWXlDLG1CQUFtQjtPQVhqRCxZQUFZLENBOEJ4QjtJQUFELG1CQUFDO0NBQUEsSUFBQTtTQTlCWSxZQUFZOzs7Ozs7SUFXWCwyQ0FBZ0QiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBY3Rpb24sIFNlbGVjdG9yLCBTdGF0ZSwgU3RhdGVDb250ZXh0IH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBmcm9tIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IHN3aXRjaE1hcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcclxuaW1wb3J0IHsgR2V0QXBwQ29uZmlndXJhdGlvbiB9IGZyb20gJy4uL2FjdGlvbnMvY29uZmlnLmFjdGlvbnMnO1xyXG5pbXBvcnQgeyBTZXRMYW5ndWFnZSwgU2V0VGVuYW50IH0gZnJvbSAnLi4vYWN0aW9ucy9zZXNzaW9uLmFjdGlvbnMnO1xyXG5pbXBvcnQgeyBBQlAsIFNlc3Npb24gfSBmcm9tICcuLi9tb2RlbHMnO1xyXG5pbXBvcnQgeyBMb2NhbGl6YXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvbG9jYWxpemF0aW9uLnNlcnZpY2UnO1xyXG5cclxuQFN0YXRlPFNlc3Npb24uU3RhdGU+KHtcclxuICBuYW1lOiAnU2Vzc2lvblN0YXRlJyxcclxuICBkZWZhdWx0czoge30gYXMgU2Vzc2lvbi5TdGF0ZSxcclxufSlcclxuZXhwb3J0IGNsYXNzIFNlc3Npb25TdGF0ZSB7XHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0TGFuZ3VhZ2UoeyBsYW5ndWFnZSB9OiBTZXNzaW9uLlN0YXRlKTogc3RyaW5nIHtcclxuICAgIHJldHVybiBsYW5ndWFnZTtcclxuICB9XHJcblxyXG4gIEBTZWxlY3RvcigpXHJcbiAgc3RhdGljIGdldFRlbmFudCh7IHRlbmFudCB9OiBTZXNzaW9uLlN0YXRlKTogQUJQLkJhc2ljSXRlbSB7XHJcbiAgICByZXR1cm4gdGVuYW50O1xyXG4gIH1cclxuXHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBsb2NhbGl6YXRpb25TZXJ2aWNlOiBMb2NhbGl6YXRpb25TZXJ2aWNlKSB7fVxyXG5cclxuICBAQWN0aW9uKFNldExhbmd1YWdlKVxyXG4gIHNldExhbmd1YWdlKHsgcGF0Y2hTdGF0ZSwgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PFNlc3Npb24uU3RhdGU+LCB7IHBheWxvYWQgfTogU2V0TGFuZ3VhZ2UpIHtcclxuICAgIHBhdGNoU3RhdGUoe1xyXG4gICAgICBsYW5ndWFnZTogcGF5bG9hZCxcclxuICAgIH0pO1xyXG5cclxuICAgIHJldHVybiBkaXNwYXRjaChuZXcgR2V0QXBwQ29uZmlndXJhdGlvbigpKS5waXBlKFxyXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gZnJvbSh0aGlzLmxvY2FsaXphdGlvblNlcnZpY2UucmVnaXN0ZXJMb2NhbGUocGF5bG9hZCkpKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKFNldFRlbmFudClcclxuICBzZXRUZW5hbnQoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxTZXNzaW9uLlN0YXRlPiwgeyBwYXlsb2FkIH06IFNldFRlbmFudCkge1xyXG4gICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgIHRlbmFudDogcGF5bG9hZCxcclxuICAgIH0pO1xyXG4gIH1cclxufVxyXG4iXX0=