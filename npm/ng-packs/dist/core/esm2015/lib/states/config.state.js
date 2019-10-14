var ConfigState_1;
/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { Action, createSelector, Selector, State, StateContext, Store } from '@ngxs/store';
import { of } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import snq from 'snq';
import { GetAppConfiguration, PatchRouteByName } from '../actions/config.actions';
import { SetLanguage } from '../actions/session.actions';
import { ApplicationConfigurationService } from '../services/application-configuration.service';
import { organizeRoutes } from '../utils/route-utils';
import { SessionState } from './session.state';
let ConfigState = (ConfigState_1 = class ConfigState {
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
    return state.environment.application || /** @type {?} */ ({});
  }
  /**
   * @param {?} key
   * @return {?}
   */
  static getOne(key) {
    /** @type {?} */
    const selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (state => {
        return state[key];
      }),
    );
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
    const selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (state => {
        return /** @type {?} */ (keys).reduce(
          /**
           * @param {?} acc
           * @param {?} val
           * @return {?}
           */
          (acc, val) => {
            if (acc) {
              return acc[val];
            }
            return undefined;
          },
          state,
        );
      }),
    );
    return selector;
  }
  /**
   * @param {?=} path
   * @param {?=} name
   * @return {?}
   */
  static getRoute(path, name) {
    /** @type {?} */
    const selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (state => {
        const { flattedRoutes } = state;
        return /** @type {?} */ (flattedRoutes).find(
          /**
           * @param {?} route
           * @return {?}
           */
          route => {
            if (path && route.path === path) {
              return route;
            } else if (name && route.name === name) {
              return route;
            }
          },
        );
      }),
    );
    return selector;
  }
  /**
   * @param {?=} key
   * @return {?}
   */
  static getApiUrl(key) {
    /** @type {?} */
    const selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (state => {
        return state.environment.apis[key || 'default'].url;
      }),
    );
    return selector;
  }
  /**
   * @param {?} key
   * @return {?}
   */
  static getSetting(key) {
    /** @type {?} */
    const selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (state => {
        return snq(
          /**
           * @return {?}
           */
          () => state.setting.values[key],
        );
      }),
    );
    return selector;
  }
  /**
   * @param {?=} keyword
   * @return {?}
   */
  static getSettings(keyword) {
    /** @type {?} */
    const selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (state => {
        if (keyword) {
          /** @type {?} */
          const keys = snq(
            /**
             * @return {?}
             */
            (() =>
              Object.keys(state.setting.values).filter(
                /**
                 * @param {?} key
                 * @return {?}
                 */
                key => key.indexOf(keyword) > -1,
              )),
            [],
          );
          if (keys.length) {
            return keys.reduce(
              /**
               * @param {?} acc
               * @param {?} key
               * @return {?}
               */
              (acc, key) => Object.assign({}, acc, { [key]: state.setting.values[key] }),
              {},
            );
          }
        }
        return snq(
          /**
           * @return {?}
           */
          () => state.setting.values,
          {},
        );
      }),
    );
    return selector;
  }
  /**
   * @param {?} key
   * @return {?}
   */
  static getGrantedPolicy(key) {
    /** @type {?} */
    const selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (state => {
        if (!key) return true;
        return snq(
          /**
           * @return {?}
           */
          () => state.auth.grantedPolicies[key],
          false,
        );
      }),
    );
    return selector;
  }
  /**
   *
   * \@deprecated, Use getLocalization instead. To be delete in v1
   * @param {?} key
   * @param {...?} interpolateParams
   * @return {?}
   */
  static getCopy(key, ...interpolateParams) {
    if (!key) key = '';
    /** @type {?} */
    const keys = /** @type {?} */ (key.split('::'));
    /** @type {?} */
    const selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (state => {
        if (!state.localization) return key;
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
          keys[0] = snq(
            /**
             * @return {?}
             */
            () => defaultResourceName,
          );
        }
        /** @type {?} */
        let copy = /** @type {?} */ (keys).reduce(
          /**
           * @param {?} acc
           * @param {?} val
           * @return {?}
           */
          ((acc, val) => {
            if (acc) {
              return acc[val];
            }
            return undefined;
          }),
          state.localization.values,
        );
        interpolateParams = interpolateParams.filter(
          /**
           * @param {?} params
           * @return {?}
           */
          params => params != null,
        );
        if (copy && interpolateParams && interpolateParams.length) {
          interpolateParams.forEach(
            /**
             * @param {?} param
             * @return {?}
             */
            param => {
              copy = copy.replace(/[\'\"]?\{[\d]+\}[\'\"]?/, param);
            },
          );
        }
        return copy || key;
      }),
    );
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
    if (!key) key = '';
    /** @type {?} */
    const keys = /** @type {?} */ (key.split('::'));
    /** @type {?} */
    const selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (state => {
        if (!state.localization) return defaultValue || key;
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
          keys[0] = snq(
            /**
             * @return {?}
             */
            () => defaultResourceName,
          );
        }
        /** @type {?} */
        let localization = /** @type {?} */ (keys).reduce(
          /**
           * @param {?} acc
           * @param {?} val
           * @return {?}
           */
          ((acc, val) => {
            if (acc) {
              return acc[val];
            }
            return undefined;
          }),
          state.localization.values,
        );
        interpolateParams = interpolateParams.filter(
          /**
           * @param {?} params
           * @return {?}
           */
          params => params != null,
        );
        if (localization && interpolateParams && interpolateParams.length) {
          interpolateParams.forEach(
            /**
             * @param {?} param
             * @return {?}
             */
            param => {
              localization = localization.replace(/[\'\"]?\{[\d]+\}[\'\"]?/, param);
            },
          );
        }
        if (typeof localization !== 'string') localization = '';
        return localization || defaultValue || key;
      }),
    );
    return selector;
  }
  /**
   * @param {?} __0
   * @return {?}
   */
  addData({ patchState, dispatch }) {
    return this.appConfigurationService.getConfiguration().pipe(
      tap(
        /**
         * @param {?} configuration
         * @return {?}
         */
        configuration => patchState(Object.assign({}, configuration)),
      ),
      switchMap(
        /**
         * @param {?} configuration
         * @return {?}
         */
        configuration => {
          /** @type {?} */
          let defaultLang = configuration.setting.values['Abp.Localization.DefaultLanguage'];
          if (defaultLang.includes(';')) {
            defaultLang = defaultLang.split(';')[0];
          }
          return this.store.selectSnapshot(SessionState.getLanguage)
            ? of(null)
            : dispatch(new SetLanguage(defaultLang));
        },
      ),
    );
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
    const index = routes.findIndex(
      /**
       * @param {?} route
       * @return {?}
       */
      (route => route.name === name),
    );
    routes = patchRouteDeep(routes, name, newValue);
    return patchState({
      routes,
    });
  }
});
ConfigState.ctorParameters = () => [{ type: ApplicationConfigurationService }, { type: Store }];
tslib_1.__decorate(
  [
    Action(GetAppConfiguration),
    tslib_1.__metadata('design:type', Function),
    tslib_1.__metadata('design:paramtypes', [Object]),
    tslib_1.__metadata('design:returntype', void 0),
  ],
  ConfigState.prototype,
  'addData',
  null,
);
tslib_1.__decorate(
  [
    Action(PatchRouteByName),
    tslib_1.__metadata('design:type', Function),
    tslib_1.__metadata('design:paramtypes', [Object, PatchRouteByName]),
    tslib_1.__metadata('design:returntype', void 0),
  ],
  ConfigState.prototype,
  'patchRoute',
  null,
);
tslib_1.__decorate(
  [
    Selector(),
    tslib_1.__metadata('design:type', Function),
    tslib_1.__metadata('design:paramtypes', [Object]),
    tslib_1.__metadata('design:returntype', void 0),
  ],
  ConfigState,
  'getAll',
  null,
);
tslib_1.__decorate(
  [
    Selector(),
    tslib_1.__metadata('design:type', Function),
    tslib_1.__metadata('design:paramtypes', [Object]),
    tslib_1.__metadata('design:returntype', Object),
  ],
  ConfigState,
  'getApplicationInfo',
  null,
);
ConfigState = ConfigState_1 = tslib_1.__decorate(
  [
    State({
      name: 'ConfigState',
      defaults: /** @type {?} */ ({}),
    }),
    tslib_1.__metadata('design:paramtypes', [ApplicationConfigurationService, Store]),
  ],
  ConfigState,
);
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
  routes = routes.map(
    /**
     * @param {?} route
     * @return {?}
     */
    route => {
      if (route.name === name) {
        if (newValue.path) {
          newValue.url = `${parentUrl}/${newValue.path}`;
        }
        if (newValue.children && newValue.children.length) {
          newValue.children = newValue.children.map(
            /**
             * @param {?} child
             * @return {?}
             */
            child => Object.assign({}, child, { url: `${parentUrl}/${route.path}/${child.path}` }),
          );
        }
        return Object.assign({}, route, newValue);
      } else if (route.children && route.children.length) {
        route.children = patchRouteDeep(route.children, name, newValue, (parentUrl || '/') + route.path);
      }
      return route;
    },
  );
  if (parentUrl) {
    // recursive block
    return routes;
  }
  return organizeRoutes(routes);
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9jb25maWcuc3RhdGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLE1BQU0sRUFBRSxjQUFjLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBRSxZQUFZLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzNGLE9BQU8sRUFBRSxFQUFFLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDMUIsT0FBTyxFQUFFLFNBQVMsRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNoRCxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFDdEIsT0FBTyxFQUFFLG1CQUFtQixFQUFFLGdCQUFnQixFQUFFLE1BQU0sMkJBQTJCLENBQUM7QUFDbEYsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBR3pELE9BQU8sRUFBRSwrQkFBK0IsRUFBRSxNQUFNLCtDQUErQyxDQUFDO0FBQ2hHLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0saUJBQWlCLENBQUM7SUFNbEMsV0FBVyx5QkFBWCxXQUFXOzs7OztJQWdPdEIsWUFBb0IsdUJBQXdELEVBQVUsS0FBWTtRQUE5RSw0QkFBdUIsR0FBdkIsdUJBQXVCLENBQWlDO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7O0lBOU50RyxNQUFNLENBQUMsTUFBTSxDQUFDLEtBQW1CO1FBQy9CLE9BQU8sS0FBSyxDQUFDO0lBQ2YsQ0FBQzs7Ozs7SUFHRCxNQUFNLENBQUMsa0JBQWtCLENBQUMsS0FBbUI7UUFDM0MsT0FBTyxLQUFLLENBQUMsV0FBVyxDQUFDLFdBQVcsSUFBSSxDQUFDLG1CQUFBLEVBQUUsRUFBc0IsQ0FBQyxDQUFDO0lBQ3JFLENBQUM7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RCxPQUFPLGNBQWMsQ0FBQyxNQUFNLENBQUMsQ0FBQztBQUNoQyxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBjcmVhdGVTZWxlY3RvciwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgb2YgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IHN3aXRjaE1hcCwgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuaW1wb3J0IHsgR2V0QXBwQ29uZmlndXJhdGlvbiwgUGF0Y2hSb3V0ZUJ5TmFtZSB9IGZyb20gJy4uL2FjdGlvbnMvY29uZmlnLmFjdGlvbnMnO1xuaW1wb3J0IHsgU2V0TGFuZ3VhZ2UgfSBmcm9tICcuLi9hY3Rpb25zL3Nlc3Npb24uYWN0aW9ucyc7XG5pbXBvcnQgeyBBQlAgfSBmcm9tICcuLi9tb2RlbHMvY29tbW9uJztcbmltcG9ydCB7IENvbmZpZyB9IGZyb20gJy4uL21vZGVscy9jb25maWcnO1xuaW1wb3J0IHsgQXBwbGljYXRpb25Db25maWd1cmF0aW9uU2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL2FwcGxpY2F0aW9uLWNvbmZpZ3VyYXRpb24uc2VydmljZSc7XG5pbXBvcnQgeyBvcmdhbml6ZVJvdXRlcyB9IGZyb20gJy4uL3V0aWxzL3JvdXRlLXV0aWxzJztcbmltcG9ydCB7IFNlc3Npb25TdGF0ZSB9IGZyb20gJy4vc2Vzc2lvbi5zdGF0ZSc7XG5cbkBTdGF0ZTxDb25maWcuU3RhdGU+KHtcbiAgbmFtZTogJ0NvbmZpZ1N0YXRlJyxcbiAgZGVmYXVsdHM6IHt9IGFzIENvbmZpZy5TdGF0ZSxcbn0pXG5leHBvcnQgY2xhc3MgQ29uZmlnU3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0QWxsKHN0YXRlOiBDb25maWcuU3RhdGUpIHtcbiAgICByZXR1cm4gc3RhdGU7XG4gIH1cblxuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0QXBwbGljYXRpb25JbmZvKHN0YXRlOiBDb25maWcuU3RhdGUpOiBDb25maWcuQXBwbGljYXRpb24ge1xuICAgIHJldHVybiBzdGF0ZS5lbnZpcm9ubWVudC5hcHBsaWNhdGlvbiB8fCAoe30gYXMgQ29uZmlnLkFwcGxpY2F0aW9uKTtcbiAgfVxuXG4gIHN0YXRpYyBnZXRPbmUoa2V5OiBzdHJpbmcpIHtcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxuICAgICAgW0NvbmZpZ1N0YXRlXSxcbiAgICAgIChzdGF0ZTogQ29uZmlnLlN0YXRlKSA9PiB7XG4gICAgICAgIHJldHVybiBzdGF0ZVtrZXldO1xuICAgICAgfSxcbiAgICApO1xuXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xuICB9XG5cbiAgc3RhdGljIGdldERlZXAoa2V5czogc3RyaW5nW10gfCBzdHJpbmcpIHtcbiAgICBpZiAodHlwZW9mIGtleXMgPT09ICdzdHJpbmcnKSB7XG4gICAgICBrZXlzID0ga2V5cy5zcGxpdCgnLicpO1xuICAgIH1cblxuICAgIGlmICghQXJyYXkuaXNBcnJheShrZXlzKSkge1xuICAgICAgdGhyb3cgbmV3IEVycm9yKCdUaGUgYXJndW1lbnQgbXVzdCBiZSBhIGRvdCBzdHJpbmcgb3IgYW4gc3RyaW5nIGFycmF5LicpO1xuICAgIH1cblxuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoXG4gICAgICBbQ29uZmlnU3RhdGVdLFxuICAgICAgKHN0YXRlOiBDb25maWcuU3RhdGUpID0+IHtcbiAgICAgICAgcmV0dXJuIChrZXlzIGFzIHN0cmluZ1tdKS5yZWR1Y2UoKGFjYywgdmFsKSA9PiB7XG4gICAgICAgICAgaWYgKGFjYykge1xuICAgICAgICAgICAgcmV0dXJuIGFjY1t2YWxdO1xuICAgICAgICAgIH1cblxuICAgICAgICAgIHJldHVybiB1bmRlZmluZWQ7XG4gICAgICAgIH0sIHN0YXRlKTtcbiAgICAgIH0sXG4gICAgKTtcblxuICAgIHJldHVybiBzZWxlY3RvcjtcbiAgfVxuXG4gIHN0YXRpYyBnZXRSb3V0ZShwYXRoPzogc3RyaW5nLCBuYW1lPzogc3RyaW5nKSB7XG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcbiAgICAgIFtDb25maWdTdGF0ZV0sXG4gICAgICAoc3RhdGU6IENvbmZpZy5TdGF0ZSkgPT4ge1xuICAgICAgICBjb25zdCB7IGZsYXR0ZWRSb3V0ZXMgfSA9IHN0YXRlO1xuICAgICAgICByZXR1cm4gKGZsYXR0ZWRSb3V0ZXMgYXMgQUJQLkZ1bGxSb3V0ZVtdKS5maW5kKHJvdXRlID0+IHtcbiAgICAgICAgICBpZiAocGF0aCAmJiByb3V0ZS5wYXRoID09PSBwYXRoKSB7XG4gICAgICAgICAgICByZXR1cm4gcm91dGU7XG4gICAgICAgICAgfSBlbHNlIGlmIChuYW1lICYmIHJvdXRlLm5hbWUgPT09IG5hbWUpIHtcbiAgICAgICAgICAgIHJldHVybiByb3V0ZTtcbiAgICAgICAgICB9XG4gICAgICAgIH0pO1xuICAgICAgfSxcbiAgICApO1xuXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xuICB9XG5cbiAgc3RhdGljIGdldEFwaVVybChrZXk/OiBzdHJpbmcpIHtcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxuICAgICAgW0NvbmZpZ1N0YXRlXSxcbiAgICAgIChzdGF0ZTogQ29uZmlnLlN0YXRlKTogc3RyaW5nID0+IHtcbiAgICAgICAgcmV0dXJuIHN0YXRlLmVudmlyb25tZW50LmFwaXNba2V5IHx8ICdkZWZhdWx0J10udXJsO1xuICAgICAgfSxcbiAgICApO1xuXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xuICB9XG5cbiAgc3RhdGljIGdldFNldHRpbmcoa2V5OiBzdHJpbmcpIHtcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxuICAgICAgW0NvbmZpZ1N0YXRlXSxcbiAgICAgIChzdGF0ZTogQ29uZmlnLlN0YXRlKSA9PiB7XG4gICAgICAgIHJldHVybiBzbnEoKCkgPT4gc3RhdGUuc2V0dGluZy52YWx1ZXNba2V5XSk7XG4gICAgICB9LFxuICAgICk7XG4gICAgcmV0dXJuIHNlbGVjdG9yO1xuICB9XG5cbiAgc3RhdGljIGdldFNldHRpbmdzKGtleXdvcmQ/OiBzdHJpbmcpIHtcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxuICAgICAgW0NvbmZpZ1N0YXRlXSxcbiAgICAgIChzdGF0ZTogQ29uZmlnLlN0YXRlKSA9PiB7XG4gICAgICAgIGlmIChrZXl3b3JkKSB7XG4gICAgICAgICAgY29uc3Qga2V5cyA9IHNucSgoKSA9PiBPYmplY3Qua2V5cyhzdGF0ZS5zZXR0aW5nLnZhbHVlcykuZmlsdGVyKGtleSA9PiBrZXkuaW5kZXhPZihrZXl3b3JkKSA+IC0xKSwgW10pO1xuXG4gICAgICAgICAgaWYgKGtleXMubGVuZ3RoKSB7XG4gICAgICAgICAgICByZXR1cm4ga2V5cy5yZWR1Y2UoKGFjYywga2V5KSA9PiAoeyAuLi5hY2MsIFtrZXldOiBzdGF0ZS5zZXR0aW5nLnZhbHVlc1trZXldIH0pLCB7fSk7XG4gICAgICAgICAgfVxuICAgICAgICB9XG5cbiAgICAgICAgcmV0dXJuIHNucSgoKSA9PiBzdGF0ZS5zZXR0aW5nLnZhbHVlcywge30pO1xuICAgICAgfSxcbiAgICApO1xuICAgIHJldHVybiBzZWxlY3RvcjtcbiAgfVxuXG4gIHN0YXRpYyBnZXRHcmFudGVkUG9saWN5KGtleTogc3RyaW5nKSB7XG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcbiAgICAgIFtDb25maWdTdGF0ZV0sXG4gICAgICAoc3RhdGU6IENvbmZpZy5TdGF0ZSk6IGJvb2xlYW4gPT4ge1xuICAgICAgICBpZiAoIWtleSkgcmV0dXJuIHRydWU7XG4gICAgICAgIHJldHVybiBzbnEoKCkgPT4gc3RhdGUuYXV0aC5ncmFudGVkUG9saWNpZXNba2V5XSwgZmFsc2UpO1xuICAgICAgfSxcbiAgICApO1xuXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xuICB9XG5cbiAgLyoqXG4gICAqXG4gICAqIEBkZXByZWNhdGVkLCBVc2UgZ2V0TG9jYWxpemF0aW9uIGluc3RlYWQuIFRvIGJlIGRlbGV0ZSBpbiB2MVxuICAgKi9cbiAgc3RhdGljIGdldENvcHkoa2V5OiBzdHJpbmcsIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXSkge1xuICAgIGlmICgha2V5KSBrZXkgPSAnJztcblxuICAgIGNvbnN0IGtleXMgPSBrZXkuc3BsaXQoJzo6JykgYXMgc3RyaW5nW107XG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcbiAgICAgIFtDb25maWdTdGF0ZV0sXG4gICAgICAoc3RhdGU6IENvbmZpZy5TdGF0ZSkgPT4ge1xuICAgICAgICBpZiAoIXN0YXRlLmxvY2FsaXphdGlvbikgcmV0dXJuIGtleTtcblxuICAgICAgICBjb25zdCB7IGRlZmF1bHRSZXNvdXJjZU5hbWUgfSA9IHN0YXRlLmVudmlyb25tZW50LmxvY2FsaXphdGlvbjtcbiAgICAgICAgaWYgKGtleXNbMF0gPT09ICcnKSB7XG4gICAgICAgICAgaWYgKCFkZWZhdWx0UmVzb3VyY2VOYW1lKSB7XG4gICAgICAgICAgICB0aHJvdyBuZXcgRXJyb3IoXG4gICAgICAgICAgICAgIGBQbGVhc2UgY2hlY2sgeW91ciBlbnZpcm9ubWVudC4gTWF5IHlvdSBmb3JnZXQgc2V0IGRlZmF1bHRSZXNvdXJjZU5hbWU/XG4gICAgICAgICAgICAgIEhlcmUgaXMgdGhlIGV4YW1wbGU6XG4gICAgICAgICAgICAgICB7IHByb2R1Y3Rpb246IGZhbHNlLFxuICAgICAgICAgICAgICAgICBsb2NhbGl6YXRpb246IHtcbiAgICAgICAgICAgICAgICAgICBkZWZhdWx0UmVzb3VyY2VOYW1lOiAnTXlQcm9qZWN0TmFtZSdcbiAgICAgICAgICAgICAgICAgIH1cbiAgICAgICAgICAgICAgIH1gLFxuICAgICAgICAgICAgKTtcbiAgICAgICAgICB9XG5cbiAgICAgICAgICBrZXlzWzBdID0gc25xKCgpID0+IGRlZmF1bHRSZXNvdXJjZU5hbWUpO1xuICAgICAgICB9XG5cbiAgICAgICAgbGV0IGNvcHkgPSAoa2V5cyBhcyBhbnkpLnJlZHVjZSgoYWNjLCB2YWwpID0+IHtcbiAgICAgICAgICBpZiAoYWNjKSB7XG4gICAgICAgICAgICByZXR1cm4gYWNjW3ZhbF07XG4gICAgICAgICAgfVxuXG4gICAgICAgICAgcmV0dXJuIHVuZGVmaW5lZDtcbiAgICAgICAgfSwgc3RhdGUubG9jYWxpemF0aW9uLnZhbHVlcyk7XG5cbiAgICAgICAgaW50ZXJwb2xhdGVQYXJhbXMgPSBpbnRlcnBvbGF0ZVBhcmFtcy5maWx0ZXIocGFyYW1zID0+IHBhcmFtcyAhPSBudWxsKTtcbiAgICAgICAgaWYgKGNvcHkgJiYgaW50ZXJwb2xhdGVQYXJhbXMgJiYgaW50ZXJwb2xhdGVQYXJhbXMubGVuZ3RoKSB7XG4gICAgICAgICAgaW50ZXJwb2xhdGVQYXJhbXMuZm9yRWFjaChwYXJhbSA9PiB7XG4gICAgICAgICAgICBjb3B5ID0gY29weS5yZXBsYWNlKC9bXFwnXFxcIl0/XFx7W1xcZF0rXFx9W1xcJ1xcXCJdPy8sIHBhcmFtKTtcbiAgICAgICAgICB9KTtcbiAgICAgICAgfVxuXG4gICAgICAgIHJldHVybiBjb3B5IHx8IGtleTtcbiAgICAgIH0sXG4gICAgKTtcblxuICAgIHJldHVybiBzZWxlY3RvcjtcbiAgfVxuXG4gIHN0YXRpYyBnZXRMb2NhbGl6YXRpb24oa2V5OiBzdHJpbmcgfCBDb25maWcuTG9jYWxpemF0aW9uV2l0aERlZmF1bHQsIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXSkge1xuICAgIGxldCBkZWZhdWx0VmFsdWU6IHN0cmluZztcblxuICAgIGlmICh0eXBlb2Yga2V5ICE9PSAnc3RyaW5nJykge1xuICAgICAgZGVmYXVsdFZhbHVlID0ga2V5LmRlZmF1bHRWYWx1ZTtcbiAgICAgIGtleSA9IGtleS5rZXk7XG4gICAgfVxuXG4gICAgaWYgKCFrZXkpIGtleSA9ICcnO1xuXG4gICAgY29uc3Qga2V5cyA9IGtleS5zcGxpdCgnOjonKSBhcyBzdHJpbmdbXTtcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxuICAgICAgW0NvbmZpZ1N0YXRlXSxcbiAgICAgIChzdGF0ZTogQ29uZmlnLlN0YXRlKSA9PiB7XG4gICAgICAgIGlmICghc3RhdGUubG9jYWxpemF0aW9uKSByZXR1cm4gZGVmYXVsdFZhbHVlIHx8IGtleTtcblxuICAgICAgICBjb25zdCB7IGRlZmF1bHRSZXNvdXJjZU5hbWUgfSA9IHN0YXRlLmVudmlyb25tZW50LmxvY2FsaXphdGlvbjtcbiAgICAgICAgaWYgKGtleXNbMF0gPT09ICcnKSB7XG4gICAgICAgICAgaWYgKCFkZWZhdWx0UmVzb3VyY2VOYW1lKSB7XG4gICAgICAgICAgICB0aHJvdyBuZXcgRXJyb3IoXG4gICAgICAgICAgICAgIGBQbGVhc2UgY2hlY2sgeW91ciBlbnZpcm9ubWVudC4gTWF5IHlvdSBmb3JnZXQgc2V0IGRlZmF1bHRSZXNvdXJjZU5hbWU/XG4gICAgICAgICAgICAgIEhlcmUgaXMgdGhlIGV4YW1wbGU6XG4gICAgICAgICAgICAgICB7IHByb2R1Y3Rpb246IGZhbHNlLFxuICAgICAgICAgICAgICAgICBsb2NhbGl6YXRpb246IHtcbiAgICAgICAgICAgICAgICAgICBkZWZhdWx0UmVzb3VyY2VOYW1lOiAnTXlQcm9qZWN0TmFtZSdcbiAgICAgICAgICAgICAgICAgIH1cbiAgICAgICAgICAgICAgIH1gLFxuICAgICAgICAgICAgKTtcbiAgICAgICAgICB9XG5cbiAgICAgICAgICBrZXlzWzBdID0gc25xKCgpID0+IGRlZmF1bHRSZXNvdXJjZU5hbWUpO1xuICAgICAgICB9XG5cbiAgICAgICAgbGV0IGxvY2FsaXphdGlvbiA9IChrZXlzIGFzIGFueSkucmVkdWNlKChhY2MsIHZhbCkgPT4ge1xuICAgICAgICAgIGlmIChhY2MpIHtcbiAgICAgICAgICAgIHJldHVybiBhY2NbdmFsXTtcbiAgICAgICAgICB9XG5cbiAgICAgICAgICByZXR1cm4gdW5kZWZpbmVkO1xuICAgICAgICB9LCBzdGF0ZS5sb2NhbGl6YXRpb24udmFsdWVzKTtcblxuICAgICAgICBpbnRlcnBvbGF0ZVBhcmFtcyA9IGludGVycG9sYXRlUGFyYW1zLmZpbHRlcihwYXJhbXMgPT4gcGFyYW1zICE9IG51bGwpO1xuICAgICAgICBpZiAobG9jYWxpemF0aW9uICYmIGludGVycG9sYXRlUGFyYW1zICYmIGludGVycG9sYXRlUGFyYW1zLmxlbmd0aCkge1xuICAgICAgICAgIGludGVycG9sYXRlUGFyYW1zLmZvckVhY2gocGFyYW0gPT4ge1xuICAgICAgICAgICAgbG9jYWxpemF0aW9uID0gbG9jYWxpemF0aW9uLnJlcGxhY2UoL1tcXCdcXFwiXT9cXHtbXFxkXStcXH1bXFwnXFxcIl0/LywgcGFyYW0pO1xuICAgICAgICAgIH0pO1xuICAgICAgICB9XG5cbiAgICAgICAgaWYgKHR5cGVvZiBsb2NhbGl6YXRpb24gIT09ICdzdHJpbmcnKSBsb2NhbGl6YXRpb24gPSAnJztcbiAgICAgICAgcmV0dXJuIGxvY2FsaXphdGlvbiB8fCBkZWZhdWx0VmFsdWUgfHwga2V5O1xuICAgICAgfSxcbiAgICApO1xuXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBhcHBDb25maWd1cmF0aW9uU2VydmljZTogQXBwbGljYXRpb25Db25maWd1cmF0aW9uU2VydmljZSwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgQEFjdGlvbihHZXRBcHBDb25maWd1cmF0aW9uKVxuICBhZGREYXRhKHsgcGF0Y2hTdGF0ZSwgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PENvbmZpZy5TdGF0ZT4pIHtcbiAgICByZXR1cm4gdGhpcy5hcHBDb25maWd1cmF0aW9uU2VydmljZS5nZXRDb25maWd1cmF0aW9uKCkucGlwZShcbiAgICAgIHRhcChjb25maWd1cmF0aW9uID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIC4uLmNvbmZpZ3VyYXRpb24sXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICAgIHN3aXRjaE1hcChjb25maWd1cmF0aW9uID0+IHtcbiAgICAgICAgbGV0IGRlZmF1bHRMYW5nOiBzdHJpbmcgPSBjb25maWd1cmF0aW9uLnNldHRpbmcudmFsdWVzWydBYnAuTG9jYWxpemF0aW9uLkRlZmF1bHRMYW5ndWFnZSddO1xuXG4gICAgICAgIGlmIChkZWZhdWx0TGFuZy5pbmNsdWRlcygnOycpKSB7XG4gICAgICAgICAgZGVmYXVsdExhbmcgPSBkZWZhdWx0TGFuZy5zcGxpdCgnOycpWzBdO1xuICAgICAgICB9XG5cbiAgICAgICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoU2Vzc2lvblN0YXRlLmdldExhbmd1YWdlKSA/IG9mKG51bGwpIDogZGlzcGF0Y2gobmV3IFNldExhbmd1YWdlKGRlZmF1bHRMYW5nKSk7XG4gICAgICB9KSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihQYXRjaFJvdXRlQnlOYW1lKVxuICBwYXRjaFJvdXRlKHsgcGF0Y2hTdGF0ZSwgZ2V0U3RhdGUgfTogU3RhdGVDb250ZXh0PENvbmZpZy5TdGF0ZT4sIHsgbmFtZSwgbmV3VmFsdWUgfTogUGF0Y2hSb3V0ZUJ5TmFtZSkge1xuICAgIGxldCByb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSA9IGdldFN0YXRlKCkucm91dGVzO1xuXG4gICAgY29uc3QgaW5kZXggPSByb3V0ZXMuZmluZEluZGV4KHJvdXRlID0+IHJvdXRlLm5hbWUgPT09IG5hbWUpO1xuXG4gICAgcm91dGVzID0gcGF0Y2hSb3V0ZURlZXAocm91dGVzLCBuYW1lLCBuZXdWYWx1ZSk7XG5cbiAgICByZXR1cm4gcGF0Y2hTdGF0ZSh7XG4gICAgICByb3V0ZXMsXG4gICAgfSk7XG4gIH1cbn1cblxuZnVuY3Rpb24gcGF0Y2hSb3V0ZURlZXAoXG4gIHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdLFxuICBuYW1lOiBzdHJpbmcsXG4gIG5ld1ZhbHVlOiBQYXJ0aWFsPEFCUC5GdWxsUm91dGU+LFxuICBwYXJlbnRVcmw6IHN0cmluZyA9IG51bGwsXG4pOiBBQlAuRnVsbFJvdXRlW10ge1xuICByb3V0ZXMgPSByb3V0ZXMubWFwKHJvdXRlID0+IHtcbiAgICBpZiAocm91dGUubmFtZSA9PT0gbmFtZSkge1xuICAgICAgaWYgKG5ld1ZhbHVlLnBhdGgpIHtcbiAgICAgICAgbmV3VmFsdWUudXJsID0gYCR7cGFyZW50VXJsfS8ke25ld1ZhbHVlLnBhdGh9YDtcbiAgICAgIH1cblxuICAgICAgaWYgKG5ld1ZhbHVlLmNoaWxkcmVuICYmIG5ld1ZhbHVlLmNoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgICBuZXdWYWx1ZS5jaGlsZHJlbiA9IG5ld1ZhbHVlLmNoaWxkcmVuLm1hcChjaGlsZCA9PiAoe1xuICAgICAgICAgIC4uLmNoaWxkLFxuICAgICAgICAgIHVybDogYCR7cGFyZW50VXJsfS8ke3JvdXRlLnBhdGh9LyR7Y2hpbGQucGF0aH1gLFxuICAgICAgICB9KSk7XG4gICAgICB9XG5cbiAgICAgIHJldHVybiB7IC4uLnJvdXRlLCAuLi5uZXdWYWx1ZSB9O1xuICAgIH0gZWxzZSBpZiAocm91dGUuY2hpbGRyZW4gJiYgcm91dGUuY2hpbGRyZW4ubGVuZ3RoKSB7XG4gICAgICByb3V0ZS5jaGlsZHJlbiA9IHBhdGNoUm91dGVEZWVwKHJvdXRlLmNoaWxkcmVuLCBuYW1lLCBuZXdWYWx1ZSwgKHBhcmVudFVybCB8fCAnLycpICsgcm91dGUucGF0aCk7XG4gICAgfVxuXG4gICAgcmV0dXJuIHJvdXRlO1xuICB9KTtcblxuICBpZiAocGFyZW50VXJsKSB7XG4gICAgLy8gcmVjdXJzaXZlIGJsb2NrXG4gICAgcmV0dXJuIHJvdXRlcztcbiAgfVxuXG4gIHJldHVybiBvcmdhbml6ZVJvdXRlcyhyb3V0ZXMpO1xufVxuIl19
