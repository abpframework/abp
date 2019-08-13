/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
    return routes
        .map((/**
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
    }))
        .filter((/**
     * @param {?} route
     * @return {?}
     */
    route => route.path || (route.children && route.children.length)));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGUtdXRpbHMuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvdXRpbHMvcm91dGUtdXRpbHMudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7QUFFQSxNQUFNLFVBQVUsY0FBYyxDQUM1QixNQUF1QixFQUN2QixXQUE0QixFQUFFLEVBQzlCLGFBQWEsR0FBRyxtQkFBQSxFQUFFLEVBQW1CLEVBQ3JDLGFBQXFCLElBQUk7O1VBRW5CLE1BQU07Ozs7SUFBRyxLQUFLLENBQUMsRUFBRTtRQUNyQixJQUFJLEtBQUssQ0FBQyxRQUFRLEVBQUU7WUFDbEIsS0FBSyxDQUFDLFFBQVEsR0FBRyxjQUFjLENBQUMsS0FBSyxDQUFDLFFBQVEsRUFBRSxRQUFRLEVBQUUsYUFBYSxFQUFFLEtBQUssQ0FBQyxJQUFJLENBQUMsQ0FBQztTQUN0RjtRQUVELElBQUksS0FBSyxDQUFDLFVBQVUsSUFBSSxLQUFLLENBQUMsVUFBVSxLQUFLLFVBQVUsRUFBRTtZQUN2RCxhQUFhLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO1lBQzFCLE9BQU8sS0FBSyxDQUFDO1NBQ2Q7UUFFRCxPQUFPLElBQUksQ0FBQztJQUNkLENBQUMsQ0FBQTtJQUVELElBQUksVUFBVSxFQUFFO1FBQ2Qsa0JBQWtCO1FBQ2xCLE9BQU8sTUFBTSxDQUFDLE1BQU0sQ0FBQyxNQUFNLENBQUMsQ0FBQztLQUM5Qjs7VUFFSyxjQUFjLEdBQUcsTUFBTSxDQUFDLE1BQU0sQ0FBQyxNQUFNLENBQUM7SUFFNUMsSUFBSSxhQUFhLENBQUMsTUFBTSxFQUFFO1FBQ3hCLE9BQU8sVUFBVSxDQUFDLGFBQWEsQ0FBQyxDQUFDLEdBQUcsY0FBYyxFQUFFLEdBQUcsUUFBUSxDQUFDLEVBQUUsYUFBYSxDQUFDLENBQUMsQ0FBQztLQUNuRjtJQUVELE9BQU8sY0FBYyxDQUFDO0FBQ3hCLENBQUM7Ozs7OztBQUVELE1BQU0sVUFBVSxhQUFhLENBQUMsTUFBdUIsRUFBRSxhQUE4QjtJQUNuRixPQUFPLE1BQU07U0FDVixHQUFHOzs7O0lBQUMsS0FBSyxDQUFDLEVBQUU7UUFDWCxJQUFJLEtBQUssQ0FBQyxRQUFRLElBQUksS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDM0MsS0FBSyxDQUFDLFFBQVEsR0FBRyxhQUFhLENBQUMsS0FBSyxDQUFDLFFBQVEsRUFBRSxhQUFhLENBQUMsQ0FBQztTQUMvRDs7Y0FFSyxlQUFlLEdBQUcsYUFBYSxDQUFDLE1BQU07Ozs7UUFBQyxNQUFNLENBQUMsRUFBRSxDQUFDLE1BQU0sQ0FBQyxVQUFVLEtBQUssS0FBSyxDQUFDLElBQUksRUFBQztRQUN4RixJQUFJLGVBQWUsSUFBSSxlQUFlLENBQUMsTUFBTSxFQUFFO1lBQzdDLEtBQUssQ0FBQyxRQUFRLEdBQUcsQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDLFFBQVEsSUFBSSxFQUFFLENBQUMsRUFBRSxHQUFHLGVBQWUsQ0FBQyxDQUFDO1NBQ2xFO1FBRUQsT0FBTyxLQUFLLENBQUM7SUFDZixDQUFDLEVBQUM7U0FDRCxNQUFNOzs7O0lBQUMsS0FBSyxDQUFDLEVBQUUsQ0FBQyxLQUFLLENBQUMsSUFBSSxJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsSUFBSSxLQUFLLENBQUMsUUFBUSxDQUFDLE1BQU0sQ0FBQyxFQUFDLENBQUM7QUFDOUUsQ0FBQzs7Ozs7QUFFRCxNQUFNLFVBQVUsVUFBVSxDQUFDLFNBQTBCLEVBQUU7SUFDckQsSUFBSSxDQUFDLE1BQU0sQ0FBQyxNQUFNO1FBQUUsT0FBTyxFQUFFLENBQUM7SUFDOUIsT0FBTyxNQUFNO1NBQ1YsSUFBSTs7Ozs7SUFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLEVBQUUsRUFBRSxDQUFDLENBQUMsQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDLEtBQUssRUFBQztTQUNqQyxHQUFHOzs7O0lBQUMsS0FBSyxDQUFDLEVBQUU7UUFDWCxJQUFJLEtBQUssQ0FBQyxRQUFRLElBQUksS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDM0MsS0FBSyxDQUFDLFFBQVEsR0FBRyxVQUFVLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxDQUFDO1NBQzdDO1FBRUQsT0FBTyxLQUFLLENBQUM7SUFDZixDQUFDLEVBQUMsQ0FBQztBQUNQLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBQlAgfSBmcm9tICcuLi9tb2RlbHMnO1xuXG5leHBvcnQgZnVuY3Rpb24gb3JnYW5pemVSb3V0ZXMoXG4gIHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdLFxuICB3cmFwcGVyczogQUJQLkZ1bGxSb3V0ZVtdID0gW10sXG4gIHBhcmVudE5hbWVBcnIgPSBbXSBhcyBBQlAuRnVsbFJvdXRlW10sXG4gIHBhcmVudE5hbWU6IHN0cmluZyA9IG51bGwsXG4pOiBBQlAuRnVsbFJvdXRlW10ge1xuICBjb25zdCBmaWx0ZXIgPSByb3V0ZSA9PiB7XG4gICAgaWYgKHJvdXRlLmNoaWxkcmVuKSB7XG4gICAgICByb3V0ZS5jaGlsZHJlbiA9IG9yZ2FuaXplUm91dGVzKHJvdXRlLmNoaWxkcmVuLCB3cmFwcGVycywgcGFyZW50TmFtZUFyciwgcm91dGUubmFtZSk7XG4gICAgfVxuXG4gICAgaWYgKHJvdXRlLnBhcmVudE5hbWUgJiYgcm91dGUucGFyZW50TmFtZSAhPT0gcGFyZW50TmFtZSkge1xuICAgICAgcGFyZW50TmFtZUFyci5wdXNoKHJvdXRlKTtcbiAgICAgIHJldHVybiBmYWxzZTtcbiAgICB9XG5cbiAgICByZXR1cm4gdHJ1ZTtcbiAgfTtcblxuICBpZiAocGFyZW50TmFtZSkge1xuICAgIC8vIHJlY3Vyc2l2ZSBibG9ja1xuICAgIHJldHVybiByb3V0ZXMuZmlsdGVyKGZpbHRlcik7XG4gIH1cblxuICBjb25zdCBmaWx0ZXJlZFJvdXRlcyA9IHJvdXRlcy5maWx0ZXIoZmlsdGVyKTtcblxuICBpZiAocGFyZW50TmFtZUFyci5sZW5ndGgpIHtcbiAgICByZXR1cm4gc29ydFJvdXRlcyhzZXRDaGlsZFJvdXRlKFsuLi5maWx0ZXJlZFJvdXRlcywgLi4ud3JhcHBlcnNdLCBwYXJlbnROYW1lQXJyKSk7XG4gIH1cblxuICByZXR1cm4gZmlsdGVyZWRSb3V0ZXM7XG59XG5cbmV4cG9ydCBmdW5jdGlvbiBzZXRDaGlsZFJvdXRlKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdLCBwYXJlbnROYW1lQXJyOiBBQlAuRnVsbFJvdXRlW10pOiBBQlAuRnVsbFJvdXRlW10ge1xuICByZXR1cm4gcm91dGVzXG4gICAgLm1hcChyb3V0ZSA9PiB7XG4gICAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XG4gICAgICAgIHJvdXRlLmNoaWxkcmVuID0gc2V0Q2hpbGRSb3V0ZShyb3V0ZS5jaGlsZHJlbiwgcGFyZW50TmFtZUFycik7XG4gICAgICB9XG5cbiAgICAgIGNvbnN0IGZvdW5kZWRDaGlsZHJlbiA9IHBhcmVudE5hbWVBcnIuZmlsdGVyKHBhcmVudCA9PiBwYXJlbnQucGFyZW50TmFtZSA9PT0gcm91dGUubmFtZSk7XG4gICAgICBpZiAoZm91bmRlZENoaWxkcmVuICYmIGZvdW5kZWRDaGlsZHJlbi5sZW5ndGgpIHtcbiAgICAgICAgcm91dGUuY2hpbGRyZW4gPSBbLi4uKHJvdXRlLmNoaWxkcmVuIHx8IFtdKSwgLi4uZm91bmRlZENoaWxkcmVuXTtcbiAgICAgIH1cblxuICAgICAgcmV0dXJuIHJvdXRlO1xuICAgIH0pXG4gICAgLmZpbHRlcihyb3V0ZSA9PiByb3V0ZS5wYXRoIHx8IChyb3V0ZS5jaGlsZHJlbiAmJiByb3V0ZS5jaGlsZHJlbi5sZW5ndGgpKTtcbn1cblxuZXhwb3J0IGZ1bmN0aW9uIHNvcnRSb3V0ZXMocm91dGVzOiBBQlAuRnVsbFJvdXRlW10gPSBbXSk6IEFCUC5GdWxsUm91dGVbXSB7XG4gIGlmICghcm91dGVzLmxlbmd0aCkgcmV0dXJuIFtdO1xuICByZXR1cm4gcm91dGVzXG4gICAgLnNvcnQoKGEsIGIpID0+IGEub3JkZXIgLSBiLm9yZGVyKVxuICAgIC5tYXAocm91dGUgPT4ge1xuICAgICAgaWYgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgICByb3V0ZS5jaGlsZHJlbiA9IHNvcnRSb3V0ZXMocm91dGUuY2hpbGRyZW4pO1xuICAgICAgfVxuXG4gICAgICByZXR1cm4gcm91dGU7XG4gICAgfSk7XG59XG4iXX0=