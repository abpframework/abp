/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/layout.state.ts
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvbGF5b3V0LnN0YXRlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7OztBQUFBLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFnQixRQUFRLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLG9CQUFvQixFQUFFLDZCQUE2QixFQUFFLE1BQU0sMkJBQTJCLENBQUM7QUFHaEcsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0lBTVQsV0FBVyxTQUFYLFdBQVc7Ozs7O0lBRXRCLE1BQU0sQ0FBQyxxQkFBcUIsQ0FBQyxFQUFFLGtCQUFrQixFQUFnQjtRQUMvRCxPQUFPLGtCQUFrQixDQUFDO0lBQzVCLENBQUM7Ozs7OztJQUdELGVBQWUsQ0FBQyxFQUFFLFFBQVEsRUFBRSxVQUFVLEVBQThCLEVBQUUsRUFBRSxPQUFPLEdBQUcsRUFBRSxFQUF3QjtZQUN0RyxFQUFFLGtCQUFrQixFQUFFLEdBQUcsUUFBUSxFQUFFO1FBRXZDLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLE9BQU8sQ0FBQyxFQUFFO1lBQzNCLE9BQU8sR0FBRyxDQUFDLE9BQU8sQ0FBQyxDQUFDO1NBQ3JCO1FBRUQsSUFBSSxrQkFBa0IsQ0FBQyxNQUFNLEVBQUU7WUFDN0IsT0FBTyxHQUFHLEdBQUc7OztZQUNYLEdBQUcsRUFBRSxDQUNILENBQUMsbUJBQUEsT0FBTyxFQUE4QixDQUFDLENBQUMsTUFBTTs7OztZQUM1QyxDQUFDLEVBQUUsSUFBSSxFQUFFLEVBQUUsRUFBRSxDQUFDLGtCQUFrQixDQUFDLFNBQVM7Ozs7WUFBQyxHQUFHLENBQUMsRUFBRSxDQUFDLEdBQUcsQ0FBQyxJQUFJLEtBQUssSUFBSSxFQUFDLEdBQUcsQ0FBQyxFQUN6RSxHQUNILEVBQUUsQ0FDSCxDQUFDO1NBQ0g7UUFFRCxJQUFJLENBQUMsT0FBTyxDQUFDLE1BQU07WUFBRSxPQUFPO1FBRTVCLGtCQUFrQixHQUFHLENBQUMsR0FBRyxrQkFBa0IsRUFBRSxHQUFHLE9BQU8sQ0FBQzthQUNyRCxHQUFHOzs7O1FBQUMsT0FBTyxDQUFDLEVBQUUsQ0FBQyxtQkFBTSxPQUFPLElBQUUsS0FBSyxFQUFFLE9BQU8sQ0FBQyxLQUFLLElBQUksRUFBRSxJQUFHLEVBQUM7YUFDNUQsSUFBSTs7Ozs7UUFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLEVBQUUsRUFBRSxDQUFDLENBQUMsQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDLEtBQUssRUFBQyxDQUFDO1FBRXJDLE9BQU8sVUFBVSxDQUFDO1lBQ2hCLGtCQUFrQjtTQUNuQixDQUFDLENBQUM7SUFDTCxDQUFDOzs7Ozs7SUFHRCxrQkFBa0IsQ0FBQyxFQUFFLFFBQVEsRUFBRSxVQUFVLEVBQThCLEVBQUUsRUFBRSxJQUFJLEVBQWlDO1lBQzFHLEVBQUUsa0JBQWtCLEVBQUUsR0FBRyxRQUFRLEVBQUU7O2NBRWpDLEtBQUssR0FBRyxrQkFBa0IsQ0FBQyxTQUFTOzs7O1FBQUMsT0FBTyxDQUFDLEVBQUUsQ0FBQyxPQUFPLENBQUMsSUFBSSxLQUFLLElBQUksRUFBQztRQUU1RSxJQUFJLEtBQUssR0FBRyxDQUFDLENBQUMsRUFBRTtZQUNkLGtCQUFrQixHQUFHLGtCQUFrQixDQUFDLE1BQU0sQ0FBQyxLQUFLLEVBQUUsQ0FBQyxDQUFDLENBQUM7U0FDMUQ7UUFFRCxPQUFPLFVBQVUsQ0FBQztZQUNoQixrQkFBa0I7U0FDbkIsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztDQUNGLENBQUE7QUExQ0M7SUFEQyxNQUFNLENBQUMsb0JBQW9CLENBQUM7O3FEQUMyRCxvQkFBb0I7O2tEQTBCM0c7QUFHRDtJQURDLE1BQU0sQ0FBQyw2QkFBNkIsQ0FBQzs7cURBQzZDLDZCQUE2Qjs7cURBWS9HO0FBOUNEO0lBREMsUUFBUSxFQUFFOzs7OzhDQUdWO0FBSlUsV0FBVztJQUp2QixLQUFLLENBQWU7UUFDbkIsSUFBSSxFQUFFLGFBQWE7UUFDbkIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsa0JBQWtCLEVBQUUsRUFBRSxFQUFFLEVBQWdCO0tBQ3JELENBQUM7R0FDVyxXQUFXLENBaUR2QjtTQWpEWSxXQUFXIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhdGUsIEFjdGlvbiwgU3RhdGVDb250ZXh0LCBTZWxlY3RvciB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IEFkZE5hdmlnYXRpb25FbGVtZW50LCBSZW1vdmVOYXZpZ2F0aW9uRWxlbWVudEJ5TmFtZSB9IGZyb20gJy4uL2FjdGlvbnMvbGF5b3V0LmFjdGlvbnMnO1xuaW1wb3J0IHsgTGF5b3V0IH0gZnJvbSAnLi4vbW9kZWxzL2xheW91dCc7XG5pbXBvcnQgeyBUZW1wbGF0ZVJlZiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuXG5AU3RhdGU8TGF5b3V0LlN0YXRlPih7XG4gIG5hbWU6ICdMYXlvdXRTdGF0ZScsXG4gIGRlZmF1bHRzOiB7IG5hdmlnYXRpb25FbGVtZW50czogW10gfSBhcyBMYXlvdXQuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIExheW91dFN0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldE5hdmlnYXRpb25FbGVtZW50cyh7IG5hdmlnYXRpb25FbGVtZW50cyB9OiBMYXlvdXQuU3RhdGUpOiBMYXlvdXQuTmF2aWdhdGlvbkVsZW1lbnRbXSB7XG4gICAgcmV0dXJuIG5hdmlnYXRpb25FbGVtZW50cztcbiAgfVxuXG4gIEBBY3Rpb24oQWRkTmF2aWdhdGlvbkVsZW1lbnQpXG4gIGxheW91dEFkZEFjdGlvbih7IGdldFN0YXRlLCBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxMYXlvdXQuU3RhdGU+LCB7IHBheWxvYWQgPSBbXSB9OiBBZGROYXZpZ2F0aW9uRWxlbWVudCkge1xuICAgIGxldCB7IG5hdmlnYXRpb25FbGVtZW50cyB9ID0gZ2V0U3RhdGUoKTtcblxuICAgIGlmICghQXJyYXkuaXNBcnJheShwYXlsb2FkKSkge1xuICAgICAgcGF5bG9hZCA9IFtwYXlsb2FkXTtcbiAgICB9XG5cbiAgICBpZiAobmF2aWdhdGlvbkVsZW1lbnRzLmxlbmd0aCkge1xuICAgICAgcGF5bG9hZCA9IHNucShcbiAgICAgICAgKCkgPT5cbiAgICAgICAgICAocGF5bG9hZCBhcyBMYXlvdXQuTmF2aWdhdGlvbkVsZW1lbnRbXSkuZmlsdGVyKFxuICAgICAgICAgICAgKHsgbmFtZSB9KSA9PiBuYXZpZ2F0aW9uRWxlbWVudHMuZmluZEluZGV4KG5hdiA9PiBuYXYubmFtZSA9PT0gbmFtZSkgPCAwLFxuICAgICAgICAgICksXG4gICAgICAgIFtdLFxuICAgICAgKTtcbiAgICB9XG5cbiAgICBpZiAoIXBheWxvYWQubGVuZ3RoKSByZXR1cm47XG5cbiAgICBuYXZpZ2F0aW9uRWxlbWVudHMgPSBbLi4ubmF2aWdhdGlvbkVsZW1lbnRzLCAuLi5wYXlsb2FkXVxuICAgICAgLm1hcChlbGVtZW50ID0+ICh7IC4uLmVsZW1lbnQsIG9yZGVyOiBlbGVtZW50Lm9yZGVyIHx8IDk5IH0pKVxuICAgICAgLnNvcnQoKGEsIGIpID0+IGEub3JkZXIgLSBiLm9yZGVyKTtcblxuICAgIHJldHVybiBwYXRjaFN0YXRlKHtcbiAgICAgIG5hdmlnYXRpb25FbGVtZW50cyxcbiAgICB9KTtcbiAgfVxuXG4gIEBBY3Rpb24oUmVtb3ZlTmF2aWdhdGlvbkVsZW1lbnRCeU5hbWUpXG4gIGxheW91dFJlbW92ZUFjdGlvbih7IGdldFN0YXRlLCBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxMYXlvdXQuU3RhdGU+LCB7IG5hbWUgfTogUmVtb3ZlTmF2aWdhdGlvbkVsZW1lbnRCeU5hbWUpIHtcbiAgICBsZXQgeyBuYXZpZ2F0aW9uRWxlbWVudHMgfSA9IGdldFN0YXRlKCk7XG5cbiAgICBjb25zdCBpbmRleCA9IG5hdmlnYXRpb25FbGVtZW50cy5maW5kSW5kZXgoZWxlbWVudCA9PiBlbGVtZW50Lm5hbWUgPT09IG5hbWUpO1xuXG4gICAgaWYgKGluZGV4ID4gLTEpIHtcbiAgICAgIG5hdmlnYXRpb25FbGVtZW50cyA9IG5hdmlnYXRpb25FbGVtZW50cy5zcGxpY2UoaW5kZXgsIDEpO1xuICAgIH1cblxuICAgIHJldHVybiBwYXRjaFN0YXRlKHtcbiAgICAgIG5hdmlnYXRpb25FbGVtZW50cyxcbiAgICB9KTtcbiAgfVxufVxuIl19