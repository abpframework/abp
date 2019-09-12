/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5iYXNpYy8iLCJzb3VyY2VzIjpbImxpYi9zdGF0ZXMvbGF5b3V0LnN0YXRlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQWdCLFFBQVEsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwRSxPQUFPLEVBQUUsb0JBQW9CLEVBQUUsNkJBQTZCLEVBQUUsTUFBTSwyQkFBMkIsQ0FBQztBQUdoRyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7SUFNVCxXQUFXLFNBQVgsV0FBVzs7Ozs7SUFFdEIsTUFBTSxDQUFDLHFCQUFxQixDQUFDLEVBQUUsa0JBQWtCLEVBQWdCO1FBQy9ELE9BQU8sa0JBQWtCLENBQUM7SUFDNUIsQ0FBQzs7Ozs7O0lBR0QsZUFBZSxDQUFDLEVBQUUsUUFBUSxFQUFFLFVBQVUsRUFBOEIsRUFBRSxFQUFFLE9BQU8sR0FBRyxFQUFFLEVBQXdCO1lBQ3RHLEVBQUUsa0JBQWtCLEVBQUUsR0FBRyxRQUFRLEVBQUU7UUFFdkMsSUFBSSxDQUFDLEtBQUssQ0FBQyxPQUFPLENBQUMsT0FBTyxDQUFDLEVBQUU7WUFDM0IsT0FBTyxHQUFHLENBQUMsT0FBTyxDQUFDLENBQUM7U0FDckI7UUFFRCxJQUFJLGtCQUFrQixDQUFDLE1BQU0sRUFBRTtZQUM3QixPQUFPLEdBQUcsR0FBRzs7O1lBQ1gsR0FBRyxFQUFFLENBQ0gsQ0FBQyxtQkFBQSxPQUFPLEVBQThCLENBQUMsQ0FBQyxNQUFNOzs7O1lBQzVDLENBQUMsRUFBRSxJQUFJLEVBQUUsRUFBRSxFQUFFLENBQUMsa0JBQWtCLENBQUMsU0FBUzs7OztZQUFDLEdBQUcsQ0FBQyxFQUFFLENBQUMsR0FBRyxDQUFDLElBQUksS0FBSyxJQUFJLEVBQUMsR0FBRyxDQUFDLEVBQ3pFLEdBQ0gsRUFBRSxDQUNILENBQUM7U0FDSDtRQUVELElBQUksQ0FBQyxPQUFPLENBQUMsTUFBTTtZQUFFLE9BQU87UUFFNUIsa0JBQWtCLEdBQUcsQ0FBQyxHQUFHLGtCQUFrQixFQUFFLEdBQUcsT0FBTyxDQUFDO2FBQ3JELEdBQUc7Ozs7UUFBQyxPQUFPLENBQUMsRUFBRSxDQUFDLG1CQUFNLE9BQU8sSUFBRSxLQUFLLEVBQUUsT0FBTyxDQUFDLEtBQUssSUFBSSxFQUFFLElBQUcsRUFBQzthQUM1RCxJQUFJOzs7OztRQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsRUFBRSxFQUFFLENBQUMsQ0FBQyxDQUFDLEtBQUssR0FBRyxDQUFDLENBQUMsS0FBSyxFQUFDLENBQUM7UUFFckMsT0FBTyxVQUFVLENBQUM7WUFDaEIsa0JBQWtCO1NBQ25CLENBQUMsQ0FBQztJQUNMLENBQUM7Ozs7OztJQUdELGtCQUFrQixDQUFDLEVBQUUsUUFBUSxFQUFFLFVBQVUsRUFBOEIsRUFBRSxFQUFFLElBQUksRUFBaUM7WUFDMUcsRUFBRSxrQkFBa0IsRUFBRSxHQUFHLFFBQVEsRUFBRTs7Y0FFakMsS0FBSyxHQUFHLGtCQUFrQixDQUFDLFNBQVM7Ozs7UUFBQyxPQUFPLENBQUMsRUFBRSxDQUFDLE9BQU8sQ0FBQyxJQUFJLEtBQUssSUFBSSxFQUFDO1FBRTVFLElBQUksS0FBSyxHQUFHLENBQUMsQ0FBQyxFQUFFO1lBQ2Qsa0JBQWtCLEdBQUcsa0JBQWtCLENBQUMsTUFBTSxDQUFDLEtBQUssRUFBRSxDQUFDLENBQUMsQ0FBQztTQUMxRDtRQUVELE9BQU8sVUFBVSxDQUFDO1lBQ2hCLGtCQUFrQjtTQUNuQixDQUFDLENBQUM7SUFDTCxDQUFDO0NBQ0YsQ0FBQTtBQTFDQztJQURDLE1BQU0sQ0FBQyxvQkFBb0IsQ0FBQzs7cURBQzJELG9CQUFvQjs7a0RBMEIzRztBQUdEO0lBREMsTUFBTSxDQUFDLDZCQUE2QixDQUFDOztxREFDNkMsNkJBQTZCOztxREFZL0c7QUE5Q0Q7SUFEQyxRQUFRLEVBQUU7Ozs7OENBR1Y7QUFKVSxXQUFXO0lBSnZCLEtBQUssQ0FBZTtRQUNuQixJQUFJLEVBQUUsYUFBYTtRQUNuQixRQUFRLEVBQUUsbUJBQUEsRUFBRSxrQkFBa0IsRUFBRSxFQUFFLEVBQUUsRUFBZ0I7S0FDckQsQ0FBQztHQUNXLFdBQVcsQ0FpRHZCO1NBakRZLFdBQVciLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBTdGF0ZSwgQWN0aW9uLCBTdGF0ZUNvbnRleHQsIFNlbGVjdG9yIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgQWRkTmF2aWdhdGlvbkVsZW1lbnQsIFJlbW92ZU5hdmlnYXRpb25FbGVtZW50QnlOYW1lIH0gZnJvbSAnLi4vYWN0aW9ucy9sYXlvdXQuYWN0aW9ucyc7XG5pbXBvcnQgeyBMYXlvdXQgfSBmcm9tICcuLi9tb2RlbHMvbGF5b3V0JztcbmltcG9ydCB7IFRlbXBsYXRlUmVmIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5cbkBTdGF0ZTxMYXlvdXQuU3RhdGU+KHtcbiAgbmFtZTogJ0xheW91dFN0YXRlJyxcbiAgZGVmYXVsdHM6IHsgbmF2aWdhdGlvbkVsZW1lbnRzOiBbXSB9IGFzIExheW91dC5TdGF0ZSxcbn0pXG5leHBvcnQgY2xhc3MgTGF5b3V0U3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0TmF2aWdhdGlvbkVsZW1lbnRzKHsgbmF2aWdhdGlvbkVsZW1lbnRzIH06IExheW91dC5TdGF0ZSk6IExheW91dC5OYXZpZ2F0aW9uRWxlbWVudFtdIHtcbiAgICByZXR1cm4gbmF2aWdhdGlvbkVsZW1lbnRzO1xuICB9XG5cbiAgQEFjdGlvbihBZGROYXZpZ2F0aW9uRWxlbWVudClcbiAgbGF5b3V0QWRkQWN0aW9uKHsgZ2V0U3RhdGUsIHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PExheW91dC5TdGF0ZT4sIHsgcGF5bG9hZCA9IFtdIH06IEFkZE5hdmlnYXRpb25FbGVtZW50KSB7XG4gICAgbGV0IHsgbmF2aWdhdGlvbkVsZW1lbnRzIH0gPSBnZXRTdGF0ZSgpO1xuXG4gICAgaWYgKCFBcnJheS5pc0FycmF5KHBheWxvYWQpKSB7XG4gICAgICBwYXlsb2FkID0gW3BheWxvYWRdO1xuICAgIH1cblxuICAgIGlmIChuYXZpZ2F0aW9uRWxlbWVudHMubGVuZ3RoKSB7XG4gICAgICBwYXlsb2FkID0gc25xKFxuICAgICAgICAoKSA9PlxuICAgICAgICAgIChwYXlsb2FkIGFzIExheW91dC5OYXZpZ2F0aW9uRWxlbWVudFtdKS5maWx0ZXIoXG4gICAgICAgICAgICAoeyBuYW1lIH0pID0+IG5hdmlnYXRpb25FbGVtZW50cy5maW5kSW5kZXgobmF2ID0+IG5hdi5uYW1lID09PSBuYW1lKSA8IDAsXG4gICAgICAgICAgKSxcbiAgICAgICAgW10sXG4gICAgICApO1xuICAgIH1cblxuICAgIGlmICghcGF5bG9hZC5sZW5ndGgpIHJldHVybjtcblxuICAgIG5hdmlnYXRpb25FbGVtZW50cyA9IFsuLi5uYXZpZ2F0aW9uRWxlbWVudHMsIC4uLnBheWxvYWRdXG4gICAgICAubWFwKGVsZW1lbnQgPT4gKHsgLi4uZWxlbWVudCwgb3JkZXI6IGVsZW1lbnQub3JkZXIgfHwgOTkgfSkpXG4gICAgICAuc29ydCgoYSwgYikgPT4gYS5vcmRlciAtIGIub3JkZXIpO1xuXG4gICAgcmV0dXJuIHBhdGNoU3RhdGUoe1xuICAgICAgbmF2aWdhdGlvbkVsZW1lbnRzLFxuICAgIH0pO1xuICB9XG5cbiAgQEFjdGlvbihSZW1vdmVOYXZpZ2F0aW9uRWxlbWVudEJ5TmFtZSlcbiAgbGF5b3V0UmVtb3ZlQWN0aW9uKHsgZ2V0U3RhdGUsIHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PExheW91dC5TdGF0ZT4sIHsgbmFtZSB9OiBSZW1vdmVOYXZpZ2F0aW9uRWxlbWVudEJ5TmFtZSkge1xuICAgIGxldCB7IG5hdmlnYXRpb25FbGVtZW50cyB9ID0gZ2V0U3RhdGUoKTtcblxuICAgIGNvbnN0IGluZGV4ID0gbmF2aWdhdGlvbkVsZW1lbnRzLmZpbmRJbmRleChlbGVtZW50ID0+IGVsZW1lbnQubmFtZSA9PT0gbmFtZSk7XG5cbiAgICBpZiAoaW5kZXggPiAtMSkge1xuICAgICAgbmF2aWdhdGlvbkVsZW1lbnRzID0gbmF2aWdhdGlvbkVsZW1lbnRzLnNwbGljZShpbmRleCwgMSk7XG4gICAgfVxuXG4gICAgcmV0dXJuIHBhdGNoU3RhdGUoe1xuICAgICAgbmF2aWdhdGlvbkVsZW1lbnRzLFxuICAgIH0pO1xuICB9XG59XG4iXX0=