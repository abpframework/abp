/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGUtdXRpbHMuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvdXRpbHMvcm91dGUtdXRpbHMudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7O0FBRUEsTUFBTSxVQUFVLGNBQWMsQ0FDNUIsTUFBdUIsRUFDdkIsUUFBOEIsRUFDOUIsYUFBcUMsRUFDckMsVUFBeUI7SUFGekIseUJBQUEsRUFBQSxhQUE4QjtJQUM5Qiw4QkFBQSxFQUFBLG1DQUFnQixFQUFFLEVBQW1CO0lBQ3JDLDJCQUFBLEVBQUEsaUJBQXlCOztRQUVuQixNQUFNOzs7O0lBQUcsVUFBQSxLQUFLO1FBQ2xCLElBQUksS0FBSyxDQUFDLFFBQVEsSUFBSSxLQUFLLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUMzQyxLQUFLLENBQUMsUUFBUSxHQUFHLGNBQWMsQ0FBQyxLQUFLLENBQUMsUUFBUSxFQUFFLFFBQVEsRUFBRSxhQUFhLEVBQUUsS0FBSyxDQUFDLElBQUksQ0FBQyxDQUFDO1NBQ3RGO1FBRUQsSUFBSSxLQUFLLENBQUMsVUFBVSxJQUFJLEtBQUssQ0FBQyxVQUFVLEtBQUssVUFBVSxFQUFFO1lBQ3ZELGFBQWEsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7WUFDMUIsT0FBTyxLQUFLLENBQUM7U0FDZDtRQUVELE9BQU8sSUFBSSxDQUFDO0lBQ2QsQ0FBQyxDQUFBO0lBRUQsSUFBSSxVQUFVLEVBQUU7UUFDZCxrQkFBa0I7UUFDbEIsT0FBTyxNQUFNLENBQUMsTUFBTSxDQUFDLE1BQU0sQ0FBQyxDQUFDO0tBQzlCOztRQUVLLGNBQWMsR0FBRyxNQUFNLENBQUMsTUFBTSxDQUFDLE1BQU0sQ0FBQztJQUU1QyxJQUFJLGFBQWEsQ0FBQyxNQUFNLEVBQUU7UUFDeEIsT0FBTyxVQUFVLENBQUMsYUFBYSxrQkFBSyxjQUFjLEVBQUssUUFBUSxHQUFHLGFBQWEsQ0FBQyxDQUFDLENBQUM7S0FDbkY7SUFFRCxPQUFPLGNBQWMsQ0FBQztBQUN4QixDQUFDOzs7Ozs7QUFFRCxNQUFNLFVBQVUsYUFBYSxDQUFDLE1BQXVCLEVBQUUsYUFBOEI7SUFDbkYsT0FBTyxNQUFNLENBQUMsR0FBRzs7OztJQUFDLFVBQUEsS0FBSztRQUNyQixJQUFJLEtBQUssQ0FBQyxRQUFRLElBQUksS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDM0MsS0FBSyxDQUFDLFFBQVEsR0FBRyxhQUFhLENBQUMsS0FBSyxDQUFDLFFBQVEsRUFBRSxhQUFhLENBQUMsQ0FBQztTQUMvRDs7WUFFSyxlQUFlLEdBQUcsYUFBYSxDQUFDLE1BQU07Ozs7UUFBQyxVQUFBLE1BQU0sSUFBSSxPQUFBLE1BQU0sQ0FBQyxVQUFVLEtBQUssS0FBSyxDQUFDLElBQUksRUFBaEMsQ0FBZ0MsRUFBQztRQUN4RixJQUFJLGVBQWUsSUFBSSxlQUFlLENBQUMsTUFBTSxFQUFFO1lBQzdDLEtBQUssQ0FBQyxRQUFRLG9CQUFPLENBQUMsS0FBSyxDQUFDLFFBQVEsSUFBSSxFQUFFLENBQUMsRUFBSyxlQUFlLENBQUMsQ0FBQztTQUNsRTtRQUVELE9BQU8sS0FBSyxDQUFDO0lBQ2YsQ0FBQyxFQUFDLENBQUM7QUFDTCxDQUFDOzs7OztBQUVELE1BQU0sVUFBVSxVQUFVLENBQUMsTUFBNEI7SUFBNUIsdUJBQUEsRUFBQSxXQUE0QjtJQUNyRCxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU07UUFBRSxPQUFPLEVBQUUsQ0FBQztJQUM5QixPQUFPLE1BQU07U0FDVixHQUFHOzs7OztJQUFDLFVBQUMsS0FBSyxFQUFFLEtBQUs7UUFDaEIsNEJBQ0ssS0FBSyxJQUNSLEtBQUssRUFBRSxPQUFPLEtBQUssQ0FBQyxLQUFLLEtBQUssV0FBVyxDQUFDLENBQUMsQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxLQUFLLENBQUMsS0FBSyxJQUNuRTtJQUNKLENBQUMsRUFBQztTQUNELElBQUk7Ozs7O0lBQUMsVUFBQyxDQUFDLEVBQUUsQ0FBQyxJQUFLLE9BQUEsQ0FBQyxDQUFDLEtBQUssR0FBRyxDQUFDLENBQUMsS0FBSyxFQUFqQixDQUFpQixFQUFDO1NBQ2pDLEdBQUc7Ozs7SUFBQyxVQUFBLEtBQUs7UUFDUixJQUFJLEtBQUssQ0FBQyxRQUFRLElBQUksS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDM0MsS0FBSyxDQUFDLFFBQVEsR0FBRyxVQUFVLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxDQUFDO1NBQzdDO1FBRUQsT0FBTyxLQUFLLENBQUM7SUFDZixDQUFDLEVBQUMsQ0FBQztBQUNQLENBQUM7O0lBRUssVUFBVSxHQUFHLG1CQUFBLEVBQUUsRUFBbUI7Ozs7O0FBRXhDLE1BQU0sVUFBVSxZQUFZLENBQUMsTUFBdUM7SUFDbEUsSUFBSSxDQUFDLEtBQUssQ0FBQyxPQUFPLENBQUMsTUFBTSxDQUFDLEVBQUU7UUFDMUIsTUFBTSxHQUFHLENBQUMsTUFBTSxDQUFDLENBQUM7S0FDbkI7SUFFRCxVQUFVLENBQUMsSUFBSSxPQUFmLFVBQVUsbUJBQVMsTUFBTSxHQUFFO0FBQzdCLENBQUM7Ozs7QUFFRCxNQUFNLFVBQVUsWUFBWTtJQUMxQixPQUFPLFVBQVUsQ0FBQztBQUNwQixDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQUJQIH0gZnJvbSAnLi4vbW9kZWxzJztcblxuZXhwb3J0IGZ1bmN0aW9uIG9yZ2FuaXplUm91dGVzKFxuICByb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSxcbiAgd3JhcHBlcnM6IEFCUC5GdWxsUm91dGVbXSA9IFtdLFxuICBwYXJlbnROYW1lQXJyID0gW10gYXMgQUJQLkZ1bGxSb3V0ZVtdLFxuICBwYXJlbnROYW1lOiBzdHJpbmcgPSBudWxsLFxuKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgY29uc3QgZmlsdGVyID0gcm91dGUgPT4ge1xuICAgIGlmIChyb3V0ZS5jaGlsZHJlbiAmJiByb3V0ZS5jaGlsZHJlbi5sZW5ndGgpIHtcbiAgICAgIHJvdXRlLmNoaWxkcmVuID0gb3JnYW5pemVSb3V0ZXMocm91dGUuY2hpbGRyZW4sIHdyYXBwZXJzLCBwYXJlbnROYW1lQXJyLCByb3V0ZS5uYW1lKTtcbiAgICB9XG5cbiAgICBpZiAocm91dGUucGFyZW50TmFtZSAmJiByb3V0ZS5wYXJlbnROYW1lICE9PSBwYXJlbnROYW1lKSB7XG4gICAgICBwYXJlbnROYW1lQXJyLnB1c2gocm91dGUpO1xuICAgICAgcmV0dXJuIGZhbHNlO1xuICAgIH1cblxuICAgIHJldHVybiB0cnVlO1xuICB9O1xuXG4gIGlmIChwYXJlbnROYW1lKSB7XG4gICAgLy8gcmVjdXJzaXZlIGJsb2NrXG4gICAgcmV0dXJuIHJvdXRlcy5maWx0ZXIoZmlsdGVyKTtcbiAgfVxuXG4gIGNvbnN0IGZpbHRlcmVkUm91dGVzID0gcm91dGVzLmZpbHRlcihmaWx0ZXIpO1xuXG4gIGlmIChwYXJlbnROYW1lQXJyLmxlbmd0aCkge1xuICAgIHJldHVybiBzb3J0Um91dGVzKHNldENoaWxkUm91dGUoWy4uLmZpbHRlcmVkUm91dGVzLCAuLi53cmFwcGVyc10sIHBhcmVudE5hbWVBcnIpKTtcbiAgfVxuXG4gIHJldHVybiBmaWx0ZXJlZFJvdXRlcztcbn1cblxuZXhwb3J0IGZ1bmN0aW9uIHNldENoaWxkUm91dGUocm91dGVzOiBBQlAuRnVsbFJvdXRlW10sIHBhcmVudE5hbWVBcnI6IEFCUC5GdWxsUm91dGVbXSk6IEFCUC5GdWxsUm91dGVbXSB7XG4gIHJldHVybiByb3V0ZXMubWFwKHJvdXRlID0+IHtcbiAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XG4gICAgICByb3V0ZS5jaGlsZHJlbiA9IHNldENoaWxkUm91dGUocm91dGUuY2hpbGRyZW4sIHBhcmVudE5hbWVBcnIpO1xuICAgIH1cblxuICAgIGNvbnN0IGZvdW5kZWRDaGlsZHJlbiA9IHBhcmVudE5hbWVBcnIuZmlsdGVyKHBhcmVudCA9PiBwYXJlbnQucGFyZW50TmFtZSA9PT0gcm91dGUubmFtZSk7XG4gICAgaWYgKGZvdW5kZWRDaGlsZHJlbiAmJiBmb3VuZGVkQ2hpbGRyZW4ubGVuZ3RoKSB7XG4gICAgICByb3V0ZS5jaGlsZHJlbiA9IFsuLi4ocm91dGUuY2hpbGRyZW4gfHwgW10pLCAuLi5mb3VuZGVkQ2hpbGRyZW5dO1xuICAgIH1cblxuICAgIHJldHVybiByb3V0ZTtcbiAgfSk7XG59XG5cbmV4cG9ydCBmdW5jdGlvbiBzb3J0Um91dGVzKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdID0gW10pOiBBQlAuRnVsbFJvdXRlW10ge1xuICBpZiAoIXJvdXRlcy5sZW5ndGgpIHJldHVybiBbXTtcbiAgcmV0dXJuIHJvdXRlc1xuICAgIC5tYXAoKHJvdXRlLCBpbmRleCkgPT4ge1xuICAgICAgcmV0dXJuIHtcbiAgICAgICAgLi4ucm91dGUsXG4gICAgICAgIG9yZGVyOiB0eXBlb2Ygcm91dGUub3JkZXIgPT09ICd1bmRlZmluZWQnID8gaW5kZXggKyAxIDogcm91dGUub3JkZXIsXG4gICAgICB9O1xuICAgIH0pXG4gICAgLnNvcnQoKGEsIGIpID0+IGEub3JkZXIgLSBiLm9yZGVyKVxuICAgIC5tYXAocm91dGUgPT4ge1xuICAgICAgaWYgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgICByb3V0ZS5jaGlsZHJlbiA9IHNvcnRSb3V0ZXMocm91dGUuY2hpbGRyZW4pO1xuICAgICAgfVxuXG4gICAgICByZXR1cm4gcm91dGU7XG4gICAgfSk7XG59XG5cbmNvbnN0IEFCUF9ST1VURVMgPSBbXSBhcyBBQlAuRnVsbFJvdXRlW107XG5cbmV4cG9ydCBmdW5jdGlvbiBhZGRBYnBSb3V0ZXMocm91dGVzOiBBQlAuRnVsbFJvdXRlIHwgQUJQLkZ1bGxSb3V0ZVtdKTogdm9pZCB7XG4gIGlmICghQXJyYXkuaXNBcnJheShyb3V0ZXMpKSB7XG4gICAgcm91dGVzID0gW3JvdXRlc107XG4gIH1cblxuICBBQlBfUk9VVEVTLnB1c2goLi4ucm91dGVzKTtcbn1cblxuZXhwb3J0IGZ1bmN0aW9uIGdldEFicFJvdXRlcygpOiBBQlAuRnVsbFJvdXRlW10ge1xuICByZXR1cm4gQUJQX1JPVVRFUztcbn1cbiJdfQ==