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
let FeatureManagementState = class FeatureManagementState {
    /**
     * @param {?} featureManagementService
     */
    constructor(featureManagementService) {
        this.featureManagementService = featureManagementService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getFeatures({ features }) {
        return features || [];
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    getFeatures({ patchState }, { payload }) {
        return this.featureManagementService.getFeatures(payload).pipe(tap((/**
         * @param {?} __0
         * @return {?}
         */
        ({ features }) => patchState({
            features,
        }))));
    }
    /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    updateFeatures(_, { payload }) {
        return this.featureManagementService.updateFeatures(payload);
    }
};
FeatureManagementState.ctorParameters = () => [
    { type: FeatureManagementService }
];
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
export { FeatureManagementState };
if (false) {
    /**
     * @type {?}
     * @private
     */
    FeatureManagementState.prototype.featureManagementService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmVhdHVyZS1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5mZWF0dXJlLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL2ZlYXR1cmUtbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQWdCLE1BQU0sYUFBYSxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNyQyxPQUFPLEVBQUUsV0FBVyxFQUFFLGNBQWMsRUFBRSxNQUFNLHVDQUF1QyxDQUFDO0FBRXBGLE9BQU8sRUFBRSx3QkFBd0IsRUFBRSxNQUFNLHdDQUF3QyxDQUFDO0lBTXJFLHNCQUFzQixTQUF0QixzQkFBc0I7Ozs7SUFNakMsWUFBb0Isd0JBQWtEO1FBQWxELDZCQUF3QixHQUF4Qix3QkFBd0IsQ0FBMEI7SUFBRyxDQUFDOzs7OztJQUoxRSxNQUFNLENBQUMsV0FBVyxDQUFDLEVBQUUsUUFBUSxFQUEyQjtRQUN0RCxPQUFPLFFBQVEsSUFBSSxFQUFFLENBQUM7SUFDeEIsQ0FBQzs7Ozs7O0lBS0QsV0FBVyxDQUFDLEVBQUUsVUFBVSxFQUF5QyxFQUFFLEVBQUUsT0FBTyxFQUFlO1FBQ3pGLE9BQU8sSUFBSSxDQUFDLHdCQUF3QixDQUFDLFdBQVcsQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQzVELEdBQUc7Ozs7UUFBQyxDQUFDLEVBQUUsUUFBUSxFQUFFLEVBQUUsRUFBRSxDQUNuQixVQUFVLENBQUM7WUFDVCxRQUFRO1NBQ1QsQ0FBQyxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELGNBQWMsQ0FBQyxDQUFDLEVBQUUsRUFBRSxPQUFPLEVBQWtCO1FBQzNDLE9BQU8sSUFBSSxDQUFDLHdCQUF3QixDQUFDLGNBQWMsQ0FBQyxPQUFPLENBQUMsQ0FBQztJQUMvRCxDQUFDO0NBQ0YsQ0FBQTs7WUFqQitDLHdCQUF3Qjs7QUFHdEU7SUFEQyxNQUFNLENBQUMsV0FBVyxDQUFDOztxREFDNEQsV0FBVzs7eURBUTFGO0FBR0Q7SUFEQyxNQUFNLENBQUMsY0FBYyxDQUFDOztxREFDUSxjQUFjOzs0REFFNUM7QUFwQkQ7SUFEQyxRQUFRLEVBQUU7Ozs7K0NBR1Y7QUFKVSxzQkFBc0I7SUFKbEMsS0FBSyxDQUEwQjtRQUM5QixJQUFJLEVBQUUsd0JBQXdCO1FBQzlCLFFBQVEsRUFBRSxtQkFBQSxFQUFFLFFBQVEsRUFBRSxFQUFFLEVBQUUsRUFBMkI7S0FDdEQsQ0FBQzs2Q0FPOEMsd0JBQXdCO0dBTjNELHNCQUFzQixDQXVCbEM7U0F2Qlksc0JBQXNCOzs7Ozs7SUFNckIsMERBQTBEIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBTZWxlY3RvciwgU3RhdGUsIFN0YXRlQ29udGV4dCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5pbXBvcnQgeyBHZXRGZWF0dXJlcywgVXBkYXRlRmVhdHVyZXMgfSBmcm9tICcuLi9hY3Rpb25zL2ZlYXR1cmUtbWFuYWdlbWVudC5hY3Rpb25zJztcclxuaW1wb3J0IHsgRmVhdHVyZU1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvZmVhdHVyZS1tYW5hZ2VtZW50JztcclxuaW1wb3J0IHsgRmVhdHVyZU1hbmFnZW1lbnRTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvZmVhdHVyZS1tYW5hZ2VtZW50LnNlcnZpY2UnO1xyXG5cclxuQFN0YXRlPEZlYXR1cmVNYW5hZ2VtZW50LlN0YXRlPih7XHJcbiAgbmFtZTogJ0ZlYXR1cmVNYW5hZ2VtZW50U3RhdGUnLFxyXG4gIGRlZmF1bHRzOiB7IGZlYXR1cmVzOiB7fSB9IGFzIEZlYXR1cmVNYW5hZ2VtZW50LlN0YXRlLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgRmVhdHVyZU1hbmFnZW1lbnRTdGF0ZSB7XHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0RmVhdHVyZXMoeyBmZWF0dXJlcyB9OiBGZWF0dXJlTWFuYWdlbWVudC5TdGF0ZSkge1xyXG4gICAgcmV0dXJuIGZlYXR1cmVzIHx8IFtdO1xyXG4gIH1cclxuXHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBmZWF0dXJlTWFuYWdlbWVudFNlcnZpY2U6IEZlYXR1cmVNYW5hZ2VtZW50U2VydmljZSkge31cclxuXHJcbiAgQEFjdGlvbihHZXRGZWF0dXJlcylcclxuICBnZXRGZWF0dXJlcyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PEZlYXR1cmVNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldEZlYXR1cmVzKSB7XHJcbiAgICByZXR1cm4gdGhpcy5mZWF0dXJlTWFuYWdlbWVudFNlcnZpY2UuZ2V0RmVhdHVyZXMocGF5bG9hZCkucGlwZShcclxuICAgICAgdGFwKCh7IGZlYXR1cmVzIH0pID0+XHJcbiAgICAgICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgICAgICBmZWF0dXJlcyxcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKFVwZGF0ZUZlYXR1cmVzKVxyXG4gIHVwZGF0ZUZlYXR1cmVzKF8sIHsgcGF5bG9hZCB9OiBVcGRhdGVGZWF0dXJlcykge1xyXG4gICAgcmV0dXJuIHRoaXMuZmVhdHVyZU1hbmFnZW1lbnRTZXJ2aWNlLnVwZGF0ZUZlYXR1cmVzKHBheWxvYWQpO1xyXG4gIH1cclxufVxyXG4iXX0=