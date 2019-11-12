var ConfigState_1;
/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/config.state.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, createSelector, Selector, State, StateContext, Store } from '@ngxs/store';
import { of } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import snq from 'snq';
import { GetAppConfiguration, PatchRouteByName } from '../actions/config.actions';
import { SetLanguage } from '../actions/session.actions';
import { ApplicationConfigurationService } from '../services/application-configuration.service';
import { organizeRoutes } from '../utils/route-utils';
import { SessionState } from './session.state';
let ConfigState = ConfigState_1 = class ConfigState {
    /**
     * @param {?} appConfigurationService
     * @param {?} store
     */
    constructor(appConfigurationService, store) {
        this.appConfigurationService = appConfigurationService;
        this.store = store;
    }
    /**
     * @param {?} state
     * @return {?}
     */
    static getAll(state) {
        return state;
    }
    /**
     * @param {?} state
     * @return {?}
     */
    static getApplicationInfo(state) {
        return state.environment.application || ((/** @type {?} */ ({})));
    }
    /**
     * @param {?} key
     * @return {?}
     */
    static getOne(key) {
        /** @type {?} */
        const selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        (state) => {
            return state[key];
        }));
        return selector;
    }
    /**
     * @param {?} keys
     * @return {?}
     */
    static getDeep(keys) {
        if (typeof keys === 'string') {
            keys = keys.split('.');
        }
        if (!Array.isArray(keys)) {
            throw new Error('The argument must be a dot string or an string array.');
        }
        /** @type {?} */
        const selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        (state) => {
            return ((/** @type {?} */ (keys))).reduce((/**
             * @param {?} acc
             * @param {?} val
             * @return {?}
             */
            (acc, val) => {
                if (acc) {
                    return acc[val];
                }
                return undefined;
            }), state);
        }));
        return selector;
    }
    /**
     * @param {?=} path
     * @param {?=} name
     * @return {?}
     */
    static getRoute(path, name) {
        /** @type {?} */
        const selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        (state) => {
            const { flattedRoutes } = state;
            return ((/** @type {?} */ (flattedRoutes))).find((/**
             * @param {?} route
             * @return {?}
             */
            route => {
                if (path && route.path === path) {
                    return route;
                }
                else if (name && route.name === name) {
                    return route;
                }
            }));
        }));
        return selector;
    }
    /**
     * @param {?=} key
     * @return {?}
     */
    static getApiUrl(key) {
        /** @type {?} */
        const selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        (state) => {
            return state.environment.apis[key || 'default'].url;
        }));
        return selector;
    }
    /**
     * @param {?} key
     * @return {?}
     */
    static getSetting(key) {
        /** @type {?} */
        const selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        (state) => {
            return snq((/**
             * @return {?}
             */
            () => state.setting.values[key]));
        }));
        return selector;
    }
    /**
     * @param {?=} keyword
     * @return {?}
     */
    static getSettings(keyword) {
        /** @type {?} */
        const selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        (state) => {
            if (keyword) {
                /** @type {?} */
                const keys = snq((/**
                 * @return {?}
                 */
                () => Object.keys(state.setting.values).filter((/**
                 * @param {?} key
                 * @return {?}
                 */
                key => key.indexOf(keyword) > -1))), []);
                if (keys.length) {
                    return keys.reduce((/**
                     * @param {?} acc
                     * @param {?} key
                     * @return {?}
                     */
                    (acc, key) => (Object.assign({}, acc, { [key]: state.setting.values[key] }))), {});
                }
            }
            return snq((/**
             * @return {?}
             */
            () => state.setting.values), {});
        }));
        return selector;
    }
    /**
     * @param {?} key
     * @return {?}
     */
    static getGrantedPolicy(key) {
        /** @type {?} */
        const selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        (state) => {
            if (!key)
                return true;
            return snq((/**
             * @return {?}
             */
            () => state.auth.grantedPolicies[key]), false);
        }));
        return selector;
    }
    /**
     * @param {?} key
     * @param {...?} interpolateParams
     * @return {?}
     */
    static getLocalization(key, ...interpolateParams) {
        /** @type {?} */
        let defaultValue;
        if (typeof key !== 'string') {
            defaultValue = key.defaultValue;
            key = key.key;
        }
        if (!key)
            key = '';
        /** @type {?} */
        const keys = (/** @type {?} */ (key.split('::')));
        /** @type {?} */
        const selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        (state) => {
            if (!state.localization)
                return defaultValue || key;
            const { defaultResourceName } = state.environment.localization;
            if (keys[0] === '') {
                if (!defaultResourceName) {
                    throw new Error(`Please check your environment. May you forget set defaultResourceName?
              Here is the example:
               { production: false,
                 localization: {
                   defaultResourceName: 'MyProjectName'
                  }
               }`);
                }
                keys[0] = snq((/**
                 * @return {?}
                 */
                () => defaultResourceName));
            }
            /** @type {?} */
            let localization = ((/** @type {?} */ (keys))).reduce((/**
             * @param {?} acc
             * @param {?} val
             * @return {?}
             */
            (acc, val) => {
                if (acc) {
                    return acc[val];
                }
                return undefined;
            }), state.localization.values);
            interpolateParams = interpolateParams.filter((/**
             * @param {?} params
             * @return {?}
             */
            params => params != null));
            if (localization && interpolateParams && interpolateParams.length) {
                interpolateParams.forEach((/**
                 * @param {?} param
                 * @return {?}
                 */
                param => {
                    localization = localization.replace(/[\'\"]?\{[\d]+\}[\'\"]?/, param);
                }));
            }
            if (typeof localization !== 'string')
                localization = '';
            return localization || defaultValue || key;
        }));
        return selector;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    addData({ patchState, dispatch }) {
        return this.appConfigurationService.getConfiguration().pipe(tap((/**
         * @param {?} configuration
         * @return {?}
         */
        configuration => patchState(Object.assign({}, configuration)))), switchMap((/**
         * @param {?} configuration
         * @return {?}
         */
        configuration => {
            /** @type {?} */
            let defaultLang = configuration.setting.values['Abp.Localization.DefaultLanguage'];
            if (defaultLang.includes(';')) {
                defaultLang = defaultLang.split(';')[0];
            }
            return this.store.selectSnapshot(SessionState.getLanguage) ? of(null) : dispatch(new SetLanguage(defaultLang));
        })));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    patchRoute({ patchState, getState }, { name, newValue }) {
        /** @type {?} */
        let routes = getState().routes;
        /** @type {?} */
        const index = routes.findIndex((/**
         * @param {?} route
         * @return {?}
         */
        route => route.name === name));
        routes = patchRouteDeep(routes, name, newValue);
        return patchState({
            routes,
        });
    }
};
ConfigState.ctorParameters = () => [
    { type: ApplicationConfigurationService },
    { type: Store }
];
tslib_1.__decorate([
    Action(GetAppConfiguration),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object]),
    tslib_1.__metadata("design:returntype", void 0)
], ConfigState.prototype, "addData", null);
tslib_1.__decorate([
    Action(PatchRouteByName),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, PatchRouteByName]),
    tslib_1.__metadata("design:returntype", void 0)
], ConfigState.prototype, "patchRoute", null);
tslib_1.__decorate([
    Selector(),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object]),
    tslib_1.__metadata("design:returntype", void 0)
], ConfigState, "getAll", null);
tslib_1.__decorate([
    Selector(),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object]),
    tslib_1.__metadata("design:returntype", Object)
], ConfigState, "getApplicationInfo", null);
ConfigState = ConfigState_1 = tslib_1.__decorate([
    State({
        name: 'ConfigState',
        defaults: (/** @type {?} */ ({})),
    }),
    tslib_1.__metadata("design:paramtypes", [ApplicationConfigurationService, Store])
], ConfigState);
export { ConfigState };
if (false) {
    /**
     * @type {?}
     * @private
     */
    ConfigState.prototype.appConfigurationService;
    /**
     * @type {?}
     * @private
     */
    ConfigState.prototype.store;
}
/**
 * @param {?} routes
 * @param {?} name
 * @param {?} newValue
 * @param {?=} parentUrl
 * @return {?}
 */
function patchRouteDeep(routes, name, newValue, parentUrl = '') {
    routes = routes.map((/**
     * @param {?} route
     * @return {?}
     */
    route => {
        if (route.name === name) {
            newValue.url = `${parentUrl}/${(!newValue.path && newValue.path === '' ? route.path : newValue.path) || ''}`;
            if (newValue.children && newValue.children.length) {
                newValue.children = newValue.children.map((/**
                 * @param {?} child
                 * @return {?}
                 */
                child => (Object.assign({}, child, { url: `${newValue.url}/${child.path}`.replace('//', '/') }))));
            }
            return Object.assign({}, route, newValue);
        }
        else if (route.children && route.children.length) {
            route.children = patchRouteDeep(route.children, name, newValue, (parentUrl || '/') + route.path);
        }
        return route;
    }));
    if (parentUrl) {
        // recursive block
        return routes;
    }
    return organizeRoutes(routes);
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9jb25maWcuc3RhdGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsY0FBYyxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUMzRixPQUFPLEVBQUUsRUFBRSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzFCLE9BQU8sRUFBRSxTQUFTLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDaEQsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLDJCQUEyQixDQUFDO0FBQ2xGLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUd6RCxPQUFPLEVBQUUsK0JBQStCLEVBQUUsTUFBTSwrQ0FBK0MsQ0FBQztBQUNoRyxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdEQsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLGlCQUFpQixDQUFDO0lBTWxDLFdBQVcseUJBQVgsV0FBVzs7Ozs7SUE0S3RCLFlBQW9CLHVCQUF3RCxFQUFVLEtBQVk7UUFBOUUsNEJBQXVCLEdBQXZCLHVCQUF1QixDQUFpQztRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7OztJQTFLdEcsTUFBTSxDQUFDLE1BQU0sQ0FBQyxLQUFtQjtRQUMvQixPQUFPLEtBQUssQ0FBQztJQUNmLENBQUM7Ozs7O0lBR0QsTUFBTSxDQUFDLGtCQUFrQixDQUFDLEtBQW1CO1FBQzNDLE9BQU8sS0FBSyxDQUFDLFdBQVcsQ0FBQyxXQUFXLElBQUksQ0FBQyxtQkFBQSxFQUFFLEVBQXNCLENBQUMsQ0FBQztJQUNyRSxDQUFD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Q0FBQyxHQUFHLEdBQUcsR0FBRyxTQUFTLElBQUksQ0FBQyxDQUFDLFFBQVEsQ0FBQyxJQUFJLElBQUksUUFBUSxDQUFDLElBQUksS0FBSyxFQUFFLENBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLFFBQVEsQ0FBQyxJQUFJLENBQUMsSUFBSSxFQUFFLEVBQUUsQ0FBQztZQUU3RyxJQUFJLFFBQVEsQ0FBQyxRQUFRLElBQUksUUFBUSxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7Z0JBQ2pELFFBQVEsQ0FBQyxRQUFRLEdBQUcsUUFBUSxDQUFDLFFBQVEsQ0FBQyxHQUFHOzs7O2dCQUFDLEtBQUssQ0FBQyxFQUFFLENBQUMsbUJBQzlDLEtBQUssSUFDUixHQUFHLEVBQUUsR0FBRyxRQUFRLENBQUMsR0FBRyxJQUFJLEtBQUssQ0FBQyxJQUFJLEVBQUUsQ0FBQyxPQUFPLENBQUMsSUFBSSxFQUFFLEdBQUcsQ0FBQyxJQUN2RCxFQUFDLENBQUM7YUFDTDtZQUVELHlCQUFZLEtBQUssRUFBSyxRQUFRLEVBQUc7U0FDbEM7YUFBTSxJQUFJLEtBQUssQ0FBQyxRQUFRLElBQUksS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDbEQsS0FBSyxDQUFDLFFBQVEsR0FBRyxjQUFjLENBQUMsS0FBSyxDQUFDLFFBQVEsRUFBRSxJQUFJLEVBQUUsUUFBUSxFQUFFLENBQUMsU0FBUyxJQUFJLEdBQUcsQ0FBQyxHQUFHLEtBQUssQ0FBQyxJQUFJLENBQUMsQ0FBQztTQUNsRztRQUVELE9BQU8sS0FBSyxDQUFDO0lBQ2YsQ0FBQyxFQUFDLENBQUM7SUFFSCxJQUFJLFNBQVMsRUFBRTtRQUNiLGtCQUFrQjtRQUNsQixPQUFPLE1BQU0sQ0FBQztLQUNmO0lBRUQsT0FBTyxjQUFjLENBQUMsTUFBTSxDQUFDLENBQUM7QUFDaEMsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgY3JlYXRlU2VsZWN0b3IsIFNlbGVjdG9yLCBTdGF0ZSwgU3RhdGVDb250ZXh0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgb2YgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgc3dpdGNoTWFwLCB0YXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XHJcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcclxuaW1wb3J0IHsgR2V0QXBwQ29uZmlndXJhdGlvbiwgUGF0Y2hSb3V0ZUJ5TmFtZSB9IGZyb20gJy4uL2FjdGlvbnMvY29uZmlnLmFjdGlvbnMnO1xyXG5pbXBvcnQgeyBTZXRMYW5ndWFnZSB9IGZyb20gJy4uL2FjdGlvbnMvc2Vzc2lvbi5hY3Rpb25zJztcclxuaW1wb3J0IHsgQUJQIH0gZnJvbSAnLi4vbW9kZWxzL2NvbW1vbic7XHJcbmltcG9ydCB7IENvbmZpZyB9IGZyb20gJy4uL21vZGVscy9jb25maWcnO1xyXG5pbXBvcnQgeyBBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvYXBwbGljYXRpb24tY29uZmlndXJhdGlvbi5zZXJ2aWNlJztcclxuaW1wb3J0IHsgb3JnYW5pemVSb3V0ZXMgfSBmcm9tICcuLi91dGlscy9yb3V0ZS11dGlscyc7XHJcbmltcG9ydCB7IFNlc3Npb25TdGF0ZSB9IGZyb20gJy4vc2Vzc2lvbi5zdGF0ZSc7XHJcblxyXG5AU3RhdGU8Q29uZmlnLlN0YXRlPih7XHJcbiAgbmFtZTogJ0NvbmZpZ1N0YXRlJyxcclxuICBkZWZhdWx0czoge30gYXMgQ29uZmlnLlN0YXRlLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgQ29uZmlnU3RhdGUge1xyXG4gIEBTZWxlY3RvcigpXHJcbiAgc3RhdGljIGdldEFsbChzdGF0ZTogQ29uZmlnLlN0YXRlKSB7XHJcbiAgICByZXR1cm4gc3RhdGU7XHJcbiAgfVxyXG5cclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXRBcHBsaWNhdGlvbkluZm8oc3RhdGU6IENvbmZpZy5TdGF0ZSk6IENvbmZpZy5BcHBsaWNhdGlvbiB7XHJcbiAgICByZXR1cm4gc3RhdGUuZW52aXJvbm1lbnQuYXBwbGljYXRpb24gfHwgKHt9IGFzIENvbmZpZy5BcHBsaWNhdGlvbik7XHJcbiAgfVxyXG5cclxuICBzdGF0aWMgZ2V0T25lKGtleTogc3RyaW5nKSB7XHJcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxyXG4gICAgICBbQ29uZmlnU3RhdGVdLFxyXG4gICAgICAoc3RhdGU6IENvbmZpZy5TdGF0ZSkgPT4ge1xyXG4gICAgICAgIHJldHVybiBzdGF0ZVtrZXldO1xyXG4gICAgICB9LFxyXG4gICAgKTtcclxuXHJcbiAgICByZXR1cm4gc2VsZWN0b3I7XHJcbiAgfVxyXG5cclxuICBzdGF0aWMgZ2V0RGVlcChrZXlzOiBzdHJpbmdbXSB8IHN0cmluZykge1xyXG4gICAgaWYgKHR5cGVvZiBrZXlzID09PSAnc3RyaW5nJykge1xyXG4gICAgICBrZXlzID0ga2V5cy5zcGxpdCgnLicpO1xyXG4gICAgfVxyXG5cclxuICAgIGlmICghQXJyYXkuaXNBcnJheShrZXlzKSkge1xyXG4gICAgICB0aHJvdyBuZXcgRXJyb3IoJ1RoZSBhcmd1bWVudCBtdXN0IGJlIGEgZG90IHN0cmluZyBvciBhbiBzdHJpbmcgYXJyYXkuJyk7XHJcbiAgICB9XHJcblxyXG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcclxuICAgICAgW0NvbmZpZ1N0YXRlXSxcclxuICAgICAgKHN0YXRlOiBDb25maWcuU3RhdGUpID0+IHtcclxuICAgICAgICByZXR1cm4gKGtleXMgYXMgc3RyaW5nW10pLnJlZHVjZSgoYWNjLCB2YWwpID0+IHtcclxuICAgICAgICAgIGlmIChhY2MpIHtcclxuICAgICAgICAgICAgcmV0dXJuIGFjY1t2YWxdO1xyXG4gICAgICAgICAgfVxyXG5cclxuICAgICAgICAgIHJldHVybiB1bmRlZmluZWQ7XHJcbiAgICAgICAgfSwgc3RhdGUpO1xyXG4gICAgICB9LFxyXG4gICAgKTtcclxuXHJcbiAgICByZXR1cm4gc2VsZWN0b3I7XHJcbiAgfVxyXG5cclxuICBzdGF0aWMgZ2V0Um91dGUocGF0aD86IHN0cmluZywgbmFtZT86IHN0cmluZykge1xyXG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcclxuICAgICAgW0NvbmZpZ1N0YXRlXSxcclxuICAgICAgKHN0YXRlOiBDb25maWcuU3RhdGUpID0+IHtcclxuICAgICAgICBjb25zdCB7IGZsYXR0ZWRSb3V0ZXMgfSA9IHN0YXRlO1xyXG4gICAgICAgIHJldHVybiAoZmxhdHRlZFJvdXRlcyBhcyBBQlAuRnVsbFJvdXRlW10pLmZpbmQocm91dGUgPT4ge1xyXG4gICAgICAgICAgaWYgKHBhdGggJiYgcm91dGUucGF0aCA9PT0gcGF0aCkge1xyXG4gICAgICAgICAgICByZXR1cm4gcm91dGU7XHJcbiAgICAgICAgICB9IGVsc2UgaWYgKG5hbWUgJiYgcm91dGUubmFtZSA9PT0gbmFtZSkge1xyXG4gICAgICAgICAgICByZXR1cm4gcm91dGU7XHJcbiAgICAgICAgICB9XHJcbiAgICAgICAgfSk7XHJcbiAgICAgIH0sXHJcbiAgICApO1xyXG5cclxuICAgIHJldHVybiBzZWxlY3RvcjtcclxuICB9XHJcblxyXG4gIHN0YXRpYyBnZXRBcGlVcmwoa2V5Pzogc3RyaW5nKSB7XHJcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxyXG4gICAgICBbQ29uZmlnU3RhdGVdLFxyXG4gICAgICAoc3RhdGU6IENvbmZpZy5TdGF0ZSk6IHN0cmluZyA9PiB7XHJcbiAgICAgICAgcmV0dXJuIHN0YXRlLmVudmlyb25tZW50LmFwaXNba2V5IHx8ICdkZWZhdWx0J10udXJsO1xyXG4gICAgICB9LFxyXG4gICAgKTtcclxuXHJcbiAgICByZXR1cm4gc2VsZWN0b3I7XHJcbiAgfVxyXG5cclxuICBzdGF0aWMgZ2V0U2V0dGluZyhrZXk6IHN0cmluZykge1xyXG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcclxuICAgICAgW0NvbmZpZ1N0YXRlXSxcclxuICAgICAgKHN0YXRlOiBDb25maWcuU3RhdGUpID0+IHtcclxuICAgICAgICByZXR1cm4gc25xKCgpID0+IHN0YXRlLnNldHRpbmcudmFsdWVzW2tleV0pO1xyXG4gICAgICB9LFxyXG4gICAgKTtcclxuICAgIHJldHVybiBzZWxlY3RvcjtcclxuICB9XHJcblxyXG4gIHN0YXRpYyBnZXRTZXR0aW5ncyhrZXl3b3JkPzogc3RyaW5nKSB7XHJcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxyXG4gICAgICBbQ29uZmlnU3RhdGVdLFxyXG4gICAgICAoc3RhdGU6IENvbmZpZy5TdGF0ZSkgPT4ge1xyXG4gICAgICAgIGlmIChrZXl3b3JkKSB7XHJcbiAgICAgICAgICBjb25zdCBrZXlzID0gc25xKCgpID0+IE9iamVjdC5rZXlzKHN0YXRlLnNldHRpbmcudmFsdWVzKS5maWx0ZXIoa2V5ID0+IGtleS5pbmRleE9mKGtleXdvcmQpID4gLTEpLCBbXSk7XHJcblxyXG4gICAgICAgICAgaWYgKGtleXMubGVuZ3RoKSB7XHJcbiAgICAgICAgICAgIHJldHVybiBrZXlzLnJlZHVjZSgoYWNjLCBrZXkpID0+ICh7IC4uLmFjYywgW2tleV06IHN0YXRlLnNldHRpbmcudmFsdWVzW2tleV0gfSksIHt9KTtcclxuICAgICAgICAgIH1cclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIHJldHVybiBzbnEoKCkgPT4gc3RhdGUuc2V0dGluZy52YWx1ZXMsIHt9KTtcclxuICAgICAgfSxcclxuICAgICk7XHJcbiAgICByZXR1cm4gc2VsZWN0b3I7XHJcbiAgfVxyXG5cclxuICBzdGF0aWMgZ2V0R3JhbnRlZFBvbGljeShrZXk6IHN0cmluZykge1xyXG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcclxuICAgICAgW0NvbmZpZ1N0YXRlXSxcclxuICAgICAgKHN0YXRlOiBDb25maWcuU3RhdGUpOiBib29sZWFuID0+IHtcclxuICAgICAgICBpZiAoIWtleSkgcmV0dXJuIHRydWU7XHJcbiAgICAgICAgcmV0dXJuIHNucSgoKSA9PiBzdGF0ZS5hdXRoLmdyYW50ZWRQb2xpY2llc1trZXldLCBmYWxzZSk7XHJcbiAgICAgIH0sXHJcbiAgICApO1xyXG5cclxuICAgIHJldHVybiBzZWxlY3RvcjtcclxuICB9XHJcblxyXG4gIHN0YXRpYyBnZXRMb2NhbGl6YXRpb24oa2V5OiBzdHJpbmcgfCBDb25maWcuTG9jYWxpemF0aW9uV2l0aERlZmF1bHQsIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXSkge1xyXG4gICAgbGV0IGRlZmF1bHRWYWx1ZTogc3RyaW5nO1xyXG5cclxuICAgIGlmICh0eXBlb2Yga2V5ICE9PSAnc3RyaW5nJykge1xyXG4gICAgICBkZWZhdWx0VmFsdWUgPSBrZXkuZGVmYXVsdFZhbHVlO1xyXG4gICAgICBrZXkgPSBrZXkua2V5O1xyXG4gICAgfVxyXG5cclxuICAgIGlmICgha2V5KSBrZXkgPSAnJztcclxuXHJcbiAgICBjb25zdCBrZXlzID0ga2V5LnNwbGl0KCc6OicpIGFzIHN0cmluZ1tdO1xyXG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcclxuICAgICAgW0NvbmZpZ1N0YXRlXSxcclxuICAgICAgKHN0YXRlOiBDb25maWcuU3RhdGUpID0+IHtcclxuICAgICAgICBpZiAoIXN0YXRlLmxvY2FsaXphdGlvbikgcmV0dXJuIGRlZmF1bHRWYWx1ZSB8fCBrZXk7XHJcblxyXG4gICAgICAgIGNvbnN0IHsgZGVmYXVsdFJlc291cmNlTmFtZSB9ID0gc3RhdGUuZW52aXJvbm1lbnQubG9jYWxpemF0aW9uO1xyXG4gICAgICAgIGlmIChrZXlzWzBdID09PSAnJykge1xyXG4gICAgICAgICAgaWYgKCFkZWZhdWx0UmVzb3VyY2VOYW1lKSB7XHJcbiAgICAgICAgICAgIHRocm93IG5ldyBFcnJvcihcclxuICAgICAgICAgICAgICBgUGxlYXNlIGNoZWNrIHlvdXIgZW52aXJvbm1lbnQuIE1heSB5b3UgZm9yZ2V0IHNldCBkZWZhdWx0UmVzb3VyY2VOYW1lP1xyXG4gICAgICAgICAgICAgIEhlcmUgaXMgdGhlIGV4YW1wbGU6XHJcbiAgICAgICAgICAgICAgIHsgcHJvZHVjdGlvbjogZmFsc2UsXHJcbiAgICAgICAgICAgICAgICAgbG9jYWxpemF0aW9uOiB7XHJcbiAgICAgICAgICAgICAgICAgICBkZWZhdWx0UmVzb3VyY2VOYW1lOiAnTXlQcm9qZWN0TmFtZSdcclxuICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICB9YCxcclxuICAgICAgICAgICAgKTtcclxuICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICBrZXlzWzBdID0gc25xKCgpID0+IGRlZmF1bHRSZXNvdXJjZU5hbWUpO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgbGV0IGxvY2FsaXphdGlvbiA9IChrZXlzIGFzIGFueSkucmVkdWNlKChhY2MsIHZhbCkgPT4ge1xyXG4gICAgICAgICAgaWYgKGFjYykge1xyXG4gICAgICAgICAgICByZXR1cm4gYWNjW3ZhbF07XHJcbiAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgcmV0dXJuIHVuZGVmaW5lZDtcclxuICAgICAgICB9LCBzdGF0ZS5sb2NhbGl6YXRpb24udmFsdWVzKTtcclxuXHJcbiAgICAgICAgaW50ZXJwb2xhdGVQYXJhbXMgPSBpbnRlcnBvbGF0ZVBhcmFtcy5maWx0ZXIocGFyYW1zID0+IHBhcmFtcyAhPSBudWxsKTtcclxuICAgICAgICBpZiAobG9jYWxpemF0aW9uICYmIGludGVycG9sYXRlUGFyYW1zICYmIGludGVycG9sYXRlUGFyYW1zLmxlbmd0aCkge1xyXG4gICAgICAgICAgaW50ZXJwb2xhdGVQYXJhbXMuZm9yRWFjaChwYXJhbSA9PiB7XHJcbiAgICAgICAgICAgIGxvY2FsaXphdGlvbiA9IGxvY2FsaXphdGlvbi5yZXBsYWNlKC9bXFwnXFxcIl0/XFx7W1xcZF0rXFx9W1xcJ1xcXCJdPy8sIHBhcmFtKTtcclxuICAgICAgICAgIH0pO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgaWYgKHR5cGVvZiBsb2NhbGl6YXRpb24gIT09ICdzdHJpbmcnKSBsb2NhbGl6YXRpb24gPSAnJztcclxuICAgICAgICByZXR1cm4gbG9jYWxpemF0aW9uIHx8IGRlZmF1bHRWYWx1ZSB8fCBrZXk7XHJcbiAgICAgIH0sXHJcbiAgICApO1xyXG5cclxuICAgIHJldHVybiBzZWxlY3RvcjtcclxuICB9XHJcblxyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgYXBwQ29uZmlndXJhdGlvblNlcnZpY2U6IEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvblNlcnZpY2UsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxyXG5cclxuICBAQWN0aW9uKEdldEFwcENvbmZpZ3VyYXRpb24pXHJcbiAgYWRkRGF0YSh7IHBhdGNoU3RhdGUsIGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxDb25maWcuU3RhdGU+KSB7XHJcbiAgICByZXR1cm4gdGhpcy5hcHBDb25maWd1cmF0aW9uU2VydmljZS5nZXRDb25maWd1cmF0aW9uKCkucGlwZShcclxuICAgICAgdGFwKGNvbmZpZ3VyYXRpb24gPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIC4uLmNvbmZpZ3VyYXRpb24sXHJcbiAgICAgICAgfSksXHJcbiAgICAgICksXHJcbiAgICAgIHN3aXRjaE1hcChjb25maWd1cmF0aW9uID0+IHtcclxuICAgICAgICBsZXQgZGVmYXVsdExhbmc6IHN0cmluZyA9IGNvbmZpZ3VyYXRpb24uc2V0dGluZy52YWx1ZXNbJ0FicC5Mb2NhbGl6YXRpb24uRGVmYXVsdExhbmd1YWdlJ107XHJcblxyXG4gICAgICAgIGlmIChkZWZhdWx0TGFuZy5pbmNsdWRlcygnOycpKSB7XHJcbiAgICAgICAgICBkZWZhdWx0TGFuZyA9IGRlZmF1bHRMYW5nLnNwbGl0KCc7JylbMF07XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXNzaW9uU3RhdGUuZ2V0TGFuZ3VhZ2UpID8gb2YobnVsbCkgOiBkaXNwYXRjaChuZXcgU2V0TGFuZ3VhZ2UoZGVmYXVsdExhbmcpKTtcclxuICAgICAgfSksXHJcbiAgICApO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihQYXRjaFJvdXRlQnlOYW1lKVxyXG4gIHBhdGNoUm91dGUoeyBwYXRjaFN0YXRlLCBnZXRTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8Q29uZmlnLlN0YXRlPiwgeyBuYW1lLCBuZXdWYWx1ZSB9OiBQYXRjaFJvdXRlQnlOYW1lKSB7XHJcbiAgICBsZXQgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10gPSBnZXRTdGF0ZSgpLnJvdXRlcztcclxuXHJcbiAgICBjb25zdCBpbmRleCA9IHJvdXRlcy5maW5kSW5kZXgocm91dGUgPT4gcm91dGUubmFtZSA9PT0gbmFtZSk7XHJcblxyXG4gICAgcm91dGVzID0gcGF0Y2hSb3V0ZURlZXAocm91dGVzLCBuYW1lLCBuZXdWYWx1ZSk7XHJcblxyXG4gICAgcmV0dXJuIHBhdGNoU3RhdGUoe1xyXG4gICAgICByb3V0ZXMsXHJcbiAgICB9KTtcclxuICB9XHJcbn1cclxuXHJcbmZ1bmN0aW9uIHBhdGNoUm91dGVEZWVwKFxyXG4gIHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdLFxyXG4gIG5hbWU6IHN0cmluZyxcclxuICBuZXdWYWx1ZTogUGFydGlhbDxBQlAuRnVsbFJvdXRlPixcclxuICBwYXJlbnRVcmw6IHN0cmluZyA9ICcnLFxyXG4pOiBBQlAuRnVsbFJvdXRlW10ge1xyXG4gIHJvdXRlcyA9IHJvdXRlcy5tYXAocm91dGUgPT4ge1xyXG4gICAgaWYgKHJvdXRlLm5hbWUgPT09IG5hbWUpIHtcclxuICAgICAgbmV3VmFsdWUudXJsID0gYCR7cGFyZW50VXJsfS8keyghbmV3VmFsdWUucGF0aCAmJiBuZXdWYWx1ZS5wYXRoID09PSAnJyA/IHJvdXRlLnBhdGggOiBuZXdWYWx1ZS5wYXRoKSB8fCAnJ31gO1xyXG5cclxuICAgICAgaWYgKG5ld1ZhbHVlLmNoaWxkcmVuICYmIG5ld1ZhbHVlLmNoaWxkcmVuLmxlbmd0aCkge1xyXG4gICAgICAgIG5ld1ZhbHVlLmNoaWxkcmVuID0gbmV3VmFsdWUuY2hpbGRyZW4ubWFwKGNoaWxkID0+ICh7XHJcbiAgICAgICAgICAuLi5jaGlsZCxcclxuICAgICAgICAgIHVybDogYCR7bmV3VmFsdWUudXJsfS8ke2NoaWxkLnBhdGh9YC5yZXBsYWNlKCcvLycsICcvJyksXHJcbiAgICAgICAgfSkpO1xyXG4gICAgICB9XHJcblxyXG4gICAgICByZXR1cm4geyAuLi5yb3V0ZSwgLi4ubmV3VmFsdWUgfTtcclxuICAgIH0gZWxzZSBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XHJcbiAgICAgIHJvdXRlLmNoaWxkcmVuID0gcGF0Y2hSb3V0ZURlZXAocm91dGUuY2hpbGRyZW4sIG5hbWUsIG5ld1ZhbHVlLCAocGFyZW50VXJsIHx8ICcvJykgKyByb3V0ZS5wYXRoKTtcclxuICAgIH1cclxuXHJcbiAgICByZXR1cm4gcm91dGU7XHJcbiAgfSk7XHJcblxyXG4gIGlmIChwYXJlbnRVcmwpIHtcclxuICAgIC8vIHJlY3Vyc2l2ZSBibG9ja1xyXG4gICAgcmV0dXJuIHJvdXRlcztcclxuICB9XHJcblxyXG4gIHJldHVybiBvcmdhbml6ZVJvdXRlcyhyb3V0ZXMpO1xyXG59XHJcbiJdfQ==