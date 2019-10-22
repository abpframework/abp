/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvbGF5b3V0LnN0YXRlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQWdCLFFBQVEsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwRSxPQUFPLEVBQUUsb0JBQW9CLEVBQUUsNkJBQTZCLEVBQUUsTUFBTSwyQkFBMkIsQ0FBQztBQUdoRyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7OztJQXVEdEIsQ0FBQzs7Ozs7SUEvQ1EsaUNBQXFCOzs7O0lBQTVCLFVBQTZCLEVBQW9DO1lBQWxDLDBDQUFrQjtRQUMvQyxPQUFPLGtCQUFrQixDQUFDO0lBQzVCLENBQUM7Ozs7OztJQUdELHFDQUFlOzs7OztJQUFmLFVBQWdCLEVBQW9ELEVBQUUsRUFBc0M7WUFBMUYsc0JBQVEsRUFBRSwwQkFBVTtZQUFrQyxlQUFZLEVBQVosaUNBQVk7UUFDNUUsSUFBQSxrREFBa0I7UUFFeEIsSUFBSSxDQUFDLEtBQUssQ0FBQyxPQUFPLENBQUMsT0FBTyxDQUFDLEVBQUU7WUFDM0IsT0FBTyxHQUFHLENBQUMsT0FBTyxDQUFDLENBQUM7U0FDckI7UUFFRCxJQUFJLGtCQUFrQixDQUFDLE1BQU0sRUFBRTtZQUM3QixPQUFPLEdBQUcsR0FBRzs7O1lBQ1g7Z0JBQ0UsT0FBQSxDQUFDLG1CQUFBLE9BQU8sRUFBOEIsQ0FBQyxDQUFDLE1BQU07Ozs7Z0JBQzVDLFVBQUMsRUFBUTt3QkFBTixjQUFJO29CQUFPLE9BQUEsa0JBQWtCLENBQUMsU0FBUzs7OztvQkFBQyxVQUFBLEdBQUcsSUFBSSxPQUFBLEdBQUcsQ0FBQyxJQUFJLEtBQUssSUFBSSxFQUFqQixDQUFpQixFQUFDLEdBQUcsQ0FBQztnQkFBMUQsQ0FBMEQsRUFDekU7WUFGRCxDQUVDLEdBQ0gsRUFBRSxDQUNILENBQUM7U0FDSDtRQUVELElBQUksQ0FBQyxPQUFPLENBQUMsTUFBTTtZQUFFLE9BQU87UUFFNUIsa0JBQWtCLEdBQUcsaUJBQUksa0JBQWtCLEVBQUssT0FBTyxFQUNwRCxHQUFHOzs7O1FBQUMsVUFBQSxPQUFPLElBQUksT0FBQSxzQkFBTSxPQUFPLElBQUUsS0FBSyxFQUFFLE9BQU8sQ0FBQyxLQUFLLElBQUksRUFBRSxJQUFHLEVBQTVDLENBQTRDLEVBQUM7YUFDNUQsSUFBSTs7Ozs7UUFBQyxVQUFDLENBQUMsRUFBRSxDQUFDLElBQUssT0FBQSxDQUFDLENBQUMsS0FBSyxHQUFHLENBQUMsQ0FBQyxLQUFLLEVBQWpCLENBQWlCLEVBQUMsQ0FBQztRQUVyQyxPQUFPLFVBQVUsQ0FBQztZQUNoQixrQkFBa0Isb0JBQUE7U0FDbkIsQ0FBQyxDQUFDO0lBQ0wsQ0FBQzs7Ozs7O0lBR0Qsd0NBQWtCOzs7OztJQUFsQixVQUFtQixFQUFvRCxFQUFFLEVBQXVDO1lBQTNGLHNCQUFRLEVBQUUsMEJBQVU7WUFBa0MsY0FBSTtRQUN2RSxJQUFBLGtEQUFrQjs7WUFFbEIsS0FBSyxHQUFHLGtCQUFrQixDQUFDLFNBQVM7Ozs7UUFBQyxVQUFBLE9BQU8sSUFBSSxPQUFBLE9BQU8sQ0FBQyxJQUFJLEtBQUssSUFBSSxFQUFyQixDQUFxQixFQUFDO1FBRTVFLElBQUksS0FBSyxHQUFHLENBQUMsQ0FBQyxFQUFFO1lBQ2Qsa0JBQWtCLEdBQUcsa0JBQWtCLENBQUMsTUFBTSxDQUFDLEtBQUssRUFBRSxDQUFDLENBQUMsQ0FBQztTQUMxRDtRQUVELE9BQU8sVUFBVSxDQUFDO1lBQ2hCLGtCQUFrQixvQkFBQTtTQUNuQixDQUFDLENBQUM7SUFDTCxDQUFDO0lBekNEO1FBREMsTUFBTSxDQUFDLG9CQUFvQixDQUFDOzt5REFDMkQsb0JBQW9COztzREEwQjNHO0lBR0Q7UUFEQyxNQUFNLENBQUMsNkJBQTZCLENBQUM7O3lEQUM2Qyw2QkFBNkI7O3lEQVkvRztJQTlDRDtRQURDLFFBQVEsRUFBRTs7OztrREFHVjtJQUpVLFdBQVc7UUFKdkIsS0FBSyxDQUFlO1lBQ25CLElBQUksRUFBRSxhQUFhO1lBQ25CLFFBQVEsRUFBRSxtQkFBQSxFQUFFLGtCQUFrQixFQUFFLEVBQUUsRUFBRSxFQUFnQjtTQUNyRCxDQUFDO09BQ1csV0FBVyxDQWlEdkI7SUFBRCxrQkFBQztDQUFBLElBQUE7U0FqRFksV0FBVyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFN0YXRlLCBBY3Rpb24sIFN0YXRlQ29udGV4dCwgU2VsZWN0b3IgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IEFkZE5hdmlnYXRpb25FbGVtZW50LCBSZW1vdmVOYXZpZ2F0aW9uRWxlbWVudEJ5TmFtZSB9IGZyb20gJy4uL2FjdGlvbnMvbGF5b3V0LmFjdGlvbnMnO1xyXG5pbXBvcnQgeyBMYXlvdXQgfSBmcm9tICcuLi9tb2RlbHMvbGF5b3V0JztcclxuaW1wb3J0IHsgVGVtcGxhdGVSZWYgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xyXG5cclxuQFN0YXRlPExheW91dC5TdGF0ZT4oe1xyXG4gIG5hbWU6ICdMYXlvdXRTdGF0ZScsXHJcbiAgZGVmYXVsdHM6IHsgbmF2aWdhdGlvbkVsZW1lbnRzOiBbXSB9IGFzIExheW91dC5TdGF0ZSxcclxufSlcclxuZXhwb3J0IGNsYXNzIExheW91dFN0YXRlIHtcclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXROYXZpZ2F0aW9uRWxlbWVudHMoeyBuYXZpZ2F0aW9uRWxlbWVudHMgfTogTGF5b3V0LlN0YXRlKTogTGF5b3V0Lk5hdmlnYXRpb25FbGVtZW50W10ge1xyXG4gICAgcmV0dXJuIG5hdmlnYXRpb25FbGVtZW50cztcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oQWRkTmF2aWdhdGlvbkVsZW1lbnQpXHJcbiAgbGF5b3V0QWRkQWN0aW9uKHsgZ2V0U3RhdGUsIHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PExheW91dC5TdGF0ZT4sIHsgcGF5bG9hZCA9IFtdIH06IEFkZE5hdmlnYXRpb25FbGVtZW50KSB7XHJcbiAgICBsZXQgeyBuYXZpZ2F0aW9uRWxlbWVudHMgfSA9IGdldFN0YXRlKCk7XHJcblxyXG4gICAgaWYgKCFBcnJheS5pc0FycmF5KHBheWxvYWQpKSB7XHJcbiAgICAgIHBheWxvYWQgPSBbcGF5bG9hZF07XHJcbiAgICB9XHJcblxyXG4gICAgaWYgKG5hdmlnYXRpb25FbGVtZW50cy5sZW5ndGgpIHtcclxuICAgICAgcGF5bG9hZCA9IHNucShcclxuICAgICAgICAoKSA9PlxyXG4gICAgICAgICAgKHBheWxvYWQgYXMgTGF5b3V0Lk5hdmlnYXRpb25FbGVtZW50W10pLmZpbHRlcihcclxuICAgICAgICAgICAgKHsgbmFtZSB9KSA9PiBuYXZpZ2F0aW9uRWxlbWVudHMuZmluZEluZGV4KG5hdiA9PiBuYXYubmFtZSA9PT0gbmFtZSkgPCAwLFxyXG4gICAgICAgICAgKSxcclxuICAgICAgICBbXSxcclxuICAgICAgKTtcclxuICAgIH1cclxuXHJcbiAgICBpZiAoIXBheWxvYWQubGVuZ3RoKSByZXR1cm47XHJcblxyXG4gICAgbmF2aWdhdGlvbkVsZW1lbnRzID0gWy4uLm5hdmlnYXRpb25FbGVtZW50cywgLi4ucGF5bG9hZF1cclxuICAgICAgLm1hcChlbGVtZW50ID0+ICh7IC4uLmVsZW1lbnQsIG9yZGVyOiBlbGVtZW50Lm9yZGVyIHx8IDk5IH0pKVxyXG4gICAgICAuc29ydCgoYSwgYikgPT4gYS5vcmRlciAtIGIub3JkZXIpO1xyXG5cclxuICAgIHJldHVybiBwYXRjaFN0YXRlKHtcclxuICAgICAgbmF2aWdhdGlvbkVsZW1lbnRzLFxyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKFJlbW92ZU5hdmlnYXRpb25FbGVtZW50QnlOYW1lKVxyXG4gIGxheW91dFJlbW92ZUFjdGlvbih7IGdldFN0YXRlLCBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxMYXlvdXQuU3RhdGU+LCB7IG5hbWUgfTogUmVtb3ZlTmF2aWdhdGlvbkVsZW1lbnRCeU5hbWUpIHtcclxuICAgIGxldCB7IG5hdmlnYXRpb25FbGVtZW50cyB9ID0gZ2V0U3RhdGUoKTtcclxuXHJcbiAgICBjb25zdCBpbmRleCA9IG5hdmlnYXRpb25FbGVtZW50cy5maW5kSW5kZXgoZWxlbWVudCA9PiBlbGVtZW50Lm5hbWUgPT09IG5hbWUpO1xyXG5cclxuICAgIGlmIChpbmRleCA+IC0xKSB7XHJcbiAgICAgIG5hdmlnYXRpb25FbGVtZW50cyA9IG5hdmlnYXRpb25FbGVtZW50cy5zcGxpY2UoaW5kZXgsIDEpO1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiBwYXRjaFN0YXRlKHtcclxuICAgICAgbmF2aWdhdGlvbkVsZW1lbnRzLFxyXG4gICAgfSk7XHJcbiAgfVxyXG59XHJcbiJdfQ==