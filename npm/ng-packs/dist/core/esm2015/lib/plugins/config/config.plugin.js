/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { Router } from '@angular/router';
import { actionMatcher, InitState, setValue, UpdateState } from '@ngxs/store';
import snq from 'snq';
import { organizeRoutes, getAbpRoutes } from '../../utils/route-utils';
import clone from 'just-clone';
/** @type {?} */
export const NGXS_CONFIG_PLUGIN_OPTIONS = new InjectionToken('NGXS_CONFIG_PLUGIN_OPTIONS');
export class ConfigPlugin {
  /**
   * @param {?} options
   * @param {?} router
   */
  constructor(options, router) {
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
  handle(state, event, next) {
    /** @type {?} */
    const matches = actionMatcher(event);
    /** @type {?} */
    const isInitAction = matches(InitState) || matches(UpdateState);
    if (isInitAction && !this.initialized) {
      /** @type {?} */
      const transformedRoutes = transformRoutes(this.router.config);
      let { routes } = transformedRoutes;
      const { wrappers } = transformedRoutes;
      routes = organizeRoutes(routes, wrappers);
      /** @type {?} */
      const flattedRoutes = flatRoutes(clone(routes));
      state = setValue(
        state,
        'ConfigState',
        Object.assign({}, state.ConfigState && Object.assign({}, state.ConfigState), this.options, {
          routes,
          flattedRoutes,
        }),
      );
      this.initialized = true;
    }
    return next(state, event);
  }
}
ConfigPlugin.decorators = [{ type: Injectable }];
/** @nocollapse */
ConfigPlugin.ctorParameters = () => [
  { type: undefined, decorators: [{ type: Inject, args: [NGXS_CONFIG_PLUGIN_OPTIONS] }] },
  { type: Router },
];
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
function transformRoutes(routes = [], wrappers = []) {
  // TODO: remove in v1
  /** @type {?} */
  const oldAbpRoutes = routes
    .filter(
      /**
       * @param {?} route
       * @return {?}
       */
      (route => {
        return snq(
          /**
           * @return {?}
           */
          () =>
            route.data.routes.routes.find(
              /**
               * @param {?} r
               * @return {?}
               */
              r => r.path === route.path,
            ),
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
      ((acc, val) => [...acc, ...val.data.routes.routes]),
      [],
    );
  // tslint:disable-next-line: deprecation
  /** @type {?} */
  const abpRoutes = [...getAbpRoutes(), ...oldAbpRoutes];
  wrappers = abpRoutes.filter(
    /**
     * @param {?} ar
     * @return {?}
     */
    ar => ar.wrapper,
  );
  /** @type {?} */
  const transformed = /** @type {?} */ ([]);
  routes
    .filter(
      /**
       * @param {?} route
       * @return {?}
       */
      route => route.component || route.loadChildren,
    )
    .forEach(
      /**
       * @param {?} route
       * @return {?}
       */
      route => {
        /** @type {?} */
        const abpPackage = abpRoutes.find(
          /**
           * @param {?} abp
           * @return {?}
           */
          (abp => abp.path.toLowerCase() === route.path.toLowerCase() && !abp.wrapper),
        );
        const { length } = transformed;
        if (abpPackage) {
          transformed.push(abpPackage);
        }
        if (transformed.length === length && (route.data || {}).routes) {
          transformed.push(
            /** @type {?} */ (Object.assign({}, route.data.routes, {
              path: route.path,
              name: snq(
                /**
                 * @return {?}
                 */
                () => route.data.routes.name,
                route.path,
              ),
              children: route.data.routes.children || [],
            })),
          );
        }
      },
    );
  return { routes: setUrls(transformed), wrappers };
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
      route =>
        Object.assign(
          {},
          route,
          { url: `${parentUrl}/${route.path}` },
          route.children &&
            route.children.length && {
              children: setUrls(route.children, `${parentUrl}/${route.path}`),
            },
        ),
    );
  }
  return routes.map(
    /**
     * @param {?} route
     * @return {?}
     */
    route =>
      Object.assign(
        {},
        route,
        { url: `/${route.path}` },
        route.children &&
          route.children.length && {
            children: setUrls(route.children, `/${route.path}`),
          },
      ),
  );
}
/**
 * @param {?} routes
 * @return {?}
 */
function flatRoutes(routes) {
  /** @type {?} */
  const flat
  /**
   * @param {?} r
   * @return {?}
   */ = (r => {
    return r.reduce(
      /**
       * @param {?} acc
       * @param {?} val
       * @return {?}
       */
      (acc, val) => {
        /** @type {?} */
        let value = [val];
        if (val.children) {
          value = [val, ...flat(val.children)];
        }
        return [...acc, ...value];
      },
      [],
    );
  });
  return flat(routes);
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnBsdWdpbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9wbHVnaW5zL2NvbmZpZy9jb25maWcucGx1Z2luLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFVBQVUsRUFBRSxjQUFjLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDbkUsT0FBTyxFQUFFLE1BQU0sRUFBVSxNQUFNLGlCQUFpQixDQUFDO0FBQ2pELE9BQU8sRUFBRSxhQUFhLEVBQUUsU0FBUyxFQUFnQyxRQUFRLEVBQUUsV0FBVyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVHLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUV0QixPQUFPLEVBQUUsY0FBYyxFQUFFLFlBQVksRUFBRSxNQUFNLHlCQUF5QixDQUFDO0FBQ3ZFLE9BQU8sS0FBSyxNQUFNLFlBQVksQ0FBQzs7QUFFL0IsTUFBTSxPQUFPLDBCQUEwQixHQUFHLElBQUksY0FBYyxDQUFDLDRCQUE0QixDQUFDO0FBRzFGLE1BQU0sT0FBTyxZQUFZOzs7OztJQUd2QixZQUF3RCxPQUFpQixFQUFVLE1BQWM7UUFBekMsWUFBTyxHQUFQLE9BQU8sQ0FBVTtRQUFVLFdBQU0sR0FBTixNQUFNLENBQVE7UUFGekYsZ0JBQVcsR0FBRyxLQUFLLENBQUM7SUFFd0UsQ0FBQzs7Ozs7OztJQUVyRyxNQUFNLENBQUMsS0FBVSxFQUFFLEtBQVUsRUFBRSxJQUFzQjs7Y0FDN0MsT0FBTyxHQUFHLGFBQWEsQ0FBQyxLQUFLLENBQUM7O2NBQzlCLFlBQVksR0FBRyxPQUFPLENBQUMsU0FBUyxDQUFDLElBQUksT0FBTyxDQUFDLFdBQVcsQ0FBQztRQUUvRCxJQUFJLFlBQVksSUFBSSxDQUFDLElBQUksQ0FBQyxXQUFXLEVBQUU7O2tCQUMvQixpQkFBaUIsR0FBRyxlQUFlLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxNQUFNLENBQUM7Z0JBQ3pELEVB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QU07Ozs7O1FBQUMsQ0FBQyxHQUFHLEVBQUUsR0FBRyxFQUFFLEVBQUU7O2dCQUN2QixLQUFLLEdBQW9CLENBQUMsR0FBRyxDQUFDO1lBQ2xDLElBQUksR0FBRyxDQUFDLFFBQVEsRUFBRTtnQkFDaEIsS0FBSyxHQUFHLENBQUMsR0FBRyxFQUFFLEdBQUcsSUFBSSxDQUFDLEdBQUcsQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDO2FBQ3RDO1lBRUQsT0FBTyxDQUFDLEdBQUcsR0FBRyxFQUFFLEdBQUcsS0FBSyxDQUFDLENBQUM7UUFDNUIsQ0FBQyxHQUFFLEVBQUUsQ0FBQyxDQUFDO0lBQ1QsQ0FBQyxDQUFBO0lBRUQsT0FBTyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUM7QUFDdEIsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdCwgSW5qZWN0YWJsZSwgSW5qZWN0aW9uVG9rZW4gfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJvdXRlciwgUm91dGVzIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IGFjdGlvbk1hdGNoZXIsIEluaXRTdGF0ZSwgTmd4c05leHRQbHVnaW5GbiwgTmd4c1BsdWdpbiwgc2V0VmFsdWUsIFVwZGF0ZVN0YXRlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuaW1wb3J0IHsgQUJQIH0gZnJvbSAnLi4vLi4vbW9kZWxzJztcbmltcG9ydCB7IG9yZ2FuaXplUm91dGVzLCBnZXRBYnBSb3V0ZXMgfSBmcm9tICcuLi8uLi91dGlscy9yb3V0ZS11dGlscyc7XG5pbXBvcnQgY2xvbmUgZnJvbSAnanVzdC1jbG9uZSc7XG5cbmV4cG9ydCBjb25zdCBOR1hTX0NPTkZJR19QTFVHSU5fT1BUSU9OUyA9IG5ldyBJbmplY3Rpb25Ub2tlbignTkdYU19DT05GSUdfUExVR0lOX09QVElPTlMnKTtcblxuQEluamVjdGFibGUoKVxuZXhwb3J0IGNsYXNzIENvbmZpZ1BsdWdpbiBpbXBsZW1lbnRzIE5neHNQbHVnaW4ge1xuICBwcml2YXRlIGluaXRpYWxpemVkID0gZmFsc2U7XG5cbiAgY29uc3RydWN0b3IoQEluamVjdChOR1hTX0NPTkZJR19QTFVHSU5fT1BUSU9OUykgcHJpdmF0ZSBvcHRpb25zOiBBQlAuUm9vdCwgcHJpdmF0ZSByb3V0ZXI6IFJvdXRlcikge31cblxuICBoYW5kbGUoc3RhdGU6IGFueSwgZXZlbnQ6IGFueSwgbmV4dDogTmd4c05leHRQbHVnaW5Gbikge1xuICAgIGNvbnN0IG1hdGNoZXMgPSBhY3Rpb25NYXRjaGVyKGV2ZW50KTtcbiAgICBjb25zdCBpc0luaXRBY3Rpb24gPSBtYXRjaGVzKEluaXRTdGF0ZSkgfHwgbWF0Y2hlcyhVcGRhdGVTdGF0ZSk7XG5cbiAgICBpZiAoaXNJbml0QWN0aW9uICYmICF0aGlzLmluaXRpYWxpemVkKSB7XG4gICAgICBjb25zdCB0cmFuc2Zvcm1lZFJvdXRlcyA9IHRyYW5zZm9ybVJvdXRlcyh0aGlzLnJvdXRlci5jb25maWcpO1xuICAgICAgbGV0IHsgcm91dGVzIH0gPSB0cmFuc2Zvcm1lZFJvdXRlcztcbiAgICAgIGNvbnN0IHsgd3JhcHBlcnMgfSA9IHRyYW5zZm9ybWVkUm91dGVzO1xuXG4gICAgICByb3V0ZXMgPSBvcmdhbml6ZVJvdXRlcyhyb3V0ZXMsIHdyYXBwZXJzKTtcbiAgICAgIGNvbnN0IGZsYXR0ZWRSb3V0ZXMgPSBmbGF0Um91dGVzKGNsb25lKHJvdXRlcykpO1xuICAgICAgc3RhdGUgPSBzZXRWYWx1ZShzdGF0ZSwgJ0NvbmZpZ1N0YXRlJywge1xuICAgICAgICAuLi4oc3RhdGUuQ29uZmlnU3RhdGUgJiYgeyAuLi5zdGF0ZS5Db25maWdTdGF0ZSB9KSxcbiAgICAgICAgLi4udGhpcy5vcHRpb25zLFxuICAgICAgICByb3V0ZXMsXG4gICAgICAgIGZsYXR0ZWRSb3V0ZXMsXG4gICAgICB9KTtcblxuICAgICAgdGhpcy5pbml0aWFsaXplZCA9IHRydWU7XG4gICAgfVxuXG4gICAgcmV0dXJuIG5leHQoc3RhdGUsIGV2ZW50KTtcbiAgfVxufVxuXG5mdW5jdGlvbiB0cmFuc2Zvcm1Sb3V0ZXMocm91dGVzOiBSb3V0ZXMgPSBbXSwgd3JhcHBlcnM6IEFCUC5GdWxsUm91dGVbXSA9IFtdKTogYW55IHtcbiAgLy8gVE9ETzogcmVtb3ZlIGluIHYxXG4gIGNvbnN0IG9sZEFicFJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdID0gcm91dGVzXG4gICAgLmZpbHRlcihyb3V0ZSA9PiB7XG4gICAgICByZXR1cm4gc25xKCgpID0+IHJvdXRlLmRhdGEucm91dGVzLnJvdXRlcy5maW5kKHIgPT4gci5wYXRoID09PSByb3V0ZS5wYXRoKSwgZmFsc2UpO1xuICAgIH0pXG4gICAgLnJlZHVjZSgoYWNjLCB2YWwpID0+IFsuLi5hY2MsIC4uLnZhbC5kYXRhLnJvdXRlcy5yb3V0ZXNdLCBbXSk7XG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogZGVwcmVjYXRpb25cbiAgY29uc3QgYWJwUm91dGVzID0gWy4uLmdldEFicFJvdXRlcygpLCAuLi5vbGRBYnBSb3V0ZXNdO1xuXG4gIHdyYXBwZXJzID0gYWJwUm91dGVzLmZpbHRlcihhciA9PiBhci53cmFwcGVyKTtcbiAgY29uc3QgdHJhbnNmb3JtZWQgPSBbXSBhcyBBQlAuRnVsbFJvdXRlW107XG4gIHJvdXRlc1xuICAgIC5maWx0ZXIocm91dGUgPT4gcm91dGUuY29tcG9uZW50IHx8IHJvdXRlLmxvYWRDaGlsZHJlbilcbiAgICAuZm9yRWFjaChyb3V0ZSA9PiB7XG4gICAgICBjb25zdCBhYnBQYWNrYWdlID0gYWJwUm91dGVzLmZpbmQoYWJwID0+IGFicC5wYXRoLnRvTG93ZXJDYXNlKCkgPT09IHJvdXRlLnBhdGgudG9Mb3dlckNhc2UoKSAmJiAhYWJwLndyYXBwZXIpO1xuXG4gICAgICBjb25zdCB7IGxlbmd0aCB9ID0gdHJhbnNmb3JtZWQ7XG5cbiAgICAgIGlmIChhYnBQYWNrYWdlKSB7XG4gICAgICAgIHRyYW5zZm9ybWVkLnB1c2goYWJwUGFja2FnZSk7XG4gICAgICB9XG5cbiAgICAgIGlmICh0cmFuc2Zvcm1lZC5sZW5ndGggPT09IGxlbmd0aCAmJiAocm91dGUuZGF0YSB8fCB7fSkucm91dGVzKSB7XG4gICAgICAgIHRyYW5zZm9ybWVkLnB1c2goe1xuICAgICAgICAgIC4uLnJvdXRlLmRhdGEucm91dGVzLFxuICAgICAgICAgIHBhdGg6IHJvdXRlLnBhdGgsXG4gICAgICAgICAgbmFtZTogc25xKCgpID0+IHJvdXRlLmRhdGEucm91dGVzLm5hbWUsIHJvdXRlLnBhdGgpLFxuICAgICAgICAgIGNoaWxkcmVuOiByb3V0ZS5kYXRhLnJvdXRlcy5jaGlsZHJlbiB8fCBbXSxcbiAgICAgICAgfSBhcyBBQlAuRnVsbFJvdXRlKTtcbiAgICAgIH1cbiAgICB9KTtcblxuICByZXR1cm4geyByb3V0ZXM6IHNldFVybHModHJhbnNmb3JtZWQpLCB3cmFwcGVycyB9O1xufVxuXG5mdW5jdGlvbiBzZXRVcmxzKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdLCBwYXJlbnRVcmw/OiBzdHJpbmcpOiBBQlAuRnVsbFJvdXRlW10ge1xuICBpZiAocGFyZW50VXJsKSB7XG4gICAgLy8gdGhpcyBpZiBibG9jayB1c2luZyBmb3Igb25seSByZWN1cnNpdmUgY2FsbFxuXG4gICAgcmV0dXJuIHJvdXRlcy5tYXAocm91dGUgPT4gKHtcbiAgICAgIC4uLnJvdXRlLFxuICAgICAgdXJsOiBgJHtwYXJlbnRVcmx9LyR7cm91dGUucGF0aH1gLFxuICAgICAgLi4uKHJvdXRlLmNoaWxkcmVuICYmXG4gICAgICAgIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCAmJiB7XG4gICAgICAgICAgY2hpbGRyZW46IHNldFVybHMocm91dGUuY2hpbGRyZW4sIGAke3BhcmVudFVybH0vJHtyb3V0ZS5wYXRofWApLFxuICAgICAgICB9KSxcbiAgICB9KSk7XG4gIH1cblxuICByZXR1cm4gcm91dGVzLm1hcChyb3V0ZSA9PiAoe1xuICAgIC4uLnJvdXRlLFxuICAgIHVybDogYC8ke3JvdXRlLnBhdGh9YCxcbiAgICAuLi4ocm91dGUuY2hpbGRyZW4gJiZcbiAgICAgIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCAmJiB7XG4gICAgICAgIGNoaWxkcmVuOiBzZXRVcmxzKHJvdXRlLmNoaWxkcmVuLCBgLyR7cm91dGUucGF0aH1gKSxcbiAgICAgIH0pLFxuICB9KSk7XG59XG5cbmZ1bmN0aW9uIGZsYXRSb3V0ZXMocm91dGVzOiBBQlAuRnVsbFJvdXRlW10pOiBBQlAuRnVsbFJvdXRlW10ge1xuICBjb25zdCBmbGF0ID0gKHI6IEFCUC5GdWxsUm91dGVbXSkgPT4ge1xuICAgIHJldHVybiByLnJlZHVjZSgoYWNjLCB2YWwpID0+IHtcbiAgICAgIGxldCB2YWx1ZTogQUJQLkZ1bGxSb3V0ZVtdID0gW3ZhbF07XG4gICAgICBpZiAodmFsLmNoaWxkcmVuKSB7XG4gICAgICAgIHZhbHVlID0gW3ZhbCwgLi4uZmxhdCh2YWwuY2hpbGRyZW4pXTtcbiAgICAgIH1cblxuICAgICAgcmV0dXJuIFsuLi5hY2MsIC4uLnZhbHVlXTtcbiAgICB9LCBbXSk7XG4gIH07XG5cbiAgcmV0dXJuIGZsYXQocm91dGVzKTtcbn1cbiJdfQ==
