/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { RouteReuseStrategy } from '@angular/router';
export class CustomRouteReuseStategy {
    constructor() {
        this.handlers = {};
    }
    /**
     * @param {?} route
     * @return {?}
     */
    shouldDetach(route) {
        return route.data.shouldReuse || false;
    }
    /**
     * @param {?} route
     * @param {?} handle
     * @return {?}
     */
    store(route, handle) {
        // if (route.data.shouldReuse) {
        this.handlers[route.routeConfig.path] = handle;
        // }
    }
    /**
     * @param {?} route
     * @return {?}
     */
    shouldAttach(route) {
        return !!route.routeConfig && !!this.handlers[route.routeConfig.path];
    }
    /**
     * @param {?} route
     * @return {?}
     */
    retrieve(route) {
        if (!route.routeConfig)
            return null;
        return this.handlers[route.routeConfig.path];
    }
    /**
     * @param {?} future
     * @param {?} curr
     * @return {?}
     */
    shouldReuseRoute(future, curr) {
        return future.data.shouldReuse || true;
    }
}
if (false) {
    /** @type {?} */
    CustomRouteReuseStategy.prototype.handlers;
}
/** @type {?} */
export const RouteReuseProvider = { provide: RouteReuseStrategy, useClass: CustomRouteReuseStategy };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmV1c2Utc3RyYXRlZ3kucHJvdmlkZXIuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvcHJvdmlkZXJzL3JldXNlLXN0cmF0ZWd5LnByb3ZpZGVyLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFFQSxPQUFPLEVBQUUsa0JBQWtCLEVBQStDLE1BQU0saUJBQWlCLENBQUM7QUFFbEcsTUFBTSxPQUFPLHVCQUF1QjtJQUFwQztRQUNFLGFBQVEsR0FBMkMsRUFBRSxDQUFDO0lBd0J4RCxDQUFDOzs7OztJQXRCQyxZQUFZLENBQUMsS0FBNkI7UUFDeEMsT0FBTyxLQUFLLENBQUMsSUFBSSxDQUFDLFdBQVcsSUFBSSxLQUFLLENBQUM7SUFDekMsQ0FBQzs7Ozs7O0lBRUQsS0FBSyxDQUFDLEtBQTZCLEVBQUUsTUFBVTtRQUM3QyxnQ0FBZ0M7UUFDaEMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxLQUFLLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxHQUFHLE1BQU0sQ0FBQztRQUMvQyxJQUFJO0lBQ04sQ0FBQzs7Ozs7SUFFRCxZQUFZLENBQUMsS0FBNkI7UUFDeEMsT0FBTyxDQUFDLENBQUMsS0FBSyxDQUFDLFdBQVcsSUFBSSxDQUFDLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxLQUFLLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxDQUFDO0lBQ3hFLENBQUM7Ozs7O0lBRUQsUUFBUSxDQUFDLEtBQTZCO1FBQ3BDLElBQUksQ0FBQyxLQUFLLENBQUMsV0FBVztZQUFFLE9BQU8sSUFBSSxDQUFDO1FBQ3BDLE9BQU8sSUFBSSxDQUFDLFFBQVEsQ0FBQyxLQUFLLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxDQUFDO0lBQy9DLENBQUM7Ozs7OztJQUVELGdCQUFnQixDQUFDLE1BQThCLEVBQUUsSUFBNEI7UUFDM0UsT0FBTyxNQUFNLENBQUMsSUFBSSxDQUFDLFdBQVcsSUFBSSxJQUFJLENBQUM7SUFDekMsQ0FBQztDQUNGOzs7SUF4QkMsMkNBQXNEOzs7QUEwQnhELE1BQU0sT0FBTyxrQkFBa0IsR0FBYSxFQUFFLE9BQU8sRUFBRSxrQkFBa0IsRUFBRSxRQUFRLEVBQUUsdUJBQXVCLEVBQUUiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBQcm92aWRlciB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuXG5pbXBvcnQgeyBSb3V0ZVJldXNlU3RyYXRlZ3ksIEFjdGl2YXRlZFJvdXRlU25hcHNob3QsIERldGFjaGVkUm91dGVIYW5kbGUgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuXG5leHBvcnQgY2xhc3MgQ3VzdG9tUm91dGVSZXVzZVN0YXRlZ3kgaW1wbGVtZW50cyBSb3V0ZVJldXNlU3RyYXRlZ3kge1xuICBoYW5kbGVyczogeyBba2V5OiBzdHJpbmddOiBEZXRhY2hlZFJvdXRlSGFuZGxlIH0gPSB7fTtcblxuICBzaG91bGREZXRhY2gocm91dGU6IEFjdGl2YXRlZFJvdXRlU25hcHNob3QpOiBib29sZWFuIHtcbiAgICByZXR1cm4gcm91dGUuZGF0YS5zaG91bGRSZXVzZSB8fCBmYWxzZTtcbiAgfVxuXG4gIHN0b3JlKHJvdXRlOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90LCBoYW5kbGU6IHt9KTogdm9pZCB7XG4gICAgLy8gaWYgKHJvdXRlLmRhdGEuc2hvdWxkUmV1c2UpIHtcbiAgICB0aGlzLmhhbmRsZXJzW3JvdXRlLnJvdXRlQ29uZmlnLnBhdGhdID0gaGFuZGxlO1xuICAgIC8vIH1cbiAgfVxuXG4gIHNob3VsZEF0dGFjaChyb3V0ZTogQWN0aXZhdGVkUm91dGVTbmFwc2hvdCk6IGJvb2xlYW4ge1xuICAgIHJldHVybiAhIXJvdXRlLnJvdXRlQ29uZmlnICYmICEhdGhpcy5oYW5kbGVyc1tyb3V0ZS5yb3V0ZUNvbmZpZy5wYXRoXTtcbiAgfVxuXG4gIHJldHJpZXZlKHJvdXRlOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90KToge30ge1xuICAgIGlmICghcm91dGUucm91dGVDb25maWcpIHJldHVybiBudWxsO1xuICAgIHJldHVybiB0aGlzLmhhbmRsZXJzW3JvdXRlLnJvdXRlQ29uZmlnLnBhdGhdO1xuICB9XG5cbiAgc2hvdWxkUmV1c2VSb3V0ZShmdXR1cmU6IEFjdGl2YXRlZFJvdXRlU25hcHNob3QsIGN1cnI6IEFjdGl2YXRlZFJvdXRlU25hcHNob3QpOiBib29sZWFuIHtcbiAgICByZXR1cm4gZnV0dXJlLmRhdGEuc2hvdWxkUmV1c2UgfHwgdHJ1ZTtcbiAgfVxufVxuXG5leHBvcnQgY29uc3QgUm91dGVSZXVzZVByb3ZpZGVyOiBQcm92aWRlciA9IHsgcHJvdmlkZTogUm91dGVSZXVzZVN0cmF0ZWd5LCB1c2VDbGFzczogQ3VzdG9tUm91dGVSZXVzZVN0YXRlZ3kgfTtcbiJdfQ==