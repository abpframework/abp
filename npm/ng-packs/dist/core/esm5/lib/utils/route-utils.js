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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGUtdXRpbHMuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvdXRpbHMvcm91dGUtdXRpbHMudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7OztBQUVBLE1BQU0sVUFBVSxjQUFjLENBQzVCLE1BQXVCLEVBQ3ZCLFFBQThCLEVBQzlCLGFBQXFDLEVBQ3JDLFVBQXlCO0lBRnpCLHlCQUFBLEVBQUEsYUFBOEI7SUFDOUIsOEJBQUEsRUFBQSxtQ0FBZ0IsRUFBRSxFQUFtQjtJQUNyQywyQkFBQSxFQUFBLGlCQUF5Qjs7UUFFbkIsTUFBTTs7OztJQUFHLFVBQUEsS0FBSztRQUNsQixJQUFJLEtBQUssQ0FBQyxRQUFRLElBQUksS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDM0MsS0FBSyxDQUFDLFFBQVEsR0FBRyxjQUFjLENBQUMsS0FBSyxDQUFDLFFBQVEsRUFBRSxRQUFRLEVBQUUsYUFBYSxFQUFFLEtBQUssQ0FBQyxJQUFJLENBQUMsQ0FBQztTQUN0RjtRQUVELElBQUksS0FBSyxDQUFDLFVBQVUsSUFBSSxLQUFLLENBQUMsVUFBVSxLQUFLLFVBQVUsRUFBRTtZQUN2RCxhQUFhLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO1lBQzFCLE9BQU8sS0FBSyxDQUFDO1NBQ2Q7UUFFRCxPQUFPLElBQUksQ0FBQztJQUNkLENBQUMsQ0FBQTtJQUVELElBQUksVUFBVSxFQUFFO1FBQ2Qsa0JBQWtCO1FBQ2xCLE9BQU8sTUFBTSxDQUFDLE1BQU0sQ0FBQyxNQUFNLENBQUMsQ0FBQztLQUM5Qjs7UUFFSyxjQUFjLEdBQUcsTUFBTSxDQUFDLE1BQU0sQ0FBQyxNQUFNLENBQUM7SUFFNUMsSUFBSSxhQUFhLENBQUMsTUFBTSxFQUFFO1FBQ3hCLE9BQU8sVUFBVSxDQUFDLGFBQWEsa0JBQUssY0FBYyxFQUFLLFFBQVEsR0FBRyxhQUFhLENBQUMsQ0FBQyxDQUFDO0tBQ25GO0lBRUQsT0FBTyxjQUFjLENBQUM7QUFDeEIsQ0FBQzs7Ozs7O0FBRUQsTUFBTSxVQUFVLGFBQWEsQ0FBQyxNQUF1QixFQUFFLGFBQThCO0lBQ25GLE9BQU8sTUFBTSxDQUFDLEdBQUc7Ozs7SUFBQyxVQUFBLEtBQUs7UUFDckIsSUFBSSxLQUFLLENBQUMsUUFBUSxJQUFJLEtBQUssQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO1lBQzNDLEtBQUssQ0FBQyxRQUFRLEdBQUcsYUFBYSxDQUFDLEtBQUssQ0FBQyxRQUFRLEVBQUUsYUFBYSxDQUFDLENBQUM7U0FDL0Q7O1lBRUssZUFBZSxHQUFHLGFBQWEsQ0FBQyxNQUFNOzs7O1FBQUMsVUFBQSxNQUFNLElBQUksT0FBQSxNQUFNLENBQUMsVUFBVSxLQUFLLEtBQUssQ0FBQyxJQUFJLEVBQWhDLENBQWdDLEVBQUM7UUFDeEYsSUFBSSxlQUFlLElBQUksZUFBZSxDQUFDLE1BQU0sRUFBRTtZQUM3QyxLQUFLLENBQUMsUUFBUSxvQkFBTyxDQUFDLEtBQUssQ0FBQyxRQUFRLElBQUksRUFBRSxDQUFDLEVBQUssZUFBZSxDQUFDLENBQUM7U0FDbEU7UUFFRCxPQUFPLEtBQUssQ0FBQztJQUNmLENBQUMsRUFBQyxDQUFDO0FBQ0wsQ0FBQzs7Ozs7QUFFRCxNQUFNLFVBQVUsVUFBVSxDQUFDLE1BQTRCO0lBQTVCLHVCQUFBLEVBQUEsV0FBNEI7SUFDckQsSUFBSSxDQUFDLE1BQU0sQ0FBQyxNQUFNO1FBQUUsT0FBTyxFQUFFLENBQUM7SUFDOUIsT0FBTyxNQUFNO1NBQ1YsR0FBRzs7Ozs7SUFBQyxVQUFDLEtBQUssRUFBRSxLQUFLO1FBQ2hCLDRCQUNLLEtBQUssSUFDUixLQUFLLEVBQUUsT0FBTyxLQUFLLENBQUMsS0FBSyxLQUFLLFdBQVcsQ0FBQyxDQUFDLENBQUMsS0FBSyxHQUFHLENBQUMsQ0FBQyxDQUFDLENBQUMsS0FBSyxDQUFDLEtBQUssSUFDbkU7SUFDSixDQUFDLEVBQUM7U0FDRCxJQUFJOzs7OztJQUFDLFVBQUMsQ0FBQyxFQUFFLENBQUMsSUFBSyxPQUFBLENBQUMsQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDLEtBQUssRUFBakIsQ0FBaUIsRUFBQztTQUNqQyxHQUFHOzs7O0lBQUMsVUFBQSxLQUFLO1FBQ1IsSUFBSSxLQUFLLENBQUMsUUFBUSxJQUFJLEtBQUssQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO1lBQzNDLEtBQUssQ0FBQyxRQUFRLEdBQUcsVUFBVSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsQ0FBQztTQUM3QztRQUVELE9BQU8sS0FBSyxDQUFDO0lBQ2YsQ0FBQyxFQUFDLENBQUM7QUFDUCxDQUFDOztJQUVLLFVBQVUsR0FBRyxtQkFBQSxFQUFFLEVBQW1COzs7OztBQUV4QyxNQUFNLFVBQVUsWUFBWSxDQUFDLE1BQXVDO0lBQ2xFLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLE1BQU0sQ0FBQyxFQUFFO1FBQzFCLE1BQU0sR0FBRyxDQUFDLE1BQU0sQ0FBQyxDQUFDO0tBQ25CO0lBRUQsVUFBVSxDQUFDLElBQUksT0FBZixVQUFVLG1CQUFTLE1BQU0sR0FBRTtBQUM3QixDQUFDOzs7O0FBRUQsTUFBTSxVQUFVLFlBQVk7SUFDMUIsT0FBTyxVQUFVLENBQUM7QUFDcEIsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCB9IGZyb20gJy4uL21vZGVscyc7XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gb3JnYW5pemVSb3V0ZXMoXHJcbiAgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10sXHJcbiAgd3JhcHBlcnM6IEFCUC5GdWxsUm91dGVbXSA9IFtdLFxyXG4gIHBhcmVudE5hbWVBcnIgPSBbXSBhcyBBQlAuRnVsbFJvdXRlW10sXHJcbiAgcGFyZW50TmFtZTogc3RyaW5nID0gbnVsbCxcclxuKTogQUJQLkZ1bGxSb3V0ZVtdIHtcclxuICBjb25zdCBmaWx0ZXIgPSByb3V0ZSA9PiB7XHJcbiAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XHJcbiAgICAgIHJvdXRlLmNoaWxkcmVuID0gb3JnYW5pemVSb3V0ZXMocm91dGUuY2hpbGRyZW4sIHdyYXBwZXJzLCBwYXJlbnROYW1lQXJyLCByb3V0ZS5uYW1lKTtcclxuICAgIH1cclxuXHJcbiAgICBpZiAocm91dGUucGFyZW50TmFtZSAmJiByb3V0ZS5wYXJlbnROYW1lICE9PSBwYXJlbnROYW1lKSB7XHJcbiAgICAgIHBhcmVudE5hbWVBcnIucHVzaChyb3V0ZSk7XHJcbiAgICAgIHJldHVybiBmYWxzZTtcclxuICAgIH1cclxuXHJcbiAgICByZXR1cm4gdHJ1ZTtcclxuICB9O1xyXG5cclxuICBpZiAocGFyZW50TmFtZSkge1xyXG4gICAgLy8gcmVjdXJzaXZlIGJsb2NrXHJcbiAgICByZXR1cm4gcm91dGVzLmZpbHRlcihmaWx0ZXIpO1xyXG4gIH1cclxuXHJcbiAgY29uc3QgZmlsdGVyZWRSb3V0ZXMgPSByb3V0ZXMuZmlsdGVyKGZpbHRlcik7XHJcblxyXG4gIGlmIChwYXJlbnROYW1lQXJyLmxlbmd0aCkge1xyXG4gICAgcmV0dXJuIHNvcnRSb3V0ZXMoc2V0Q2hpbGRSb3V0ZShbLi4uZmlsdGVyZWRSb3V0ZXMsIC4uLndyYXBwZXJzXSwgcGFyZW50TmFtZUFycikpO1xyXG4gIH1cclxuXHJcbiAgcmV0dXJuIGZpbHRlcmVkUm91dGVzO1xyXG59XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gc2V0Q2hpbGRSb3V0ZShyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSwgcGFyZW50TmFtZUFycjogQUJQLkZ1bGxSb3V0ZVtdKTogQUJQLkZ1bGxSb3V0ZVtdIHtcclxuICByZXR1cm4gcm91dGVzLm1hcChyb3V0ZSA9PiB7XHJcbiAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XHJcbiAgICAgIHJvdXRlLmNoaWxkcmVuID0gc2V0Q2hpbGRSb3V0ZShyb3V0ZS5jaGlsZHJlbiwgcGFyZW50TmFtZUFycik7XHJcbiAgICB9XHJcblxyXG4gICAgY29uc3QgZm91bmRlZENoaWxkcmVuID0gcGFyZW50TmFtZUFyci5maWx0ZXIocGFyZW50ID0+IHBhcmVudC5wYXJlbnROYW1lID09PSByb3V0ZS5uYW1lKTtcclxuICAgIGlmIChmb3VuZGVkQ2hpbGRyZW4gJiYgZm91bmRlZENoaWxkcmVuLmxlbmd0aCkge1xyXG4gICAgICByb3V0ZS5jaGlsZHJlbiA9IFsuLi4ocm91dGUuY2hpbGRyZW4gfHwgW10pLCAuLi5mb3VuZGVkQ2hpbGRyZW5dO1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiByb3V0ZTtcclxuICB9KTtcclxufVxyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIHNvcnRSb3V0ZXMocm91dGVzOiBBQlAuRnVsbFJvdXRlW10gPSBbXSk6IEFCUC5GdWxsUm91dGVbXSB7XHJcbiAgaWYgKCFyb3V0ZXMubGVuZ3RoKSByZXR1cm4gW107XHJcbiAgcmV0dXJuIHJvdXRlc1xyXG4gICAgLm1hcCgocm91dGUsIGluZGV4KSA9PiB7XHJcbiAgICAgIHJldHVybiB7XHJcbiAgICAgICAgLi4ucm91dGUsXHJcbiAgICAgICAgb3JkZXI6IHR5cGVvZiByb3V0ZS5vcmRlciA9PT0gJ3VuZGVmaW5lZCcgPyBpbmRleCArIDEgOiByb3V0ZS5vcmRlcixcclxuICAgICAgfTtcclxuICAgIH0pXHJcbiAgICAuc29ydCgoYSwgYikgPT4gYS5vcmRlciAtIGIub3JkZXIpXHJcbiAgICAubWFwKHJvdXRlID0+IHtcclxuICAgICAgaWYgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCkge1xyXG4gICAgICAgIHJvdXRlLmNoaWxkcmVuID0gc29ydFJvdXRlcyhyb3V0ZS5jaGlsZHJlbik7XHJcbiAgICAgIH1cclxuXHJcbiAgICAgIHJldHVybiByb3V0ZTtcclxuICAgIH0pO1xyXG59XHJcblxyXG5jb25zdCBBQlBfUk9VVEVTID0gW10gYXMgQUJQLkZ1bGxSb3V0ZVtdO1xyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIGFkZEFicFJvdXRlcyhyb3V0ZXM6IEFCUC5GdWxsUm91dGUgfCBBQlAuRnVsbFJvdXRlW10pOiB2b2lkIHtcclxuICBpZiAoIUFycmF5LmlzQXJyYXkocm91dGVzKSkge1xyXG4gICAgcm91dGVzID0gW3JvdXRlc107XHJcbiAgfVxyXG5cclxuICBBQlBfUk9VVEVTLnB1c2goLi4ucm91dGVzKTtcclxufVxyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIGdldEFicFJvdXRlcygpOiBBQlAuRnVsbFJvdXRlW10ge1xyXG4gIHJldHVybiBBQlBfUk9VVEVTO1xyXG59XHJcbiJdfQ==