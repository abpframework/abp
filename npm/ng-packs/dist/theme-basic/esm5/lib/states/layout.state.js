/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvbGF5b3V0LnN0YXRlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQWdCLFFBQVEsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwRSxPQUFPLEVBQUUsb0JBQW9CLEVBQUUsNkJBQTZCLEVBQUUsTUFBTSwyQkFBMkIsQ0FBQztBQUdoRyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7OztJQXVEdEIsQ0FBQzs7Ozs7SUEvQ1EsaUNBQXFCOzs7O0lBQTVCLFVBQTZCLEVBQW9DO1lBQWxDLDBDQUFrQjtRQUMvQyxPQUFPLGtCQUFrQixDQUFDO0lBQzVCLENBQUM7Ozs7OztJQUdELHFDQUFlOzs7OztJQUFmLFVBQWdCLEVBQW9ELEVBQUUsRUFBc0M7WUFBMUYsc0JBQVEsRUFBRSwwQkFBVTtZQUFrQyxlQUFZLEVBQVosaUNBQVk7UUFDNUUsSUFBQSxrREFBa0I7UUFFeEIsSUFBSSxDQUFDLEtBQUssQ0FBQyxPQUFPLENBQUMsT0FBTyxDQUFDLEVBQUU7WUFDM0IsT0FBTyxHQUFHLENBQUMsT0FBTyxDQUFDLENBQUM7U0FDckI7UUFFRCxJQUFJLGtCQUFrQixDQUFDLE1BQU0sRUFBRTtZQUM3QixPQUFPLEdBQUcsR0FBRzs7O1lBQ1g7Z0JBQ0UsT0FBQSxDQUFDLG1CQUFBLE9BQU8sRUFBOEIsQ0FBQyxDQUFDLE1BQU07Ozs7Z0JBQzVDLFVBQUMsRUFBUTt3QkFBTixjQUFJO29CQUFPLE9BQUEsa0JBQWtCLENBQUMsU0FBUzs7OztvQkFBQyxVQUFBLEdBQUcsSUFBSSxPQUFBLEdBQUcsQ0FBQyxJQUFJLEtBQUssSUFBSSxFQUFqQixDQUFpQixFQUFDLEdBQUcsQ0FBQztnQkFBMUQsQ0FBMEQsRUFDekU7WUFGRCxDQUVDLEdBQ0gsRUFBRSxDQUNILENBQUM7U0FDSDtRQUVELElBQUksQ0FBQyxPQUFPLENBQUMsTUFBTTtZQUFFLE9BQU87UUFFNUIsa0JBQWtCLEdBQUcsaUJBQUksa0JBQWtCLEVBQUssT0FBTyxFQUNwRCxHQUFHOzs7O1FBQUMsVUFBQSxPQUFPLElBQUksT0FBQSxzQkFBTSxPQUFPLElBQUUsS0FBSyxFQUFFLE9BQU8sQ0FBQyxLQUFLLElBQUksRUFBRSxJQUFHLEVBQTVDLENBQTRDLEVBQUM7YUFDNUQsSUFBSTs7Ozs7UUFBQyxVQUFDLENBQUMsRUFBRSxDQUFDLElBQUssT0FBQSxDQUFDLENBQUMsS0FBSyxHQUFHLENBQUMsQ0FBQyxLQUFLLEVBQWpCLENBQWlCLEVBQUMsQ0FBQztRQUVyQyxPQUFPLFVBQVUsQ0FBQztZQUNoQixrQkFBa0Isb0JBQUE7U0FDbkIsQ0FBQyxDQUFDO0lBQ0wsQ0FBQzs7Ozs7O0lBR0Qsd0NBQWtCOzs7OztJQUFsQixVQUFtQixFQUFvRCxFQUFFLEVBQXVDO1lBQTNGLHNCQUFRLEVBQUUsMEJBQVU7WUFBa0MsY0FBSTtRQUN2RSxJQUFBLGtEQUFrQjs7WUFFbEIsS0FBSyxHQUFHLGtCQUFrQixDQUFDLFNBQVM7Ozs7UUFBQyxVQUFBLE9BQU8sSUFBSSxPQUFBLE9BQU8sQ0FBQyxJQUFJLEtBQUssSUFBSSxFQUFyQixDQUFxQixFQUFDO1FBRTVFLElBQUksS0FBSyxHQUFHLENBQUMsQ0FBQyxFQUFFO1lBQ2Qsa0JBQWtCLEdBQUcsa0JBQWtCLENBQUMsTUFBTSxDQUFDLEtBQUssRUFBRSxDQUFDLENBQUMsQ0FBQztTQUMxRDtRQUVELE9BQU8sVUFBVSxDQUFDO1lBQ2hCLGtCQUFrQixvQkFBQTtTQUNuQixDQUFDLENBQUM7SUFDTCxDQUFDO0lBekNEO1FBREMsTUFBTSxDQUFDLG9CQUFvQixDQUFDOzt5REFDMkQsb0JBQW9COztzREEwQjNHO0lBR0Q7UUFEQyxNQUFNLENBQUMsNkJBQTZCLENBQUM7O3lEQUM2Qyw2QkFBNkI7O3lEQVkvRztJQTlDRDtRQURDLFFBQVEsRUFBRTs7OztrREFHVjtJQUpVLFdBQVc7UUFKdkIsS0FBSyxDQUFlO1lBQ25CLElBQUksRUFBRSxhQUFhO1lBQ25CLFFBQVEsRUFBRSxtQkFBQSxFQUFFLGtCQUFrQixFQUFFLEVBQUUsRUFBRSxFQUFnQjtTQUNyRCxDQUFDO09BQ1csV0FBVyxDQWlEdkI7SUFBRCxrQkFBQztDQUFBLElBQUE7U0FqRFksV0FBVyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFN0YXRlLCBBY3Rpb24sIFN0YXRlQ29udGV4dCwgU2VsZWN0b3IgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBBZGROYXZpZ2F0aW9uRWxlbWVudCwgUmVtb3ZlTmF2aWdhdGlvbkVsZW1lbnRCeU5hbWUgfSBmcm9tICcuLi9hY3Rpb25zL2xheW91dC5hY3Rpb25zJztcbmltcG9ydCB7IExheW91dCB9IGZyb20gJy4uL21vZGVscy9sYXlvdXQnO1xuaW1wb3J0IHsgVGVtcGxhdGVSZWYgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcblxuQFN0YXRlPExheW91dC5TdGF0ZT4oe1xuICBuYW1lOiAnTGF5b3V0U3RhdGUnLFxuICBkZWZhdWx0czogeyBuYXZpZ2F0aW9uRWxlbWVudHM6IFtdIH0gYXMgTGF5b3V0LlN0YXRlLFxufSlcbmV4cG9ydCBjbGFzcyBMYXlvdXRTdGF0ZSB7XG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXROYXZpZ2F0aW9uRWxlbWVudHMoeyBuYXZpZ2F0aW9uRWxlbWVudHMgfTogTGF5b3V0LlN0YXRlKTogTGF5b3V0Lk5hdmlnYXRpb25FbGVtZW50W10ge1xuICAgIHJldHVybiBuYXZpZ2F0aW9uRWxlbWVudHM7XG4gIH1cblxuICBAQWN0aW9uKEFkZE5hdmlnYXRpb25FbGVtZW50KVxuICBsYXlvdXRBZGRBY3Rpb24oeyBnZXRTdGF0ZSwgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8TGF5b3V0LlN0YXRlPiwgeyBwYXlsb2FkID0gW10gfTogQWRkTmF2aWdhdGlvbkVsZW1lbnQpIHtcbiAgICBsZXQgeyBuYXZpZ2F0aW9uRWxlbWVudHMgfSA9IGdldFN0YXRlKCk7XG5cbiAgICBpZiAoIUFycmF5LmlzQXJyYXkocGF5bG9hZCkpIHtcbiAgICAgIHBheWxvYWQgPSBbcGF5bG9hZF07XG4gICAgfVxuXG4gICAgaWYgKG5hdmlnYXRpb25FbGVtZW50cy5sZW5ndGgpIHtcbiAgICAgIHBheWxvYWQgPSBzbnEoXG4gICAgICAgICgpID0+XG4gICAgICAgICAgKHBheWxvYWQgYXMgTGF5b3V0Lk5hdmlnYXRpb25FbGVtZW50W10pLmZpbHRlcihcbiAgICAgICAgICAgICh7IG5hbWUgfSkgPT4gbmF2aWdhdGlvbkVsZW1lbnRzLmZpbmRJbmRleChuYXYgPT4gbmF2Lm5hbWUgPT09IG5hbWUpIDwgMCxcbiAgICAgICAgICApLFxuICAgICAgICBbXSxcbiAgICAgICk7XG4gICAgfVxuXG4gICAgaWYgKCFwYXlsb2FkLmxlbmd0aCkgcmV0dXJuO1xuXG4gICAgbmF2aWdhdGlvbkVsZW1lbnRzID0gWy4uLm5hdmlnYXRpb25FbGVtZW50cywgLi4ucGF5bG9hZF1cbiAgICAgIC5tYXAoZWxlbWVudCA9PiAoeyAuLi5lbGVtZW50LCBvcmRlcjogZWxlbWVudC5vcmRlciB8fCA5OSB9KSlcbiAgICAgIC5zb3J0KChhLCBiKSA9PiBhLm9yZGVyIC0gYi5vcmRlcik7XG5cbiAgICByZXR1cm4gcGF0Y2hTdGF0ZSh7XG4gICAgICBuYXZpZ2F0aW9uRWxlbWVudHMsXG4gICAgfSk7XG4gIH1cblxuICBAQWN0aW9uKFJlbW92ZU5hdmlnYXRpb25FbGVtZW50QnlOYW1lKVxuICBsYXlvdXRSZW1vdmVBY3Rpb24oeyBnZXRTdGF0ZSwgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8TGF5b3V0LlN0YXRlPiwgeyBuYW1lIH06IFJlbW92ZU5hdmlnYXRpb25FbGVtZW50QnlOYW1lKSB7XG4gICAgbGV0IHsgbmF2aWdhdGlvbkVsZW1lbnRzIH0gPSBnZXRTdGF0ZSgpO1xuXG4gICAgY29uc3QgaW5kZXggPSBuYXZpZ2F0aW9uRWxlbWVudHMuZmluZEluZGV4KGVsZW1lbnQgPT4gZWxlbWVudC5uYW1lID09PSBuYW1lKTtcblxuICAgIGlmIChpbmRleCA+IC0xKSB7XG4gICAgICBuYXZpZ2F0aW9uRWxlbWVudHMgPSBuYXZpZ2F0aW9uRWxlbWVudHMuc3BsaWNlKGluZGV4LCAxKTtcbiAgICB9XG5cbiAgICByZXR1cm4gcGF0Y2hTdGF0ZSh7XG4gICAgICBuYXZpZ2F0aW9uRWxlbWVudHMsXG4gICAgfSk7XG4gIH1cbn1cbiJdfQ==