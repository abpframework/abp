/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { State, Action, Selector } from '@ngxs/store';
import { AddNavigationElement, RemoveNavigationElementByName } from '../actions/layout.actions';
import snq from 'snq';
let LayoutState = class LayoutState {
    /**
     * @param {?} __0
     * @return {?}
     */
    static getNavigationElements({ navigationElements }) {
        return navigationElements;
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    layoutAddAction({ getState, patchState }, { payload = [] }) {
        let { navigationElements } = getState();
        if (!Array.isArray(payload)) {
            payload = [payload];
        }
        if (navigationElements.length) {
            payload = snq((/**
             * @return {?}
             */
            () => ((/** @type {?} */ (payload))).filter((/**
             * @param {?} __0
             * @return {?}
             */
            ({ name }) => navigationElements.findIndex((/**
             * @param {?} nav
             * @return {?}
             */
            nav => nav.name === name)) < 0))), []);
        }
        if (!payload.length)
            return;
        navigationElements = [...navigationElements, ...payload]
            .map((/**
         * @param {?} element
         * @return {?}
         */
        element => (Object.assign({}, element, { order: element.order || 99 }))))
            .sort((/**
         * @param {?} a
         * @param {?} b
         * @return {?}
         */
        (a, b) => a.order - b.order));
        return patchState({
            navigationElements,
        });
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    layoutRemoveAction({ getState, patchState }, { name }) {
        let { navigationElements } = getState();
        /** @type {?} */
        const index = navigationElements.findIndex((/**
         * @param {?} element
         * @return {?}
         */
        element => element.name === name));
        if (index > -1) {
            navigationElements = navigationElements.splice(index, 1);
        }
        return patchState({
            navigationElements,
        });
    }
};
tslib_1.__decorate([
    Action(AddNavigationElement),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, AddNavigationElement]),
    tslib_1.__metadata("design:returntype", void 0)
], LayoutState.prototype, "layoutAddAction", null);
tslib_1.__decorate([
    Action(RemoveNavigationElementByName),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, RemoveNavigationElementByName]),
    tslib_1.__metadata("design:returntype", void 0)
], LayoutState.prototype, "layoutRemoveAction", null);
tslib_1.__decorate([
    Selector(),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object]),
    tslib_1.__metadata("design:returntype", Array)
], LayoutState, "getNavigationElements", null);
LayoutState = tslib_1.__decorate([
    State({
        name: 'LayoutState',
        defaults: (/** @type {?} */ ({ navigationElements: [] })),
    })
], LayoutState);
export { LayoutState };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvbGF5b3V0LnN0YXRlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQWdCLFFBQVEsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwRSxPQUFPLEVBQUUsb0JBQW9CLEVBQUUsNkJBQTZCLEVBQUUsTUFBTSwyQkFBMkIsQ0FBQztBQUdoRyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7SUFNVCxXQUFXLFNBQVgsV0FBVzs7Ozs7SUFFdEIsTUFBTSxDQUFDLHFCQUFxQixDQUFDLEVBQUUsa0JBQWtCLEVBQWdCO1FBQy9ELE9BQU8sa0JBQWtCLENBQUM7SUFDNUIsQ0FBQzs7Ozs7O0lBR0QsZUFBZSxDQUFDLEVBQUUsUUFBUSxFQUFFLFVBQVUsRUFBOEIsRUFBRSxFQUFFLE9BQU8sR0FBRyxFQUFFLEVBQXdCO1lBQ3RHLEVBQUUsa0JBQWtCLEVBQUUsR0FBRyxRQUFRLEVBQUU7UUFFdkMsSUFBSSxDQUFDLEtBQUssQ0FBQyxPQUFPLENBQUMsT0FBTyxDQUFDLEVBQUU7WUFDM0IsT0FBTyxHQUFHLENBQUMsT0FBTyxDQUFDLENBQUM7U0FDckI7UUFFRCxJQUFJLGtCQUFrQixDQUFDLE1BQU0sRUFBRTtZQUM3QixPQUFPLEdBQUcsR0FBRzs7O1lBQ1gsR0FBRyxFQUFFLENBQ0gsQ0FBQyxtQkFBQSxPQUFPLEVBQThCLENBQUMsQ0FBQyxNQUFNOzs7O1lBQzVDLENBQUMsRUFBRSxJQUFJLEVBQUUsRUFBRSxFQUFFLENBQUMsa0JBQWtCLENBQUMsU0FBUzs7OztZQUFDLEdBQUcsQ0FBQyxFQUFFLENBQUMsR0FBRyxDQUFDLElBQUksS0FBSyxJQUFJLEVBQUMsR0FBRyxDQUFDLEVBQ3pFLEdBQ0gsRUFBRSxDQUNILENBQUM7U0FDSDtRQUVELElBQUksQ0FBQyxPQUFPLENBQUMsTUFBTTtZQUFFLE9BQU87UUFFNUIsa0JBQWtCLEdBQUcsQ0FBQyxHQUFHLGtCQUFrQixFQUFFLEdBQUcsT0FBTyxDQUFDO2FBQ3JELEdBQUc7Ozs7UUFBQyxPQUFPLENBQUMsRUFBRSxDQUFDLG1CQUFNLE9BQU8sSUFBRSxLQUFLLEVBQUUsT0FBTyxDQUFDLEtBQUssSUFBSSxFQUFFLElBQUcsRUFBQzthQUM1RCxJQUFJOzs7OztRQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsRUFBRSxFQUFFLENBQUMsQ0FBQyxDQUFDLEtBQUssR0FBRyxDQUFDLENBQUMsS0FBSyxFQUFDLENBQUM7UUFFckMsT0FBTyxVQUFVLENBQUM7WUFDaEIsa0JBQWtCO1NBQ25CLENBQUMsQ0FBQztJQUNMLENBQUM7Ozs7OztJQUdELGtCQUFrQixDQUFDLEVBQUUsUUFBUSxFQUFFLFVBQVUsRUFBOEIsRUFBRSxFQUFFLElBQUksRUFBaUM7WUFDMUcsRUFBRSxrQkFBa0IsRUFBRSxHQUFHLFFBQVEsRUFBRTs7Y0FFakMsS0FBSyxHQUFHLGtCQUFrQixDQUFDLFNBQVM7Ozs7UUFBQyxPQUFPLENBQUMsRUFBRSxDQUFDLE9BQU8sQ0FBQyxJQUFJLEtBQUssSUFBSSxFQUFDO1FBRTVFLElBQUksS0FBSyxHQUFHLENBQUMsQ0FBQyxFQUFFO1lBQ2Qsa0JBQWtCLEdBQUcsa0JBQWtCLENBQUMsTUFBTSxDQUFDLEtBQUssRUFBRSxDQUFDLENBQUMsQ0FBQztTQUMxRDtRQUVELE9BQU8sVUFBVSxDQUFDO1lBQ2hCLGtCQUFrQjtTQUNuQixDQUFDLENBQUM7SUFDTCxDQUFDO0NBQ0YsQ0FBQTtBQTFDQztJQURDLE1BQU0sQ0FBQyxvQkFBb0IsQ0FBQzs7cURBQzJELG9CQUFvQjs7a0RBMEIzRztBQUdEO0lBREMsTUFBTSxDQUFDLDZCQUE2QixDQUFDOztxREFDNkMsNkJBQTZCOztxREFZL0c7QUE5Q0Q7SUFEQyxRQUFRLEVBQUU7Ozs7OENBR1Y7QUFKVSxXQUFXO0lBSnZCLEtBQUssQ0FBZTtRQUNuQixJQUFJLEVBQUUsYUFBYTtRQUNuQixRQUFRLEVBQUUsbUJBQUEsRUFBRSxrQkFBa0IsRUFBRSxFQUFFLEVBQUUsRUFBZ0I7S0FDckQsQ0FBQztHQUNXLFdBQVcsQ0FpRHZCO1NBakRZLFdBQVciLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBTdGF0ZSwgQWN0aW9uLCBTdGF0ZUNvbnRleHQsIFNlbGVjdG9yIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBBZGROYXZpZ2F0aW9uRWxlbWVudCwgUmVtb3ZlTmF2aWdhdGlvbkVsZW1lbnRCeU5hbWUgfSBmcm9tICcuLi9hY3Rpb25zL2xheW91dC5hY3Rpb25zJztcclxuaW1wb3J0IHsgTGF5b3V0IH0gZnJvbSAnLi4vbW9kZWxzL2xheW91dCc7XHJcbmltcG9ydCB7IFRlbXBsYXRlUmVmIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcclxuXHJcbkBTdGF0ZTxMYXlvdXQuU3RhdGU+KHtcclxuICBuYW1lOiAnTGF5b3V0U3RhdGUnLFxyXG4gIGRlZmF1bHRzOiB7IG5hdmlnYXRpb25FbGVtZW50czogW10gfSBhcyBMYXlvdXQuU3RhdGUsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBMYXlvdXRTdGF0ZSB7XHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0TmF2aWdhdGlvbkVsZW1lbnRzKHsgbmF2aWdhdGlvbkVsZW1lbnRzIH06IExheW91dC5TdGF0ZSk6IExheW91dC5OYXZpZ2F0aW9uRWxlbWVudFtdIHtcclxuICAgIHJldHVybiBuYXZpZ2F0aW9uRWxlbWVudHM7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKEFkZE5hdmlnYXRpb25FbGVtZW50KVxyXG4gIGxheW91dEFkZEFjdGlvbih7IGdldFN0YXRlLCBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxMYXlvdXQuU3RhdGU+LCB7IHBheWxvYWQgPSBbXSB9OiBBZGROYXZpZ2F0aW9uRWxlbWVudCkge1xyXG4gICAgbGV0IHsgbmF2aWdhdGlvbkVsZW1lbnRzIH0gPSBnZXRTdGF0ZSgpO1xyXG5cclxuICAgIGlmICghQXJyYXkuaXNBcnJheShwYXlsb2FkKSkge1xyXG4gICAgICBwYXlsb2FkID0gW3BheWxvYWRdO1xyXG4gICAgfVxyXG5cclxuICAgIGlmIChuYXZpZ2F0aW9uRWxlbWVudHMubGVuZ3RoKSB7XHJcbiAgICAgIHBheWxvYWQgPSBzbnEoXHJcbiAgICAgICAgKCkgPT5cclxuICAgICAgICAgIChwYXlsb2FkIGFzIExheW91dC5OYXZpZ2F0aW9uRWxlbWVudFtdKS5maWx0ZXIoXHJcbiAgICAgICAgICAgICh7IG5hbWUgfSkgPT4gbmF2aWdhdGlvbkVsZW1lbnRzLmZpbmRJbmRleChuYXYgPT4gbmF2Lm5hbWUgPT09IG5hbWUpIDwgMCxcclxuICAgICAgICAgICksXHJcbiAgICAgICAgW10sXHJcbiAgICAgICk7XHJcbiAgICB9XHJcblxyXG4gICAgaWYgKCFwYXlsb2FkLmxlbmd0aCkgcmV0dXJuO1xyXG5cclxuICAgIG5hdmlnYXRpb25FbGVtZW50cyA9IFsuLi5uYXZpZ2F0aW9uRWxlbWVudHMsIC4uLnBheWxvYWRdXHJcbiAgICAgIC5tYXAoZWxlbWVudCA9PiAoeyAuLi5lbGVtZW50LCBvcmRlcjogZWxlbWVudC5vcmRlciB8fCA5OSB9KSlcclxuICAgICAgLnNvcnQoKGEsIGIpID0+IGEub3JkZXIgLSBiLm9yZGVyKTtcclxuXHJcbiAgICByZXR1cm4gcGF0Y2hTdGF0ZSh7XHJcbiAgICAgIG5hdmlnYXRpb25FbGVtZW50cyxcclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihSZW1vdmVOYXZpZ2F0aW9uRWxlbWVudEJ5TmFtZSlcclxuICBsYXlvdXRSZW1vdmVBY3Rpb24oeyBnZXRTdGF0ZSwgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8TGF5b3V0LlN0YXRlPiwgeyBuYW1lIH06IFJlbW92ZU5hdmlnYXRpb25FbGVtZW50QnlOYW1lKSB7XHJcbiAgICBsZXQgeyBuYXZpZ2F0aW9uRWxlbWVudHMgfSA9IGdldFN0YXRlKCk7XHJcblxyXG4gICAgY29uc3QgaW5kZXggPSBuYXZpZ2F0aW9uRWxlbWVudHMuZmluZEluZGV4KGVsZW1lbnQgPT4gZWxlbWVudC5uYW1lID09PSBuYW1lKTtcclxuXHJcbiAgICBpZiAoaW5kZXggPiAtMSkge1xyXG4gICAgICBuYXZpZ2F0aW9uRWxlbWVudHMgPSBuYXZpZ2F0aW9uRWxlbWVudHMuc3BsaWNlKGluZGV4LCAxKTtcclxuICAgIH1cclxuXHJcbiAgICByZXR1cm4gcGF0Y2hTdGF0ZSh7XHJcbiAgICAgIG5hdmlnYXRpb25FbGVtZW50cyxcclxuICAgIH0pO1xyXG4gIH1cclxufVxyXG4iXX0=