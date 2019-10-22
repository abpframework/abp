var ConfigState_1;
/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9jb25maWcuc3RhdGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLE1BQU0sRUFBRSxjQUFjLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBRSxZQUFZLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzNGLE9BQU8sRUFBRSxFQUFFLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDMUIsT0FBTyxFQUFFLFNBQVMsRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNoRCxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFDdEIsT0FBTyxFQUFFLG1CQUFtQixFQUFFLGdCQUFnQixFQUFFLE1BQU0sMkJBQTJCLENBQUM7QUFDbEYsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBR3pELE9BQU8sRUFBRSwrQkFBK0IsRUFBRSxNQUFNLCtDQUErQyxDQUFDO0FBQ2hHLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0saUJBQWlCLENBQUM7SUFNbEMsV0FBVyx5QkFBWCxXQUFXOzs7OztJQTRLdEIsWUFBb0IsdUJBQXdELEVBQVUsS0FBWTtRQUE5RSw0QkFBdUIsR0FBdkIsdUJBQXVCLENBQWlDO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7O0lBMUt0RyxNQUFNLENBQUMsTUFBTSxDQUFDLEtBQW1CO1FBQy9CLE9BQU8sS0FBSyxDQUFDO0lBQ2YsQ0FBQzs7Ozs7SUFHRCxNQUFNLENBQUMsa0JBQWtCLENBQUMsS0FBbUI7UUFDM0MsT0FBTyxLQUFLLENBQUMsV0FBVyxDQUFDLFdBQVcsSUFBSSxDQUFDLG1CQUFBLEVBQUUsRUFBc0IsQ0FBQyxDQUFDO0lBQ3JFLENBQUM7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QUFDLEdBQUcsR0FBRyxHQUFHLFNBQVMsSUFBSSxDQUFDLENBQUMsUUFBUSxDQUFDLElBQUksSUFBSSxRQUFRLENBQUMsSUFBSSxLQUFLLEVBQUUsQ0FBQyxDQUFDLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsUUFBUSxDQUFDLElBQUksQ0FBQyxJQUFJLEVBQUUsRUFBRSxDQUFDO1lBRTdHLElBQUksUUFBUSxDQUFDLFFBQVEsSUFBSSxRQUFRLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtnQkFDakQsUUFBUSxDQUFDLFFBQVEsR0FBRyxRQUFRLENBQUMsUUFBUSxDQUFDLEdBQUc7Ozs7Z0JBQUMsS0FBSyxDQUFDLEVBQUUsQ0FBQyxtQkFDOUMsS0FBSyxJQUNSLEdBQUcsRUFBRSxHQUFHLFFBQVEsQ0FBQyxHQUFHLElBQUksS0FBSyxDQUFDLElBQUksRUFBRSxDQUFDLE9BQU8sQ0FBQyxJQUFJLEVBQUUsR0FBRyxDQUFDLElBQ3ZELEVBQUMsQ0FBQzthQUNMO1lBRUQseUJBQVksS0FBSyxFQUFLLFFBQVEsRUFBRztTQUNsQzthQUFNLElBQUksS0FBSyxDQUFDLFFBQVEsSUFBSSxLQUFLLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUNsRCxLQUFLLENBQUMsUUFBUSxHQUFHLGNBQWMsQ0FBQyxLQUFLLENBQUMsUUFBUSxFQUFFLElBQUksRUFBRSxRQUFRLEVBQUUsQ0FBQyxTQUFTLElBQUksR0FBRyxDQUFDLEdBQUcsS0FBSyxDQUFDLElBQUksQ0FBQyxDQUFDO1NBQ2xHO1FBRUQsT0FBTyxLQUFLLENBQUM7SUFDZixDQUFDLEVBQUMsQ0FBQztJQUVILElBQUksU0FBUyxFQUFFO1FBQ2Isa0JBQWtCO1FBQ2xCLE9BQU8sTUFBTSxDQUFDO0tBQ2Y7SUFFRCxPQUFPLGNBQWMsQ0FBQyxNQUFNLENBQUMsQ0FBQztBQUNoQyxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBjcmVhdGVTZWxlY3RvciwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBvZiB9IGZyb20gJ3J4anMnO1xyXG5pbXBvcnQgeyBzd2l0Y2hNYXAsIHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcclxuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xyXG5pbXBvcnQgeyBHZXRBcHBDb25maWd1cmF0aW9uLCBQYXRjaFJvdXRlQnlOYW1lIH0gZnJvbSAnLi4vYWN0aW9ucy9jb25maWcuYWN0aW9ucyc7XHJcbmltcG9ydCB7IFNldExhbmd1YWdlIH0gZnJvbSAnLi4vYWN0aW9ucy9zZXNzaW9uLmFjdGlvbnMnO1xyXG5pbXBvcnQgeyBBQlAgfSBmcm9tICcuLi9tb2RlbHMvY29tbW9uJztcclxuaW1wb3J0IHsgQ29uZmlnIH0gZnJvbSAnLi4vbW9kZWxzL2NvbmZpZyc7XHJcbmltcG9ydCB7IEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvblNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9hcHBsaWNhdGlvbi1jb25maWd1cmF0aW9uLnNlcnZpY2UnO1xyXG5pbXBvcnQgeyBvcmdhbml6ZVJvdXRlcyB9IGZyb20gJy4uL3V0aWxzL3JvdXRlLXV0aWxzJztcclxuaW1wb3J0IHsgU2Vzc2lvblN0YXRlIH0gZnJvbSAnLi9zZXNzaW9uLnN0YXRlJztcclxuXHJcbkBTdGF0ZTxDb25maWcuU3RhdGU+KHtcclxuICBuYW1lOiAnQ29uZmlnU3RhdGUnLFxyXG4gIGRlZmF1bHRzOiB7fSBhcyBDb25maWcuU3RhdGUsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBDb25maWdTdGF0ZSB7XHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0QWxsKHN0YXRlOiBDb25maWcuU3RhdGUpIHtcclxuICAgIHJldHVybiBzdGF0ZTtcclxuICB9XHJcblxyXG4gIEBTZWxlY3RvcigpXHJcbiAgc3RhdGljIGdldEFwcGxpY2F0aW9uSW5mbyhzdGF0ZTogQ29uZmlnLlN0YXRlKTogQ29uZmlnLkFwcGxpY2F0aW9uIHtcclxuICAgIHJldHVybiBzdGF0ZS5lbnZpcm9ubWVudC5hcHBsaWNhdGlvbiB8fCAoe30gYXMgQ29uZmlnLkFwcGxpY2F0aW9uKTtcclxuICB9XHJcblxyXG4gIHN0YXRpYyBnZXRPbmUoa2V5OiBzdHJpbmcpIHtcclxuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoXHJcbiAgICAgIFtDb25maWdTdGF0ZV0sXHJcbiAgICAgIChzdGF0ZTogQ29uZmlnLlN0YXRlKSA9PiB7XHJcbiAgICAgICAgcmV0dXJuIHN0YXRlW2tleV07XHJcbiAgICAgIH0sXHJcbiAgICApO1xyXG5cclxuICAgIHJldHVybiBzZWxlY3RvcjtcclxuICB9XHJcblxyXG4gIHN0YXRpYyBnZXREZWVwKGtleXM6IHN0cmluZ1tdIHwgc3RyaW5nKSB7XHJcbiAgICBpZiAodHlwZW9mIGtleXMgPT09ICdzdHJpbmcnKSB7XHJcbiAgICAgIGtleXMgPSBrZXlzLnNwbGl0KCcuJyk7XHJcbiAgICB9XHJcblxyXG4gICAgaWYgKCFBcnJheS5pc0FycmF5KGtleXMpKSB7XHJcbiAgICAgIHRocm93IG5ldyBFcnJvcignVGhlIGFyZ3VtZW50IG11c3QgYmUgYSBkb3Qgc3RyaW5nIG9yIGFuIHN0cmluZyBhcnJheS4nKTtcclxuICAgIH1cclxuXHJcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxyXG4gICAgICBbQ29uZmlnU3RhdGVdLFxyXG4gICAgICAoc3RhdGU6IENvbmZpZy5TdGF0ZSkgPT4ge1xyXG4gICAgICAgIHJldHVybiAoa2V5cyBhcyBzdHJpbmdbXSkucmVkdWNlKChhY2MsIHZhbCkgPT4ge1xyXG4gICAgICAgICAgaWYgKGFjYykge1xyXG4gICAgICAgICAgICByZXR1cm4gYWNjW3ZhbF07XHJcbiAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgcmV0dXJuIHVuZGVmaW5lZDtcclxuICAgICAgICB9LCBzdGF0ZSk7XHJcbiAgICAgIH0sXHJcbiAgICApO1xyXG5cclxuICAgIHJldHVybiBzZWxlY3RvcjtcclxuICB9XHJcblxyXG4gIHN0YXRpYyBnZXRSb3V0ZShwYXRoPzogc3RyaW5nLCBuYW1lPzogc3RyaW5nKSB7XHJcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxyXG4gICAgICBbQ29uZmlnU3RhdGVdLFxyXG4gICAgICAoc3RhdGU6IENvbmZpZy5TdGF0ZSkgPT4ge1xyXG4gICAgICAgIGNvbnN0IHsgZmxhdHRlZFJvdXRlcyB9ID0gc3RhdGU7XHJcbiAgICAgICAgcmV0dXJuIChmbGF0dGVkUm91dGVzIGFzIEFCUC5GdWxsUm91dGVbXSkuZmluZChyb3V0ZSA9PiB7XHJcbiAgICAgICAgICBpZiAocGF0aCAmJiByb3V0ZS5wYXRoID09PSBwYXRoKSB7XHJcbiAgICAgICAgICAgIHJldHVybiByb3V0ZTtcclxuICAgICAgICAgIH0gZWxzZSBpZiAobmFtZSAmJiByb3V0ZS5uYW1lID09PSBuYW1lKSB7XHJcbiAgICAgICAgICAgIHJldHVybiByb3V0ZTtcclxuICAgICAgICAgIH1cclxuICAgICAgICB9KTtcclxuICAgICAgfSxcclxuICAgICk7XHJcblxyXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xyXG4gIH1cclxuXHJcbiAgc3RhdGljIGdldEFwaVVybChrZXk/OiBzdHJpbmcpIHtcclxuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoXHJcbiAgICAgIFtDb25maWdTdGF0ZV0sXHJcbiAgICAgIChzdGF0ZTogQ29uZmlnLlN0YXRlKTogc3RyaW5nID0+IHtcclxuICAgICAgICByZXR1cm4gc3RhdGUuZW52aXJvbm1lbnQuYXBpc1trZXkgfHwgJ2RlZmF1bHQnXS51cmw7XHJcbiAgICAgIH0sXHJcbiAgICApO1xyXG5cclxuICAgIHJldHVybiBzZWxlY3RvcjtcclxuICB9XHJcblxyXG4gIHN0YXRpYyBnZXRTZXR0aW5nKGtleTogc3RyaW5nKSB7XHJcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxyXG4gICAgICBbQ29uZmlnU3RhdGVdLFxyXG4gICAgICAoc3RhdGU6IENvbmZpZy5TdGF0ZSkgPT4ge1xyXG4gICAgICAgIHJldHVybiBzbnEoKCkgPT4gc3RhdGUuc2V0dGluZy52YWx1ZXNba2V5XSk7XHJcbiAgICAgIH0sXHJcbiAgICApO1xyXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xyXG4gIH1cclxuXHJcbiAgc3RhdGljIGdldFNldHRpbmdzKGtleXdvcmQ/OiBzdHJpbmcpIHtcclxuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoXHJcbiAgICAgIFtDb25maWdTdGF0ZV0sXHJcbiAgICAgIChzdGF0ZTogQ29uZmlnLlN0YXRlKSA9PiB7XHJcbiAgICAgICAgaWYgKGtleXdvcmQpIHtcclxuICAgICAgICAgIGNvbnN0IGtleXMgPSBzbnEoKCkgPT4gT2JqZWN0LmtleXMoc3RhdGUuc2V0dGluZy52YWx1ZXMpLmZpbHRlcihrZXkgPT4ga2V5LmluZGV4T2Yoa2V5d29yZCkgPiAtMSksIFtdKTtcclxuXHJcbiAgICAgICAgICBpZiAoa2V5cy5sZW5ndGgpIHtcclxuICAgICAgICAgICAgcmV0dXJuIGtleXMucmVkdWNlKChhY2MsIGtleSkgPT4gKHsgLi4uYWNjLCBba2V5XTogc3RhdGUuc2V0dGluZy52YWx1ZXNba2V5XSB9KSwge30pO1xyXG4gICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgcmV0dXJuIHNucSgoKSA9PiBzdGF0ZS5zZXR0aW5nLnZhbHVlcywge30pO1xyXG4gICAgICB9LFxyXG4gICAgKTtcclxuICAgIHJldHVybiBzZWxlY3RvcjtcclxuICB9XHJcblxyXG4gIHN0YXRpYyBnZXRHcmFudGVkUG9saWN5KGtleTogc3RyaW5nKSB7XHJcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxyXG4gICAgICBbQ29uZmlnU3RhdGVdLFxyXG4gICAgICAoc3RhdGU6IENvbmZpZy5TdGF0ZSk6IGJvb2xlYW4gPT4ge1xyXG4gICAgICAgIGlmICgha2V5KSByZXR1cm4gdHJ1ZTtcclxuICAgICAgICByZXR1cm4gc25xKCgpID0+IHN0YXRlLmF1dGguZ3JhbnRlZFBvbGljaWVzW2tleV0sIGZhbHNlKTtcclxuICAgICAgfSxcclxuICAgICk7XHJcblxyXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xyXG4gIH1cclxuXHJcbiAgc3RhdGljIGdldExvY2FsaXphdGlvbihrZXk6IHN0cmluZyB8IENvbmZpZy5Mb2NhbGl6YXRpb25XaXRoRGVmYXVsdCwgLi4uaW50ZXJwb2xhdGVQYXJhbXM6IHN0cmluZ1tdKSB7XHJcbiAgICBsZXQgZGVmYXVsdFZhbHVlOiBzdHJpbmc7XHJcblxyXG4gICAgaWYgKHR5cGVvZiBrZXkgIT09ICdzdHJpbmcnKSB7XHJcbiAgICAgIGRlZmF1bHRWYWx1ZSA9IGtleS5kZWZhdWx0VmFsdWU7XHJcbiAgICAgIGtleSA9IGtleS5rZXk7XHJcbiAgICB9XHJcblxyXG4gICAgaWYgKCFrZXkpIGtleSA9ICcnO1xyXG5cclxuICAgIGNvbnN0IGtleXMgPSBrZXkuc3BsaXQoJzo6JykgYXMgc3RyaW5nW107XHJcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxyXG4gICAgICBbQ29uZmlnU3RhdGVdLFxyXG4gICAgICAoc3RhdGU6IENvbmZpZy5TdGF0ZSkgPT4ge1xyXG4gICAgICAgIGlmICghc3RhdGUubG9jYWxpemF0aW9uKSByZXR1cm4gZGVmYXVsdFZhbHVlIHx8IGtleTtcclxuXHJcbiAgICAgICAgY29uc3QgeyBkZWZhdWx0UmVzb3VyY2VOYW1lIH0gPSBzdGF0ZS5lbnZpcm9ubWVudC5sb2NhbGl6YXRpb247XHJcbiAgICAgICAgaWYgKGtleXNbMF0gPT09ICcnKSB7XHJcbiAgICAgICAgICBpZiAoIWRlZmF1bHRSZXNvdXJjZU5hbWUpIHtcclxuICAgICAgICAgICAgdGhyb3cgbmV3IEVycm9yKFxyXG4gICAgICAgICAgICAgIGBQbGVhc2UgY2hlY2sgeW91ciBlbnZpcm9ubWVudC4gTWF5IHlvdSBmb3JnZXQgc2V0IGRlZmF1bHRSZXNvdXJjZU5hbWU/XHJcbiAgICAgICAgICAgICAgSGVyZSBpcyB0aGUgZXhhbXBsZTpcclxuICAgICAgICAgICAgICAgeyBwcm9kdWN0aW9uOiBmYWxzZSxcclxuICAgICAgICAgICAgICAgICBsb2NhbGl6YXRpb246IHtcclxuICAgICAgICAgICAgICAgICAgIGRlZmF1bHRSZXNvdXJjZU5hbWU6ICdNeVByb2plY3ROYW1lJ1xyXG4gICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgIH1gLFxyXG4gICAgICAgICAgICApO1xyXG4gICAgICAgICAgfVxyXG5cclxuICAgICAgICAgIGtleXNbMF0gPSBzbnEoKCkgPT4gZGVmYXVsdFJlc291cmNlTmFtZSk7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICBsZXQgbG9jYWxpemF0aW9uID0gKGtleXMgYXMgYW55KS5yZWR1Y2UoKGFjYywgdmFsKSA9PiB7XHJcbiAgICAgICAgICBpZiAoYWNjKSB7XHJcbiAgICAgICAgICAgIHJldHVybiBhY2NbdmFsXTtcclxuICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICByZXR1cm4gdW5kZWZpbmVkO1xyXG4gICAgICAgIH0sIHN0YXRlLmxvY2FsaXphdGlvbi52YWx1ZXMpO1xyXG5cclxuICAgICAgICBpbnRlcnBvbGF0ZVBhcmFtcyA9IGludGVycG9sYXRlUGFyYW1zLmZpbHRlcihwYXJhbXMgPT4gcGFyYW1zICE9IG51bGwpO1xyXG4gICAgICAgIGlmIChsb2NhbGl6YXRpb24gJiYgaW50ZXJwb2xhdGVQYXJhbXMgJiYgaW50ZXJwb2xhdGVQYXJhbXMubGVuZ3RoKSB7XHJcbiAgICAgICAgICBpbnRlcnBvbGF0ZVBhcmFtcy5mb3JFYWNoKHBhcmFtID0+IHtcclxuICAgICAgICAgICAgbG9jYWxpemF0aW9uID0gbG9jYWxpemF0aW9uLnJlcGxhY2UoL1tcXCdcXFwiXT9cXHtbXFxkXStcXH1bXFwnXFxcIl0/LywgcGFyYW0pO1xyXG4gICAgICAgICAgfSk7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICBpZiAodHlwZW9mIGxvY2FsaXphdGlvbiAhPT0gJ3N0cmluZycpIGxvY2FsaXphdGlvbiA9ICcnO1xyXG4gICAgICAgIHJldHVybiBsb2NhbGl6YXRpb24gfHwgZGVmYXVsdFZhbHVlIHx8IGtleTtcclxuICAgICAgfSxcclxuICAgICk7XHJcblxyXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xyXG4gIH1cclxuXHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBhcHBDb25maWd1cmF0aW9uU2VydmljZTogQXBwbGljYXRpb25Db25maWd1cmF0aW9uU2VydmljZSwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XHJcblxyXG4gIEBBY3Rpb24oR2V0QXBwQ29uZmlndXJhdGlvbilcclxuICBhZGREYXRhKHsgcGF0Y2hTdGF0ZSwgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PENvbmZpZy5TdGF0ZT4pIHtcclxuICAgIHJldHVybiB0aGlzLmFwcENvbmZpZ3VyYXRpb25TZXJ2aWNlLmdldENvbmZpZ3VyYXRpb24oKS5waXBlKFxyXG4gICAgICB0YXAoY29uZmlndXJhdGlvbiA9PlxyXG4gICAgICAgIHBhdGNoU3RhdGUoe1xyXG4gICAgICAgICAgLi4uY29uZmlndXJhdGlvbixcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICAgc3dpdGNoTWFwKGNvbmZpZ3VyYXRpb24gPT4ge1xyXG4gICAgICAgIGxldCBkZWZhdWx0TGFuZzogc3RyaW5nID0gY29uZmlndXJhdGlvbi5zZXR0aW5nLnZhbHVlc1snQWJwLkxvY2FsaXphdGlvbi5EZWZhdWx0TGFuZ3VhZ2UnXTtcclxuXHJcbiAgICAgICAgaWYgKGRlZmF1bHRMYW5nLmluY2x1ZGVzKCc7JykpIHtcclxuICAgICAgICAgIGRlZmF1bHRMYW5nID0gZGVmYXVsdExhbmcuc3BsaXQoJzsnKVswXTtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFNlc3Npb25TdGF0ZS5nZXRMYW5ndWFnZSkgPyBvZihudWxsKSA6IGRpc3BhdGNoKG5ldyBTZXRMYW5ndWFnZShkZWZhdWx0TGFuZykpO1xyXG4gICAgICB9KSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKFBhdGNoUm91dGVCeU5hbWUpXHJcbiAgcGF0Y2hSb3V0ZSh7IHBhdGNoU3RhdGUsIGdldFN0YXRlIH06IFN0YXRlQ29udGV4dDxDb25maWcuU3RhdGU+LCB7IG5hbWUsIG5ld1ZhbHVlIH06IFBhdGNoUm91dGVCeU5hbWUpIHtcclxuICAgIGxldCByb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSA9IGdldFN0YXRlKCkucm91dGVzO1xyXG5cclxuICAgIGNvbnN0IGluZGV4ID0gcm91dGVzLmZpbmRJbmRleChyb3V0ZSA9PiByb3V0ZS5uYW1lID09PSBuYW1lKTtcclxuXHJcbiAgICByb3V0ZXMgPSBwYXRjaFJvdXRlRGVlcChyb3V0ZXMsIG5hbWUsIG5ld1ZhbHVlKTtcclxuXHJcbiAgICByZXR1cm4gcGF0Y2hTdGF0ZSh7XHJcbiAgICAgIHJvdXRlcyxcclxuICAgIH0pO1xyXG4gIH1cclxufVxyXG5cclxuZnVuY3Rpb24gcGF0Y2hSb3V0ZURlZXAoXHJcbiAgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10sXHJcbiAgbmFtZTogc3RyaW5nLFxyXG4gIG5ld1ZhbHVlOiBQYXJ0aWFsPEFCUC5GdWxsUm91dGU+LFxyXG4gIHBhcmVudFVybDogc3RyaW5nID0gJycsXHJcbik6IEFCUC5GdWxsUm91dGVbXSB7XHJcbiAgcm91dGVzID0gcm91dGVzLm1hcChyb3V0ZSA9PiB7XHJcbiAgICBpZiAocm91dGUubmFtZSA9PT0gbmFtZSkge1xyXG4gICAgICBuZXdWYWx1ZS51cmwgPSBgJHtwYXJlbnRVcmx9LyR7KCFuZXdWYWx1ZS5wYXRoICYmIG5ld1ZhbHVlLnBhdGggPT09ICcnID8gcm91dGUucGF0aCA6IG5ld1ZhbHVlLnBhdGgpIHx8ICcnfWA7XHJcblxyXG4gICAgICBpZiAobmV3VmFsdWUuY2hpbGRyZW4gJiYgbmV3VmFsdWUuY2hpbGRyZW4ubGVuZ3RoKSB7XHJcbiAgICAgICAgbmV3VmFsdWUuY2hpbGRyZW4gPSBuZXdWYWx1ZS5jaGlsZHJlbi5tYXAoY2hpbGQgPT4gKHtcclxuICAgICAgICAgIC4uLmNoaWxkLFxyXG4gICAgICAgICAgdXJsOiBgJHtuZXdWYWx1ZS51cmx9LyR7Y2hpbGQucGF0aH1gLnJlcGxhY2UoJy8vJywgJy8nKSxcclxuICAgICAgICB9KSk7XHJcbiAgICAgIH1cclxuXHJcbiAgICAgIHJldHVybiB7IC4uLnJvdXRlLCAuLi5uZXdWYWx1ZSB9O1xyXG4gICAgfSBlbHNlIGlmIChyb3V0ZS5jaGlsZHJlbiAmJiByb3V0ZS5jaGlsZHJlbi5sZW5ndGgpIHtcclxuICAgICAgcm91dGUuY2hpbGRyZW4gPSBwYXRjaFJvdXRlRGVlcChyb3V0ZS5jaGlsZHJlbiwgbmFtZSwgbmV3VmFsdWUsIChwYXJlbnRVcmwgfHwgJy8nKSArIHJvdXRlLnBhdGgpO1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiByb3V0ZTtcclxuICB9KTtcclxuXHJcbiAgaWYgKHBhcmVudFVybCkge1xyXG4gICAgLy8gcmVjdXJzaXZlIGJsb2NrXHJcbiAgICByZXR1cm4gcm91dGVzO1xyXG4gIH1cclxuXHJcbiAgcmV0dXJuIG9yZ2FuaXplUm91dGVzKHJvdXRlcyk7XHJcbn1cclxuIl19