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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGUtdXRpbHMuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvdXRpbHMvcm91dGUtdXRpbHMudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7O0FBRUEsTUFBTSxVQUFVLGNBQWMsQ0FDNUIsTUFBdUIsRUFDdkIsV0FBNEIsRUFBRSxFQUM5QixhQUFhLEdBQUcsbUJBQUEsRUFBRSxFQUFtQixFQUNyQyxhQUFxQixJQUFJOztVQUVuQixNQUFNOzs7O0lBQUcsS0FBSyxDQUFDLEVBQUU7UUFDckIsSUFBSSxLQUFLLENBQUMsUUFBUSxJQUFJLEtBQUssQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO1lBQzNDLEtBQUssQ0FBQyxRQUFRLEdBQUcsY0FBYyxDQUFDLEtBQUssQ0FBQyxRQUFRLEVBQUUsUUFBUSxFQUFFLGFBQWEsRUFBRSxLQUFLLENBQUMsSUFBSSxDQUFDLENBQUM7U0FDdEY7UUFFRCxJQUFJLEtBQUssQ0FBQyxVQUFVLElBQUksS0FBSyxDQUFDLFVBQVUsS0FBSyxVQUFVLEVBQUU7WUFDdkQsYUFBYSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztZQUMxQixPQUFPLEtBQUssQ0FBQztTQUNkO1FBRUQsT0FBTyxJQUFJLENBQUM7SUFDZCxDQUFDLENBQUE7SUFFRCxJQUFJLFVBQVUsRUFBRTtRQUNkLGtCQUFrQjtRQUNsQixPQUFPLE1BQU0sQ0FBQyxNQUFNLENBQUMsTUFBTSxDQUFDLENBQUM7S0FDOUI7O1VBRUssY0FBYyxHQUFHLE1BQU0sQ0FBQyxNQUFNLENBQUMsTUFBTSxDQUFDO0lBRTVDLElBQUksYUFBYSxDQUFDLE1BQU0sRUFBRTtRQUN4QixPQUFPLFVBQVUsQ0FBQyxhQUFhLENBQUMsQ0FBQyxHQUFHLGNBQWMsRUFBRSxHQUFHLFFBQVEsQ0FBQyxFQUFFLGFBQWEsQ0FBQyxDQUFDLENBQUM7S0FDbkY7SUFFRCxPQUFPLGNBQWMsQ0FBQztBQUN4QixDQUFDOzs7Ozs7QUFFRCxNQUFNLFVBQVUsYUFBYSxDQUFDLE1BQXVCLEVBQUUsYUFBOEI7SUFDbkYsT0FBTyxNQUFNLENBQUMsR0FBRzs7OztJQUFDLEtBQUssQ0FBQyxFQUFFO1FBQ3hCLElBQUksS0FBSyxDQUFDLFFBQVEsSUFBSSxLQUFLLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUMzQyxLQUFLLENBQUMsUUFBUSxHQUFHLGFBQWEsQ0FBQyxLQUFLLENBQUMsUUFBUSxFQUFFLGFBQWEsQ0FBQyxDQUFDO1NBQy9EOztjQUVLLGVBQWUsR0FBRyxhQUFhLENBQUMsTUFBTTs7OztRQUFDLE1BQU0sQ0FBQyxFQUFFLENBQUMsTUFBTSxDQUFDLFVBQVUsS0FBSyxLQUFLLENBQUMsSUFBSSxFQUFDO1FBQ3hGLElBQUksZUFBZSxJQUFJLGVBQWUsQ0FBQyxNQUFNLEVBQUU7WUFDN0MsS0FBSyxDQUFDLFFBQVEsR0FBRyxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUMsUUFBUSxJQUFJLEVBQUUsQ0FBQyxFQUFFLEdBQUcsZUFBZSxDQUFDLENBQUM7U0FDbEU7UUFFRCxPQUFPLEtBQUssQ0FBQztJQUNmLENBQUMsRUFBQyxDQUFDO0FBQ0wsQ0FBQzs7Ozs7QUFFRCxNQUFNLFVBQVUsVUFBVSxDQUFDLFNBQTBCLEVBQUU7SUFDckQsSUFBSSxDQUFDLE1BQU0sQ0FBQyxNQUFNO1FBQUUsT0FBTyxFQUFFLENBQUM7SUFDOUIsT0FBTyxNQUFNO1NBQ1YsR0FBRzs7Ozs7SUFBQyxDQUFDLEtBQUssRUFBRSxLQUFLLEVBQUUsRUFBRTtRQUNwQix5QkFDSyxLQUFLLElBQ1IsS0FBSyxFQUFFLE9BQU8sS0FBSyxDQUFDLEtBQUssS0FBSyxXQUFXLENBQUMsQ0FBQyxDQUFDLEtBQUssR0FBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxLQUFLLElBQ25FO0lBQ0osQ0FBQyxFQUFDO1NBQ0QsSUFBSTs7Ozs7SUFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLEVBQUUsRUFBRSxDQUFDLENBQUMsQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDLEtBQUssRUFBQztTQUNqQyxHQUFHOzs7O0lBQUMsS0FBSyxDQUFDLEVBQUU7UUFDWCxJQUFJLEtBQUssQ0FBQyxRQUFRLElBQUksS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDM0MsS0FBSyxDQUFDLFFBQVEsR0FBRyxVQUFVLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxDQUFDO1NBQzdDO1FBRUQsT0FBTyxLQUFLLENBQUM7SUFDZixDQUFDLEVBQUMsQ0FBQztBQUNQLENBQUM7O01BRUssVUFBVSxHQUFHLG1CQUFBLEVBQUUsRUFBbUI7Ozs7O0FBRXhDLE1BQU0sVUFBVSxZQUFZLENBQUMsTUFBdUM7SUFDbEUsSUFBSSxDQUFDLEtBQUssQ0FBQyxPQUFPLENBQUMsTUFBTSxDQUFDLEVBQUU7UUFDMUIsTUFBTSxHQUFHLENBQUMsTUFBTSxDQUFDLENBQUM7S0FDbkI7SUFFRCxVQUFVLENBQUMsSUFBSSxDQUFDLEdBQUcsTUFBTSxDQUFDLENBQUM7QUFDN0IsQ0FBQzs7OztBQUVELE1BQU0sVUFBVSxZQUFZO0lBQzFCLE9BQU8sVUFBVSxDQUFDO0FBQ3BCLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBQlAgfSBmcm9tICcuLi9tb2RlbHMnO1xyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIG9yZ2FuaXplUm91dGVzKFxyXG4gIHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdLFxyXG4gIHdyYXBwZXJzOiBBQlAuRnVsbFJvdXRlW10gPSBbXSxcclxuICBwYXJlbnROYW1lQXJyID0gW10gYXMgQUJQLkZ1bGxSb3V0ZVtdLFxyXG4gIHBhcmVudE5hbWU6IHN0cmluZyA9IG51bGwsXHJcbik6IEFCUC5GdWxsUm91dGVbXSB7XHJcbiAgY29uc3QgZmlsdGVyID0gcm91dGUgPT4ge1xyXG4gICAgaWYgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCkge1xyXG4gICAgICByb3V0ZS5jaGlsZHJlbiA9IG9yZ2FuaXplUm91dGVzKHJvdXRlLmNoaWxkcmVuLCB3cmFwcGVycywgcGFyZW50TmFtZUFyciwgcm91dGUubmFtZSk7XHJcbiAgICB9XHJcblxyXG4gICAgaWYgKHJvdXRlLnBhcmVudE5hbWUgJiYgcm91dGUucGFyZW50TmFtZSAhPT0gcGFyZW50TmFtZSkge1xyXG4gICAgICBwYXJlbnROYW1lQXJyLnB1c2gocm91dGUpO1xyXG4gICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICB9XHJcblxyXG4gICAgcmV0dXJuIHRydWU7XHJcbiAgfTtcclxuXHJcbiAgaWYgKHBhcmVudE5hbWUpIHtcclxuICAgIC8vIHJlY3Vyc2l2ZSBibG9ja1xyXG4gICAgcmV0dXJuIHJvdXRlcy5maWx0ZXIoZmlsdGVyKTtcclxuICB9XHJcblxyXG4gIGNvbnN0IGZpbHRlcmVkUm91dGVzID0gcm91dGVzLmZpbHRlcihmaWx0ZXIpO1xyXG5cclxuICBpZiAocGFyZW50TmFtZUFyci5sZW5ndGgpIHtcclxuICAgIHJldHVybiBzb3J0Um91dGVzKHNldENoaWxkUm91dGUoWy4uLmZpbHRlcmVkUm91dGVzLCAuLi53cmFwcGVyc10sIHBhcmVudE5hbWVBcnIpKTtcclxuICB9XHJcblxyXG4gIHJldHVybiBmaWx0ZXJlZFJvdXRlcztcclxufVxyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIHNldENoaWxkUm91dGUocm91dGVzOiBBQlAuRnVsbFJvdXRlW10sIHBhcmVudE5hbWVBcnI6IEFCUC5GdWxsUm91dGVbXSk6IEFCUC5GdWxsUm91dGVbXSB7XHJcbiAgcmV0dXJuIHJvdXRlcy5tYXAocm91dGUgPT4ge1xyXG4gICAgaWYgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCkge1xyXG4gICAgICByb3V0ZS5jaGlsZHJlbiA9IHNldENoaWxkUm91dGUocm91dGUuY2hpbGRyZW4sIHBhcmVudE5hbWVBcnIpO1xyXG4gICAgfVxyXG5cclxuICAgIGNvbnN0IGZvdW5kZWRDaGlsZHJlbiA9IHBhcmVudE5hbWVBcnIuZmlsdGVyKHBhcmVudCA9PiBwYXJlbnQucGFyZW50TmFtZSA9PT0gcm91dGUubmFtZSk7XHJcbiAgICBpZiAoZm91bmRlZENoaWxkcmVuICYmIGZvdW5kZWRDaGlsZHJlbi5sZW5ndGgpIHtcclxuICAgICAgcm91dGUuY2hpbGRyZW4gPSBbLi4uKHJvdXRlLmNoaWxkcmVuIHx8IFtdKSwgLi4uZm91bmRlZENoaWxkcmVuXTtcclxuICAgIH1cclxuXHJcbiAgICByZXR1cm4gcm91dGU7XHJcbiAgfSk7XHJcbn1cclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBzb3J0Um91dGVzKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdID0gW10pOiBBQlAuRnVsbFJvdXRlW10ge1xyXG4gIGlmICghcm91dGVzLmxlbmd0aCkgcmV0dXJuIFtdO1xyXG4gIHJldHVybiByb3V0ZXNcclxuICAgIC5tYXAoKHJvdXRlLCBpbmRleCkgPT4ge1xyXG4gICAgICByZXR1cm4ge1xyXG4gICAgICAgIC4uLnJvdXRlLFxyXG4gICAgICAgIG9yZGVyOiB0eXBlb2Ygcm91dGUub3JkZXIgPT09ICd1bmRlZmluZWQnID8gaW5kZXggKyAxIDogcm91dGUub3JkZXIsXHJcbiAgICAgIH07XHJcbiAgICB9KVxyXG4gICAgLnNvcnQoKGEsIGIpID0+IGEub3JkZXIgLSBiLm9yZGVyKVxyXG4gICAgLm1hcChyb3V0ZSA9PiB7XHJcbiAgICAgIGlmIChyb3V0ZS5jaGlsZHJlbiAmJiByb3V0ZS5jaGlsZHJlbi5sZW5ndGgpIHtcclxuICAgICAgICByb3V0ZS5jaGlsZHJlbiA9IHNvcnRSb3V0ZXMocm91dGUuY2hpbGRyZW4pO1xyXG4gICAgICB9XHJcblxyXG4gICAgICByZXR1cm4gcm91dGU7XHJcbiAgICB9KTtcclxufVxyXG5cclxuY29uc3QgQUJQX1JPVVRFUyA9IFtdIGFzIEFCUC5GdWxsUm91dGVbXTtcclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBhZGRBYnBSb3V0ZXMocm91dGVzOiBBQlAuRnVsbFJvdXRlIHwgQUJQLkZ1bGxSb3V0ZVtdKTogdm9pZCB7XHJcbiAgaWYgKCFBcnJheS5pc0FycmF5KHJvdXRlcykpIHtcclxuICAgIHJvdXRlcyA9IFtyb3V0ZXNdO1xyXG4gIH1cclxuXHJcbiAgQUJQX1JPVVRFUy5wdXNoKC4uLnJvdXRlcyk7XHJcbn1cclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBnZXRBYnBSb3V0ZXMoKTogQUJQLkZ1bGxSb3V0ZVtdIHtcclxuICByZXR1cm4gQUJQX1JPVVRFUztcclxufVxyXG4iXX0=