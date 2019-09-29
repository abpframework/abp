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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGUtdXRpbHMuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvdXRpbHMvcm91dGUtdXRpbHMudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7QUFFQSxNQUFNLFVBQVUsY0FBYyxDQUM1QixNQUF1QixFQUN2QixXQUE0QixFQUFFLEVBQzlCLGFBQWEsR0FBRyxtQkFBQSxFQUFFLEVBQW1CLEVBQ3JDLGFBQXFCLElBQUk7O1VBRW5CLE1BQU07Ozs7SUFBRyxLQUFLLENBQUMsRUFBRTtRQUNyQixJQUFJLEtBQUssQ0FBQyxRQUFRLElBQUksS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDM0MsS0FBSyxDQUFDLFFBQVEsR0FBRyxjQUFjLENBQUMsS0FBSyxDQUFDLFFBQVEsRUFBRSxRQUFRLEVBQUUsYUFBYSxFQUFFLEtBQUssQ0FBQyxJQUFJLENBQUMsQ0FBQztTQUN0RjtRQUVELElBQUksS0FBSyxDQUFDLFVBQVUsSUFBSSxLQUFLLENBQUMsVUFBVSxLQUFLLFVBQVUsRUFBRTtZQUN2RCxhQUFhLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO1lBQzFCLE9BQU8sS0FBSyxDQUFDO1NBQ2Q7UUFFRCxPQUFPLElBQUksQ0FBQztJQUNkLENBQUMsQ0FBQTtJQUVELElBQUksVUFBVSxFQUFFO1FBQ2Qsa0JBQWtCO1FBQ2xCLE9BQU8sTUFBTSxDQUFDLE1BQU0sQ0FBQyxNQUFNLENBQUMsQ0FBQztLQUM5Qjs7VUFFSyxjQUFjLEdBQUcsTUFBTSxDQUFDLE1BQU0sQ0FBQyxNQUFNLENBQUM7SUFFNUMsSUFBSSxhQUFhLENBQUMsTUFBTSxFQUFFO1FBQ3hCLE9BQU8sVUFBVSxDQUFDLGFBQWEsQ0FBQyxDQUFDLEdBQUcsY0FBYyxFQUFFLEdBQUcsUUFBUSxDQUFDLEVBQUUsYUFBYSxDQUFDLENBQUMsQ0FBQztLQUNuRjtJQUVELE9BQU8sY0FBYyxDQUFDO0FBQ3hCLENBQUM7Ozs7OztBQUVELE1BQU0sVUFBVSxhQUFhLENBQUMsTUFBdUIsRUFBRSxhQUE4QjtJQUNuRixPQUFPLE1BQU0sQ0FBQyxHQUFHOzs7O0lBQUMsS0FBSyxDQUFDLEVBQUU7UUFDeEIsSUFBSSxLQUFLLENBQUMsUUFBUSxJQUFJLEtBQUssQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO1lBQzNDLEtBQUssQ0FBQyxRQUFRLEdBQUcsYUFBYSxDQUFDLEtBQUssQ0FBQyxRQUFRLEVBQUUsYUFBYSxDQUFDLENBQUM7U0FDL0Q7O2NBRUssZUFBZSxHQUFHLGFBQWEsQ0FBQyxNQUFNOzs7O1FBQUMsTUFBTSxDQUFDLEVBQUUsQ0FBQyxNQUFNLENBQUMsVUFBVSxLQUFLLEtBQUssQ0FBQyxJQUFJLEVBQUM7UUFDeEYsSUFBSSxlQUFlLElBQUksZUFBZSxDQUFDLE1BQU0sRUFBRTtZQUM3QyxLQUFLLENBQUMsUUFBUSxHQUFHLENBQUMsR0FBRyxDQUFDLEtBQUssQ0FBQyxRQUFRLElBQUksRUFBRSxDQUFDLEVBQUUsR0FBRyxlQUFlLENBQUMsQ0FBQztTQUNsRTtRQUVELE9BQU8sS0FBSyxDQUFDO0lBQ2YsQ0FBQyxFQUFDLENBQUM7QUFDTCxDQUFDOzs7OztBQUVELE1BQU0sVUFBVSxVQUFVLENBQUMsU0FBMEIsRUFBRTtJQUNyRCxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU07UUFBRSxPQUFPLEVBQUUsQ0FBQztJQUM5QixPQUFPLE1BQU07U0FDVixJQUFJOzs7OztJQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsRUFBRSxFQUFFLENBQUMsQ0FBQyxDQUFDLEtBQUssR0FBRyxDQUFDLENBQUMsS0FBSyxFQUFDO1NBQ2pDLEdBQUc7Ozs7SUFBQyxLQUFLLENBQUMsRUFBRTtRQUNYLElBQUksS0FBSyxDQUFDLFFBQVEsSUFBSSxLQUFLLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUMzQyxLQUFLLENBQUMsUUFBUSxHQUFHLFVBQVUsQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLENBQUM7U0FDN0M7UUFFRCxPQUFPLEtBQUssQ0FBQztJQUNmLENBQUMsRUFBQyxDQUFDO0FBQ1AsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCB9IGZyb20gJy4uL21vZGVscyc7XG5cbmV4cG9ydCBmdW5jdGlvbiBvcmdhbml6ZVJvdXRlcyhcbiAgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10sXG4gIHdyYXBwZXJzOiBBQlAuRnVsbFJvdXRlW10gPSBbXSxcbiAgcGFyZW50TmFtZUFyciA9IFtdIGFzIEFCUC5GdWxsUm91dGVbXSxcbiAgcGFyZW50TmFtZTogc3RyaW5nID0gbnVsbCxcbik6IEFCUC5GdWxsUm91dGVbXSB7XG4gIGNvbnN0IGZpbHRlciA9IHJvdXRlID0+IHtcbiAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XG4gICAgICByb3V0ZS5jaGlsZHJlbiA9IG9yZ2FuaXplUm91dGVzKHJvdXRlLmNoaWxkcmVuLCB3cmFwcGVycywgcGFyZW50TmFtZUFyciwgcm91dGUubmFtZSk7XG4gICAgfVxuXG4gICAgaWYgKHJvdXRlLnBhcmVudE5hbWUgJiYgcm91dGUucGFyZW50TmFtZSAhPT0gcGFyZW50TmFtZSkge1xuICAgICAgcGFyZW50TmFtZUFyci5wdXNoKHJvdXRlKTtcbiAgICAgIHJldHVybiBmYWxzZTtcbiAgICB9XG5cbiAgICByZXR1cm4gdHJ1ZTtcbiAgfTtcblxuICBpZiAocGFyZW50TmFtZSkge1xuICAgIC8vIHJlY3Vyc2l2ZSBibG9ja1xuICAgIHJldHVybiByb3V0ZXMuZmlsdGVyKGZpbHRlcik7XG4gIH1cblxuICBjb25zdCBmaWx0ZXJlZFJvdXRlcyA9IHJvdXRlcy5maWx0ZXIoZmlsdGVyKTtcblxuICBpZiAocGFyZW50TmFtZUFyci5sZW5ndGgpIHtcbiAgICByZXR1cm4gc29ydFJvdXRlcyhzZXRDaGlsZFJvdXRlKFsuLi5maWx0ZXJlZFJvdXRlcywgLi4ud3JhcHBlcnNdLCBwYXJlbnROYW1lQXJyKSk7XG4gIH1cblxuICByZXR1cm4gZmlsdGVyZWRSb3V0ZXM7XG59XG5cbmV4cG9ydCBmdW5jdGlvbiBzZXRDaGlsZFJvdXRlKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdLCBwYXJlbnROYW1lQXJyOiBBQlAuRnVsbFJvdXRlW10pOiBBQlAuRnVsbFJvdXRlW10ge1xuICByZXR1cm4gcm91dGVzLm1hcChyb3V0ZSA9PiB7XG4gICAgaWYgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgcm91dGUuY2hpbGRyZW4gPSBzZXRDaGlsZFJvdXRlKHJvdXRlLmNoaWxkcmVuLCBwYXJlbnROYW1lQXJyKTtcbiAgICB9XG5cbiAgICBjb25zdCBmb3VuZGVkQ2hpbGRyZW4gPSBwYXJlbnROYW1lQXJyLmZpbHRlcihwYXJlbnQgPT4gcGFyZW50LnBhcmVudE5hbWUgPT09IHJvdXRlLm5hbWUpO1xuICAgIGlmIChmb3VuZGVkQ2hpbGRyZW4gJiYgZm91bmRlZENoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgcm91dGUuY2hpbGRyZW4gPSBbLi4uKHJvdXRlLmNoaWxkcmVuIHx8IFtdKSwgLi4uZm91bmRlZENoaWxkcmVuXTtcbiAgICB9XG5cbiAgICByZXR1cm4gcm91dGU7XG4gIH0pO1xufVxuXG5leHBvcnQgZnVuY3Rpb24gc29ydFJvdXRlcyhyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSA9IFtdKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgaWYgKCFyb3V0ZXMubGVuZ3RoKSByZXR1cm4gW107XG4gIHJldHVybiByb3V0ZXNcbiAgICAuc29ydCgoYSwgYikgPT4gYS5vcmRlciAtIGIub3JkZXIpXG4gICAgLm1hcChyb3V0ZSA9PiB7XG4gICAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XG4gICAgICAgIHJvdXRlLmNoaWxkcmVuID0gc29ydFJvdXRlcyhyb3V0ZS5jaGlsZHJlbik7XG4gICAgICB9XG5cbiAgICAgIHJldHVybiByb3V0ZTtcbiAgICB9KTtcbn1cbiJdfQ==