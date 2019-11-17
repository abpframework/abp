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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvbGF5b3V0LnN0YXRlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7OztBQUFBLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFnQixRQUFRLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLG9CQUFvQixFQUFFLDZCQUE2QixFQUFFLE1BQU0sMkJBQTJCLENBQUM7QUFHaEcsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0lBTVQsV0FBVyxTQUFYLFdBQVc7Ozs7O0lBRXRCLE1BQU0sQ0FBQyxxQkFBcUIsQ0FBQyxFQUFFLGtCQUFrQixFQUFnQjtRQUMvRCxPQUFPLGtCQUFrQixDQUFDO0lBQzVCLENBQUM7Ozs7OztJQUdELGVBQWUsQ0FBQyxFQUFFLFFBQVEsRUFBRSxVQUFVLEVBQThCLEVBQUUsRUFBRSxPQUFPLEdBQUcsRUFBRSxFQUF3QjtZQUN0RyxFQUFFLGtCQUFrQixFQUFFLEdBQUcsUUFBUSxFQUFFO1FBRXZDLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLE9BQU8sQ0FBQyxFQUFFO1lBQzNCLE9BQU8sR0FBRyxDQUFDLE9BQU8sQ0FBQyxDQUFDO1NBQ3JCO1FBRUQsSUFBSSxrQkFBa0IsQ0FBQyxNQUFNLEVBQUU7WUFDN0IsT0FBTyxHQUFHLEdBQUc7OztZQUNYLEdBQUcsRUFBRSxDQUNILENBQUMsbUJBQUEsT0FBTyxFQUE4QixDQUFDLENBQUMsTUFBTTs7OztZQUM1QyxDQUFDLEVBQUUsSUFBSSxFQUFFLEVBQUUsRUFBRSxDQUFDLGtCQUFrQixDQUFDLFNBQVM7Ozs7WUFBQyxHQUFHLENBQUMsRUFBRSxDQUFDLEdBQUcsQ0FBQyxJQUFJLEtBQUssSUFBSSxFQUFDLEdBQUcsQ0FBQyxFQUN6RSxHQUNILEVBQUUsQ0FDSCxDQUFDO1NBQ0g7UUFFRCxJQUFJLENBQUMsT0FBTyxDQUFDLE1BQU07WUFBRSxPQUFPO1FBRTVCLGtCQUFrQixHQUFHLENBQUMsR0FBRyxrQkFBa0IsRUFBRSxHQUFHLE9BQU8sQ0FBQzthQUNyRCxHQUFHOzs7O1FBQUMsT0FBTyxDQUFDLEVBQUUsQ0FBQyxtQkFBTSxPQUFPLElBQUUsS0FBSyxFQUFFLE9BQU8sQ0FBQyxLQUFLLElBQUksRUFBRSxJQUFHLEVBQUM7YUFDNUQsSUFBSTs7Ozs7UUFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLEVBQUUsRUFBRSxDQUFDLENBQUMsQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDLEtBQUssRUFBQyxDQUFDO1FBRXJDLE9BQU8sVUFBVSxDQUFDO1lBQ2hCLGtCQUFrQjtTQUNuQixDQUFDLENBQUM7SUFDTCxDQUFDOzs7Ozs7SUFHRCxrQkFBa0IsQ0FBQyxFQUFFLFFBQVEsRUFBRSxVQUFVLEVBQThCLEVBQUUsRUFBRSxJQUFJLEVBQWlDO1lBQzFHLEVBQUUsa0JBQWtCLEVBQUUsR0FBRyxRQUFRLEVBQUU7O2NBRWpDLEtBQUssR0FBRyxrQkFBa0IsQ0FBQyxTQUFTOzs7O1FBQUMsT0FBTyxDQUFDLEVBQUUsQ0FBQyxPQUFPLENBQUMsSUFBSSxLQUFLLElBQUksRUFBQztRQUU1RSxJQUFJLEtBQUssR0FBRyxDQUFDLENBQUMsRUFBRTtZQUNkLGtCQUFrQixHQUFHLGtCQUFrQixDQUFDLE1BQU0sQ0FBQyxLQUFLLEVBQUUsQ0FBQyxDQUFDLENBQUM7U0FDMUQ7UUFFRCxPQUFPLFVBQVUsQ0FBQztZQUNoQixrQkFBa0I7U0FDbkIsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztDQUNGLENBQUE7QUExQ0M7SUFEQyxNQUFNLENBQUMsb0JBQW9CLENBQUM7O3FEQUMyRCxvQkFBb0I7O2tEQTBCM0c7QUFHRDtJQURDLE1BQU0sQ0FBQyw2QkFBNkIsQ0FBQzs7cURBQzZDLDZCQUE2Qjs7cURBWS9HO0FBOUNEO0lBREMsUUFBUSxFQUFFOzs7OzhDQUdWO0FBSlUsV0FBVztJQUp2QixLQUFLLENBQWU7UUFDbkIsSUFBSSxFQUFFLGFBQWE7UUFDbkIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsa0JBQWtCLEVBQUUsRUFBRSxFQUFFLEVBQWdCO0tBQ3JELENBQUM7R0FDVyxXQUFXLENBaUR2QjtTQWpEWSxXQUFXIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhdGUsIEFjdGlvbiwgU3RhdGVDb250ZXh0LCBTZWxlY3RvciB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgQWRkTmF2aWdhdGlvbkVsZW1lbnQsIFJlbW92ZU5hdmlnYXRpb25FbGVtZW50QnlOYW1lIH0gZnJvbSAnLi4vYWN0aW9ucy9sYXlvdXQuYWN0aW9ucyc7XHJcbmltcG9ydCB7IExheW91dCB9IGZyb20gJy4uL21vZGVscy9sYXlvdXQnO1xyXG5pbXBvcnQgeyBUZW1wbGF0ZVJlZiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XHJcblxyXG5AU3RhdGU8TGF5b3V0LlN0YXRlPih7XHJcbiAgbmFtZTogJ0xheW91dFN0YXRlJyxcclxuICBkZWZhdWx0czogeyBuYXZpZ2F0aW9uRWxlbWVudHM6IFtdIH0gYXMgTGF5b3V0LlN0YXRlLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgTGF5b3V0U3RhdGUge1xyXG4gIEBTZWxlY3RvcigpXHJcbiAgc3RhdGljIGdldE5hdmlnYXRpb25FbGVtZW50cyh7IG5hdmlnYXRpb25FbGVtZW50cyB9OiBMYXlvdXQuU3RhdGUpOiBMYXlvdXQuTmF2aWdhdGlvbkVsZW1lbnRbXSB7XHJcbiAgICByZXR1cm4gbmF2aWdhdGlvbkVsZW1lbnRzO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihBZGROYXZpZ2F0aW9uRWxlbWVudClcclxuICBsYXlvdXRBZGRBY3Rpb24oeyBnZXRTdGF0ZSwgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8TGF5b3V0LlN0YXRlPiwgeyBwYXlsb2FkID0gW10gfTogQWRkTmF2aWdhdGlvbkVsZW1lbnQpIHtcclxuICAgIGxldCB7IG5hdmlnYXRpb25FbGVtZW50cyB9ID0gZ2V0U3RhdGUoKTtcclxuXHJcbiAgICBpZiAoIUFycmF5LmlzQXJyYXkocGF5bG9hZCkpIHtcclxuICAgICAgcGF5bG9hZCA9IFtwYXlsb2FkXTtcclxuICAgIH1cclxuXHJcbiAgICBpZiAobmF2aWdhdGlvbkVsZW1lbnRzLmxlbmd0aCkge1xyXG4gICAgICBwYXlsb2FkID0gc25xKFxyXG4gICAgICAgICgpID0+XHJcbiAgICAgICAgICAocGF5bG9hZCBhcyBMYXlvdXQuTmF2aWdhdGlvbkVsZW1lbnRbXSkuZmlsdGVyKFxyXG4gICAgICAgICAgICAoeyBuYW1lIH0pID0+IG5hdmlnYXRpb25FbGVtZW50cy5maW5kSW5kZXgobmF2ID0+IG5hdi5uYW1lID09PSBuYW1lKSA8IDAsXHJcbiAgICAgICAgICApLFxyXG4gICAgICAgIFtdLFxyXG4gICAgICApO1xyXG4gICAgfVxyXG5cclxuICAgIGlmICghcGF5bG9hZC5sZW5ndGgpIHJldHVybjtcclxuXHJcbiAgICBuYXZpZ2F0aW9uRWxlbWVudHMgPSBbLi4ubmF2aWdhdGlvbkVsZW1lbnRzLCAuLi5wYXlsb2FkXVxyXG4gICAgICAubWFwKGVsZW1lbnQgPT4gKHsgLi4uZWxlbWVudCwgb3JkZXI6IGVsZW1lbnQub3JkZXIgfHwgOTkgfSkpXHJcbiAgICAgIC5zb3J0KChhLCBiKSA9PiBhLm9yZGVyIC0gYi5vcmRlcik7XHJcblxyXG4gICAgcmV0dXJuIHBhdGNoU3RhdGUoe1xyXG4gICAgICBuYXZpZ2F0aW9uRWxlbWVudHMsXHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oUmVtb3ZlTmF2aWdhdGlvbkVsZW1lbnRCeU5hbWUpXHJcbiAgbGF5b3V0UmVtb3ZlQWN0aW9uKHsgZ2V0U3RhdGUsIHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PExheW91dC5TdGF0ZT4sIHsgbmFtZSB9OiBSZW1vdmVOYXZpZ2F0aW9uRWxlbWVudEJ5TmFtZSkge1xyXG4gICAgbGV0IHsgbmF2aWdhdGlvbkVsZW1lbnRzIH0gPSBnZXRTdGF0ZSgpO1xyXG5cclxuICAgIGNvbnN0IGluZGV4ID0gbmF2aWdhdGlvbkVsZW1lbnRzLmZpbmRJbmRleChlbGVtZW50ID0+IGVsZW1lbnQubmFtZSA9PT0gbmFtZSk7XHJcblxyXG4gICAgaWYgKGluZGV4ID4gLTEpIHtcclxuICAgICAgbmF2aWdhdGlvbkVsZW1lbnRzID0gbmF2aWdhdGlvbkVsZW1lbnRzLnNwbGljZShpbmRleCwgMSk7XHJcbiAgICB9XHJcblxyXG4gICAgcmV0dXJuIHBhdGNoU3RhdGUoe1xyXG4gICAgICBuYXZpZ2F0aW9uRWxlbWVudHMsXHJcbiAgICB9KTtcclxuICB9XHJcbn1cclxuIl19