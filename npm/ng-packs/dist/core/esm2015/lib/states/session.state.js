/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { SetLanguage, SetTenant } from '../actions/session.actions';
let SessionState = class SessionState {
    constructor() { }
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
    setLanguage({ patchState }, { payload }) {
        patchState({
            language: payload,
        });
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
    tslib_1.__metadata("design:paramtypes", [])
], SessionState);
export { SessionState };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2Vzc2lvbi5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvc2Vzc2lvbi5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFdBQVcsRUFBRSxTQUFTLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztJQU92RCxZQUFZLFNBQVosWUFBWTtJQVd2QixnQkFBZSxDQUFDOzs7OztJQVRoQixNQUFNLENBQUMsV0FBVyxDQUFDLEVBQUUsUUFBUSxFQUFpQjtRQUM1QyxPQUFPLFFBQVEsQ0FBQztJQUNsQixDQUFDOzs7OztJQUdELE1BQU0sQ0FBQyxTQUFTLENBQUMsRUFBRSxNQUFNLEVBQWlCO1FBQ3hDLE9BQU8sTUFBTSxDQUFDO0lBQ2hCLENBQUM7Ozs7OztJQUtELFdBQVcsQ0FBQyxFQUFFLFVBQVUsRUFBK0IsRUFBRSxFQUFFLE9BQU8sRUFBZTtRQUMvRSxVQUFVLENBQUM7WUFDVCxRQUFRLEVBQUUsT0FBTztTQUNsQixDQUFDLENBQUM7SUFDTCxDQUFDOzs7Ozs7SUFHRCxXQUFXLENBQUMsRUFBRSxVQUFVLEVBQStCLEVBQUUsRUFBRSxPQUFPLEVBQWE7UUFDN0UsVUFBVSxDQUFDO1lBQ1QsTUFBTSxFQUFFLE9BQU87U0FDaEIsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztDQUNGLENBQUE7QUFaQztJQURDLE1BQU0sQ0FBQyxXQUFXLENBQUM7O3FEQUNrRCxXQUFXOzsrQ0FJaEY7QUFHRDtJQURDLE1BQU0sQ0FBQyxTQUFTLENBQUM7O3FEQUNvRCxTQUFTOzsrQ0FJOUU7QUF2QkQ7SUFEQyxRQUFRLEVBQUU7Ozs7cUNBR1Y7QUFHRDtJQURDLFFBQVEsRUFBRTs7OzttQ0FHVjtBQVRVLFlBQVk7SUFKeEIsS0FBSyxDQUFnQjtRQUNwQixJQUFJLEVBQUUsY0FBYztRQUNwQixRQUFRLEVBQUUsbUJBQUEsRUFBRSxFQUFpQjtLQUM5QixDQUFDOztHQUNXLFlBQVksQ0EwQnhCO1NBMUJZLFlBQVkiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBY3Rpb24sIFNlbGVjdG9yLCBTdGF0ZSwgU3RhdGVDb250ZXh0IH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgU2V0TGFuZ3VhZ2UsIFNldFRlbmFudCB9IGZyb20gJy4uL2FjdGlvbnMvc2Vzc2lvbi5hY3Rpb25zJztcbmltcG9ydCB7IEFCUCwgU2Vzc2lvbiB9IGZyb20gJy4uL21vZGVscyc7XG5cbkBTdGF0ZTxTZXNzaW9uLlN0YXRlPih7XG4gIG5hbWU6ICdTZXNzaW9uU3RhdGUnLFxuICBkZWZhdWx0czoge30gYXMgU2Vzc2lvbi5TdGF0ZSxcbn0pXG5leHBvcnQgY2xhc3MgU2Vzc2lvblN0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldExhbmd1YWdlKHsgbGFuZ3VhZ2UgfTogU2Vzc2lvbi5TdGF0ZSk6IHN0cmluZyB7XG4gICAgcmV0dXJuIGxhbmd1YWdlO1xuICB9XG5cbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldFRlbmFudCh7IHRlbmFudCB9OiBTZXNzaW9uLlN0YXRlKTogQUJQLkJhc2ljSXRlbSB7XG4gICAgcmV0dXJuIHRlbmFudDtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKCkge31cblxuICBAQWN0aW9uKFNldExhbmd1YWdlKVxuICBzZXRMYW5ndWFnZSh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFNlc3Npb24uU3RhdGU+LCB7IHBheWxvYWQgfTogU2V0TGFuZ3VhZ2UpIHtcbiAgICBwYXRjaFN0YXRlKHtcbiAgICAgIGxhbmd1YWdlOiBwYXlsb2FkLFxuICAgIH0pO1xuICB9XG5cbiAgQEFjdGlvbihTZXRUZW5hbnQpXG4gIHNldFRlbmFudElkKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8U2Vzc2lvbi5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBTZXRUZW5hbnQpIHtcbiAgICBwYXRjaFN0YXRlKHtcbiAgICAgIHRlbmFudDogcGF5bG9hZCxcbiAgICB9KTtcbiAgfVxufVxuIl19