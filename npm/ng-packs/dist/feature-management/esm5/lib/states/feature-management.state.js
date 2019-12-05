/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/feature-management.state.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
        return features || [];
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
    FeatureManagementState.ctorParameters = function () { return [
        { type: FeatureManagementService }
    ]; };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmVhdHVyZS1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5mZWF0dXJlLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL2ZlYXR1cmUtbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQWdCLE1BQU0sYUFBYSxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNyQyxPQUFPLEVBQUUsV0FBVyxFQUFFLGNBQWMsRUFBRSxNQUFNLHVDQUF1QyxDQUFDO0FBRXBGLE9BQU8sRUFBRSx3QkFBd0IsRUFBRSxNQUFNLHdDQUF3QyxDQUFDOztJQVloRixnQ0FBb0Isd0JBQWtEO1FBQWxELDZCQUF3QixHQUF4Qix3QkFBd0IsQ0FBMEI7SUFBRyxDQUFDOzs7OztJQUpuRSxrQ0FBVzs7OztJQUFsQixVQUFtQixFQUFxQztZQUFuQyxzQkFBUTtRQUMzQixPQUFPLFFBQVEsSUFBSSxFQUFFLENBQUM7SUFDeEIsQ0FBQzs7Ozs7O0lBS0QsNENBQVc7Ozs7O0lBQVgsVUFBWSxFQUFxRCxFQUFFLEVBQXdCO1lBQTdFLDBCQUFVO1lBQTZDLG9CQUFPO1FBQzFFLE9BQU8sSUFBSSxDQUFDLHdCQUF3QixDQUFDLFdBQVcsQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQzVELEdBQUc7Ozs7UUFBQyxVQUFDLEVBQVk7Z0JBQVYsc0JBQVE7WUFDYixPQUFBLFVBQVUsQ0FBQztnQkFDVCxRQUFRLFVBQUE7YUFDVCxDQUFDO1FBRkYsQ0FFRSxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELCtDQUFjOzs7OztJQUFkLFVBQWUsQ0FBQyxFQUFFLEVBQTJCO1lBQXpCLG9CQUFPO1FBQ3pCLE9BQU8sSUFBSSxDQUFDLHdCQUF3QixDQUFDLGNBQWMsQ0FBQyxPQUFPLENBQUMsQ0FBQztJQUMvRCxDQUFDOztnQkFoQjZDLHdCQUF3Qjs7SUFHdEU7UUFEQyxNQUFNLENBQUMsV0FBVyxDQUFDOzt5REFDNEQsV0FBVzs7NkRBUTFGO0lBR0Q7UUFEQyxNQUFNLENBQUMsY0FBYyxDQUFDOzt5REFDUSxjQUFjOztnRUFFNUM7SUFwQkQ7UUFEQyxRQUFRLEVBQUU7Ozs7bURBR1Y7SUFKVSxzQkFBc0I7UUFKbEMsS0FBSyxDQUEwQjtZQUM5QixJQUFJLEVBQUUsd0JBQXdCO1lBQzlCLFFBQVEsRUFBRSxtQkFBQSxFQUFFLFFBQVEsRUFBRSxFQUFFLEVBQUUsRUFBMkI7U0FDdEQsQ0FBQztpREFPOEMsd0JBQXdCO09BTjNELHNCQUFzQixDQXVCbEM7SUFBRCw2QkFBQztDQUFBLElBQUE7U0F2Qlksc0JBQXNCOzs7Ozs7SUFNckIsMERBQTBEIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBTZWxlY3RvciwgU3RhdGUsIFN0YXRlQ29udGV4dCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7IEdldEZlYXR1cmVzLCBVcGRhdGVGZWF0dXJlcyB9IGZyb20gJy4uL2FjdGlvbnMvZmVhdHVyZS1tYW5hZ2VtZW50LmFjdGlvbnMnO1xuaW1wb3J0IHsgRmVhdHVyZU1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvZmVhdHVyZS1tYW5hZ2VtZW50JztcbmltcG9ydCB7IEZlYXR1cmVNYW5hZ2VtZW50U2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL2ZlYXR1cmUtbWFuYWdlbWVudC5zZXJ2aWNlJztcblxuQFN0YXRlPEZlYXR1cmVNYW5hZ2VtZW50LlN0YXRlPih7XG4gIG5hbWU6ICdGZWF0dXJlTWFuYWdlbWVudFN0YXRlJyxcbiAgZGVmYXVsdHM6IHsgZmVhdHVyZXM6IHt9IH0gYXMgRmVhdHVyZU1hbmFnZW1lbnQuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIEZlYXR1cmVNYW5hZ2VtZW50U3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0RmVhdHVyZXMoeyBmZWF0dXJlcyB9OiBGZWF0dXJlTWFuYWdlbWVudC5TdGF0ZSkge1xuICAgIHJldHVybiBmZWF0dXJlcyB8fCBbXTtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgZmVhdHVyZU1hbmFnZW1lbnRTZXJ2aWNlOiBGZWF0dXJlTWFuYWdlbWVudFNlcnZpY2UpIHt9XG5cbiAgQEFjdGlvbihHZXRGZWF0dXJlcylcbiAgZ2V0RmVhdHVyZXMoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxGZWF0dXJlTWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRGZWF0dXJlcykge1xuICAgIHJldHVybiB0aGlzLmZlYXR1cmVNYW5hZ2VtZW50U2VydmljZS5nZXRGZWF0dXJlcyhwYXlsb2FkKS5waXBlKFxuICAgICAgdGFwKCh7IGZlYXR1cmVzIH0pID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIGZlYXR1cmVzLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oVXBkYXRlRmVhdHVyZXMpXG4gIHVwZGF0ZUZlYXR1cmVzKF8sIHsgcGF5bG9hZCB9OiBVcGRhdGVGZWF0dXJlcykge1xuICAgIHJldHVybiB0aGlzLmZlYXR1cmVNYW5hZ2VtZW50U2VydmljZS51cGRhdGVGZWF0dXJlcyhwYXlsb2FkKTtcbiAgfVxufVxuIl19