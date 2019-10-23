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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGUtdXRpbHMuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvdXRpbHMvcm91dGUtdXRpbHMudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7O0FBRUEsTUFBTSxVQUFVLGNBQWMsQ0FDNUIsTUFBdUIsRUFDdkIsUUFBOEIsRUFDOUIsYUFBcUMsRUFDckMsVUFBeUI7SUFGekIseUJBQUEsRUFBQSxhQUE4QjtJQUM5Qiw4QkFBQSxFQUFBLG1DQUFnQixFQUFFLEVBQW1CO0lBQ3JDLDJCQUFBLEVBQUEsaUJBQXlCOztRQUVuQixNQUFNOzs7O0lBQUcsVUFBQSxLQUFLO1FBQ2xCLElBQUksS0FBSyxDQUFDLFFBQVEsSUFBSSxLQUFLLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUMzQyxLQUFLLENBQUMsUUFBUSxHQUFHLGNBQWMsQ0FBQyxLQUFLLENBQUMsUUFBUSxFQUFFLFFBQVEsRUFBRSxhQUFhLEVBQUUsS0FBSyxDQUFDLElBQUksQ0FBQyxDQUFDO1NBQ3RGO1FBRUQsSUFBSSxLQUFLLENBQUMsVUFBVSxJQUFJLEtBQUssQ0FBQyxVQUFVLEtBQUssVUFBVSxFQUFFO1lBQ3ZELGFBQWEsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7WUFDMUIsT0FBTyxLQUFLLENBQUM7U0FDZDtRQUVELE9BQU8sSUFBSSxDQUFDO0lBQ2QsQ0FBQyxDQUFBO0lBRUQsSUFBSSxVQUFVLEVBQUU7UUFDZCxrQkFBa0I7UUFDbEIsT0FBTyxNQUFNLENBQUMsTUFBTSxDQUFDLE1BQU0sQ0FBQyxDQUFDO0tBQzlCOztRQUVLLGNBQWMsR0FBRyxNQUFNLENBQUMsTUFBTSxDQUFDLE1BQU0sQ0FBQztJQUU1QyxJQUFJLGFBQWEsQ0FBQyxNQUFNLEVBQUU7UUFDeEIsT0FBTyxVQUFVLENBQUMsYUFBYSxrQkFBSyxjQUFjLEVBQUssUUFBUSxHQUFHLGFBQWEsQ0FBQyxDQUFDLENBQUM7S0FDbkY7SUFFRCxPQUFPLGNBQWMsQ0FBQztBQUN4QixDQUFDOzs7Ozs7QUFFRCxNQUFNLFVBQVUsYUFBYSxDQUFDLE1BQXVCLEVBQUUsYUFBOEI7SUFDbkYsT0FBTyxNQUFNLENBQUMsR0FBRzs7OztJQUFDLFVBQUEsS0FBSztRQUNyQixJQUFJLEtBQUssQ0FBQyxRQUFRLElBQUksS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDM0MsS0FBSyxDQUFDLFFBQVEsR0FBRyxhQUFhLENBQUMsS0FBSyxDQUFDLFFBQVEsRUFBRSxhQUFhLENBQUMsQ0FBQztTQUMvRDs7WUFFSyxlQUFlLEdBQUcsYUFBYSxDQUFDLE1BQU07Ozs7UUFBQyxVQUFBLE1BQU0sSUFBSSxPQUFBLE1BQU0sQ0FBQyxVQUFVLEtBQUssS0FBSyxDQUFDLElBQUksRUFBaEMsQ0FBZ0MsRUFBQztRQUN4RixJQUFJLGVBQWUsSUFBSSxlQUFlLENBQUMsTUFBTSxFQUFFO1lBQzdDLEtBQUssQ0FBQyxRQUFRLG9CQUFPLENBQUMsS0FBSyxDQUFDLFFBQVEsSUFBSSxFQUFFLENBQUMsRUFBSyxlQUFlLENBQUMsQ0FBQztTQUNsRTtRQUVELE9BQU8sS0FBSyxDQUFDO0lBQ2YsQ0FBQyxFQUFDLENBQUM7QUFDTCxDQUFDOzs7OztBQUVELE1BQU0sVUFBVSxVQUFVLENBQUMsTUFBNEI7SUFBNUIsdUJBQUEsRUFBQSxXQUE0QjtJQUNyRCxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU07UUFBRSxPQUFPLEVBQUUsQ0FBQztJQUM5QixPQUFPLE1BQU07U0FDVixJQUFJOzs7OztJQUFDLFVBQUMsQ0FBQyxFQUFFLENBQUMsSUFBSyxPQUFBLENBQUMsQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDLEtBQUssRUFBakIsQ0FBaUIsRUFBQztTQUNqQyxHQUFHOzs7O0lBQUMsVUFBQSxLQUFLO1FBQ1IsSUFBSSxLQUFLLENBQUMsUUFBUSxJQUFJLEtBQUssQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO1lBQzNDLEtBQUssQ0FBQyxRQUFRLEdBQUcsVUFBVSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsQ0FBQztTQUM3QztRQUVELE9BQU8sS0FBSyxDQUFDO0lBQ2YsQ0FBQyxFQUFDLENBQUM7QUFDUCxDQUFDOztJQUVLLFVBQVUsR0FBRyxtQkFBQSxFQUFFLEVBQW1COzs7OztBQUV4QyxNQUFNLFVBQVUsWUFBWSxDQUFDLE1BQXVDO0lBQ2xFLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLE1BQU0sQ0FBQyxFQUFFO1FBQzFCLE1BQU0sR0FBRyxDQUFDLE1BQU0sQ0FBQyxDQUFDO0tBQ25CO0lBRUQsVUFBVSxDQUFDLElBQUksT0FBZixVQUFVLG1CQUFTLE1BQU0sR0FBRTtBQUM3QixDQUFDOzs7O0FBRUQsTUFBTSxVQUFVLFlBQVk7SUFDMUIsT0FBTyxVQUFVLENBQUM7QUFDcEIsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCB9IGZyb20gJy4uL21vZGVscyc7XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gb3JnYW5pemVSb3V0ZXMoXHJcbiAgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10sXHJcbiAgd3JhcHBlcnM6IEFCUC5GdWxsUm91dGVbXSA9IFtdLFxyXG4gIHBhcmVudE5hbWVBcnIgPSBbXSBhcyBBQlAuRnVsbFJvdXRlW10sXHJcbiAgcGFyZW50TmFtZTogc3RyaW5nID0gbnVsbCxcclxuKTogQUJQLkZ1bGxSb3V0ZVtdIHtcclxuICBjb25zdCBmaWx0ZXIgPSByb3V0ZSA9PiB7XHJcbiAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XHJcbiAgICAgIHJvdXRlLmNoaWxkcmVuID0gb3JnYW5pemVSb3V0ZXMocm91dGUuY2hpbGRyZW4sIHdyYXBwZXJzLCBwYXJlbnROYW1lQXJyLCByb3V0ZS5uYW1lKTtcclxuICAgIH1cclxuXHJcbiAgICBpZiAocm91dGUucGFyZW50TmFtZSAmJiByb3V0ZS5wYXJlbnROYW1lICE9PSBwYXJlbnROYW1lKSB7XHJcbiAgICAgIHBhcmVudE5hbWVBcnIucHVzaChyb3V0ZSk7XHJcbiAgICAgIHJldHVybiBmYWxzZTtcclxuICAgIH1cclxuXHJcbiAgICByZXR1cm4gdHJ1ZTtcclxuICB9O1xyXG5cclxuICBpZiAocGFyZW50TmFtZSkge1xyXG4gICAgLy8gcmVjdXJzaXZlIGJsb2NrXHJcbiAgICByZXR1cm4gcm91dGVzLmZpbHRlcihmaWx0ZXIpO1xyXG4gIH1cclxuXHJcbiAgY29uc3QgZmlsdGVyZWRSb3V0ZXMgPSByb3V0ZXMuZmlsdGVyKGZpbHRlcik7XHJcblxyXG4gIGlmIChwYXJlbnROYW1lQXJyLmxlbmd0aCkge1xyXG4gICAgcmV0dXJuIHNvcnRSb3V0ZXMoc2V0Q2hpbGRSb3V0ZShbLi4uZmlsdGVyZWRSb3V0ZXMsIC4uLndyYXBwZXJzXSwgcGFyZW50TmFtZUFycikpO1xyXG4gIH1cclxuXHJcbiAgcmV0dXJuIGZpbHRlcmVkUm91dGVzO1xyXG59XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gc2V0Q2hpbGRSb3V0ZShyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSwgcGFyZW50TmFtZUFycjogQUJQLkZ1bGxSb3V0ZVtdKTogQUJQLkZ1bGxSb3V0ZVtdIHtcclxuICByZXR1cm4gcm91dGVzLm1hcChyb3V0ZSA9PiB7XHJcbiAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XHJcbiAgICAgIHJvdXRlLmNoaWxkcmVuID0gc2V0Q2hpbGRSb3V0ZShyb3V0ZS5jaGlsZHJlbiwgcGFyZW50TmFtZUFycik7XHJcbiAgICB9XHJcblxyXG4gICAgY29uc3QgZm91bmRlZENoaWxkcmVuID0gcGFyZW50TmFtZUFyci5maWx0ZXIocGFyZW50ID0+IHBhcmVudC5wYXJlbnROYW1lID09PSByb3V0ZS5uYW1lKTtcclxuICAgIGlmIChmb3VuZGVkQ2hpbGRyZW4gJiYgZm91bmRlZENoaWxkcmVuLmxlbmd0aCkge1xyXG4gICAgICByb3V0ZS5jaGlsZHJlbiA9IFsuLi4ocm91dGUuY2hpbGRyZW4gfHwgW10pLCAuLi5mb3VuZGVkQ2hpbGRyZW5dO1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiByb3V0ZTtcclxuICB9KTtcclxufVxyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIHNvcnRSb3V0ZXMocm91dGVzOiBBQlAuRnVsbFJvdXRlW10gPSBbXSk6IEFCUC5GdWxsUm91dGVbXSB7XHJcbiAgaWYgKCFyb3V0ZXMubGVuZ3RoKSByZXR1cm4gW107XHJcbiAgcmV0dXJuIHJvdXRlc1xyXG4gICAgLnNvcnQoKGEsIGIpID0+IGEub3JkZXIgLSBiLm9yZGVyKVxyXG4gICAgLm1hcChyb3V0ZSA9PiB7XHJcbiAgICAgIGlmIChyb3V0ZS5jaGlsZHJlbiAmJiByb3V0ZS5jaGlsZHJlbi5sZW5ndGgpIHtcclxuICAgICAgICByb3V0ZS5jaGlsZHJlbiA9IHNvcnRSb3V0ZXMocm91dGUuY2hpbGRyZW4pO1xyXG4gICAgICB9XHJcblxyXG4gICAgICByZXR1cm4gcm91dGU7XHJcbiAgICB9KTtcclxufVxyXG5cclxuY29uc3QgQUJQX1JPVVRFUyA9IFtdIGFzIEFCUC5GdWxsUm91dGVbXTtcclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBhZGRBYnBSb3V0ZXMocm91dGVzOiBBQlAuRnVsbFJvdXRlIHwgQUJQLkZ1bGxSb3V0ZVtdKTogdm9pZCB7XHJcbiAgaWYgKCFBcnJheS5pc0FycmF5KHJvdXRlcykpIHtcclxuICAgIHJvdXRlcyA9IFtyb3V0ZXNdO1xyXG4gIH1cclxuXHJcbiAgQUJQX1JPVVRFUy5wdXNoKC4uLnJvdXRlcyk7XHJcbn1cclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBnZXRBYnBSb3V0ZXMoKTogQUJQLkZ1bGxSb3V0ZVtdIHtcclxuICByZXR1cm4gQUJQX1JPVVRFUztcclxufVxyXG4iXX0=