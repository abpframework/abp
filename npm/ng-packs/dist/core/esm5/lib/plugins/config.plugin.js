/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { Router } from '@angular/router';
import { actionMatcher, InitState, setValue, UpdateState } from '@ngxs/store';
import snq from 'snq';
import { organizeRoutes, getAbpRoutes } from '../utils/route-utils';
import clone from 'just-clone';
/** @type {?} */
export var NGXS_CONFIG_PLUGIN_OPTIONS = new InjectionToken('NGXS_CONFIG_PLUGIN_OPTIONS');
var ConfigPlugin = /** @class */ (function () {
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
    ConfigPlugin.prototype.handle = /**
     * @param {?} state
     * @param {?} event
     * @param {?} next
     * @return {?}
     */
    function (state, event, next) {
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
            state = setValue(state, 'ConfigState', tslib_1.__assign({}, (state.ConfigState && tslib_1.__assign({}, state.ConfigState)), this.options, { routes: routes,
                flattedRoutes: flattedRoutes }));
            this.initialized = true;
        }
        return next(state, event);
    };
    ConfigPlugin.decorators = [
        { type: Injectable }
    ];
    /** @nocollapse */
    ConfigPlugin.ctorParameters = function () { return [
        { type: undefined, decorators: [{ type: Inject, args: [NGXS_CONFIG_PLUGIN_OPTIONS,] }] },
        { type: Router }
    ]; };
    return ConfigPlugin;
}());
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
    if (routes === void 0) { routes = []; }
    if (wrappers === void 0) { wrappers = []; }
    // TODO: remove in v1
    /** @type {?} */
    var oldAbpRoutes = routes
        .filter((/**
     * @param {?} route
     * @return {?}
     */
    function (route) {
        return snq((/**
         * @return {?}
         */
        function () { return route.data.routes.routes.find((/**
         * @param {?} r
         * @return {?}
         */
        function (r) { return r.path === route.path; })); }), false);
    }))
        .reduce((/**
     * @param {?} acc
     * @param {?} val
     * @return {?}
     */
    function (acc, val) { return tslib_1.__spread(acc, val.data.routes.routes); }), []);
    // tslint:disable-next-line: deprecation
    /** @type {?} */
    var abpRoutes = tslib_1.__spread(getAbpRoutes(), oldAbpRoutes);
    wrappers = abpRoutes.filter((/**
     * @param {?} ar
     * @return {?}
     */
    function (ar) { return ar.wrapper; }));
    /** @type {?} */
    var transformed = (/** @type {?} */ ([]));
    routes
        .filter((/**
     * @param {?} route
     * @return {?}
     */
    function (route) { return route.component || route.loadChildren; }))
        .forEach((/**
     * @param {?} route
     * @return {?}
     */
    function (route) {
        /** @type {?} */
        var abpPackage = abpRoutes.find((/**
         * @param {?} abp
         * @return {?}
         */
        function (abp) { return abp.path.toLowerCase() === route.path.toLowerCase() && !abp.wrapper; }));
        var length = transformed.length;
        if (abpPackage) {
            transformed.push(abpPackage);
        }
        if (transformed.length === length && (route.data || {}).routes) {
            transformed.push((/** @type {?} */ (tslib_1.__assign({}, route.data.routes, { path: route.path, name: snq((/**
                 * @return {?}
                 */
                function () { return route.data.routes.name; }), route.path), children: route.data.routes.children || [] }))));
        }
    }));
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
        return routes.map((/**
         * @param {?} route
         * @return {?}
         */
        function (route) { return (tslib_1.__assign({}, route, { url: parentUrl + "/" + route.path }, (route.children &&
            route.children.length && {
            children: setUrls(route.children, parentUrl + "/" + route.path),
        }))); }));
    }
    return routes.map((/**
     * @param {?} route
     * @return {?}
     */
    function (route) { return (tslib_1.__assign({}, route, { url: "/" + route.path }, (route.children &&
        route.children.length && {
        children: setUrls(route.children, "/" + route.path),
    }))); }));
}
/**
 * @param {?} routes
 * @return {?}
 */
function flatRoutes(routes) {
    /** @type {?} */
    var flat = (/**
     * @param {?} r
     * @return {?}
     */
    function (r) {
        return r.reduce((/**
         * @param {?} acc
         * @param {?} val
         * @return {?}
         */
        function (acc, val) {
            /** @type {?} */
            var value = [val];
            if (val.children) {
                value = tslib_1.__spread([val], flat(val.children));
            }
            return tslib_1.__spread(acc, value);
        }), []);
    });
    return flat(routes);
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnBsdWdpbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9wbHVnaW5zL2NvbmZpZy5wbHVnaW4udHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFVBQVUsRUFBRSxjQUFjLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDbkUsT0FBTyxFQUFFLE1BQU0sRUFBVSxNQUFNLGlCQUFpQixDQUFDO0FBQ2pELE9BQU8sRUFBRSxhQUFhLEVBQUUsU0FBUyxFQUFnQyxRQUFRLEVBQUUsV0FBVyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVHLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUV0QixPQUFPLEVBQUUsY0FBYyxFQUFFLFlBQVksRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3BFLE9BQU8sS0FBSyxNQUFNLFlBQVksQ0FBQzs7QUFFL0IsTUFBTSxLQUFPLDBCQUEwQixHQUFHLElBQUksY0FBYyxDQUFDLDRCQUE0QixDQUFDO0FBRTFGO0lBSUUsc0JBQXdELE9BQWlCLEVBQVUsTUFBYztRQUF6QyxZQUFPLEdBQVAsT0FBTyxDQUFVO1FBQVUsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUZ6RixnQkFBVyxHQUFHLEtBQUssQ0FBQztJQUV3RSxDQUFDOzs7Ozs7O0lBRXJHLDZCQUFNOzs7Ozs7SUFBTixVQUFPLEtBQVUsRUFBRSxLQUFVLEVBQUUsSUFBc0I7O1lBQzdDLE9BQU8sR0FBRyxhQUFhLENBQUMsS0FBSyxDQUFDOztZQUM5QixZQUFZLEdBQUcsT0FBTyxDQUFDLFNBQVMsQ0FBQyxJQUFJLE9BQU8sQ0FBQyxXQUFXLENBQUM7UUFFL0QsSUFBSSxZQUFZLElBQUksQ0FBQyxJQUFJLENBQUMsV0FBVyxFQUFFOztnQkFDL0IsaUJBQWlCLEdBQUcsZUFBZSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsTUFBTSxDQUFDO1lBQ3ZELElBQUEsaUNBQU07WUFDSixJ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RyxVQUFDLENBQWtCO1FBQzlCLE9BQU8sQ0FBQyxDQUFDLE1BQU07Ozs7O1FBQUMsVUFBQyxHQUFHLEVBQUUsR0FBRzs7Z0JBQ25CLEtBQUssR0FBb0IsQ0FBQyxHQUFHLENBQUM7WUFDbEMsSUFBSSxHQUFHLENBQUMsUUFBUSxFQUFFO2dCQUNoQixLQUFLLHFCQUFJLEdBQUcsR0FBSyxJQUFJLENBQUMsR0FBRyxDQUFDLFFBQVEsQ0FBQyxDQUFDLENBQUM7YUFDdEM7WUFFRCx3QkFBVyxHQUFHLEVBQUssS0FBSyxFQUFFO1FBQzVCLENBQUMsR0FBRSxFQUFFLENBQUMsQ0FBQztJQUNULENBQUMsQ0FBQTtJQUVELE9BQU8sSUFBSSxDQUFDLE1BQU0sQ0FBQyxDQUFDO0FBQ3RCLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3QsIEluamVjdGFibGUsIEluamVjdGlvblRva2VuIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBSb3V0ZXIsIFJvdXRlcyB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBhY3Rpb25NYXRjaGVyLCBJbml0U3RhdGUsIE5neHNOZXh0UGx1Z2luRm4sIE5neHNQbHVnaW4sIHNldFZhbHVlLCBVcGRhdGVTdGF0ZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcbmltcG9ydCB7IEFCUCB9IGZyb20gJy4uL21vZGVscyc7XG5pbXBvcnQgeyBvcmdhbml6ZVJvdXRlcywgZ2V0QWJwUm91dGVzIH0gZnJvbSAnLi4vdXRpbHMvcm91dGUtdXRpbHMnO1xuaW1wb3J0IGNsb25lIGZyb20gJ2p1c3QtY2xvbmUnO1xuXG5leHBvcnQgY29uc3QgTkdYU19DT05GSUdfUExVR0lOX09QVElPTlMgPSBuZXcgSW5qZWN0aW9uVG9rZW4oJ05HWFNfQ09ORklHX1BMVUdJTl9PUFRJT05TJyk7XG5cbkBJbmplY3RhYmxlKClcbmV4cG9ydCBjbGFzcyBDb25maWdQbHVnaW4gaW1wbGVtZW50cyBOZ3hzUGx1Z2luIHtcbiAgcHJpdmF0ZSBpbml0aWFsaXplZCA9IGZhbHNlO1xuXG4gIGNvbnN0cnVjdG9yKEBJbmplY3QoTkdYU19DT05GSUdfUExVR0lOX09QVElPTlMpIHByaXZhdGUgb3B0aW9uczogQUJQLlJvb3QsIHByaXZhdGUgcm91dGVyOiBSb3V0ZXIpIHt9XG5cbiAgaGFuZGxlKHN0YXRlOiBhbnksIGV2ZW50OiBhbnksIG5leHQ6IE5neHNOZXh0UGx1Z2luRm4pIHtcbiAgICBjb25zdCBtYXRjaGVzID0gYWN0aW9uTWF0Y2hlcihldmVudCk7XG4gICAgY29uc3QgaXNJbml0QWN0aW9uID0gbWF0Y2hlcyhJbml0U3RhdGUpIHx8IG1hdGNoZXMoVXBkYXRlU3RhdGUpO1xuXG4gICAgaWYgKGlzSW5pdEFjdGlvbiAmJiAhdGhpcy5pbml0aWFsaXplZCkge1xuICAgICAgY29uc3QgdHJhbnNmb3JtZWRSb3V0ZXMgPSB0cmFuc2Zvcm1Sb3V0ZXModGhpcy5yb3V0ZXIuY29uZmlnKTtcbiAgICAgIGxldCB7IHJvdXRlcyB9ID0gdHJhbnNmb3JtZWRSb3V0ZXM7XG4gICAgICBjb25zdCB7IHdyYXBwZXJzIH0gPSB0cmFuc2Zvcm1lZFJvdXRlcztcblxuICAgICAgcm91dGVzID0gb3JnYW5pemVSb3V0ZXMocm91dGVzLCB3cmFwcGVycyk7XG4gICAgICBjb25zdCBmbGF0dGVkUm91dGVzID0gZmxhdFJvdXRlcyhjbG9uZShyb3V0ZXMpKTtcbiAgICAgIHN0YXRlID0gc2V0VmFsdWUoc3RhdGUsICdDb25maWdTdGF0ZScsIHtcbiAgICAgICAgLi4uKHN0YXRlLkNvbmZpZ1N0YXRlICYmIHsgLi4uc3RhdGUuQ29uZmlnU3RhdGUgfSksXG4gICAgICAgIC4uLnRoaXMub3B0aW9ucyxcbiAgICAgICAgcm91dGVzLFxuICAgICAgICBmbGF0dGVkUm91dGVzLFxuICAgICAgfSk7XG5cbiAgICAgIHRoaXMuaW5pdGlhbGl6ZWQgPSB0cnVlO1xuICAgIH1cblxuICAgIHJldHVybiBuZXh0KHN0YXRlLCBldmVudCk7XG4gIH1cbn1cblxuZnVuY3Rpb24gdHJhbnNmb3JtUm91dGVzKHJvdXRlczogUm91dGVzID0gW10sIHdyYXBwZXJzOiBBQlAuRnVsbFJvdXRlW10gPSBbXSk6IGFueSB7XG4gIC8vIFRPRE86IHJlbW92ZSBpbiB2MVxuICBjb25zdCBvbGRBYnBSb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSA9IHJvdXRlc1xuICAgIC5maWx0ZXIocm91dGUgPT4ge1xuICAgICAgcmV0dXJuIHNucSgoKSA9PiByb3V0ZS5kYXRhLnJvdXRlcy5yb3V0ZXMuZmluZChyID0+IHIucGF0aCA9PT0gcm91dGUucGF0aCksIGZhbHNlKTtcbiAgICB9KVxuICAgIC5yZWR1Y2UoKGFjYywgdmFsKSA9PiBbLi4uYWNjLCAuLi52YWwuZGF0YS5yb3V0ZXMucm91dGVzXSwgW10pO1xuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IGRlcHJlY2F0aW9uXG4gIGNvbnN0IGFicFJvdXRlcyA9IFsuLi5nZXRBYnBSb3V0ZXMoKSwgLi4ub2xkQWJwUm91dGVzXTtcblxuICB3cmFwcGVycyA9IGFicFJvdXRlcy5maWx0ZXIoYXIgPT4gYXIud3JhcHBlcik7XG4gIGNvbnN0IHRyYW5zZm9ybWVkID0gW10gYXMgQUJQLkZ1bGxSb3V0ZVtdO1xuICByb3V0ZXNcbiAgICAuZmlsdGVyKHJvdXRlID0+IHJvdXRlLmNvbXBvbmVudCB8fCByb3V0ZS5sb2FkQ2hpbGRyZW4pXG4gICAgLmZvckVhY2gocm91dGUgPT4ge1xuICAgICAgY29uc3QgYWJwUGFja2FnZSA9IGFicFJvdXRlcy5maW5kKGFicCA9PiBhYnAucGF0aC50b0xvd2VyQ2FzZSgpID09PSByb3V0ZS5wYXRoLnRvTG93ZXJDYXNlKCkgJiYgIWFicC53cmFwcGVyKTtcblxuICAgICAgY29uc3QgeyBsZW5ndGggfSA9IHRyYW5zZm9ybWVkO1xuXG4gICAgICBpZiAoYWJwUGFja2FnZSkge1xuICAgICAgICB0cmFuc2Zvcm1lZC5wdXNoKGFicFBhY2thZ2UpO1xuICAgICAgfVxuXG4gICAgICBpZiAodHJhbnNmb3JtZWQubGVuZ3RoID09PSBsZW5ndGggJiYgKHJvdXRlLmRhdGEgfHwge30pLnJvdXRlcykge1xuICAgICAgICB0cmFuc2Zvcm1lZC5wdXNoKHtcbiAgICAgICAgICAuLi5yb3V0ZS5kYXRhLnJvdXRlcyxcbiAgICAgICAgICBwYXRoOiByb3V0ZS5wYXRoLFxuICAgICAgICAgIG5hbWU6IHNucSgoKSA9PiByb3V0ZS5kYXRhLnJvdXRlcy5uYW1lLCByb3V0ZS5wYXRoKSxcbiAgICAgICAgICBjaGlsZHJlbjogcm91dGUuZGF0YS5yb3V0ZXMuY2hpbGRyZW4gfHwgW10sXG4gICAgICAgIH0gYXMgQUJQLkZ1bGxSb3V0ZSk7XG4gICAgICB9XG4gICAgfSk7XG5cbiAgcmV0dXJuIHsgcm91dGVzOiBzZXRVcmxzKHRyYW5zZm9ybWVkKSwgd3JhcHBlcnMgfTtcbn1cblxuZnVuY3Rpb24gc2V0VXJscyhyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSwgcGFyZW50VXJsPzogc3RyaW5nKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgaWYgKHBhcmVudFVybCkge1xuICAgIC8vIHRoaXMgaWYgYmxvY2sgdXNpbmcgZm9yIG9ubHkgcmVjdXJzaXZlIGNhbGxcblxuICAgIHJldHVybiByb3V0ZXMubWFwKHJvdXRlID0+ICh7XG4gICAgICAuLi5yb3V0ZSxcbiAgICAgIHVybDogYCR7cGFyZW50VXJsfS8ke3JvdXRlLnBhdGh9YCxcbiAgICAgIC4uLihyb3V0ZS5jaGlsZHJlbiAmJlxuICAgICAgICByb3V0ZS5jaGlsZHJlbi5sZW5ndGggJiYge1xuICAgICAgICAgIGNoaWxkcmVuOiBzZXRVcmxzKHJvdXRlLmNoaWxkcmVuLCBgJHtwYXJlbnRVcmx9LyR7cm91dGUucGF0aH1gKSxcbiAgICAgICAgfSksXG4gICAgfSkpO1xuICB9XG5cbiAgcmV0dXJuIHJvdXRlcy5tYXAocm91dGUgPT4gKHtcbiAgICAuLi5yb3V0ZSxcbiAgICB1cmw6IGAvJHtyb3V0ZS5wYXRofWAsXG4gICAgLi4uKHJvdXRlLmNoaWxkcmVuICYmXG4gICAgICByb3V0ZS5jaGlsZHJlbi5sZW5ndGggJiYge1xuICAgICAgICBjaGlsZHJlbjogc2V0VXJscyhyb3V0ZS5jaGlsZHJlbiwgYC8ke3JvdXRlLnBhdGh9YCksXG4gICAgICB9KSxcbiAgfSkpO1xufVxuXG5mdW5jdGlvbiBmbGF0Um91dGVzKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgY29uc3QgZmxhdCA9IChyOiBBQlAuRnVsbFJvdXRlW10pID0+IHtcbiAgICByZXR1cm4gci5yZWR1Y2UoKGFjYywgdmFsKSA9PiB7XG4gICAgICBsZXQgdmFsdWU6IEFCUC5GdWxsUm91dGVbXSA9IFt2YWxdO1xuICAgICAgaWYgKHZhbC5jaGlsZHJlbikge1xuICAgICAgICB2YWx1ZSA9IFt2YWwsIC4uLmZsYXQodmFsLmNoaWxkcmVuKV07XG4gICAgICB9XG5cbiAgICAgIHJldHVybiBbLi4uYWNjLCAuLi52YWx1ZV07XG4gICAgfSwgW10pO1xuICB9O1xuXG4gIHJldHVybiBmbGF0KHJvdXRlcyk7XG59XG4iXX0=