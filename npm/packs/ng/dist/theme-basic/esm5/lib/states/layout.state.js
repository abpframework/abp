/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { State, Action, Selector } from '@ngxs/store';
import { LayoutAddNavigationElement } from '../actions/layout.actions';
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
    LayoutState.prototype.layoutAction = /**
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
    tslib_1.__decorate([
        Action(LayoutAddNavigationElement),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, LayoutAddNavigationElement]),
        tslib_1.__metadata("design:returntype", void 0)
    ], LayoutState.prototype, "layoutAction", null);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvbGF5b3V0LnN0YXRlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQWdCLFFBQVEsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwRSxPQUFPLEVBQUUsMEJBQTBCLEVBQUUsTUFBTSwyQkFBMkIsQ0FBQztBQUd2RSxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7OztJQXdDdEIsQ0FBQzs7Ozs7SUFoQ1EsaUNBQXFCOzs7O0lBQTVCLFVBQTZCLEVBQW9DO1lBQWxDLDBDQUFrQjtRQUMvQyxPQUFPLGtCQUFrQixDQUFDO0lBQzVCLENBQUM7Ozs7OztJQUdELGtDQUFZOzs7OztJQUFaLFVBQWEsRUFBb0QsRUFBRSxFQUE0QztZQUFoRyxzQkFBUSxFQUFFLDBCQUFVO1lBQWtDLGVBQVksRUFBWixpQ0FBWTtRQUN6RSxJQUFBLGtEQUFrQjtRQUV4QixJQUFJLENBQUMsS0FBSyxDQUFDLE9BQU8sQ0FBQyxPQUFPLENBQUMsRUFBRTtZQUMzQixPQUFPLEdBQUcsQ0FBQyxPQUFPLENBQUMsQ0FBQztTQUNyQjtRQUVELElBQUksa0JBQWtCLENBQUMsTUFBTSxFQUFFO1lBQzdCLE9BQU8sR0FBRyxHQUFHOzs7WUFDWDtnQkFDRSxPQUFBLENBQUMsbUJBQUEsT0FBTyxFQUE4QixDQUFDLENBQUMsTUFBTTs7OztnQkFDNUMsVUFBQyxFQUFRO3dCQUFOLGNBQUk7b0JBQU8sT0FBQSxrQkFBa0IsQ0FBQyxTQUFTOzs7O29CQUFDLFVBQUEsR0FBRyxJQUFJLE9BQUEsR0FBRyxDQUFDLElBQUksS0FBSyxJQUFJLEVBQWpCLENBQWlCLEVBQUMsR0FBRyxDQUFDO2dCQUExRCxDQUEwRCxFQUN6RTtZQUZELENBRUMsR0FDSCxFQUFFLENBQ0gsQ0FBQztTQUNIO1FBRUQsSUFBSSxDQUFDLE9BQU8sQ0FBQyxNQUFNO1lBQUUsT0FBTztRQUU1QixrQkFBa0IsR0FBRyxpQkFBSSxrQkFBa0IsRUFBSyxPQUFPLEVBQ3BELEdBQUc7Ozs7UUFBQyxVQUFBLE9BQU8sSUFBSSxPQUFBLHNCQUFNLE9BQU8sSUFBRSxLQUFLLEVBQUUsT0FBTyxDQUFDLEtBQUssSUFBSSxFQUFFLElBQUcsRUFBNUMsQ0FBNEMsRUFBQzthQUM1RCxJQUFJOzs7OztRQUFDLFVBQUMsQ0FBQyxFQUFFLENBQUMsSUFBSyxPQUFBLENBQUMsQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDLEtBQUssRUFBakIsQ0FBaUIsRUFBQyxDQUFDO1FBRXJDLE9BQU8sVUFBVSxDQUFDO1lBQ2hCLGtCQUFrQixvQkFBQTtTQUNuQixDQUFDLENBQUM7SUFDTCxDQUFDO0lBMUJEO1FBREMsTUFBTSxDQUFDLDBCQUEwQixDQUFDOzt5REFDa0QsMEJBQTBCOzttREEwQjlHO0lBL0JEO1FBREMsUUFBUSxFQUFFOzs7O2tEQUdWO0lBSlUsV0FBVztRQUp2QixLQUFLLENBQWU7WUFDbkIsSUFBSSxFQUFFLGFBQWE7WUFDbkIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsa0JBQWtCLEVBQUUsRUFBRSxFQUFFLEVBQWdCO1NBQ3JELENBQUM7T0FDVyxXQUFXLENBa0N2QjtJQUFELGtCQUFDO0NBQUEsSUFBQTtTQWxDWSxXQUFXIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhdGUsIEFjdGlvbiwgU3RhdGVDb250ZXh0LCBTZWxlY3RvciB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IExheW91dEFkZE5hdmlnYXRpb25FbGVtZW50IH0gZnJvbSAnLi4vYWN0aW9ucy9sYXlvdXQuYWN0aW9ucyc7XG5pbXBvcnQgeyBMYXlvdXQgfSBmcm9tICcuLi9tb2RlbHMvbGF5b3V0JztcbmltcG9ydCB7IFRlbXBsYXRlUmVmIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5cbkBTdGF0ZTxMYXlvdXQuU3RhdGU+KHtcbiAgbmFtZTogJ0xheW91dFN0YXRlJyxcbiAgZGVmYXVsdHM6IHsgbmF2aWdhdGlvbkVsZW1lbnRzOiBbXSB9IGFzIExheW91dC5TdGF0ZSxcbn0pXG5leHBvcnQgY2xhc3MgTGF5b3V0U3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0TmF2aWdhdGlvbkVsZW1lbnRzKHsgbmF2aWdhdGlvbkVsZW1lbnRzIH06IExheW91dC5TdGF0ZSk6IExheW91dC5OYXZpZ2F0aW9uRWxlbWVudFtdIHtcbiAgICByZXR1cm4gbmF2aWdhdGlvbkVsZW1lbnRzO1xuICB9XG5cbiAgQEFjdGlvbihMYXlvdXRBZGROYXZpZ2F0aW9uRWxlbWVudClcbiAgbGF5b3V0QWN0aW9uKHsgZ2V0U3RhdGUsIHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PExheW91dC5TdGF0ZT4sIHsgcGF5bG9hZCA9IFtdIH06IExheW91dEFkZE5hdmlnYXRpb25FbGVtZW50KSB7XG4gICAgbGV0IHsgbmF2aWdhdGlvbkVsZW1lbnRzIH0gPSBnZXRTdGF0ZSgpO1xuXG4gICAgaWYgKCFBcnJheS5pc0FycmF5KHBheWxvYWQpKSB7XG4gICAgICBwYXlsb2FkID0gW3BheWxvYWRdO1xuICAgIH1cblxuICAgIGlmIChuYXZpZ2F0aW9uRWxlbWVudHMubGVuZ3RoKSB7XG4gICAgICBwYXlsb2FkID0gc25xKFxuICAgICAgICAoKSA9PlxuICAgICAgICAgIChwYXlsb2FkIGFzIExheW91dC5OYXZpZ2F0aW9uRWxlbWVudFtdKS5maWx0ZXIoXG4gICAgICAgICAgICAoeyBuYW1lIH0pID0+IG5hdmlnYXRpb25FbGVtZW50cy5maW5kSW5kZXgobmF2ID0+IG5hdi5uYW1lID09PSBuYW1lKSA8IDAsXG4gICAgICAgICAgKSxcbiAgICAgICAgW10sXG4gICAgICApO1xuICAgIH1cblxuICAgIGlmICghcGF5bG9hZC5sZW5ndGgpIHJldHVybjtcblxuICAgIG5hdmlnYXRpb25FbGVtZW50cyA9IFsuLi5uYXZpZ2F0aW9uRWxlbWVudHMsIC4uLnBheWxvYWRdXG4gICAgICAubWFwKGVsZW1lbnQgPT4gKHsgLi4uZWxlbWVudCwgb3JkZXI6IGVsZW1lbnQub3JkZXIgfHwgOTkgfSkpXG4gICAgICAuc29ydCgoYSwgYikgPT4gYS5vcmRlciAtIGIub3JkZXIpO1xuXG4gICAgcmV0dXJuIHBhdGNoU3RhdGUoe1xuICAgICAgbmF2aWdhdGlvbkVsZW1lbnRzLFxuICAgIH0pO1xuICB9XG59XG4iXX0=