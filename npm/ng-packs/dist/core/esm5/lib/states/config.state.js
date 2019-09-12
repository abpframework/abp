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
     * @return {?}
     */
    ConfigState.getRoute = /**
     * @param {?=} path
     * @param {?=} name
     * @return {?}
     */
    function (path, name) {
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
            return snq((/**
             * @return {?}
             */
            function () { return state.auth.grantedPolicies[key]; }), false);
        }));
        return selector;
    };
    /**
     * @param {?} key
     * @param {...?} interpolateParams
     * @return {?}
     */
    ConfigState.getCopy = /**
     * @param {?} key
     * @param {...?} interpolateParams
     * @return {?}
     */
    function (key) {
        var interpolateParams = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            interpolateParams[_i - 1] = arguments[_i];
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
                return key;
            var defaultResourceName = state.environment.localization.defaultResourceName;
            if (keys[0] === '') {
                if (!defaultResourceName) {
                    throw new Error("Please check your environment. May you forget set defaultResourceName? \n              Here is the example:\n               { production: false,\n                 localization: {\n                   defaultResourceName: 'MyProjectName'\n                  }\n               }");
                }
                keys[0] = snq((/**
                 * @return {?}
                 */
                function () { return defaultResourceName; }));
            }
            /** @type {?} */
            var copy = ((/** @type {?} */ (keys))).reduce((/**
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
            if (copy && interpolateParams && interpolateParams.length) {
                interpolateParams.forEach((/**
                 * @param {?} param
                 * @return {?}
                 */
                function (param) {
                    copy = copy.replace(/[\'\"]?\{[\d]+\}[\'\"]?/, param);
                }));
            }
            return copy || key;
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
            return _this.store.selectSnapshot(SessionState.getLanguage) ? of(null) : dispatch(new SetLanguage(defaultLang));
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
    if (parentUrl === void 0) { parentUrl = null; }
    routes = routes.map((/**
     * @param {?} route
     * @return {?}
     */
    function (route) {
        if (route.name === name) {
            if (newValue.path) {
                newValue.url = parentUrl + "/" + newValue.path;
            }
            if (newValue.children && newValue.children.length) {
                newValue.children = newValue.children.map((/**
                 * @param {?} child
                 * @return {?}
                 */
                function (child) { return (tslib_1.__assign({}, child, { url: parentUrl + "/" + route.path + "/" + child.path })); }));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9jb25maWcuc3RhdGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsS0FBSyxFQUFFLFFBQVEsRUFBRSxjQUFjLEVBQUUsTUFBTSxFQUFnQixLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFFM0YsT0FBTyxFQUFFLG1CQUFtQixFQUFFLGdCQUFnQixFQUFFLE1BQU0sMkJBQTJCLENBQUM7QUFDbEYsT0FBTyxFQUFFLCtCQUErQixFQUFFLE1BQU0sK0NBQStDLENBQUM7QUFDaEcsT0FBTyxFQUFFLEdBQUcsRUFBRSxTQUFTLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNoRCxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFDdEIsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLFlBQVksQ0FBQztBQUN6QyxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDL0MsT0FBTyxFQUFFLEVBQUUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUMxQixPQUFPLEVBQTZCLGNBQWMsRUFBRSxNQUFNLHNCQUFzQixDQUFDOztJQXlKL0UscUJBQW9CLHVCQUF3RCxFQUFVLEtBQVk7UUFBOUUsNEJBQXVCLEdBQXZCLHVCQUF1QixDQUFpQztRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDO29CQW5KM0YsV0FBVzs7Ozs7SUFFZixrQkFBTTs7OztJQUFiLFVBQWMsS0FBbUI7UUFDL0IsT0FBTyxLQUFLLENBQUM7SUFDZixDQUFDOzs7OztJQUdNLDhCQUFrQjs7OztJQUF6QixVQUEwQixLQUFtQjtRQUMzQyxPQUFPLEtBQUssQ0FBQyxXQUFXLENBQUMsV0FBVyxJQUFJLENBQUMsbUJBQUEsRUFBRSxFQUFzQixDQUFDLENBQUM7SUFDckUsQ0FBQzs7Ozs7SUFFTSxrQkFBTTs7OztJQUFiLFVB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O2dCQUNqQixRQUFRLENBQUMsR0FBRyxHQUFNLFNBQVMsU0FBSSxRQUFRLENBQUMsSUFBTSxDQUFDO2FBQ2hEO1lBRUQsSUFBSSxRQUFRLENBQUMsUUFBUSxJQUFJLFFBQVEsQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO2dCQUNqRCxRQUFRLENBQUMsUUFBUSxHQUFHLFFBQVEsQ0FBQyxRQUFRLENBQUMsR0FBRzs7OztnQkFBQyxVQUFBLEtBQUssSUFBSSxPQUFBLHNCQUM5QyxLQUFLLElBQ1IsR0FBRyxFQUFLLFNBQVMsU0FBSSxLQUFLLENBQUMsSUFBSSxTQUFJLEtBQUssQ0FBQyxJQUFNLElBQy9DLEVBSGlELENBR2pELEVBQUMsQ0FBQzthQUNMO1lBRUQsNEJBQVksS0FBSyxFQUFLLFFBQVEsRUFBRztTQUNsQzthQUFNLElBQUksS0FBSyxDQUFDLFFBQVEsSUFBSSxLQUFLLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUNsRCxLQUFLLENBQUMsUUFBUSxHQUFHLGNBQWMsQ0FBQyxLQUFLLENBQUMsUUFBUSxFQUFFLElBQUksRUFBRSxRQUFRLEVBQUUsQ0FBQyxTQUFTLElBQUksR0FBRyxDQUFDLEdBQUcsS0FBSyxDQUFDLElBQUksQ0FBQyxDQUFDO1NBQ2xHO1FBRUQsT0FBTyxLQUFLLENBQUM7SUFDZixDQUFDLEVBQUMsQ0FBQztJQUVILElBQUksU0FBUyxFQUFFO1FBQ2Isa0JBQWtCO1FBQ2xCLE9BQU8sTUFBTSxDQUFDO0tBQ2Y7SUFFRCxPQUFPLGNBQWMsQ0FBQyxNQUFNLENBQUMsQ0FBQztBQUNoQyxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhdGUsIFNlbGVjdG9yLCBjcmVhdGVTZWxlY3RvciwgQWN0aW9uLCBTdGF0ZUNvbnRleHQsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgQ29uZmlnLCBBQlAgfSBmcm9tICcuLi9tb2RlbHMnO1xuaW1wb3J0IHsgR2V0QXBwQ29uZmlndXJhdGlvbiwgUGF0Y2hSb3V0ZUJ5TmFtZSB9IGZyb20gJy4uL2FjdGlvbnMvY29uZmlnLmFjdGlvbnMnO1xuaW1wb3J0IHsgQXBwbGljYXRpb25Db25maWd1cmF0aW9uU2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL2FwcGxpY2F0aW9uLWNvbmZpZ3VyYXRpb24uc2VydmljZSc7XG5pbXBvcnQgeyB0YXAsIHN3aXRjaE1hcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcbmltcG9ydCB7IFNldExhbmd1YWdlIH0gZnJvbSAnLi4vYWN0aW9ucyc7XG5pbXBvcnQgeyBTZXNzaW9uU3RhdGUgfSBmcm9tICcuL3Nlc3Npb24uc3RhdGUnO1xuaW1wb3J0IHsgb2YgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IHNldENoaWxkUm91dGUsIHNvcnRSb3V0ZXMsIG9yZ2FuaXplUm91dGVzIH0gZnJvbSAnLi4vdXRpbHMvcm91dGUtdXRpbHMnO1xuXG5AU3RhdGU8Q29uZmlnLlN0YXRlPih7XG4gIG5hbWU6ICdDb25maWdTdGF0ZScsXG4gIGRlZmF1bHRzOiB7fSBhcyBDb25maWcuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIENvbmZpZ1N0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldEFsbChzdGF0ZTogQ29uZmlnLlN0YXRlKSB7XG4gICAgcmV0dXJuIHN0YXRlO1xuICB9XG5cbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldEFwcGxpY2F0aW9uSW5mbyhzdGF0ZTogQ29uZmlnLlN0YXRlKTogQ29uZmlnLkFwcGxpY2F0aW9uIHtcbiAgICByZXR1cm4gc3RhdGUuZW52aXJvbm1lbnQuYXBwbGljYXRpb24gfHwgKHt9IGFzIENvbmZpZy5BcHBsaWNhdGlvbik7XG4gIH1cblxuICBzdGF0aWMgZ2V0T25lKGtleTogc3RyaW5nKSB7XG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcbiAgICAgIFtDb25maWdTdGF0ZV0sXG4gICAgICBmdW5jdGlvbihzdGF0ZTogQ29uZmlnLlN0YXRlKSB7XG4gICAgICAgIHJldHVybiBzdGF0ZVtrZXldO1xuICAgICAgfSxcbiAgICApO1xuXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xuICB9XG5cbiAgc3RhdGljIGdldERlZXAoa2V5czogc3RyaW5nW10gfCBzdHJpbmcpIHtcbiAgICBpZiAodHlwZW9mIGtleXMgPT09ICdzdHJpbmcnKSB7XG4gICAgICBrZXlzID0ga2V5cy5zcGxpdCgnLicpO1xuICAgIH1cblxuICAgIGlmICghQXJyYXkuaXNBcnJheShrZXlzKSkge1xuICAgICAgdGhyb3cgbmV3IEVycm9yKCdUaGUgYXJndW1lbnQgbXVzdCBiZSBhIGRvdCBzdHJpbmcgb3IgYW4gc3RyaW5nIGFycmF5LicpO1xuICAgIH1cblxuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoXG4gICAgICBbQ29uZmlnU3RhdGVdLFxuICAgICAgZnVuY3Rpb24oc3RhdGU6IENvbmZpZy5TdGF0ZSkge1xuICAgICAgICByZXR1cm4gKGtleXMgYXMgc3RyaW5nW10pLnJlZHVjZSgoYWNjLCB2YWwpID0+IHtcbiAgICAgICAgICBpZiAoYWNjKSB7XG4gICAgICAgICAgICByZXR1cm4gYWNjW3ZhbF07XG4gICAgICAgICAgfVxuXG4gICAgICAgICAgcmV0dXJuIHVuZGVmaW5lZDtcbiAgICAgICAgfSwgc3RhdGUpO1xuICAgICAgfSxcbiAgICApO1xuXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xuICB9XG5cbiAgc3RhdGljIGdldFJvdXRlKHBhdGg/OiBzdHJpbmcsIG5hbWU/OiBzdHJpbmcpIHtcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxuICAgICAgW0NvbmZpZ1N0YXRlXSxcbiAgICAgIGZ1bmN0aW9uKHN0YXRlOiBDb25maWcuU3RhdGUpIHtcbiAgICAgICAgY29uc3QgeyBmbGF0dGVkUm91dGVzIH0gPSBzdGF0ZTtcbiAgICAgICAgcmV0dXJuIChmbGF0dGVkUm91dGVzIGFzIEFCUC5GdWxsUm91dGVbXSkuZmluZChyb3V0ZSA9PiB7XG4gICAgICAgICAgaWYgKHBhdGggJiYgcm91dGUucGF0aCA9PT0gcGF0aCkge1xuICAgICAgICAgICAgcmV0dXJuIHJvdXRlO1xuICAgICAgICAgIH0gZWxzZSBpZiAobmFtZSAmJiByb3V0ZS5uYW1lID09PSBuYW1lKSB7XG4gICAgICAgICAgICByZXR1cm4gcm91dGU7XG4gICAgICAgICAgfVxuICAgICAgICB9KTtcbiAgICAgIH0sXG4gICAgKTtcblxuICAgIHJldHVybiBzZWxlY3RvcjtcbiAgfVxuXG4gIHN0YXRpYyBnZXRBcGlVcmwoa2V5Pzogc3RyaW5nKSB7XG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcbiAgICAgIFtDb25maWdTdGF0ZV0sXG4gICAgICBmdW5jdGlvbihzdGF0ZTogQ29uZmlnLlN0YXRlKTogc3RyaW5nIHtcbiAgICAgICAgcmV0dXJuIHN0YXRlLmVudmlyb25tZW50LmFwaXNba2V5IHx8ICdkZWZhdWx0J10udXJsO1xuICAgICAgfSxcbiAgICApO1xuXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xuICB9XG5cbiAgc3RhdGljIGdldFNldHRpbmcoa2V5OiBzdHJpbmcpIHtcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxuICAgICAgW0NvbmZpZ1N0YXRlXSxcbiAgICAgIGZ1bmN0aW9uKHN0YXRlOiBDb25maWcuU3RhdGUpIHtcbiAgICAgICAgcmV0dXJuIHNucSgoKSA9PiBzdGF0ZS5zZXR0aW5nLnZhbHVlc1trZXldKTtcbiAgICAgIH0sXG4gICAgKTtcblxuICAgIHJldHVybiBzZWxlY3RvcjtcbiAgfVxuXG4gIHN0YXRpYyBnZXRHcmFudGVkUG9saWN5KGtleTogc3RyaW5nKSB7XG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcbiAgICAgIFtDb25maWdTdGF0ZV0sXG4gICAgICBmdW5jdGlvbihzdGF0ZTogQ29uZmlnLlN0YXRlKTogYm9vbGVhbiB7XG4gICAgICAgIGlmICgha2V5KSByZXR1cm4gdHJ1ZTtcbiAgICAgICAgcmV0dXJuIHNucSgoKSA9PiBzdGF0ZS5hdXRoLmdyYW50ZWRQb2xpY2llc1trZXldLCBmYWxzZSk7XG4gICAgICB9LFxuICAgICk7XG5cbiAgICByZXR1cm4gc2VsZWN0b3I7XG4gIH1cblxuICBzdGF0aWMgZ2V0Q29weShrZXk6IHN0cmluZywgLi4uaW50ZXJwb2xhdGVQYXJhbXM6IHN0cmluZ1tdKSB7XG4gICAgaWYgKCFrZXkpIGtleSA9ICcnO1xuXG4gICAgY29uc3Qga2V5cyA9IGtleS5zcGxpdCgnOjonKSBhcyBzdHJpbmdbXTtcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxuICAgICAgW0NvbmZpZ1N0YXRlXSxcbiAgICAgIGZ1bmN0aW9uKHN0YXRlOiBDb25maWcuU3RhdGUpIHtcbiAgICAgICAgaWYgKCFzdGF0ZS5sb2NhbGl6YXRpb24pIHJldHVybiBrZXk7XG5cbiAgICAgICAgY29uc3QgeyBkZWZhdWx0UmVzb3VyY2VOYW1lIH0gPSBzdGF0ZS5lbnZpcm9ubWVudC5sb2NhbGl6YXRpb247XG4gICAgICAgIGlmIChrZXlzWzBdID09PSAnJykge1xuICAgICAgICAgIGlmICghZGVmYXVsdFJlc291cmNlTmFtZSkge1xuICAgICAgICAgICAgdGhyb3cgbmV3IEVycm9yKFxuICAgICAgICAgICAgICBgUGxlYXNlIGNoZWNrIHlvdXIgZW52aXJvbm1lbnQuIE1heSB5b3UgZm9yZ2V0IHNldCBkZWZhdWx0UmVzb3VyY2VOYW1lPyBcbiAgICAgICAgICAgICAgSGVyZSBpcyB0aGUgZXhhbXBsZTpcbiAgICAgICAgICAgICAgIHsgcHJvZHVjdGlvbjogZmFsc2UsXG4gICAgICAgICAgICAgICAgIGxvY2FsaXphdGlvbjoge1xuICAgICAgICAgICAgICAgICAgIGRlZmF1bHRSZXNvdXJjZU5hbWU6ICdNeVByb2plY3ROYW1lJ1xuICAgICAgICAgICAgICAgICAgfVxuICAgICAgICAgICAgICAgfWAsXG4gICAgICAgICAgICApO1xuICAgICAgICAgIH1cblxuICAgICAgICAgIGtleXNbMF0gPSBzbnEoKCkgPT4gZGVmYXVsdFJlc291cmNlTmFtZSk7XG4gICAgICAgIH1cblxuICAgICAgICBsZXQgY29weSA9IChrZXlzIGFzIGFueSkucmVkdWNlKChhY2MsIHZhbCkgPT4ge1xuICAgICAgICAgIGlmIChhY2MpIHtcbiAgICAgICAgICAgIHJldHVybiBhY2NbdmFsXTtcbiAgICAgICAgICB9XG5cbiAgICAgICAgICByZXR1cm4gdW5kZWZpbmVkO1xuICAgICAgICB9LCBzdGF0ZS5sb2NhbGl6YXRpb24udmFsdWVzKTtcblxuICAgICAgICBpbnRlcnBvbGF0ZVBhcmFtcyA9IGludGVycG9sYXRlUGFyYW1zLmZpbHRlcihwYXJhbXMgPT4gcGFyYW1zICE9IG51bGwpO1xuICAgICAgICBpZiAoY29weSAmJiBpbnRlcnBvbGF0ZVBhcmFtcyAmJiBpbnRlcnBvbGF0ZVBhcmFtcy5sZW5ndGgpIHtcbiAgICAgICAgICBpbnRlcnBvbGF0ZVBhcmFtcy5mb3JFYWNoKHBhcmFtID0+IHtcbiAgICAgICAgICAgIGNvcHkgPSBjb3B5LnJlcGxhY2UoL1tcXCdcXFwiXT9cXHtbXFxkXStcXH1bXFwnXFxcIl0/LywgcGFyYW0pO1xuICAgICAgICAgIH0pO1xuICAgICAgICB9XG5cbiAgICAgICAgcmV0dXJuIGNvcHkgfHwga2V5O1xuICAgICAgfSxcbiAgICApO1xuXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBhcHBDb25maWd1cmF0aW9uU2VydmljZTogQXBwbGljYXRpb25Db25maWd1cmF0aW9uU2VydmljZSwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgQEFjdGlvbihHZXRBcHBDb25maWd1cmF0aW9uKVxuICBhZGREYXRhKHsgcGF0Y2hTdGF0ZSwgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PENvbmZpZy5TdGF0ZT4pIHtcbiAgICByZXR1cm4gdGhpcy5hcHBDb25maWd1cmF0aW9uU2VydmljZS5nZXRDb25maWd1cmF0aW9uKCkucGlwZShcbiAgICAgIHRhcChjb25maWd1cmF0aW9uID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIC4uLmNvbmZpZ3VyYXRpb24sXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICAgIHN3aXRjaE1hcChjb25maWd1cmF0aW9uID0+IHtcbiAgICAgICAgbGV0IGRlZmF1bHRMYW5nOiBzdHJpbmcgPSBjb25maWd1cmF0aW9uLnNldHRpbmcudmFsdWVzWydBYnAuTG9jYWxpemF0aW9uLkRlZmF1bHRMYW5ndWFnZSddO1xuXG4gICAgICAgIGlmIChkZWZhdWx0TGFuZy5pbmNsdWRlcygnOycpKSB7XG4gICAgICAgICAgZGVmYXVsdExhbmcgPSBkZWZhdWx0TGFuZy5zcGxpdCgnOycpWzBdO1xuICAgICAgICB9XG5cbiAgICAgICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoU2Vzc2lvblN0YXRlLmdldExhbmd1YWdlKSA/IG9mKG51bGwpIDogZGlzcGF0Y2gobmV3IFNldExhbmd1YWdlKGRlZmF1bHRMYW5nKSk7XG4gICAgICB9KSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihQYXRjaFJvdXRlQnlOYW1lKVxuICBwYXRjaFJvdXRlKHsgcGF0Y2hTdGF0ZSwgZ2V0U3RhdGUgfTogU3RhdGVDb250ZXh0PENvbmZpZy5TdGF0ZT4sIHsgbmFtZSwgbmV3VmFsdWUgfTogUGF0Y2hSb3V0ZUJ5TmFtZSkge1xuICAgIGxldCByb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSA9IGdldFN0YXRlKCkucm91dGVzO1xuXG4gICAgY29uc3QgaW5kZXggPSByb3V0ZXMuZmluZEluZGV4KHJvdXRlID0+IHJvdXRlLm5hbWUgPT09IG5hbWUpO1xuXG4gICAgcm91dGVzID0gcGF0Y2hSb3V0ZURlZXAocm91dGVzLCBuYW1lLCBuZXdWYWx1ZSk7XG5cbiAgICByZXR1cm4gcGF0Y2hTdGF0ZSh7XG4gICAgICByb3V0ZXMsXG4gICAgfSk7XG4gIH1cbn1cblxuZnVuY3Rpb24gcGF0Y2hSb3V0ZURlZXAoXG4gIHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdLFxuICBuYW1lOiBzdHJpbmcsXG4gIG5ld1ZhbHVlOiBQYXJ0aWFsPEFCUC5GdWxsUm91dGU+LFxuICBwYXJlbnRVcmw6IHN0cmluZyA9IG51bGwsXG4pOiBBQlAuRnVsbFJvdXRlW10ge1xuICByb3V0ZXMgPSByb3V0ZXMubWFwKHJvdXRlID0+IHtcbiAgICBpZiAocm91dGUubmFtZSA9PT0gbmFtZSkge1xuICAgICAgaWYgKG5ld1ZhbHVlLnBhdGgpIHtcbiAgICAgICAgbmV3VmFsdWUudXJsID0gYCR7cGFyZW50VXJsfS8ke25ld1ZhbHVlLnBhdGh9YDtcbiAgICAgIH1cblxuICAgICAgaWYgKG5ld1ZhbHVlLmNoaWxkcmVuICYmIG5ld1ZhbHVlLmNoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgICBuZXdWYWx1ZS5jaGlsZHJlbiA9IG5ld1ZhbHVlLmNoaWxkcmVuLm1hcChjaGlsZCA9PiAoe1xuICAgICAgICAgIC4uLmNoaWxkLFxuICAgICAgICAgIHVybDogYCR7cGFyZW50VXJsfS8ke3JvdXRlLnBhdGh9LyR7Y2hpbGQucGF0aH1gLFxuICAgICAgICB9KSk7XG4gICAgICB9XG5cbiAgICAgIHJldHVybiB7IC4uLnJvdXRlLCAuLi5uZXdWYWx1ZSB9O1xuICAgIH0gZWxzZSBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XG4gICAgICByb3V0ZS5jaGlsZHJlbiA9IHBhdGNoUm91dGVEZWVwKHJvdXRlLmNoaWxkcmVuLCBuYW1lLCBuZXdWYWx1ZSwgKHBhcmVudFVybCB8fCAnLycpICsgcm91dGUucGF0aCk7XG4gICAgfVxuXG4gICAgcmV0dXJuIHJvdXRlO1xuICB9KTtcblxuICBpZiAocGFyZW50VXJsKSB7XG4gICAgLy8gcmVjdXJzaXZlIGJsb2NrXG4gICAgcmV0dXJuIHJvdXRlcztcbiAgfVxuXG4gIHJldHVybiBvcmdhbml6ZVJvdXRlcyhyb3V0ZXMpO1xufVxuIl19