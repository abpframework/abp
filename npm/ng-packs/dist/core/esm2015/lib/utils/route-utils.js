/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/**
 * @param {?} routes
 * @param {?=} wrappers
 * @param {?=} parentNameArr
 * @param {?=} parentName
 * @return {?}
 */
export function organizeRoutes(routes, wrappers = [], parentNameArr = (/** @type {?} */ ([])), parentName = null) {
    /** @type {?} */
    const filter = (/**
     * @param {?} route
     * @return {?}
     */
    route => {
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
    const filteredRoutes = routes.filter(filter);
    if (parentNameArr.length) {
        return sortRoutes(setChildRoute([...filteredRoutes, ...wrappers], parentNameArr));
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
    route => {
        if (route.children && route.children.length) {
            route.children = setChildRoute(route.children, parentNameArr);
        }
        /** @type {?} */
        const foundedChildren = parentNameArr.filter((/**
         * @param {?} parent
         * @return {?}
         */
        parent => parent.parentName === route.name));
        if (foundedChildren && foundedChildren.length) {
            route.children = [...(route.children || []), ...foundedChildren];
        }
        return route;
    }));
}
/**
 * @param {?=} routes
 * @return {?}
 */
export function sortRoutes(routes = []) {
    if (!routes.length)
        return [];
    return routes
        .sort((/**
     * @param {?} a
     * @param {?} b
     * @return {?}
     */
    (a, b) => a.order - b.order))
        .map((/**
     * @param {?} route
     * @return {?}
     */
    route => {
        if (route.children && route.children.length) {
            route.children = sortRoutes(route.children);
        }
        return route;
    }));
}
/** @type {?} */
const ABP_ROUTES = (/** @type {?} */ ([]));
/**
 * @param {?} routes
 * @return {?}
 */
export function addAbpRoutes(routes) {
    if (!Array.isArray(routes)) {
        routes = [routes];
    }
    ABP_ROUTES.push(...routes);
}
/**
 * @return {?}
 */
export function getAbpRoutes() {
    return ABP_ROUTES;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGUtdXRpbHMuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvdXRpbHMvcm91dGUtdXRpbHMudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7QUFFQSxNQUFNLFVBQVUsY0FBYyxDQUM1QixNQUF1QixFQUN2QixXQUE0QixFQUFFLEVBQzlCLGFBQWEsR0FBRyxtQkFBQSxFQUFFLEVBQW1CLEVBQ3JDLGFBQXFCLElBQUk7O1VBRW5CLE1BQU07Ozs7SUFBRyxLQUFLLENBQUMsRUFBRTtRQUNyQixJQUFJLEtBQUssQ0FBQyxRQUFRLElBQUksS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDM0MsS0FBSyxDQUFDLFFBQVEsR0FBRyxjQUFjLENBQUMsS0FBSyxDQUFDLFFBQVEsRUFBRSxRQUFRLEVBQUUsYUFBYSxFQUFFLEtBQUssQ0FBQyxJQUFJLENBQUMsQ0FBQztTQUN0RjtRQUVELElBQUksS0FBSyxDQUFDLFVBQVUsSUFBSSxLQUFLLENBQUMsVUFBVSxLQUFLLFVBQVUsRUFBRTtZQUN2RCxhQUFhLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO1lBQzFCLE9BQU8sS0FBSyxDQUFDO1NBQ2Q7UUFFRCxPQUFPLElBQUksQ0FBQztJQUNkLENBQUMsQ0FBQTtJQUVELElBQUksVUFBVSxFQUFFO1FBQ2Qsa0JBQWtCO1FBQ2xCLE9BQU8sTUFBTSxDQUFDLE1BQU0sQ0FBQyxNQUFNLENBQUMsQ0FBQztLQUM5Qjs7VUFFSyxjQUFjLEdBQUcsTUFBTSxDQUFDLE1BQU0sQ0FBQyxNQUFNLENBQUM7SUFFNUMsSUFBSSxhQUFhLENBQUMsTUFBTSxFQUFFO1FBQ3hCLE9BQU8sVUFBVSxDQUFDLGFBQWEsQ0FBQyxDQUFDLEdBQUcsY0FBYyxFQUFFLEdBQUcsUUFBUSxDQUFDLEVBQUUsYUFBYSxDQUFDLENBQUMsQ0FBQztLQUNuRjtJQUVELE9BQU8sY0FBYyxDQUFDO0FBQ3hCLENBQUM7Ozs7OztBQUVELE1BQU0sVUFBVSxhQUFhLENBQUMsTUFBdUIsRUFBRSxhQUE4QjtJQUNuRixPQUFPLE1BQU0sQ0FBQyxHQUFHOzs7O0lBQUMsS0FBSyxDQUFDLEVBQUU7UUFDeEIsSUFBSSxLQUFLLENBQUMsUUFBUSxJQUFJLEtBQUssQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO1lBQzNDLEtBQUssQ0FBQyxRQUFRLEdBQUcsYUFBYSxDQUFDLEtBQUssQ0FBQyxRQUFRLEVBQUUsYUFBYSxDQUFDLENBQUM7U0FDL0Q7O2NBRUssZUFBZSxHQUFHLGFBQWEsQ0FBQyxNQUFNOzs7O1FBQUMsTUFBTSxDQUFDLEVBQUUsQ0FBQyxNQUFNLENBQUMsVUFBVSxLQUFLLEtBQUssQ0FBQyxJQUFJLEVBQUM7UUFDeEYsSUFBSSxlQUFlLElBQUksZUFBZSxDQUFDLE1BQU0sRUFBRTtZQUM3QyxLQUFLLENBQUMsUUFBUSxHQUFHLENBQUMsR0FBRyxDQUFDLEtBQUssQ0FBQyxRQUFRLElBQUksRUFBRSxDQUFDLEVBQUUsR0FBRyxlQUFlLENBQUMsQ0FBQztTQUNsRTtRQUVELE9BQU8sS0FBSyxDQUFDO0lBQ2YsQ0FBQyxFQUFDLENBQUM7QUFDTCxDQUFDOzs7OztBQUVELE1BQU0sVUFBVSxVQUFVLENBQUMsU0FBMEIsRUFBRTtJQUNyRCxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU07UUFBRSxPQUFPLEVBQUUsQ0FBQztJQUM5QixPQUFPLE1BQU07U0FDVixJQUFJOzs7OztJQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsRUFBRSxFQUFFLENBQUMsQ0FBQyxDQUFDLEtBQUssR0FBRyxDQUFDLENBQUMsS0FBSyxFQUFDO1NBQ2pDLEdBQUc7Ozs7SUFBQyxLQUFLLENBQUMsRUFBRTtRQUNYLElBQUksS0FBSyxDQUFDLFFBQVEsSUFBSSxLQUFLLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUMzQyxLQUFLLENBQUMsUUFBUSxHQUFHLFVBQVUsQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLENBQUM7U0FDN0M7UUFFRCxPQUFPLEtBQUssQ0FBQztJQUNmLENBQUMsRUFBQyxDQUFDO0FBQ1AsQ0FBQzs7TUFFSyxVQUFVLEdBQUcsbUJBQUEsRUFBRSxFQUFtQjs7Ozs7QUFFeEMsTUFBTSxVQUFVLFlBQVksQ0FBQyxNQUF1QztJQUNsRSxJQUFJLENBQUMsS0FBSyxDQUFDLE9BQU8sQ0FBQyxNQUFNLENBQUMsRUFBRTtRQUMxQixNQUFNLEdBQUcsQ0FBQyxNQUFNLENBQUMsQ0FBQztLQUNuQjtJQUVELFVBQVUsQ0FBQyxJQUFJLENBQUMsR0FBRyxNQUFNLENBQUMsQ0FBQztBQUM3QixDQUFDOzs7O0FBRUQsTUFBTSxVQUFVLFlBQVk7SUFDMUIsT0FBTyxVQUFVLENBQUM7QUFDcEIsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCB9IGZyb20gJy4uL21vZGVscyc7XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gb3JnYW5pemVSb3V0ZXMoXHJcbiAgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10sXHJcbiAgd3JhcHBlcnM6IEFCUC5GdWxsUm91dGVbXSA9IFtdLFxyXG4gIHBhcmVudE5hbWVBcnIgPSBbXSBhcyBBQlAuRnVsbFJvdXRlW10sXHJcbiAgcGFyZW50TmFtZTogc3RyaW5nID0gbnVsbCxcclxuKTogQUJQLkZ1bGxSb3V0ZVtdIHtcclxuICBjb25zdCBmaWx0ZXIgPSByb3V0ZSA9PiB7XHJcbiAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XHJcbiAgICAgIHJvdXRlLmNoaWxkcmVuID0gb3JnYW5pemVSb3V0ZXMocm91dGUuY2hpbGRyZW4sIHdyYXBwZXJzLCBwYXJlbnROYW1lQXJyLCByb3V0ZS5uYW1lKTtcclxuICAgIH1cclxuXHJcbiAgICBpZiAocm91dGUucGFyZW50TmFtZSAmJiByb3V0ZS5wYXJlbnROYW1lICE9PSBwYXJlbnROYW1lKSB7XHJcbiAgICAgIHBhcmVudE5hbWVBcnIucHVzaChyb3V0ZSk7XHJcbiAgICAgIHJldHVybiBmYWxzZTtcclxuICAgIH1cclxuXHJcbiAgICByZXR1cm4gdHJ1ZTtcclxuICB9O1xyXG5cclxuICBpZiAocGFyZW50TmFtZSkge1xyXG4gICAgLy8gcmVjdXJzaXZlIGJsb2NrXHJcbiAgICByZXR1cm4gcm91dGVzLmZpbHRlcihmaWx0ZXIpO1xyXG4gIH1cclxuXHJcbiAgY29uc3QgZmlsdGVyZWRSb3V0ZXMgPSByb3V0ZXMuZmlsdGVyKGZpbHRlcik7XHJcblxyXG4gIGlmIChwYXJlbnROYW1lQXJyLmxlbmd0aCkge1xyXG4gICAgcmV0dXJuIHNvcnRSb3V0ZXMoc2V0Q2hpbGRSb3V0ZShbLi4uZmlsdGVyZWRSb3V0ZXMsIC4uLndyYXBwZXJzXSwgcGFyZW50TmFtZUFycikpO1xyXG4gIH1cclxuXHJcbiAgcmV0dXJuIGZpbHRlcmVkUm91dGVzO1xyXG59XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gc2V0Q2hpbGRSb3V0ZShyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSwgcGFyZW50TmFtZUFycjogQUJQLkZ1bGxSb3V0ZVtdKTogQUJQLkZ1bGxSb3V0ZVtdIHtcclxuICByZXR1cm4gcm91dGVzLm1hcChyb3V0ZSA9PiB7XHJcbiAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XHJcbiAgICAgIHJvdXRlLmNoaWxkcmVuID0gc2V0Q2hpbGRSb3V0ZShyb3V0ZS5jaGlsZHJlbiwgcGFyZW50TmFtZUFycik7XHJcbiAgICB9XHJcblxyXG4gICAgY29uc3QgZm91bmRlZENoaWxkcmVuID0gcGFyZW50TmFtZUFyci5maWx0ZXIocGFyZW50ID0+IHBhcmVudC5wYXJlbnROYW1lID09PSByb3V0ZS5uYW1lKTtcclxuICAgIGlmIChmb3VuZGVkQ2hpbGRyZW4gJiYgZm91bmRlZENoaWxkcmVuLmxlbmd0aCkge1xyXG4gICAgICByb3V0ZS5jaGlsZHJlbiA9IFsuLi4ocm91dGUuY2hpbGRyZW4gfHwgW10pLCAuLi5mb3VuZGVkQ2hpbGRyZW5dO1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiByb3V0ZTtcclxuICB9KTtcclxufVxyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIHNvcnRSb3V0ZXMocm91dGVzOiBBQlAuRnVsbFJvdXRlW10gPSBbXSk6IEFCUC5GdWxsUm91dGVbXSB7XHJcbiAgaWYgKCFyb3V0ZXMubGVuZ3RoKSByZXR1cm4gW107XHJcbiAgcmV0dXJuIHJvdXRlc1xyXG4gICAgLnNvcnQoKGEsIGIpID0+IGEub3JkZXIgLSBiLm9yZGVyKVxyXG4gICAgLm1hcChyb3V0ZSA9PiB7XHJcbiAgICAgIGlmIChyb3V0ZS5jaGlsZHJlbiAmJiByb3V0ZS5jaGlsZHJlbi5sZW5ndGgpIHtcclxuICAgICAgICByb3V0ZS5jaGlsZHJlbiA9IHNvcnRSb3V0ZXMocm91dGUuY2hpbGRyZW4pO1xyXG4gICAgICB9XHJcblxyXG4gICAgICByZXR1cm4gcm91dGU7XHJcbiAgICB9KTtcclxufVxyXG5cclxuY29uc3QgQUJQX1JPVVRFUyA9IFtdIGFzIEFCUC5GdWxsUm91dGVbXTtcclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBhZGRBYnBSb3V0ZXMocm91dGVzOiBBQlAuRnVsbFJvdXRlIHwgQUJQLkZ1bGxSb3V0ZVtdKTogdm9pZCB7XHJcbiAgaWYgKCFBcnJheS5pc0FycmF5KHJvdXRlcykpIHtcclxuICAgIHJvdXRlcyA9IFtyb3V0ZXNdO1xyXG4gIH1cclxuXHJcbiAgQUJQX1JPVVRFUy5wdXNoKC4uLnJvdXRlcyk7XHJcbn1cclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBnZXRBYnBSb3V0ZXMoKTogQUJQLkZ1bGxSb3V0ZVtdIHtcclxuICByZXR1cm4gQUJQX1JPVVRFUztcclxufVxyXG4iXX0=