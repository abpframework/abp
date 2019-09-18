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
                const { children } = val;
                delete val.children;
                value = [val, ...flat(children)];
            }
            return [...acc, ...value];
        }), []);
    });
    return flat(routes);
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnBsdWdpbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9wbHVnaW5zL2NvbmZpZy9jb25maWcucGx1Z2luLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFVBQVUsRUFBRSxjQUFjLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDbkUsT0FBTyxFQUFFLE1BQU0sRUFBVSxNQUFNLGlCQUFpQixDQUFDO0FBQ2pELE9BQU8sRUFBRSxhQUFhLEVBQUUsU0FBUyxFQUFnQyxRQUFRLEVBQUUsV0FBVyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVHLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUV0QixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0seUJBQXlCLENBQUM7QUFDekQsT0FBTyxLQUFLLE1BQU0sWUFBWSxDQUFDOztBQUUvQixNQUFNLE9BQU8sMEJBQTBCLEdBQUcsSUFBSSxjQUFjLENBQUMsNEJBQTRCLENBQUM7QUFHMUYsTUFBTSxPQUFPLFlBQVk7Ozs7O0lBR3ZCLFlBQXdELE9BQWlCLEVBQVUsTUFBYztRQUF6QyxZQUFPLEdBQVAsT0FBTyxDQUFVO1FBQVUsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUZ6RixnQkFBVyxHQUFZLEtBQUssQ0FBQztJQUUrRCxDQUFDOzs7Ozs7O0lBRXJHLE1BQU0sQ0FBQyxLQUFVLEVBQUUsS0FBVSxFQUFFLElBQXNCOztjQUM3QyxPQUFPLEdBQUcsYUFBYSxDQUFDLEtBQUssQ0FBQzs7Y0FDOUIsWUFBWSxHQUFHLE9BQU8sQ0FBQyxTQUFTLENBQUMsSUFBSSxPQUFPLENBQUMsV0FBVyxDQUFDO1FBRS9ELDZHQUE2RztRQUM3RyxJQUFJLFlBQVksSUFBSSxDQUFDLElBQUksQ0FBQyxXQUFXLEVBQUU7Z0JBQ2pDLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxHQUFHLGVBQWUsQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFD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QUcsRUFBRSxFQUFFOztnQkFDdkIsS0FBSyxHQUFvQixDQUFDLEdBQUcsQ0FBQztZQUNsQyxJQUFJLEdBQUcsQ0FBQyxRQUFRLEVBQUU7c0JBQ1YsRUFBRSxRQUFRLEVBQUUsR0FBRyxHQUFHO2dCQUN4QixPQUFPLEdBQUcsQ0FBQyxRQUFRLENBQUM7Z0JBQ3BCLEtBQUssR0FBRyxDQUFDLEdBQUcsRUFBRSxHQUFHLElBQUksQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDO2FBQ2xDO1lBRUQsT0FBTyxDQUFDLEdBQUcsR0FBRyxFQUFFLEdBQUcsS0FBSyxDQUFDLENBQUM7UUFDNUIsQ0FBQyxHQUFFLEVBQUUsQ0FBQyxDQUFDO0lBQ1QsQ0FBQyxDQUFBO0lBRUQsT0FBTyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUM7QUFDdEIsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdCwgSW5qZWN0YWJsZSwgSW5qZWN0aW9uVG9rZW4gfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJvdXRlciwgUm91dGVzIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IGFjdGlvbk1hdGNoZXIsIEluaXRTdGF0ZSwgTmd4c05leHRQbHVnaW5GbiwgTmd4c1BsdWdpbiwgc2V0VmFsdWUsIFVwZGF0ZVN0YXRlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuaW1wb3J0IHsgQUJQIH0gZnJvbSAnLi4vLi4vbW9kZWxzJztcbmltcG9ydCB7IG9yZ2FuaXplUm91dGVzIH0gZnJvbSAnLi4vLi4vdXRpbHMvcm91dGUtdXRpbHMnO1xuaW1wb3J0IGNsb25lIGZyb20gJ2p1c3QtY2xvbmUnO1xuXG5leHBvcnQgY29uc3QgTkdYU19DT05GSUdfUExVR0lOX09QVElPTlMgPSBuZXcgSW5qZWN0aW9uVG9rZW4oJ05HWFNfQ09ORklHX1BMVUdJTl9PUFRJT05TJyk7XG5cbkBJbmplY3RhYmxlKClcbmV4cG9ydCBjbGFzcyBDb25maWdQbHVnaW4gaW1wbGVtZW50cyBOZ3hzUGx1Z2luIHtcbiAgcHJpdmF0ZSBpbml0aWFsaXplZDogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIGNvbnN0cnVjdG9yKEBJbmplY3QoTkdYU19DT05GSUdfUExVR0lOX09QVElPTlMpIHByaXZhdGUgb3B0aW9uczogQUJQLlJvb3QsIHByaXZhdGUgcm91dGVyOiBSb3V0ZXIpIHt9XG5cbiAgaGFuZGxlKHN0YXRlOiBhbnksIGV2ZW50OiBhbnksIG5leHQ6IE5neHNOZXh0UGx1Z2luRm4pIHtcbiAgICBjb25zdCBtYXRjaGVzID0gYWN0aW9uTWF0Y2hlcihldmVudCk7XG4gICAgY29uc3QgaXNJbml0QWN0aW9uID0gbWF0Y2hlcyhJbml0U3RhdGUpIHx8IG1hdGNoZXMoVXBkYXRlU3RhdGUpO1xuXG4gICAgLy8gY29uc3QgbGF5b3V0cyA9IHNucSgoKSA9PiB0aGlzLm9wdGlvbnMucmVxdWlyZW1lbnRzLmxheW91dHMuZmlsdGVyKGxheW91dCA9PiBsYXlvdXQgaW5zdGFuY2VvZiBUeXBlKSwgW10pO1xuICAgIGlmIChpc0luaXRBY3Rpb24gJiYgIXRoaXMuaW5pdGlhbGl6ZWQpIHtcbiAgICAgIGxldCB7IHJvdXRlcywgd3JhcHBlcnMgfSA9IHRyYW5zZm9ybVJvdXRlcyh0aGlzLnJvdXRlci5jb25maWcpO1xuICAgICAgcm91dGVzID0gb3JnYW5pemVSb3V0ZXMocm91dGVzLCB3cmFwcGVycyk7XG4gICAgICBjb25zdCBmbGF0dGVkUm91dGVzID0gZmxhdFJvdXRlcyhjbG9uZShyb3V0ZXMpKTtcbiAgICAgIHN0YXRlID0gc2V0VmFsdWUoc3RhdGUsICdDb25maWdTdGF0ZScsIHtcbiAgICAgICAgLi4uKHN0YXRlLkNvbmZpZ1N0YXRlICYmIHsgLi4uc3RhdGUuQ29uZmlnU3RhdGUgfSksXG4gICAgICAgIC4uLnRoaXMub3B0aW9ucyxcbiAgICAgICAgcm91dGVzLFxuICAgICAgICBmbGF0dGVkUm91dGVzLFxuICAgICAgfSk7XG5cbiAgICAgIHRoaXMuaW5pdGlhbGl6ZWQgPSB0cnVlO1xuICAgIH1cblxuICAgIHJldHVybiBuZXh0KHN0YXRlLCBldmVudCk7XG4gIH1cbn1cblxuZnVuY3Rpb24gdHJhbnNmb3JtUm91dGVzKHJvdXRlczogUm91dGVzID0gW10sIHdyYXBwZXJzOiBBQlAuRnVsbFJvdXRlW10gPSBbXSk6IGFueSB7XG4gIGNvbnN0IGFicFJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdID0gcm91dGVzXG4gICAgLmZpbHRlcihyb3V0ZSA9PiB7XG4gICAgICByZXR1cm4gc25xKCgpID0+IHJvdXRlLmRhdGEucm91dGVzLnJvdXRlcy5maW5kKHIgPT4gci5wYXRoID09PSByb3V0ZS5wYXRoKSwgZmFsc2UpO1xuICAgIH0pXG4gICAgLnJlZHVjZSgoYWNjLCB2YWwpID0+IFsuLi5hY2MsIC4uLnZhbC5kYXRhLnJvdXRlcy5yb3V0ZXNdLCBbXSk7XG5cbiAgd3JhcHBlcnMgPSBhYnBSb3V0ZXMuZmlsdGVyKGFyID0+IGFyLndyYXBwZXIpO1xuICBjb25zdCB0cmFuc2Zvcm1lZCA9IFtdIGFzIEFCUC5GdWxsUm91dGVbXTtcbiAgcm91dGVzXG4gICAgLmZpbHRlcihyb3V0ZSA9PiAocm91dGUuZGF0YSB8fCB7fSkucm91dGVzICYmIChyb3V0ZS5jb21wb25lbnQgfHwgcm91dGUubG9hZENoaWxkcmVuKSlcbiAgICAuZm9yRWFjaChyb3V0ZSA9PiB7XG4gICAgICBjb25zdCBhYnBQYWNrYWdlID0gYWJwUm91dGVzLmZpbmQoXG4gICAgICAgIGFicCA9PiBhYnAucGF0aC50b0xvd2VyQ2FzZSgpID09PSByb3V0ZS5wYXRoLnRvTG93ZXJDYXNlKCkgJiYgc25xKCgpID0+IHJvdXRlLmRhdGEucm91dGVzLnJvdXRlcy5sZW5ndGgsIGZhbHNlKSxcbiAgICAgICk7XG4gICAgICBjb25zdCB7IGxlbmd0aCB9ID0gdHJhbnNmb3JtZWQ7XG5cbiAgICAgIGlmIChhYnBQYWNrYWdlKSB7XG4gICAgICAgIHRyYW5zZm9ybWVkLnB1c2goYWJwUGFja2FnZSk7XG4gICAgICB9XG5cbiAgICAgIGlmICh0cmFuc2Zvcm1lZC5sZW5ndGggPT09IGxlbmd0aCkge1xuICAgICAgICB0cmFuc2Zvcm1lZC5wdXNoKHtcbiAgICAgICAgICAuLi5yb3V0ZS5kYXRhLnJvdXRlcyxcbiAgICAgICAgICBwYXRoOiByb3V0ZS5wYXRoLFxuICAgICAgICAgIG5hbWU6IHNucSgoKSA9PiByb3V0ZS5kYXRhLnJvdXRlcy5uYW1lLCByb3V0ZS5wYXRoKSxcbiAgICAgICAgICBjaGlsZHJlbjogcm91dGUuZGF0YS5yb3V0ZXMuY2hpbGRyZW4gfHwgW10sXG4gICAgICAgIH0gYXMgQUJQLkZ1bGxSb3V0ZSk7XG4gICAgICB9XG4gICAgfSk7XG5cbiAgcmV0dXJuIHsgcm91dGVzOiBzZXRVcmxzKHRyYW5zZm9ybWVkKSwgd3JhcHBlcnMgfTtcbn1cblxuZnVuY3Rpb24gc2V0VXJscyhyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSwgcGFyZW50VXJsPzogc3RyaW5nKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgaWYgKHBhcmVudFVybCkge1xuICAgIC8vIHRoaXMgaWYgYmxvY2sgdXNpbmcgZm9yIG9ubHkgcmVjdXJzaXZlIGNhbGxcblxuICAgIHJldHVybiByb3V0ZXMubWFwKHJvdXRlID0+ICh7XG4gICAgICAuLi5yb3V0ZSxcbiAgICAgIHVybDogYCR7cGFyZW50VXJsfS8ke3JvdXRlLnBhdGh9YCxcbiAgICAgIC4uLihyb3V0ZS5jaGlsZHJlbiAmJlxuICAgICAgICByb3V0ZS5jaGlsZHJlbi5sZW5ndGggJiYge1xuICAgICAgICAgIGNoaWxkcmVuOiBzZXRVcmxzKHJvdXRlLmNoaWxkcmVuLCBgJHtwYXJlbnRVcmx9LyR7cm91dGUucGF0aH1gKSxcbiAgICAgICAgfSksXG4gICAgfSkpO1xuICB9XG5cbiAgcmV0dXJuIHJvdXRlcy5tYXAocm91dGUgPT4gKHtcbiAgICAuLi5yb3V0ZSxcbiAgICB1cmw6IGAvJHtyb3V0ZS5wYXRofWAsXG4gICAgLi4uKHJvdXRlLmNoaWxkcmVuICYmXG4gICAgICByb3V0ZS5jaGlsZHJlbi5sZW5ndGggJiYge1xuICAgICAgICBjaGlsZHJlbjogc2V0VXJscyhyb3V0ZS5jaGlsZHJlbiwgYC8ke3JvdXRlLnBhdGh9YCksXG4gICAgICB9KSxcbiAgfSkpO1xufVxuXG5mdW5jdGlvbiBmbGF0Um91dGVzKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgY29uc3QgZmxhdCA9IChyOiBBQlAuRnVsbFJvdXRlW10pID0+IHtcbiAgICByZXR1cm4gci5yZWR1Y2UoKGFjYywgdmFsKSA9PiB7XG4gICAgICBsZXQgdmFsdWU6IEFCUC5GdWxsUm91dGVbXSA9IFt2YWxdO1xuICAgICAgaWYgKHZhbC5jaGlsZHJlbikge1xuICAgICAgICBjb25zdCB7IGNoaWxkcmVuIH0gPSB2YWw7XG4gICAgICAgIGRlbGV0ZSB2YWwuY2hpbGRyZW47XG4gICAgICAgIHZhbHVlID0gW3ZhbCwgLi4uZmxhdChjaGlsZHJlbildO1xuICAgICAgfVxuXG4gICAgICByZXR1cm4gWy4uLmFjYywgLi4udmFsdWVdO1xuICAgIH0sIFtdKTtcbiAgfTtcblxuICByZXR1cm4gZmxhdChyb3V0ZXMpO1xufVxuIl19