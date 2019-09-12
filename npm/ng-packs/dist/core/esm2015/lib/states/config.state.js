var ConfigState_1;
/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { State, Selector, createSelector, Action, Store } from '@ngxs/store';
import { GetAppConfiguration, PatchRouteByName } from '../actions/config.actions';
import { ApplicationConfigurationService } from '../services/application-configuration.service';
import { tap, switchMap } from 'rxjs/operators';
import snq from 'snq';
import { SetLanguage } from '../actions';
import { SessionState } from './session.state';
import { of } from 'rxjs';
import { organizeRoutes } from '../utils/route-utils';
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
        function (state) {
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
        function (state) {
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
        function (state) {
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
        function (state) {
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
        function (state) {
            return snq((/**
             * @return {?}
             */
            () => state.setting.values[key]));
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
        function (state) {
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
    static getCopy(key, ...interpolateParams) {
        if (!key)
            key = '';
        /** @type {?} */
        const keys = (/** @type {?} */ (key.split('::')));
        /** @type {?} */
        const selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            if (!state.localization)
                return key;
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
            let copy = ((/** @type {?} */ (keys))).reduce((/**
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
            if (copy && interpolateParams && interpolateParams.length) {
                interpolateParams.forEach((/**
                 * @param {?} param
                 * @return {?}
                 */
                param => {
                    copy = copy.replace(/[\'\"]?\{[\d]+\}[\'\"]?/, param);
                }));
            }
            return copy || key;
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
function patchRouteDeep(routes, name, newValue, parentUrl = null) {
    routes = routes.map((/**
     * @param {?} route
     * @return {?}
     */
    route => {
        if (route.name === name) {
            if (newValue.path) {
                newValue.url = `${parentUrl}/${newValue.path}`;
            }
            if (newValue.children && newValue.children.length) {
                newValue.children = newValue.children.map((/**
                 * @param {?} child
                 * @return {?}
                 */
                child => (Object.assign({}, child, { url: `${parentUrl}/${route.path}/${child.path}` }))));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9jb25maWcuc3RhdGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLEtBQUssRUFBRSxRQUFRLEVBQUUsY0FBYyxFQUFFLE1BQU0sRUFBZ0IsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBRTNGLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLDJCQUEyQixDQUFDO0FBQ2xGLE9BQU8sRUFBRSwrQkFBK0IsRUFBRSxNQUFNLCtDQUErQyxDQUFDO0FBQ2hHLE9BQU8sRUFBRSxHQUFHLEVBQUUsU0FBUyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDaEQsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxZQUFZLENBQUM7QUFDekMsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQy9DLE9BQU8sRUFBRSxFQUFFLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDMUIsT0FBTyxFQUE2QixjQUFjLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztJQU1wRSxXQUFXLHlCQUFYLFdBQVc7Ozs7O0lBbUp0QixZQUFvQix1QkFBd0QsRUFBVSxLQUFZO1FBQTlFLDRCQUF1QixHQUF2Qix1QkFBdUIsQ0FBaUM7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQzs7Ozs7SUFqSnRHLE1BQU0sQ0FBQyxNQUFNLENBQUMsS0FBbUI7UUFDL0IsT0FBTyxLQUFLLENBQUM7SUFDZixDQUFDOzs7OztJQUdELE1BQU0sQ0FBQyxrQkFBa0IsQ0FBQyxLQUFtQjtRQUMzQyxPQUFPLEtBQUssQ0FBQyxXQUFXLENBQUMsV0FBVyxJQUFJLENBQUMsbUJBQUEsRUFBRSxFQUFzQixDQUFDLENBQUM7SUFDckUsQ0FBQzs7Ozs7SUFFRCxN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QUFDO2FBQ2hEO1lBRUQsSUFBSSxRQUFRLENBQUMsUUFBUSxJQUFJLFFBQVEsQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO2dCQUNqRCxRQUFRLENBQUMsUUFBUSxHQUFHLFFBQVEsQ0FBQyxRQUFRLENBQUMsR0FBRzs7OztnQkFBQyxLQUFLLENBQUMsRUFBRSxDQUFDLG1CQUM5QyxLQUFLLElBQ1IsR0FBRyxFQUFFLEdBQUcsU0FBUyxJQUFJLEtBQUssQ0FBQyxJQUFJLElBQUksS0FBSyxDQUFDLElBQUksRUFBRSxJQUMvQyxFQUFDLENBQUM7YUFDTDtZQUVELHlCQUFZLEtBQUssRUFBSyxRQUFRLEVBQUc7U0FDbEM7YUFBTSxJQUFJLEtBQUssQ0FBQyxRQUFRLElBQUksS0FBSyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDbEQsS0FBSyxDQUFDLFFBQVEsR0FBRyxjQUFjLENBQUMsS0FBSyxDQUFDLFFBQVEsRUFBRSxJQUFJLEVBQUUsUUFBUSxFQUFFLENBQUMsU0FBUyxJQUFJLEdBQUcsQ0FBQyxHQUFHLEtBQUssQ0FBQyxJQUFJLENBQUMsQ0FBQztTQUNsRztRQUVELE9BQU8sS0FBSyxDQUFDO0lBQ2YsQ0FBQyxFQUFDLENBQUM7SUFFSCxJQUFJLFNBQVMsRUFBRTtRQUNiLGtCQUFrQjtRQUNsQixPQUFPLE1BQU0sQ0FBQztLQUNmO0lBRUQsT0FBTyxjQUFjLENBQUMsTUFBTSxDQUFDLENBQUM7QUFDaEMsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFN0YXRlLCBTZWxlY3RvciwgY3JlYXRlU2VsZWN0b3IsIEFjdGlvbiwgU3RhdGVDb250ZXh0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IENvbmZpZywgQUJQIH0gZnJvbSAnLi4vbW9kZWxzJztcbmltcG9ydCB7IEdldEFwcENvbmZpZ3VyYXRpb24sIFBhdGNoUm91dGVCeU5hbWUgfSBmcm9tICcuLi9hY3Rpb25zL2NvbmZpZy5hY3Rpb25zJztcbmltcG9ydCB7IEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvblNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9hcHBsaWNhdGlvbi1jb25maWd1cmF0aW9uLnNlcnZpY2UnO1xuaW1wb3J0IHsgdGFwLCBzd2l0Y2hNYXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQgeyBTZXRMYW5ndWFnZSB9IGZyb20gJy4uL2FjdGlvbnMnO1xuaW1wb3J0IHsgU2Vzc2lvblN0YXRlIH0gZnJvbSAnLi9zZXNzaW9uLnN0YXRlJztcbmltcG9ydCB7IG9mIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBzZXRDaGlsZFJvdXRlLCBzb3J0Um91dGVzLCBvcmdhbml6ZVJvdXRlcyB9IGZyb20gJy4uL3V0aWxzL3JvdXRlLXV0aWxzJztcblxuQFN0YXRlPENvbmZpZy5TdGF0ZT4oe1xuICBuYW1lOiAnQ29uZmlnU3RhdGUnLFxuICBkZWZhdWx0czoge30gYXMgQ29uZmlnLlN0YXRlLFxufSlcbmV4cG9ydCBjbGFzcyBDb25maWdTdGF0ZSB7XG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRBbGwoc3RhdGU6IENvbmZpZy5TdGF0ZSkge1xuICAgIHJldHVybiBzdGF0ZTtcbiAgfVxuXG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRBcHBsaWNhdGlvbkluZm8oc3RhdGU6IENvbmZpZy5TdGF0ZSk6IENvbmZpZy5BcHBsaWNhdGlvbiB7XG4gICAgcmV0dXJuIHN0YXRlLmVudmlyb25tZW50LmFwcGxpY2F0aW9uIHx8ICh7fSBhcyBDb25maWcuQXBwbGljYXRpb24pO1xuICB9XG5cbiAgc3RhdGljIGdldE9uZShrZXk6IHN0cmluZykge1xuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoXG4gICAgICBbQ29uZmlnU3RhdGVdLFxuICAgICAgZnVuY3Rpb24oc3RhdGU6IENvbmZpZy5TdGF0ZSkge1xuICAgICAgICByZXR1cm4gc3RhdGVba2V5XTtcbiAgICAgIH0sXG4gICAgKTtcblxuICAgIHJldHVybiBzZWxlY3RvcjtcbiAgfVxuXG4gIHN0YXRpYyBnZXREZWVwKGtleXM6IHN0cmluZ1tdIHwgc3RyaW5nKSB7XG4gICAgaWYgKHR5cGVvZiBrZXlzID09PSAnc3RyaW5nJykge1xuICAgICAga2V5cyA9IGtleXMuc3BsaXQoJy4nKTtcbiAgICB9XG5cbiAgICBpZiAoIUFycmF5LmlzQXJyYXkoa2V5cykpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcignVGhlIGFyZ3VtZW50IG11c3QgYmUgYSBkb3Qgc3RyaW5nIG9yIGFuIHN0cmluZyBhcnJheS4nKTtcbiAgICB9XG5cbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxuICAgICAgW0NvbmZpZ1N0YXRlXSxcbiAgICAgIGZ1bmN0aW9uKHN0YXRlOiBDb25maWcuU3RhdGUpIHtcbiAgICAgICAgcmV0dXJuIChrZXlzIGFzIHN0cmluZ1tdKS5yZWR1Y2UoKGFjYywgdmFsKSA9PiB7XG4gICAgICAgICAgaWYgKGFjYykge1xuICAgICAgICAgICAgcmV0dXJuIGFjY1t2YWxdO1xuICAgICAgICAgIH1cblxuICAgICAgICAgIHJldHVybiB1bmRlZmluZWQ7XG4gICAgICAgIH0sIHN0YXRlKTtcbiAgICAgIH0sXG4gICAgKTtcblxuICAgIHJldHVybiBzZWxlY3RvcjtcbiAgfVxuXG4gIHN0YXRpYyBnZXRSb3V0ZShwYXRoPzogc3RyaW5nLCBuYW1lPzogc3RyaW5nKSB7XG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcbiAgICAgIFtDb25maWdTdGF0ZV0sXG4gICAgICBmdW5jdGlvbihzdGF0ZTogQ29uZmlnLlN0YXRlKSB7XG4gICAgICAgIGNvbnN0IHsgZmxhdHRlZFJvdXRlcyB9ID0gc3RhdGU7XG4gICAgICAgIHJldHVybiAoZmxhdHRlZFJvdXRlcyBhcyBBQlAuRnVsbFJvdXRlW10pLmZpbmQocm91dGUgPT4ge1xuICAgICAgICAgIGlmIChwYXRoICYmIHJvdXRlLnBhdGggPT09IHBhdGgpIHtcbiAgICAgICAgICAgIHJldHVybiByb3V0ZTtcbiAgICAgICAgICB9IGVsc2UgaWYgKG5hbWUgJiYgcm91dGUubmFtZSA9PT0gbmFtZSkge1xuICAgICAgICAgICAgcmV0dXJuIHJvdXRlO1xuICAgICAgICAgIH1cbiAgICAgICAgfSk7XG4gICAgICB9LFxuICAgICk7XG5cbiAgICByZXR1cm4gc2VsZWN0b3I7XG4gIH1cblxuICBzdGF0aWMgZ2V0QXBpVXJsKGtleT86IHN0cmluZykge1xuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoXG4gICAgICBbQ29uZmlnU3RhdGVdLFxuICAgICAgZnVuY3Rpb24oc3RhdGU6IENvbmZpZy5TdGF0ZSk6IHN0cmluZyB7XG4gICAgICAgIHJldHVybiBzdGF0ZS5lbnZpcm9ubWVudC5hcGlzW2tleSB8fCAnZGVmYXVsdCddLnVybDtcbiAgICAgIH0sXG4gICAgKTtcblxuICAgIHJldHVybiBzZWxlY3RvcjtcbiAgfVxuXG4gIHN0YXRpYyBnZXRTZXR0aW5nKGtleTogc3RyaW5nKSB7XG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcbiAgICAgIFtDb25maWdTdGF0ZV0sXG4gICAgICBmdW5jdGlvbihzdGF0ZTogQ29uZmlnLlN0YXRlKSB7XG4gICAgICAgIHJldHVybiBzbnEoKCkgPT4gc3RhdGUuc2V0dGluZy52YWx1ZXNba2V5XSk7XG4gICAgICB9LFxuICAgICk7XG5cbiAgICByZXR1cm4gc2VsZWN0b3I7XG4gIH1cblxuICBzdGF0aWMgZ2V0R3JhbnRlZFBvbGljeShrZXk6IHN0cmluZykge1xuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoXG4gICAgICBbQ29uZmlnU3RhdGVdLFxuICAgICAgZnVuY3Rpb24oc3RhdGU6IENvbmZpZy5TdGF0ZSk6IGJvb2xlYW4ge1xuICAgICAgICBpZiAoIWtleSkgcmV0dXJuIHRydWU7XG4gICAgICAgIHJldHVybiBzbnEoKCkgPT4gc3RhdGUuYXV0aC5ncmFudGVkUG9saWNpZXNba2V5XSwgZmFsc2UpO1xuICAgICAgfSxcbiAgICApO1xuXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xuICB9XG5cbiAgc3RhdGljIGdldENvcHkoa2V5OiBzdHJpbmcsIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXSkge1xuICAgIGlmICgha2V5KSBrZXkgPSAnJztcblxuICAgIGNvbnN0IGtleXMgPSBrZXkuc3BsaXQoJzo6JykgYXMgc3RyaW5nW107XG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcbiAgICAgIFtDb25maWdTdGF0ZV0sXG4gICAgICBmdW5jdGlvbihzdGF0ZTogQ29uZmlnLlN0YXRlKSB7XG4gICAgICAgIGlmICghc3RhdGUubG9jYWxpemF0aW9uKSByZXR1cm4ga2V5O1xuXG4gICAgICAgIGNvbnN0IHsgZGVmYXVsdFJlc291cmNlTmFtZSB9ID0gc3RhdGUuZW52aXJvbm1lbnQubG9jYWxpemF0aW9uO1xuICAgICAgICBpZiAoa2V5c1swXSA9PT0gJycpIHtcbiAgICAgICAgICBpZiAoIWRlZmF1bHRSZXNvdXJjZU5hbWUpIHtcbiAgICAgICAgICAgIHRocm93IG5ldyBFcnJvcihcbiAgICAgICAgICAgICAgYFBsZWFzZSBjaGVjayB5b3VyIGVudmlyb25tZW50LiBNYXkgeW91IGZvcmdldCBzZXQgZGVmYXVsdFJlc291cmNlTmFtZT8gXG4gICAgICAgICAgICAgIEhlcmUgaXMgdGhlIGV4YW1wbGU6XG4gICAgICAgICAgICAgICB7IHByb2R1Y3Rpb246IGZhbHNlLFxuICAgICAgICAgICAgICAgICBsb2NhbGl6YXRpb246IHtcbiAgICAgICAgICAgICAgICAgICBkZWZhdWx0UmVzb3VyY2VOYW1lOiAnTXlQcm9qZWN0TmFtZSdcbiAgICAgICAgICAgICAgICAgIH1cbiAgICAgICAgICAgICAgIH1gLFxuICAgICAgICAgICAgKTtcbiAgICAgICAgICB9XG5cbiAgICAgICAgICBrZXlzWzBdID0gc25xKCgpID0+IGRlZmF1bHRSZXNvdXJjZU5hbWUpO1xuICAgICAgICB9XG5cbiAgICAgICAgbGV0IGNvcHkgPSAoa2V5cyBhcyBhbnkpLnJlZHVjZSgoYWNjLCB2YWwpID0+IHtcbiAgICAgICAgICBpZiAoYWNjKSB7XG4gICAgICAgICAgICByZXR1cm4gYWNjW3ZhbF07XG4gICAgICAgICAgfVxuXG4gICAgICAgICAgcmV0dXJuIHVuZGVmaW5lZDtcbiAgICAgICAgfSwgc3RhdGUubG9jYWxpemF0aW9uLnZhbHVlcyk7XG5cbiAgICAgICAgaW50ZXJwb2xhdGVQYXJhbXMgPSBpbnRlcnBvbGF0ZVBhcmFtcy5maWx0ZXIocGFyYW1zID0+IHBhcmFtcyAhPSBudWxsKTtcbiAgICAgICAgaWYgKGNvcHkgJiYgaW50ZXJwb2xhdGVQYXJhbXMgJiYgaW50ZXJwb2xhdGVQYXJhbXMubGVuZ3RoKSB7XG4gICAgICAgICAgaW50ZXJwb2xhdGVQYXJhbXMuZm9yRWFjaChwYXJhbSA9PiB7XG4gICAgICAgICAgICBjb3B5ID0gY29weS5yZXBsYWNlKC9bXFwnXFxcIl0/XFx7W1xcZF0rXFx9W1xcJ1xcXCJdPy8sIHBhcmFtKTtcbiAgICAgICAgICB9KTtcbiAgICAgICAgfVxuXG4gICAgICAgIHJldHVybiBjb3B5IHx8IGtleTtcbiAgICAgIH0sXG4gICAgKTtcblxuICAgIHJldHVybiBzZWxlY3RvcjtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgYXBwQ29uZmlndXJhdGlvblNlcnZpY2U6IEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvblNlcnZpY2UsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIEBBY3Rpb24oR2V0QXBwQ29uZmlndXJhdGlvbilcbiAgYWRkRGF0YSh7IHBhdGNoU3RhdGUsIGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxDb25maWcuU3RhdGU+KSB7XG4gICAgcmV0dXJuIHRoaXMuYXBwQ29uZmlndXJhdGlvblNlcnZpY2UuZ2V0Q29uZmlndXJhdGlvbigpLnBpcGUoXG4gICAgICB0YXAoY29uZmlndXJhdGlvbiA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICAuLi5jb25maWd1cmF0aW9uLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgICBzd2l0Y2hNYXAoY29uZmlndXJhdGlvbiA9PiB7XG4gICAgICAgIGxldCBkZWZhdWx0TGFuZzogc3RyaW5nID0gY29uZmlndXJhdGlvbi5zZXR0aW5nLnZhbHVlc1snQWJwLkxvY2FsaXphdGlvbi5EZWZhdWx0TGFuZ3VhZ2UnXTtcblxuICAgICAgICBpZiAoZGVmYXVsdExhbmcuaW5jbHVkZXMoJzsnKSkge1xuICAgICAgICAgIGRlZmF1bHRMYW5nID0gZGVmYXVsdExhbmcuc3BsaXQoJzsnKVswXTtcbiAgICAgICAgfVxuXG4gICAgICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFNlc3Npb25TdGF0ZS5nZXRMYW5ndWFnZSkgPyBvZihudWxsKSA6IGRpc3BhdGNoKG5ldyBTZXRMYW5ndWFnZShkZWZhdWx0TGFuZykpO1xuICAgICAgfSksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oUGF0Y2hSb3V0ZUJ5TmFtZSlcbiAgcGF0Y2hSb3V0ZSh7IHBhdGNoU3RhdGUsIGdldFN0YXRlIH06IFN0YXRlQ29udGV4dDxDb25maWcuU3RhdGU+LCB7IG5hbWUsIG5ld1ZhbHVlIH06IFBhdGNoUm91dGVCeU5hbWUpIHtcbiAgICBsZXQgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10gPSBnZXRTdGF0ZSgpLnJvdXRlcztcblxuICAgIGNvbnN0IGluZGV4ID0gcm91dGVzLmZpbmRJbmRleChyb3V0ZSA9PiByb3V0ZS5uYW1lID09PSBuYW1lKTtcblxuICAgIHJvdXRlcyA9IHBhdGNoUm91dGVEZWVwKHJvdXRlcywgbmFtZSwgbmV3VmFsdWUpO1xuXG4gICAgcmV0dXJuIHBhdGNoU3RhdGUoe1xuICAgICAgcm91dGVzLFxuICAgIH0pO1xuICB9XG59XG5cbmZ1bmN0aW9uIHBhdGNoUm91dGVEZWVwKFxuICByb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSxcbiAgbmFtZTogc3RyaW5nLFxuICBuZXdWYWx1ZTogUGFydGlhbDxBQlAuRnVsbFJvdXRlPixcbiAgcGFyZW50VXJsOiBzdHJpbmcgPSBudWxsLFxuKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgcm91dGVzID0gcm91dGVzLm1hcChyb3V0ZSA9PiB7XG4gICAgaWYgKHJvdXRlLm5hbWUgPT09IG5hbWUpIHtcbiAgICAgIGlmIChuZXdWYWx1ZS5wYXRoKSB7XG4gICAgICAgIG5ld1ZhbHVlLnVybCA9IGAke3BhcmVudFVybH0vJHtuZXdWYWx1ZS5wYXRofWA7XG4gICAgICB9XG5cbiAgICAgIGlmIChuZXdWYWx1ZS5jaGlsZHJlbiAmJiBuZXdWYWx1ZS5jaGlsZHJlbi5sZW5ndGgpIHtcbiAgICAgICAgbmV3VmFsdWUuY2hpbGRyZW4gPSBuZXdWYWx1ZS5jaGlsZHJlbi5tYXAoY2hpbGQgPT4gKHtcbiAgICAgICAgICAuLi5jaGlsZCxcbiAgICAgICAgICB1cmw6IGAke3BhcmVudFVybH0vJHtyb3V0ZS5wYXRofS8ke2NoaWxkLnBhdGh9YCxcbiAgICAgICAgfSkpO1xuICAgICAgfVxuXG4gICAgICByZXR1cm4geyAuLi5yb3V0ZSwgLi4ubmV3VmFsdWUgfTtcbiAgICB9IGVsc2UgaWYgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgcm91dGUuY2hpbGRyZW4gPSBwYXRjaFJvdXRlRGVlcChyb3V0ZS5jaGlsZHJlbiwgbmFtZSwgbmV3VmFsdWUsIChwYXJlbnRVcmwgfHwgJy8nKSArIHJvdXRlLnBhdGgpO1xuICAgIH1cblxuICAgIHJldHVybiByb3V0ZTtcbiAgfSk7XG5cbiAgaWYgKHBhcmVudFVybCkge1xuICAgIC8vIHJlY3Vyc2l2ZSBibG9ja1xuICAgIHJldHVybiByb3V0ZXM7XG4gIH1cblxuICByZXR1cm4gb3JnYW5pemVSb3V0ZXMocm91dGVzKTtcbn1cbiJdfQ==