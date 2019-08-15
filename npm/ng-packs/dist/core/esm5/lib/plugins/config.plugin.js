/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { Router } from '@angular/router';
import { actionMatcher, InitState, setValue, UpdateState } from '@ngxs/store';
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
        function () { return route.data.routes.length; }), false); }));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnBsdWdpbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9wbHVnaW5zL2NvbmZpZy5wbHVnaW4udHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFVBQVUsRUFBRSxjQUFjLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDbkUsT0FBTyxFQUFFLE1BQU0sRUFBVSxNQUFNLGlCQUFpQixDQUFDO0FBQ2pELE9BQU8sRUFBRSxhQUFhLEVBQUUsU0FBUyxFQUFnQyxRQUFRLEVBQUUsV0FBVyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVHLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUV0QixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0JBQXNCLENBQUM7O0FBRXRELE1BQU0sS0FBTywwQkFBMEIsR0FBRyxJQUFJLGNBQWMsQ0FBQyw0QkFBNEIsQ0FBQztBQUUxRjtJQUlFLHNCQUF3RCxPQUFpQixFQUFVLE1BQWM7UUFBekMsWUFBTyxHQUFQLE9BQU8sQ0FBVTtRQUFVLFdBQU0sR0FBTixNQUFNLENBQVE7UUFGekYsZ0JBQVcsR0FBWSxLQUFLLENBQUM7SUFFK0QsQ0FBQzs7Ozs7OztJQUVyRyw2QkFBTTs7Ozs7O0lBQU4sVUFBTyxLQUFVLEVBQUUsS0FBVSxFQUFFLElBQXNCOztZQUM3QyxPQUFPLEdBQUcsYUFBYSxDQUFDLEtBQUssQ0FBQzs7WUFDOUIsWUFBWSxHQUFHLE9BQU8sQ0FBQyxTQUFTLENBQUMsSUFBSSxPQUFPLENBQUMsV0FBVyxDQUFDO1FBRS9ELDZHQUE2RztRQUM3RyxJQUFJLFlBQVksSUFBSSxDQUFDLElBQUksQ0FBQyxXQUFXLEVBQUU7WUFDakMsSUFBQSx3Q0FBMEQsRUFBeEQsa0JBQU0sRUFBRSxzQkFBZ0Q7WUFDOUQsTUFBTSxHQUFHLGNBQWMsQ0FBQyxNQUFNLEVBQUUsUUFBUSxDQUFDLENBQUM7WUFFMUMsS0FBSyxHQUFHLFFB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QU8sQ0FBQyxXQUFXLENBQUMsRUFBRSxRQUFRLFVBQUEsRUFBRSxDQUFDO0FBQ3BELENBQUM7Ozs7OztBQUVELFNBQVMsT0FBTyxDQUFDLE1BQXVCLEVBQUUsU0FBa0I7SUFDMUQsSUFBSSxTQUFTLEVBQUU7UUFDYiw4Q0FBOEM7UUFFOUMsT0FBTyxNQUFNLENBQUMsR0FBRzs7OztRQUFDLFVBQUEsS0FBSyxJQUFJLE9BQUEsc0JBQ3RCLEtBQUssSUFDUixHQUFHLEVBQUssU0FBUyxTQUFJLEtBQUssQ0FBQyxJQUFNLElBQzlCLENBQUMsS0FBSyxDQUFDLFFBQVE7WUFDaEIsS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLElBQUk7WUFDdkIsUUFBUSxFQUFFLE9BQU8sQ0FBQyxLQUFLLENBQUMsUUFBUSxFQUFLLFNBQVMsU0FBSSxLQUFLLENBQUMsSUFBTSxDQUFDO1NBQ2hFLENBQUMsRUFDSixFQVB5QixDQU96QixFQUFDLENBQUM7S0FDTDtJQUVELE9BQU8sTUFBTSxDQUFDLEdBQUc7Ozs7SUFBQyxVQUFBLEtBQUssSUFBSSxPQUFBLHNCQUN0QixLQUFLLElBQ1IsR0FBRyxFQUFFLE1BQUksS0FBSyxDQUFDLElBQU0sSUFDbEIsQ0FBQyxLQUFLLENBQUMsUUFBUTtRQUNoQixLQUFLLENBQUMsUUFBUSxDQUFDLE1BQU0sSUFBSTtRQUN2QixRQUFRLEVBQUUsT0FBTyxDQUFDLEtBQUssQ0FBQyxRQUFRLEVBQUUsTUFBSSxLQUFLLENBQUMsSUFBTSxDQUFDO0tBQ3BELENBQUMsRUFDSixFQVB5QixDQU96QixFQUFDLENBQUM7QUFDTixDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0LCBJbmplY3RhYmxlLCBJbmplY3Rpb25Ub2tlbiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgUm91dGVyLCBSb3V0ZXMgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgYWN0aW9uTWF0Y2hlciwgSW5pdFN0YXRlLCBOZ3hzTmV4dFBsdWdpbkZuLCBOZ3hzUGx1Z2luLCBzZXRWYWx1ZSwgVXBkYXRlU3RhdGUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQgeyBBQlAgfSBmcm9tICcuLi9tb2RlbHMnO1xuaW1wb3J0IHsgb3JnYW5pemVSb3V0ZXMgfSBmcm9tICcuLi91dGlscy9yb3V0ZS11dGlscyc7XG5cbmV4cG9ydCBjb25zdCBOR1hTX0NPTkZJR19QTFVHSU5fT1BUSU9OUyA9IG5ldyBJbmplY3Rpb25Ub2tlbignTkdYU19DT05GSUdfUExVR0lOX09QVElPTlMnKTtcblxuQEluamVjdGFibGUoKVxuZXhwb3J0IGNsYXNzIENvbmZpZ1BsdWdpbiBpbXBsZW1lbnRzIE5neHNQbHVnaW4ge1xuICBwcml2YXRlIGluaXRpYWxpemVkOiBib29sZWFuID0gZmFsc2U7XG5cbiAgY29uc3RydWN0b3IoQEluamVjdChOR1hTX0NPTkZJR19QTFVHSU5fT1BUSU9OUykgcHJpdmF0ZSBvcHRpb25zOiBBQlAuUm9vdCwgcHJpdmF0ZSByb3V0ZXI6IFJvdXRlcikge31cblxuICBoYW5kbGUoc3RhdGU6IGFueSwgZXZlbnQ6IGFueSwgbmV4dDogTmd4c05leHRQbHVnaW5Gbikge1xuICAgIGNvbnN0IG1hdGNoZXMgPSBhY3Rpb25NYXRjaGVyKGV2ZW50KTtcbiAgICBjb25zdCBpc0luaXRBY3Rpb24gPSBtYXRjaGVzKEluaXRTdGF0ZSkgfHwgbWF0Y2hlcyhVcGRhdGVTdGF0ZSk7XG5cbiAgICAvLyBjb25zdCBsYXlvdXRzID0gc25xKCgpID0+IHRoaXMub3B0aW9ucy5yZXF1aXJlbWVudHMubGF5b3V0cy5maWx0ZXIobGF5b3V0ID0+IGxheW91dCBpbnN0YW5jZW9mIFR5cGUpLCBbXSk7XG4gICAgaWYgKGlzSW5pdEFjdGlvbiAmJiAhdGhpcy5pbml0aWFsaXplZCkge1xuICAgICAgbGV0IHsgcm91dGVzLCB3cmFwcGVycyB9ID0gdHJhbnNmb3JtUm91dGVzKHRoaXMucm91dGVyLmNvbmZpZyk7XG4gICAgICByb3V0ZXMgPSBvcmdhbml6ZVJvdXRlcyhyb3V0ZXMsIHdyYXBwZXJzKTtcblxuICAgICAgc3RhdGUgPSBzZXRWYWx1ZShzdGF0ZSwgJ0NvbmZpZ1N0YXRlJywge1xuICAgICAgICAuLi4oc3RhdGUuQ29uZmlnU3RhdGUgJiYgeyAuLi5zdGF0ZS5Db25maWdTdGF0ZSB9KSxcbiAgICAgICAgLi4udGhpcy5vcHRpb25zLFxuICAgICAgICByb3V0ZXMsXG4gICAgICB9KTtcblxuICAgICAgdGhpcy5pbml0aWFsaXplZCA9IHRydWU7XG4gICAgfVxuXG4gICAgcmV0dXJuIG5leHQoc3RhdGUsIGV2ZW50KTtcbiAgfVxufVxuXG5mdW5jdGlvbiB0cmFuc2Zvcm1Sb3V0ZXMocm91dGVzOiBSb3V0ZXMgPSBbXSwgd3JhcHBlcnM6IEFCUC5GdWxsUm91dGVbXSA9IFtdKTogYW55IHtcbiAgY29uc3QgYWJwUm91dGVzOiBBQlAuRnVsbFJvdXRlW10gPSByb3V0ZXNcbiAgICAuZmlsdGVyKHJvdXRlID0+IHtcbiAgICAgIHJldHVybiBzbnEoKCkgPT4gcm91dGUuZGF0YS5yb3V0ZXMuZmluZChyID0+IHIucGF0aCA9PT0gcm91dGUucGF0aCksIGZhbHNlKTtcbiAgICB9KVxuICAgIC5yZWR1Y2UoKGFjYywgdmFsKSA9PiBbLi4uYWNjLCAuLi52YWwuZGF0YS5yb3V0ZXNdLCBbXSk7XG5cbiAgd3JhcHBlcnMgPSBhYnBSb3V0ZXMuZmlsdGVyKGFyID0+IGFyLndyYXBwZXIpO1xuICBjb25zdCB0cmFuc2Zvcm1lZCA9IFtdIGFzIEFCUC5GdWxsUm91dGVbXTtcbiAgcm91dGVzXG4gICAgLmZpbHRlcihyb3V0ZSA9PiAocm91dGUuZGF0YSB8fCB7fSkucm91dGVzICYmIChyb3V0ZS5jb21wb25lbnQgfHwgcm91dGUubG9hZENoaWxkcmVuKSlcbiAgICAuZm9yRWFjaChyb3V0ZSA9PiB7XG4gICAgICBjb25zdCBhYnBQYWNrYWdlID0gYWJwUm91dGVzLmZpbmQoXG4gICAgICAgIGFicCA9PiBhYnAucGF0aC50b0xvd2VyQ2FzZSgpID09PSByb3V0ZS5wYXRoLnRvTG93ZXJDYXNlKCkgJiYgc25xKCgpID0+IHJvdXRlLmRhdGEucm91dGVzLmxlbmd0aCwgZmFsc2UpLFxuICAgICAgKTtcbiAgICAgIGNvbnN0IHsgbGVuZ3RoIH0gPSB0cmFuc2Zvcm1lZDtcblxuICAgICAgaWYgKGFicFBhY2thZ2UpIHtcbiAgICAgICAgdHJhbnNmb3JtZWQucHVzaChhYnBQYWNrYWdlKTtcbiAgICAgIH1cblxuICAgICAgaWYgKHRyYW5zZm9ybWVkLmxlbmd0aCA9PT0gbGVuZ3RoKSB7XG4gICAgICAgIHRyYW5zZm9ybWVkLnB1c2goe1xuICAgICAgICAgIHBhdGg6IHJvdXRlLnBhdGgsXG4gICAgICAgICAgbmFtZTogc25xKCgpID0+IHJvdXRlLmRhdGEucm91dGVzLm5hbWUsIHJvdXRlLnBhdGgpLFxuICAgICAgICAgIGNoaWxkcmVuOiByb3V0ZS5kYXRhLnJvdXRlcy5jaGlsZHJlbiB8fCBbXSxcbiAgICAgICAgfSBhcyBBQlAuRnVsbFJvdXRlKTtcbiAgICAgIH1cbiAgICB9KTtcblxuICByZXR1cm4geyByb3V0ZXM6IHNldFVybHModHJhbnNmb3JtZWQpLCB3cmFwcGVycyB9O1xufVxuXG5mdW5jdGlvbiBzZXRVcmxzKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdLCBwYXJlbnRVcmw/OiBzdHJpbmcpOiBBQlAuRnVsbFJvdXRlW10ge1xuICBpZiAocGFyZW50VXJsKSB7XG4gICAgLy8gdGhpcyBpZiBibG9jayB1c2luZyBmb3Igb25seSByZWN1cnNpdmUgY2FsbFxuXG4gICAgcmV0dXJuIHJvdXRlcy5tYXAocm91dGUgPT4gKHtcbiAgICAgIC4uLnJvdXRlLFxuICAgICAgdXJsOiBgJHtwYXJlbnRVcmx9LyR7cm91dGUucGF0aH1gLFxuICAgICAgLi4uKHJvdXRlLmNoaWxkcmVuICYmXG4gICAgICAgIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCAmJiB7XG4gICAgICAgICAgY2hpbGRyZW46IHNldFVybHMocm91dGUuY2hpbGRyZW4sIGAke3BhcmVudFVybH0vJHtyb3V0ZS5wYXRofWApLFxuICAgICAgICB9KSxcbiAgICB9KSk7XG4gIH1cblxuICByZXR1cm4gcm91dGVzLm1hcChyb3V0ZSA9PiAoe1xuICAgIC4uLnJvdXRlLFxuICAgIHVybDogYC8ke3JvdXRlLnBhdGh9YCxcbiAgICAuLi4ocm91dGUuY2hpbGRyZW4gJiZcbiAgICAgIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCAmJiB7XG4gICAgICAgIGNoaWxkcmVuOiBzZXRVcmxzKHJvdXRlLmNoaWxkcmVuLCBgLyR7cm91dGUucGF0aH1gKSxcbiAgICAgIH0pLFxuICB9KSk7XG59XG4iXX0=