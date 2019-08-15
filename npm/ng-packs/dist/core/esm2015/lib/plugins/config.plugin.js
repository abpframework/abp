/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { Router } from '@angular/router';
import { actionMatcher, InitState, setValue, UpdateState } from '@ngxs/store';
import snq from 'snq';
import { organizeRoutes } from '../utils/route-utils';
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
        // const layouts = snq(() => this.options.requirements.layouts.filter(layout => layout instanceof Type), []);
        if (isInitAction && !this.initialized) {
            let { routes, wrappers } = transformRoutes(this.router.config);
            routes = organizeRoutes(routes, wrappers);
            state = setValue(state, 'ConfigState', Object.assign({}, (state.ConfigState && Object.assign({}, state.ConfigState)), this.options, { routes }));
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
    /** @type {?} */
    const abpRoutes = routes
        .filter((/**
     * @param {?} route
     * @return {?}
     */
    route => {
        return snq((/**
         * @return {?}
         */
        () => route.data.routes.find((/**
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
    (acc, val) => [...acc, ...val.data.routes]), []);
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
    route => (route.data || {}).routes && (route.component || route.loadChildren)))
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
        abp => abp.path.toLowerCase() === route.path.toLowerCase() && snq((/**
         * @return {?}
         */
        () => route.data.routes.length), false)));
        const { length } = transformed;
        if (abpPackage) {
            transformed.push(abpPackage);
        }
        if (transformed.length === length) {
            transformed.push((/** @type {?} */ ({
                path: route.path,
                name: snq((/**
                 * @return {?}
                 */
                () => route.data.routes.name), route.path),
                children: route.data.routes.children || [],
            })));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnBsdWdpbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9wbHVnaW5zL2NvbmZpZy5wbHVnaW4udHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsVUFBVSxFQUFFLGNBQWMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUNuRSxPQUFPLEVBQUUsTUFBTSxFQUFVLE1BQU0saUJBQWlCLENBQUM7QUFDakQsT0FBTyxFQUFFLGFBQWEsRUFBRSxTQUFTLEVBQWdDLFFBQVEsRUFBRSxXQUFXLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDNUcsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBRXRCLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQzs7QUFFdEQsTUFBTSxPQUFPLDBCQUEwQixHQUFHLElBQUksY0FBYyxDQUFDLDRCQUE0QixDQUFDO0FBRzFGLE1BQU0sT0FBTyxZQUFZOzs7OztJQUd2QixZQUF3RCxPQUFpQixFQUFVLE1BQWM7UUFBekMsWUFBTyxHQUFQLE9BQU8sQ0FBVTtRQUFVLFdBQU0sR0FBTixNQUFNLENBQVE7UUFGekYsZ0JBQVcsR0FBWSxLQUFLLENBQUM7SUFFK0QsQ0FBQzs7Ozs7OztJQUVyRyxNQUFNLENBQUMsS0FBVSxFQUFFLEtBQVUsRUFBRSxJQUFzQjs7Y0FDN0MsT0FBTyxHQUFHLGFBQWEsQ0FBQyxLQUFLLENBQUM7O2NBQzlCLFlBQVksR0FBRyxPQUFPLENBQUMsU0FBUyxDQUFDLElBQUksT0FBTyxDQUFDLFdBQVcsQ0FBQztRQUUvRCw2R0FBNkc7UUFDN0csSUFBSSxZQUFZLElBQUksQ0FBQyxJQUFJLENBQUMsV0FBVyxFQUFFO2dCQUNqQyxFQUFFLE1BQU0sRUFBRSxRQUFRLEVBQUUsR0FBRyxlQUFlLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxNQUFNLENBQUM7WUFDOUQsTUFBTSxHQUFHLGNBQWMsQ0FBQyxNQUFNLEVBQUUsUUFBUSxDQUFDLENBQUM7WUFFMUMsS0FBSyxHQUFHLFFBQVEsQ0FBQyxLQUFLLEVBQUUsYUFBYSxvQkFDaEMsQ0FBQyxLQUFLLENBQUMsV0FBVyxzQkFBUyxLQUFLLENBQUMsV0FBVyxDQUFFLENBQUMsRUFDL0MsSUFBSSxDQUFDLE9BQU8sSUFDZixNQUFNLElBQ04sQ0FBQztZQUVILElBQUksQ0FBQyxXQUFXLEdBQUcsSUFBSSxDQUFDO1NBQ3pCO1FBRUQsT0FBTyxJQUFJLENBQUMsS0FBSyxFQUFFLEtBQUssQ0FBQyxDQUFDO0lBQzVCLENBQUM7OztZQXpCRixVQUFVOzs7OzRDQUlJLE1BQU0sU0FBQywwQkFBMEI7WUFadkMsTUFBTTs7Ozs7OztJQVViLG1DQUFxQzs7Ozs7SUFFekIsK0JBQTZEOzs7OztJQUFFLDhCQUFzQjs7Ozs7OztBQXdCbkcsU0FBUyxlQUFlLENBQUMsU0FBaUIsRUFBRSxFQUFFLFdBQTRCLEVBQUU7O1VBQ3BFLFNBQVMsR0FBb0IsTUFBTTtTQUN0QyxNQUFNOzs7O0lBQUMsS0FBSyxDQUFDLEVBQUU7UUFDZCxPQUFPLEdBQUc7OztRQUFDLEdBQUcsRUFBRSxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLElBQUk7Ozs7UUFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxJQUFJLEtBQUssS0FBSyxDQUFDLElBQUksRUFBQyxHQUFFLEtBQUssQ0FBQyxDQUFDO0lBQzlFLENBQUMsRUFBQztTQUNELE1BQU07Ozs7O0lBQUMsQ0FBQyxHQUFHLEVBQUUsR0FBRyxFQUFFLEVBQUUsQ0FBQyxDQUFDLEdBQUcsR0FBRyxFQUFFLEdBQUcsR0FBRyxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsR0FBRSxFQUFFLENBQUM7SUFFekQsUUFBUSxHQUFHLFNBQVMsQ0FBQyxNQUFNOzs7O0lBQUMsRUFBRSxDQUFDLEVBQUUsQ0FBQyxFQUFFLENBQUMsT0FBTyxFQUFDLENBQUM7O1VBQ3hDLFdBQVcsR0FBRyxtQkFBQSxFQUFFLEVBQW1CO0lBQ3pDLE1BQU07U0FDSCxNQUFNOzs7O0lBQUMsS0FBSyxDQUFDLEVBQUUsQ0FBQyxDQUFDLEtBQUssQ0FBQyxJQUFJLElBQUksRUFBRSxDQUFDLENBQUMsTUFBTSxJQUFJLENBQUMsS0FBSyxDQUFDLFNBQVMsSUFBSSxLQUFLLENBQUMsWUFBWSxDQUFDLEVBQUM7U0FDckYsT0FBTzs7OztJQUFDLEtBQUssQ0FBQyxFQUFFOztjQUNULFVBQVUsR0FBRyxTQUFTLENBQUMsSUFBSTs7OztRQUMvQixHQUFHLENBQUMsRUFBRSxDQUFDLEdBQUcsQ0FBQyxJQUFJLENBQUMsV0FBVyxFQUFFLEtBQUssS0FBSyxDQUFDLElBQUksQ0FBQyxXQUFXLEVBQUUsSUFBSSxHQUFHOzs7UUFBQyxHQUFHLEVBQUUsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxNQUFNLEdBQUUsS0FBSyxDQUFDLEVBQ3pHO2NBQ0ssRUFBRSxNQUFNLEVBQUUsR0FBRyxXQUFXO1FBRTlCLElBQUksVUFBVSxFQUFFO1lBQ2QsV0FBVyxDQUFDLElBQUksQ0FBQyxVQUFVLENBQUMsQ0FBQztTQUM5QjtRQUVELElBQUksV0FBVyxDQUFDLE1BQU0sS0FBSyxNQUFNLEVBQUU7WUFDakMsV0FBVyxDQUFDLElBQUksQ0FBQyxtQkFBQTtnQkFDZixJQUFJLEVBQUUsS0FBSyxDQUFDLElBQUk7Z0JBQ2hCLElBQUksRUFBRSxHQUFHOzs7Z0JBQUMsR0FBRyxFQUFFLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxHQUFFLEtBQUssQ0FBQyxJQUFJLENBQUM7Z0JBQ25ELFFBQVEsRUFBRSxLQUFLLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxRQUFRLElBQUksRUFBRTthQUMzQyxFQUFpQixDQUFDLENBQUM7U0FDckI7SUFDSCxDQUFDLEVBQUMsQ0FBQztJQUVMLE9BQU8sRUFBRSxNQUFNLEVBQUUsT0FBTyxDQUFDLFdBQVcsQ0FBQyxFQUFFLFFBQVEsRUFBRSxDQUFDO0FBQ3BELENBQUM7Ozs7OztBQUVELFNBQVMsT0FBTyxDQUFDLE1BQXVCLEVBQUUsU0FBa0I7SUFDMUQsSUFBSSxTQUFTLEVBQUU7UUFDYiw4Q0FBOEM7UUFFOUMsT0FBTyxNQUFNLENBQUMsR0FBRzs7OztRQUFDLEtBQUssQ0FBQyxFQUFFLENBQUMsbUJBQ3RCLEtBQUssSUFDUixHQUFHLEVBQUUsR0FBRyxTQUFTLElBQUksS0FBSyxDQUFDLElBQUksRUFBRSxJQUM5QixDQUFDLEtBQUssQ0FBQyxRQUFRO1lBQ2hCLEtBQUssQ0FBQyxRQUFRLENBQUMsTUFBTSxJQUFJO1lBQ3ZCLFFBQVEsRUFBRSxPQUFPLENBQUMsS0FBSyxDQUFDLFFBQVEsRUFBRSxHQUFHLFNBQVMsSUFBSSxLQUFLLENBQUMsSUFBSSxFQUFFLENBQUM7U0FDaEUsQ0FBQyxFQUNKLEVBQUMsQ0FBQztLQUNMO0lBRUQsT0FBTyxNQUFNLENBQUMsR0FBRzs7OztJQUFDLEtBQUssQ0FBQyxFQUFFLENBQUMsbUJBQ3RCLEtBQUssSUFDUixHQUFHLEVBQUUsSUFBSSxLQUFLLENBQUMsSUFBSSxFQUFFLElBQ2xCLENBQUMsS0FBSyxDQUFDLFFBQVE7UUFDaEIsS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLElBQUk7UUFDdkIsUUFBUSxFQUFFLE9BQU8sQ0FBQyxLQUFLLENBQUMsUUFBUSxFQUFFLElBQUksS0FBSyxDQUFDLElBQUksRUFBRSxDQUFDO0tBQ3BELENBQUMsRUFDSixFQUFDLENBQUM7QUFDTixDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0LCBJbmplY3RhYmxlLCBJbmplY3Rpb25Ub2tlbiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgUm91dGVyLCBSb3V0ZXMgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgYWN0aW9uTWF0Y2hlciwgSW5pdFN0YXRlLCBOZ3hzTmV4dFBsdWdpbkZuLCBOZ3hzUGx1Z2luLCBzZXRWYWx1ZSwgVXBkYXRlU3RhdGUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQgeyBBQlAgfSBmcm9tICcuLi9tb2RlbHMnO1xuaW1wb3J0IHsgb3JnYW5pemVSb3V0ZXMgfSBmcm9tICcuLi91dGlscy9yb3V0ZS11dGlscyc7XG5cbmV4cG9ydCBjb25zdCBOR1hTX0NPTkZJR19QTFVHSU5fT1BUSU9OUyA9IG5ldyBJbmplY3Rpb25Ub2tlbignTkdYU19DT05GSUdfUExVR0lOX09QVElPTlMnKTtcblxuQEluamVjdGFibGUoKVxuZXhwb3J0IGNsYXNzIENvbmZpZ1BsdWdpbiBpbXBsZW1lbnRzIE5neHNQbHVnaW4ge1xuICBwcml2YXRlIGluaXRpYWxpemVkOiBib29sZWFuID0gZmFsc2U7XG5cbiAgY29uc3RydWN0b3IoQEluamVjdChOR1hTX0NPTkZJR19QTFVHSU5fT1BUSU9OUykgcHJpdmF0ZSBvcHRpb25zOiBBQlAuUm9vdCwgcHJpdmF0ZSByb3V0ZXI6IFJvdXRlcikge31cblxuICBoYW5kbGUoc3RhdGU6IGFueSwgZXZlbnQ6IGFueSwgbmV4dDogTmd4c05leHRQbHVnaW5Gbikge1xuICAgIGNvbnN0IG1hdGNoZXMgPSBhY3Rpb25NYXRjaGVyKGV2ZW50KTtcbiAgICBjb25zdCBpc0luaXRBY3Rpb24gPSBtYXRjaGVzKEluaXRTdGF0ZSkgfHwgbWF0Y2hlcyhVcGRhdGVTdGF0ZSk7XG5cbiAgICAvLyBjb25zdCBsYXlvdXRzID0gc25xKCgpID0+IHRoaXMub3B0aW9ucy5yZXF1aXJlbWVudHMubGF5b3V0cy5maWx0ZXIobGF5b3V0ID0+IGxheW91dCBpbnN0YW5jZW9mIFR5cGUpLCBbXSk7XG4gICAgaWYgKGlzSW5pdEFjdGlvbiAmJiAhdGhpcy5pbml0aWFsaXplZCkge1xuICAgICAgbGV0IHsgcm91dGVzLCB3cmFwcGVycyB9ID0gdHJhbnNmb3JtUm91dGVzKHRoaXMucm91dGVyLmNvbmZpZyk7XG4gICAgICByb3V0ZXMgPSBvcmdhbml6ZVJvdXRlcyhyb3V0ZXMsIHdyYXBwZXJzKTtcblxuICAgICAgc3RhdGUgPSBzZXRWYWx1ZShzdGF0ZSwgJ0NvbmZpZ1N0YXRlJywge1xuICAgICAgICAuLi4oc3RhdGUuQ29uZmlnU3RhdGUgJiYgeyAuLi5zdGF0ZS5Db25maWdTdGF0ZSB9KSxcbiAgICAgICAgLi4udGhpcy5vcHRpb25zLFxuICAgICAgICByb3V0ZXMsXG4gICAgICB9KTtcblxuICAgICAgdGhpcy5pbml0aWFsaXplZCA9IHRydWU7XG4gICAgfVxuXG4gICAgcmV0dXJuIG5leHQoc3RhdGUsIGV2ZW50KTtcbiAgfVxufVxuXG5mdW5jdGlvbiB0cmFuc2Zvcm1Sb3V0ZXMocm91dGVzOiBSb3V0ZXMgPSBbXSwgd3JhcHBlcnM6IEFCUC5GdWxsUm91dGVbXSA9IFtdKTogYW55IHtcbiAgY29uc3QgYWJwUm91dGVzOiBBQlAuRnVsbFJvdXRlW10gPSByb3V0ZXNcbiAgICAuZmlsdGVyKHJvdXRlID0+IHtcbiAgICAgIHJldHVybiBzbnEoKCkgPT4gcm91dGUuZGF0YS5yb3V0ZXMuZmluZChyID0+IHIucGF0aCA9PT0gcm91dGUucGF0aCksIGZhbHNlKTtcbiAgICB9KVxuICAgIC5yZWR1Y2UoKGFjYywgdmFsKSA9PiBbLi4uYWNjLCAuLi52YWwuZGF0YS5yb3V0ZXNdLCBbXSk7XG5cbiAgd3JhcHBlcnMgPSBhYnBSb3V0ZXMuZmlsdGVyKGFyID0+IGFyLndyYXBwZXIpO1xuICBjb25zdCB0cmFuc2Zvcm1lZCA9IFtdIGFzIEFCUC5GdWxsUm91dGVbXTtcbiAgcm91dGVzXG4gICAgLmZpbHRlcihyb3V0ZSA9PiAocm91dGUuZGF0YSB8fCB7fSkucm91dGVzICYmIChyb3V0ZS5jb21wb25lbnQgfHwgcm91dGUubG9hZENoaWxkcmVuKSlcbiAgICAuZm9yRWFjaChyb3V0ZSA9PiB7XG4gICAgICBjb25zdCBhYnBQYWNrYWdlID0gYWJwUm91dGVzLmZpbmQoXG4gICAgICAgIGFicCA9PiBhYnAucGF0aC50b0xvd2VyQ2FzZSgpID09PSByb3V0ZS5wYXRoLnRvTG93ZXJDYXNlKCkgJiYgc25xKCgpID0+IHJvdXRlLmRhdGEucm91dGVzLmxlbmd0aCwgZmFsc2UpLFxuICAgICAgKTtcbiAgICAgIGNvbnN0IHsgbGVuZ3RoIH0gPSB0cmFuc2Zvcm1lZDtcblxuICAgICAgaWYgKGFicFBhY2thZ2UpIHtcbiAgICAgICAgdHJhbnNmb3JtZWQucHVzaChhYnBQYWNrYWdlKTtcbiAgICAgIH1cblxuICAgICAgaWYgKHRyYW5zZm9ybWVkLmxlbmd0aCA9PT0gbGVuZ3RoKSB7XG4gICAgICAgIHRyYW5zZm9ybWVkLnB1c2goe1xuICAgICAgICAgIHBhdGg6IHJvdXRlLnBhdGgsXG4gICAgICAgICAgbmFtZTogc25xKCgpID0+IHJvdXRlLmRhdGEucm91dGVzLm5hbWUsIHJvdXRlLnBhdGgpLFxuICAgICAgICAgIGNoaWxkcmVuOiByb3V0ZS5kYXRhLnJvdXRlcy5jaGlsZHJlbiB8fCBbXSxcbiAgICAgICAgfSBhcyBBQlAuRnVsbFJvdXRlKTtcbiAgICAgIH1cbiAgICB9KTtcblxuICByZXR1cm4geyByb3V0ZXM6IHNldFVybHModHJhbnNmb3JtZWQpLCB3cmFwcGVycyB9O1xufVxuXG5mdW5jdGlvbiBzZXRVcmxzKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdLCBwYXJlbnRVcmw/OiBzdHJpbmcpOiBBQlAuRnVsbFJvdXRlW10ge1xuICBpZiAocGFyZW50VXJsKSB7XG4gICAgLy8gdGhpcyBpZiBibG9jayB1c2luZyBmb3Igb25seSByZWN1cnNpdmUgY2FsbFxuXG4gICAgcmV0dXJuIHJvdXRlcy5tYXAocm91dGUgPT4gKHtcbiAgICAgIC4uLnJvdXRlLFxuICAgICAgdXJsOiBgJHtwYXJlbnRVcmx9LyR7cm91dGUucGF0aH1gLFxuICAgICAgLi4uKHJvdXRlLmNoaWxkcmVuICYmXG4gICAgICAgIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCAmJiB7XG4gICAgICAgICAgY2hpbGRyZW46IHNldFVybHMocm91dGUuY2hpbGRyZW4sIGAke3BhcmVudFVybH0vJHtyb3V0ZS5wYXRofWApLFxuICAgICAgICB9KSxcbiAgICB9KSk7XG4gIH1cblxuICByZXR1cm4gcm91dGVzLm1hcChyb3V0ZSA9PiAoe1xuICAgIC4uLnJvdXRlLFxuICAgIHVybDogYC8ke3JvdXRlLnBhdGh9YCxcbiAgICAuLi4ocm91dGUuY2hpbGRyZW4gJiZcbiAgICAgIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCAmJiB7XG4gICAgICAgIGNoaWxkcmVuOiBzZXRVcmxzKHJvdXRlLmNoaWxkcmVuLCBgLyR7cm91dGUucGF0aH1gKSxcbiAgICAgIH0pLFxuICB9KSk7XG59XG4iXX0=