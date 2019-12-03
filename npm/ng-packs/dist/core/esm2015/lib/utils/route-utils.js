/**
 * @fileoverview added by tsickle
 * Generated from: lib/utils/route-utils.ts
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
        .map((/**
     * @param {?} route
     * @param {?} index
     * @return {?}
     */
    (route, index) => {
        return Object.assign({}, route, { order: typeof route.order === 'undefined' ? index + 1 : route.order });
    }))
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGUtdXRpbHMuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvdXRpbHMvcm91dGUtdXRpbHMudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7O0FBRUEsTUFBTSxVQUFVLGNBQWMsQ0FDNUIsTUFBdUIsRUFDdkIsV0FBNEIsRUFBRSxFQUM5QixhQUFhLEdBQUcsbUJBQUEsRUFBRSxFQUFtQixFQUNyQyxhQUFxQixJQUFJOztVQUVuQixNQUFNOzs7O0lBQUcsS0FBSyxDQUFDLEVBQUU7UUFDckIsSUFBSSxLQUFLLENBQUMsUUFBUSxJQUFJLEtBQUssQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO1lBQzNDLEtBQUssQ0FBQyxRQUFRLEdBQUcsY0FBYyxDQUFDLEtBQUssQ0FBQyxRQUFRLEVBQUUsUUFBUSxFQUFFLGFBQWEsRUFBRSxLQUFLLENBQUMsSUFBSSxDQUFDLENBQUM7U0FDdEY7UUFFRCxJQUFJLEtBQUssQ0FBQyxVQUFVLElBQUksS0FBSyxDQUFDLFVBQVUsS0FBSyxVQUFVLEVBQUU7WUFDdkQsYUFBYSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztZQUMxQixPQUFPLEtBQUssQ0FBQztTQUNkO1FBRUQsT0FBTyxJQUFJLENBQUM7SUFDZCxDQUFDLENBQUE7SUFFRCxJQUFJLFVBQVUsRUFBRTtRQUNkLGtCQUFrQjtRQUNsQixPQUFPLE1BQU0sQ0FBQyxNQUFNLENBQUMsTUFBTSxDQUFDLENBQUM7S0FDOUI7O1VBRUssY0FBYyxHQUFHLE1BQU0sQ0FBQyxNQUFNLENBQUMsTUFBTSxDQUFDO0lBRTVDLElBQUksYUFBYSxDQUFDLE1BQU0sRUFBRTtRQUN4QixPQUFPLFVBQVUsQ0FBQyxhQUFhLENBQUMsQ0FBQyxHQUFHLGNBQWMsRUFBRSxHQUFHLFFBQVEsQ0FBQyxFQUFFLGFBQWEsQ0FBQyxDQUFDLENBQUM7S0FDbkY7SUFFRCxPQUFPLGNBQWMsQ0FBQztBQUN4QixDQUFDOzs7Ozs7QUFFRCxNQUFNLFVBQVUsYUFBYSxDQUFDLE1BQXVCLEVBQUUsYUFBOEI7SUFDbkYsT0FBTyxNQUFNLENBQUMsR0FBRzs7OztJQUFDLEtBQUssQ0FBQyxFQUFFO1FBQ3hCLElBQUksS0FBSyxDQUFDLFFBQVEsSUFBSSxLQUFLLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUMzQyxLQUFLLENBQUMsUUFBUSxHQUFHLGFBQWEsQ0FBQyxLQUFLLENBQUMsUUFBUSxFQUFFLGFBQWEsQ0FBQyxDQUFDO1NBQy9EOztjQUVLLGVBQWUsR0FBRyxhQUFhLENBQUMsTUFBTTs7OztRQUFDLE1BQU0sQ0FBQyxFQUFFLENBQUMsTUFBTSxDQUFDLFVBQVUsS0FBSyxLQUFLLENBQUMsSUFBSSxFQUFDO1FBQ3hGLElBQUksZUFBZSxJQUFJLGVBQWUsQ0FBQyxNQUFNLEVBQUU7WUFDN0MsS0FBSyxDQUFDLFFBQVEsR0FBRyxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUMsUUFBUSxJQUFJLEVBQUUsQ0FBQyxFQUFFLEdBQUcsZUFBZSxDQUFDLENBQUM7U0FDbEU7UUFFRCxPQUFPLEtBQUssQ0FBQztJQUNmLENBQUMsRUFBQyxDQUFDO0FBQ0wsQ0FBQzs7Ozs7QUFFRCxNQUFNLFVBQVUsVUFBVSxDQUFDLFNBQTBCLEVBQUU7SUFDckQsSUFBSSxDQUFDLE1BQU0sQ0FBQyxNQUFNO1FBQUUsT0FBTyxFQUFFLENBQUM7SUFDOUIsT0FBTyxNQUFNO1NBQ1YsR0FBRzs7Ozs7SUFBQyxDQUFDLEtBQUssRUFBRSxLQUFLLEVBQUUsRUFBRTtRQUNwQix5QkFDSyxLQUFLLElBQ1IsS0FBSyxFQUFFLE9BQU8sS0FBSyxDQUFDLEtBQUssS0FBSyxXQUFXLENBQUMsQ0FBQyxDQUFDLEtBQUssR0FBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxLQUFLLElBQ25FO0lBQ0osQ0FBQyxFQUFDO1NBQ0QsSUFBSTs7Ozs7SUFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLEVBQUUsRUFBRSxDQUFDLENBQUMsQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDLEtBQUssRUFBQztTQUNqQyxHQUFHOzs7O0lBQUMsS0FBSyxDQUFDLEVBQUU7UUFDWCxJQUFJLEtBQUssQ0FBQyxRQUFRLElBQUksS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDM0MsS0FBSyxDQUFDLFFBQVEsR0FBRyxVQUFVLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxDQUFDO1NBQzdDO1FBRUQsT0FBTyxLQUFLLENBQUM7SUFDZixDQUFDLEVBQUMsQ0FBQztBQUNQLENBQUM7O01BRUssVUFBVSxHQUFHLG1CQUFBLEVBQUUsRUFBbUI7Ozs7O0FBRXhDLE1BQU0sVUFBVSxZQUFZLENBQUMsTUFBdUM7SUFDbEUsSUFBSSxDQUFDLEtBQUssQ0FBQyxPQUFPLENBQUMsTUFBTSxDQUFDLEVBQUU7UUFDMUIsTUFBTSxHQUFHLENBQUMsTUFBTSxDQUFDLENBQUM7S0FDbkI7SUFFRCxVQUFVLENBQUMsSUFBSSxDQUFDLEdBQUcsTUFBTSxDQUFDLENBQUM7QUFDN0IsQ0FBQzs7OztBQUVELE1BQU0sVUFBVSxZQUFZO0lBQzFCLE9BQU8sVUFBVSxDQUFDO0FBQ3BCLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBQlAgfSBmcm9tICcuLi9tb2RlbHMnO1xuXG5leHBvcnQgZnVuY3Rpb24gb3JnYW5pemVSb3V0ZXMoXG4gIHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdLFxuICB3cmFwcGVyczogQUJQLkZ1bGxSb3V0ZVtdID0gW10sXG4gIHBhcmVudE5hbWVBcnIgPSBbXSBhcyBBQlAuRnVsbFJvdXRlW10sXG4gIHBhcmVudE5hbWU6IHN0cmluZyA9IG51bGwsXG4pOiBBQlAuRnVsbFJvdXRlW10ge1xuICBjb25zdCBmaWx0ZXIgPSByb3V0ZSA9PiB7XG4gICAgaWYgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgcm91dGUuY2hpbGRyZW4gPSBvcmdhbml6ZVJvdXRlcyhyb3V0ZS5jaGlsZHJlbiwgd3JhcHBlcnMsIHBhcmVudE5hbWVBcnIsIHJvdXRlLm5hbWUpO1xuICAgIH1cblxuICAgIGlmIChyb3V0ZS5wYXJlbnROYW1lICYmIHJvdXRlLnBhcmVudE5hbWUgIT09IHBhcmVudE5hbWUpIHtcbiAgICAgIHBhcmVudE5hbWVBcnIucHVzaChyb3V0ZSk7XG4gICAgICByZXR1cm4gZmFsc2U7XG4gICAgfVxuXG4gICAgcmV0dXJuIHRydWU7XG4gIH07XG5cbiAgaWYgKHBhcmVudE5hbWUpIHtcbiAgICAvLyByZWN1cnNpdmUgYmxvY2tcbiAgICByZXR1cm4gcm91dGVzLmZpbHRlcihmaWx0ZXIpO1xuICB9XG5cbiAgY29uc3QgZmlsdGVyZWRSb3V0ZXMgPSByb3V0ZXMuZmlsdGVyKGZpbHRlcik7XG5cbiAgaWYgKHBhcmVudE5hbWVBcnIubGVuZ3RoKSB7XG4gICAgcmV0dXJuIHNvcnRSb3V0ZXMoc2V0Q2hpbGRSb3V0ZShbLi4uZmlsdGVyZWRSb3V0ZXMsIC4uLndyYXBwZXJzXSwgcGFyZW50TmFtZUFycikpO1xuICB9XG5cbiAgcmV0dXJuIGZpbHRlcmVkUm91dGVzO1xufVxuXG5leHBvcnQgZnVuY3Rpb24gc2V0Q2hpbGRSb3V0ZShyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSwgcGFyZW50TmFtZUFycjogQUJQLkZ1bGxSb3V0ZVtdKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgcmV0dXJuIHJvdXRlcy5tYXAocm91dGUgPT4ge1xuICAgIGlmIChyb3V0ZS5jaGlsZHJlbiAmJiByb3V0ZS5jaGlsZHJlbi5sZW5ndGgpIHtcbiAgICAgIHJvdXRlLmNoaWxkcmVuID0gc2V0Q2hpbGRSb3V0ZShyb3V0ZS5jaGlsZHJlbiwgcGFyZW50TmFtZUFycik7XG4gICAgfVxuXG4gICAgY29uc3QgZm91bmRlZENoaWxkcmVuID0gcGFyZW50TmFtZUFyci5maWx0ZXIocGFyZW50ID0+IHBhcmVudC5wYXJlbnROYW1lID09PSByb3V0ZS5uYW1lKTtcbiAgICBpZiAoZm91bmRlZENoaWxkcmVuICYmIGZvdW5kZWRDaGlsZHJlbi5sZW5ndGgpIHtcbiAgICAgIHJvdXRlLmNoaWxkcmVuID0gWy4uLihyb3V0ZS5jaGlsZHJlbiB8fCBbXSksIC4uLmZvdW5kZWRDaGlsZHJlbl07XG4gICAgfVxuXG4gICAgcmV0dXJuIHJvdXRlO1xuICB9KTtcbn1cblxuZXhwb3J0IGZ1bmN0aW9uIHNvcnRSb3V0ZXMocm91dGVzOiBBQlAuRnVsbFJvdXRlW10gPSBbXSk6IEFCUC5GdWxsUm91dGVbXSB7XG4gIGlmICghcm91dGVzLmxlbmd0aCkgcmV0dXJuIFtdO1xuICByZXR1cm4gcm91dGVzXG4gICAgLm1hcCgocm91dGUsIGluZGV4KSA9PiB7XG4gICAgICByZXR1cm4ge1xuICAgICAgICAuLi5yb3V0ZSxcbiAgICAgICAgb3JkZXI6IHR5cGVvZiByb3V0ZS5vcmRlciA9PT0gJ3VuZGVmaW5lZCcgPyBpbmRleCArIDEgOiByb3V0ZS5vcmRlcixcbiAgICAgIH07XG4gICAgfSlcbiAgICAuc29ydCgoYSwgYikgPT4gYS5vcmRlciAtIGIub3JkZXIpXG4gICAgLm1hcChyb3V0ZSA9PiB7XG4gICAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XG4gICAgICAgIHJvdXRlLmNoaWxkcmVuID0gc29ydFJvdXRlcyhyb3V0ZS5jaGlsZHJlbik7XG4gICAgICB9XG5cbiAgICAgIHJldHVybiByb3V0ZTtcbiAgICB9KTtcbn1cblxuY29uc3QgQUJQX1JPVVRFUyA9IFtdIGFzIEFCUC5GdWxsUm91dGVbXTtcblxuZXhwb3J0IGZ1bmN0aW9uIGFkZEFicFJvdXRlcyhyb3V0ZXM6IEFCUC5GdWxsUm91dGUgfCBBQlAuRnVsbFJvdXRlW10pOiB2b2lkIHtcbiAgaWYgKCFBcnJheS5pc0FycmF5KHJvdXRlcykpIHtcbiAgICByb3V0ZXMgPSBbcm91dGVzXTtcbiAgfVxuXG4gIEFCUF9ST1VURVMucHVzaCguLi5yb3V0ZXMpO1xufVxuXG5leHBvcnQgZnVuY3Rpb24gZ2V0QWJwUm91dGVzKCk6IEFCUC5GdWxsUm91dGVbXSB7XG4gIHJldHVybiBBQlBfUk9VVEVTO1xufVxuIl19