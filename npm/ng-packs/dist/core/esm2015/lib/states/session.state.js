/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { State, Action, Selector } from '@ngxs/store';
import { SessionSetLanguage } from '../actions/session.actions';
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
     * @param {?} __1
     * @return {?}
     */
    sessionSetLanguage({ patchState }, { payload }) {
        patchState({
            language: payload,
        });
    }
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
export { SessionState };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2Vzc2lvbi5zdGF0ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvc2Vzc2lvbi5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFnQixRQUFRLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sNEJBQTRCLENBQUM7SUFPbkQsWUFBWSxTQUFaLFlBQVk7SUFNdkIsZ0JBQWUsQ0FBQzs7Ozs7SUFKaEIsTUFBTSxDQUFDLFdBQVcsQ0FBQyxFQUFFLFFBQVEsRUFBaUI7UUFDNUMsT0FBTyxRQUFRLENBQUM7SUFDbEIsQ0FBQzs7Ozs7O0lBS0Qsa0JBQWtCLENBQUMsRUFBRSxVQUFVLEVBQStCLEVBQUUsRUFBRSxPQUFPLEVBQXNCO1FBQzdGLFVBQVUsQ0FBQztZQUNULFFBQVEsRUFBRSxPQUFPO1NBQ2xCLENBQUMsQ0FBQztJQUNMLENBQUM7Q0FDRixDQUFBO0FBTEM7SUFEQyxNQUFNLENBQUMsa0JBQWtCLENBQUM7O3FEQUNrRCxrQkFBa0I7O3NEQUk5RjtBQVhEO0lBREMsUUFBUSxFQUFFOzs7O3FDQUdWO0FBSlUsWUFBWTtJQUp4QixLQUFLLENBQWdCO1FBQ3BCLElBQUksRUFBRSxjQUFjO1FBQ3BCLFFBQVEsRUFBRSxtQkFBQSxFQUFFLEVBQWlCO0tBQzlCLENBQUM7O0dBQ1csWUFBWSxDQWN4QjtTQWRZLFlBQVkiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBTdGF0ZSwgQWN0aW9uLCBTdGF0ZUNvbnRleHQsIFNlbGVjdG9yIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgU2Vzc2lvblNldExhbmd1YWdlIH0gZnJvbSAnLi4vYWN0aW9ucy9zZXNzaW9uLmFjdGlvbnMnO1xuaW1wb3J0IHsgU2Vzc2lvbiB9IGZyb20gJy4uL21vZGVscy9zZXNzaW9uJztcblxuQFN0YXRlPFNlc3Npb24uU3RhdGU+KHtcbiAgbmFtZTogJ1Nlc3Npb25TdGF0ZScsXG4gIGRlZmF1bHRzOiB7fSBhcyBTZXNzaW9uLlN0YXRlLFxufSlcbmV4cG9ydCBjbGFzcyBTZXNzaW9uU3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0TGFuZ3VhZ2UoeyBsYW5ndWFnZSB9OiBTZXNzaW9uLlN0YXRlKTogc3RyaW5nIHtcbiAgICByZXR1cm4gbGFuZ3VhZ2U7XG4gIH1cblxuICBjb25zdHJ1Y3RvcigpIHt9XG5cbiAgQEFjdGlvbihTZXNzaW9uU2V0TGFuZ3VhZ2UpXG4gIHNlc3Npb25TZXRMYW5ndWFnZSh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFNlc3Npb24uU3RhdGU+LCB7IHBheWxvYWQgfTogU2Vzc2lvblNldExhbmd1YWdlKSB7XG4gICAgcGF0Y2hTdGF0ZSh7XG4gICAgICBsYW5ndWFnZTogcGF5bG9hZCxcbiAgICB9KTtcbiAgfVxufVxuIl19