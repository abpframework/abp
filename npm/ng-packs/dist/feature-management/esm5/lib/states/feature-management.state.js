/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { tap } from 'rxjs/operators';
import { GetFeatures, UpdateFeatures } from '../actions/feature-management.actions';
import { FeatureManagementService } from '../services/feature-management.service';
var FeatureManagementState = /** @class */ (function () {
    function FeatureManagementState(featureManagementService) {
        this.featureManagementService = featureManagementService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    FeatureManagementState.getFeatures = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var features = _a.features;
        return features;
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    FeatureManagementState.prototype.getFeatures = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState;
        var payload = _b.payload;
        return this.featureManagementService.getFeatures(payload).pipe(tap((/**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var features = _a.features;
            return patchState({
                features: features,
            });
        })));
    };
    /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    FeatureManagementState.prototype.updateFeatures = /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    function (_, _a) {
        var payload = _a.payload;
        return this.featureManagementService.updateFeatures(payload);
    };
    tslib_1.__decorate([
        Action(GetFeatures),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, GetFeatures]),
        tslib_1.__metadata("design:returntype", void 0)
    ], FeatureManagementState.prototype, "getFeatures", null);
    tslib_1.__decorate([
        Action(UpdateFeatures),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, UpdateFeatures]),
        tslib_1.__metadata("design:returntype", void 0)
    ], FeatureManagementState.prototype, "updateFeatures", null);
    tslib_1.__decorate([
        Selector(),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object]),
        tslib_1.__metadata("design:returntype", void 0)
    ], FeatureManagementState, "getFeatures", null);
    FeatureManagementState = tslib_1.__decorate([
        State({
            name: 'FeatureManagementState',
            defaults: (/** @type {?} */ ({ features: {} })),
        }),
        tslib_1.__metadata("design:paramtypes", [FeatureManagementService])
    ], FeatureManagementState);
    return FeatureManagementState;
}());
export { FeatureManagementState };
if (false) {
    /**
     * @type {?}
     * @private
     */
    FeatureManagementState.prototype.featureManagementService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmVhdHVyZS1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5mZWF0dXJlLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL2ZlYXR1cmUtbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLEdBQUcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3JDLE9BQU8sRUFBRSxXQUFXLEVBQUUsY0FBYyxFQUFFLE1BQU0sdUNBQXVDLENBQUM7QUFFcEYsT0FBTyxFQUFFLHdCQUF3QixFQUFFLE1BQU0sd0NBQXdDLENBQUM7O0lBWWhGLGdDQUFvQix3QkFBa0Q7UUFBbEQsNkJBQXdCLEdBQXhCLHdCQUF3QixDQUEwQjtJQUFHLENBQUM7Ozs7O0lBSm5FLGtDQUFXOzs7O0lBQWxCLFVBQW1CLEVBQXFDO1lBQW5DLHNCQUFRO1FBQzNCLE9BQU8sUUFBUSxDQUFDO0lBQ2xCLENBQUM7Ozs7OztJQUtELDRDQUFXOzs7OztJQUFYLFVBQVksRUFBcUQsRUFBRSxFQUF3QjtZQUE3RSwwQkFBVTtZQUE2QyxvQkFBTztRQUMxRSxPQUFPLElBQUksQ0FBQyx3QkFBd0IsQ0FBQyxXQUFXLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUM1RCxHQUFHOzs7O1FBQUMsVUFBQyxFQUFZO2dCQUFWLHNCQUFRO1lBQ2IsT0FBQSxVQUFVLENBQUM7Z0JBQ1QsUUFBUSxVQUFBO2FBQ1QsQ0FBQztRQUZGLENBRUUsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCwrQ0FBYzs7Ozs7SUFBZCxVQUFlLENBQUMsRUFBRSxFQUEyQjtZQUF6QixvQkFBTztRQUN6QixPQUFPLElBQUksQ0FBQyx3QkFBd0IsQ0FBQyxjQUFjLENBQUMsT0FBTyxDQUFDLENBQUM7SUFDL0QsQ0FBQztJQWJEO1FBREMsTUFBTSxDQUFDLFdBQVcsQ0FBQzs7eURBQzRELFdBQVc7OzZEQVExRjtJQUdEO1FBREMsTUFBTSxDQUFDLGNBQWMsQ0FBQzs7eURBQ1EsY0FBYzs7Z0VBRTVDO0lBcEJEO1FBREMsUUFBUSxFQUFFOzs7O21EQUdWO0lBSlUsc0JBQXNCO1FBSmxDLEtBQUssQ0FBMEI7WUFDOUIsSUFBSSxFQUFFLHdCQUF3QjtZQUM5QixRQUFRLEVBQUUsbUJBQUEsRUFBRSxRQUFRLEVBQUUsRUFBRSxFQUFFLEVBQTJCO1NBQ3RELENBQUM7aURBTzhDLHdCQUF3QjtPQU4zRCxzQkFBc0IsQ0F1QmxDO0lBQUQsNkJBQUM7Q0FBQSxJQUFBO1NBdkJZLHNCQUFzQjs7Ozs7O0lBTXJCLDBEQUEwRCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyB0YXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgeyBHZXRGZWF0dXJlcywgVXBkYXRlRmVhdHVyZXMgfSBmcm9tICcuLi9hY3Rpb25zL2ZlYXR1cmUtbWFuYWdlbWVudC5hY3Rpb25zJztcbmltcG9ydCB7IEZlYXR1cmVNYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL2ZlYXR1cmUtbWFuYWdlbWVudCc7XG5pbXBvcnQgeyBGZWF0dXJlTWFuYWdlbWVudFNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9mZWF0dXJlLW1hbmFnZW1lbnQuc2VydmljZSc7XG5cbkBTdGF0ZTxGZWF0dXJlTWFuYWdlbWVudC5TdGF0ZT4oe1xuICBuYW1lOiAnRmVhdHVyZU1hbmFnZW1lbnRTdGF0ZScsXG4gIGRlZmF1bHRzOiB7IGZlYXR1cmVzOiB7fSB9IGFzIEZlYXR1cmVNYW5hZ2VtZW50LlN0YXRlLFxufSlcbmV4cG9ydCBjbGFzcyBGZWF0dXJlTWFuYWdlbWVudFN0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldEZlYXR1cmVzKHsgZmVhdHVyZXMgfTogRmVhdHVyZU1hbmFnZW1lbnQuU3RhdGUpIHtcbiAgICByZXR1cm4gZmVhdHVyZXM7XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGZlYXR1cmVNYW5hZ2VtZW50U2VydmljZTogRmVhdHVyZU1hbmFnZW1lbnRTZXJ2aWNlKSB7fVxuXG4gIEBBY3Rpb24oR2V0RmVhdHVyZXMpXG4gIGdldEZlYXR1cmVzKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8RmVhdHVyZU1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0RmVhdHVyZXMpIHtcbiAgICByZXR1cm4gdGhpcy5mZWF0dXJlTWFuYWdlbWVudFNlcnZpY2UuZ2V0RmVhdHVyZXMocGF5bG9hZCkucGlwZShcbiAgICAgIHRhcCgoeyBmZWF0dXJlcyB9KSA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICBmZWF0dXJlcyxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKFVwZGF0ZUZlYXR1cmVzKVxuICB1cGRhdGVGZWF0dXJlcyhfLCB7IHBheWxvYWQgfTogVXBkYXRlRmVhdHVyZXMpIHtcbiAgICByZXR1cm4gdGhpcy5mZWF0dXJlTWFuYWdlbWVudFNlcnZpY2UudXBkYXRlRmVhdHVyZXMocGF5bG9hZCk7XG4gIH1cbn1cbiJdfQ==