/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { State, Action, Selector } from '@ngxs/store';
import { LayoutAddNavigationElement, LayoutRemoveNavigationElementByName } from '../actions/layout.actions';
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
    Action(LayoutAddNavigationElement),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, LayoutAddNavigationElement]),
    tslib_1.__metadata("design:returntype", void 0)
], LayoutState.prototype, "layoutAddAction", null);
tslib_1.__decorate([
    Action(LayoutRemoveNavigationElementByName),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, LayoutRemoveNavigationElementByName]),
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvbGF5b3V0LnN0YXRlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQWdCLFFBQVEsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwRSxPQUFPLEVBQUUsMEJBQTBCLEVBQUUsbUNBQW1DLEVBQUUsTUFBTSwyQkFBMkIsQ0FBQztBQUc1RyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7SUFNVCxXQUFXLFNBQVgsV0FBVzs7Ozs7SUFFdEIsTUFBTSxDQUFDLHFCQUFxQixDQUFDLEVBQUUsa0JBQWtCLEVBQWdCO1FBQy9ELE9BQU8sa0JBQWtCLENBQUM7SUFDNUIsQ0FBQzs7Ozs7O0lBR0QsZUFBZSxDQUFDLEVBQUUsUUFBUSxFQUFFLFVBQVUsRUFBOEIsRUFBRSxFQUFFLE9BQU8sR0FBRyxFQUFFLEVBQThCO1lBQzVHLEVBQUUsa0JBQWtCLEVBQUUsR0FBRyxRQUFRLEVBQUU7UUFFdkMsSUFBSSxDQUFDLEtBQUssQ0FBQyxPQUFPLENBQUMsT0FBTyxDQUFDLEVBQUU7WUFDM0IsT0FBTyxHQUFHLENBQUMsT0FBTyxDQUFDLENBQUM7U0FDckI7UUFFRCxJQUFJLGtCQUFrQixDQUFDLE1BQU0sRUFBRTtZQUM3QixPQUFPLEdBQUcsR0FBRzs7O1lBQ1gsR0FBRyxFQUFFLENBQ0gsQ0FBQyxtQkFBQSxPQUFPLEVBQThCLENBQUMsQ0FBQyxNQUFNOzs7O1lBQzVDLENBQUMsRUFBRSxJQUFJLEVBQUUsRUFBRSxFQUFFLENBQUMsa0JBQWtCLENBQUMsU0FBUzs7OztZQUFDLEdBQUcsQ0FBQyxFQUFFLENBQUMsR0FBRyxDQUFDLElBQUksS0FBSyxJQUFJLEVBQUMsR0FBRyxDQUFDLEVBQ3pFLEdBQ0gsRUFBRSxDQUNILENBQUM7U0FDSDtRQUVELElBQUksQ0FBQyxPQUFPLENBQUMsTUFBTTtZQUFFLE9BQU87UUFFNUIsa0JBQWtCLEdBQUcsQ0FBQyxHQUFHLGtCQUFrQixFQUFFLEdBQUcsT0FBTyxDQUFDO2FBQ3JELEdBQUc7Ozs7UUFBQyxPQUFPLENBQUMsRUFBRSxDQUFDLG1CQUFNLE9BQU8sSUFBRSxLQUFLLEVBQUUsT0FBTyxDQUFDLEtBQUssSUFBSSxFQUFFLElBQUcsRUFBQzthQUM1RCxJQUFJOzs7OztRQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsRUFBRSxFQUFFLENBQUMsQ0FBQyxDQUFDLEtBQUssR0FBRyxDQUFDLENBQUMsS0FBSyxFQUFDLENBQUM7UUFFckMsT0FBTyxVQUFVLENBQUM7WUFDaEIsa0JBQWtCO1NBQ25CLENBQUMsQ0FBQztJQUNMLENBQUM7Ozs7OztJQUdELGtCQUFrQixDQUNoQixFQUFFLFFBQVEsRUFBRSxVQUFVLEVBQThCLEVBQ3BELEVBQUUsSUFBSSxFQUF1QztZQUV6QyxFQUFFLGtCQUFrQixFQUFFLEdBQUcsUUFBUSxFQUFFOztjQUVqQyxLQUFLLEdBQUcsa0JBQWtCLENBQUMsU0FBUzs7OztRQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsT0FBTyxDQUFDLElBQUksS0FBSyxJQUFJLEVBQUM7UUFFNUUsSUFBSSxLQUFLLEdBQUcsQ0FBQyxDQUFDLEVBQUU7WUFDZCxrQkFBa0IsR0FBRyxrQkFBa0IsQ0FBQyxNQUFNLENBQUMsS0FBSyxFQUFFLENBQUMsQ0FBQyxDQUFDO1NBQzFEO1FBRUQsT0FBTyxVQUFVLENBQUM7WUFDaEIsa0JBQWtCO1NBQ25CLENBQUMsQ0FBQztJQUNMLENBQUM7Q0FDRixDQUFBO0FBN0NDO0lBREMsTUFBTSxDQUFDLDBCQUEwQixDQUFDOztxREFDcUQsMEJBQTBCOztrREEwQmpIO0FBR0Q7SUFEQyxNQUFNLENBQUMsbUNBQW1DLENBQUM7O3FEQUdoQyxtQ0FBbUM7O3FEQWE5QztBQWpERDtJQURDLFFBQVEsRUFBRTs7Ozs4Q0FHVjtBQUpVLFdBQVc7SUFKdkIsS0FBSyxDQUFlO1FBQ25CLElBQUksRUFBRSxhQUFhO1FBQ25CLFFBQVEsRUFBRSxtQkFBQSxFQUFFLGtCQUFrQixFQUFFLEVBQUUsRUFBRSxFQUFnQjtLQUNyRCxDQUFDO0dBQ1csV0FBVyxDQW9EdkI7U0FwRFksV0FBVyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFN0YXRlLCBBY3Rpb24sIFN0YXRlQ29udGV4dCwgU2VsZWN0b3IgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBMYXlvdXRBZGROYXZpZ2F0aW9uRWxlbWVudCwgTGF5b3V0UmVtb3ZlTmF2aWdhdGlvbkVsZW1lbnRCeU5hbWUgfSBmcm9tICcuLi9hY3Rpb25zL2xheW91dC5hY3Rpb25zJztcbmltcG9ydCB7IExheW91dCB9IGZyb20gJy4uL21vZGVscy9sYXlvdXQnO1xuaW1wb3J0IHsgVGVtcGxhdGVSZWYgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcblxuQFN0YXRlPExheW91dC5TdGF0ZT4oe1xuICBuYW1lOiAnTGF5b3V0U3RhdGUnLFxuICBkZWZhdWx0czogeyBuYXZpZ2F0aW9uRWxlbWVudHM6IFtdIH0gYXMgTGF5b3V0LlN0YXRlLFxufSlcbmV4cG9ydCBjbGFzcyBMYXlvdXRTdGF0ZSB7XG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXROYXZpZ2F0aW9uRWxlbWVudHMoeyBuYXZpZ2F0aW9uRWxlbWVudHMgfTogTGF5b3V0LlN0YXRlKTogTGF5b3V0Lk5hdmlnYXRpb25FbGVtZW50W10ge1xuICAgIHJldHVybiBuYXZpZ2F0aW9uRWxlbWVudHM7XG4gIH1cblxuICBAQWN0aW9uKExheW91dEFkZE5hdmlnYXRpb25FbGVtZW50KVxuICBsYXlvdXRBZGRBY3Rpb24oeyBnZXRTdGF0ZSwgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8TGF5b3V0LlN0YXRlPiwgeyBwYXlsb2FkID0gW10gfTogTGF5b3V0QWRkTmF2aWdhdGlvbkVsZW1lbnQpIHtcbiAgICBsZXQgeyBuYXZpZ2F0aW9uRWxlbWVudHMgfSA9IGdldFN0YXRlKCk7XG5cbiAgICBpZiAoIUFycmF5LmlzQXJyYXkocGF5bG9hZCkpIHtcbiAgICAgIHBheWxvYWQgPSBbcGF5bG9hZF07XG4gICAgfVxuXG4gICAgaWYgKG5hdmlnYXRpb25FbGVtZW50cy5sZW5ndGgpIHtcbiAgICAgIHBheWxvYWQgPSBzbnEoXG4gICAgICAgICgpID0+XG4gICAgICAgICAgKHBheWxvYWQgYXMgTGF5b3V0Lk5hdmlnYXRpb25FbGVtZW50W10pLmZpbHRlcihcbiAgICAgICAgICAgICh7IG5hbWUgfSkgPT4gbmF2aWdhdGlvbkVsZW1lbnRzLmZpbmRJbmRleChuYXYgPT4gbmF2Lm5hbWUgPT09IG5hbWUpIDwgMCxcbiAgICAgICAgICApLFxuICAgICAgICBbXSxcbiAgICAgICk7XG4gICAgfVxuXG4gICAgaWYgKCFwYXlsb2FkLmxlbmd0aCkgcmV0dXJuO1xuXG4gICAgbmF2aWdhdGlvbkVsZW1lbnRzID0gWy4uLm5hdmlnYXRpb25FbGVtZW50cywgLi4ucGF5bG9hZF1cbiAgICAgIC5tYXAoZWxlbWVudCA9PiAoeyAuLi5lbGVtZW50LCBvcmRlcjogZWxlbWVudC5vcmRlciB8fCA5OSB9KSlcbiAgICAgIC5zb3J0KChhLCBiKSA9PiBhLm9yZGVyIC0gYi5vcmRlcik7XG5cbiAgICByZXR1cm4gcGF0Y2hTdGF0ZSh7XG4gICAgICBuYXZpZ2F0aW9uRWxlbWVudHMsXG4gICAgfSk7XG4gIH1cblxuICBAQWN0aW9uKExheW91dFJlbW92ZU5hdmlnYXRpb25FbGVtZW50QnlOYW1lKVxuICBsYXlvdXRSZW1vdmVBY3Rpb24oXG4gICAgeyBnZXRTdGF0ZSwgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8TGF5b3V0LlN0YXRlPixcbiAgICB7IG5hbWUgfTogTGF5b3V0UmVtb3ZlTmF2aWdhdGlvbkVsZW1lbnRCeU5hbWUsXG4gICkge1xuICAgIGxldCB7IG5hdmlnYXRpb25FbGVtZW50cyB9ID0gZ2V0U3RhdGUoKTtcblxuICAgIGNvbnN0IGluZGV4ID0gbmF2aWdhdGlvbkVsZW1lbnRzLmZpbmRJbmRleChlbGVtZW50ID0+IGVsZW1lbnQubmFtZSA9PT0gbmFtZSk7XG5cbiAgICBpZiAoaW5kZXggPiAtMSkge1xuICAgICAgbmF2aWdhdGlvbkVsZW1lbnRzID0gbmF2aWdhdGlvbkVsZW1lbnRzLnNwbGljZShpbmRleCwgMSk7XG4gICAgfVxuXG4gICAgcmV0dXJuIHBhdGNoU3RhdGUoe1xuICAgICAgbmF2aWdhdGlvbkVsZW1lbnRzLFxuICAgIH0pO1xuICB9XG59XG4iXX0=