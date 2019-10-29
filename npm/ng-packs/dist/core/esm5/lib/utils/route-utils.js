/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
/**
 * @param {?} routes
 * @param {?=} wrappers
 * @param {?=} parentNameArr
 * @param {?=} parentName
 * @return {?}
 */
export function organizeRoutes(routes, wrappers, parentNameArr, parentName) {
  if (wrappers === void 0) {
    wrappers = [];
  }
  if (parentNameArr === void 0) {
    parentNameArr = /** @type {?} */ ([]);
  }
  if (parentName === void 0) {
    parentName = null;
  }
  /** @type {?} */
  var filter
  /**
   * @param {?} route
   * @return {?}
   */ = (function(route) {
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
  return routes.map(
    /**
     * @param {?} route
     * @return {?}
     */
    function(route) {
      if (route.children && route.children.length) {
        route.children = setChildRoute(route.children, parentNameArr);
      }
      /** @type {?} */
      var foundedChildren = parentNameArr.filter(
        /**
         * @param {?} parent
         * @return {?}
         */
        (function(parent) {
          return parent.parentName === route.name;
        }),
      );
      if (foundedChildren && foundedChildren.length) {
        route.children = tslib_1.__spread(route.children || [], foundedChildren);
      }
      return route;
    },
  );
}
/**
 * @param {?=} routes
 * @return {?}
 */
export function sortRoutes(routes) {
  if (routes === void 0) {
    routes = [];
  }
  if (!routes.length) return [];
  return routes
    .sort(
      /**
       * @param {?} a
       * @param {?} b
       * @return {?}
       */
      function(a, b) {
        return a.order - b.order;
      },
    )
    .map(
      /**
       * @param {?} route
       * @return {?}
       */
      function(route) {
        if (route.children && route.children.length) {
          route.children = sortRoutes(route.children);
        }
        return route;
      },
    );
}
/** @type {?} */
var ABP_ROUTES = /** @type {?} */ ([]);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm91dGUtdXRpbHMuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvdXRpbHMvcm91dGUtdXRpbHMudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7O0FBRUEsTUFBTSxVQUFVLGNBQWMsQ0FDNUIsTUFBdUIsRUFDdkIsUUFBOEIsRUFDOUIsYUFBcUMsRUFDckMsVUFBeUI7SUFGekIseUJBQUEsRUFBQSxhQUE4QjtJQUM5Qiw4QkFBQSxFQUFBLG1DQUFnQixFQUFFLEVBQW1CO0lBQ3JDLDJCQUFBLEVBQUEsaUJBQXlCOztRQUVuQixNQUFNOzs7O0lBQUcsVUFBQSxLQUFLO1FBQ2xCLElBQUksS0FBSyxDQUFDLFFBQVEsSUFBSSxLQUFLLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUMzQyxLQUFLLENBQUMsUUFBUSxHQUFHLGNBQWMsQ0FBQyxLQUFLLENBQUMsUUFBUSxFQUFFLFFBQVEsRUFBRSxhQUFhLEVBQUUsS0FBSyxDQUFDLElBQUksQ0FBQyxDQUFDO1NBQ3RGO1FBRUQsSUFBSSxLQUFLLENBQUMsVUFBVSxJQUFJLEtBQUssQ0FBQyxVQUFVLEtBQUssVUFBVSxFQUFFO1lBQ3ZELGFBQWEsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7WUFDMUIsT0FBTyxLQUFLLENBQUM7U0FDZDtRQUVELE9BQU8sSUFBSSxDQUFDO0lBQ2QsQ0FBQyxDQUFBO0lBRUQsSUFBSSxVQUFVLEVBQUU7UUFDZCxrQkFBa0I7UUFDbEIsT0FBTyxNQUFNLENBQUMsTUFBTSxDQUFDLE1BQU0sQ0FBQyxDQUFDO0tBQzlCOztRQUVLLGNBQWMsR0FBRyxNQUFNLENBQUMsTUFBTSxDQUFDLE1BQU0sQ0FBQztJQUU1QyxJQUFJLGFBQWEsQ0FBQyxNQUFNLEVBQUU7UUFDeEIsT0FBTyxVQUFVLENBQUMsYUFBYSxrQkFBSyxjQUFjLEVBQUssUUFBUSxHQUFHLGFBQWEsQ0FBQyxDQUFDLENBQUM7S0FDbkY7SUFFRCxPQUFPLGNBQWMsQ0FBQztBQUN4QixDQUFDOzs7Ozs7QUFFRCxNQUFNLFVBQVUsYUFBYSxDQUFDLE1BQXVCLEVBQUUsYUFBOEI7SUFDbkYsT0FBTyxNQUFNLENBQUMsR0FBRzs7OztJQUFDLFVBQUEsS0FBSztRQUNyQixJQUFJLEtBQUssQ0FBQyxRQUFRLElBQUksS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDM0MsS0FBSyxDQUFDLFFBQVEsR0FBRyxhQUFhLENBQUMsS0FBSyxDQUFDLFFBQVEsRUFBRSxhQUFhLENBQUMsQ0FBQztTQUMvRDs7WUFFSyxlQUFlLEdBQUcsYUFBYSxDQUFDLE1BQU07Ozs7UUFBQyxVQUFBLE1BQU0sSUFBSSxPQUFBLE1BQU0sQ0FBQyxVQUFVLEtBQUssS0FBSyxDQUFDLElBQUksRUFBaEMsQ0FBZ0MsRUFBQztRQUN4RixJQUFJLGVBQWUsSUFBSSxlQUFlLENBQUMsTUFBTSxFQUFFO1lBQzdDLEtBQUssQ0FBQyxRQUFRLG9CQUFPLENBQUMsS0FBSyxDQUFDLFFBQVEsSUFBSSxFQUFFLENBQUMsRUFBSyxlQUFlLENBQUMsQ0FBQztTQUNsRTtRQUVELE9BQU8sS0FBSyxDQUFDO0lBQ2YsQ0FBQyxFQUFDLENBQUM7QUFDTCxDQUFDOzs7OztBQUVELE1BQU0sVUFBVSxVQUFVLENBQUMsTUFBNEI7SUFBNUIsdUJBQUEsRUFBQSxXQUE0QjtJQUNyRCxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU07UUFBRSxPQUFPLEVBQUUsQ0FBQztJQUM5QixPQUFPLE1BQU07U0FDVixJQUFJOzs7OztJQUFDLFVBQUMsQ0FBQyxFQUFFLENBQUMsSUFBSyxPQUFBLENBQUMsQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDLEtBQUssRUFBakIsQ0FBaUIsRUFBQztTQUNqQyxHQUFHOzs7O0lBQUMsVUFBQSxLQUFLO1FBQ1IsSUFBSSxLQUFLLENBQUMsUUFBUSxJQUFJLEtBQUssQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO1lBQzNDLEtBQUssQ0FBQyxRQUFRLEdBQUcsVUFBVSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsQ0FBQztTQUM3QztRQUVELE9BQU8sS0FBSyxDQUFDO0lBQ2YsQ0FBQyxFQUFDLENBQUM7QUFDUCxDQUFDOztJQUVLLFVBQVUsR0FBRyxtQkFBQSxFQUFFLEVBQW1COzs7OztBQUV4QyxNQUFNLFVBQVUsWUFBWSxDQUFDLE1BQXVDO0lBQ2xFLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLE1BQU0sQ0FBQyxFQUFFO1FBQzFCLE1BQU0sR0FBRyxDQUFDLE1BQU0sQ0FBQyxDQUFDO0tBQ25CO0lBRUQsVUFBVSxDQUFDLElBQUksT0FBZixVQUFVLG1CQUFTLE1BQU0sR0FBRTtBQUM3QixDQUFDOzs7O0FBRUQsTUFBTSxVQUFVLFlBQVk7SUFDMUIsT0FBTyxVQUFVLENBQUM7QUFDcEIsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCB9IGZyb20gJy4uL21vZGVscyc7XG5cbmV4cG9ydCBmdW5jdGlvbiBvcmdhbml6ZVJvdXRlcyhcbiAgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10sXG4gIHdyYXBwZXJzOiBBQlAuRnVsbFJvdXRlW10gPSBbXSxcbiAgcGFyZW50TmFtZUFyciA9IFtdIGFzIEFCUC5GdWxsUm91dGVbXSxcbiAgcGFyZW50TmFtZTogc3RyaW5nID0gbnVsbCxcbik6IEFCUC5GdWxsUm91dGVbXSB7XG4gIGNvbnN0IGZpbHRlciA9IHJvdXRlID0+IHtcbiAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XG4gICAgICByb3V0ZS5jaGlsZHJlbiA9IG9yZ2FuaXplUm91dGVzKHJvdXRlLmNoaWxkcmVuLCB3cmFwcGVycywgcGFyZW50TmFtZUFyciwgcm91dGUubmFtZSk7XG4gICAgfVxuXG4gICAgaWYgKHJvdXRlLnBhcmVudE5hbWUgJiYgcm91dGUucGFyZW50TmFtZSAhPT0gcGFyZW50TmFtZSkge1xuICAgICAgcGFyZW50TmFtZUFyci5wdXNoKHJvdXRlKTtcbiAgICAgIHJldHVybiBmYWxzZTtcbiAgICB9XG5cbiAgICByZXR1cm4gdHJ1ZTtcbiAgfTtcblxuICBpZiAocGFyZW50TmFtZSkge1xuICAgIC8vIHJlY3Vyc2l2ZSBibG9ja1xuICAgIHJldHVybiByb3V0ZXMuZmlsdGVyKGZpbHRlcik7XG4gIH1cblxuICBjb25zdCBmaWx0ZXJlZFJvdXRlcyA9IHJvdXRlcy5maWx0ZXIoZmlsdGVyKTtcblxuICBpZiAocGFyZW50TmFtZUFyci5sZW5ndGgpIHtcbiAgICByZXR1cm4gc29ydFJvdXRlcyhzZXRDaGlsZFJvdXRlKFsuLi5maWx0ZXJlZFJvdXRlcywgLi4ud3JhcHBlcnNdLCBwYXJlbnROYW1lQXJyKSk7XG4gIH1cblxuICByZXR1cm4gZmlsdGVyZWRSb3V0ZXM7XG59XG5cbmV4cG9ydCBmdW5jdGlvbiBzZXRDaGlsZFJvdXRlKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdLCBwYXJlbnROYW1lQXJyOiBBQlAuRnVsbFJvdXRlW10pOiBBQlAuRnVsbFJvdXRlW10ge1xuICByZXR1cm4gcm91dGVzLm1hcChyb3V0ZSA9PiB7XG4gICAgaWYgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgcm91dGUuY2hpbGRyZW4gPSBzZXRDaGlsZFJvdXRlKHJvdXRlLmNoaWxkcmVuLCBwYXJlbnROYW1lQXJyKTtcbiAgICB9XG5cbiAgICBjb25zdCBmb3VuZGVkQ2hpbGRyZW4gPSBwYXJlbnROYW1lQXJyLmZpbHRlcihwYXJlbnQgPT4gcGFyZW50LnBhcmVudE5hbWUgPT09IHJvdXRlLm5hbWUpO1xuICAgIGlmIChmb3VuZGVkQ2hpbGRyZW4gJiYgZm91bmRlZENoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgcm91dGUuY2hpbGRyZW4gPSBbLi4uKHJvdXRlLmNoaWxkcmVuIHx8IFtdKSwgLi4uZm91bmRlZENoaWxkcmVuXTtcbiAgICB9XG5cbiAgICByZXR1cm4gcm91dGU7XG4gIH0pO1xufVxuXG5leHBvcnQgZnVuY3Rpb24gc29ydFJvdXRlcyhyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSA9IFtdKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgaWYgKCFyb3V0ZXMubGVuZ3RoKSByZXR1cm4gW107XG4gIHJldHVybiByb3V0ZXNcbiAgICAuc29ydCgoYSwgYikgPT4gYS5vcmRlciAtIGIub3JkZXIpXG4gICAgLm1hcChyb3V0ZSA9PiB7XG4gICAgICBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XG4gICAgICAgIHJvdXRlLmNoaWxkcmVuID0gc29ydFJvdXRlcyhyb3V0ZS5jaGlsZHJlbik7XG4gICAgICB9XG5cbiAgICAgIHJldHVybiByb3V0ZTtcbiAgICB9KTtcbn1cblxuY29uc3QgQUJQX1JPVVRFUyA9IFtdIGFzIEFCUC5GdWxsUm91dGVbXTtcblxuZXhwb3J0IGZ1bmN0aW9uIGFkZEFicFJvdXRlcyhyb3V0ZXM6IEFCUC5GdWxsUm91dGUgfCBBQlAuRnVsbFJvdXRlW10pOiB2b2lkIHtcbiAgaWYgKCFBcnJheS5pc0FycmF5KHJvdXRlcykpIHtcbiAgICByb3V0ZXMgPSBbcm91dGVzXTtcbiAgfVxuXG4gIEFCUF9ST1VURVMucHVzaCguLi5yb3V0ZXMpO1xufVxuXG5leHBvcnQgZnVuY3Rpb24gZ2V0QWJwUm91dGVzKCk6IEFCUC5GdWxsUm91dGVbXSB7XG4gIHJldHVybiBBQlBfUk9VVEVTO1xufVxuIl19
