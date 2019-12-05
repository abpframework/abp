/**
 * @fileoverview added by tsickle
 * Generated from: lib/utils/route-utils.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
/**
 * @param {?} routes
 * @param {?=} wrappers
 * @param {?=} parentNameArr
 * @param {?=} parentName
 * @return {?}
 */
export function organizeRoutes(routes, wrappers, parentNameArr, parentName) {
    if (wrappers === void 0) { wrappers = []; }
    if (parentNameArr === void 0) { parentNameArr = (/** @type {?} */ ([])); }
    if (parentName === void 0) { parentName = null; }
    /** @type {?} */
    var filter = (/**
     * @param {?} route
     * @return {?}
     */
    function (route) {
        if (route.children && route.children.length) {
            route.children = organizeRoutes(route.children, wrappers, parentNameArr, route.name);
        }
        if (route.parentName && route.parentName !== parentName) {
            parentNameArr.push(route);
            return false;
        }
        return true;
    });
    if (parentName) {
        // recursive block
        return routes.filter(filter);
    }
    /** @type {?} */
    var filteredRoutes = routes.filter(filter);
    if (parentNameArr.length) {
        return sortRoutes(setChildRoute(tslib_1.__spread(filteredRoutes, wrappers), parentNameArr));
    }
    return filteredRoutes;
}
/**
 * @param {?} routes
 * @param {?} parentNameArr
 * @return {?}
 */
export function setChildRoute(routes, parentNameArr) {
    return routes.map((/**
     * @param {?} route
     * @return {?}
     */
    function (route) {
        if (route.children && route.children.length) {
            route.children = setChildRoute(route.children, parentNameArr);
        }
        /** @type {?} */
        var foundedChildren = parentNameArr.filter((/**
         * @param {?} parent
         * @return {?}
         */
        function (parent) { return parent.parentName === route.name; }));
        if (foundedChildren && foundedChildren.length) {
            route.children = tslib_1.__spread((route.children || []), foundedChildren);
        }
        return route;
    }));
}
/**
 * @param {?=} routes
 * @return {?}
 */
export function sortRoutes(routes) {
    if (routes === void 0) { routes = []; }
    if (!routes.length)
        return [];
    return routes
        .map((/**
     * @param {?} route
     * @param {?} index
     * @return {?}
     */
    function (route, index) {
        return tslib_1.__assign({}, route, { order: typeof route.order === 'undefined' ? index + 1 : route.order });
    }))
        .sort((/**
     * @param {?} a
     * @param {?} b
     * @return {?}
     */
    function (a, b) { return a.order - b.order; }))
        .map((/**
     * @param {?} route
     * @return {?}
     */
    function (route) {
        if (route.children && route.children.length) {
            route.children = sortRoutes(route.children);
        }
        return route;
    }));
}
/** @type {?} */
var ABP_ROUTES = (/** @type {?} */ ([]));
/**
 * @param {?} routes
 * @return {?}
 */
export function addAbpRoutes(routes) {
    if (!Array.isArray(routes)) {
        routes = [routes];
    }
    ABP_ROUTES.push.apply(ABP_ROUTES, tslib_1.__spread(routes));
}
/**
 * @return {?}
 */
export function getAbpRoutes() {
    return ABP_ROUTES;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGUtdXRpbHMuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvdXRpbHMvcm91dGUtdXRpbHMudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7OztBQUVBLE1BQU0sVUFBVSxjQUFjLENBQzVCLE1BQXVCLEVBQ3ZCLFFBQThCLEVBQzlCLGFBQXFDLEVBQ3JDLFVBQXlCO0lBRnpCLHlCQUFBLEVBQUEsYUFBOEI7SUFDOUIsOEJBQUEsRUFBQSxtQ0FBZ0IsRUFBRSxFQUFtQjtJQUNyQywyQkFBQSxFQUFBLGlCQUF5Qjs7UUFFbkIsTUFBTTs7OztJQUFHLFVBQUEsS0FBSztRQUNsQixJQUFJLEtBQUssQ0FBQyxRQUFRLElBQUksS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDM0MsS0FBSyxDQUFDLFFBQVEsR0FBRyxjQUFjLENBQUMsS0FBSyxDQUFDLFFBQVEsRUFBRSxRQUFRLEVBQUUsYUFBYSxFQUFFLEtBQUssQ0FBQyxJQUFJLENBQUMsQ0FBQztTQUN0RjtRQUVELElBQUksS0FBSyxDQUFDLFVBQVUsSUFBSSxLQUFLLENBQUMsVUFBVSxLQUFLLFVBQVUsRUFBRTtZQUN2RCxhQUFhLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO1lBQzFCLE9BQU8sS0FBSyxDQUFDO1NBQ2Q7UUFFRCxPQUFPLElBQUksQ0FBQztJQUNkLENBQUMsQ0FBQTtJQUVELElBQUksVUFBVSxFQUFFO1FBQ2Qsa0JBQWtCO1FBQ2xCLE9BQU8sTUFBTSxDQUFDLE1BQU0sQ0FBQyxNQUFNLENBQUMsQ0FBQztLQUM5Qjs7UUFFSyxjQUFjLEdBQUcsTUFBTSxDQUFDLE1BQU0sQ0FBQyxNQUFNLENBQUM7SUFFNUMsSUFBSSxhQUFhLENBQUMsTUFBTSxFQUFFO1FBQ3hCLE9BQU8sVUFBVSxDQUFDLGFBQWEsa0JBQUssY0FBYyxFQUFLLFFBQVEsR0FBRyxhQUFhLENBQUMsQ0FBQyxDQUFDO0tBQ25GO0lBRUQsT0FBTyxjQUFjLENBQUM7QUFDeEIsQ0FBQzs7Ozs7O0FBRUQsTUFBTSxVQUFVLGFBQWEsQ0FBQyxNQUF1QixFQUFFLGFBQThCO0lBQ25GLE9BQU8sTUFBTSxDQUFDLEdBQUc7Ozs7SUFBQyxVQUFBLEtBQUs7UUFDckIsSUFBSSxLQUFLLENBQUMsUUFBUSxJQUFJLEtBQUssQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO1lBQzNDLEtBQUssQ0FBQyxRQUFRLEdBQUcsYUFBYSxDQUFDLEtBQUssQ0FBQyxRQUFRLEVBQUUsYUFBYSxDQUFDLENBQUM7U0FDL0Q7O1lBRUssZUFBZSxHQUFHLGFBQWEsQ0FBQyxNQUFNOzs7O1FBQUMsVUFBQSxNQUFNLElBQUksT0FBQSxNQUFNLENBQUMsVUFBVSxLQUFLLEtBQUssQ0FBQyxJQUFJLEVBQWhDLENBQWdDLEVBQUM7UUFDeEYsSUFBSSxlQUFlLElBQUksZUFBZSxDQUFDLE1BQU0sRUFBRTtZQUM3QyxLQUFLLENBQUMsUUFBUSxvQkFBTyxDQUFDLEtBQUssQ0FBQyxRQUFRLElBQUksRUFBRSxDQUFDLEVBQUssZUFBZSxDQUFDLENBQUM7U0FDbEU7UUFFRCxPQUFPLEtBQUssQ0FBQztJQUNmLENBQUMsRUFBQyxDQUFDO0FBQ0wsQ0FBQzs7Ozs7QUFFRCxNQUFNLFVBQVUsVUFBVSxDQUFDLE1BQTRCO0lBQTVCLHVCQUFBLEVBQUEsV0FBNEI7SUFDckQsSUFBSSxDQUFDLE1BQU0sQ0FBQyxNQUFNO1FBQUUsT0FBTyxFQUFFLENBQUM7SUFDOUIsT0FBTyxNQUFNO1NBQ1YsR0FBRzs7Ozs7SUFBQyxVQUFDLEtBQUssRUFBRSxLQUFLO1FBQ2hCLDRCQUNLLEtBQUssSUFDUixLQUFLLEVBQUUsT0FBTyxLQUFLLENBQUMsS0FBSyxLQUFLLFdBQVcsQ0FBQyxDQUFDLENBQUMsS0FBSyxHQUFHLENBQUMsQ0FBQyxDQUFDLENBQUMsS0FBSyxDQUFDLEtBQUssSUFDbkU7SUFDSixDQUFDLEVBQUM7U0FDRCxJQUFJOzs7OztJQUFDLFVBQUMsQ0FBQyxFQUFFLENBQUMsSUFBSyxPQUFBLENBQUMsQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDLEtBQUssRUFBakIsQ0FBaUIsRUFBQztTQUNqQyxHQUFHOzs7O0lBQUMsVUFBQSxLQUFLO1FBQ1IsSUFBSSxLQUFLLENBQUMsUUFBUSxJQUFJLEtBQUssQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO1lBQzNDLEtBQUssQ0FBQyxRQUFRLEdBQUcsVUFBVSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsQ0FBQztTQUM3QztRQUVELE9BQU8sS0FBSyxDQUFDO0lBQ2YsQ0FBQyxFQUFDLENBQUM7QUFDUCxDQUFDOztJQUVLLFVBQVUsR0FBRyxtQkFBQSxFQUFFLEVBQW1COzs7OztBQUV4QyxNQUFNLFVBQVUsWUFBWSxDQUFDLE1BQXVDO0lBQ2xFLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLE1BQU0sQ0FBQyxFQUFFO1FBQzFCLE1BQU0sR0FBRyxDQUFDLE1BQU0sQ0FBQyxDQUFDO0tBQ25CO0lBRUQsVUFBVSxDQUFDLElBQUksT0FBZixVQUFVLG1CQUFTLE1BQU0sR0FBRTtBQUM3QixDQUFDOzs7O0FBRUQsTUFBTSxVQUFVLFlBQVk7SUFDMUIsT0FBTyxVQUFVLENBQUM7QUFDcEIsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCB9IGZyb20gJy4uL21vZGVscyc7XG5cbmV4cG9ydCBmdW5jdGlvbiBvcmdhbml6ZVJvdXRlcyhcbiAgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10sXG4gIHdyYXBwZXJzOiBBQlAuRnVsbFJvdXRlW10gPSBbXSxcbiAgcGFyZW50TmFtZUFyciA9IFtdIGFzIEFCUC5GdWxsUm91dGVbXSxcbiAgcGFyZW50TmFtZTogc3RyaW5nID0gbnVsbCxcbik6IEFCUC5GdWxsUm91dGVbXSB7XG4gIGNvbnN0IGZpbHRlciA9IHJvdXRlID0+IHtcbiAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XG4gICAgICByb3V0ZS5jaGlsZHJlbiA9IG9yZ2FuaXplUm91dGVzKHJvdXRlLmNoaWxkcmVuLCB3cmFwcGVycywgcGFyZW50TmFtZUFyciwgcm91dGUubmFtZSk7XG4gICAgfVxuXG4gICAgaWYgKHJvdXRlLnBhcmVudE5hbWUgJiYgcm91dGUucGFyZW50TmFtZSAhPT0gcGFyZW50TmFtZSkge1xuICAgICAgcGFyZW50TmFtZUFyci5wdXNoKHJvdXRlKTtcbiAgICAgIHJldHVybiBmYWxzZTtcbiAgICB9XG5cbiAgICByZXR1cm4gdHJ1ZTtcbiAgfTtcblxuICBpZiAocGFyZW50TmFtZSkge1xuICAgIC8vIHJlY3Vyc2l2ZSBibG9ja1xuICAgIHJldHVybiByb3V0ZXMuZmlsdGVyKGZpbHRlcik7XG4gIH1cblxuICBjb25zdCBmaWx0ZXJlZFJvdXRlcyA9IHJvdXRlcy5maWx0ZXIoZmlsdGVyKTtcblxuICBpZiAocGFyZW50TmFtZUFyci5sZW5ndGgpIHtcbiAgICByZXR1cm4gc29ydFJvdXRlcyhzZXRDaGlsZFJvdXRlKFsuLi5maWx0ZXJlZFJvdXRlcywgLi4ud3JhcHBlcnNdLCBwYXJlbnROYW1lQXJyKSk7XG4gIH1cblxuICByZXR1cm4gZmlsdGVyZWRSb3V0ZXM7XG59XG5cbmV4cG9ydCBmdW5jdGlvbiBzZXRDaGlsZFJvdXRlKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdLCBwYXJlbnROYW1lQXJyOiBBQlAuRnVsbFJvdXRlW10pOiBBQlAuRnVsbFJvdXRlW10ge1xuICByZXR1cm4gcm91dGVzLm1hcChyb3V0ZSA9PiB7XG4gICAgaWYgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgcm91dGUuY2hpbGRyZW4gPSBzZXRDaGlsZFJvdXRlKHJvdXRlLmNoaWxkcmVuLCBwYXJlbnROYW1lQXJyKTtcbiAgICB9XG5cbiAgICBjb25zdCBmb3VuZGVkQ2hpbGRyZW4gPSBwYXJlbnROYW1lQXJyLmZpbHRlcihwYXJlbnQgPT4gcGFyZW50LnBhcmVudE5hbWUgPT09IHJvdXRlLm5hbWUpO1xuICAgIGlmIChmb3VuZGVkQ2hpbGRyZW4gJiYgZm91bmRlZENoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgcm91dGUuY2hpbGRyZW4gPSBbLi4uKHJvdXRlLmNoaWxkcmVuIHx8IFtdKSwgLi4uZm91bmRlZENoaWxkcmVuXTtcbiAgICB9XG5cbiAgICByZXR1cm4gcm91dGU7XG4gIH0pO1xufVxuXG5leHBvcnQgZnVuY3Rpb24gc29ydFJvdXRlcyhyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSA9IFtdKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgaWYgKCFyb3V0ZXMubGVuZ3RoKSByZXR1cm4gW107XG4gIHJldHVybiByb3V0ZXNcbiAgICAubWFwKChyb3V0ZSwgaW5kZXgpID0+IHtcbiAgICAgIHJldHVybiB7XG4gICAgICAgIC4uLnJvdXRlLFxuICAgICAgICBvcmRlcjogdHlwZW9mIHJvdXRlLm9yZGVyID09PSAndW5kZWZpbmVkJyA/IGluZGV4ICsgMSA6IHJvdXRlLm9yZGVyLFxuICAgICAgfTtcbiAgICB9KVxuICAgIC5zb3J0KChhLCBiKSA9PiBhLm9yZGVyIC0gYi5vcmRlcilcbiAgICAubWFwKHJvdXRlID0+IHtcbiAgICAgIGlmIChyb3V0ZS5jaGlsZHJlbiAmJiByb3V0ZS5jaGlsZHJlbi5sZW5ndGgpIHtcbiAgICAgICAgcm91dGUuY2hpbGRyZW4gPSBzb3J0Um91dGVzKHJvdXRlLmNoaWxkcmVuKTtcbiAgICAgIH1cblxuICAgICAgcmV0dXJuIHJvdXRlO1xuICAgIH0pO1xufVxuXG5jb25zdCBBQlBfUk9VVEVTID0gW10gYXMgQUJQLkZ1bGxSb3V0ZVtdO1xuXG5leHBvcnQgZnVuY3Rpb24gYWRkQWJwUm91dGVzKHJvdXRlczogQUJQLkZ1bGxSb3V0ZSB8IEFCUC5GdWxsUm91dGVbXSk6IHZvaWQge1xuICBpZiAoIUFycmF5LmlzQXJyYXkocm91dGVzKSkge1xuICAgIHJvdXRlcyA9IFtyb3V0ZXNdO1xuICB9XG5cbiAgQUJQX1JPVVRFUy5wdXNoKC4uLnJvdXRlcyk7XG59XG5cbmV4cG9ydCBmdW5jdGlvbiBnZXRBYnBSb3V0ZXMoKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgcmV0dXJuIEFCUF9ST1VURVM7XG59XG4iXX0=