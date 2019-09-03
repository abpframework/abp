/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { RouteReuseStrategy } from '@angular/router';
var CustomRouteReuseStategy = /** @class */ (function () {
    function CustomRouteReuseStategy() {
        this.handlers = {};
    }
    /**
     * @param {?} route
     * @return {?}
     */
    CustomRouteReuseStategy.prototype.shouldDetach = /**
     * @param {?} route
     * @return {?}
     */
    function (route) {
        return route.data.shouldReuse || false;
    };
    /**
     * @param {?} route
     * @param {?} handle
     * @return {?}
     */
    CustomRouteReuseStategy.prototype.store = /**
     * @param {?} route
     * @param {?} handle
     * @return {?}
     */
    function (route, handle) {
        // if (route.data.shouldReuse) {
        this.handlers[route.routeConfig.path] = handle;
        // }
    };
    /**
     * @param {?} route
     * @return {?}
     */
    CustomRouteReuseStategy.prototype.shouldAttach = /**
     * @param {?} route
     * @return {?}
     */
    function (route) {
        return !!route.routeConfig && !!this.handlers[route.routeConfig.path];
    };
    /**
     * @param {?} route
     * @return {?}
     */
    CustomRouteReuseStategy.prototype.retrieve = /**
     * @param {?} route
     * @return {?}
     */
    function (route) {
        if (!route.routeConfig)
            return null;
        return this.handlers[route.routeConfig.path];
    };
    /**
     * @param {?} future
     * @param {?} curr
     * @return {?}
     */
    CustomRouteReuseStategy.prototype.shouldReuseRoute = /**
     * @param {?} future
     * @param {?} curr
     * @return {?}
     */
    function (future, curr) {
        return future.data.shouldReuse || true;
    };
    return CustomRouteReuseStategy;
}());
export { CustomRouteReuseStategy };
if (false) {
    /** @type {?} */
    CustomRouteReuseStategy.prototype.handlers;
}
/** @type {?} */
export var RouteReuseProvider = { provide: RouteReuseStrategy, useClass: CustomRouteReuseStategy };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmV1c2Utc3RyYXRlZ3kucHJvdmlkZXIuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvcHJvdmlkZXJzL3JldXNlLXN0cmF0ZWd5LnByb3ZpZGVyLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFFQSxPQUFPLEVBQUUsa0JBQWtCLEVBQStDLE1BQU0saUJBQWlCLENBQUM7QUFFbEc7SUFBQTtRQUNFLGFBQVEsR0FBMkMsRUFBRSxDQUFDO0lBd0J4RCxDQUFDOzs7OztJQXRCQyw4Q0FBWTs7OztJQUFaLFVBQWEsS0FBNkI7UUFDeEMsT0FBTyxLQUFLLENBQUMsSUFBSSxDQUFDLFdBQVcsSUFBSSxLQUFLLENBQUM7SUFDekMsQ0FBQzs7Ozs7O0lBRUQsdUNBQUs7Ozs7O0lBQUwsVUFBTSxLQUE2QixFQUFFLE1BQVU7UUFDN0MsZ0NBQWdDO1FBQ2hDLElBQUksQ0FBQyxRQUFRLENBQUMsS0FBSyxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsR0FBRyxNQUFNLENBQUM7UUFDL0MsSUFBSTtJQUNOLENBQUM7Ozs7O0lBRUQsOENBQVk7Ozs7SUFBWixVQUFhLEtBQTZCO1FBQ3hDLE9BQU8sQ0FBQyxDQUFDLEtBQUssQ0FBQyxXQUFXLElBQUksQ0FBQyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsS0FBSyxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsQ0FBQztJQUN4RSxDQUFDOzs7OztJQUVELDBDQUFROzs7O0lBQVIsVUFBUyxLQUE2QjtRQUNwQyxJQUFJLENBQUMsS0FBSyxDQUFDLFdBQVc7WUFBRSxPQUFPLElBQUksQ0FBQztRQUNwQyxPQUFPLElBQUksQ0FBQyxRQUFRLENBQUMsS0FBSyxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsQ0FBQztJQUMvQyxDQUFDOzs7Ozs7SUFFRCxrREFBZ0I7Ozs7O0lBQWhCLFVBQWlCLE1BQThCLEVBQUUsSUFBNEI7UUFDM0UsT0FBTyxNQUFNLENBQUMsSUFBSSxDQUFDLFdBQVcsSUFBSSxJQUFJLENBQUM7SUFDekMsQ0FBQztJQUNILDhCQUFDO0FBQUQsQ0FBQyxBQXpCRCxJQXlCQzs7OztJQXhCQywyQ0FBc0Q7OztBQTBCeEQsTUFBTSxLQUFPLGtCQUFrQixHQUFhLEVBQUUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLFFBQVEsRUFBRSx1QkFBdUIsRUFBRSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFByb3ZpZGVyIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbmltcG9ydCB7IFJvdXRlUmV1c2VTdHJhdGVneSwgQWN0aXZhdGVkUm91dGVTbmFwc2hvdCwgRGV0YWNoZWRSb3V0ZUhhbmRsZSB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5cbmV4cG9ydCBjbGFzcyBDdXN0b21Sb3V0ZVJldXNlU3RhdGVneSBpbXBsZW1lbnRzIFJvdXRlUmV1c2VTdHJhdGVneSB7XG4gIGhhbmRsZXJzOiB7IFtrZXk6IHN0cmluZ106IERldGFjaGVkUm91dGVIYW5kbGUgfSA9IHt9O1xuXG4gIHNob3VsZERldGFjaChyb3V0ZTogQWN0aXZhdGVkUm91dGVTbmFwc2hvdCk6IGJvb2xlYW4ge1xuICAgIHJldHVybiByb3V0ZS5kYXRhLnNob3VsZFJldXNlIHx8IGZhbHNlO1xuICB9XG5cbiAgc3RvcmUocm91dGU6IEFjdGl2YXRlZFJvdXRlU25hcHNob3QsIGhhbmRsZToge30pOiB2b2lkIHtcbiAgICAvLyBpZiAocm91dGUuZGF0YS5zaG91bGRSZXVzZSkge1xuICAgIHRoaXMuaGFuZGxlcnNbcm91dGUucm91dGVDb25maWcucGF0aF0gPSBoYW5kbGU7XG4gICAgLy8gfVxuICB9XG5cbiAgc2hvdWxkQXR0YWNoKHJvdXRlOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90KTogYm9vbGVhbiB7XG4gICAgcmV0dXJuICEhcm91dGUucm91dGVDb25maWcgJiYgISF0aGlzLmhhbmRsZXJzW3JvdXRlLnJvdXRlQ29uZmlnLnBhdGhdO1xuICB9XG5cbiAgcmV0cmlldmUocm91dGU6IEFjdGl2YXRlZFJvdXRlU25hcHNob3QpOiB7fSB7XG4gICAgaWYgKCFyb3V0ZS5yb3V0ZUNvbmZpZykgcmV0dXJuIG51bGw7XG4gICAgcmV0dXJuIHRoaXMuaGFuZGxlcnNbcm91dGUucm91dGVDb25maWcucGF0aF07XG4gIH1cblxuICBzaG91bGRSZXVzZVJvdXRlKGZ1dHVyZTogQWN0aXZhdGVkUm91dGVTbmFwc2hvdCwgY3VycjogQWN0aXZhdGVkUm91dGVTbmFwc2hvdCk6IGJvb2xlYW4ge1xuICAgIHJldHVybiBmdXR1cmUuZGF0YS5zaG91bGRSZXVzZSB8fCB0cnVlO1xuICB9XG59XG5cbmV4cG9ydCBjb25zdCBSb3V0ZVJldXNlUHJvdmlkZXI6IFByb3ZpZGVyID0geyBwcm92aWRlOiBSb3V0ZVJldXNlU3RyYXRlZ3ksIHVzZUNsYXNzOiBDdXN0b21Sb3V0ZVJldXNlU3RhdGVneSB9O1xuIl19