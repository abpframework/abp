/**
 * @fileoverview added by tsickle
 * Generated from: lib/plugins/config.plugin.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { Router } from '@angular/router';
import { actionMatcher, InitState, setValue, UpdateState } from '@ngxs/store';
import snq from 'snq';
import { organizeRoutes, getAbpRoutes } from '../utils/route-utils';
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
            state = setValue(state, 'ConfigState', Object.assign({}, (state.ConfigState && Object.assign({}, state.ConfigState)), this.options, { routes,
                flattedRoutes }));
            this.initialized = true;
        }
        return next(state, event);
    }
}
ConfigPlugin.decorators = [
    { type: Injectable }
];
/** @nocollapse */
ConfigPlugin.ctorParameters = () => [
    { type: undefined, decorators: [{ type: Inject, args: [NGXS_CONFIG_PLUGIN_OPTIONS,] }] },
    { type: Router }
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
        .filter((/**
     * @param {?} route
     * @return {?}
     */
    route => {
        return snq((/**
         * @return {?}
         */
        () => route.data.routes.routes.find((/**
         * @param {?} r
         * @return {?}
         */
        r => r.path === route.path))), false);
    }))
        .reduce((/**
     * @param {?} acc
     * @param {?} val
     * @return {?}
     */
    (acc, val) => [...acc, ...val.data.routes.routes]), []);
    // tslint:disable-next-line: deprecation
    /** @type {?} */
    const abpRoutes = [...getAbpRoutes(), ...oldAbpRoutes];
    wrappers = abpRoutes.filter((/**
     * @param {?} ar
     * @return {?}
     */
    ar => ar.wrapper));
    /** @type {?} */
    const transformed = (/** @type {?} */ ([]));
    routes
        .filter((/**
     * @param {?} route
     * @return {?}
     */
    route => route.component || route.loadChildren))
        .forEach((/**
     * @param {?} route
     * @return {?}
     */
    route => {
        /** @type {?} */
        const abpPackage = abpRoutes.find((/**
         * @param {?} abp
         * @return {?}
         */
        abp => abp.path.toLowerCase() === route.path.toLowerCase() && !abp.wrapper));
        const { length } = transformed;
        if (abpPackage) {
            transformed.push(abpPackage);
        }
        if (transformed.length === length && (route.data || {}).routes) {
            transformed.push((/** @type {?} */ (Object.assign({}, route.data.routes, { path: route.path, name: snq((/**
                 * @return {?}
                 */
                () => route.data.routes.name), route.path), children: route.data.routes.children || [] }))));
        }
    }));
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
        return routes.map((/**
         * @param {?} route
         * @return {?}
         */
        route => (Object.assign({}, route, { url: `${parentUrl}/${route.path}` }, (route.children &&
            route.children.length && {
            children: setUrls(route.children, `${parentUrl}/${route.path}`),
        })))));
    }
    return routes.map((/**
     * @param {?} route
     * @return {?}
     */
    route => (Object.assign({}, route, { url: `/${route.path}` }, (route.children &&
        route.children.length && {
        children: setUrls(route.children, `/${route.path}`),
    })))));
}
/**
 * @param {?} routes
 * @return {?}
 */
function flatRoutes(routes) {
    /** @type {?} */
    const flat = (/**
     * @param {?} r
     * @return {?}
     */
    (r) => {
        return r.reduce((/**
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
        }), []);
    });
    return flat(routes);
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnBsdWdpbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9wbHVnaW5zL2NvbmZpZy5wbHVnaW4udHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFVBQVUsRUFBRSxjQUFjLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDbkUsT0FBTyxFQUFFLE1BQU0sRUFBVSxNQUFNLGlCQUFpQixDQUFDO0FBQ2pELE9BQU8sRUFBRSxhQUFhLEVBQUUsU0FBUyxFQUFnQyxRQUFRLEVBQUUsV0FBVyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVHLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUV0QixPQUFPLEVBQUUsY0FBYyxFQUFFLFlBQVksRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3BFLE9BQU8sS0FBSyxNQUFNLFlBQVksQ0FBQzs7QUFFL0IsTUFBTSxPQUFPLDBCQUEwQixHQUFHLElBQUksY0FBYyxDQUFDLDRCQUE0QixDQUFDO0FBRzFGLE1BQU0sT0FBTyxZQUFZOzs7OztJQUd2QixZQUF3RCxPQUFpQixFQUFVLE1BQWM7UUFBekMsWUFBTyxHQUFQLE9BQU8sQ0FBVTtRQUFVLFdBQU0sR0FBTixNQUFNLENBQVE7UUFGekYsZ0JBQVcsR0FBRyxLQUFLLENBQUM7SUFFd0UsQ0FBQzs7Ozs7OztJQUVyRyxNQUFNLENBQUMsS0FBVSxFQUFFLEtBQVUsRUFBRSxJQUFzQjs7Y0FDN0MsT0FBTyxHQUFHLGFBQWEsQ0FBQyxLQUFLLENBQUM7O2NBQzlCLFlBQVksR0FBRyxPQUFPLENBQUMsU0FBUyxDQUFDLElBQUksT0FBTyxDQUFDLFdBQVcsQ0FBQztRQUUvRCxJQUFJLFlBQVksSUFBSSxDQUFDLElBQUksQ0FBQyxXQUFXLEVBQUU7O2tCQUMvQixpQkFBaUIsR0FBRyxlQUFlLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxNQUFNLENBQUM7Z0JBQ3pELEVBQUUsTUFB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O1FBQUMsQ0FBQyxHQUFHLEVBQUUsR0FBRyxFQUFFLEVBQUU7O2dCQUN2QixLQUFLLEdBQW9CLENBQUMsR0FBRyxDQUFDO1lBQ2xDLElBQUksR0FBRyxDQUFDLFFBQVEsRUFBRTtnQkFDaEIsS0FBSyxHQUFHLENBQUMsR0FBRyxFQUFFLEdBQUcsSUFBSSxDQUFDLEdBQUcsQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDO2FBQ3RDO1lBRUQsT0FBTyxDQUFDLEdBQUcsR0FBRyxFQUFFLEdBQUcsS0FBSyxDQUFDLENBQUM7UUFDNUIsQ0FBQyxHQUFFLEVBQUUsQ0FBQyxDQUFDO0lBQ1QsQ0FBQyxDQUFBO0lBRUQsT0FBTyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUM7QUFDdEIsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdCwgSW5qZWN0YWJsZSwgSW5qZWN0aW9uVG9rZW4gfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgUm91dGVyLCBSb3V0ZXMgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xyXG5pbXBvcnQgeyBhY3Rpb25NYXRjaGVyLCBJbml0U3RhdGUsIE5neHNOZXh0UGx1Z2luRm4sIE5neHNQbHVnaW4sIHNldFZhbHVlLCBVcGRhdGVTdGF0ZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xyXG5pbXBvcnQgeyBBQlAgfSBmcm9tICcuLi9tb2RlbHMnO1xyXG5pbXBvcnQgeyBvcmdhbml6ZVJvdXRlcywgZ2V0QWJwUm91dGVzIH0gZnJvbSAnLi4vdXRpbHMvcm91dGUtdXRpbHMnO1xyXG5pbXBvcnQgY2xvbmUgZnJvbSAnanVzdC1jbG9uZSc7XHJcblxyXG5leHBvcnQgY29uc3QgTkdYU19DT05GSUdfUExVR0lOX09QVElPTlMgPSBuZXcgSW5qZWN0aW9uVG9rZW4oJ05HWFNfQ09ORklHX1BMVUdJTl9PUFRJT05TJyk7XHJcblxyXG5ASW5qZWN0YWJsZSgpXHJcbmV4cG9ydCBjbGFzcyBDb25maWdQbHVnaW4gaW1wbGVtZW50cyBOZ3hzUGx1Z2luIHtcclxuICBwcml2YXRlIGluaXRpYWxpemVkID0gZmFsc2U7XHJcblxyXG4gIGNvbnN0cnVjdG9yKEBJbmplY3QoTkdYU19DT05GSUdfUExVR0lOX09QVElPTlMpIHByaXZhdGUgb3B0aW9uczogQUJQLlJvb3QsIHByaXZhdGUgcm91dGVyOiBSb3V0ZXIpIHt9XHJcblxyXG4gIGhhbmRsZShzdGF0ZTogYW55LCBldmVudDogYW55LCBuZXh0OiBOZ3hzTmV4dFBsdWdpbkZuKSB7XHJcbiAgICBjb25zdCBtYXRjaGVzID0gYWN0aW9uTWF0Y2hlcihldmVudCk7XHJcbiAgICBjb25zdCBpc0luaXRBY3Rpb24gPSBtYXRjaGVzKEluaXRTdGF0ZSkgfHwgbWF0Y2hlcyhVcGRhdGVTdGF0ZSk7XHJcblxyXG4gICAgaWYgKGlzSW5pdEFjdGlvbiAmJiAhdGhpcy5pbml0aWFsaXplZCkge1xyXG4gICAgICBjb25zdCB0cmFuc2Zvcm1lZFJvdXRlcyA9IHRyYW5zZm9ybVJvdXRlcyh0aGlzLnJvdXRlci5jb25maWcpO1xyXG4gICAgICBsZXQgeyByb3V0ZXMgfSA9IHRyYW5zZm9ybWVkUm91dGVzO1xyXG4gICAgICBjb25zdCB7IHdyYXBwZXJzIH0gPSB0cmFuc2Zvcm1lZFJvdXRlcztcclxuXHJcbiAgICAgIHJvdXRlcyA9IG9yZ2FuaXplUm91dGVzKHJvdXRlcywgd3JhcHBlcnMpO1xyXG4gICAgICBjb25zdCBmbGF0dGVkUm91dGVzID0gZmxhdFJvdXRlcyhjbG9uZShyb3V0ZXMpKTtcclxuICAgICAgc3RhdGUgPSBzZXRWYWx1ZShzdGF0ZSwgJ0NvbmZpZ1N0YXRlJywge1xyXG4gICAgICAgIC4uLihzdGF0ZS5Db25maWdTdGF0ZSAmJiB7IC4uLnN0YXRlLkNvbmZpZ1N0YXRlIH0pLFxyXG4gICAgICAgIC4uLnRoaXMub3B0aW9ucyxcclxuICAgICAgICByb3V0ZXMsXHJcbiAgICAgICAgZmxhdHRlZFJvdXRlcyxcclxuICAgICAgfSk7XHJcblxyXG4gICAgICB0aGlzLmluaXRpYWxpemVkID0gdHJ1ZTtcclxuICAgIH1cclxuXHJcbiAgICByZXR1cm4gbmV4dChzdGF0ZSwgZXZlbnQpO1xyXG4gIH1cclxufVxyXG5cclxuZnVuY3Rpb24gdHJhbnNmb3JtUm91dGVzKHJvdXRlczogUm91dGVzID0gW10sIHdyYXBwZXJzOiBBQlAuRnVsbFJvdXRlW10gPSBbXSk6IGFueSB7XHJcbiAgLy8gVE9ETzogcmVtb3ZlIGluIHYxXHJcbiAgY29uc3Qgb2xkQWJwUm91dGVzOiBBQlAuRnVsbFJvdXRlW10gPSByb3V0ZXNcclxuICAgIC5maWx0ZXIocm91dGUgPT4ge1xyXG4gICAgICByZXR1cm4gc25xKCgpID0+IHJvdXRlLmRhdGEucm91dGVzLnJvdXRlcy5maW5kKHIgPT4gci5wYXRoID09PSByb3V0ZS5wYXRoKSwgZmFsc2UpO1xyXG4gICAgfSlcclxuICAgIC5yZWR1Y2UoKGFjYywgdmFsKSA9PiBbLi4uYWNjLCAuLi52YWwuZGF0YS5yb3V0ZXMucm91dGVzXSwgW10pO1xyXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogZGVwcmVjYXRpb25cclxuICBjb25zdCBhYnBSb3V0ZXMgPSBbLi4uZ2V0QWJwUm91dGVzKCksIC4uLm9sZEFicFJvdXRlc107XHJcblxyXG4gIHdyYXBwZXJzID0gYWJwUm91dGVzLmZpbHRlcihhciA9PiBhci53cmFwcGVyKTtcclxuICBjb25zdCB0cmFuc2Zvcm1lZCA9IFtdIGFzIEFCUC5GdWxsUm91dGVbXTtcclxuICByb3V0ZXNcclxuICAgIC5maWx0ZXIocm91dGUgPT4gcm91dGUuY29tcG9uZW50IHx8IHJvdXRlLmxvYWRDaGlsZHJlbilcclxuICAgIC5mb3JFYWNoKHJvdXRlID0+IHtcclxuICAgICAgY29uc3QgYWJwUGFja2FnZSA9IGFicFJvdXRlcy5maW5kKGFicCA9PiBhYnAucGF0aC50b0xvd2VyQ2FzZSgpID09PSByb3V0ZS5wYXRoLnRvTG93ZXJDYXNlKCkgJiYgIWFicC53cmFwcGVyKTtcclxuXHJcbiAgICAgIGNvbnN0IHsgbGVuZ3RoIH0gPSB0cmFuc2Zvcm1lZDtcclxuXHJcbiAgICAgIGlmIChhYnBQYWNrYWdlKSB7XHJcbiAgICAgICAgdHJhbnNmb3JtZWQucHVzaChhYnBQYWNrYWdlKTtcclxuICAgICAgfVxyXG5cclxuICAgICAgaWYgKHRyYW5zZm9ybWVkLmxlbmd0aCA9PT0gbGVuZ3RoICYmIChyb3V0ZS5kYXRhIHx8IHt9KS5yb3V0ZXMpIHtcclxuICAgICAgICB0cmFuc2Zvcm1lZC5wdXNoKHtcclxuICAgICAgICAgIC4uLnJvdXRlLmRhdGEucm91dGVzLFxyXG4gICAgICAgICAgcGF0aDogcm91dGUucGF0aCxcclxuICAgICAgICAgIG5hbWU6IHNucSgoKSA9PiByb3V0ZS5kYXRhLnJvdXRlcy5uYW1lLCByb3V0ZS5wYXRoKSxcclxuICAgICAgICAgIGNoaWxkcmVuOiByb3V0ZS5kYXRhLnJvdXRlcy5jaGlsZHJlbiB8fCBbXSxcclxuICAgICAgICB9IGFzIEFCUC5GdWxsUm91dGUpO1xyXG4gICAgICB9XHJcbiAgICB9KTtcclxuXHJcbiAgcmV0dXJuIHsgcm91dGVzOiBzZXRVcmxzKHRyYW5zZm9ybWVkKSwgd3JhcHBlcnMgfTtcclxufVxyXG5cclxuZnVuY3Rpb24gc2V0VXJscyhyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSwgcGFyZW50VXJsPzogc3RyaW5nKTogQUJQLkZ1bGxSb3V0ZVtdIHtcclxuICBpZiAocGFyZW50VXJsKSB7XHJcbiAgICAvLyB0aGlzIGlmIGJsb2NrIHVzaW5nIGZvciBvbmx5IHJlY3Vyc2l2ZSBjYWxsXHJcblxyXG4gICAgcmV0dXJuIHJvdXRlcy5tYXAocm91dGUgPT4gKHtcclxuICAgICAgLi4ucm91dGUsXHJcbiAgICAgIHVybDogYCR7cGFyZW50VXJsfS8ke3JvdXRlLnBhdGh9YCxcclxuICAgICAgLi4uKHJvdXRlLmNoaWxkcmVuICYmXHJcbiAgICAgICAgcm91dGUuY2hpbGRyZW4ubGVuZ3RoICYmIHtcclxuICAgICAgICAgIGNoaWxkcmVuOiBzZXRVcmxzKHJvdXRlLmNoaWxkcmVuLCBgJHtwYXJlbnRVcmx9LyR7cm91dGUucGF0aH1gKSxcclxuICAgICAgICB9KSxcclxuICAgIH0pKTtcclxuICB9XHJcblxyXG4gIHJldHVybiByb3V0ZXMubWFwKHJvdXRlID0+ICh7XHJcbiAgICAuLi5yb3V0ZSxcclxuICAgIHVybDogYC8ke3JvdXRlLnBhdGh9YCxcclxuICAgIC4uLihyb3V0ZS5jaGlsZHJlbiAmJlxyXG4gICAgICByb3V0ZS5jaGlsZHJlbi5sZW5ndGggJiYge1xyXG4gICAgICAgIGNoaWxkcmVuOiBzZXRVcmxzKHJvdXRlLmNoaWxkcmVuLCBgLyR7cm91dGUucGF0aH1gKSxcclxuICAgICAgfSksXHJcbiAgfSkpO1xyXG59XHJcblxyXG5mdW5jdGlvbiBmbGF0Um91dGVzKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdKTogQUJQLkZ1bGxSb3V0ZVtdIHtcclxuICBjb25zdCBmbGF0ID0gKHI6IEFCUC5GdWxsUm91dGVbXSkgPT4ge1xyXG4gICAgcmV0dXJuIHIucmVkdWNlKChhY2MsIHZhbCkgPT4ge1xyXG4gICAgICBsZXQgdmFsdWU6IEFCUC5GdWxsUm91dGVbXSA9IFt2YWxdO1xyXG4gICAgICBpZiAodmFsLmNoaWxkcmVuKSB7XHJcbiAgICAgICAgdmFsdWUgPSBbdmFsLCAuLi5mbGF0KHZhbC5jaGlsZHJlbildO1xyXG4gICAgICB9XHJcblxyXG4gICAgICByZXR1cm4gWy4uLmFjYywgLi4udmFsdWVdO1xyXG4gICAgfSwgW10pO1xyXG4gIH07XHJcblxyXG4gIHJldHVybiBmbGF0KHJvdXRlcyk7XHJcbn1cclxuIl19