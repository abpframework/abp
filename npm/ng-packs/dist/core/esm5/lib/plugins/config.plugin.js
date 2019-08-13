/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Injectable, Inject, InjectionToken } from '@angular/core';
import { setValue, actionMatcher, InitState, UpdateState } from '@ngxs/store';
import { Router } from '@angular/router';
import snq from 'snq';
import { organizeRoutes } from '../utils/route-utils';
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
            state = setValue(state, 'ConfigState', tslib_1.__assign({}, (state.ConfigState && tslib_1.__assign({}, state.ConfigState)), this.options, { routes: routes }));
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
        function () { return route.data.routes.find((/**
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
    function (acc, val) { return tslib_1.__spread(acc, val.data.routes); }), []);
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
        function (abp) { return abp.path.toLowerCase() === route.path.toLowerCase(); }));
        var length = transformed.length;
        if (abpPackage) {
            transformed.push(abpPackage);
        }
        if (transformed.length === length) {
            transformed.push((/** @type {?} */ ({
                path: route.path,
                name: snq((/**
                 * @return {?}
                 */
                function () { return route.data.routes.name; }), route.path),
                children: route.data.routes.children || [],
            })));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnBsdWdpbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9wbHVnaW5zL2NvbmZpZy5wbHVnaW4udHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRSxjQUFjLEVBQVEsTUFBTSxlQUFlLENBQUM7QUFDekUsT0FBTyxFQUFjLFFBQVEsRUFBRSxhQUFhLEVBQUUsU0FBUyxFQUFFLFdBQVcsRUFBb0IsTUFBTSxhQUFhLENBQUM7QUFDNUcsT0FBTyxFQUFFLE1BQU0sRUFBVSxNQUFNLGlCQUFpQixDQUFDO0FBRWpELE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUN0QixPQUFPLEVBQTZCLGNBQWMsRUFBRSxNQUFNLHNCQUFzQixDQUFDOztBQUVqRixNQUFNLEtBQU8sMEJBQTBCLEdBQUcsSUFBSSxjQUFjLENBQUMsNEJBQTRCLENBQUM7QUFFMUY7SUFJRSxzQkFBd0QsT0FBaUIsRUFBVSxNQUFjO1FBQXpDLFlBQU8sR0FBUCxPQUFPLENBQVU7UUFBVSxXQUFNLEdBQU4sTUFBTSxDQUFRO1FBRnpGLGdCQUFXLEdBQVksS0FBSyxDQUFDO0lBRStELENBQUM7Ozs7Ozs7SUFFckcsNkJBQU07Ozs7OztJQUFOLFVBQU8sS0FBVSxFQUFFLEtBQVUsRUFBRSxJQUFzQjs7WUFDN0MsT0FBTyxHQUFHLGFBQWEsQ0FBQyxLQUFLLENBQUM7O1lBQzlCLFlBQVksR0FBRyxPQUFPLENBQUMsU0FBUyxDQUFDLElBQUksT0FBTyxDQUFDLFdBQVcsQ0FBQztRQUUvRCw2R0FBNkc7UUFDN0csSUFBSSxZQUFZLElBQUksQ0FBQyxJQUFJLENBQUMsV0FBVyxFQUFFO1lBQ2pDLElBQUEsd0NBQTBELEVBQXhELGtCQUFNLEVBQUUsc0JBQWdEO1lBQzlELE1BQU0sR0FBRyxjQUFjLENBQUMsTUFBTSxFQUFFLFFBQVEsQ0FBQyxDQUFDO1lBRTFDLEtBQUssR0FBRyxR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LEdBQUc7Ozs7UUFBQyxVQUFBLEtBQUssSUFBSSxPQUFBLHNCQUN0QixLQUFLLElBQ1IsR0FBRyxFQUFLLFNBQVMsU0FBSSxLQUFLLENBQUMsSUFBTSxJQUM5QixDQUFDLEtBQUssQ0FBQyxRQUFRO1lBQ2hCLEtBQUssQ0FBQyxRQUFRLENBQUMsTUFBTSxJQUFJO1lBQ3ZCLFFBQVEsRUFBRSxPQUFPLENBQUMsS0FBSyxDQUFDLFFBQVEsRUFBSyxTQUFTLFNBQUksS0FBSyxDQUFDLElBQU0sQ0FBQztTQUNoRSxDQUFDLEVBQ0osRUFQeUIsQ0FPekIsRUFBQyxDQUFDO0tBQ0w7SUFFRCxPQUFPLE1BQU0sQ0FBQyxHQUFHOzs7O0lBQUMsVUFBQSxLQUFLLElBQUksT0FBQSxzQkFDdEIsS0FBSyxJQUNSLEdBQUcsRUFBRSxNQUFJLEtBQUssQ0FBQyxJQUFNLElBQ2xCLENBQUMsS0FBSyxDQUFDLFFBQVE7UUFDaEIsS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLElBQUk7UUFDdkIsUUFBUSxFQUFFLE9BQU8sQ0FBQyxLQUFLLENBQUMsUUFBUSxFQUFFLE1BQUksS0FBSyxDQUFDLElBQU0sQ0FBQztLQUNwRCxDQUFDLEVBQ0osRUFQeUIsQ0FPekIsRUFBQyxDQUFDO0FBQ04sQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUsIEluamVjdCwgSW5qZWN0aW9uVG9rZW4sIFR5cGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5neHNQbHVnaW4sIHNldFZhbHVlLCBhY3Rpb25NYXRjaGVyLCBJbml0U3RhdGUsIFVwZGF0ZVN0YXRlLCBOZ3hzTmV4dFBsdWdpbkZuIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgUm91dGVyLCBSb3V0ZXMgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgQUJQIH0gZnJvbSAnLi4vbW9kZWxzJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcbmltcG9ydCB7IHNldENoaWxkUm91dGUsIHNvcnRSb3V0ZXMsIG9yZ2FuaXplUm91dGVzIH0gZnJvbSAnLi4vdXRpbHMvcm91dGUtdXRpbHMnO1xuXG5leHBvcnQgY29uc3QgTkdYU19DT05GSUdfUExVR0lOX09QVElPTlMgPSBuZXcgSW5qZWN0aW9uVG9rZW4oJ05HWFNfQ09ORklHX1BMVUdJTl9PUFRJT05TJyk7XG5cbkBJbmplY3RhYmxlKClcbmV4cG9ydCBjbGFzcyBDb25maWdQbHVnaW4gaW1wbGVtZW50cyBOZ3hzUGx1Z2luIHtcbiAgcHJpdmF0ZSBpbml0aWFsaXplZDogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIGNvbnN0cnVjdG9yKEBJbmplY3QoTkdYU19DT05GSUdfUExVR0lOX09QVElPTlMpIHByaXZhdGUgb3B0aW9uczogQUJQLlJvb3QsIHByaXZhdGUgcm91dGVyOiBSb3V0ZXIpIHt9XG5cbiAgaGFuZGxlKHN0YXRlOiBhbnksIGV2ZW50OiBhbnksIG5leHQ6IE5neHNOZXh0UGx1Z2luRm4pIHtcbiAgICBjb25zdCBtYXRjaGVzID0gYWN0aW9uTWF0Y2hlcihldmVudCk7XG4gICAgY29uc3QgaXNJbml0QWN0aW9uID0gbWF0Y2hlcyhJbml0U3RhdGUpIHx8IG1hdGNoZXMoVXBkYXRlU3RhdGUpO1xuXG4gICAgLy8gY29uc3QgbGF5b3V0cyA9IHNucSgoKSA9PiB0aGlzLm9wdGlvbnMucmVxdWlyZW1lbnRzLmxheW91dHMuZmlsdGVyKGxheW91dCA9PiBsYXlvdXQgaW5zdGFuY2VvZiBUeXBlKSwgW10pO1xuICAgIGlmIChpc0luaXRBY3Rpb24gJiYgIXRoaXMuaW5pdGlhbGl6ZWQpIHtcbiAgICAgIGxldCB7IHJvdXRlcywgd3JhcHBlcnMgfSA9IHRyYW5zZm9ybVJvdXRlcyh0aGlzLnJvdXRlci5jb25maWcpO1xuICAgICAgcm91dGVzID0gb3JnYW5pemVSb3V0ZXMocm91dGVzLCB3cmFwcGVycyk7XG5cbiAgICAgIHN0YXRlID0gc2V0VmFsdWUoc3RhdGUsICdDb25maWdTdGF0ZScsIHtcbiAgICAgICAgLi4uKHN0YXRlLkNvbmZpZ1N0YXRlICYmIHsgLi4uc3RhdGUuQ29uZmlnU3RhdGUgfSksXG4gICAgICAgIC4uLnRoaXMub3B0aW9ucyxcbiAgICAgICAgcm91dGVzLFxuICAgICAgfSk7XG5cbiAgICAgIHRoaXMuaW5pdGlhbGl6ZWQgPSB0cnVlO1xuICAgIH1cblxuICAgIHJldHVybiBuZXh0KHN0YXRlLCBldmVudCk7XG4gIH1cbn1cblxuZnVuY3Rpb24gdHJhbnNmb3JtUm91dGVzKHJvdXRlczogUm91dGVzID0gW10sIHdyYXBwZXJzOiBBQlAuRnVsbFJvdXRlW10gPSBbXSk6IGFueSB7XG4gIGNvbnN0IGFicFJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdID0gcm91dGVzXG4gICAgLmZpbHRlcihyb3V0ZSA9PiB7XG4gICAgICByZXR1cm4gc25xKCgpID0+IHJvdXRlLmRhdGEucm91dGVzLmZpbmQociA9PiByLnBhdGggPT09IHJvdXRlLnBhdGgpLCBmYWxzZSk7XG4gICAgfSlcbiAgICAucmVkdWNlKChhY2MsIHZhbCkgPT4gWy4uLmFjYywgLi4udmFsLmRhdGEucm91dGVzXSwgW10pO1xuXG4gIHdyYXBwZXJzID0gYWJwUm91dGVzLmZpbHRlcihhciA9PiBhci53cmFwcGVyKTtcbiAgY29uc3QgdHJhbnNmb3JtZWQgPSBbXSBhcyBBQlAuRnVsbFJvdXRlW107XG4gIHJvdXRlc1xuICAgIC5maWx0ZXIocm91dGUgPT4gcm91dGUuY29tcG9uZW50IHx8IHJvdXRlLmxvYWRDaGlsZHJlbilcbiAgICAuZm9yRWFjaChyb3V0ZSA9PiB7XG4gICAgICBjb25zdCBhYnBQYWNrYWdlID0gYWJwUm91dGVzLmZpbmQoYWJwID0+IGFicC5wYXRoLnRvTG93ZXJDYXNlKCkgPT09IHJvdXRlLnBhdGgudG9Mb3dlckNhc2UoKSk7XG4gICAgICBjb25zdCB7IGxlbmd0aCB9ID0gdHJhbnNmb3JtZWQ7XG5cbiAgICAgIGlmIChhYnBQYWNrYWdlKSB7XG4gICAgICAgIHRyYW5zZm9ybWVkLnB1c2goYWJwUGFja2FnZSk7XG4gICAgICB9XG5cbiAgICAgIGlmICh0cmFuc2Zvcm1lZC5sZW5ndGggPT09IGxlbmd0aCkge1xuICAgICAgICB0cmFuc2Zvcm1lZC5wdXNoKHtcbiAgICAgICAgICBwYXRoOiByb3V0ZS5wYXRoLFxuICAgICAgICAgIG5hbWU6IHNucSgoKSA9PiByb3V0ZS5kYXRhLnJvdXRlcy5uYW1lLCByb3V0ZS5wYXRoKSxcbiAgICAgICAgICBjaGlsZHJlbjogcm91dGUuZGF0YS5yb3V0ZXMuY2hpbGRyZW4gfHwgW10sXG4gICAgICAgIH0gYXMgQUJQLkZ1bGxSb3V0ZSk7XG4gICAgICB9XG4gICAgfSk7XG5cbiAgcmV0dXJuIHsgcm91dGVzOiBzZXRVcmxzKHRyYW5zZm9ybWVkKSwgd3JhcHBlcnMgfTtcbn1cblxuZnVuY3Rpb24gc2V0VXJscyhyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSwgcGFyZW50VXJsPzogc3RyaW5nKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgaWYgKHBhcmVudFVybCkge1xuICAgIC8vIHRoaXMgaWYgYmxvY2sgdXNpbmcgZm9yIG9ubHkgcmVjdXJzaXZlIGNhbGxcblxuICAgIHJldHVybiByb3V0ZXMubWFwKHJvdXRlID0+ICh7XG4gICAgICAuLi5yb3V0ZSxcbiAgICAgIHVybDogYCR7cGFyZW50VXJsfS8ke3JvdXRlLnBhdGh9YCxcbiAgICAgIC4uLihyb3V0ZS5jaGlsZHJlbiAmJlxuICAgICAgICByb3V0ZS5jaGlsZHJlbi5sZW5ndGggJiYge1xuICAgICAgICAgIGNoaWxkcmVuOiBzZXRVcmxzKHJvdXRlLmNoaWxkcmVuLCBgJHtwYXJlbnRVcmx9LyR7cm91dGUucGF0aH1gKSxcbiAgICAgICAgfSksXG4gICAgfSkpO1xuICB9XG5cbiAgcmV0dXJuIHJvdXRlcy5tYXAocm91dGUgPT4gKHtcbiAgICAuLi5yb3V0ZSxcbiAgICB1cmw6IGAvJHtyb3V0ZS5wYXRofWAsXG4gICAgLi4uKHJvdXRlLmNoaWxkcmVuICYmXG4gICAgICByb3V0ZS5jaGlsZHJlbi5sZW5ndGggJiYge1xuICAgICAgICBjaGlsZHJlbjogc2V0VXJscyhyb3V0ZS5jaGlsZHJlbiwgYC8ke3JvdXRlLnBhdGh9YCksXG4gICAgICB9KSxcbiAgfSkpO1xufVxuIl19