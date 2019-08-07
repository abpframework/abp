/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { State, Action, Selector } from '@ngxs/store';
import { LayoutAddNavigationElement, LayoutRemoveNavigationElementByName } from '../actions/layout.actions';
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
    return LayoutState;
}());
export { LayoutState };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvbGF5b3V0LnN0YXRlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQWdCLFFBQVEsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwRSxPQUFPLEVBQUUsMEJBQTBCLEVBQUUsbUNBQW1DLEVBQUUsTUFBTSwyQkFBMkIsQ0FBQztBQUc1RyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7OztJQTBEdEIsQ0FBQzs7Ozs7SUFsRFEsaUNBQXFCOzs7O0lBQTVCLFVBQTZCLEVBQW9DO1lBQWxDLDBDQUFrQjtRQUMvQyxPQUFPLGtCQUFrQixDQUFDO0lBQzVCLENBQUM7Ozs7OztJQUdELHFDQUFlOzs7OztJQUFmLFVBQWdCLEVBQW9ELEVBQUUsRUFBNEM7WUFBaEcsc0JBQVEsRUFBRSwwQkFBVTtZQUFrQyxlQUFZLEVBQVosaUNBQVk7UUFDNUUsSUFBQSxrREFBa0I7UUFFeEIsSUFBSSxDQUFDLEtBQUssQ0FBQyxPQUFPLENBQUMsT0FBTyxDQUFDLEVBQUU7WUFDM0IsT0FBTyxHQUFHLENBQUMsT0FBTyxDQUFDLENBQUM7U0FDckI7UUFFRCxJQUFJLGtCQUFrQixDQUFDLE1BQU0sRUFBRTtZQUM3QixPQUFPLEdBQUcsR0FBRzs7O1lBQ1g7Z0JBQ0UsT0FBQSxDQUFDLG1CQUFBLE9BQU8sRUFBOEIsQ0FBQyxDQUFDLE1BQU07Ozs7Z0JBQzVDLFVBQUMsRUFBUTt3QkFBTixjQUFJO29CQUFPLE9BQUEsa0JBQWtCLENBQUMsU0FBUzs7OztvQkFBQyxVQUFBLEdBQUcsSUFBSSxPQUFBLEdBQUcsQ0FBQyxJQUFJLEtBQUssSUFBSSxFQUFqQixDQUFpQixFQUFDLEdBQUcsQ0FBQztnQkFBMUQsQ0FBMEQsRUFDekU7WUFGRCxDQUVDLEdBQ0gsRUFBRSxDQUNILENBQUM7U0FDSDtRQUVELElBQUksQ0FBQyxPQUFPLENBQUMsTUFBTTtZQUFFLE9BQU87UUFFNUIsa0JBQWtCLEdBQUcsaUJBQUksa0JBQWtCLEVBQUssT0FBTyxFQUNwRCxHQUFHOzs7O1FBQUMsVUFBQSxPQUFPLElBQUksT0FBQSxzQkFBTSxPQUFPLElBQUUsS0FBSyxFQUFFLE9BQU8sQ0FBQyxLQUFLLElBQUksRUFBRSxJQUFHLEVBQTVDLENBQTRDLEVBQUM7YUFDNUQsSUFBSTs7Ozs7UUFBQyxVQUFDLENBQUMsRUFBRSxDQUFDLElBQUssT0FBQSxDQUFDLENBQUMsS0FBSyxHQUFHLENBQUMsQ0FBQyxLQUFLLEVBQWpCLENBQWlCLEVBQUMsQ0FBQztRQUVyQyxPQUFPLFVBQVUsQ0FBQztZQUNoQixrQkFBa0Isb0JBQUE7U0FDbkIsQ0FBQyxDQUFDO0lBQ0wsQ0FBQzs7Ozs7O0lBR0Qsd0NBQWtCOzs7OztJQUFsQixVQUNFLEVBQW9ELEVBQ3BELEVBQTZDO1lBRDNDLHNCQUFRLEVBQUUsMEJBQVU7WUFDcEIsY0FBSTtRQUVBLElBQUEsa0RBQWtCOztZQUVsQixLQUFLLEdBQUcsa0JBQWtCLENBQUMsU0FBUzs7OztRQUFDLFVBQUEsT0FBTyxJQUFJLE9BQUEsT0FBTyxDQUFDLElBQUksS0FBSyxJQUFJLEVBQXJCLENBQXFCLEVBQUM7UUFFNUUsSUFBSSxLQUFLLEdBQUcsQ0FBQyxDQUFDLEVBQUU7WUFDZCxrQkFBa0IsR0FBRyxrQkFBa0IsQ0FBQyxNQUFNLENBQUMsS0FBSyxFQUFFLENBQUMsQ0FBQyxDQUFDO1NBQzFEO1FBRUQsT0FBTyxVQUFVLENBQUM7WUFDaEIsa0JBQWtCLG9CQUFBO1NBQ25CLENBQUMsQ0FBQztJQUNMLENBQUM7SUE1Q0Q7UUFEQyxNQUFNLENBQUMsMEJBQTBCLENBQUM7O3lEQUNxRCwwQkFBMEI7O3NEQTBCakg7SUFHRDtRQURDLE1BQU0sQ0FBQyxtQ0FBbUMsQ0FBQzs7eURBR2hDLG1DQUFtQzs7eURBYTlDO0lBakREO1FBREMsUUFBUSxFQUFFOzs7O2tEQUdWO0lBSlUsV0FBVztRQUp2QixLQUFLLENBQWU7WUFDbkIsSUFBSSxFQUFFLGFBQWE7WUFDbkIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsa0JBQWtCLEVBQUUsRUFBRSxFQUFFLEVBQWdCO1NBQ3JELENBQUM7T0FDVyxXQUFXLENBb0R2QjtJQUFELGtCQUFDO0NBQUEsSUFBQTtTQXBEWSxXQUFXIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhdGUsIEFjdGlvbiwgU3RhdGVDb250ZXh0LCBTZWxlY3RvciB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IExheW91dEFkZE5hdmlnYXRpb25FbGVtZW50LCBMYXlvdXRSZW1vdmVOYXZpZ2F0aW9uRWxlbWVudEJ5TmFtZSB9IGZyb20gJy4uL2FjdGlvbnMvbGF5b3V0LmFjdGlvbnMnO1xuaW1wb3J0IHsgTGF5b3V0IH0gZnJvbSAnLi4vbW9kZWxzL2xheW91dCc7XG5pbXBvcnQgeyBUZW1wbGF0ZVJlZiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuXG5AU3RhdGU8TGF5b3V0LlN0YXRlPih7XG4gIG5hbWU6ICdMYXlvdXRTdGF0ZScsXG4gIGRlZmF1bHRzOiB7IG5hdmlnYXRpb25FbGVtZW50czogW10gfSBhcyBMYXlvdXQuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIExheW91dFN0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldE5hdmlnYXRpb25FbGVtZW50cyh7IG5hdmlnYXRpb25FbGVtZW50cyB9OiBMYXlvdXQuU3RhdGUpOiBMYXlvdXQuTmF2aWdhdGlvbkVsZW1lbnRbXSB7XG4gICAgcmV0dXJuIG5hdmlnYXRpb25FbGVtZW50cztcbiAgfVxuXG4gIEBBY3Rpb24oTGF5b3V0QWRkTmF2aWdhdGlvbkVsZW1lbnQpXG4gIGxheW91dEFkZEFjdGlvbih7IGdldFN0YXRlLCBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxMYXlvdXQuU3RhdGU+LCB7IHBheWxvYWQgPSBbXSB9OiBMYXlvdXRBZGROYXZpZ2F0aW9uRWxlbWVudCkge1xuICAgIGxldCB7IG5hdmlnYXRpb25FbGVtZW50cyB9ID0gZ2V0U3RhdGUoKTtcblxuICAgIGlmICghQXJyYXkuaXNBcnJheShwYXlsb2FkKSkge1xuICAgICAgcGF5bG9hZCA9IFtwYXlsb2FkXTtcbiAgICB9XG5cbiAgICBpZiAobmF2aWdhdGlvbkVsZW1lbnRzLmxlbmd0aCkge1xuICAgICAgcGF5bG9hZCA9IHNucShcbiAgICAgICAgKCkgPT5cbiAgICAgICAgICAocGF5bG9hZCBhcyBMYXlvdXQuTmF2aWdhdGlvbkVsZW1lbnRbXSkuZmlsdGVyKFxuICAgICAgICAgICAgKHsgbmFtZSB9KSA9PiBuYXZpZ2F0aW9uRWxlbWVudHMuZmluZEluZGV4KG5hdiA9PiBuYXYubmFtZSA9PT0gbmFtZSkgPCAwLFxuICAgICAgICAgICksXG4gICAgICAgIFtdLFxuICAgICAgKTtcbiAgICB9XG5cbiAgICBpZiAoIXBheWxvYWQubGVuZ3RoKSByZXR1cm47XG5cbiAgICBuYXZpZ2F0aW9uRWxlbWVudHMgPSBbLi4ubmF2aWdhdGlvbkVsZW1lbnRzLCAuLi5wYXlsb2FkXVxuICAgICAgLm1hcChlbGVtZW50ID0+ICh7IC4uLmVsZW1lbnQsIG9yZGVyOiBlbGVtZW50Lm9yZGVyIHx8IDk5IH0pKVxuICAgICAgLnNvcnQoKGEsIGIpID0+IGEub3JkZXIgLSBiLm9yZGVyKTtcblxuICAgIHJldHVybiBwYXRjaFN0YXRlKHtcbiAgICAgIG5hdmlnYXRpb25FbGVtZW50cyxcbiAgICB9KTtcbiAgfVxuXG4gIEBBY3Rpb24oTGF5b3V0UmVtb3ZlTmF2aWdhdGlvbkVsZW1lbnRCeU5hbWUpXG4gIGxheW91dFJlbW92ZUFjdGlvbihcbiAgICB7IGdldFN0YXRlLCBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxMYXlvdXQuU3RhdGU+LFxuICAgIHsgbmFtZSB9OiBMYXlvdXRSZW1vdmVOYXZpZ2F0aW9uRWxlbWVudEJ5TmFtZSxcbiAgKSB7XG4gICAgbGV0IHsgbmF2aWdhdGlvbkVsZW1lbnRzIH0gPSBnZXRTdGF0ZSgpO1xuXG4gICAgY29uc3QgaW5kZXggPSBuYXZpZ2F0aW9uRWxlbWVudHMuZmluZEluZGV4KGVsZW1lbnQgPT4gZWxlbWVudC5uYW1lID09PSBuYW1lKTtcblxuICAgIGlmIChpbmRleCA+IC0xKSB7XG4gICAgICBuYXZpZ2F0aW9uRWxlbWVudHMgPSBuYXZpZ2F0aW9uRWxlbWVudHMuc3BsaWNlKGluZGV4LCAxKTtcbiAgICB9XG5cbiAgICByZXR1cm4gcGF0Y2hTdGF0ZSh7XG4gICAgICBuYXZpZ2F0aW9uRWxlbWVudHMsXG4gICAgfSk7XG4gIH1cbn1cbiJdfQ==