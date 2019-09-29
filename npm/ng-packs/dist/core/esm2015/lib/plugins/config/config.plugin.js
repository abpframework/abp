/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { Router } from '@angular/router';
import { actionMatcher, InitState, setValue, UpdateState } from '@ngxs/store';
import snq from 'snq';
import { organizeRoutes } from '../../utils/route-utils';
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
        // const layouts = snq(() => this.options.requirements.layouts.filter(layout => layout instanceof Type), []);
        if (isInitAction && !this.initialized) {
            let { routes, wrappers } = transformRoutes(this.router.config);
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
        () => route.data.routes.routes.length), false)));
        const { length } = transformed;
        if (abpPackage) {
            transformed.push(abpPackage);
        }
        if (transformed.length === length) {
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnBsdWdpbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9wbHVnaW5zL2NvbmZpZy9jb25maWcucGx1Z2luLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFVBQVUsRUFBRSxjQUFjLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDbkUsT0FBTyxFQUFFLE1BQU0sRUFBVSxNQUFNLGlCQUFpQixDQUFDO0FBQ2pELE9BQU8sRUFBRSxhQUFhLEVBQUUsU0FBUyxFQUFnQyxRQUFRLEVBQUUsV0FBVyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVHLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUV0QixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0seUJBQXlCLENBQUM7QUFDekQsT0FBTyxLQUFLLE1BQU0sWUFBWSxDQUFDOztBQUUvQixNQUFNLE9BQU8sMEJBQTBCLEdBQUcsSUFBSSxjQUFjLENBQUMsNEJBQTRCLENBQUM7QUFHMUYsTUFBTSxPQUFPLFlBQVk7Ozs7O0lBR3ZCLFlBQXdELE9BQWlCLEVBQVUsTUFBYztRQUF6QyxZQUFPLEdBQVAsT0FBTyxDQUFVO1FBQVUsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUZ6RixnQkFBVyxHQUFZLEtBQUssQ0FBQztJQUUrRCxDQUFDOzs7Ozs7O0lBRXJHLE1BQU0sQ0FBQyxLQUFVLEVBQUUsS0FBVSxFQUFFLElBQXNCOztjQUM3QyxPQUFPLEdBQUcsYUFBYSxDQUFDLEtBQUssQ0FBQzs7Y0FDOUIsWUFBWSxHQUFHLE9BQU8sQ0FBQyxTQUFTLENBQUMsSUFBSSxPQUFPLENBQUMsV0FBVyxDQUFDO1FBRS9ELDZHQUE2RztRQUM3RyxJQUFJLFlBQVksSUFBSSxDQUFDLElBQUksQ0FBQyxXQUFXLEVBQUU7Z0JBQ2pDLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxHQUFHLGVBQWUsQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFD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QUcsRUFBRSxFQUFFOztnQkFDdkIsS0FBSyxHQUFvQixDQUFDLEdBQUcsQ0FBQztZQUNsQyxJQUFJLEdBQUcsQ0FBQyxRQUFRLEVBQUU7Z0JBQ2hCLEtBQUssR0FBRyxDQUFDLEdBQUcsRUFBRSxHQUFHLElBQUksQ0FBQyxHQUFHLENBQUMsUUFBUSxDQUFDLENBQUMsQ0FBQzthQUN0QztZQUVELE9BQU8sQ0FBQyxHQUFHLEdBQUcsRUFBRSxHQUFHLEtBQUssQ0FBQyxDQUFDO1FBQzVCLENBQUMsR0FBRSxFQUFFLENBQUMsQ0FBQztJQUNULENBQUMsQ0FBQTtJQUVELE9BQU8sSUFBSSxDQUFDLE1BQU0sQ0FBQyxDQUFDO0FBQ3RCLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3QsIEluamVjdGFibGUsIEluamVjdGlvblRva2VuIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBSb3V0ZXIsIFJvdXRlcyB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBhY3Rpb25NYXRjaGVyLCBJbml0U3RhdGUsIE5neHNOZXh0UGx1Z2luRm4sIE5neHNQbHVnaW4sIHNldFZhbHVlLCBVcGRhdGVTdGF0ZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcbmltcG9ydCB7IEFCUCB9IGZyb20gJy4uLy4uL21vZGVscyc7XG5pbXBvcnQgeyBvcmdhbml6ZVJvdXRlcyB9IGZyb20gJy4uLy4uL3V0aWxzL3JvdXRlLXV0aWxzJztcbmltcG9ydCBjbG9uZSBmcm9tICdqdXN0LWNsb25lJztcblxuZXhwb3J0IGNvbnN0IE5HWFNfQ09ORklHX1BMVUdJTl9PUFRJT05TID0gbmV3IEluamVjdGlvblRva2VuKCdOR1hTX0NPTkZJR19QTFVHSU5fT1BUSU9OUycpO1xuXG5ASW5qZWN0YWJsZSgpXG5leHBvcnQgY2xhc3MgQ29uZmlnUGx1Z2luIGltcGxlbWVudHMgTmd4c1BsdWdpbiB7XG4gIHByaXZhdGUgaW5pdGlhbGl6ZWQ6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBjb25zdHJ1Y3RvcihASW5qZWN0KE5HWFNfQ09ORklHX1BMVUdJTl9PUFRJT05TKSBwcml2YXRlIG9wdGlvbnM6IEFCUC5Sb290LCBwcml2YXRlIHJvdXRlcjogUm91dGVyKSB7fVxuXG4gIGhhbmRsZShzdGF0ZTogYW55LCBldmVudDogYW55LCBuZXh0OiBOZ3hzTmV4dFBsdWdpbkZuKSB7XG4gICAgY29uc3QgbWF0Y2hlcyA9IGFjdGlvbk1hdGNoZXIoZXZlbnQpO1xuICAgIGNvbnN0IGlzSW5pdEFjdGlvbiA9IG1hdGNoZXMoSW5pdFN0YXRlKSB8fCBtYXRjaGVzKFVwZGF0ZVN0YXRlKTtcblxuICAgIC8vIGNvbnN0IGxheW91dHMgPSBzbnEoKCkgPT4gdGhpcy5vcHRpb25zLnJlcXVpcmVtZW50cy5sYXlvdXRzLmZpbHRlcihsYXlvdXQgPT4gbGF5b3V0IGluc3RhbmNlb2YgVHlwZSksIFtdKTtcbiAgICBpZiAoaXNJbml0QWN0aW9uICYmICF0aGlzLmluaXRpYWxpemVkKSB7XG4gICAgICBsZXQgeyByb3V0ZXMsIHdyYXBwZXJzIH0gPSB0cmFuc2Zvcm1Sb3V0ZXModGhpcy5yb3V0ZXIuY29uZmlnKTtcbiAgICAgIHJvdXRlcyA9IG9yZ2FuaXplUm91dGVzKHJvdXRlcywgd3JhcHBlcnMpO1xuICAgICAgY29uc3QgZmxhdHRlZFJvdXRlcyA9IGZsYXRSb3V0ZXMoY2xvbmUocm91dGVzKSk7XG4gICAgICBzdGF0ZSA9IHNldFZhbHVlKHN0YXRlLCAnQ29uZmlnU3RhdGUnLCB7XG4gICAgICAgIC4uLihzdGF0ZS5Db25maWdTdGF0ZSAmJiB7IC4uLnN0YXRlLkNvbmZpZ1N0YXRlIH0pLFxuICAgICAgICAuLi50aGlzLm9wdGlvbnMsXG4gICAgICAgIHJvdXRlcyxcbiAgICAgICAgZmxhdHRlZFJvdXRlcyxcbiAgICAgIH0pO1xuXG4gICAgICB0aGlzLmluaXRpYWxpemVkID0gdHJ1ZTtcbiAgICB9XG5cbiAgICByZXR1cm4gbmV4dChzdGF0ZSwgZXZlbnQpO1xuICB9XG59XG5cbmZ1bmN0aW9uIHRyYW5zZm9ybVJvdXRlcyhyb3V0ZXM6IFJvdXRlcyA9IFtdLCB3cmFwcGVyczogQUJQLkZ1bGxSb3V0ZVtdID0gW10pOiBhbnkge1xuICBjb25zdCBhYnBSb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSA9IHJvdXRlc1xuICAgIC5maWx0ZXIocm91dGUgPT4ge1xuICAgICAgcmV0dXJuIHNucSgoKSA9PiByb3V0ZS5kYXRhLnJvdXRlcy5yb3V0ZXMuZmluZChyID0+IHIucGF0aCA9PT0gcm91dGUucGF0aCksIGZhbHNlKTtcbiAgICB9KVxuICAgIC5yZWR1Y2UoKGFjYywgdmFsKSA9PiBbLi4uYWNjLCAuLi52YWwuZGF0YS5yb3V0ZXMucm91dGVzXSwgW10pO1xuXG4gIHdyYXBwZXJzID0gYWJwUm91dGVzLmZpbHRlcihhciA9PiBhci53cmFwcGVyKTtcbiAgY29uc3QgdHJhbnNmb3JtZWQgPSBbXSBhcyBBQlAuRnVsbFJvdXRlW107XG4gIHJvdXRlc1xuICAgIC5maWx0ZXIocm91dGUgPT4gKHJvdXRlLmRhdGEgfHwge30pLnJvdXRlcyAmJiAocm91dGUuY29tcG9uZW50IHx8IHJvdXRlLmxvYWRDaGlsZHJlbikpXG4gICAgLmZvckVhY2gocm91dGUgPT4ge1xuICAgICAgY29uc3QgYWJwUGFja2FnZSA9IGFicFJvdXRlcy5maW5kKFxuICAgICAgICBhYnAgPT4gYWJwLnBhdGgudG9Mb3dlckNhc2UoKSA9PT0gcm91dGUucGF0aC50b0xvd2VyQ2FzZSgpICYmIHNucSgoKSA9PiByb3V0ZS5kYXRhLnJvdXRlcy5yb3V0ZXMubGVuZ3RoLCBmYWxzZSksXG4gICAgICApO1xuICAgICAgY29uc3QgeyBsZW5ndGggfSA9IHRyYW5zZm9ybWVkO1xuXG4gICAgICBpZiAoYWJwUGFja2FnZSkge1xuICAgICAgICB0cmFuc2Zvcm1lZC5wdXNoKGFicFBhY2thZ2UpO1xuICAgICAgfVxuXG4gICAgICBpZiAodHJhbnNmb3JtZWQubGVuZ3RoID09PSBsZW5ndGgpIHtcbiAgICAgICAgdHJhbnNmb3JtZWQucHVzaCh7XG4gICAgICAgICAgLi4ucm91dGUuZGF0YS5yb3V0ZXMsXG4gICAgICAgICAgcGF0aDogcm91dGUucGF0aCxcbiAgICAgICAgICBuYW1lOiBzbnEoKCkgPT4gcm91dGUuZGF0YS5yb3V0ZXMubmFtZSwgcm91dGUucGF0aCksXG4gICAgICAgICAgY2hpbGRyZW46IHJvdXRlLmRhdGEucm91dGVzLmNoaWxkcmVuIHx8IFtdLFxuICAgICAgICB9IGFzIEFCUC5GdWxsUm91dGUpO1xuICAgICAgfVxuICAgIH0pO1xuXG4gIHJldHVybiB7IHJvdXRlczogc2V0VXJscyh0cmFuc2Zvcm1lZCksIHdyYXBwZXJzIH07XG59XG5cbmZ1bmN0aW9uIHNldFVybHMocm91dGVzOiBBQlAuRnVsbFJvdXRlW10sIHBhcmVudFVybD86IHN0cmluZyk6IEFCUC5GdWxsUm91dGVbXSB7XG4gIGlmIChwYXJlbnRVcmwpIHtcbiAgICAvLyB0aGlzIGlmIGJsb2NrIHVzaW5nIGZvciBvbmx5IHJlY3Vyc2l2ZSBjYWxsXG5cbiAgICByZXR1cm4gcm91dGVzLm1hcChyb3V0ZSA9PiAoe1xuICAgICAgLi4ucm91dGUsXG4gICAgICB1cmw6IGAke3BhcmVudFVybH0vJHtyb3V0ZS5wYXRofWAsXG4gICAgICAuLi4ocm91dGUuY2hpbGRyZW4gJiZcbiAgICAgICAgcm91dGUuY2hpbGRyZW4ubGVuZ3RoICYmIHtcbiAgICAgICAgICBjaGlsZHJlbjogc2V0VXJscyhyb3V0ZS5jaGlsZHJlbiwgYCR7cGFyZW50VXJsfS8ke3JvdXRlLnBhdGh9YCksXG4gICAgICAgIH0pLFxuICAgIH0pKTtcbiAgfVxuXG4gIHJldHVybiByb3V0ZXMubWFwKHJvdXRlID0+ICh7XG4gICAgLi4ucm91dGUsXG4gICAgdXJsOiBgLyR7cm91dGUucGF0aH1gLFxuICAgIC4uLihyb3V0ZS5jaGlsZHJlbiAmJlxuICAgICAgcm91dGUuY2hpbGRyZW4ubGVuZ3RoICYmIHtcbiAgICAgICAgY2hpbGRyZW46IHNldFVybHMocm91dGUuY2hpbGRyZW4sIGAvJHtyb3V0ZS5wYXRofWApLFxuICAgICAgfSksXG4gIH0pKTtcbn1cblxuZnVuY3Rpb24gZmxhdFJvdXRlcyhyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSk6IEFCUC5GdWxsUm91dGVbXSB7XG4gIGNvbnN0IGZsYXQgPSAocjogQUJQLkZ1bGxSb3V0ZVtdKSA9PiB7XG4gICAgcmV0dXJuIHIucmVkdWNlKChhY2MsIHZhbCkgPT4ge1xuICAgICAgbGV0IHZhbHVlOiBBQlAuRnVsbFJvdXRlW10gPSBbdmFsXTtcbiAgICAgIGlmICh2YWwuY2hpbGRyZW4pIHtcbiAgICAgICAgdmFsdWUgPSBbdmFsLCAuLi5mbGF0KHZhbC5jaGlsZHJlbildO1xuICAgICAgfVxuXG4gICAgICByZXR1cm4gWy4uLmFjYywgLi4udmFsdWVdO1xuICAgIH0sIFtdKTtcbiAgfTtcblxuICByZXR1cm4gZmxhdChyb3V0ZXMpO1xufVxuIl19