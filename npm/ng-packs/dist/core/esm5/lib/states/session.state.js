/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { State, Action, Selector } from '@ngxs/store';
import { SessionSetLanguage } from '../actions/session.actions';
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
     * @param {?} __1
     * @return {?}
     */
    SessionState.prototype.sessionSetLanguage = /**
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
    tslib_1.__decorate([
        Action(SessionSetLanguage),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, SessionSetLanguage]),
        tslib_1.__metadata("design:returntype", void 0)
    ], SessionState.prototype, "sessionSetLanguage", null);
    tslib_1.__decorate([
        Selector(),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object]),
        tslib_1.__metadata("design:returntype", String)
    ], SessionState, "getLanguage", null);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2Vzc2lvbi5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvc2Vzc2lvbi5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFnQixRQUFRLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sNEJBQTRCLENBQUM7O0lBYTlEO0lBQWUsQ0FBQzs7Ozs7SUFKVCx3QkFBVzs7OztJQUFsQixVQUFtQixFQUEyQjtZQUF6QixzQkFBUTtRQUMzQixPQUFPLFFBQVEsQ0FBQztJQUNsQixDQUFDOzs7Ozs7SUFLRCx5Q0FBa0I7Ozs7O0lBQWxCLFVBQW1CLEVBQTJDLEVBQUUsRUFBK0I7WUFBMUUsMEJBQVU7WUFBbUMsb0JBQU87UUFDdkUsVUFBVSxDQUFDO1lBQ1QsUUFBUSxFQUFFLE9BQU87U0FDbEIsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztJQUpEO1FBREMsTUFBTSxDQUFDLGtCQUFrQixDQUFDOzt5REFDa0Qsa0JBQWtCOzswREFJOUY7SUFYRDtRQURDLFFBQVEsRUFBRTs7Ozt5Q0FHVjtJQUpVLFlBQVk7UUFKeEIsS0FBSyxDQUFnQjtZQUNwQixJQUFJLEVBQUUsY0FBYztZQUNwQixRQUFRLEVBQUUsbUJBQUEsRUFBRSxFQUFpQjtTQUM5QixDQUFDOztPQUNXLFlBQVksQ0FjeEI7SUFBRCxtQkFBQztDQUFBLElBQUE7U0FkWSxZQUFZIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhdGUsIEFjdGlvbiwgU3RhdGVDb250ZXh0LCBTZWxlY3RvciB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IFNlc3Npb25TZXRMYW5ndWFnZSB9IGZyb20gJy4uL2FjdGlvbnMvc2Vzc2lvbi5hY3Rpb25zJztcbmltcG9ydCB7IFNlc3Npb24gfSBmcm9tICcuLi9tb2RlbHMvc2Vzc2lvbic7XG5cbkBTdGF0ZTxTZXNzaW9uLlN0YXRlPih7XG4gIG5hbWU6ICdTZXNzaW9uU3RhdGUnLFxuICBkZWZhdWx0czoge30gYXMgU2Vzc2lvbi5TdGF0ZSxcbn0pXG5leHBvcnQgY2xhc3MgU2Vzc2lvblN0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldExhbmd1YWdlKHsgbGFuZ3VhZ2UgfTogU2Vzc2lvbi5TdGF0ZSk6IHN0cmluZyB7XG4gICAgcmV0dXJuIGxhbmd1YWdlO1xuICB9XG5cbiAgY29uc3RydWN0b3IoKSB7fVxuXG4gIEBBY3Rpb24oU2Vzc2lvblNldExhbmd1YWdlKVxuICBzZXNzaW9uU2V0TGFuZ3VhZ2UoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxTZXNzaW9uLlN0YXRlPiwgeyBwYXlsb2FkIH06IFNlc3Npb25TZXRMYW5ndWFnZSkge1xuICAgIHBhdGNoU3RhdGUoe1xuICAgICAgbGFuZ3VhZ2U6IHBheWxvYWQsXG4gICAgfSk7XG4gIH1cbn1cbiJdfQ==