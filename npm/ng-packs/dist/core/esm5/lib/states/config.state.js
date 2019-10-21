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
var ConfigState = /** @class */ (function() {
  function ConfigState(appConfigurationService, store) {
    this.appConfigurationService = appConfigurationService;
    this.store = store;
  }
  ConfigState_1 = ConfigState;
  /**
   * @param {?} state
   * @return {?}
   */
  ConfigState.getAll
  /**
   * @param {?} state
   * @return {?}
   */ = function(state) {
    return state;
  };
  /**
   * @param {?} state
   * @return {?}
   */
  ConfigState.getApplicationInfo
  /**
   * @param {?} state
   * @return {?}
   */ = function(state) {
    return state.environment.application || /** @type {?} */ ({});
  };
  /**
   * @param {?} key
   * @return {?}
   */
  ConfigState.getOne
  /**
   * @param {?} key
   * @return {?}
   */ = function(key) {
    /** @type {?} */
    var selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (function(state) {
        return state[key];
      }),
    );
    return selector;
  };
  /**
   * @param {?} keys
   * @return {?}
   */
  ConfigState.getDeep
  /**
   * @param {?} keys
   * @return {?}
   */ = function(keys) {
    if (typeof keys === 'string') {
      keys = keys.split('.');
    }
    if (!Array.isArray(keys)) {
      throw new Error('The argument must be a dot string or an string array.');
    }
    /** @type {?} */
    var selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (function(state) {
        return /** @type {?} */ (keys).reduce(
          /**
           * @param {?} acc
           * @param {?} val
           * @return {?}
           */
          function(acc, val) {
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
  };
  /**
   * @param {?=} path
   * @param {?=} name
   * @return {?}
   */
  ConfigState.getRoute
  /**
   * @param {?=} path
   * @param {?=} name
   * @return {?}
   */ = function(path, name) {
    /** @type {?} */
    var selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (function(state) {
        var flattedRoutes = state.flattedRoutes;
        return /** @type {?} */ (flattedRoutes).find(
          /**
           * @param {?} route
           * @return {?}
           */
          function(route) {
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
  };
  /**
   * @param {?=} key
   * @return {?}
   */
  ConfigState.getApiUrl
  /**
   * @param {?=} key
   * @return {?}
   */ = function(key) {
    /** @type {?} */
    var selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (function(state) {
        return state.environment.apis[key || 'default'].url;
      }),
    );
    return selector;
  };
  /**
   * @param {?} key
   * @return {?}
   */
  ConfigState.getSetting
  /**
   * @param {?} key
   * @return {?}
   */ = function(key) {
    /** @type {?} */
    var selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (function(state) {
        return snq(
          /**
           * @return {?}
           */
          function() {
            return state.setting.values[key];
          },
        );
      }),
    );
    return selector;
  };
  /**
   * @param {?=} keyword
   * @return {?}
   */
  ConfigState.getSettings
  /**
   * @param {?=} keyword
   * @return {?}
   */ = function(keyword) {
    /** @type {?} */
    var selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (function(state) {
        if (keyword) {
          /** @type {?} */
          var keys = snq(
            /**
             * @return {?}
             */
            (function() {
              return Object.keys(state.setting.values).filter(
                /**
                 * @param {?} key
                 * @return {?}
                 */
                function(key) {
                  return key.indexOf(keyword) > -1;
                },
              );
            }),
            [],
          );
          if (keys.length) {
            return keys.reduce(
              /**
               * @param {?} acc
               * @param {?} key
               * @return {?}
               */
              function(acc, key) {
                var _a;
                return tslib_1.__assign({}, acc, ((_a = {}), (_a[key] = state.setting.values[key]), _a));
              },
              {},
            );
          }
        }
        return snq(
          /**
           * @return {?}
           */
          function() {
            return state.setting.values;
          },
          {},
        );
      }),
    );
    return selector;
  };
  /**
   * @param {?} key
   * @return {?}
   */
  ConfigState.getGrantedPolicy
  /**
   * @param {?} key
   * @return {?}
   */ = function(key) {
    /** @type {?} */
    var selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (function(state) {
        if (!key) return true;
        return snq(
          /**
           * @return {?}
           */
          function() {
            return state.auth.grantedPolicies[key];
          },
          false,
        );
      }),
    );
    return selector;
  };
  /**
   *
   * @deprecated, Use getLocalization instead. To be delete in v1
   */
  /**
   *
   * \@deprecated, Use getLocalization instead. To be delete in v1
   * @param {?} key
   * @param {...?} interpolateParams
   * @return {?}
   */
  ConfigState.getCopy
  /**
   *
   * \@deprecated, Use getLocalization instead. To be delete in v1
   * @param {?} key
   * @param {...?} interpolateParams
   * @return {?}
   */ = function(key) {
    var interpolateParams = [];
    for (var _i = 1; _i < arguments.length; _i++) {
      interpolateParams[_i - 1] = arguments[_i];
    }
    if (!key) key = '';
    /** @type {?} */
    var keys = /** @type {?} */ (key.split('::'));
    /** @type {?} */
    var selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (function(state) {
        if (!state.localization) return key;
        var defaultResourceName = state.environment.localization.defaultResourceName;
        if (keys[0] === '') {
          if (!defaultResourceName) {
            throw new Error(
              "Please check your environment. May you forget set defaultResourceName?\n              Here is the example:\n               { production: false,\n                 localization: {\n                   defaultResourceName: 'MyProjectName'\n                  }\n               }",
            );
          }
          keys[0] = snq(
            /**
             * @return {?}
             */
            function() {
              return defaultResourceName;
            },
          );
        }
        /** @type {?} */
        var copy = /** @type {?} */ (keys).reduce(
          /**
           * @param {?} acc
           * @param {?} val
           * @return {?}
           */
          (function(acc, val) {
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
          function(params) {
            return params != null;
          },
        );
        if (copy && interpolateParams && interpolateParams.length) {
          interpolateParams.forEach(
            /**
             * @param {?} param
             * @return {?}
             */
            function(param) {
              copy = copy.replace(/[\'\"]?\{[\d]+\}[\'\"]?/, param);
            },
          );
        }
        return copy || key;
      }),
    );
    return selector;
  };
  /**
   * @param {?} key
   * @param {...?} interpolateParams
   * @return {?}
   */
  ConfigState.getLocalization
  /**
   * @param {?} key
   * @param {...?} interpolateParams
   * @return {?}
   */ = function(key) {
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
    if (!key) key = '';
    /** @type {?} */
    var keys = /** @type {?} */ (key.split('::'));
    /** @type {?} */
    var selector = createSelector(
      [ConfigState_1]
      /**
       * @param {?} state
       * @return {?}
       */,
      (function(state) {
        if (!state.localization) return defaultValue || key;
        var defaultResourceName = state.environment.localization.defaultResourceName;
        if (keys[0] === '') {
          if (!defaultResourceName) {
            throw new Error(
              "Please check your environment. May you forget set defaultResourceName?\n              Here is the example:\n               { production: false,\n                 localization: {\n                   defaultResourceName: 'MyProjectName'\n                  }\n               }",
            );
          }
          keys[0] = snq(
            /**
             * @return {?}
             */
            function() {
              return defaultResourceName;
            },
          );
        }
        /** @type {?} */
        var localization = /** @type {?} */ (keys).reduce(
          /**
           * @param {?} acc
           * @param {?} val
           * @return {?}
           */
          (function(acc, val) {
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
          function(params) {
            return params != null;
          },
        );
        if (localization && interpolateParams && interpolateParams.length) {
          interpolateParams.forEach(
            /**
             * @param {?} param
             * @return {?}
             */
            function(param) {
              localization = localization.replace(/[\'\"]?\{[\d]+\}[\'\"]?/, param);
            },
          );
        }
        if (typeof localization !== 'string') localization = '';
        return localization || defaultValue || key;
      }),
    );
    return selector;
  };
  /**
   * @param {?} __0
   * @return {?}
   */
  ConfigState.prototype.addData
  /**
   * @param {?} __0
   * @return {?}
   */ = function(_a) {
    var _this = this;
    var patchState = _a.patchState,
      dispatch = _a.dispatch;
    return this.appConfigurationService.getConfiguration().pipe(
      tap(
        /**
         * @param {?} configuration
         * @return {?}
         */
        function(configuration) {
          return patchState(tslib_1.__assign({}, configuration));
        },
      ),
      switchMap(
        /**
         * @param {?} configuration
         * @return {?}
         */
        function(configuration) {
          /** @type {?} */
          var defaultLang = configuration.setting.values['Abp.Localization.DefaultLanguage'];
          if (defaultLang.includes(';')) {
            defaultLang = defaultLang.split(';')[0];
          }
          return _this.store.selectSnapshot(SessionState.getLanguage)
            ? of(null)
            : dispatch(new SetLanguage(defaultLang));
        },
      ),
    );
  };
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  ConfigState.prototype.patchRoute
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */ = function(_a, _b) {
    var patchState = _a.patchState,
      getState = _a.getState;
    var name = _b.name,
      newValue = _b.newValue;
    /** @type {?} */
    var routes = getState().routes;
    /** @type {?} */
    var index = routes.findIndex(
      /**
       * @param {?} route
       * @return {?}
       */
      (function(route) {
        return route.name === name;
      }),
    );
    routes = patchRouteDeep(routes, name, newValue);
    return patchState({
      routes: routes,
    });
  };
  var ConfigState_1;
  ConfigState.ctorParameters = function() {
    return [{ type: ApplicationConfigurationService }, { type: Store }];
  };
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
  return ConfigState;
})();
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
  if (parentUrl === void 0) {
    parentUrl = null;
  }
  routes = routes.map(
    /**
     * @param {?} route
     * @return {?}
     */
    function(route) {
      if (route.name === name) {
        if (newValue.path) {
          newValue.url = parentUrl + '/' + newValue.path;
        }
        if (newValue.children && newValue.children.length) {
          newValue.children = newValue.children.map(
            /**
             * @param {?} child
             * @return {?}
             */
            function(child) {
              return tslib_1.__assign({}, child, { url: parentUrl + '/' + route.path + '/' + child.path });
            },
          );
        }
        return tslib_1.__assign({}, route, newValue);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9jb25maWcuc3RhdGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLGNBQWMsRUFBRSxRQUFRLEVBQUUsS0FBSyxFQUFFLFlBQVksRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDM0YsT0FBTyxFQUFFLEVBQUUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUMxQixPQUFPLEVBQUUsU0FBUyxFQUFFLEdBQUcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ2hELE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUN0QixPQUFPLEVBQUUsbUJBQW1CLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSwyQkFBMkIsQ0FBQztBQUNsRixPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFHekQsT0FBTyxFQUFFLCtCQUErQixFQUFFLE1BQU0sK0NBQStDLENBQUM7QUFDaEcsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3RELE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQzs7SUFzTzdDLHFCQUFvQix1QkFBd0QsRUFBVSxLQUFZO1FBQTlFLDRCQUF1QixHQUF2Qix1QkFBdUIsQ0FBaUM7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQztvQkFoTzNGLFdBQVc7Ozs7O0lBRWYsa0JBQU07Ozs7SUFBYixVQUFjLEtBQW1CO1FBQy9CLE9BQU8sS0FBSyxDQUFDO0lBQ2YsQ0FBQzs7Ozs7SUFHTSw4QkFBa0I7Ozs7SUFBekIsVUFBMEIsS0FBbUI7UUFDM0MsT0FBTyxLQUFLLENBQUMsV0FBVyxDQUFDLFdBQVcsSUFBSSxDQUFDLG1CQUFBLEVBQUUsRUFBc0IsQ0FBQyxDQUFDO0lBQ3JFLENBQUM7Ozs7O0lBRU0sa0JBQU07Ozs7SUFBYixVQUFjLEdBQVc7O1lBQ2pCLFFBQVEsR0FBRyxjQUFjLENBQzdCLENBQUMsYUFBVyxDQUFDOzs7O1FBQ2IsVUFBQyxLQUFtQjtZQUNsQixPQUFPLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQztRQUNwQixDQUFDLEVBQ0Y7UUFFRCxPQUFPLFFBQVEsQ0FBQztJQUNsQixDQUFDOzs7OztJQUVNLG1CQUFPOzs7O0lBQWQsVUFBZSxJQUF1QjtRQUNwQyxJQUFJLE9BQU8sSUFBSSxLQUFLLFFBQVEsRUFBRTtZQUM1QixJQUFJLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQztTQUN4QjtRQUVELElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLElBQUksQ0FBQyxFQUFFO1lBQ3hCLE1BQU0sSUFBSSxLQUFLLENBQUMsdURBQXVELENBQUMsQ0FBQztTQUMxRTs7WUFFSyxRQUFRLEdBQUcsY0FBYyxDQUM3QixDQUFDLGFBQVcsQ0FBQzs7OztRQUNiLFVBQUMsS0FBbUI7WUFDbEIsT0FBTyxDQUFDLG1CQUFBLElBQUksRUFBWSxDQUFDLENBQUMsTUFBTTs7Ozs7WUFBQyxVQUFDLEdBQUcsRUFBRSxHQUFHO2dCQUN4QyxJQUFJLEdBQUcsRUFBRTtvQkFDUCxPQUFPLEdBQUcsQ0FBQyxHQUFHLENBQUMsQ0FBQztpQkFDakI7Z0JBRUQsT0FBTyxTQUFTLENBQUM7WUFDbkIsQ0FBQyxHQUFFLEtBQUssQ0FBQyxDQUFDO1FBQ1osQ0FBQyxFQUNGO1FBRUQsT0FBTyxRQUFRLENBQUM7SUFDbEIsQ0FBQzs7Ozs7O0lBRU0sb0JBQVE7Ozs7O0lBQWYsVUFBZ0IsSUFBYSxFQUFFLElBQWE7O1lBQ3BDLFFBQVEsR0FBRyxjQUFjLENBQzdCLENBQUMsYUFBVyxDQUFDOzs7O1FBQ2IsVUFBQyxLQUFtQjtZQUNWLElBQUEsbUNBQWE7WUFDckIsT0FBTyxDQUFDLG1CQUFBLGFBQWEsRUFBbUIsQ0FBQyxDQUFDLElBQUk7Ozs7WUFBQyxVQUFBLEtBQUs7Z0JBQ2xELElBQUksSUFBSSxJQUFJLEtBQUssQ0FBQyxJQUFJLEtBQUssSUFBSSxFQUFFO29CQUMvQixPQUFPLEtBQUssQ0FBQztpQkFDZDtxQkFBTSxJQUFJLElBQUksSUFBSSxLQUFLLENBQUMsSUFBSSxLQUFLLElBQUksRUFBRTtvQkFDdEMsT0FBTyxLQUFLLENBQUM7aUJBQ2Q7WUFDSCxDQUFDLEVBQUMsQ0FBQztRQUNMLENBQUMsRUFDRjtRQUVELE9BQU8sUUFBUSxDQUFDO0lBQ2xCLENBQUM7Ozs7O0lBRU0scUJBQVM7Ozs7SUFBaEIsVUFBaUIsR0FBWTs7WUFDckIsUUFBUSxHQUFHLGNBQWMsQ0FDN0IsQ0FBQyxhQUFXLENBQUM7Ozs7UUFDYixVQUFDLEtBQW1CO1lBQ2xCLE9BQU8sS0FBSyxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsR0FBRyxJQUFJLFNBQVMsQ0FBQyxDQUFDLEdBQUcsQ0FBQztRQUN0RCxDQUFDLEVBQ0Y7UUFFRCxPQUFPLFFBQVEsQ0FBQztJQUNsQixDQUFDOzs7OztJQUVNLHNCQUFVOzs7O0lBQWpCLFVBQWtCLEdBQVc7O1lBQ3JCLFFBQVEsR0FBRyxjQUFjLENBQzdCLENBQUMsYUFBVyxDQUFDOzs7O1FBQ2IsVUFBQyxLQUFtQjtZQUNsQixPQUFPLEdBQUc7OztZQUFDLGNBQU0sT0FBQSxLQUFLLENBQUMsT0FBTyxDQUFDLE1BQU0sQ0FBQyxHQUFHLENBQUMsRUFBekIsQ0FBeUIsRUFBQyxDQUFDO1FBQzlDLENBQUMsRUFDRjtRQUNELE9BQU8sUUFBUSxDQUFDO0lBQ2xCLENBQUM7Ozs7O0lBRU0sdUJBQVc7Ozs7SUFBbEIsVUFBbUIsT0FBZ0I7O1lBQzNCLFFBQVEsR0FBRyxjQUFjLENBQzdCLENBQUMsYUFBVyxDQUFDOzs7O1FBQ2IsVUFBQyxLQUFtQjtZQUNsQixJQUFJLE9BQU8sRUFBRTs7b0JBQ0wsSUFBSSxHQUFHLEdBQUc7OztnQkFBQyxjQUFNLE9BQUEsTUFBTSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLE1BQU0sQ0FBQyxDQUFDLE1BQU07Ozs7Z0JBQUMsVUFBQSxHQUFHLElBQUksT0FBQSxHQUFHLENBQUMsT0FBTyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxFQUF6QixDQUF5QixFQUFDLEVBQTFFLENBQTBFLEdBQUUsRUFBRSxDQUFDO2dCQUV0RyxJQUFJLElBQUksQ0FBQyxNQUFNLEVBQUU7b0JBQ2YsT0FBTyxJQUFJLENBQUMsTUFBTTs7Ozs7b0JBQUMsVUFBQyxHQUFHLEVBQUUsR0FBRzs7d0JBQUssT0FBQSxzQkFBTSxHQUFHLGVBQUcsR0FBRyxJQUFHLEtBQUssQ0FBQyxPQUFPLENBQUMsTUFBTSxDQUFDLEdBQUcsQ0FBQyxPQUFHO29CQUE5QyxDQUE4QyxHQUFFLEVBQUUsQ0FBQyxDQUFDO2lCQUN0RjthQUNGO1lBRUQsT0FBTyxHQUFHOzs7WUFBQyxjQUFNLE9BQUEsS0FBSyxDQUFDLE9BQU8sQ0FBQyxNQUFNLEVBQXBCLENBQW9CLEdBQUUsRUFBRSxDQUFDLENBQUM7UUFDN0MsQ0FBQyxFQUNGO1FBQ0QsT0FBTyxRQUFRLENBQUM7SUFDbEIsQ0FBQzs7Ozs7SUFFTSw0QkFBZ0I7Ozs7SUFBdkIsVUFBd0IsR0FBVzs7WUFDM0IsUUFBUSxHQUFHLGNBQWMsQ0FDN0IsQ0FBQyxhQUFXLENBQUM7Ozs7UUFDYixVQUFDLEtBQW1CO1lBQ2xCLElBQUksQ0FBQyxHQUFHO2dCQUFFLE9BQU8sSUFBSSxDQUFDO1lBQ3RCLE9BQU8sR0FBRzs7O1lBQUMsY0FBTSxPQUFBLEtBQUssQ0FBQyxJQUFJLENBQUMsZUFBZSxDQUFDLEdBQUcsQ0FBQyxFQUEvQixDQUErQixHQUFFLEtBQUssQ0FBQyxDQUFDO1FBQzNELENBQUMsRUFDRjtRQUVELE9BQU8sUUFBUSxDQUFDO0lBQ2xCLENBQUM7SUFFRDs7O09BR0c7Ozs7Ozs7O0lBQ0ksbUJBQU87Ozs7Ozs7SUFBZCxVQUFlLEdBQVc7UUFBRSwyQkFBOEI7YUFBOUIsVUFBOEIsRUFBOUIscUJBQThCLEVBQTlCLElBQThCO1lBQTlCLDBDQUE4Qjs7UUFDeEQsSUFBSSxDQUFDLEdBQUc7WUFBRSxHQUFHLEdBQUcsRUFBRSxDQUFDOztZQUViLElBQUksR0FBRyxtQkFBQSxHQUFHLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxFQUFZOztZQUNsQyxRQUFRLEdBQUcsY0FBYyxDQUM3QixDQUFDLGFBQVcsQ0FBQzs7OztRQUNiLFVBQUMsS0FBbUI7WUFDbEIsSUFBSSxDQUFDLEtBQUssQ0FBQyxZQUFZO2dCQUFFLE9BQU8sR0FBRyxDQUFDO1lBRTVCLElBQUEsd0VBQW1CO1lBQzNCLElBQUksSUFBSSxDQUFDLENBQUMsQ0FBQyxLQUFLLEVBQUUsRUFBRTtnQkFDbEIsSUFBSSxDQUFDLG1CQUFtQixFQUFFO29CQUN4QixNQUFNLElBQUksS0FBSyxDQUNiLG1SQU1HLENBQ0osQ0FBQztpQkFDSDtnQkFFRCxJQUFJLENBQUMsQ0FBQyxDQUFDLEdBQUcsR0FBRzs7O2dCQUFDLGNBQU0sT0FBQSxtQkFBbUIsRUFBbkIsQ0FBbUIsRUFBQyxDQUFDO2FBQzFDOztnQkFFRyxJQUFJLEdBQUcsQ0FBQyxtQkFBQSxJQUFJLEVBQU8sQ0FBQyxDQUFDLE1BQU07Ozs7O1lBQUMsVUFBQyxHQUFHLEVBQUUsR0FBRztnQkFDdkMsSUFBSSxHQUFHLEVBQUU7b0JBQ1AsT0FBTyxHQUFHLENBQUMsR0FBRyxDQUFDLENBQUM7aUJBQ2pCO2dCQUVELE9BQU8sU0FBUyxDQUFDO1lBQ25CLENBQUMsR0FBRSxLQUFLLENBQUMsWUFBWSxDQUFDLE1BQU0sQ0FBQztZQUU3QixpQkFBaUIsR0FBRyxpQkFBaUIsQ0FBQyxNQUFNOzs7O1lBQUMsVUFBQSxNQUFNLElBQUksT0FBQSxNQUFNLElBQUksSUFBSSxFQUFkLENBQWMsRUFBQyxDQUFDO1lBQ3ZFLElBQUksSUFBSSxJQUFJLGlCQUFpQixJQUFJLGlCQUFpQixDQUFDLE1BQU0sRUFBRTtnQkFDekQsaUJBQWlCLENBQUMsT0FBTzs7OztnQkFBQyxVQUFBLEtBQUs7b0JBQzdCLElBQUksR0FBRyxJQUFJLENBQUMsT0FBTyxDQUFDLHlCQUF5QixFQUFFLEtBQUssQ0FBQyxDQUFDO2dCQUN4RCxDQUFDLEVBQUMsQ0FBQzthQUNKO1lBRUQsT0FBTyxJQUFJLElBQUksR0FBRyxDQUFDO1FBQ3JCLENBQUMsRUFDRjtRQUVELE9BQU8sUUFBUSxDQUFDO0lBQ2xCLENBQUM7Ozs7OztJQUVNLDJCQUFlOzs7OztJQUF0QixVQUF1QixHQUE0QztRQUFFLDJCQUE4QjthQUE5QixVQUE4QixFQUE5QixxQkFBOEIsRUFBOUIsSUFBOEI7WUFBOUIsMENBQThCOzs7WUFDN0YsWUFBb0I7UUFFeEIsSUFBSSxPQUFPLEdBQUcsS0FBSyxRQUFRLEVBQUU7WUFDM0IsWUFBWSxHQUFHLEdBQUcsQ0FBQyxZQUFZLENBQUM7WUFDaEMsR0FBRyxHQUFHLEdBQUcsQ0FBQyxHQUFHLENBQUM7U0FDZjtRQUVELElBQUksQ0FBQyxHQUFHO1lBQUUsR0FBRyxHQUFHLEVBQUUsQ0FBQzs7WUFFYixJQUFJLEdBQUcsbUJBQUEsR0FBRyxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsRUFBWTs7WUFDbEMsUUFBUSxHQUFHLGNBQWMsQ0FDN0IsQ0FBQyxhQUFXLENBQUM7Ozs7UUFDYixVQUFDLEtBQW1CO1lBQ2xCLElBQUksQ0FBQyxLQUFLLENBQUMsWUFBWTtnQkFBRSxPQUFPLFlBQVksSUFBSSxHQUFHLENBQUM7WUFFNUMsSUFBQSx3RUFBbUI7WUFDM0IsSUFBSSxJQUFJLENBQUMsQ0FBQyxDQUFDLEtBQUssRUFBRSxFQUFFO2dCQUNsQixJQUFJLENBQUMsbUJBQW1CLEVBQUU7b0JBQ3hCLE1BQU0sSUFBSSxLQUFLLENBQ2IsbVJBTUcsQ0FDSixDQUFDO2lCQUNIO2dCQUVELElBQUksQ0FBQyxDQUFDLENBQUMsR0FBRyxHQUFHOzs7Z0JBQUMsY0FBTSxPQUFBLG1CQUFtQixFQUFuQixDQUFtQixFQUFDLENBQUM7YUFDMUM7O2dCQUVHLFlBQVksR0FBRyxDQUFDLG1CQUFBLElBQUksRUFBTyxDQUFDLENBQUMsTUFBTTs7Ozs7WUFBQyxVQUFDLEdBQUcsRUFBRSxHQUFHO2dCQUMvQyxJQUFJLEdBQUcsRUFBRTtvQkFDUCxPQUFPLEdBQUcsQ0FBQyxHQUFHLENBQUMsQ0FBQztpQkFDakI7Z0JBRUQsT0FBTyxTQUFTLENBQUM7WUFDbkIsQ0FBQyxHQUFFLEtBQUssQ0FBQyxZQUFZLENBQUMsTUFBTSxDQUFDO1lBRTdCLGlCQUFpQixHQUFHLGlCQUFpQixDQUFDLE1BQU07Ozs7WUFBQyxVQUFBLE1BQU0sSUFBSSxPQUFBLE1BQU0sSUFBSSxJQUFJLEVBQWQsQ0FBYyxFQUFDLENBQUM7WUFDdkUsSUFBSSxZQUFZLElBQUksaUJBQWlCLElBQUksaUJBQWlCLENBQUMsTUFBTSxFQUFFO2dCQUNqRSxpQkFBaUIsQ0FBQyxPQUFPOzs7O2dCQUFDLFVBQUEsS0FBSztvQkFDN0IsWUFBWSxHQUFHLFlBQVksQ0FBQyxPQUFPLENBQUMseUJBQXlCLEVBQUUsS0FBSyxDQUFDLENBQUM7Z0JBQ3hFLENBQUMsRUFBQyxDQUFDO2FBQ0o7WUFFRCxJQUFJLE9BQU8sWUFBWSxLQUFLLFFBQVE7Z0JBQUUsWUFBWSxHQUFHLEVBQUUsQ0FBQztZQUN4RCxPQUFPLFlBQVksSUFBSSxZQUFZLElBQUksR0FBRyxDQUFDO1FBQzdDLENBQUMsRUFDRjtRQUVELE9BQU8sUUFBUSxDQUFDO0lBQ2xCLENBQUM7Ozs7O0lBS0QsNkJBQU87Ozs7SUFBUCxVQUFRLEVBQW9EO1FBRDVELGlCQWtCQztZQWpCUywwQkFBVSxFQUFFLHNCQUFRO1FBQzVCLE9BQU8sSUFBSSxDQUFDLHVCQUF1QixDQUFDLGdCQUFnQixFQUFFLENBQUMsSUFBSSxDQUN6RCxHQUFHOzs7O1FBQUMsVUFBQSxhQUFhO1lBQ2YsT0FBQSxVQUFVLHNCQUNMLGFBQWEsRUFDaEI7UUFGRixDQUVFLEVBQ0gsRUFDRCxTQUFTOzs7O1FBQUMsVUFBQSxhQUFhOztnQkFDakIsV0FBVyxHQUFXLGFBQWEsQ0FBQyxPQUFPLENBQUMsTUFBTSxDQUFDLGtDQUFrQyxDQUFDO1lBRTFGLElBQUksV0FBVyxDQUFDLFFBQVEsQ0FBQyxHQUFHLENBQUMsRUFBRTtnQkFDN0IsV0FBVyxHQUFHLFdBQVcsQ0FBQyxLQUFLLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7YUFDekM7WUFFRCxPQUFPLEtBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFlBQVksQ0FBQyxXQUFXLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBRSxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxRQUFRLENBQUMsSUFBSSxXQUFXLENBQUMsV0FBVyxDQUFDLENBQUMsQ0FBQztRQUNqSCxDQUFDLEVBQUMsQ0FDSCxDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QsZ0NBQVU7Ozs7O0lBQVYsVUFBVyxFQUFvRCxFQUFFLEVBQW9DO1lBQXhGLDBCQUFVLEVBQUUsc0JBQVE7WUFBa0MsY0FBSSxFQUFFLHNCQUFROztZQUMzRSxNQUFNLEdBQW9CLFFBQVEsRUFBRSxDQUFDLE1BQU07O1lBRXpDLEtBQUssR0FBRyxNQUFNLENBQUMsU0FBUzs7OztRQUFDLFVBQUEsS0FBSyxJQUFJLE9BQUEsS0FBSyxDQUFDLElBQUksS0FBSyxJQUFJLEVBQW5CLENBQW1CLEVBQUM7UUFFNUQsTUFBTSxHQUFHLGNBQWMsQ0FBQyxNQUFNLEVBQUUsSUFBSSxFQUFFLFFBQVEsQ0FBQyxDQUFDO1FBRWhELE9BQU8sVUFBVSxDQUFDO1lBQ2hCLE1BQU0sUUFBQTtTQUNQLENBQUMsQ0FBQztJQUNMLENBQUM7OztnQkFqQzRDLCtCQUErQjtnQkFBaUIsS0FBSzs7SUFHbEc7UUFEQyxNQUFNLENBQUMsbUJBQW1CLENBQUM7Ozs7OENBa0IzQjtJQUdEO1FBREMsTUFBTSxDQUFDLGdCQUFnQixDQUFDOzt5REFDNEQsZ0JBQWdCOztpREFVcEc7SUEvUEQ7UUFEQyxRQUFRLEVBQUU7Ozs7bUNBR1Y7SUFHRDtRQURDLFFBQVEsRUFBRTs7OzsrQ0FHVjtJQVRVLFdBQVc7UUFKdkIsS0FBSyxDQUFlO1lBQ25CLElBQUksRUFBRSxhQUFhO1lBQ25CLFFBQVEsRUFBRSxtQkFBQSxFQUFFLEVBQWdCO1NBQzdCLENBQUM7aURBaU82QywrQkFBK0IsRUFBaUIsS0FBSztPQWhPdkYsV0FBVyxDQWtRdkI7SUFBRCxrQkFBQztDQUFBLElBQUE7U0FsUVksV0FBVzs7Ozs7O0lBZ09WLDhDQUFnRTs7Ozs7SUFBRSw0QkFBb0I7Ozs7Ozs7OztBQW9DcEcsU0FBUyxjQUFjLENBQ3JCLE1BQXVCLEVBQ3ZCLElBQVksRUFDWixRQUFnQyxFQUNoQyxTQUF3QjtJQUF4QiwwQkFBQSxFQUFBLGdCQUF3QjtJQUV4QixNQUFNLEdBQUcsTUFBTSxDQUFDLEdBQUc7Ozs7SUFBQyxVQUFBLEtBQUs7UUFDdkIsSUFBSSxLQUFLLENBQUMsSUFBSSxLQUFLLElBQUksRUFBRTtZQUN2QixJQUFJLFFBQVEsQ0FBQyxJQUFJLEVBQUU7Z0JBQ2pCLFFBQVEsQ0FBQyxHQUFHLEdBQU0sU0FBUyxTQUFJLFFBQVEsQ0FBQyxJQUFNLENBQUM7YUFDaEQ7WUFFRCxJQUFJLFFBQVEsQ0FBQyxRQUFRLElBQUksUUFBUSxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7Z0JBQ2pELFFBQVEsQ0FBQyxRQUFRLEdBQUcsUUFBUSxDQUFDLFFBQVEsQ0FBQyxHQUFHOzs7O2dCQUFDLFVBQUEsS0FBSyxJQUFJLE9BQUEsc0JBQzlDLEtBQUssSUFDUixHQUFHLEVBQUssU0FBUyxTQUFJLEtBQUssQ0FBQyxJQUFJLFNBQUksS0FBSyxDQUFDLElBQU0sSUFDL0MsRUFIaUQsQ0FHakQsRUFBQyxDQUFDO2FBQ0w7WUFFRCw0QkFBWSxLQUFLLEVBQUssUUFBUSxFQUFHO1NBQ2xDO2FBQU0sSUFBSSxLQUFLLENBQUMsUUFBUSxJQUFJLEtBQUssQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO1lBQ2xELEtBQUssQ0FBQyxRQUFRLEdBQUcsY0FBYyxDQUFDLEtBQUssQ0FBQyxRQUFRLEVBQUUsSUFBSSxFQUFFLFFBQVEsRUFBRSxDQUFDLFNBQVMsSUFBSSxHQUFHLENBQUMsR0FBRyxLQUFLLENBQUMsSUFBSSxDQUFDLENBQUM7U0FDbEc7UUFFRCxPQUFPLEtBQUssQ0FBQztJQUNmLENBQUMsRUFBQyxDQUFDO0lBRUgsSUFBSSxTQUFTLEVBQUU7UUFDYixrQkFBa0I7UUFDbEIsT0FBTyxNQUFNLENBQUM7S0FDZjtJQUVELE9BQU8sY0FBYyxDQUFDLE1BQU0sQ0FBQyxDQUFDO0FBQ2hDLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBY3Rpb24sIGNyZWF0ZVNlbGVjdG9yLCBTZWxlY3RvciwgU3RhdGUsIFN0YXRlQ29udGV4dCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBvZiB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgc3dpdGNoTWFwLCB0YXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQgeyBHZXRBcHBDb25maWd1cmF0aW9uLCBQYXRjaFJvdXRlQnlOYW1lIH0gZnJvbSAnLi4vYWN0aW9ucy9jb25maWcuYWN0aW9ucyc7XG5pbXBvcnQgeyBTZXRMYW5ndWFnZSB9IGZyb20gJy4uL2FjdGlvbnMvc2Vzc2lvbi5hY3Rpb25zJztcbmltcG9ydCB7IEFCUCB9IGZyb20gJy4uL21vZGVscy9jb21tb24nO1xuaW1wb3J0IHsgQ29uZmlnIH0gZnJvbSAnLi4vbW9kZWxzL2NvbmZpZyc7XG5pbXBvcnQgeyBBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvYXBwbGljYXRpb24tY29uZmlndXJhdGlvbi5zZXJ2aWNlJztcbmltcG9ydCB7IG9yZ2FuaXplUm91dGVzIH0gZnJvbSAnLi4vdXRpbHMvcm91dGUtdXRpbHMnO1xuaW1wb3J0IHsgU2Vzc2lvblN0YXRlIH0gZnJvbSAnLi9zZXNzaW9uLnN0YXRlJztcblxuQFN0YXRlPENvbmZpZy5TdGF0ZT4oe1xuICBuYW1lOiAnQ29uZmlnU3RhdGUnLFxuICBkZWZhdWx0czoge30gYXMgQ29uZmlnLlN0YXRlLFxufSlcbmV4cG9ydCBjbGFzcyBDb25maWdTdGF0ZSB7XG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRBbGwoc3RhdGU6IENvbmZpZy5TdGF0ZSkge1xuICAgIHJldHVybiBzdGF0ZTtcbiAgfVxuXG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRBcHBsaWNhdGlvbkluZm8oc3RhdGU6IENvbmZpZy5TdGF0ZSk6IENvbmZpZy5BcHBsaWNhdGlvbiB7XG4gICAgcmV0dXJuIHN0YXRlLmVudmlyb25tZW50LmFwcGxpY2F0aW9uIHx8ICh7fSBhcyBDb25maWcuQXBwbGljYXRpb24pO1xuICB9XG5cbiAgc3RhdGljIGdldE9uZShrZXk6IHN0cmluZykge1xuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoXG4gICAgICBbQ29uZmlnU3RhdGVdLFxuICAgICAgKHN0YXRlOiBDb25maWcuU3RhdGUpID0+IHtcbiAgICAgICAgcmV0dXJuIHN0YXRlW2tleV07XG4gICAgICB9LFxuICAgICk7XG5cbiAgICByZXR1cm4gc2VsZWN0b3I7XG4gIH1cblxuICBzdGF0aWMgZ2V0RGVlcChrZXlzOiBzdHJpbmdbXSB8IHN0cmluZykge1xuICAgIGlmICh0eXBlb2Yga2V5cyA9PT0gJ3N0cmluZycpIHtcbiAgICAgIGtleXMgPSBrZXlzLnNwbGl0KCcuJyk7XG4gICAgfVxuXG4gICAgaWYgKCFBcnJheS5pc0FycmF5KGtleXMpKSB7XG4gICAgICB0aHJvdyBuZXcgRXJyb3IoJ1RoZSBhcmd1bWVudCBtdXN0IGJlIGEgZG90IHN0cmluZyBvciBhbiBzdHJpbmcgYXJyYXkuJyk7XG4gICAgfVxuXG4gICAgY29uc3Qgc2VsZWN0b3IgPSBjcmVhdGVTZWxlY3RvcihcbiAgICAgIFtDb25maWdTdGF0ZV0sXG4gICAgICAoc3RhdGU6IENvbmZpZy5TdGF0ZSkgPT4ge1xuICAgICAgICByZXR1cm4gKGtleXMgYXMgc3RyaW5nW10pLnJlZHVjZSgoYWNjLCB2YWwpID0+IHtcbiAgICAgICAgICBpZiAoYWNjKSB7XG4gICAgICAgICAgICByZXR1cm4gYWNjW3ZhbF07XG4gICAgICAgICAgfVxuXG4gICAgICAgICAgcmV0dXJuIHVuZGVmaW5lZDtcbiAgICAgICAgfSwgc3RhdGUpO1xuICAgICAgfSxcbiAgICApO1xuXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xuICB9XG5cbiAgc3RhdGljIGdldFJvdXRlKHBhdGg/OiBzdHJpbmcsIG5hbWU/OiBzdHJpbmcpIHtcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxuICAgICAgW0NvbmZpZ1N0YXRlXSxcbiAgICAgIChzdGF0ZTogQ29uZmlnLlN0YXRlKSA9PiB7XG4gICAgICAgIGNvbnN0IHsgZmxhdHRlZFJvdXRlcyB9ID0gc3RhdGU7XG4gICAgICAgIHJldHVybiAoZmxhdHRlZFJvdXRlcyBhcyBBQlAuRnVsbFJvdXRlW10pLmZpbmQocm91dGUgPT4ge1xuICAgICAgICAgIGlmIChwYXRoICYmIHJvdXRlLnBhdGggPT09IHBhdGgpIHtcbiAgICAgICAgICAgIHJldHVybiByb3V0ZTtcbiAgICAgICAgICB9IGVsc2UgaWYgKG5hbWUgJiYgcm91dGUubmFtZSA9PT0gbmFtZSkge1xuICAgICAgICAgICAgcmV0dXJuIHJvdXRlO1xuICAgICAgICAgIH1cbiAgICAgICAgfSk7XG4gICAgICB9LFxuICAgICk7XG5cbiAgICByZXR1cm4gc2VsZWN0b3I7XG4gIH1cblxuICBzdGF0aWMgZ2V0QXBpVXJsKGtleT86IHN0cmluZykge1xuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoXG4gICAgICBbQ29uZmlnU3RhdGVdLFxuICAgICAgKHN0YXRlOiBDb25maWcuU3RhdGUpOiBzdHJpbmcgPT4ge1xuICAgICAgICByZXR1cm4gc3RhdGUuZW52aXJvbm1lbnQuYXBpc1trZXkgfHwgJ2RlZmF1bHQnXS51cmw7XG4gICAgICB9LFxuICAgICk7XG5cbiAgICByZXR1cm4gc2VsZWN0b3I7XG4gIH1cblxuICBzdGF0aWMgZ2V0U2V0dGluZyhrZXk6IHN0cmluZykge1xuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoXG4gICAgICBbQ29uZmlnU3RhdGVdLFxuICAgICAgKHN0YXRlOiBDb25maWcuU3RhdGUpID0+IHtcbiAgICAgICAgcmV0dXJuIHNucSgoKSA9PiBzdGF0ZS5zZXR0aW5nLnZhbHVlc1trZXldKTtcbiAgICAgIH0sXG4gICAgKTtcbiAgICByZXR1cm4gc2VsZWN0b3I7XG4gIH1cblxuICBzdGF0aWMgZ2V0U2V0dGluZ3Moa2V5d29yZD86IHN0cmluZykge1xuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoXG4gICAgICBbQ29uZmlnU3RhdGVdLFxuICAgICAgKHN0YXRlOiBDb25maWcuU3RhdGUpID0+IHtcbiAgICAgICAgaWYgKGtleXdvcmQpIHtcbiAgICAgICAgICBjb25zdCBrZXlzID0gc25xKCgpID0+IE9iamVjdC5rZXlzKHN0YXRlLnNldHRpbmcudmFsdWVzKS5maWx0ZXIoa2V5ID0+IGtleS5pbmRleE9mKGtleXdvcmQpID4gLTEpLCBbXSk7XG5cbiAgICAgICAgICBpZiAoa2V5cy5sZW5ndGgpIHtcbiAgICAgICAgICAgIHJldHVybiBrZXlzLnJlZHVjZSgoYWNjLCBrZXkpID0+ICh7IC4uLmFjYywgW2tleV06IHN0YXRlLnNldHRpbmcudmFsdWVzW2tleV0gfSksIHt9KTtcbiAgICAgICAgICB9XG4gICAgICAgIH1cblxuICAgICAgICByZXR1cm4gc25xKCgpID0+IHN0YXRlLnNldHRpbmcudmFsdWVzLCB7fSk7XG4gICAgICB9LFxuICAgICk7XG4gICAgcmV0dXJuIHNlbGVjdG9yO1xuICB9XG5cbiAgc3RhdGljIGdldEdyYW50ZWRQb2xpY3koa2V5OiBzdHJpbmcpIHtcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxuICAgICAgW0NvbmZpZ1N0YXRlXSxcbiAgICAgIChzdGF0ZTogQ29uZmlnLlN0YXRlKTogYm9vbGVhbiA9PiB7XG4gICAgICAgIGlmICgha2V5KSByZXR1cm4gdHJ1ZTtcbiAgICAgICAgcmV0dXJuIHNucSgoKSA9PiBzdGF0ZS5hdXRoLmdyYW50ZWRQb2xpY2llc1trZXldLCBmYWxzZSk7XG4gICAgICB9LFxuICAgICk7XG5cbiAgICByZXR1cm4gc2VsZWN0b3I7XG4gIH1cblxuICAvKipcbiAgICpcbiAgICogQGRlcHJlY2F0ZWQsIFVzZSBnZXRMb2NhbGl6YXRpb24gaW5zdGVhZC4gVG8gYmUgZGVsZXRlIGluIHYxXG4gICAqL1xuICBzdGF0aWMgZ2V0Q29weShrZXk6IHN0cmluZywgLi4uaW50ZXJwb2xhdGVQYXJhbXM6IHN0cmluZ1tdKSB7XG4gICAgaWYgKCFrZXkpIGtleSA9ICcnO1xuXG4gICAgY29uc3Qga2V5cyA9IGtleS5zcGxpdCgnOjonKSBhcyBzdHJpbmdbXTtcbiAgICBjb25zdCBzZWxlY3RvciA9IGNyZWF0ZVNlbGVjdG9yKFxuICAgICAgW0NvbmZpZ1N0YXRlXSxcbiAgICAgIChzdGF0ZTogQ29uZmlnLlN0YXRlKSA9PiB7XG4gICAgICAgIGlmICghc3RhdGUubG9jYWxpemF0aW9uKSByZXR1cm4ga2V5O1xuXG4gICAgICAgIGNvbnN0IHsgZGVmYXVsdFJlc291cmNlTmFtZSB9ID0gc3RhdGUuZW52aXJvbm1lbnQubG9jYWxpemF0aW9uO1xuICAgICAgICBpZiAoa2V5c1swXSA9PT0gJycpIHtcbiAgICAgICAgICBpZiAoIWRlZmF1bHRSZXNvdXJjZU5hbWUpIHtcbiAgICAgICAgICAgIHRocm93IG5ldyBFcnJvcihcbiAgICAgICAgICAgICAgYFBsZWFzZSBjaGVjayB5b3VyIGVudmlyb25tZW50LiBNYXkgeW91IGZvcmdldCBzZXQgZGVmYXVsdFJlc291cmNlTmFtZT9cbiAgICAgICAgICAgICAgSGVyZSBpcyB0aGUgZXhhbXBsZTpcbiAgICAgICAgICAgICAgIHsgcHJvZHVjdGlvbjogZmFsc2UsXG4gICAgICAgICAgICAgICAgIGxvY2FsaXphdGlvbjoge1xuICAgICAgICAgICAgICAgICAgIGRlZmF1bHRSZXNvdXJjZU5hbWU6ICdNeVByb2plY3ROYW1lJ1xuICAgICAgICAgICAgICAgICAgfVxuICAgICAgICAgICAgICAgfWAsXG4gICAgICAgICAgICApO1xuICAgICAgICAgIH1cblxuICAgICAgICAgIGtleXNbMF0gPSBzbnEoKCkgPT4gZGVmYXVsdFJlc291cmNlTmFtZSk7XG4gICAgICAgIH1cblxuICAgICAgICBsZXQgY29weSA9IChrZXlzIGFzIGFueSkucmVkdWNlKChhY2MsIHZhbCkgPT4ge1xuICAgICAgICAgIGlmIChhY2MpIHtcbiAgICAgICAgICAgIHJldHVybiBhY2NbdmFsXTtcbiAgICAgICAgICB9XG5cbiAgICAgICAgICByZXR1cm4gdW5kZWZpbmVkO1xuICAgICAgICB9LCBzdGF0ZS5sb2NhbGl6YXRpb24udmFsdWVzKTtcblxuICAgICAgICBpbnRlcnBvbGF0ZVBhcmFtcyA9IGludGVycG9sYXRlUGFyYW1zLmZpbHRlcihwYXJhbXMgPT4gcGFyYW1zICE9IG51bGwpO1xuICAgICAgICBpZiAoY29weSAmJiBpbnRlcnBvbGF0ZVBhcmFtcyAmJiBpbnRlcnBvbGF0ZVBhcmFtcy5sZW5ndGgpIHtcbiAgICAgICAgICBpbnRlcnBvbGF0ZVBhcmFtcy5mb3JFYWNoKHBhcmFtID0+IHtcbiAgICAgICAgICAgIGNvcHkgPSBjb3B5LnJlcGxhY2UoL1tcXCdcXFwiXT9cXHtbXFxkXStcXH1bXFwnXFxcIl0/LywgcGFyYW0pO1xuICAgICAgICAgIH0pO1xuICAgICAgICB9XG5cbiAgICAgICAgcmV0dXJuIGNvcHkgfHwga2V5O1xuICAgICAgfSxcbiAgICApO1xuXG4gICAgcmV0dXJuIHNlbGVjdG9yO1xuICB9XG5cbiAgc3RhdGljIGdldExvY2FsaXphdGlvbihrZXk6IHN0cmluZyB8IENvbmZpZy5Mb2NhbGl6YXRpb25XaXRoRGVmYXVsdCwgLi4uaW50ZXJwb2xhdGVQYXJhbXM6IHN0cmluZ1tdKSB7XG4gICAgbGV0IGRlZmF1bHRWYWx1ZTogc3RyaW5nO1xuXG4gICAgaWYgKHR5cGVvZiBrZXkgIT09ICdzdHJpbmcnKSB7XG4gICAgICBkZWZhdWx0VmFsdWUgPSBrZXkuZGVmYXVsdFZhbHVlO1xuICAgICAga2V5ID0ga2V5LmtleTtcbiAgICB9XG5cbiAgICBpZiAoIWtleSkga2V5ID0gJyc7XG5cbiAgICBjb25zdCBrZXlzID0ga2V5LnNwbGl0KCc6OicpIGFzIHN0cmluZ1tdO1xuICAgIGNvbnN0IHNlbGVjdG9yID0gY3JlYXRlU2VsZWN0b3IoXG4gICAgICBbQ29uZmlnU3RhdGVdLFxuICAgICAgKHN0YXRlOiBDb25maWcuU3RhdGUpID0+IHtcbiAgICAgICAgaWYgKCFzdGF0ZS5sb2NhbGl6YXRpb24pIHJldHVybiBkZWZhdWx0VmFsdWUgfHwga2V5O1xuXG4gICAgICAgIGNvbnN0IHsgZGVmYXVsdFJlc291cmNlTmFtZSB9ID0gc3RhdGUuZW52aXJvbm1lbnQubG9jYWxpemF0aW9uO1xuICAgICAgICBpZiAoa2V5c1swXSA9PT0gJycpIHtcbiAgICAgICAgICBpZiAoIWRlZmF1bHRSZXNvdXJjZU5hbWUpIHtcbiAgICAgICAgICAgIHRocm93IG5ldyBFcnJvcihcbiAgICAgICAgICAgICAgYFBsZWFzZSBjaGVjayB5b3VyIGVudmlyb25tZW50LiBNYXkgeW91IGZvcmdldCBzZXQgZGVmYXVsdFJlc291cmNlTmFtZT9cbiAgICAgICAgICAgICAgSGVyZSBpcyB0aGUgZXhhbXBsZTpcbiAgICAgICAgICAgICAgIHsgcHJvZHVjdGlvbjogZmFsc2UsXG4gICAgICAgICAgICAgICAgIGxvY2FsaXphdGlvbjoge1xuICAgICAgICAgICAgICAgICAgIGRlZmF1bHRSZXNvdXJjZU5hbWU6ICdNeVByb2plY3ROYW1lJ1xuICAgICAgICAgICAgICAgICAgfVxuICAgICAgICAgICAgICAgfWAsXG4gICAgICAgICAgICApO1xuICAgICAgICAgIH1cblxuICAgICAgICAgIGtleXNbMF0gPSBzbnEoKCkgPT4gZGVmYXVsdFJlc291cmNlTmFtZSk7XG4gICAgICAgIH1cblxuICAgICAgICBsZXQgbG9jYWxpemF0aW9uID0gKGtleXMgYXMgYW55KS5yZWR1Y2UoKGFjYywgdmFsKSA9PiB7XG4gICAgICAgICAgaWYgKGFjYykge1xuICAgICAgICAgICAgcmV0dXJuIGFjY1t2YWxdO1xuICAgICAgICAgIH1cblxuICAgICAgICAgIHJldHVybiB1bmRlZmluZWQ7XG4gICAgICAgIH0sIHN0YXRlLmxvY2FsaXphdGlvbi52YWx1ZXMpO1xuXG4gICAgICAgIGludGVycG9sYXRlUGFyYW1zID0gaW50ZXJwb2xhdGVQYXJhbXMuZmlsdGVyKHBhcmFtcyA9PiBwYXJhbXMgIT0gbnVsbCk7XG4gICAgICAgIGlmIChsb2NhbGl6YXRpb24gJiYgaW50ZXJwb2xhdGVQYXJhbXMgJiYgaW50ZXJwb2xhdGVQYXJhbXMubGVuZ3RoKSB7XG4gICAgICAgICAgaW50ZXJwb2xhdGVQYXJhbXMuZm9yRWFjaChwYXJhbSA9PiB7XG4gICAgICAgICAgICBsb2NhbGl6YXRpb24gPSBsb2NhbGl6YXRpb24ucmVwbGFjZSgvW1xcJ1xcXCJdP1xce1tcXGRdK1xcfVtcXCdcXFwiXT8vLCBwYXJhbSk7XG4gICAgICAgICAgfSk7XG4gICAgICAgIH1cblxuICAgICAgICBpZiAodHlwZW9mIGxvY2FsaXphdGlvbiAhPT0gJ3N0cmluZycpIGxvY2FsaXphdGlvbiA9ICcnO1xuICAgICAgICByZXR1cm4gbG9jYWxpemF0aW9uIHx8IGRlZmF1bHRWYWx1ZSB8fCBrZXk7XG4gICAgICB9LFxuICAgICk7XG5cbiAgICByZXR1cm4gc2VsZWN0b3I7XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGFwcENvbmZpZ3VyYXRpb25TZXJ2aWNlOiBBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb25TZXJ2aWNlLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cblxuICBAQWN0aW9uKEdldEFwcENvbmZpZ3VyYXRpb24pXG4gIGFkZERhdGEoeyBwYXRjaFN0YXRlLCBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8Q29uZmlnLlN0YXRlPikge1xuICAgIHJldHVybiB0aGlzLmFwcENvbmZpZ3VyYXRpb25TZXJ2aWNlLmdldENvbmZpZ3VyYXRpb24oKS5waXBlKFxuICAgICAgdGFwKGNvbmZpZ3VyYXRpb24gPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgLi4uY29uZmlndXJhdGlvbixcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICAgc3dpdGNoTWFwKGNvbmZpZ3VyYXRpb24gPT4ge1xuICAgICAgICBsZXQgZGVmYXVsdExhbmc6IHN0cmluZyA9IGNvbmZpZ3VyYXRpb24uc2V0dGluZy52YWx1ZXNbJ0FicC5Mb2NhbGl6YXRpb24uRGVmYXVsdExhbmd1YWdlJ107XG5cbiAgICAgICAgaWYgKGRlZmF1bHRMYW5nLmluY2x1ZGVzKCc7JykpIHtcbiAgICAgICAgICBkZWZhdWx0TGFuZyA9IGRlZmF1bHRMYW5nLnNwbGl0KCc7JylbMF07XG4gICAgICAgIH1cblxuICAgICAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXNzaW9uU3RhdGUuZ2V0TGFuZ3VhZ2UpID8gb2YobnVsbCkgOiBkaXNwYXRjaChuZXcgU2V0TGFuZ3VhZ2UoZGVmYXVsdExhbmcpKTtcbiAgICAgIH0pLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKFBhdGNoUm91dGVCeU5hbWUpXG4gIHBhdGNoUm91dGUoeyBwYXRjaFN0YXRlLCBnZXRTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8Q29uZmlnLlN0YXRlPiwgeyBuYW1lLCBuZXdWYWx1ZSB9OiBQYXRjaFJvdXRlQnlOYW1lKSB7XG4gICAgbGV0IHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdID0gZ2V0U3RhdGUoKS5yb3V0ZXM7XG5cbiAgICBjb25zdCBpbmRleCA9IHJvdXRlcy5maW5kSW5kZXgocm91dGUgPT4gcm91dGUubmFtZSA9PT0gbmFtZSk7XG5cbiAgICByb3V0ZXMgPSBwYXRjaFJvdXRlRGVlcChyb3V0ZXMsIG5hbWUsIG5ld1ZhbHVlKTtcblxuICAgIHJldHVybiBwYXRjaFN0YXRlKHtcbiAgICAgIHJvdXRlcyxcbiAgICB9KTtcbiAgfVxufVxuXG5mdW5jdGlvbiBwYXRjaFJvdXRlRGVlcChcbiAgcm91dGVzOiBBQlAuRnVsbFJvdXRlW10sXG4gIG5hbWU6IHN0cmluZyxcbiAgbmV3VmFsdWU6IFBhcnRpYWw8QUJQLkZ1bGxSb3V0ZT4sXG4gIHBhcmVudFVybDogc3RyaW5nID0gbnVsbCxcbik6IEFCUC5GdWxsUm91dGVbXSB7XG4gIHJvdXRlcyA9IHJvdXRlcy5tYXAocm91dGUgPT4ge1xuICAgIGlmIChyb3V0ZS5uYW1lID09PSBuYW1lKSB7XG4gICAgICBpZiAobmV3VmFsdWUucGF0aCkge1xuICAgICAgICBuZXdWYWx1ZS51cmwgPSBgJHtwYXJlbnRVcmx9LyR7bmV3VmFsdWUucGF0aH1gO1xuICAgICAgfVxuXG4gICAgICBpZiAobmV3VmFsdWUuY2hpbGRyZW4gJiYgbmV3VmFsdWUuY2hpbGRyZW4ubGVuZ3RoKSB7XG4gICAgICAgIG5ld1ZhbHVlLmNoaWxkcmVuID0gbmV3VmFsdWUuY2hpbGRyZW4ubWFwKGNoaWxkID0+ICh7XG4gICAgICAgICAgLi4uY2hpbGQsXG4gICAgICAgICAgdXJsOiBgJHtwYXJlbnRVcmx9LyR7cm91dGUucGF0aH0vJHtjaGlsZC5wYXRofWAsXG4gICAgICAgIH0pKTtcbiAgICAgIH1cblxuICAgICAgcmV0dXJuIHsgLi4ucm91dGUsIC4uLm5ld1ZhbHVlIH07XG4gICAgfSBlbHNlIGlmIChyb3V0ZS5jaGlsZHJlbiAmJiByb3V0ZS5jaGlsZHJlbi5sZW5ndGgpIHtcbiAgICAgIHJvdXRlLmNoaWxkcmVuID0gcGF0Y2hSb3V0ZURlZXAocm91dGUuY2hpbGRyZW4sIG5hbWUsIG5ld1ZhbHVlLCAocGFyZW50VXJsIHx8ICcvJykgKyByb3V0ZS5wYXRoKTtcbiAgICB9XG5cbiAgICByZXR1cm4gcm91dGU7XG4gIH0pO1xuXG4gIGlmIChwYXJlbnRVcmwpIHtcbiAgICAvLyByZWN1cnNpdmUgYmxvY2tcbiAgICByZXR1cm4gcm91dGVzO1xuICB9XG5cbiAgcmV0dXJuIG9yZ2FuaXplUm91dGVzKHJvdXRlcyk7XG59XG4iXX0=
