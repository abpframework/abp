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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvbGF5b3V0LnN0YXRlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7OztBQUFBLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFnQixRQUFRLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLG9CQUFvQixFQUFFLDZCQUE2QixFQUFFLE1BQU0sMkJBQTJCLENBQUM7QUFHaEcsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDOzs7SUF1RHRCLENBQUM7Ozs7O0lBL0NRLGlDQUFxQjs7OztJQUE1QixVQUE2QixFQUFvQztZQUFsQywwQ0FBa0I7UUFDL0MsT0FBTyxrQkFBa0IsQ0FBQztJQUM1QixDQUFDOzs7Ozs7SUFHRCxxQ0FBZTs7Ozs7SUFBZixVQUFnQixFQUFvRCxFQUFFLEVBQXNDO1lBQTFGLHNCQUFRLEVBQUUsMEJBQVU7WUFBa0MsZUFBWSxFQUFaLGlDQUFZO1FBQzVFLElBQUEsa0RBQWtCO1FBRXhCLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLE9BQU8sQ0FBQyxFQUFFO1lBQzNCLE9BQU8sR0FBRyxDQUFDLE9BQU8sQ0FBQyxDQUFDO1NBQ3JCO1FBRUQsSUFBSSxrQkFBa0IsQ0FBQyxNQUFNLEVBQUU7WUFDN0IsT0FBTyxHQUFHLEdBQUc7OztZQUNYO2dCQUNFLE9BQUEsQ0FBQyxtQkFBQSxPQUFPLEVBQThCLENBQUMsQ0FBQyxNQUFNOzs7O2dCQUM1QyxVQUFDLEVBQVE7d0JBQU4sY0FBSTtvQkFBTyxPQUFBLGtCQUFrQixDQUFDLFNBQVM7Ozs7b0JBQUMsVUFBQSxHQUFHLElBQUksT0FBQSxHQUFHLENBQUMsSUFBSSxLQUFLLElBQUksRUFBakIsQ0FBaUIsRUFBQyxHQUFHLENBQUM7Z0JBQTFELENBQTBELEVBQ3pFO1lBRkQsQ0FFQyxHQUNILEVBQUUsQ0FDSCxDQUFDO1NBQ0g7UUFFRCxJQUFJLENBQUMsT0FBTyxDQUFDLE1BQU07WUFBRSxPQUFPO1FBRTVCLGtCQUFrQixHQUFHLGlCQUFJLGtCQUFrQixFQUFLLE9BQU8sRUFDcEQsR0FBRzs7OztRQUFDLFVBQUEsT0FBTyxJQUFJLE9BQUEsc0JBQU0sT0FBTyxJQUFFLEtBQUssRUFBRSxPQUFPLENBQUMsS0FBSyxJQUFJLEVBQUUsSUFBRyxFQUE1QyxDQUE0QyxFQUFDO2FBQzVELElBQUk7Ozs7O1FBQUMsVUFBQyxDQUFDLEVBQUUsQ0FBQyxJQUFLLE9BQUEsQ0FBQyxDQUFDLEtBQUssR0FBRyxDQUFDLENBQUMsS0FBSyxFQUFqQixDQUFpQixFQUFDLENBQUM7UUFFckMsT0FBTyxVQUFVLENBQUM7WUFDaEIsa0JBQWtCLG9CQUFBO1NBQ25CLENBQUMsQ0FBQztJQUNMLENBQUM7Ozs7OztJQUdELHdDQUFrQjs7Ozs7SUFBbEIsVUFBbUIsRUFBb0QsRUFBRSxFQUF1QztZQUEzRixzQkFBUSxFQUFFLDBCQUFVO1lBQWtDLGNBQUk7UUFDdkUsSUFBQSxrREFBa0I7O1lBRWxCLEtBQUssR0FBRyxrQkFBa0IsQ0FBQyxTQUFTOzs7O1FBQUMsVUFBQSxPQUFPLElBQUksT0FBQSxPQUFPLENBQUMsSUFBSSxLQUFLLElBQUksRUFBckIsQ0FBcUIsRUFBQztRQUU1RSxJQUFJLEtBQUssR0FBRyxDQUFDLENBQUMsRUFBRTtZQUNkLGtCQUFrQixHQUFHLGtCQUFrQixDQUFDLE1BQU0sQ0FBQyxLQUFLLEVBQUUsQ0FBQyxDQUFDLENBQUM7U0FDMUQ7UUFFRCxPQUFPLFVBQVUsQ0FBQztZQUNoQixrQkFBa0Isb0JBQUE7U0FDbkIsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztJQXpDRDtRQURDLE1BQU0sQ0FBQyxvQkFBb0IsQ0FBQzs7eURBQzJELG9CQUFvQjs7c0RBMEIzRztJQUdEO1FBREMsTUFBTSxDQUFDLDZCQUE2QixDQUFDOzt5REFDNkMsNkJBQTZCOzt5REFZL0c7SUE5Q0Q7UUFEQyxRQUFRLEVBQUU7Ozs7a0RBR1Y7SUFKVSxXQUFXO1FBSnZCLEtBQUssQ0FBZTtZQUNuQixJQUFJLEVBQUUsYUFBYTtZQUNuQixRQUFRLEVBQUUsbUJBQUEsRUFBRSxrQkFBa0IsRUFBRSxFQUFFLEVBQUUsRUFBZ0I7U0FDckQsQ0FBQztPQUNXLFdBQVcsQ0FpRHZCO0lBQUQsa0JBQUM7Q0FBQSxJQUFBO1NBakRZLFdBQVciLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBTdGF0ZSwgQWN0aW9uLCBTdGF0ZUNvbnRleHQsIFNlbGVjdG9yIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgQWRkTmF2aWdhdGlvbkVsZW1lbnQsIFJlbW92ZU5hdmlnYXRpb25FbGVtZW50QnlOYW1lIH0gZnJvbSAnLi4vYWN0aW9ucy9sYXlvdXQuYWN0aW9ucyc7XG5pbXBvcnQgeyBMYXlvdXQgfSBmcm9tICcuLi9tb2RlbHMvbGF5b3V0JztcbmltcG9ydCB7IFRlbXBsYXRlUmVmIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5cbkBTdGF0ZTxMYXlvdXQuU3RhdGU+KHtcbiAgbmFtZTogJ0xheW91dFN0YXRlJyxcbiAgZGVmYXVsdHM6IHsgbmF2aWdhdGlvbkVsZW1lbnRzOiBbXSB9IGFzIExheW91dC5TdGF0ZSxcbn0pXG5leHBvcnQgY2xhc3MgTGF5b3V0U3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0TmF2aWdhdGlvbkVsZW1lbnRzKHsgbmF2aWdhdGlvbkVsZW1lbnRzIH06IExheW91dC5TdGF0ZSk6IExheW91dC5OYXZpZ2F0aW9uRWxlbWVudFtdIHtcbiAgICByZXR1cm4gbmF2aWdhdGlvbkVsZW1lbnRzO1xuICB9XG5cbiAgQEFjdGlvbihBZGROYXZpZ2F0aW9uRWxlbWVudClcbiAgbGF5b3V0QWRkQWN0aW9uKHsgZ2V0U3RhdGUsIHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PExheW91dC5TdGF0ZT4sIHsgcGF5bG9hZCA9IFtdIH06IEFkZE5hdmlnYXRpb25FbGVtZW50KSB7XG4gICAgbGV0IHsgbmF2aWdhdGlvbkVsZW1lbnRzIH0gPSBnZXRTdGF0ZSgpO1xuXG4gICAgaWYgKCFBcnJheS5pc0FycmF5KHBheWxvYWQpKSB7XG4gICAgICBwYXlsb2FkID0gW3BheWxvYWRdO1xuICAgIH1cblxuICAgIGlmIChuYXZpZ2F0aW9uRWxlbWVudHMubGVuZ3RoKSB7XG4gICAgICBwYXlsb2FkID0gc25xKFxuICAgICAgICAoKSA9PlxuICAgICAgICAgIChwYXlsb2FkIGFzIExheW91dC5OYXZpZ2F0aW9uRWxlbWVudFtdKS5maWx0ZXIoXG4gICAgICAgICAgICAoeyBuYW1lIH0pID0+IG5hdmlnYXRpb25FbGVtZW50cy5maW5kSW5kZXgobmF2ID0+IG5hdi5uYW1lID09PSBuYW1lKSA8IDAsXG4gICAgICAgICAgKSxcbiAgICAgICAgW10sXG4gICAgICApO1xuICAgIH1cblxuICAgIGlmICghcGF5bG9hZC5sZW5ndGgpIHJldHVybjtcblxuICAgIG5hdmlnYXRpb25FbGVtZW50cyA9IFsuLi5uYXZpZ2F0aW9uRWxlbWVudHMsIC4uLnBheWxvYWRdXG4gICAgICAubWFwKGVsZW1lbnQgPT4gKHsgLi4uZWxlbWVudCwgb3JkZXI6IGVsZW1lbnQub3JkZXIgfHwgOTkgfSkpXG4gICAgICAuc29ydCgoYSwgYikgPT4gYS5vcmRlciAtIGIub3JkZXIpO1xuXG4gICAgcmV0dXJuIHBhdGNoU3RhdGUoe1xuICAgICAgbmF2aWdhdGlvbkVsZW1lbnRzLFxuICAgIH0pO1xuICB9XG5cbiAgQEFjdGlvbihSZW1vdmVOYXZpZ2F0aW9uRWxlbWVudEJ5TmFtZSlcbiAgbGF5b3V0UmVtb3ZlQWN0aW9uKHsgZ2V0U3RhdGUsIHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PExheW91dC5TdGF0ZT4sIHsgbmFtZSB9OiBSZW1vdmVOYXZpZ2F0aW9uRWxlbWVudEJ5TmFtZSkge1xuICAgIGxldCB7IG5hdmlnYXRpb25FbGVtZW50cyB9ID0gZ2V0U3RhdGUoKTtcblxuICAgIGNvbnN0IGluZGV4ID0gbmF2aWdhdGlvbkVsZW1lbnRzLmZpbmRJbmRleChlbGVtZW50ID0+IGVsZW1lbnQubmFtZSA9PT0gbmFtZSk7XG5cbiAgICBpZiAoaW5kZXggPiAtMSkge1xuICAgICAgbmF2aWdhdGlvbkVsZW1lbnRzID0gbmF2aWdhdGlvbkVsZW1lbnRzLnNwbGljZShpbmRleCwgMSk7XG4gICAgfVxuXG4gICAgcmV0dXJuIHBhdGNoU3RhdGUoe1xuICAgICAgbmF2aWdhdGlvbkVsZW1lbnRzLFxuICAgIH0pO1xuICB9XG59XG4iXX0=