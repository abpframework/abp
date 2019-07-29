import * as tslib_1 from "tslib";
var ConfigState_1;
/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { State, Selector, createSelector, Action, Store } from '@ngxs/store';
import { ConfigGetAppConfiguration, PatchRouteByName } from '../actions/config.actions';
import { ApplicationConfigurationService } from '../services/application-configuration.service';
import { tap, switchMap } from 'rxjs/operators';
import snq from 'snq';
import { SessionSetLanguage } from '../actions';
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
     * @param {?=} condition
     * @return {?}
     */
    static getGrantedPolicy(condition = '') {
        /** @type {?} */
        const keys = condition
            .replace(/\(|\)|\!|\s/g, '')
            .split(/\|\||&&/)
            .filter((/**
         * @param {?} key
         * @return {?}
         */
        key => key));
        /** @type {?} */
        const selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            if (!keys.length)
                return true;
            /** @type {?} */
            const getPolicy = (/**
             * @param {?} key
             * @return {?}
             */
            key => snq((/**
             * @return {?}
             */
            () => state.auth.grantedPolicies[key]), false));
            if (keys.length > 1) {
                keys.forEach((/**
                 * @param {?} key
                 * @return {?}
                 */
                key => {
                    /** @type {?} */
                    const value = getPolicy(key);
                    condition = condition.replace(key, value);
                }));
                // tslint:disable-next-line: no-eval
                return eval(`!!${condition}`);
            }
            return getPolicy(condition);
        }));
        return selector;
    }
    /**
     * @param {?} key
     * @param {...?} interpolateParams
     * @return {?}
     */
    static getCopy(key, ...interpolateParams) {
        /** @type {?} */
        const keys = (/** @type {?} */ (key.split('::')));
        /** @type {?} */
        const selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
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
            let copy = keys.reduce((/**
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
            if (copy && interpolateParams && interpolateParams.length) {
                interpolateParams.forEach((/**
                 * @param {?} param
                 * @param {?} index
                 * @return {?}
                 */
                (param, index) => {
                    copy = copy.replace(`'{${index}}'`, param);
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
        configuration => this.store.selectSnapshot(SessionState.getLanguage)
            ? of(null)
            : dispatch(new SessionSetLanguage(snq((/**
             * @return {?}
             */
            () => configuration.setting.values['Abp.Localization.DefaultLanguage'])))))));
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
    Action(ConfigGetAppConfiguration),
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9jb25maWcuc3RhdGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLEtBQUssRUFBRSxRQUFRLEVBQUUsY0FBYyxFQUFFLE1BQU0sRUFBZ0IsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBRTNGLE9BQU8sRUFBRSx5QkFBeUIsRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLDJCQUEyQixDQUFDO0FBQ3hGLE9BQU8sRUFBRSwrQkFBK0IsRUFBRSxNQUFNLCtDQUErQyxDQUFDO0FBQ2hHLE9BQU8sRUFBRSxHQUFHLEVBQUUsU0FBUyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDaEQsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxrQkFBa0IsRUFBRSxNQUFNLFlBQVksQ0FBQztBQUNoRCxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDL0MsT0FBTyxFQUFFLEVBQUUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUMxQixPQUFPLEVBQTZCLGNBQWMsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0lBTXBFLFdBQVcseUJBQVgsV0FBVzs7Ozs7SUF3SXRCLFlBQW9CLHVCQUF3RCxFQUFVLEtBQVk7UUFBOUUsNEJBQXVCLEdBQXZCLHVCQUF1QixDQUFpQztRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7OztJQXRJdEcsTUFBTSxDQUFDLE1BQU0sQ0FBQyxLQUFtQjtRQUMvQixPQUFPLEtBQUssQ0FBQztJQUNmLENBQUM7Ozs7O0lBRUQsTUFBTSxDQUFDLE1BQU0sQ0FBQyxHQUFXOztjQUNqQixRQUFRLEdBQUcsY0FBYyxDQUM3QixDQUFDLGFBQVcsQ0FBQzs7OztRQUNiLFVBQVMsS0FBbUI7WUFDMUIsT0FBTyxLQUFLLENBQUMsR0FBRyxDQUFDLENB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QUUsSUFBSSxFQUFFLFFBQVEsRUFBRSxDQUFDLFNBQVMsSUFBSSxHQUFHLENBQUMsR0FBRyxLQUFLLENBQUMsSUFBSSxDQUFDLENBQUM7U0FDbEc7UUFFRCxPQUFPLEtBQUssQ0FBQztJQUNmLENBQUMsRUFBQyxDQUFDO0lBRUgsSUFBSSxTQUFTLEVBQUU7UUFDYixrQkFBa0I7UUFDbEIsT0FBTyxNQUFNLENBQUM7S0FDZjtJQUVELE9BQU8sY0FBYyxDQUFDLE1BQU0sQ0FBQyxDQUFDO0FBQ2hDLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBTdGF0ZSwgU2VsZWN0b3IsIGNyZWF0ZVNlbGVjdG9yLCBBY3Rpb24sIFN0YXRlQ29udGV4dCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBDb25maWcsIEFCUCB9IGZyb20gJy4uL21vZGVscyc7XG5pbXBvcnQgeyBDb25maWdHZXRBcHBDb25maWd1cmF0aW9uLCBQYXRjaFJvdXRlQnlOYW1lIH0gZnJvbSAnLi4vYWN0aW9ucy9jb25maWcuYWN0aW9ucyc7XG5pbXBvcnQgeyBBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvYXBwbGljYXRpb24tY29uZmlndXJhdGlvbi5zZXJ2aWNlJztcbmltcG9ydCB7IHRhcCwgc3dpdGNoTWFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuaW1wb3J0IHsgU2Vzc2lvblNldExhbmd1YWdlIH0gZnJvbSAnLi4vYWN0aW9ucyc7XG5pbXBvcnQgeyBTZXNzaW9uU3RhdGUgfSBmcm9tICcuL3Nlc3Npb24uc3RhdGUnO1xuaW1wb3J0IHsgb2YgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IHNldENoaWxkUm91dGUsIHNvcnRSb3V0ZXMsIG9yZ2FuaXplUm91dGVzIH0gZnJvbSAnLi4vdXRpbHMvcm91dGUtdXRpbHMnO1xuXG5AU3RhdGU8Q29uZmlnLlN0YXRlPih7XG4gIG5hbWU6ICdDb25maWdTdGF0ZScsXG4gIGRlZmF1bHRzOiB7fSBhcyBDb25maWcuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIENvbmZpZ1N0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldEFsbChzdGF0ZTogQ29uZmlnLlN0YXRlKSB7XG4gICAgcmV0dXJuIHN0YXRlO1xuICB9XG5cbiAgc3RhdGljIGdldE9uZShrZXk6IHN0cmluZykge1xuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoXG4gICAgICBbQ29uZmlnU3RhdGVdLFxuICAgICAgZnVuY3Rpb24oc3RhdGU6IENvbmZpZy5TdGF0ZSkge1xuICAgICAgICByZXR1cm4gc3RhdGVba2V5XTtcbiAgICAgIH0sXG4gICAgKTtcblxuICAgIHJldHVybiBzZWxlY3RvcjtcbiAgfVxuXG4gIHN0YXRpYyBnZXREZWVwKGtleXM6IHN0cmluZ1tdIHwgc3RyaW5nKSB7XG4gICAgaWYgKHR5cGVvZiBrZXlzID09PSAnc3RyaW5nJykge1xuICAgICAga2V5cyA9IGtleXMuc3BsaXQoJy4nKTtcbiAgICB9XG5cbiAgICBpZiAoIUFycmF5LmlzQXJyYXkoa2V5cykpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcignVGhlIGFyZ3VtZW50IG11c3QgYmUgYSBkb3Qgc3RyaW5nIG9yIGFuIHN0cmluZyBhcnJheS4nKTtcbiAgICB9XG5cbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxuICAgICAgW0NvbmZpZ1N0YXRlXSxcbiAgICAgIGZ1bmN0aW9uKHN0YXRlOiBDb25maWcuU3RhdGUpIHtcbiAgICAgICAgcmV0dXJuIChrZXlzIGFzIHN0cmluZ1tdKS5yZWR1Y2UoKGFjYywgdmFsKSA9PiB7XG4gICAgICAgICAgaWYgKGFjYykge1xuICAgICAgICAgICAgcmV0dXJuIGFjY1t2YWxdO1xuICAgICAgICAgIH1cblxuICAgICAgICAgIHJldHVybiB1bmRlZmluZWQ7XG4gICAgICAgIH0sIHN0YXRlKTtcbiAgICAgIH0sXG4gICAgKTtcblxuICAgIHJldHVybiBzZWxlY3RvcjtcbiAgfVxuXG4gIHN0YXRpYyBnZXRBcGlVcmwoa2V5Pzogc3RyaW5nKSB7XG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcbiAgICAgIFtDb25maWdTdGF0ZV0sXG4gICAgICBmdW5jdGlvbihzdGF0ZTogQ29uZmlnLlN0YXRlKTogc3RyaW5nIHtcbiAgICAgICAgcmV0dXJuIHN0YXRlLmVudmlyb25tZW50LmFwaXNba2V5IHx8ICdkZWZhdWx0J10udXJsO1xuICAgICAgfSxcbiAgICApO1xuXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xuICB9XG5cbiAgc3RhdGljIGdldFNldHRpbmcoa2V5OiBzdHJpbmcpIHtcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxuICAgICAgW0NvbmZpZ1N0YXRlXSxcbiAgICAgIGZ1bmN0aW9uKHN0YXRlOiBDb25maWcuU3RhdGUpIHtcbiAgICAgICAgcmV0dXJuIHNucSgoKSA9PiBzdGF0ZS5zZXR0aW5nLnZhbHVlc1trZXldKTtcbiAgICAgIH0sXG4gICAgKTtcblxuICAgIHJldHVybiBzZWxlY3RvcjtcbiAgfVxuXG4gIHN0YXRpYyBnZXRHcmFudGVkUG9saWN5KGNvbmRpdGlvbjogc3RyaW5nID0gJycpIHtcbiAgICBjb25zdCBrZXlzID0gY29uZGl0aW9uXG4gICAgICAucmVwbGFjZSgvXFwofFxcKXxcXCF8XFxzL2csICcnKVxuICAgICAgLnNwbGl0KC9cXHxcXHx8JiYvKVxuICAgICAgLmZpbHRlcihrZXkgPT4ga2V5KTtcblxuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoXG4gICAgICBbQ29uZmlnU3RhdGVdLFxuICAgICAgZnVuY3Rpb24oc3RhdGU6IENvbmZpZy5TdGF0ZSk6IGJvb2xlYW4ge1xuICAgICAgICBpZiAoIWtleXMubGVuZ3RoKSByZXR1cm4gdHJ1ZTtcblxuICAgICAgICBjb25zdCBnZXRQb2xpY3kgPSBrZXkgPT4gc25xKCgpID0+IHN0YXRlLmF1dGguZ3JhbnRlZFBvbGljaWVzW2tleV0sIGZhbHNlKTtcbiAgICAgICAgaWYgKGtleXMubGVuZ3RoID4gMSkge1xuICAgICAgICAgIGtleXMuZm9yRWFjaChrZXkgPT4ge1xuICAgICAgICAgICAgY29uc3QgdmFsdWUgPSBnZXRQb2xpY3koa2V5KTtcbiAgICAgICAgICAgIGNvbmRpdGlvbiA9IGNvbmRpdGlvbi5yZXBsYWNlKGtleSwgdmFsdWUpO1xuICAgICAgICAgIH0pO1xuXG4gICAgICAgICAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby1ldmFsXG4gICAgICAgICAgcmV0dXJuIGV2YWwoYCEhJHtjb25kaXRpb259YCk7XG4gICAgICAgIH1cblxuICAgICAgICByZXR1cm4gZ2V0UG9saWN5KGNvbmRpdGlvbik7XG4gICAgICB9LFxuICAgICk7XG5cbiAgICByZXR1cm4gc2VsZWN0b3I7XG4gIH1cblxuICBzdGF0aWMgZ2V0Q29weShrZXk6IHN0cmluZywgLi4uaW50ZXJwb2xhdGVQYXJhbXM6IHN0cmluZ1tdKSB7XG4gICAgY29uc3Qga2V5cyA9IGtleS5zcGxpdCgnOjonKSBhcyBzdHJpbmdbXTtcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxuICAgICAgW0NvbmZpZ1N0YXRlXSxcbiAgICAgIGZ1bmN0aW9uKHN0YXRlOiBDb25maWcuU3RhdGUpIHtcbiAgICAgICAgY29uc3QgeyBkZWZhdWx0UmVzb3VyY2VOYW1lIH0gPSBzdGF0ZS5lbnZpcm9ubWVudC5sb2NhbGl6YXRpb247XG4gICAgICAgIGlmIChrZXlzWzBdID09PSAnJykge1xuICAgICAgICAgIGlmICghZGVmYXVsdFJlc291cmNlTmFtZSkge1xuICAgICAgICAgICAgdGhyb3cgbmV3IEVycm9yKFxuICAgICAgICAgICAgICBgUGxlYXNlIGNoZWNrIHlvdXIgZW52aXJvbm1lbnQuIE1heSB5b3UgZm9yZ2V0IHNldCBkZWZhdWx0UmVzb3VyY2VOYW1lPyBcbiAgICAgICAgICAgICAgSGVyZSBpcyB0aGUgZXhhbXBsZTpcbiAgICAgICAgICAgICAgIHsgcHJvZHVjdGlvbjogZmFsc2UsXG4gICAgICAgICAgICAgICAgIGxvY2FsaXphdGlvbjoge1xuICAgICAgICAgICAgICAgICAgIGRlZmF1bHRSZXNvdXJjZU5hbWU6ICdNeVByb2plY3ROYW1lJ1xuICAgICAgICAgICAgICAgICAgfVxuICAgICAgICAgICAgICAgfWAsXG4gICAgICAgICAgICApO1xuICAgICAgICAgIH1cblxuICAgICAgICAgIGtleXNbMF0gPSBzbnEoKCkgPT4gZGVmYXVsdFJlc291cmNlTmFtZSk7XG4gICAgICAgIH1cblxuICAgICAgICBsZXQgY29weSA9IGtleXMucmVkdWNlKChhY2MsIHZhbCkgPT4ge1xuICAgICAgICAgIGlmIChhY2MpIHtcbiAgICAgICAgICAgIHJldHVybiBhY2NbdmFsXTtcbiAgICAgICAgICB9XG5cbiAgICAgICAgICByZXR1cm4gdW5kZWZpbmVkO1xuICAgICAgICB9LCBzdGF0ZS5sb2NhbGl6YXRpb24udmFsdWVzKTtcblxuICAgICAgICBpZiAoY29weSAmJiBpbnRlcnBvbGF0ZVBhcmFtcyAmJiBpbnRlcnBvbGF0ZVBhcmFtcy5sZW5ndGgpIHtcbiAgICAgICAgICBpbnRlcnBvbGF0ZVBhcmFtcy5mb3JFYWNoKChwYXJhbSwgaW5kZXgpID0+IHtcbiAgICAgICAgICAgIGNvcHkgPSBjb3B5LnJlcGxhY2UoYCd7JHtpbmRleH19J2AsIHBhcmFtKTtcbiAgICAgICAgICB9KTtcbiAgICAgICAgfVxuXG4gICAgICAgIHJldHVybiBjb3B5IHx8IGtleTtcbiAgICAgIH0sXG4gICAgKTtcblxuICAgIHJldHVybiBzZWxlY3RvcjtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgYXBwQ29uZmlndXJhdGlvblNlcnZpY2U6IEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvblNlcnZpY2UsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIEBBY3Rpb24oQ29uZmlnR2V0QXBwQ29uZmlndXJhdGlvbilcbiAgYWRkRGF0YSh7IHBhdGNoU3RhdGUsIGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxDb25maWcuU3RhdGU+KSB7XG4gICAgcmV0dXJuIHRoaXMuYXBwQ29uZmlndXJhdGlvblNlcnZpY2UuZ2V0Q29uZmlndXJhdGlvbigpLnBpcGUoXG4gICAgICB0YXAoY29uZmlndXJhdGlvbiA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICAuLi5jb25maWd1cmF0aW9uLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgICBzd2l0Y2hNYXAoY29uZmlndXJhdGlvbiA9PlxuICAgICAgICB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFNlc3Npb25TdGF0ZS5nZXRMYW5ndWFnZSlcbiAgICAgICAgICA/IG9mKG51bGwpXG4gICAgICAgICAgOiBkaXNwYXRjaChcbiAgICAgICAgICAgICAgbmV3IFNlc3Npb25TZXRMYW5ndWFnZShzbnEoKCkgPT4gY29uZmlndXJhdGlvbi5zZXR0aW5nLnZhbHVlc1snQWJwLkxvY2FsaXphdGlvbi5EZWZhdWx0TGFuZ3VhZ2UnXSkpLFxuICAgICAgICAgICAgKSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oUGF0Y2hSb3V0ZUJ5TmFtZSlcbiAgcGF0Y2hSb3V0ZSh7IHBhdGNoU3RhdGUsIGdldFN0YXRlIH06IFN0YXRlQ29udGV4dDxDb25maWcuU3RhdGU+LCB7IG5hbWUsIG5ld1ZhbHVlIH06IFBhdGNoUm91dGVCeU5hbWUpIHtcbiAgICBsZXQgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10gPSBnZXRTdGF0ZSgpLnJvdXRlcztcblxuICAgIGNvbnN0IGluZGV4ID0gcm91dGVzLmZpbmRJbmRleChyb3V0ZSA9PiByb3V0ZS5uYW1lID09PSBuYW1lKTtcblxuICAgIHJvdXRlcyA9IHBhdGNoUm91dGVEZWVwKHJvdXRlcywgbmFtZSwgbmV3VmFsdWUpO1xuXG4gICAgcmV0dXJuIHBhdGNoU3RhdGUoe1xuICAgICAgcm91dGVzLFxuICAgIH0pO1xuICB9XG59XG5cbmZ1bmN0aW9uIHBhdGNoUm91dGVEZWVwKFxuICByb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSxcbiAgbmFtZTogc3RyaW5nLFxuICBuZXdWYWx1ZTogUGFydGlhbDxBQlAuRnVsbFJvdXRlPixcbiAgcGFyZW50VXJsOiBzdHJpbmcgPSBudWxsLFxuKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgcm91dGVzID0gcm91dGVzLm1hcChyb3V0ZSA9PiB7XG4gICAgaWYgKHJvdXRlLm5hbWUgPT09IG5hbWUpIHtcbiAgICAgIGlmIChuZXdWYWx1ZS5wYXRoKSB7XG4gICAgICAgIG5ld1ZhbHVlLnVybCA9IGAke3BhcmVudFVybH0vJHtuZXdWYWx1ZS5wYXRofWA7XG4gICAgICB9XG5cbiAgICAgIGlmIChuZXdWYWx1ZS5jaGlsZHJlbiAmJiBuZXdWYWx1ZS5jaGlsZHJlbi5sZW5ndGgpIHtcbiAgICAgICAgbmV3VmFsdWUuY2hpbGRyZW4gPSBuZXdWYWx1ZS5jaGlsZHJlbi5tYXAoY2hpbGQgPT4gKHtcbiAgICAgICAgICAuLi5jaGlsZCxcbiAgICAgICAgICB1cmw6IGAke3BhcmVudFVybH0vJHtyb3V0ZS5wYXRofS8ke2NoaWxkLnBhdGh9YCxcbiAgICAgICAgfSkpO1xuICAgICAgfVxuXG4gICAgICByZXR1cm4geyAuLi5yb3V0ZSwgLi4ubmV3VmFsdWUgfTtcbiAgICB9IGVsc2UgaWYgKHJvdXRlLmNoaWxkcmVuICYmIHJvdXRlLmNoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgcm91dGUuY2hpbGRyZW4gPSBwYXRjaFJvdXRlRGVlcChyb3V0ZS5jaGlsZHJlbiwgbmFtZSwgbmV3VmFsdWUsIChwYXJlbnRVcmwgfHwgJy8nKSArIHJvdXRlLnBhdGgpO1xuICAgIH1cblxuICAgIHJldHVybiByb3V0ZTtcbiAgfSk7XG5cbiAgaWYgKHBhcmVudFVybCkge1xuICAgIC8vIHJlY3Vyc2l2ZSBibG9ja1xuICAgIHJldHVybiByb3V0ZXM7XG4gIH1cblxuICByZXR1cm4gb3JnYW5pemVSb3V0ZXMocm91dGVzKTtcbn1cbiJdfQ==