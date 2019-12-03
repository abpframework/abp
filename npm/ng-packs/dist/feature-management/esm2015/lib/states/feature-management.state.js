/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmVhdHVyZS1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5mZWF0dXJlLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL2ZlYXR1cmUtbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLEdBQUcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3JDLE9BQU8sRUFBRSxXQUFXLEVBQUUsY0FBYyxFQUFFLE1BQU0sdUNBQXVDLENBQUM7QUFFcEYsT0FBTyxFQUFFLHdCQUF3QixFQUFFLE1BQU0sd0NBQXdDLENBQUM7SUFNckUsc0JBQXNCLFNBQXRCLHNCQUFzQjs7OztJQU1qQyxZQUFvQix3QkFBa0Q7UUFBbEQsNkJBQXdCLEdBQXhCLHdCQUF3QixDQUEwQjtJQUFHLENBQUM7Ozs7O0lBSjFFLE1BQU0sQ0FBQyxXQUFXLENBQUMsRUFBRSxRQUFRLEVBQTJCO1FBQ3RELE9BQU8sUUFBUSxJQUFJLEVBQUUsQ0FBQztJQUN4QixDQUFDOzs7Ozs7SUFLRCxXQUFXLENBQUMsRUFBRSxVQUFVLEVBQXlDLEVBQUUsRUFBRSxPQUFPLEVBQWU7UUFDekYsT0FBTyxJQUFJLENBQUMsd0JBQXdCLENBQUMsV0FBVyxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FDNUQsR0FBRzs7OztRQUFDLENBQUMsRUFBRSxRQUFRLEVBQUUsRUFBRSxFQUFFLENBQ25CLFVBQVUsQ0FBQztZQUNULFFBQVE7U0FDVCxDQUFDLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QsY0FBYyxDQUFDLENBQUMsRUFBRSxFQUFFLE9BQU8sRUFBa0I7UUFDM0MsT0FBTyxJQUFJLENBQUMsd0JBQXdCLENBQUMsY0FBYyxDQUFDLE9BQU8sQ0FBQyxDQUFDO0lBQy9ELENBQUM7Q0FDRixDQUFBOztZQWpCK0Msd0JBQXdCOztBQUd0RTtJQURDLE1BQU0sQ0FBQyxXQUFXLENBQUM7O3FEQUM0RCxXQUFXOzt5REFRMUY7QUFHRDtJQURDLE1BQU0sQ0FBQyxjQUFjLENBQUM7O3FEQUNRLGNBQWM7OzREQUU1QztBQXBCRDtJQURDLFFBQVEsRUFBRTs7OzsrQ0FHVjtBQUpVLHNCQUFzQjtJQUpsQyxLQUFLLENBQTBCO1FBQzlCLElBQUksRUFBRSx3QkFBd0I7UUFDOUIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsUUFBUSxFQUFFLEVBQUUsRUFBRSxFQUEyQjtLQUN0RCxDQUFDOzZDQU84Qyx3QkFBd0I7R0FOM0Qsc0JBQXNCLENBdUJsQztTQXZCWSxzQkFBc0I7Ozs7OztJQU1yQiwwREFBMEQiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBY3Rpb24sIFNlbGVjdG9yLCBTdGF0ZSwgU3RhdGVDb250ZXh0IH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyB0YXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XHJcbmltcG9ydCB7IEdldEZlYXR1cmVzLCBVcGRhdGVGZWF0dXJlcyB9IGZyb20gJy4uL2FjdGlvbnMvZmVhdHVyZS1tYW5hZ2VtZW50LmFjdGlvbnMnO1xyXG5pbXBvcnQgeyBGZWF0dXJlTWFuYWdlbWVudCB9IGZyb20gJy4uL21vZGVscy9mZWF0dXJlLW1hbmFnZW1lbnQnO1xyXG5pbXBvcnQgeyBGZWF0dXJlTWFuYWdlbWVudFNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9mZWF0dXJlLW1hbmFnZW1lbnQuc2VydmljZSc7XHJcblxyXG5AU3RhdGU8RmVhdHVyZU1hbmFnZW1lbnQuU3RhdGU+KHtcclxuICBuYW1lOiAnRmVhdHVyZU1hbmFnZW1lbnRTdGF0ZScsXHJcbiAgZGVmYXVsdHM6IHsgZmVhdHVyZXM6IHt9IH0gYXMgRmVhdHVyZU1hbmFnZW1lbnQuU3RhdGUsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBGZWF0dXJlTWFuYWdlbWVudFN0YXRlIHtcclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXRGZWF0dXJlcyh7IGZlYXR1cmVzIH06IEZlYXR1cmVNYW5hZ2VtZW50LlN0YXRlKSB7XHJcbiAgICByZXR1cm4gZmVhdHVyZXMgfHwgW107XHJcbiAgfVxyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGZlYXR1cmVNYW5hZ2VtZW50U2VydmljZTogRmVhdHVyZU1hbmFnZW1lbnRTZXJ2aWNlKSB7fVxyXG5cclxuICBAQWN0aW9uKEdldEZlYXR1cmVzKVxyXG4gIGdldEZlYXR1cmVzKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8RmVhdHVyZU1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0RmVhdHVyZXMpIHtcclxuICAgIHJldHVybiB0aGlzLmZlYXR1cmVNYW5hZ2VtZW50U2VydmljZS5nZXRGZWF0dXJlcyhwYXlsb2FkKS5waXBlKFxyXG4gICAgICB0YXAoKHsgZmVhdHVyZXMgfSkgPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIGZlYXR1cmVzLFxyXG4gICAgICAgIH0pLFxyXG4gICAgICApLFxyXG4gICAgKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oVXBkYXRlRmVhdHVyZXMpXHJcbiAgdXBkYXRlRmVhdHVyZXMoXywgeyBwYXlsb2FkIH06IFVwZGF0ZUZlYXR1cmVzKSB7XHJcbiAgICByZXR1cm4gdGhpcy5mZWF0dXJlTWFuYWdlbWVudFNlcnZpY2UudXBkYXRlRmVhdHVyZXMocGF5bG9hZCk7XHJcbiAgfVxyXG59XHJcbiJdfQ==