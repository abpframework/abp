/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/config.state.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, createSelector, Selector, State, StateContext, Store, } from '@ngxs/store';
import { of } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import snq from 'snq';
import { GetAppConfiguration, PatchRouteByName, } from '../actions/config.actions';
import { SetLanguage } from '../actions/session.actions';
import { ApplicationConfigurationService } from '../services/application-configuration.service';
import { organizeRoutes } from '../utils/route-utils';
import { SessionState } from './session.state';
var ConfigState = /** @class */ (function () {
    function ConfigState(appConfigurationService, store) {
        this.appConfigurationService = appConfigurationService;
        this.store = store;
    }
    ConfigState_1 = ConfigState;
    /**
     * @param {?} state
     * @return {?}
     */
    ConfigState.getAll = /**
     * @param {?} state
     * @return {?}
     */
    function (state) {
        return state;
    };
    /**
     * @param {?} state
     * @return {?}
     */
    ConfigState.getApplicationInfo = /**
     * @param {?} state
     * @return {?}
     */
    function (state) {
        return state.environment.application || ((/** @type {?} */ ({})));
    };
    /**
     * @param {?} key
     * @return {?}
     */
    ConfigState.getOne = /**
     * @param {?} key
     * @return {?}
     */
    function (key) {
        /** @type {?} */
        var selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            return state[key];
        }));
        return selector;
    };
    /**
     * @param {?} keys
     * @return {?}
     */
    ConfigState.getDeep = /**
     * @param {?} keys
     * @return {?}
     */
    function (keys) {
        if (typeof keys === 'string') {
            keys = keys.split('.');
        }
        if (!Array.isArray(keys)) {
            throw new Error('The argument must be a dot string or an string array.');
        }
        /** @type {?} */
        var selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            return ((/** @type {?} */ (keys))).reduce((/**
             * @param {?} acc
             * @param {?} val
             * @return {?}
             */
            function (acc, val) {
                if (acc) {
                    return acc[val];
                }
                return undefined;
            }), state);
        }));
        return selector;
    };
    /**
     * @param {?=} path
     * @param {?=} name
     * @param {?=} url
     * @return {?}
     */
    ConfigState.getRoute = /**
     * @param {?=} path
     * @param {?=} name
     * @param {?=} url
     * @return {?}
     */
    function (path, name, url) {
        /** @type {?} */
        var selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            var flattedRoutes = state.flattedRoutes;
            return ((/** @type {?} */ (flattedRoutes))).find((/**
             * @param {?} route
             * @return {?}
             */
            function (route) {
                if (path && route.path === path) {
                    return route;
                }
                else if (name && route.name === name) {
                    return route;
                }
                else if (url && route.url === url) {
                    return route;
                }
            }));
        }));
        return selector;
    };
    /**
     * @param {?=} key
     * @return {?}
     */
    ConfigState.getApiUrl = /**
     * @param {?=} key
     * @return {?}
     */
    function (key) {
        /** @type {?} */
        var selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            return state.environment.apis[key || 'default'].url;
        }));
        return selector;
    };
    /**
     * @param {?} key
     * @return {?}
     */
    ConfigState.getSetting = /**
     * @param {?} key
     * @return {?}
     */
    function (key) {
        /** @type {?} */
        var selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            return snq((/**
             * @return {?}
             */
            function () { return state.setting.values[key]; }));
        }));
        return selector;
    };
    /**
     * @param {?=} keyword
     * @return {?}
     */
    ConfigState.getSettings = /**
     * @param {?=} keyword
     * @return {?}
     */
    function (keyword) {
        /** @type {?} */
        var selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            if (keyword) {
                /** @type {?} */
                var keys = snq((/**
                 * @return {?}
                 */
                function () {
                    return Object.keys(state.setting.values).filter((/**
                     * @param {?} key
                     * @return {?}
                     */
                    function (key) { return key.indexOf(keyword) > -1; }));
                }), []);
                if (keys.length) {
                    return keys.reduce((/**
                     * @param {?} acc
                     * @param {?} key
                     * @return {?}
                     */
                    function (acc, key) {
                        var _a;
                        return (tslib_1.__assign({}, acc, (_a = {}, _a[key] = state.setting.values[key], _a)));
                    }), {});
                }
            }
            return snq((/**
             * @return {?}
             */
            function () { return state.setting.values; }), {});
        }));
        return selector;
    };
    /**
     * @param {?} key
     * @return {?}
     */
    ConfigState.getGrantedPolicy = /**
     * @param {?} key
     * @return {?}
     */
    function (key) {
        /** @type {?} */
        var selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            if (!key)
                return true;
            /** @type {?} */
            var getPolicy = (/**
             * @param {?} k
             * @return {?}
             */
            function (k) { return snq((/**
             * @return {?}
             */
            function () { return state.auth.grantedPolicies[k]; }), false); });
            /** @type {?} */
            var orRegexp = /\|\|/g;
            /** @type {?} */
            var andRegexp = /&&/g;
            if (orRegexp.test(key)) {
                /** @type {?} */
                var keys = key.split('||').filter((/**
                 * @param {?} k
                 * @return {?}
                 */
                function (k) { return !!k; }));
                if (keys.length !== 2)
                    return false;
                return getPolicy(keys[0].trim()) || getPolicy(keys[1].trim());
            }
            else if (andRegexp.test(key)) {
                /** @type {?} */
                var keys = key.split('&&').filter((/**
                 * @param {?} k
                 * @return {?}
                 */
                function (k) { return !!k; }));
                if (keys.length !== 2)
                    return false;
                return getPolicy(keys[0].trim()) && getPolicy(keys[1].trim());
            }
            return getPolicy(key);
        }));
        return selector;
    };
    /**
     * @param {?} key
     * @param {...?} interpolateParams
     * @return {?}
     */
    ConfigState.getLocalization = /**
     * @param {?} key
     * @param {...?} interpolateParams
     * @return {?}
     */
    function (key) {
        var interpolateParams = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            interpolateParams[_i - 1] = arguments[_i];
        }
        /** @type {?} */
        var defaultValue;
        if (typeof key !== 'string') {
            defaultValue = key.defaultValue;
            key = key.key;
        }
        if (!key)
            key = '';
        /** @type {?} */
        var keys = (/** @type {?} */ (key.split('::')));
        /** @type {?} */
        var selector = createSelector([ConfigState_1], (/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            if (!state.localization)
                return defaultValue || key;
            var defaultResourceName = state.environment.localization.defaultResourceName;
            if (keys[0] === '') {
                if (!defaultResourceName) {
                    throw new Error("Please check your environment. May you forget set defaultResourceName?\n              Here is the example:\n               { production: false,\n                 localization: {\n                   defaultResourceName: 'MyProjectName'\n                  }\n               }");
                }
                keys[0] = snq((/**
                 * @return {?}
                 */
                function () { return defaultResourceName; }));
            }
            /** @type {?} */
            var localization = ((/** @type {?} */ (keys))).reduce((/**
             * @param {?} acc
             * @param {?} val
             * @return {?}
             */
            function (acc, val) {
                if (acc) {
                    return acc[val];
                }
                return undefined;
            }), state.localization.values);
            interpolateParams = interpolateParams.filter((/**
             * @param {?} params
             * @return {?}
             */
            function (params) { return params != null; }));
            if (localization && interpolateParams && interpolateParams.length) {
                interpolateParams.forEach((/**
                 * @param {?} param
                 * @return {?}
                 */
                function (param) {
                    localization = localization.replace(/[\'\"]?\{[\d]+\}[\'\"]?/, param);
                }));
            }
            if (typeof localization !== 'string')
                localization = '';
            return localization || defaultValue || key;
        }));
        return selector;
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    ConfigState.prototype.addData = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var _this = this;
        var patchState = _a.patchState, dispatch = _a.dispatch;
        return this.appConfigurationService.getConfiguration().pipe(tap((/**
         * @param {?} configuration
         * @return {?}
         */
        function (configuration) {
            return patchState(tslib_1.__assign({}, configuration));
        })), switchMap((/**
         * @param {?} configuration
         * @return {?}
         */
        function (configuration) {
            /** @type {?} */
            var defaultLang = configuration.setting.values['Abp.Localization.DefaultLanguage'];
            if (defaultLang.includes(';')) {
                defaultLang = defaultLang.split(';')[0];
            }
            return _this.store.selectSnapshot(SessionState.getLanguage)
                ? of(null)
                : dispatch(new SetLanguage(defaultLang));
        })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    ConfigState.prototype.patchRoute = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState, getState = _a.getState;
        var name = _b.name, newValue = _b.newValue;
        /** @type {?} */
        var routes = getState().routes;
        /** @type {?} */
        var index = routes.findIndex((/**
         * @param {?} route
         * @return {?}
         */
        function (route) { return route.name === name; }));
        routes = patchRouteDeep(routes, name, newValue);
        return patchState({
            routes: routes,
        });
    };
    var ConfigState_1;
    ConfigState.ctorParameters = function () { return [
        { type: ApplicationConfigurationService },
        { type: Store }
    ]; };
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
        tslib_1.__metadata("design:paramtypes", [ApplicationConfigurationService,
            Store])
    ], ConfigState);
    return ConfigState;
}());
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
function patchRouteDeep(routes, name, newValue, parentUrl) {
    if (parentUrl === void 0) { parentUrl = ''; }
    routes = routes.map((/**
     * @param {?} route
     * @return {?}
     */
    function (route) {
        if (route.name === name) {
            newValue.url = parentUrl + "/" + ((!newValue.path && newValue.path === ''
                ? route.path
                : newValue.path) || '');
            if (newValue.children && newValue.children.length) {
                newValue.children = newValue.children.map((/**
                 * @param {?} child
                 * @return {?}
                 */
                function (child) { return (tslib_1.__assign({}, child, { url: (newValue.url + "/" + child.path).replace('//', '/') })); }));
            }
            return tslib_1.__assign({}, route, newValue);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9jb25maWcuc3RhdGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUNMLE1BQU0sRUFDTixjQUFjLEVBQ2QsUUFBUSxFQUNSLEtBQUssRUFDTCxZQUFZLEVBQ1osS0FBSyxHQUNOLE1BQU0sYUFBYSxDQUFDO0FBQ3JCLE9BQU8sRUFBRSxFQUFFLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDMUIsT0FBTyxFQUFFLFNBQVMsRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNoRCxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFDdEIsT0FBTyxFQUNMLG1CQUFtQixFQUNuQixnQkFBZ0IsR0FDakIsTUFBTSwyQkFBMkIsQ0FBQztBQUNuQyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFHekQsT0FBTyxFQUFFLCtCQUErQixFQUFFLE1BQU0sK0NBQStDLENBQUM7QUFDaEcsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3RELE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQzs7SUFpTTdDLHFCQUNVLHVCQUF3RCxFQUN4RCxLQUFZO1FBRFosNEJBQXVCLEdBQXZCLHVCQUF1QixDQUFpQztRQUN4RCxVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQ25CLENBQUM7b0JBOUxPLFdBQVc7Ozs7O0lBRWYsa0JBQU07Ozs7SUFBYixVQUFjLEtBQW1CO1FBQy9CLE9BQU8sS0FBSyxDQUFDO0lBQ2YsQ0FBQzs7Ozs7SUFHTSw4QkFBa0I7Ozs7SUFBekIsVUFBMEIsS0FBbUI7UUFDM0MsT0FBTyxLQUFLLENBQUMsV0FBVyxDQUFDLFdBQVcsSUFBSSxDQUFDLG1CQUFBLEVBQUUsRUFBc0IsQ0FBQyxDQUFDO0lBQ3JFLENBQUM7Ozs7O0lBRU0sa0JB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cmVhdGVTZWxlY3RvcixcbiAgU2VsZWN0b3IsXG4gIFN0YXRlLFxuICBTdGF0ZUNvbnRleHQsXG4gIFN0b3JlLFxufSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBvZiB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgc3dpdGNoTWFwLCB0YXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQge1xuICBHZXRBcHBDb25maWd1cmF0aW9uLFxuICBQYXRjaFJvdXRlQnlOYW1lLFxufSBmcm9tICcuLi9hY3Rpb25zL2NvbmZpZy5hY3Rpb25zJztcbmltcG9ydCB7IFNldExhbmd1YWdlIH0gZnJvbSAnLi4vYWN0aW9ucy9zZXNzaW9uLmFjdGlvbnMnO1xuaW1wb3J0IHsgQUJQIH0gZnJvbSAnLi4vbW9kZWxzL2NvbW1vbic7XG5pbXBvcnQgeyBDb25maWcgfSBmcm9tICcuLi9tb2RlbHMvY29uZmlnJztcbmltcG9ydCB7IEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvblNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9hcHBsaWNhdGlvbi1jb25maWd1cmF0aW9uLnNlcnZpY2UnO1xuaW1wb3J0IHsgb3JnYW5pemVSb3V0ZXMgfSBmcm9tICcuLi91dGlscy9yb3V0ZS11dGlscyc7XG5pbXBvcnQgeyBTZXNzaW9uU3RhdGUgfSBmcm9tICcuL3Nlc3Npb24uc3RhdGUnO1xuXG5AU3RhdGU8Q29uZmlnLlN0YXRlPih7XG4gIG5hbWU6ICdDb25maWdTdGF0ZScsXG4gIGRlZmF1bHRzOiB7fSBhcyBDb25maWcuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIENvbmZpZ1N0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldEFsbChzdGF0ZTogQ29uZmlnLlN0YXRlKSB7XG4gICAgcmV0dXJuIHN0YXRlO1xuICB9XG5cbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldEFwcGxpY2F0aW9uSW5mbyhzdGF0ZTogQ29uZmlnLlN0YXRlKTogQ29uZmlnLkFwcGxpY2F0aW9uIHtcbiAgICByZXR1cm4gc3RhdGUuZW52aXJvbm1lbnQuYXBwbGljYXRpb24gfHwgKHt9IGFzIENvbmZpZy5BcHBsaWNhdGlvbik7XG4gIH1cblxuICBzdGF0aWMgZ2V0T25lKGtleTogc3RyaW5nKSB7XG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihbQ29uZmlnU3RhdGVdLCAoc3RhdGU6IENvbmZpZy5TdGF0ZSkgPT4ge1xuICAgICAgcmV0dXJuIHN0YXRlW2tleV07XG4gICAgfSk7XG5cbiAgICByZXR1cm4gc2VsZWN0b3I7XG4gIH1cblxuICBzdGF0aWMgZ2V0RGVlcChrZXlzOiBzdHJpbmdbXSB8IHN0cmluZykge1xuICAgIGlmICh0eXBlb2Yga2V5cyA9PT0gJ3N0cmluZycpIHtcbiAgICAgIGtleXMgPSBrZXlzLnNwbGl0KCcuJyk7XG4gICAgfVxuXG4gICAgaWYgKCFBcnJheS5pc0FycmF5KGtleXMpKSB7XG4gICAgICB0aHJvdyBuZXcgRXJyb3IoJ1RoZSBhcmd1bWVudCBtdXN0IGJlIGEgZG90IHN0cmluZyBvciBhbiBzdHJpbmcgYXJyYXkuJyk7XG4gICAgfVxuXG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihbQ29uZmlnU3RhdGVdLCAoc3RhdGU6IENvbmZpZy5TdGF0ZSkgPT4ge1xuICAgICAgcmV0dXJuIChrZXlzIGFzIHN0cmluZ1tdKS5yZWR1Y2UoKGFjYywgdmFsKSA9PiB7XG4gICAgICAgIGlmIChhY2MpIHtcbiAgICAgICAgICByZXR1cm4gYWNjW3ZhbF07XG4gICAgICAgIH1cblxuICAgICAgICByZXR1cm4gdW5kZWZpbmVkO1xuICAgICAgfSwgc3RhdGUpO1xuICAgIH0pO1xuXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xuICB9XG5cbiAgc3RhdGljIGdldFJvdXRlKHBhdGg/OiBzdHJpbmcsIG5hbWU/OiBzdHJpbmcsIHVybD86IHN0cmluZykge1xuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoW0NvbmZpZ1N0YXRlXSwgKHN0YXRlOiBDb25maWcuU3RhdGUpID0+IHtcbiAgICAgIGNvbnN0IHsgZmxhdHRlZFJvdXRlcyB9ID0gc3RhdGU7XG4gICAgICByZXR1cm4gKGZsYXR0ZWRSb3V0ZXMgYXMgQUJQLkZ1bGxSb3V0ZVtdKS5maW5kKHJvdXRlID0+IHtcbiAgICAgICAgaWYgKHBhdGggJiYgcm91dGUucGF0aCA9PT0gcGF0aCkge1xuICAgICAgICAgIHJldHVybiByb3V0ZTtcbiAgICAgICAgfSBlbHNlIGlmIChuYW1lICYmIHJvdXRlLm5hbWUgPT09IG5hbWUpIHtcbiAgICAgICAgICByZXR1cm4gcm91dGU7XG4gICAgICAgIH0gZWxzZSBpZiAodXJsICYmIHJvdXRlLnVybCA9PT0gdXJsKSB7XG4gICAgICAgICAgcmV0dXJuIHJvdXRlO1xuICAgICAgICB9XG4gICAgICB9KTtcbiAgICB9KTtcblxuICAgIHJldHVybiBzZWxlY3RvcjtcbiAgfVxuXG4gIHN0YXRpYyBnZXRBcGlVcmwoa2V5Pzogc3RyaW5nKSB7XG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcbiAgICAgIFtDb25maWdTdGF0ZV0sXG4gICAgICAoc3RhdGU6IENvbmZpZy5TdGF0ZSk6IHN0cmluZyA9PiB7XG4gICAgICAgIHJldHVybiBzdGF0ZS5lbnZpcm9ubWVudC5hcGlzW2tleSB8fCAnZGVmYXVsdCddLnVybDtcbiAgICAgIH0sXG4gICAgKTtcblxuICAgIHJldHVybiBzZWxlY3RvcjtcbiAgfVxuXG4gIHN0YXRpYyBnZXRTZXR0aW5nKGtleTogc3RyaW5nKSB7XG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihbQ29uZmlnU3RhdGVdLCAoc3RhdGU6IENvbmZpZy5TdGF0ZSkgPT4ge1xuICAgICAgcmV0dXJuIHNucSgoKSA9PiBzdGF0ZS5zZXR0aW5nLnZhbHVlc1trZXldKTtcbiAgICB9KTtcbiAgICByZXR1cm4gc2VsZWN0b3I7XG4gIH1cblxuICBzdGF0aWMgZ2V0U2V0dGluZ3Moa2V5d29yZD86IHN0cmluZykge1xuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoW0NvbmZpZ1N0YXRlXSwgKHN0YXRlOiBDb25maWcuU3RhdGUpID0+IHtcbiAgICAgIGlmIChrZXl3b3JkKSB7XG4gICAgICAgIGNvbnN0IGtleXMgPSBzbnEoXG4gICAgICAgICAgKCkgPT5cbiAgICAgICAgICAgIE9iamVjdC5rZXlzKHN0YXRlLnNldHRpbmcudmFsdWVzKS5maWx0ZXIoXG4gICAgICAgICAgICAgIGtleSA9PiBrZXkuaW5kZXhPZihrZXl3b3JkKSA+IC0xLFxuICAgICAgICAgICAgKSxcbiAgICAgICAgICBbXSxcbiAgICAgICAgKTtcblxuICAgICAgICBpZiAoa2V5cy5sZW5ndGgpIHtcbiAgICAgICAgICByZXR1cm4ga2V5cy5yZWR1Y2UoXG4gICAgICAgICAgICAoYWNjLCBrZXkpID0+ICh7IC4uLmFjYywgW2tleV06IHN0YXRlLnNldHRpbmcudmFsdWVzW2tleV0gfSksXG4gICAgICAgICAgICB7fSxcbiAgICAgICAgICApO1xuICAgICAgICB9XG4gICAgICB9XG5cbiAgICAgIHJldHVybiBzbnEoKCkgPT4gc3RhdGUuc2V0dGluZy52YWx1ZXMsIHt9KTtcbiAgICB9KTtcbiAgICByZXR1cm4gc2VsZWN0b3I7XG4gIH1cblxuICBzdGF0aWMgZ2V0R3JhbnRlZFBvbGljeShrZXk6IHN0cmluZykge1xuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoXG4gICAgICBbQ29uZmlnU3RhdGVdLFxuICAgICAgKHN0YXRlOiBDb25maWcuU3RhdGUpOiBib29sZWFuID0+IHtcbiAgICAgICAgaWYgKCFrZXkpIHJldHVybiB0cnVlO1xuICAgICAgICBjb25zdCBnZXRQb2xpY3kgPSBrID0+IHNucSgoKSA9PiBzdGF0ZS5hdXRoLmdyYW50ZWRQb2xpY2llc1trXSwgZmFsc2UpO1xuXG4gICAgICAgIGNvbnN0IG9yUmVnZXhwID0gL1xcfFxcfC9nO1xuICAgICAgICBjb25zdCBhbmRSZWdleHAgPSAvJiYvZztcblxuICAgICAgICBpZiAob3JSZWdleHAudGVzdChrZXkpKSB7XG4gICAgICAgICAgY29uc3Qga2V5cyA9IGtleS5zcGxpdCgnfHwnKS5maWx0ZXIoayA9PiAhIWspO1xuXG4gICAgICAgICAgaWYgKGtleXMubGVuZ3RoICE9PSAyKSByZXR1cm4gZmFsc2U7XG5cbiAgICAgICAgICByZXR1cm4gZ2V0UG9saWN5KGtleXNbMF0udHJpbSgpKSB8fCBnZXRQb2xpY3koa2V5c1sxXS50cmltKCkpO1xuICAgICAgICB9IGVsc2UgaWYgKGFuZFJlZ2V4cC50ZXN0KGtleSkpIHtcbiAgICAgICAgICBjb25zdCBrZXlzID0ga2V5LnNwbGl0KCcmJicpLmZpbHRlcihrID0+ICEhayk7XG5cbiAgICAgICAgICBpZiAoa2V5cy5sZW5ndGggIT09IDIpIHJldHVybiBmYWxzZTtcblxuICAgICAgICAgIHJldHVybiBnZXRQb2xpY3koa2V5c1swXS50cmltKCkpICYmIGdldFBvbGljeShrZXlzWzFdLnRyaW0oKSk7XG4gICAgICAgIH1cblxuICAgICAgICByZXR1cm4gZ2V0UG9saWN5KGtleSk7XG4gICAgICB9LFxuICAgICk7XG5cbiAgICByZXR1cm4gc2VsZWN0b3I7XG4gIH1cblxuICBzdGF0aWMgZ2V0TG9jYWxpemF0aW9uKFxuICAgIGtleTogc3RyaW5nIHwgQ29uZmlnLkxvY2FsaXphdGlvbldpdGhEZWZhdWx0LFxuICAgIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXVxuICApIHtcbiAgICBsZXQgZGVmYXVsdFZhbHVlOiBzdHJpbmc7XG5cbiAgICBpZiAodHlwZW9mIGtleSAhPT0gJ3N0cmluZycpIHtcbiAgICAgIGRlZmF1bHRWYWx1ZSA9IGtleS5kZWZhdWx0VmFsdWU7XG4gICAgICBrZXkgPSBrZXkua2V5O1xuICAgIH1cblxuICAgIGlmICgha2V5KSBrZXkgPSAnJztcblxuICAgIGNvbnN0IGtleXMgPSBrZXkuc3BsaXQoJzo6JykgYXMgc3RyaW5nW107XG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihbQ29uZmlnU3RhdGVdLCAoc3RhdGU6IENvbmZpZy5TdGF0ZSkgPT4ge1xuICAgICAgaWYgKCFzdGF0ZS5sb2NhbGl6YXRpb24pIHJldHVybiBkZWZhdWx0VmFsdWUgfHwga2V5O1xuXG4gICAgICBjb25zdCB7IGRlZmF1bHRSZXNvdXJjZU5hbWUgfSA9IHN0YXRlLmVudmlyb25tZW50LmxvY2FsaXphdGlvbjtcbiAgICAgIGlmIChrZXlzWzBdID09PSAnJykge1xuICAgICAgICBpZiAoIWRlZmF1bHRSZXNvdXJjZU5hbWUpIHtcbiAgICAgICAgICB0aHJvdyBuZXcgRXJyb3IoXG4gICAgICAgICAgICBgUGxlYXNlIGNoZWNrIHlvdXIgZW52aXJvbm1lbnQuIE1heSB5b3UgZm9yZ2V0IHNldCBkZWZhdWx0UmVzb3VyY2VOYW1lP1xuICAgICAgICAgICAgICBIZXJlIGlzIHRoZSBleGFtcGxlOlxuICAgICAgICAgICAgICAgeyBwcm9kdWN0aW9uOiBmYWxzZSxcbiAgICAgICAgICAgICAgICAgbG9jYWxpemF0aW9uOiB7XG4gICAgICAgICAgICAgICAgICAgZGVmYXVsdFJlc291cmNlTmFtZTogJ015UHJvamVjdE5hbWUnXG4gICAgICAgICAgICAgICAgICB9XG4gICAgICAgICAgICAgICB9YCxcbiAgICAgICAgICApO1xuICAgICAgICB9XG5cbiAgICAgICAga2V5c1swXSA9IHNucSgoKSA9PiBkZWZhdWx0UmVzb3VyY2VOYW1lKTtcbiAgICAgIH1cblxuICAgICAgbGV0IGxvY2FsaXphdGlvbiA9IChrZXlzIGFzIGFueSkucmVkdWNlKChhY2MsIHZhbCkgPT4ge1xuICAgICAgICBpZiAoYWNjKSB7XG4gICAgICAgICAgcmV0dXJuIGFjY1t2YWxdO1xuICAgICAgICB9XG5cbiAgICAgICAgcmV0dXJuIHVuZGVmaW5lZDtcbiAgICAgIH0sIHN0YXRlLmxvY2FsaXphdGlvbi52YWx1ZXMpO1xuXG4gICAgICBpbnRlcnBvbGF0ZVBhcmFtcyA9IGludGVycG9sYXRlUGFyYW1zLmZpbHRlcihwYXJhbXMgPT4gcGFyYW1zICE9IG51bGwpO1xuICAgICAgaWYgKGxvY2FsaXphdGlvbiAmJiBpbnRlcnBvbGF0ZVBhcmFtcyAmJiBpbnRlcnBvbGF0ZVBhcmFtcy5sZW5ndGgpIHtcbiAgICAgICAgaW50ZXJwb2xhdGVQYXJhbXMuZm9yRWFjaChwYXJhbSA9PiB7XG4gICAgICAgICAgbG9jYWxpemF0aW9uID0gbG9jYWxpemF0aW9uLnJlcGxhY2UoL1tcXCdcXFwiXT9cXHtbXFxkXStcXH1bXFwnXFxcIl0/LywgcGFyYW0pO1xuICAgICAgICB9KTtcbiAgICAgIH1cblxuICAgICAgaWYgKHR5cGVvZiBsb2NhbGl6YXRpb24gIT09ICdzdHJpbmcnKSBsb2NhbGl6YXRpb24gPSAnJztcbiAgICAgIHJldHVybiBsb2NhbGl6YXRpb24gfHwgZGVmYXVsdFZhbHVlIHx8IGtleTtcbiAgICB9KTtcblxuICAgIHJldHVybiBzZWxlY3RvcjtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKFxuICAgIHByaXZhdGUgYXBwQ29uZmlndXJhdGlvblNlcnZpY2U6IEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvblNlcnZpY2UsXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXG4gICkge31cblxuICBAQWN0aW9uKEdldEFwcENvbmZpZ3VyYXRpb24pXG4gIGFkZERhdGEoeyBwYXRjaFN0YXRlLCBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8Q29uZmlnLlN0YXRlPikge1xuICAgIHJldHVybiB0aGlzLmFwcENvbmZpZ3VyYXRpb25TZXJ2aWNlLmdldENvbmZpZ3VyYXRpb24oKS5waXBlKFxuICAgICAgdGFwKGNvbmZpZ3VyYXRpb24gPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgLi4uY29uZmlndXJhdGlvbixcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICAgc3dpdGNoTWFwKGNvbmZpZ3VyYXRpb24gPT4ge1xuICAgICAgICBsZXQgZGVmYXVsdExhbmc6IHN0cmluZyA9XG4gICAgICAgICAgY29uZmlndXJhdGlvbi5zZXR0aW5nLnZhbHVlc1snQWJwLkxvY2FsaXphdGlvbi5EZWZhdWx0TGFuZ3VhZ2UnXTtcblxuICAgICAgICBpZiAoZGVmYXVsdExhbmcuaW5jbHVkZXMoJzsnKSkge1xuICAgICAgICAgIGRlZmF1bHRMYW5nID0gZGVmYXVsdExhbmcuc3BsaXQoJzsnKVswXTtcbiAgICAgICAgfVxuXG4gICAgICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFNlc3Npb25TdGF0ZS5nZXRMYW5ndWFnZSlcbiAgICAgICAgICA/IG9mKG51bGwpXG4gICAgICAgICAgOiBkaXNwYXRjaChuZXcgU2V0TGFuZ3VhZ2UoZGVmYXVsdExhbmcpKTtcbiAgICAgIH0pLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKFBhdGNoUm91dGVCeU5hbWUpXG4gIHBhdGNoUm91dGUoXG4gICAgeyBwYXRjaFN0YXRlLCBnZXRTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8Q29uZmlnLlN0YXRlPixcbiAgICB7IG5hbWUsIG5ld1ZhbHVlIH06IFBhdGNoUm91dGVCeU5hbWUsXG4gICkge1xuICAgIGxldCByb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSA9IGdldFN0YXRlKCkucm91dGVzO1xuXG4gICAgY29uc3QgaW5kZXggPSByb3V0ZXMuZmluZEluZGV4KHJvdXRlID0+IHJvdXRlLm5hbWUgPT09IG5hbWUpO1xuXG4gICAgcm91dGVzID0gcGF0Y2hSb3V0ZURlZXAocm91dGVzLCBuYW1lLCBuZXdWYWx1ZSk7XG5cbiAgICByZXR1cm4gcGF0Y2hTdGF0ZSh7XG4gICAgICByb3V0ZXMsXG4gICAgfSk7XG4gIH1cbn1cblxuZnVuY3Rpb24gcGF0Y2hSb3V0ZURlZXAoXG4gIHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdLFxuICBuYW1lOiBzdHJpbmcsXG4gIG5ld1ZhbHVlOiBQYXJ0aWFsPEFCUC5GdWxsUm91dGU+LFxuICBwYXJlbnRVcmw6IHN0cmluZyA9ICcnLFxuKTogQUJQLkZ1bGxSb3V0ZVtdIHtcbiAgcm91dGVzID0gcm91dGVzLm1hcChyb3V0ZSA9PiB7XG4gICAgaWYgKHJvdXRlLm5hbWUgPT09IG5hbWUpIHtcbiAgICAgIG5ld1ZhbHVlLnVybCA9IGAke3BhcmVudFVybH0vJHsoIW5ld1ZhbHVlLnBhdGggJiYgbmV3VmFsdWUucGF0aCA9PT0gJydcbiAgICAgICAgPyByb3V0ZS5wYXRoXG4gICAgICAgIDogbmV3VmFsdWUucGF0aCkgfHwgJyd9YDtcblxuICAgICAgaWYgKG5ld1ZhbHVlLmNoaWxkcmVuICYmIG5ld1ZhbHVlLmNoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgICBuZXdWYWx1ZS5jaGlsZHJlbiA9IG5ld1ZhbHVlLmNoaWxkcmVuLm1hcChjaGlsZCA9PiAoe1xuICAgICAgICAgIC4uLmNoaWxkLFxuICAgICAgICAgIHVybDogYCR7bmV3VmFsdWUudXJsfS8ke2NoaWxkLnBhdGh9YC5yZXBsYWNlKCcvLycsICcvJyksXG4gICAgICAgIH0pKTtcbiAgICAgIH1cblxuICAgICAgcmV0dXJuIHsgLi4ucm91dGUsIC4uLm5ld1ZhbHVlIH07XG4gICAgfSBlbHNlIGlmIChyb3V0ZS5jaGlsZHJlbiAmJiByb3V0ZS5jaGlsZHJlbi5sZW5ndGgpIHtcbiAgICAgIHJvdXRlLmNoaWxkcmVuID0gcGF0Y2hSb3V0ZURlZXAoXG4gICAgICAgIHJvdXRlLmNoaWxkcmVuLFxuICAgICAgICBuYW1lLFxuICAgICAgICBuZXdWYWx1ZSxcbiAgICAgICAgKHBhcmVudFVybCB8fCAnLycpICsgcm91dGUucGF0aCxcbiAgICAgICk7XG4gICAgfVxuXG4gICAgcmV0dXJuIHJvdXRlO1xuICB9KTtcblxuICBpZiAocGFyZW50VXJsKSB7XG4gICAgLy8gcmVjdXJzaXZlIGJsb2NrXG4gICAgcmV0dXJuIHJvdXRlcztcbiAgfVxuXG4gIHJldHVybiBvcmdhbml6ZVJvdXRlcyhyb3V0ZXMpO1xufVxuIl19