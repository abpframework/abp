/**
 * @fileoverview added by tsickle
 * Generated from: lib/plugins/config.plugin.ts
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnBsdWdpbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9wbHVnaW5zL2NvbmZpZy5wbHVnaW4udHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLE1BQU0sRUFBRSxVQUFVLEVBQUUsY0FBYyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ25FLE9BQU8sRUFBRSxNQUFNLEVBQVUsTUFBTSxpQkFBaUIsQ0FBQztBQUNqRCxPQUFPLEVBQUUsYUFBYSxFQUFFLFNBQVMsRUFBZ0MsUUFBUSxFQUFFLFdBQVcsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1RyxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFFdEIsT0FBTyxFQUFFLGNBQWMsRUFBRSxZQUFZLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUNwRSxPQUFPLEtBQUssTUFBTSxZQUFZLENBQUM7O0FBRS9CLE1BQU0sS0FBTywwQkFBMEIsR0FBRyxJQUFJLGNBQWMsQ0FBQyw0QkFBNEIsQ0FBQztBQUUxRjtJQUlFLHNCQUF3RCxPQUFpQixFQUFVLE1BQWM7UUFBekMsWUFBTyxHQUFQLE9BQU8sQ0FBVTtRQUFVLFdBQU0sR0FBTixNQUFNLENBQVE7UUFGekYsZ0JBQVcsR0FBRyxLQUFLLENBQUM7SUFFd0UsQ0FBQzs7Ozs7OztJQUVyRyw2QkFBTTs7Ozs7O0lBQU4sVUFBTyxLQUFVLEVBQUUsS0FBVSxFQUFFLElBQXNCOztZQUM3QyxPQUFPLEdBQUcsYUFBYSxDQUFDLEtBQUssQ0FBQzs7WUFDOUIsWUFBWSxHQUFHLE9BQU8sQ0FBQyxTQUFTLENBQUMsSUFBSSxPQUFPLENBQUMsV0FBVyxDQUFDO1FBRS9ELElBQUksWUFBWSxJQUFJLENBQUMsSUFBSSxDQUFDLFdBQVcsRUFBRTs7Z0JBQy9CLGlCQUFpQixHQUFHLGVBQWUsQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU0sQ0FBQztZQUN2RCxJQUFBLGlDQUFNO1lBQ0osSUFBQSxxQ0FBUTtZQUVoQixNQUFNLEdBQUcsY0FBYyxDQUFDLE1BQU0sRUFBRSxRQUFRLENBQUMsQ0FBQzs7Z0JBQ3BDLGFBQWEsR0FBRyxVQUFVLENBQUMsS0FBSyxDQUFDLE1BQU0sQ0FBQyxDQUFDO1lBQy9DLEtBQUssR0FBRyxRQUFRLENBQUMsS0FBSyxFQUFFLGFBQWEsdUJBQ2hDLENBQUMsS0FBSyxDQUFDLFdBQVcseUJBQVMsS0FBSyxDQUFDLFdBQVcsQ0FBRSxDQUFDLEVBQy9DLElBQUksQ0FBQyxPQUFPLElBQ2YsTUFBTSxRQUFBO2dCQUNOLGFBQWEsZUFBQSxJQUNiLENBQUM7WUFFSCxJQUFJLENBQUMsV0FBVyxHQUFHLElBQUksQ0FBQztTQUN6QjtRQUVELE9BQU8sSUFBSSxDQUFDLEtBQUssRUFBRSxLQUFLLENBQUMsQ0FBQztJQUM1QixDQUFDOztnQkE1QkYsVUFBVTs7OztnREFJSSxNQUFNLFNBQUMsMEJBQTBCO2dCQWJ2QyxNQUFNOztJQXNDZixtQkFBQztDQUFBLEFBN0JELElBNkJDO1NBNUJZLFlBQVk7Ozs7OztJQUN2QixtQ0FBNEI7Ozs7O0lBRWhCLCtCQUE2RDs7Ozs7SUFBRSw4QkFBc0I7Ozs7Ozs7QUEyQm5HLFNBQVMsZUFBZSxDQUFDLE1BQW1CLEVBQUUsUUFBOEI7SUFBbkQsdUJBQUEsRUFBQSxXQUFtQjtJQUFFLHlCQUFBLEVBQUEsYUFBOEI7OztRQUVwRSxZQUFZLEdBQW9CLE1BQU07U0FDekMsTUFBTTs7OztJQUFDLFVBQUEsS0FBSztRQUNYLE9BQU8sR0FBRzs7O1FBQUMsY0FBTSxPQUFBLEtBQUssQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU0sQ0FBQyxJQUFJOzs7O1FBQUMsVUFBQSxDQUFDLElBQUksT0FBQSxDQUFDLENBQUMsSUFBSSxLQUFLLEtBQUssQ0FBQyxJQUFJLEVBQXJCLENBQXFCLEVBQUMsRUFBekQsQ0FBeUQsR0FBRSxLQUFLLENBQUMsQ0FBQztJQUNyRixDQUFDLEVBQUM7U0FDRCxNQUFNOzs7OztJQUFDLFVBQUMsR0FBRyxFQUFFLEdBQUcsSUFBSyx3QkFBSSxHQUFHLEVBQUssR0FBRyxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsTUFBTSxHQUFsQyxDQUFtQyxHQUFFLEVBQUUsQ0FBQzs7O1FBRTFELFNBQVMsb0JBQU8sWUFBWSxFQUFFLEVBQUssWUFBWSxDQUFDO0lBRXRELFFBQVEsR0FBRyxTQUFTLENBQUMsTUFBTTs7OztJQUFDLFVBQUEsRUFBRSxJQUFJLE9BQUEsRUFBRSxDQUFDLE9BQU8sRUFBVixDQUFVLEVBQUMsQ0FBQzs7UUFDeEMsV0FBVyxHQUFHLG1CQUFBLEVBQUUsRUFBbUI7SUFDekMsTUFBTTtTQUNILE1BQU07Ozs7SUFBQyxVQUFBLEtBQUssSUFBSSxPQUFBLEtBQUssQ0FBQyxTQUFTLElBQUksS0FBSyxDQUFDLFlBQVksRUFBckMsQ0FBcUMsRUFBQztTQUN0RCxPQUFPOzs7O0lBQUMsVUFBQSxLQUFLOztZQUNOLFVBQVUsR0FBRyxTQUFTLENBQUMsSUFBSTs7OztRQUFDLFVBQUEsR0FBRyxJQUFJLE9BQUEsR0FBRyxDQUFDLElBQUksQ0FBQyxXQUFXLEVBQUUsS0FBSyxLQUFLLENBQUMsSUFBSSxDQUFDLFdBQVcsRUFBRSxJQUFJLENBQUMsR0FBRyxDQUFDLE9BQU8sRUFBbkUsQ0FBbUUsRUFBQztRQUVyRyxJQUFBLDJCQUFNO1FBRWQsSUFBSSxVQUFVLEVBQUU7WUFDZCxXQUFXLENBQUMsSUFBSSxDQUFDLFVBQVUsQ0FBQyxDQUFDO1NBQzlCO1FBRUQsSUFBSSxXQUFXLENBQUMsTUFBTSxLQUFLLE1BQU0sSUFBSSxDQUFDLEtBQUssQ0FBQyxJQUFJLElBQUksRUFBRSxDQUFDLENBQUMsTUFBTSxFQUFFO1lBQzlELFdBQVcsQ0FBQyxJQUFJLENBQUMsd0NBQ1osS0FBSyxDQUFDLElBQUksQ0FBQyxNQUFNLElBQ3BCLElBQUksRUFBRSxLQUFLLENBQUMsSUFBSSxFQUNoQixJQUFJLEVBQUUsR0FBRzs7O2dCQUFDLGNBQU0sT0FBQSxLQUFLLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLEVBQXRCLENBQXNCLEdBQUUsS0FBSyxDQUFDLElBQUksQ0FBQyxFQUNuRCxRQUFRLEVBQUUsS0FBSyxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsUUFBUSxJQUFJLEVBQUUsS0FDMUIsQ0FBQyxDQUFDO1NBQ3JCO0lBQ0gsQ0FBQyxFQUFDLENBQUM7SUFFTCxPQUFPLEVBQUUsTUFBTSxFQUFFLE9BQU8sQ0FBQyxXQUFXLENBQUMsRUFBRSxRQUFRLFVBQUEsRUFBRSxDQUFDO0FBQ3BELENBQUM7Ozs7OztBQUVELFNBQVMsT0FBTyxDQUFDLE1BQXVCLEVBQUUsU0FBa0I7SUFDMUQsSUFBSSxTQUFTLEVBQUU7UUFDYiw4Q0FBOEM7UUFFOUMsT0FBTyxNQUFNLENBQUMsR0FBRzs7OztRQUFDLFVBQUEsS0FBSyxJQUFJLE9BQUEsc0JBQ3RCLEtBQUssSUFDUixHQUFHLEVBQUssU0FBUyxTQUFJLEtBQUssQ0FBQyxJQUFNLElBQzlCLENBQUMsS0FBSyxDQUFDLFFBQVE7WUFDaEIsS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLElBQUk7WUFDdkIsUUFBUSxFQUFFLE9BQU8sQ0FBQyxLQUFLLENBQUMsUUFBUSxFQUFLLFNBQVMsU0FBSSxLQUFLLENBQUMsSUFBTSxDQUFDO1NBQ2hFLENBQUMsRUFDSixFQVB5QixDQU96QixFQUFDLENBQUM7S0FDTDtJQUVELE9BQU8sTUFBTSxDQUFDLEdBQUc7Ozs7SUFBQyxVQUFBLEtBQUssSUFBSSxPQUFBLHNCQUN0QixLQUFLLElBQ1IsR0FBRyxFQUFFLE1BQUksS0FBSyxDQUFDLElBQU0sSUFDbEIsQ0FBQyxLQUFLLENBQUMsUUFBUTtRQUNoQixLQUFLLENBQUMsUUFBUSxDQUFDLE1BQU0sSUFBSTtRQUN2QixRQUFRLEVBQUUsT0FBTyxDQUFDLEtBQUssQ0FBQyxRQUFRLEVBQUUsTUFBSSxLQUFLLENBQUMsSUFBTSxDQUFDO0tBQ3BELENBQUMsRUFDSixFQVB5QixDQU96QixFQUFDLENBQUM7QUFDTixDQUFDOzs7OztBQUVELFNBQVMsVUFBVSxDQUFDLE1BQXVCOztRQUNuQyxJQUFJOzs7O0lBQUcsVUFBQyxDQUFrQjtRQUM5QixPQUFPLENBQUMsQ0FBQyxNQUFNOzs7OztRQUFDLFVBQUMsR0FBRyxFQUFFLEdBQUc7O2dCQUNuQixLQUFLLEdBQW9CLENBQUMsR0FBRyxDQUFDO1lBQ2xDLElBQUksR0FBRyxDQUFDLFFBQVEsRUFBRTtnQkFDaEIsS0FBSyxxQkFBSSxHQUFHLEdBQUssSUFBSSxDQUFDLEdBQUcsQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDO2FBQ3RDO1lBRUQsd0JBQVcsR0FBRyxFQUFLLEtBQUssRUFBRTtRQUM1QixDQUFDLEdBQUUsRUFBRSxDQUFDLENBQUM7SUFDVCxDQUFDLENBQUE7SUFFRCxPQUFPLElBQUksQ0FBQyxNQUFNLENBQUMsQ0FBQztBQUN0QixDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0LCBJbmplY3RhYmxlLCBJbmplY3Rpb25Ub2tlbiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBSb3V0ZXIsIFJvdXRlcyB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XHJcbmltcG9ydCB7IGFjdGlvbk1hdGNoZXIsIEluaXRTdGF0ZSwgTmd4c05leHRQbHVnaW5GbiwgTmd4c1BsdWdpbiwgc2V0VmFsdWUsIFVwZGF0ZVN0YXRlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XHJcbmltcG9ydCB7IEFCUCB9IGZyb20gJy4uL21vZGVscyc7XHJcbmltcG9ydCB7IG9yZ2FuaXplUm91dGVzLCBnZXRBYnBSb3V0ZXMgfSBmcm9tICcuLi91dGlscy9yb3V0ZS11dGlscyc7XHJcbmltcG9ydCBjbG9uZSBmcm9tICdqdXN0LWNsb25lJztcclxuXHJcbmV4cG9ydCBjb25zdCBOR1hTX0NPTkZJR19QTFVHSU5fT1BUSU9OUyA9IG5ldyBJbmplY3Rpb25Ub2tlbignTkdYU19DT05GSUdfUExVR0lOX09QVElPTlMnKTtcclxuXHJcbkBJbmplY3RhYmxlKClcclxuZXhwb3J0IGNsYXNzIENvbmZpZ1BsdWdpbiBpbXBsZW1lbnRzIE5neHNQbHVnaW4ge1xyXG4gIHByaXZhdGUgaW5pdGlhbGl6ZWQgPSBmYWxzZTtcclxuXHJcbiAgY29uc3RydWN0b3IoQEluamVjdChOR1hTX0NPTkZJR19QTFVHSU5fT1BUSU9OUykgcHJpdmF0ZSBvcHRpb25zOiBBQlAuUm9vdCwgcHJpdmF0ZSByb3V0ZXI6IFJvdXRlcikge31cclxuXHJcbiAgaGFuZGxlKHN0YXRlOiBhbnksIGV2ZW50OiBhbnksIG5leHQ6IE5neHNOZXh0UGx1Z2luRm4pIHtcclxuICAgIGNvbnN0IG1hdGNoZXMgPSBhY3Rpb25NYXRjaGVyKGV2ZW50KTtcclxuICAgIGNvbnN0IGlzSW5pdEFjdGlvbiA9IG1hdGNoZXMoSW5pdFN0YXRlKSB8fCBtYXRjaGVzKFVwZGF0ZVN0YXRlKTtcclxuXHJcbiAgICBpZiAoaXNJbml0QWN0aW9uICYmICF0aGlzLmluaXRpYWxpemVkKSB7XHJcbiAgICAgIGNvbnN0IHRyYW5zZm9ybWVkUm91dGVzID0gdHJhbnNmb3JtUm91dGVzKHRoaXMucm91dGVyLmNvbmZpZyk7XHJcbiAgICAgIGxldCB7IHJvdXRlcyB9ID0gdHJhbnNmb3JtZWRSb3V0ZXM7XHJcbiAgICAgIGNvbnN0IHsgd3JhcHBlcnMgfSA9IHRyYW5zZm9ybWVkUm91dGVzO1xyXG5cclxuICAgICAgcm91dGVzID0gb3JnYW5pemVSb3V0ZXMocm91dGVzLCB3cmFwcGVycyk7XHJcbiAgICAgIGNvbnN0IGZsYXR0ZWRSb3V0ZXMgPSBmbGF0Um91dGVzKGNsb25lKHJvdXRlcykpO1xyXG4gICAgICBzdGF0ZSA9IHNldFZhbHVlKHN0YXRlLCAnQ29uZmlnU3RhdGUnLCB7XHJcbiAgICAgICAgLi4uKHN0YXRlLkNvbmZpZ1N0YXRlICYmIHsgLi4uc3RhdGUuQ29uZmlnU3RhdGUgfSksXHJcbiAgICAgICAgLi4udGhpcy5vcHRpb25zLFxyXG4gICAgICAgIHJvdXRlcyxcclxuICAgICAgICBmbGF0dGVkUm91dGVzLFxyXG4gICAgICB9KTtcclxuXHJcbiAgICAgIHRoaXMuaW5pdGlhbGl6ZWQgPSB0cnVlO1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiBuZXh0KHN0YXRlLCBldmVudCk7XHJcbiAgfVxyXG59XHJcblxyXG5mdW5jdGlvbiB0cmFuc2Zvcm1Sb3V0ZXMocm91dGVzOiBSb3V0ZXMgPSBbXSwgd3JhcHBlcnM6IEFCUC5GdWxsUm91dGVbXSA9IFtdKTogYW55IHtcclxuICAvLyBUT0RPOiByZW1vdmUgaW4gdjFcclxuICBjb25zdCBvbGRBYnBSb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSA9IHJvdXRlc1xyXG4gICAgLmZpbHRlcihyb3V0ZSA9PiB7XHJcbiAgICAgIHJldHVybiBzbnEoKCkgPT4gcm91dGUuZGF0YS5yb3V0ZXMucm91dGVzLmZpbmQociA9PiByLnBhdGggPT09IHJvdXRlLnBhdGgpLCBmYWxzZSk7XHJcbiAgICB9KVxyXG4gICAgLnJlZHVjZSgoYWNjLCB2YWwpID0+IFsuLi5hY2MsIC4uLnZhbC5kYXRhLnJvdXRlcy5yb3V0ZXNdLCBbXSk7XHJcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBkZXByZWNhdGlvblxyXG4gIGNvbnN0IGFicFJvdXRlcyA9IFsuLi5nZXRBYnBSb3V0ZXMoKSwgLi4ub2xkQWJwUm91dGVzXTtcclxuXHJcbiAgd3JhcHBlcnMgPSBhYnBSb3V0ZXMuZmlsdGVyKGFyID0+IGFyLndyYXBwZXIpO1xyXG4gIGNvbnN0IHRyYW5zZm9ybWVkID0gW10gYXMgQUJQLkZ1bGxSb3V0ZVtdO1xyXG4gIHJvdXRlc1xyXG4gICAgLmZpbHRlcihyb3V0ZSA9PiByb3V0ZS5jb21wb25lbnQgfHwgcm91dGUubG9hZENoaWxkcmVuKVxyXG4gICAgLmZvckVhY2gocm91dGUgPT4ge1xyXG4gICAgICBjb25zdCBhYnBQYWNrYWdlID0gYWJwUm91dGVzLmZpbmQoYWJwID0+IGFicC5wYXRoLnRvTG93ZXJDYXNlKCkgPT09IHJvdXRlLnBhdGgudG9Mb3dlckNhc2UoKSAmJiAhYWJwLndyYXBwZXIpO1xyXG5cclxuICAgICAgY29uc3QgeyBsZW5ndGggfSA9IHRyYW5zZm9ybWVkO1xyXG5cclxuICAgICAgaWYgKGFicFBhY2thZ2UpIHtcclxuICAgICAgICB0cmFuc2Zvcm1lZC5wdXNoKGFicFBhY2thZ2UpO1xyXG4gICAgICB9XHJcblxyXG4gICAgICBpZiAodHJhbnNmb3JtZWQubGVuZ3RoID09PSBsZW5ndGggJiYgKHJvdXRlLmRhdGEgfHwge30pLnJvdXRlcykge1xyXG4gICAgICAgIHRyYW5zZm9ybWVkLnB1c2goe1xyXG4gICAgICAgICAgLi4ucm91dGUuZGF0YS5yb3V0ZXMsXHJcbiAgICAgICAgICBwYXRoOiByb3V0ZS5wYXRoLFxyXG4gICAgICAgICAgbmFtZTogc25xKCgpID0+IHJvdXRlLmRhdGEucm91dGVzLm5hbWUsIHJvdXRlLnBhdGgpLFxyXG4gICAgICAgICAgY2hpbGRyZW46IHJvdXRlLmRhdGEucm91dGVzLmNoaWxkcmVuIHx8IFtdLFxyXG4gICAgICAgIH0gYXMgQUJQLkZ1bGxSb3V0ZSk7XHJcbiAgICAgIH1cclxuICAgIH0pO1xyXG5cclxuICByZXR1cm4geyByb3V0ZXM6IHNldFVybHModHJhbnNmb3JtZWQpLCB3cmFwcGVycyB9O1xyXG59XHJcblxyXG5mdW5jdGlvbiBzZXRVcmxzKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdLCBwYXJlbnRVcmw/OiBzdHJpbmcpOiBBQlAuRnVsbFJvdXRlW10ge1xyXG4gIGlmIChwYXJlbnRVcmwpIHtcclxuICAgIC8vIHRoaXMgaWYgYmxvY2sgdXNpbmcgZm9yIG9ubHkgcmVjdXJzaXZlIGNhbGxcclxuXHJcbiAgICByZXR1cm4gcm91dGVzLm1hcChyb3V0ZSA9PiAoe1xyXG4gICAgICAuLi5yb3V0ZSxcclxuICAgICAgdXJsOiBgJHtwYXJlbnRVcmx9LyR7cm91dGUucGF0aH1gLFxyXG4gICAgICAuLi4ocm91dGUuY2hpbGRyZW4gJiZcclxuICAgICAgICByb3V0ZS5jaGlsZHJlbi5sZW5ndGggJiYge1xyXG4gICAgICAgICAgY2hpbGRyZW46IHNldFVybHMocm91dGUuY2hpbGRyZW4sIGAke3BhcmVudFVybH0vJHtyb3V0ZS5wYXRofWApLFxyXG4gICAgICAgIH0pLFxyXG4gICAgfSkpO1xyXG4gIH1cclxuXHJcbiAgcmV0dXJuIHJvdXRlcy5tYXAocm91dGUgPT4gKHtcclxuICAgIC4uLnJvdXRlLFxyXG4gICAgdXJsOiBgLyR7cm91dGUucGF0aH1gLFxyXG4gICAgLi4uKHJvdXRlLmNoaWxkcmVuICYmXHJcbiAgICAgIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCAmJiB7XHJcbiAgICAgICAgY2hpbGRyZW46IHNldFVybHMocm91dGUuY2hpbGRyZW4sIGAvJHtyb3V0ZS5wYXRofWApLFxyXG4gICAgICB9KSxcclxuICB9KSk7XHJcbn1cclxuXHJcbmZ1bmN0aW9uIGZsYXRSb3V0ZXMocm91dGVzOiBBQlAuRnVsbFJvdXRlW10pOiBBQlAuRnVsbFJvdXRlW10ge1xyXG4gIGNvbnN0IGZsYXQgPSAocjogQUJQLkZ1bGxSb3V0ZVtdKSA9PiB7XHJcbiAgICByZXR1cm4gci5yZWR1Y2UoKGFjYywgdmFsKSA9PiB7XHJcbiAgICAgIGxldCB2YWx1ZTogQUJQLkZ1bGxSb3V0ZVtdID0gW3ZhbF07XHJcbiAgICAgIGlmICh2YWwuY2hpbGRyZW4pIHtcclxuICAgICAgICB2YWx1ZSA9IFt2YWwsIC4uLmZsYXQodmFsLmNoaWxkcmVuKV07XHJcbiAgICAgIH1cclxuXHJcbiAgICAgIHJldHVybiBbLi4uYWNjLCAuLi52YWx1ZV07XHJcbiAgICB9LCBbXSk7XHJcbiAgfTtcclxuXHJcbiAgcmV0dXJuIGZsYXQocm91dGVzKTtcclxufVxyXG4iXX0=