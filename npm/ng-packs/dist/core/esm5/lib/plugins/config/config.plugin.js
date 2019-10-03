/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { Router } from '@angular/router';
import { actionMatcher, InitState, setValue, UpdateState } from '@ngxs/store';
import snq from 'snq';
import { organizeRoutes } from '../../utils/route-utils';
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
        // const layouts = snq(() => this.options.requirements.layouts.filter(layout => layout instanceof Type), []);
        if (isInitAction && !this.initialized) {
            var _a = transformRoutes(this.router.config), routes = _a.routes, wrappers = _a.wrappers;
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
    /** @type {?} */
    var abpRoutes = routes
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
    function (route) { return (route.data || {}).routes && (route.component || route.loadChildren); }))
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
        function (abp) { return abp.path.toLowerCase() === route.path.toLowerCase() && snq((/**
         * @return {?}
         */
        function () { return route.data.routes.routes.length; }), false); }));
        var length = transformed.length;
        if (abpPackage) {
            transformed.push(abpPackage);
        }
        if (transformed.length === length) {
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnBsdWdpbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9wbHVnaW5zL2NvbmZpZy9jb25maWcucGx1Z2luLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLE1BQU0sRUFBRSxVQUFVLEVBQUUsY0FBYyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ25FLE9BQU8sRUFBRSxNQUFNLEVBQVUsTUFBTSxpQkFBaUIsQ0FBQztBQUNqRCxPQUFPLEVBQUUsYUFBYSxFQUFFLFNBQVMsRUFBZ0MsUUFBUSxFQUFFLFdBQVcsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1RyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFFdEIsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHlCQUF5QixDQUFDO0FBQ3pELE9BQU8sS0FBSyxNQUFNLFlBQVksQ0FBQzs7QUFFL0IsTUFBTSxLQUFPLDBCQUEwQixHQUFHLElBQUksY0FBYyxDQUFDLDRCQUE0QixDQUFDO0FBRTFGO0lBSUUsc0JBQXdELE9BQWlCLEVBQVUsTUFBYztRQUF6QyxZQUFPLEdBQVAsT0FBTyxDQUFVO1FBQVUsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUZ6RixnQkFBVyxHQUFZLEtBQUssQ0FBQztJQUUrRCxDQUFDOzs7Ozs7O0lBRXJHLDZCQUFNOzs7Ozs7SUFBTixVQUFPLEtBQVUsRUFBRSxLQUFVLEVBQUUsSUFBc0I7O1lBQzdDLE9BQU8sR0FBRyxhQUFhLENBQUMsS0FBSyxDQUFDOztZQUM5QixZQUFZLEdBQUcsT0FBTyxDQUFDLFNBQVMsQ0FBQyxJQUFJLE9BQU8sQ0FBQyxXQUFXLENBQUM7UUFFL0QsNkdBQTZHO1FBQzdHLElBQUksWUFBWSxJQUFJLENBQUMsSUFBSSxDQUFDLFdBQVcsRUFBRTtZQUNqQyxJQUFBLHdDQUEwRCxFQUF4RCxrQkFBTSxFQUFFLHNCQUFnRDtZQUM5RCxNQUFNLEdBQUcsY0FBYyxDQUFDLE1BQU0s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LFVBQUMsQ0FBa0I7UUFDOUIsT0FBTyxDQUFDLENBQUMsTUFBTTs7Ozs7UUFBQyxVQUFDLEdBQUcsRUFBRSxHQUFHOztnQkFDbkIsS0FBSyxHQUFvQixDQUFDLEdBQUcsQ0FBQztZQUNsQyxJQUFJLEdBQUcsQ0FBQyxRQUFRLEVBQUU7Z0JBQ2hCLEtBQUsscUJBQUksR0FBRyxHQUFLLElBQUksQ0FBQyxHQUFHLENBQUMsUUFBUSxDQUFDLENBQUMsQ0FBQzthQUN0QztZQUVELHdCQUFXLEdBQUcsRUFBSyxLQUFLLEVBQUU7UUFDNUIsQ0FBQyxHQUFFLEVBQUUsQ0FBQyxDQUFDO0lBQ1QsQ0FBQyxDQUFBO0lBRUQsT0FBTyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUM7QUFDdEIsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdCwgSW5qZWN0YWJsZSwgSW5qZWN0aW9uVG9rZW4gfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJvdXRlciwgUm91dGVzIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IGFjdGlvbk1hdGNoZXIsIEluaXRTdGF0ZSwgTmd4c05leHRQbHVnaW5GbiwgTmd4c1BsdWdpbiwgc2V0VmFsdWUsIFVwZGF0ZVN0YXRlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuaW1wb3J0IHsgQUJQIH0gZnJvbSAnLi4vLi4vbW9kZWxzJztcbmltcG9ydCB7IG9yZ2FuaXplUm91dGVzIH0gZnJvbSAnLi4vLi4vdXRpbHMvcm91dGUtdXRpbHMnO1xuaW1wb3J0IGNsb25lIGZyb20gJ2p1c3QtY2xvbmUnO1xuXG5leHBvcnQgY29uc3QgTkdYU19DT05GSUdfUExVR0lOX09QVElPTlMgPSBuZXcgSW5qZWN0aW9uVG9rZW4oJ05HWFNfQ09ORklHX1BMVUdJTl9PUFRJT05TJyk7XG5cbkBJbmplY3RhYmxlKClcbmV4cG9ydCBjbGFzcyBDb25maWdQbHVnaW4gaW1wbGVtZW50cyBOZ3hzUGx1Z2luIHtcbiAgcHJpdmF0ZSBpbml0aWFsaXplZDogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIGNvbnN0cnVjdG9yKEBJbmplY3QoTkdYU19DT05GSUdfUExVR0lOX09QVElPTlMpIHByaXZhdGUgb3B0aW9uczogQUJQLlJvb3QsIHByaXZhdGUgcm91dGVyOiBSb3V0ZXIpIHt9XG5cbiAgaGFuZGxlKHN0YXRlOiBhbnksIGV2ZW50OiBhbnksIG5leHQ6IE5neHNOZXh0UGx1Z2luRm4pIHtcbiAgICBjb25zdCBtYXRjaGVzID0gYWN0aW9uTWF0Y2hlcihldmVudCk7XG4gICAgY29uc3QgaXNJbml0QWN0aW9uID0gbWF0Y2hlcyhJbml0U3RhdGUpIHx8IG1hdGNoZXMoVXBkYXRlU3RhdGUpO1xuXG4gICAgLy8gY29uc3QgbGF5b3V0cyA9IHNucSgoKSA9PiB0aGlzLm9wdGlvbnMucmVxdWlyZW1lbnRzLmxheW91dHMuZmlsdGVyKGxheW91dCA9PiBsYXlvdXQgaW5zdGFuY2VvZiBUeXBlKSwgW10pO1xuICAgIGlmIChpc0luaXRBY3Rpb24gJiYgIXRoaXMuaW5pdGlhbGl6ZWQpIHtcbiAgICAgIGxldCB7IHJvdXRlcywgd3JhcHBlcnMgfSA9IHRyYW5zZm9ybVJvdXRlcyh0aGlzLnJvdXRlci5jb25maWcpO1xuICAgICAgcm91dGVzID0gb3JnYW5pemVSb3V0ZXMocm91dGVzLCB3cmFwcGVycyk7XG4gICAgICBjb25zdCBmbGF0dGVkUm91dGVzID0gZmxhdFJvdXRlcyhjbG9uZShyb3V0ZXMpKTtcbiAgICAgIHN0YXRlID0gc2V0VmFsdWUoc3RhdGUsICdDb25maWdTdGF0ZScsIHtcbiAgICAgICAgLi4uKHN0YXRlLkNvbmZpZ1N0YXRlICYmIHsgLi4uc3RhdGUuQ29uZmlnU3RhdGUgfSksXG4gICAgICAgIC4uLnRoaXMub3B0aW9ucyxcbiAgICAgICAgcm91dGVzLFxuICAgICAgICBmbGF0dGVkUm91dGVzLFxuICAgICAgfSk7XG5cbiAgICAgIHRoaXMuaW5pdGlhbGl6ZWQgPSB0cnVlO1xuICAgIH1cblxuICAgIHJldHVybiBuZXh0KHN0YXRlLCBldmVudCk7XG4gIH1cbn1cblxuZnVuY3Rpb24gdHJhbnNmb3JtUm91dGVzKHJvdXRlczogUm91dGVzID0gW10sIHdyYXBwZXJzOiBBQlAuRnVsbFJvdXRlW10gPSBbXSk6IGFueSB7XG4gIGNvbnN0IGFicFJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdID0gcm91dGVzXG4gICAgLmZpbHRlcihyb3V0ZSA9PiB7XG4gICAgICByZXR1cm4gc25xKCgpID0+IHJvdXRlLmRhdGEucm91dGVzLnJvdXRlcy5maW5kKHIgPT4gci5wYXRoID09PSByb3V0ZS5wYXRoKSwgZmFsc2UpO1xuICAgIH0pXG4gICAgLnJlZHVjZSgoYWNjLCB2YWwpID0+IFsuLi5hY2MsIC4uLnZhbC5kYXRhLnJvdXRlcy5yb3V0ZXNdLCBbXSk7XG5cbiAgd3JhcHBlcnMgPSBhYnBSb3V0ZXMuZmlsdGVyKGFyID0+IGFyLndyYXBwZXIpO1xuICBjb25zdCB0cmFuc2Zvcm1lZCA9IFtdIGFzIEFCUC5GdWxsUm91dGVbXTtcbiAgcm91dGVzXG4gICAgLmZpbHRlcihyb3V0ZSA9PiAocm91dGUuZGF0YSB8fCB7fSkucm91dGVzICYmIChyb3V0ZS5jb21wb25lbnQgfHwgcm91dGUubG9hZENoaWxkcmVuKSlcbiAgICAuZm9yRWFjaChyb3V0ZSA9PiB7XG4gICAgICBjb25zdCBhYnBQYWNrYWdlID0gYWJwUm91dGVzLmZpbmQoXG4gICAgICAgIGFicCA9PiBhYnAucGF0aC50b0xvd2VyQ2FzZSgpID09PSByb3V0ZS5wYXRoLnRvTG93ZXJDYXNlKCkgJiYgc25xKCgpID0+IHJvdXRlLmRhdGEucm91dGVzLnJvdXRlcy5sZW5ndGgsIGZhbHNlKSxcbiAgICAgICk7XG4gICAgICBjb25zdCB7IGxlbmd0aCB9ID0gdHJhbnNmb3JtZWQ7XG5cbiAgICAgIGlmIChhYnBQYWNrYWdlKSB7XG4gICAgICAgIHRyYW5zZm9ybWVkLnB1c2goYWJwUGFja2FnZSk7XG4gICAgICB9XG5cbiAgICAgIGlmICh0cmFuc2Zvcm1lZC5sZW5ndGggPT09IGxlbmd0aCkge1xuICAgICAgICB0cmFuc2Zvcm1lZC5wdXNoKHtcbiAgICAgICAgICAuLi5yb3V0ZS5kYXRhLnJvdXRlcyxcbiAgICAgICAgICBwYXRoOiByb3V0ZS5wYXRoLFxuICAgICAgICAgIG5hbWU6IHNucSgoKSA9PiByb3V0ZS5kYXRhLnJvdXRlcy5uYW1lLCByb3V0ZS5wYXRoKSxcbiAgICAgICAgICBjaGlsZHJlbjogcm91dGUuZGF0YS5yb3V0ZXMuY2hpbGRyZW4gfHwgW10sXG4gICAgICAgIH0gYXMgQUJQLkZ1bGxSb3V0ZSk7XG4gICAgICB9XG4gICAgfSk7XG5cbiAgcmV0dXJuIHsgcm91dGVzOiBzZXRVcmxzKHRyYW5zZm9ybWVkKSwgd3JhcHBlcnMgfTtcbn1cblxuZnVuY3Rpb24gc2V0VXJscyhyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSwgcGFyZW50VXJsPzogc3RyaW5nKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgaWYgKHBhcmVudFVybCkge1xuICAgIC8vIHRoaXMgaWYgYmxvY2sgdXNpbmcgZm9yIG9ubHkgcmVjdXJzaXZlIGNhbGxcblxuICAgIHJldHVybiByb3V0ZXMubWFwKHJvdXRlID0+ICh7XG4gICAgICAuLi5yb3V0ZSxcbiAgICAgIHVybDogYCR7cGFyZW50VXJsfS8ke3JvdXRlLnBhdGh9YCxcbiAgICAgIC4uLihyb3V0ZS5jaGlsZHJlbiAmJlxuICAgICAgICByb3V0ZS5jaGlsZHJlbi5sZW5ndGggJiYge1xuICAgICAgICAgIGNoaWxkcmVuOiBzZXRVcmxzKHJvdXRlLmNoaWxkcmVuLCBgJHtwYXJlbnRVcmx9LyR7cm91dGUucGF0aH1gKSxcbiAgICAgICAgfSksXG4gICAgfSkpO1xuICB9XG5cbiAgcmV0dXJuIHJvdXRlcy5tYXAocm91dGUgPT4gKHtcbiAgICAuLi5yb3V0ZSxcbiAgICB1cmw6IGAvJHtyb3V0ZS5wYXRofWAsXG4gICAgLi4uKHJvdXRlLmNoaWxkcmVuICYmXG4gICAgICByb3V0ZS5jaGlsZHJlbi5sZW5ndGggJiYge1xuICAgICAgICBjaGlsZHJlbjogc2V0VXJscyhyb3V0ZS5jaGlsZHJlbiwgYC8ke3JvdXRlLnBhdGh9YCksXG4gICAgICB9KSxcbiAgfSkpO1xufVxuXG5mdW5jdGlvbiBmbGF0Um91dGVzKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgY29uc3QgZmxhdCA9IChyOiBBQlAuRnVsbFJvdXRlW10pID0+IHtcbiAgICByZXR1cm4gci5yZWR1Y2UoKGFjYywgdmFsKSA9PiB7XG4gICAgICBsZXQgdmFsdWU6IEFCUC5GdWxsUm91dGVbXSA9IFt2YWxdO1xuICAgICAgaWYgKHZhbC5jaGlsZHJlbikge1xuICAgICAgICB2YWx1ZSA9IFt2YWwsIC4uLmZsYXQodmFsLmNoaWxkcmVuKV07XG4gICAgICB9XG5cbiAgICAgIHJldHVybiBbLi4uYWNjLCAuLi52YWx1ZV07XG4gICAgfSwgW10pO1xuICB9O1xuXG4gIHJldHVybiBmbGF0KHJvdXRlcyk7XG59XG4iXX0=