/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { Router } from '@angular/router';
import { actionMatcher, InitState, setValue, UpdateState } from '@ngxs/store';
import snq from 'snq';
import { organizeRoutes, getAbpRoutes } from '../../utils/route-utils';
import clone from 'just-clone';
/** @type {?} */
export var NGXS_CONFIG_PLUGIN_OPTIONS = new InjectionToken('NGXS_CONFIG_PLUGIN_OPTIONS');
var ConfigPlugin = /** @class */ (function() {
  function ConfigPlugin(options, router) {
    this.options = options;
    this.router = router;
    this.initialized = false;
  }
  /**
   * @param {?} state
   * @param {?} event
   * @param {?} next
   * @return {?}
   */
  ConfigPlugin.prototype.handle
  /**
   * @param {?} state
   * @param {?} event
   * @param {?} next
   * @return {?}
   */ = function(state, event, next) {
    /** @type {?} */
    var matches = actionMatcher(event);
    /** @type {?} */
    var isInitAction = matches(InitState) || matches(UpdateState);
    if (isInitAction && !this.initialized) {
      /** @type {?} */
      var transformedRoutes = transformRoutes(this.router.config);
      var routes = transformedRoutes.routes;
      var wrappers = transformedRoutes.wrappers;
      routes = organizeRoutes(routes, wrappers);
      /** @type {?} */
      var flattedRoutes = flatRoutes(clone(routes));
      state = setValue(
        state,
        'ConfigState',
        tslib_1.__assign({}, state.ConfigState && tslib_1.__assign({}, state.ConfigState), this.options, {
          routes: routes,
          flattedRoutes: flattedRoutes,
        }),
      );
      this.initialized = true;
    }
    return next(state, event);
  };
  ConfigPlugin.decorators = [{ type: Injectable }];
  /** @nocollapse */
  ConfigPlugin.ctorParameters = function() {
    return [{ type: undefined, decorators: [{ type: Inject, args: [NGXS_CONFIG_PLUGIN_OPTIONS] }] }, { type: Router }];
  };
  return ConfigPlugin;
})();
export { ConfigPlugin };
if (false) {
  /**
   * @type {?}
   * @private
   */
  ConfigPlugin.prototype.initialized;
  /**
   * @type {?}
   * @private
   */
  ConfigPlugin.prototype.options;
  /**
   * @type {?}
   * @private
   */
  ConfigPlugin.prototype.router;
}
/**
 * @param {?=} routes
 * @param {?=} wrappers
 * @return {?}
 */
function transformRoutes(routes, wrappers) {
  if (routes === void 0) {
    routes = [];
  }
  if (wrappers === void 0) {
    wrappers = [];
  }
  // TODO: remove in v1
  /** @type {?} */
  var oldAbpRoutes = routes
    .filter(
      /**
       * @param {?} route
       * @return {?}
       */
      (function(route) {
        return snq(
          /**
           * @return {?}
           */
          function() {
            return route.data.routes.routes.find(
              /**
               * @param {?} r
               * @return {?}
               */
              function(r) {
                return r.path === route.path;
              },
            );
          },
          false,
        );
      }),
    )
    .reduce(
      /**
       * @param {?} acc
       * @param {?} val
       * @return {?}
       */
      (function(acc, val) {
        return tslib_1.__spread(acc, val.data.routes.routes);
      }),
      [],
    );
  // tslint:disable-next-line: deprecation
  /** @type {?} */
  var abpRoutes = tslib_1.__spread(getAbpRoutes(), oldAbpRoutes);
  wrappers = abpRoutes.filter(
    /**
     * @param {?} ar
     * @return {?}
     */
    function(ar) {
      return ar.wrapper;
    },
  );
  /** @type {?} */
  var transformed = /** @type {?} */ ([]);
  routes
    .filter(
      /**
       * @param {?} route
       * @return {?}
       */
      function(route) {
        return route.component || route.loadChildren;
      },
    )
    .forEach(
      /**
       * @param {?} route
       * @return {?}
       */
      function(route) {
        /** @type {?} */
        var abpPackage = abpRoutes.find(
          /**
           * @param {?} abp
           * @return {?}
           */
          (function(abp) {
            return abp.path.toLowerCase() === route.path.toLowerCase() && !abp.wrapper;
          }),
        );
        var length = transformed.length;
        if (abpPackage) {
          transformed.push(abpPackage);
        }
        if (transformed.length === length && (route.data || {}).routes) {
          transformed.push(
            /** @type {?} */ (tslib_1.__assign({}, route.data.routes, {
              path: route.path,
              name: snq(
                /**
                 * @return {?}
                 */
                function() {
                  return route.data.routes.name;
                },
                route.path,
              ),
              children: route.data.routes.children || [],
            })),
          );
        }
      },
    );
  return { routes: setUrls(transformed), wrappers: wrappers };
}
/**
 * @param {?} routes
 * @param {?=} parentUrl
 * @return {?}
 */
function setUrls(routes, parentUrl) {
  if (parentUrl) {
    // this if block using for only recursive call
    return routes.map(
      /**
       * @param {?} route
       * @return {?}
       */
      function(route) {
        return tslib_1.__assign(
          {},
          route,
          { url: parentUrl + '/' + route.path },
          route.children &&
            route.children.length && {
              children: setUrls(route.children, parentUrl + '/' + route.path),
            },
        );
      },
    );
  }
  return routes.map(
    /**
     * @param {?} route
     * @return {?}
     */
    function(route) {
      return tslib_1.__assign(
        {},
        route,
        { url: '/' + route.path },
        route.children &&
          route.children.length && {
            children: setUrls(route.children, '/' + route.path),
          },
      );
    },
  );
}
/**
 * @param {?} routes
 * @return {?}
 */
function flatRoutes(routes) {
  /** @type {?} */
  var flat
  /**
   * @param {?} r
   * @return {?}
   */ = (function(r) {
    return r.reduce(
      /**
       * @param {?} acc
       * @param {?} val
       * @return {?}
       */
      function(acc, val) {
        /** @type {?} */
        var value = [val];
        if (val.children) {
          value = tslib_1.__spread([val], flat(val.children));
        }
        return tslib_1.__spread(acc, value);
      },
      [],
    );
  });
  return flat(routes);
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnBsdWdpbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9wbHVnaW5zL2NvbmZpZy9jb25maWcucGx1Z2luLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLE1BQU0sRUFBRSxVQUFVLEVBQUUsY0FBYyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ25FLE9BQU8sRUFBRSxNQUFNLEVBQVUsTUFBTSxpQkFBaUIsQ0FBQztBQUNqRCxPQUFPLEVBQUUsYUFBYSxFQUFFLFNBQVMsRUFBZ0MsUUFBUSxFQUFFLFdBQVcsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1RyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFFdEIsT0FBTyxFQUFFLGNBQWMsRUFBRSxZQUFZLEVBQUUsTUFBTSx5QkFBeUIsQ0FBQztBQUN2RSxPQUFPLEtBQUssTUFBTSxZQUFZLENBQUM7O0FBRS9CLE1BQU0sS0FBTywwQkFBMEIsR0FBRyxJQUFJLGNBQWMsQ0FBQyw0QkFBNEIsQ0FBQztBQUUxRjtJQUlFLHNCQUF3RCxPQUFpQixFQUFVLE1BQWM7UUFBekMsWUFBTyxHQUFQLE9BQU8sQ0FBVTtRQUFVLFdBQU0sR0FBTixNQUFNLENBQVE7UUFGekYsZ0JBQVcsR0FBRyxLQUFLLENBQUM7SUFFd0UsQ0FBQzs7Ozs7OztJQUVyRyw2QkFBTTs7Ozs7O0lBQU4sVUFBTyxLQUFVLEVBQUUsS0FBVSxFQUFFLElBQXNCOztZQUM3QyxPQUFPLEdBQUcsYUFBYSxDQUFDLEtBQUssQ0FBQzs7WUFDOUIsWUFBWSxHQUFHLE9BQU8sQ0FBQyxTQUFTLENBQUMsSUFBSSxPQUFPLENBQUMsV0FBVyxDQUFDO1FBRS9ELElBQUksWUFBWSxJQUFJLENBQUMsSUFBSSxDQUFDLFdBQVcsRUFBRTs7Z0JBQy9CLGlCQUFpQixHQUFHLGVBQWUsQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU0sQ0FBQztZQUN2RCxJQUFBLGlDQUFN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Ozs7O0lBQUcsVUFBQyxDQUFrQjtRQUM5QixPQUFPLENBQUMsQ0FBQyxNQUFNOzs7OztRQUFDLFVBQUMsR0FBRyxFQUFFLEdBQUc7O2dCQUNuQixLQUFLLEdBQW9CLENBQUMsR0FBRyxDQUFDO1lBQ2xDLElBQUksR0FBRyxDQUFDLFFBQVEsRUFBRTtnQkFDaEIsS0FBSyxxQkFBSSxHQUFHLEdBQUssSUFBSSxDQUFDLEdBQUcsQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDO2FBQ3RDO1lBRUQsd0JBQVcsR0FBRyxFQUFLLEtBQUssRUFBRTtRQUM1QixDQUFDLEdBQUUsRUFBRSxDQUFDLENBQUM7SUFDVCxDQUFDLENBQUE7SUFFRCxPQUFPLElBQUksQ0FBQyxNQUFNLENBQUMsQ0FBQztBQUN0QixDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0LCBJbmplY3RhYmxlLCBJbmplY3Rpb25Ub2tlbiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgUm91dGVyLCBSb3V0ZXMgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgYWN0aW9uTWF0Y2hlciwgSW5pdFN0YXRlLCBOZ3hzTmV4dFBsdWdpbkZuLCBOZ3hzUGx1Z2luLCBzZXRWYWx1ZSwgVXBkYXRlU3RhdGUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQgeyBBQlAgfSBmcm9tICcuLi8uLi9tb2RlbHMnO1xuaW1wb3J0IHsgb3JnYW5pemVSb3V0ZXMsIGdldEFicFJvdXRlcyB9IGZyb20gJy4uLy4uL3V0aWxzL3JvdXRlLXV0aWxzJztcbmltcG9ydCBjbG9uZSBmcm9tICdqdXN0LWNsb25lJztcblxuZXhwb3J0IGNvbnN0IE5HWFNfQ09ORklHX1BMVUdJTl9PUFRJT05TID0gbmV3IEluamVjdGlvblRva2VuKCdOR1hTX0NPTkZJR19QTFVHSU5fT1BUSU9OUycpO1xuXG5ASW5qZWN0YWJsZSgpXG5leHBvcnQgY2xhc3MgQ29uZmlnUGx1Z2luIGltcGxlbWVudHMgTmd4c1BsdWdpbiB7XG4gIHByaXZhdGUgaW5pdGlhbGl6ZWQgPSBmYWxzZTtcblxuICBjb25zdHJ1Y3RvcihASW5qZWN0KE5HWFNfQ09ORklHX1BMVUdJTl9PUFRJT05TKSBwcml2YXRlIG9wdGlvbnM6IEFCUC5Sb290LCBwcml2YXRlIHJvdXRlcjogUm91dGVyKSB7fVxuXG4gIGhhbmRsZShzdGF0ZTogYW55LCBldmVudDogYW55LCBuZXh0OiBOZ3hzTmV4dFBsdWdpbkZuKSB7XG4gICAgY29uc3QgbWF0Y2hlcyA9IGFjdGlvbk1hdGNoZXIoZXZlbnQpO1xuICAgIGNvbnN0IGlzSW5pdEFjdGlvbiA9IG1hdGNoZXMoSW5pdFN0YXRlKSB8fCBtYXRjaGVzKFVwZGF0ZVN0YXRlKTtcblxuICAgIGlmIChpc0luaXRBY3Rpb24gJiYgIXRoaXMuaW5pdGlhbGl6ZWQpIHtcbiAgICAgIGNvbnN0IHRyYW5zZm9ybWVkUm91dGVzID0gdHJhbnNmb3JtUm91dGVzKHRoaXMucm91dGVyLmNvbmZpZyk7XG4gICAgICBsZXQgeyByb3V0ZXMgfSA9IHRyYW5zZm9ybWVkUm91dGVzO1xuICAgICAgY29uc3QgeyB3cmFwcGVycyB9ID0gdHJhbnNmb3JtZWRSb3V0ZXM7XG5cbiAgICAgIHJvdXRlcyA9IG9yZ2FuaXplUm91dGVzKHJvdXRlcywgd3JhcHBlcnMpO1xuICAgICAgY29uc3QgZmxhdHRlZFJvdXRlcyA9IGZsYXRSb3V0ZXMoY2xvbmUocm91dGVzKSk7XG4gICAgICBzdGF0ZSA9IHNldFZhbHVlKHN0YXRlLCAnQ29uZmlnU3RhdGUnLCB7XG4gICAgICAgIC4uLihzdGF0ZS5Db25maWdTdGF0ZSAmJiB7IC4uLnN0YXRlLkNvbmZpZ1N0YXRlIH0pLFxuICAgICAgICAuLi50aGlzLm9wdGlvbnMsXG4gICAgICAgIHJvdXRlcyxcbiAgICAgICAgZmxhdHRlZFJvdXRlcyxcbiAgICAgIH0pO1xuXG4gICAgICB0aGlzLmluaXRpYWxpemVkID0gdHJ1ZTtcbiAgICB9XG5cbiAgICByZXR1cm4gbmV4dChzdGF0ZSwgZXZlbnQpO1xuICB9XG59XG5cbmZ1bmN0aW9uIHRyYW5zZm9ybVJvdXRlcyhyb3V0ZXM6IFJvdXRlcyA9IFtdLCB3cmFwcGVyczogQUJQLkZ1bGxSb3V0ZVtdID0gW10pOiBhbnkge1xuICAvLyBUT0RPOiByZW1vdmUgaW4gdjFcbiAgY29uc3Qgb2xkQWJwUm91dGVzOiBBQlAuRnVsbFJvdXRlW10gPSByb3V0ZXNcbiAgICAuZmlsdGVyKHJvdXRlID0+IHtcbiAgICAgIHJldHVybiBzbnEoKCkgPT4gcm91dGUuZGF0YS5yb3V0ZXMucm91dGVzLmZpbmQociA9PiByLnBhdGggPT09IHJvdXRlLnBhdGgpLCBmYWxzZSk7XG4gICAgfSlcbiAgICAucmVkdWNlKChhY2MsIHZhbCkgPT4gWy4uLmFjYywgLi4udmFsLmRhdGEucm91dGVzLnJvdXRlc10sIFtdKTtcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBkZXByZWNhdGlvblxuICBjb25zdCBhYnBSb3V0ZXMgPSBbLi4uZ2V0QWJwUm91dGVzKCksIC4uLm9sZEFicFJvdXRlc107XG5cbiAgd3JhcHBlcnMgPSBhYnBSb3V0ZXMuZmlsdGVyKGFyID0+IGFyLndyYXBwZXIpO1xuICBjb25zdCB0cmFuc2Zvcm1lZCA9IFtdIGFzIEFCUC5GdWxsUm91dGVbXTtcbiAgcm91dGVzXG4gICAgLmZpbHRlcihyb3V0ZSA9PiByb3V0ZS5jb21wb25lbnQgfHwgcm91dGUubG9hZENoaWxkcmVuKVxuICAgIC5mb3JFYWNoKHJvdXRlID0+IHtcbiAgICAgIGNvbnN0IGFicFBhY2thZ2UgPSBhYnBSb3V0ZXMuZmluZChhYnAgPT4gYWJwLnBhdGgudG9Mb3dlckNhc2UoKSA9PT0gcm91dGUucGF0aC50b0xvd2VyQ2FzZSgpICYmICFhYnAud3JhcHBlcik7XG5cbiAgICAgIGNvbnN0IHsgbGVuZ3RoIH0gPSB0cmFuc2Zvcm1lZDtcblxuICAgICAgaWYgKGFicFBhY2thZ2UpIHtcbiAgICAgICAgdHJhbnNmb3JtZWQucHVzaChhYnBQYWNrYWdlKTtcbiAgICAgIH1cblxuICAgICAgaWYgKHRyYW5zZm9ybWVkLmxlbmd0aCA9PT0gbGVuZ3RoICYmIChyb3V0ZS5kYXRhIHx8IHt9KS5yb3V0ZXMpIHtcbiAgICAgICAgdHJhbnNmb3JtZWQucHVzaCh7XG4gICAgICAgICAgLi4ucm91dGUuZGF0YS5yb3V0ZXMsXG4gICAgICAgICAgcGF0aDogcm91dGUucGF0aCxcbiAgICAgICAgICBuYW1lOiBzbnEoKCkgPT4gcm91dGUuZGF0YS5yb3V0ZXMubmFtZSwgcm91dGUucGF0aCksXG4gICAgICAgICAgY2hpbGRyZW46IHJvdXRlLmRhdGEucm91dGVzLmNoaWxkcmVuIHx8IFtdLFxuICAgICAgICB9IGFzIEFCUC5GdWxsUm91dGUpO1xuICAgICAgfVxuICAgIH0pO1xuXG4gIHJldHVybiB7IHJvdXRlczogc2V0VXJscyh0cmFuc2Zvcm1lZCksIHdyYXBwZXJzIH07XG59XG5cbmZ1bmN0aW9uIHNldFVybHMocm91dGVzOiBBQlAuRnVsbFJvdXRlW10sIHBhcmVudFVybD86IHN0cmluZyk6IEFCUC5GdWxsUm91dGVbXSB7XG4gIGlmIChwYXJlbnRVcmwpIHtcbiAgICAvLyB0aGlzIGlmIGJsb2NrIHVzaW5nIGZvciBvbmx5IHJlY3Vyc2l2ZSBjYWxsXG5cbiAgICByZXR1cm4gcm91dGVzLm1hcChyb3V0ZSA9PiAoe1xuICAgICAgLi4ucm91dGUsXG4gICAgICB1cmw6IGAke3BhcmVudFVybH0vJHtyb3V0ZS5wYXRofWAsXG4gICAgICAuLi4ocm91dGUuY2hpbGRyZW4gJiZcbiAgICAgICAgcm91dGUuY2hpbGRyZW4ubGVuZ3RoICYmIHtcbiAgICAgICAgICBjaGlsZHJlbjogc2V0VXJscyhyb3V0ZS5jaGlsZHJlbiwgYCR7cGFyZW50VXJsfS8ke3JvdXRlLnBhdGh9YCksXG4gICAgICAgIH0pLFxuICAgIH0pKTtcbiAgfVxuXG4gIHJldHVybiByb3V0ZXMubWFwKHJvdXRlID0+ICh7XG4gICAgLi4ucm91dGUsXG4gICAgdXJsOiBgLyR7cm91dGUucGF0aH1gLFxuICAgIC4uLihyb3V0ZS5jaGlsZHJlbiAmJlxuICAgICAgcm91dGUuY2hpbGRyZW4ubGVuZ3RoICYmIHtcbiAgICAgICAgY2hpbGRyZW46IHNldFVybHMocm91dGUuY2hpbGRyZW4sIGAvJHtyb3V0ZS5wYXRofWApLFxuICAgICAgfSksXG4gIH0pKTtcbn1cblxuZnVuY3Rpb24gZmxhdFJvdXRlcyhyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSk6IEFCUC5GdWxsUm91dGVbXSB7XG4gIGNvbnN0IGZsYXQgPSAocjogQUJQLkZ1bGxSb3V0ZVtdKSA9PiB7XG4gICAgcmV0dXJuIHIucmVkdWNlKChhY2MsIHZhbCkgPT4ge1xuICAgICAgbGV0IHZhbHVlOiBBQlAuRnVsbFJvdXRlW10gPSBbdmFsXTtcbiAgICAgIGlmICh2YWwuY2hpbGRyZW4pIHtcbiAgICAgICAgdmFsdWUgPSBbdmFsLCAuLi5mbGF0KHZhbC5jaGlsZHJlbildO1xuICAgICAgfVxuXG4gICAgICByZXR1cm4gWy4uLmFjYywgLi4udmFsdWVdO1xuICAgIH0sIFtdKTtcbiAgfTtcblxuICByZXR1cm4gZmxhdChyb3V0ZXMpO1xufVxuIl19
