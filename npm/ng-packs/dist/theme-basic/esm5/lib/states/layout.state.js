/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/layout.state.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { State, Action, Selector } from '@ngxs/store';
import { AddNavigationElement, RemoveNavigationElementByName } from '../actions/layout.actions';
import snq from 'snq';
var LayoutState = /** @class */ (function () {
    function LayoutState() {
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    LayoutState.getNavigationElements = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var navigationElements = _a.navigationElements;
        return navigationElements;
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    LayoutState.prototype.layoutAddAction = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var getState = _a.getState, patchState = _a.patchState;
        var _c = _b.payload, payload = _c === void 0 ? [] : _c;
        var navigationElements = getState().navigationElements;
        if (!Array.isArray(payload)) {
            payload = [payload];
        }
        if (navigationElements.length) {
            payload = snq((/**
             * @return {?}
             */
            function () {
                return ((/** @type {?} */ (payload))).filter((/**
                 * @param {?} __0
                 * @return {?}
                 */
                function (_a) {
                    var name = _a.name;
                    return navigationElements.findIndex((/**
                     * @param {?} nav
                     * @return {?}
                     */
                    function (nav) { return nav.name === name; })) < 0;
                }));
            }), []);
        }
        if (!payload.length)
            return;
        navigationElements = tslib_1.__spread(navigationElements, payload).map((/**
         * @param {?} element
         * @return {?}
         */
        function (element) { return (tslib_1.__assign({}, element, { order: element.order || 99 })); }))
            .sort((/**
         * @param {?} a
         * @param {?} b
         * @return {?}
         */
        function (a, b) { return a.order - b.order; }));
        return patchState({
            navigationElements: navigationElements,
        });
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    LayoutState.prototype.layoutRemoveAction = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var getState = _a.getState, patchState = _a.patchState;
        var name = _b.name;
        var navigationElements = getState().navigationElements;
        /** @type {?} */
        var index = navigationElements.findIndex((/**
         * @param {?} element
         * @return {?}
         */
        function (element) { return element.name === name; }));
        if (index > -1) {
            navigationElements = navigationElements.splice(index, 1);
        }
        return patchState({
            navigationElements: navigationElements,
        });
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
    return LayoutState;
}());
export { LayoutState };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvbGF5b3V0LnN0YXRlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7OztBQUFBLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFnQixRQUFRLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLG9CQUFvQixFQUFFLDZCQUE2QixFQUFFLE1BQU0sMkJBQTJCLENBQUM7QUFHaEcsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDOzs7SUF1RHRCLENBQUM7Ozs7O0lBL0NRLGlDQUFxQjs7OztJQUE1QixVQUE2QixFQUFvQztZQUFsQywwQ0FBa0I7UUFDL0MsT0FBTyxrQkFBa0IsQ0FBQztJQUM1QixDQUFDOzs7Ozs7SUFHRCxxQ0FBZTs7Ozs7SUFBZixVQUFnQixFQUFvRCxFQUFFLEVBQXNDO1lBQTFGLHNCQUFRLEVBQUUsMEJBQVU7WUFBa0MsZUFBWSxFQUFaLGlDQUFZO1FBQzVFLElBQUEsa0RBQWtCO1FBRXhCLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLE9BQU8sQ0FBQyxFQUFFO1lBQzNCLE9BQU8sR0FBRyxDQUFDLE9BQU8sQ0FBQyxDQUFDO1NBQ3JCO1FBRUQsSUFBSSxrQkFBa0IsQ0FBQyxNQUFNLEVBQUU7WUFDN0IsT0FBTyxHQUFHLEdBQUc7OztZQUNYO2dCQUNFLE9BQUEsQ0FBQyxtQkFBQSxPQUFPLEVBQThCLENBQUMsQ0FBQyxNQUFNOzs7O2dCQUM1QyxVQUFDLEVBQVE7d0JBQU4sY0FBSTtvQkFBTyxPQUFBLGtCQUFrQixDQUFDLFNBQVM7Ozs7b0JBQUMsVUFBQSxHQUFHLElBQUksT0FBQSxHQUFHLENBQUMsSUFBSSxLQUFLLElBQUksRUFBakIsQ0FBaUIsRUFBQyxHQUFHLENBQUM7Z0JBQTFELENBQTBELEVBQ3pFO1lBRkQsQ0FFQyxHQUNILEVBQUUsQ0FDSCxDQUFDO1NBQ0g7UUFFRCxJQUFJLENBQUMsT0FBTyxDQUFDLE1BQU07WUFBRSxPQUFPO1FBRTVCLGtCQUFrQixHQUFHLGlCQUFJLGtCQUFrQixFQUFLLE9BQU8sRUFDcEQsR0FBRzs7OztRQUFDLFVBQUEsT0FBTyxJQUFJLE9BQUEsc0JBQU0sT0FBTyxJQUFFLEtBQUssRUFBRSxPQUFPLENBQUMsS0FBSyxJQUFJLEVBQUUsSUFBRyxFQUE1QyxDQUE0QyxFQUFDO2FBQzVELElBQUk7Ozs7O1FBQUMsVUFBQyxDQUFDLEVBQUUsQ0FBQyxJQUFLLE9BQUEsQ0FBQyxDQUFDLEtBQUssR0FBRyxDQUFDLENBQUMsS0FBSyxFQUFqQixDQUFpQixFQUFDLENBQUM7UUFFckMsT0FBTyxVQUFVLENBQUM7WUFDaEIsa0JBQWtCLG9CQUFBO1NBQ25CLENBQUMsQ0FBQztJQUNMLENBQUM7Ozs7OztJQUdELHdDQUFrQjs7Ozs7SUFBbEIsVUFBbUIsRUFBb0QsRUFBRSxFQUF1QztZQUEzRixzQkFBUSxFQUFFLDBCQUFVO1lBQWtDLGNBQUk7UUFDdkUsSUFBQSxrREFBa0I7O1lBRWxCLEtBQUssR0FBRyxrQkFBa0IsQ0FBQyxTQUFTOzs7O1FBQUMsVUFBQSxPQUFPLElBQUksT0FBQSxPQUFPLENBQUMsSUFBSSxLQUFLLElBQUksRUFBckIsQ0FBcUIsRUFBQztRQUU1RSxJQUFJLEtBQUssR0FBRyxDQUFDLENBQUMsRUFBRTtZQUNkLGtCQUFrQixHQUFHLGtCQUFrQixDQUFDLE1BQU0sQ0FBQyxLQUFLLEVBQUUsQ0FBQyxDQUFDLENBQUM7U0FDMUQ7UUFFRCxPQUFPLFVBQVUsQ0FBQztZQUNoQixrQkFBa0Isb0JBQUE7U0FDbkIsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztJQXpDRDtRQURDLE1BQU0sQ0FBQyxvQkFBb0IsQ0FBQzs7eURBQzJELG9CQUFvQjs7c0RBMEIzRztJQUdEO1FBREMsTUFBTSxDQUFDLDZCQUE2QixDQUFDOzt5REFDNkMsNkJBQTZCOzt5REFZL0c7SUE5Q0Q7UUFEQyxRQUFRLEVBQUU7Ozs7a0RBR1Y7SUFKVSxXQUFXO1FBSnZCLEtBQUssQ0FBZTtZQUNuQixJQUFJLEVBQUUsYUFBYTtZQUNuQixRQUFRLEVBQUUsbUJBQUEsRUFBRSxrQkFBa0IsRUFBRSxFQUFFLEVBQUUsRUFBZ0I7U0FDckQsQ0FBQztPQUNXLFdBQVcsQ0FpRHZCO0lBQUQsa0JBQUM7Q0FBQSxJQUFBO1NBakRZLFdBQVciLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBTdGF0ZSwgQWN0aW9uLCBTdGF0ZUNvbnRleHQsIFNlbGVjdG9yIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBBZGROYXZpZ2F0aW9uRWxlbWVudCwgUmVtb3ZlTmF2aWdhdGlvbkVsZW1lbnRCeU5hbWUgfSBmcm9tICcuLi9hY3Rpb25zL2xheW91dC5hY3Rpb25zJztcclxuaW1wb3J0IHsgTGF5b3V0IH0gZnJvbSAnLi4vbW9kZWxzL2xheW91dCc7XHJcbmltcG9ydCB7IFRlbXBsYXRlUmVmIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcclxuXHJcbkBTdGF0ZTxMYXlvdXQuU3RhdGU+KHtcclxuICBuYW1lOiAnTGF5b3V0U3RhdGUnLFxyXG4gIGRlZmF1bHRzOiB7IG5hdmlnYXRpb25FbGVtZW50czogW10gfSBhcyBMYXlvdXQuU3RhdGUsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBMYXlvdXRTdGF0ZSB7XHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0TmF2aWdhdGlvbkVsZW1lbnRzKHsgbmF2aWdhdGlvbkVsZW1lbnRzIH06IExheW91dC5TdGF0ZSk6IExheW91dC5OYXZpZ2F0aW9uRWxlbWVudFtdIHtcclxuICAgIHJldHVybiBuYXZpZ2F0aW9uRWxlbWVudHM7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKEFkZE5hdmlnYXRpb25FbGVtZW50KVxyXG4gIGxheW91dEFkZEFjdGlvbih7IGdldFN0YXRlLCBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxMYXlvdXQuU3RhdGU+LCB7IHBheWxvYWQgPSBbXSB9OiBBZGROYXZpZ2F0aW9uRWxlbWVudCkge1xyXG4gICAgbGV0IHsgbmF2aWdhdGlvbkVsZW1lbnRzIH0gPSBnZXRTdGF0ZSgpO1xyXG5cclxuICAgIGlmICghQXJyYXkuaXNBcnJheShwYXlsb2FkKSkge1xyXG4gICAgICBwYXlsb2FkID0gW3BheWxvYWRdO1xyXG4gICAgfVxyXG5cclxuICAgIGlmIChuYXZpZ2F0aW9uRWxlbWVudHMubGVuZ3RoKSB7XHJcbiAgICAgIHBheWxvYWQgPSBzbnEoXHJcbiAgICAgICAgKCkgPT5cclxuICAgICAgICAgIChwYXlsb2FkIGFzIExheW91dC5OYXZpZ2F0aW9uRWxlbWVudFtdKS5maWx0ZXIoXHJcbiAgICAgICAgICAgICh7IG5hbWUgfSkgPT4gbmF2aWdhdGlvbkVsZW1lbnRzLmZpbmRJbmRleChuYXYgPT4gbmF2Lm5hbWUgPT09IG5hbWUpIDwgMCxcclxuICAgICAgICAgICksXHJcbiAgICAgICAgW10sXHJcbiAgICAgICk7XHJcbiAgICB9XHJcblxyXG4gICAgaWYgKCFwYXlsb2FkLmxlbmd0aCkgcmV0dXJuO1xyXG5cclxuICAgIG5hdmlnYXRpb25FbGVtZW50cyA9IFsuLi5uYXZpZ2F0aW9uRWxlbWVudHMsIC4uLnBheWxvYWRdXHJcbiAgICAgIC5tYXAoZWxlbWVudCA9PiAoeyAuLi5lbGVtZW50LCBvcmRlcjogZWxlbWVudC5vcmRlciB8fCA5OSB9KSlcclxuICAgICAgLnNvcnQoKGEsIGIpID0+IGEub3JkZXIgLSBiLm9yZGVyKTtcclxuXHJcbiAgICByZXR1cm4gcGF0Y2hTdGF0ZSh7XHJcbiAgICAgIG5hdmlnYXRpb25FbGVtZW50cyxcclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihSZW1vdmVOYXZpZ2F0aW9uRWxlbWVudEJ5TmFtZSlcclxuICBsYXlvdXRSZW1vdmVBY3Rpb24oeyBnZXRTdGF0ZSwgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8TGF5b3V0LlN0YXRlPiwgeyBuYW1lIH06IFJlbW92ZU5hdmlnYXRpb25FbGVtZW50QnlOYW1lKSB7XHJcbiAgICBsZXQgeyBuYXZpZ2F0aW9uRWxlbWVudHMgfSA9IGdldFN0YXRlKCk7XHJcblxyXG4gICAgY29uc3QgaW5kZXggPSBuYXZpZ2F0aW9uRWxlbWVudHMuZmluZEluZGV4KGVsZW1lbnQgPT4gZWxlbWVudC5uYW1lID09PSBuYW1lKTtcclxuXHJcbiAgICBpZiAoaW5kZXggPiAtMSkge1xyXG4gICAgICBuYXZpZ2F0aW9uRWxlbWVudHMgPSBuYXZpZ2F0aW9uRWxlbWVudHMuc3BsaWNlKGluZGV4LCAxKTtcclxuICAgIH1cclxuXHJcbiAgICByZXR1cm4gcGF0Y2hTdGF0ZSh7XHJcbiAgICAgIG5hdmlnYXRpb25FbGVtZW50cyxcclxuICAgIH0pO1xyXG4gIH1cclxufVxyXG4iXX0=