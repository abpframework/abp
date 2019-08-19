/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
        return features;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmVhdHVyZS1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5mZWF0dXJlLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL2ZlYXR1cmUtbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLEdBQUcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3JDLE9BQU8sRUFBRSxXQUFXLEVBQUUsY0FBYyxFQUFFLE1BQU0sdUNBQXVDLENBQUM7QUFFcEYsT0FBTyxFQUFFLHdCQUF3QixFQUFFLE1BQU0sd0NBQXdDLENBQUM7SUFNckUsc0JBQXNCLFNBQXRCLHNCQUFzQjs7OztJQU1qQyxZQUFvQix3QkFBa0Q7UUFBbEQsNkJBQXdCLEdBQXhCLHdCQUF3QixDQUEwQjtJQUFHLENBQUM7Ozs7O0lBSjFFLE1BQU0sQ0FBQyxXQUFXLENBQUMsRUFBRSxRQUFRLEVBQTJCO1FBQ3RELE9BQU8sUUFBUSxDQUFDO0lBQ2xCLENBQUM7Ozs7OztJQUtELFdBQVcsQ0FBQyxFQUFFLFVBQVUsRUFBeUMsRUFBRSxFQUFFLE9BQU8sRUFBZTtRQUN6RixPQUFPLElBQUksQ0FBQyx3QkFBd0IsQ0FBQyxXQUFXLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUM1RCxHQUFHOzs7O1FBQUMsQ0FBQyxFQUFFLFFBQVEsRUFBRSxFQUFFLEVBQUUsQ0FDbkIsVUFBVSxDQUFDO1lBQ1QsUUFBUTtTQUNULENBQUMsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCxjQUFjLENBQUMsQ0FBQyxFQUFFLEVBQUUsT0FBTyxFQUFrQjtRQUMzQyxPQUFPLElBQUksQ0FBQyx3QkFBd0IsQ0FBQyxjQUFjLENBQUMsT0FBTyxDQUFDLENBQUM7SUFDL0QsQ0FBQztDQUNGLENBQUE7QUFkQztJQURDLE1BQU0sQ0FBQyxXQUFXLENBQUM7O3FEQUM0RCxXQUFXOzt5REFRMUY7QUFHRDtJQURDLE1BQU0sQ0FBQyxjQUFjLENBQUM7O3FEQUNRLGNBQWM7OzREQUU1QztBQXBCRDtJQURDLFFBQVEsRUFBRTs7OzsrQ0FHVjtBQUpVLHNCQUFzQjtJQUpsQyxLQUFLLENBQTBCO1FBQzlCLElBQUksRUFBRSx3QkFBd0I7UUFDOUIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsUUFBUSxFQUFFLEVBQUUsRUFBRSxFQUEyQjtLQUN0RCxDQUFDOzZDQU84Qyx3QkFBd0I7R0FOM0Qsc0JBQXNCLENBdUJsQztTQXZCWSxzQkFBc0I7Ozs7OztJQU1yQiwwREFBMEQiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBY3Rpb24sIFNlbGVjdG9yLCBTdGF0ZSwgU3RhdGVDb250ZXh0IH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHsgR2V0RmVhdHVyZXMsIFVwZGF0ZUZlYXR1cmVzIH0gZnJvbSAnLi4vYWN0aW9ucy9mZWF0dXJlLW1hbmFnZW1lbnQuYWN0aW9ucyc7XG5pbXBvcnQgeyBGZWF0dXJlTWFuYWdlbWVudCB9IGZyb20gJy4uL21vZGVscy9mZWF0dXJlLW1hbmFnZW1lbnQnO1xuaW1wb3J0IHsgRmVhdHVyZU1hbmFnZW1lbnRTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvZmVhdHVyZS1tYW5hZ2VtZW50LnNlcnZpY2UnO1xuXG5AU3RhdGU8RmVhdHVyZU1hbmFnZW1lbnQuU3RhdGU+KHtcbiAgbmFtZTogJ0ZlYXR1cmVNYW5hZ2VtZW50U3RhdGUnLFxuICBkZWZhdWx0czogeyBmZWF0dXJlczoge30gfSBhcyBGZWF0dXJlTWFuYWdlbWVudC5TdGF0ZSxcbn0pXG5leHBvcnQgY2xhc3MgRmVhdHVyZU1hbmFnZW1lbnRTdGF0ZSB7XG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRGZWF0dXJlcyh7IGZlYXR1cmVzIH06IEZlYXR1cmVNYW5hZ2VtZW50LlN0YXRlKSB7XG4gICAgcmV0dXJuIGZlYXR1cmVzO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBmZWF0dXJlTWFuYWdlbWVudFNlcnZpY2U6IEZlYXR1cmVNYW5hZ2VtZW50U2VydmljZSkge31cblxuICBAQWN0aW9uKEdldEZlYXR1cmVzKVxuICBnZXRGZWF0dXJlcyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PEZlYXR1cmVNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldEZlYXR1cmVzKSB7XG4gICAgcmV0dXJuIHRoaXMuZmVhdHVyZU1hbmFnZW1lbnRTZXJ2aWNlLmdldEZlYXR1cmVzKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAoKHsgZmVhdHVyZXMgfSkgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgZmVhdHVyZXMsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihVcGRhdGVGZWF0dXJlcylcbiAgdXBkYXRlRmVhdHVyZXMoXywgeyBwYXlsb2FkIH06IFVwZGF0ZUZlYXR1cmVzKSB7XG4gICAgcmV0dXJuIHRoaXMuZmVhdHVyZU1hbmFnZW1lbnRTZXJ2aWNlLnVwZGF0ZUZlYXR1cmVzKHBheWxvYWQpO1xuICB9XG59XG4iXX0=