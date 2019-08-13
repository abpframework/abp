/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
        if (route.children) {
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
    return routes
        .map((/**
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
    }))
        .filter((/**
     * @param {?} route
     * @return {?}
     */
    function (route) { return route.path || (route.children && route.children.length); }));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGUtdXRpbHMuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvdXRpbHMvcm91dGUtdXRpbHMudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7O0FBRUEsTUFBTSxVQUFVLGNBQWMsQ0FDNUIsTUFBdUIsRUFDdkIsUUFBOEIsRUFDOUIsYUFBcUMsRUFDckMsVUFBeUI7SUFGekIseUJBQUEsRUFBQSxhQUE4QjtJQUM5Qiw4QkFBQSxFQUFBLG1DQUFnQixFQUFFLEVBQW1CO0lBQ3JDLDJCQUFBLEVBQUEsaUJBQXlCOztRQUVuQixNQUFNOzs7O0lBQUcsVUFBQSxLQUFLO1FBQ2xCLElBQUksS0FBSyxDQUFDLFFBQVEsRUFBRTtZQUNsQixLQUFLLENBQUMsUUFBUSxHQUFHLGNBQWMsQ0FBQyxLQUFLLENBQUMsUUFBUSxFQUFFLFFBQVEsRUFBRSxhQUFhLEVBQUUsS0FBSyxDQUFDLElBQUksQ0FBQyxDQUFDO1NBQ3RGO1FBRUQsSUFBSSxLQUFLLENBQUMsVUFBVSxJQUFJLEtBQUssQ0FBQyxVQUFVLEtBQUssVUFBVSxFQUFFO1lBQ3ZELGFBQWEsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7WUFDMUIsT0FBTyxLQUFLLENBQUM7U0FDZDtRQUVELE9BQU8sSUFBSSxDQUFDO0lBQ2QsQ0FBQyxDQUFBO0lBRUQsSUFBSSxVQUFVLEVBQUU7UUFDZCxrQkFBa0I7UUFDbEIsT0FBTyxNQUFNLENBQUMsTUFBTSxDQUFDLE1BQU0sQ0FBQyxDQUFDO0tBQzlCOztRQUVLLGNBQWMsR0FBRyxNQUFNLENBQUMsTUFBTSxDQUFDLE1BQU0sQ0FBQztJQUU1QyxJQUFJLGFBQWEsQ0FBQyxNQUFNLEVBQUU7UUFDeEIsT0FBTyxVQUFVLENBQUMsYUFBYSxrQkFBSyxjQUFjLEVBQUssUUFBUSxHQUFHLGFBQWEsQ0FBQyxDQUFDLENBQUM7S0FDbkY7SUFFRCxPQUFPLGNBQWMsQ0FBQztBQUN4QixDQUFDOzs7Ozs7QUFFRCxNQUFNLFVBQVUsYUFBYSxDQUFDLE1BQXVCLEVBQUUsYUFBOEI7SUFDbkYsT0FBTyxNQUFNO1NBQ1YsR0FBRzs7OztJQUFDLFVBQUEsS0FBSztRQUNSLElBQUksS0FBSyxDQUFDLFFBQVEsSUFBSSxLQUFLLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUMzQyxLQUFLLENBQUMsUUFBUSxHQUFHLGFBQWEsQ0FBQyxLQUFLLENBQUMsUUFBUSxFQUFFLGFBQWEsQ0FBQyxDQUFDO1NBQy9EOztZQUVLLGVBQWUsR0FBRyxhQUFhLENBQUMsTUFBTTs7OztRQUFDLFVBQUEsTUFBTSxJQUFJLE9BQUEsTUFBTSxDQUFDLFVBQVUsS0FBSyxLQUFLLENBQUMsSUFBSSxFQUFoQyxDQUFnQyxFQUFDO1FBQ3hGLElBQUksZUFBZSxJQUFJLGVBQWUsQ0FBQyxNQUFNLEVBQUU7WUFDN0MsS0FBSyxDQUFDLFFBQVEsb0JBQU8sQ0FBQyxLQUFLLENBQUMsUUFBUSxJQUFJLEVBQUUsQ0FBQyxFQUFLLGVBQWUsQ0FBQyxDQUFDO1NBQ2xFO1FBRUQsT0FBTyxLQUFLLENBQUM7SUFDZixDQUFDLEVBQUM7U0FDRCxNQUFNOzs7O0lBQUMsVUFBQSxLQUFLLElBQUksT0FBQSxLQUFLLENBQUMsSUFBSSxJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsSUFBSSxLQUFLLENBQUMsUUFBUSxDQUFDLE1BQU0sQ0FBQyxFQUF2RCxDQUF1RCxFQUFDLENBQUM7QUFDOUUsQ0FBQzs7Ozs7QUFFRCxNQUFNLFVBQVUsVUFBVSxDQUFDLE1BQTRCO0lBQTVCLHVCQUFBLEVBQUEsV0FBNEI7SUFDckQsSUFBSSxDQUFDLE1BQU0sQ0FBQyxNQUFNO1FBQUUsT0FBTyxFQUFFLENBQUM7SUFDOUIsT0FBTyxNQUFNO1NBQ1YsSUFBSTs7Ozs7SUFBQyxVQUFDLENBQUMsRUFBRSxDQUFDLElBQUssT0FBQSxDQUFDLENBQUMsS0FBSyxHQUFHLENBQUMsQ0FBQyxLQUFLLEVBQWpCLENBQWlCLEVBQUM7U0FDakMsR0FBRzs7OztJQUFDLFVBQUEsS0FBSztRQUNSLElBQUksS0FBSyxDQUFDLFFBQVEsSUFBSSxLQUFLLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUMzQyxLQUFLLENBQUMsUUFBUSxHQUFHLFVBQVUsQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLENBQUM7U0FDN0M7UUFFRCxPQUFPLEtBQUssQ0FBQztJQUNmLENBQUMsRUFBQyxDQUFDO0FBQ1AsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCB9IGZyb20gJy4uL21vZGVscyc7XG5cbmV4cG9ydCBmdW5jdGlvbiBvcmdhbml6ZVJvdXRlcyhcbiAgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10sXG4gIHdyYXBwZXJzOiBBQlAuRnVsbFJvdXRlW10gPSBbXSxcbiAgcGFyZW50TmFtZUFyciA9IFtdIGFzIEFCUC5GdWxsUm91dGVbXSxcbiAgcGFyZW50TmFtZTogc3RyaW5nID0gbnVsbCxcbik6IEFCUC5GdWxsUm91dGVbXSB7XG4gIGNvbnN0IGZpbHRlciA9IHJvdXRlID0+IHtcbiAgICBpZiAocm91dGUuY2hpbGRyZW4pIHtcbiAgICAgIHJvdXRlLmNoaWxkcmVuID0gb3JnYW5pemVSb3V0ZXMocm91dGUuY2hpbGRyZW4sIHdyYXBwZXJzLCBwYXJlbnROYW1lQXJyLCByb3V0ZS5uYW1lKTtcbiAgICB9XG5cbiAgICBpZiAocm91dGUucGFyZW50TmFtZSAmJiByb3V0ZS5wYXJlbnROYW1lICE9PSBwYXJlbnROYW1lKSB7XG4gICAgICBwYXJlbnROYW1lQXJyLnB1c2gocm91dGUpO1xuICAgICAgcmV0dXJuIGZhbHNlO1xuICAgIH1cblxuICAgIHJldHVybiB0cnVlO1xuICB9O1xuXG4gIGlmIChwYXJlbnROYW1lKSB7XG4gICAgLy8gcmVjdXJzaXZlIGJsb2NrXG4gICAgcmV0dXJuIHJvdXRlcy5maWx0ZXIoZmlsdGVyKTtcbiAgfVxuXG4gIGNvbnN0IGZpbHRlcmVkUm91dGVzID0gcm91dGVzLmZpbHRlcihmaWx0ZXIpO1xuXG4gIGlmIChwYXJlbnROYW1lQXJyLmxlbmd0aCkge1xuICAgIHJldHVybiBzb3J0Um91dGVzKHNldENoaWxkUm91dGUoWy4uLmZpbHRlcmVkUm91dGVzLCAuLi53cmFwcGVyc10sIHBhcmVudE5hbWVBcnIpKTtcbiAgfVxuXG4gIHJldHVybiBmaWx0ZXJlZFJvdXRlcztcbn1cblxuZXhwb3J0IGZ1bmN0aW9uIHNldENoaWxkUm91dGUocm91dGVzOiBBQlAuRnVsbFJvdXRlW10sIHBhcmVudE5hbWVBcnI6IEFCUC5GdWxsUm91dGVbXSk6IEFCUC5GdWxsUm91dGVbXSB7XG4gIHJldHVybiByb3V0ZXNcbiAgICAubWFwKHJvdXRlID0+IHtcbiAgICAgIGlmIChyb3V0ZS5jaGlsZHJlbiAmJiByb3V0ZS5jaGlsZHJlbi5sZW5ndGgpIHtcbiAgICAgICAgcm91dGUuY2hpbGRyZW4gPSBzZXRDaGlsZFJvdXRlKHJvdXRlLmNoaWxkcmVuLCBwYXJlbnROYW1lQXJyKTtcbiAgICAgIH1cblxuICAgICAgY29uc3QgZm91bmRlZENoaWxkcmVuID0gcGFyZW50TmFtZUFyci5maWx0ZXIocGFyZW50ID0+IHBhcmVudC5wYXJlbnROYW1lID09PSByb3V0ZS5uYW1lKTtcbiAgICAgIGlmIChmb3VuZGVkQ2hpbGRyZW4gJiYgZm91bmRlZENoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgICByb3V0ZS5jaGlsZHJlbiA9IFsuLi4ocm91dGUuY2hpbGRyZW4gfHwgW10pLCAuLi5mb3VuZGVkQ2hpbGRyZW5dO1xuICAgICAgfVxuXG4gICAgICByZXR1cm4gcm91dGU7XG4gICAgfSlcbiAgICAuZmlsdGVyKHJvdXRlID0+IHJvdXRlLnBhdGggfHwgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCkpO1xufVxuXG5leHBvcnQgZnVuY3Rpb24gc29ydFJvdXRlcyhyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSA9IFtdKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgaWYgKCFyb3V0ZXMubGVuZ3RoKSByZXR1cm4gW107XG4gIHJldHVybiByb3V0ZXNcbiAgICAuc29ydCgoYSwgYikgPT4gYS5vcmRlciAtIGIub3JkZXIpXG4gICAgLm1hcChyb3V0ZSA9PiB7XG4gICAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XG4gICAgICAgIHJvdXRlLmNoaWxkcmVuID0gc29ydFJvdXRlcyhyb3V0ZS5jaGlsZHJlbik7XG4gICAgICB9XG5cbiAgICAgIHJldHVybiByb3V0ZTtcbiAgICB9KTtcbn1cbiJdfQ==