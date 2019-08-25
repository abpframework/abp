/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { SetLanguage, SetTenant } from '../actions/session.actions';
var SessionState = /** @class */ (function () {
    function SessionState() {
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
        var patchState = _a.patchState;
        var payload = _b.payload;
        patchState({
            language: payload,
        });
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    SessionState.prototype.setTenantId = /**
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
        tslib_1.__metadata("design:paramtypes", [])
    ], SessionState);
    return SessionState;
}());
export { SessionState };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2Vzc2lvbi5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvc2Vzc2lvbi5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFdBQVcsRUFBRSxTQUFTLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQzs7SUFrQmxFO0lBQWUsQ0FBQzs7Ozs7SUFUVCx3QkFBVzs7OztJQUFsQixVQUFtQixFQUEyQjtZQUF6QixzQkFBUTtRQUMzQixPQUFPLFFBQVEsQ0FBQztJQUNsQixDQUFDOzs7OztJQUdNLHNCQUFTOzs7O0lBQWhCLFVBQWlCLEVBQXlCO1lBQXZCLGtCQUFNO1FBQ3ZCLE9BQU8sTUFBTSxDQUFDO0lBQ2hCLENBQUM7Ozs7OztJQUtELGtDQUFXOzs7OztJQUFYLFVBQVksRUFBMkMsRUFBRSxFQUF3QjtZQUFuRSwwQkFBVTtZQUFtQyxvQkFBTztRQUNoRSxVQUFVLENBQUM7WUFDVCxRQUFRLEVBQUUsT0FBTztTQUNsQixDQUFDLENBQUM7SUFDTCxDQUFDOzs7Ozs7SUFHRCxrQ0FBVzs7Ozs7SUFBWCxVQUFZLEVBQTJDLEVBQUUsRUFBc0I7WUFBakUsMEJBQVU7WUFBbUMsb0JBQU87UUFDaEUsVUFBVSxDQUFDO1lBQ1QsTUFBTSxFQUFFLE9BQU87U0FDaEIsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztJQVhEO1FBREMsTUFBTSxDQUFDLFdBQVcsQ0FBQzs7eURBQ2tELFdBQVc7O21EQUloRjtJQUdEO1FBREMsTUFBTSxDQUFDLFNBQVMsQ0FBQzs7eURBQ29ELFNBQVM7O21EQUk5RTtJQXZCRDtRQURDLFFBQVEsRUFBRTs7Ozt5Q0FHVjtJQUdEO1FBREMsUUFBUSxFQUFFOzs7O3VDQUdWO0lBVFUsWUFBWTtRQUp4QixLQUFLLENBQWdCO1lBQ3BCLElBQUksRUFBRSxjQUFjO1lBQ3BCLFFBQVEsRUFBRSxtQkFBQSxFQUFFLEVBQWlCO1NBQzlCLENBQUM7O09BQ1csWUFBWSxDQTBCeEI7SUFBRCxtQkFBQztDQUFBLElBQUE7U0ExQlksWUFBWSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBTZXRMYW5ndWFnZSwgU2V0VGVuYW50IH0gZnJvbSAnLi4vYWN0aW9ucy9zZXNzaW9uLmFjdGlvbnMnO1xuaW1wb3J0IHsgQUJQLCBTZXNzaW9uIH0gZnJvbSAnLi4vbW9kZWxzJztcblxuQFN0YXRlPFNlc3Npb24uU3RhdGU+KHtcbiAgbmFtZTogJ1Nlc3Npb25TdGF0ZScsXG4gIGRlZmF1bHRzOiB7fSBhcyBTZXNzaW9uLlN0YXRlLFxufSlcbmV4cG9ydCBjbGFzcyBTZXNzaW9uU3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0TGFuZ3VhZ2UoeyBsYW5ndWFnZSB9OiBTZXNzaW9uLlN0YXRlKTogc3RyaW5nIHtcbiAgICByZXR1cm4gbGFuZ3VhZ2U7XG4gIH1cblxuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0VGVuYW50KHsgdGVuYW50IH06IFNlc3Npb24uU3RhdGUpOiBBQlAuQmFzaWNJdGVtIHtcbiAgICByZXR1cm4gdGVuYW50O1xuICB9XG5cbiAgY29uc3RydWN0b3IoKSB7fVxuXG4gIEBBY3Rpb24oU2V0TGFuZ3VhZ2UpXG4gIHNldExhbmd1YWdlKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8U2Vzc2lvbi5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBTZXRMYW5ndWFnZSkge1xuICAgIHBhdGNoU3RhdGUoe1xuICAgICAgbGFuZ3VhZ2U6IHBheWxvYWQsXG4gICAgfSk7XG4gIH1cblxuICBAQWN0aW9uKFNldFRlbmFudClcbiAgc2V0VGVuYW50SWQoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxTZXNzaW9uLlN0YXRlPiwgeyBwYXlsb2FkIH06IFNldFRlbmFudCkge1xuICAgIHBhdGNoU3RhdGUoe1xuICAgICAgdGVuYW50OiBwYXlsb2FkLFxuICAgIH0pO1xuICB9XG59XG4iXX0=