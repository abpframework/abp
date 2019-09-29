import { State, Selector, createSelector, Action, StateContext, Store } from '@ngxs/store';
import { Config, ABP } from '../models';
import { GetAppConfiguration, PatchRouteByName } from '../actions/config.actions';
import { ApplicationConfigurationService } from '../services/application-configuration.service';
import { tap, switchMap } from 'rxjs/operators';
import snq from 'snq';
import { SetLanguage } from '../actions';
import { SessionState } from './session.state';
import { of } from 'rxjs';
import { setChildRoute, sortRoutes, organizeRoutes } from '../utils/route-utils';

@State<Config.State>({
  name: 'ConfigState',
  defaults: {} as Config.State,
})
export class ConfigState {
  @Selector()
  static getAll(state: Config.State) {
    return state;
  }

  @Selector()
  static getApplicationInfo(state: Config.State): Config.Application {
    return state.environment.application || ({} as Config.Application);
  }

  static getOne(key: string) {
    const selector = createSelector(
      [ConfigState],
      function(state: Config.State) {
        return state[key];
      },
    );

    return selector;
  }

  static getDeep(keys: string[] | string) {
    if (typeof keys === 'string') {
      keys = keys.split('.');
    }

    if (!Array.isArray(keys)) {
      throw new Error('The argument must be a dot string or an string array.');
    }

    const selector = createSelector(
      [ConfigState],
      function(state: Config.State) {
        return (keys as string[]).reduce((acc, val) => {
          if (acc) {
            return acc[val];
          }

          return undefined;
        }, state);
      },
    );

    return selector;
  }

  static getRoute(path?: string, name?: string) {
    const selector = createSelector(
      [ConfigState],
      function(state: Config.State) {
        const { flattedRoutes } = state;
        return (flattedRoutes as ABP.FullRoute[]).find(route => {
          if (path && route.path === path) {
            return route;
          } else if (name && route.name === name) {
            return route;
          }
        });
      },
    );

    return selector;
  }

  static getApiUrl(key?: string) {
    const selector = createSelector(
      [ConfigState],
      function(state: Config.State): string {
        return state.environment.apis[key || 'default'].url;
      },
    );

    return selector;
  }

  static getSetting(key: string) {
    const selector = createSelector(
      [ConfigState],
      function(state: Config.State) {
        return snq(() => state.setting.values[key]);
      },
    );

    return selector;
  }

  static getGrantedPolicy(key: string) {
    const selector = createSelector(
      [ConfigState],
      function(state: Config.State): boolean {
        if (!key) return true;
        return snq(() => state.auth.grantedPolicies[key], false);
      },
    );

    return selector;
  }

  static getCopy(key: string, ...interpolateParams: string[]) {
    if (!key) key = '';

    const keys = key.split('::') as string[];
    const selector = createSelector(
      [ConfigState],
      function(state: Config.State) {
        if (!state.localization) return key;

        const { defaultResourceName } = state.environment.localization;
        if (keys[0] === '') {
          if (!defaultResourceName) {
            throw new Error(
              `Please check your environment. May you forget set defaultResourceName? 
              Here is the example:
               { production: false,
                 localization: {
                   defaultResourceName: 'MyProjectName'
                  }
               }`,
            );
          }

          keys[0] = snq(() => defaultResourceName);
        }

        let copy = (keys as any).reduce((acc, val) => {
          if (acc) {
            return acc[val];
          }

          return undefined;
        }, state.localization.values);

        interpolateParams = interpolateParams.filter(params => params != null);
        if (copy && interpolateParams && interpolateParams.length) {
          interpolateParams.forEach(param => {
            copy = copy.replace(/[\'\"]?\{[\d]+\}[\'\"]?/, param);
          });
        }

        return copy || key;
      },
    );

    return selector;
  }

  constructor(private appConfigurationService: ApplicationConfigurationService, private store: Store) {}

  @Action(GetAppConfiguration)
  addData({ patchState, dispatch }: StateContext<Config.State>) {
    return this.appConfigurationService.getConfiguration().pipe(
      tap(configuration =>
        patchState({
          ...configuration,
        }),
      ),
      switchMap(configuration => {
        let defaultLang: string = configuration.setting.values['Abp.Localization.DefaultLanguage'];

        if (defaultLang.includes(';')) {
          defaultLang = defaultLang.split(';')[0];
        }

        return this.store.selectSnapshot(SessionState.getLanguage) ? of(null) : dispatch(new SetLanguage(defaultLang));
      }),
    );
  }

  @Action(PatchRouteByName)
  patchRoute({ patchState, getState }: StateContext<Config.State>, { name, newValue }: PatchRouteByName) {
    let routes: ABP.FullRoute[] = getState().routes;

    const index = routes.findIndex(route => route.name === name);

    routes = patchRouteDeep(routes, name, newValue);

    return patchState({
      routes,
    });
  }
}

function patchRouteDeep(
  routes: ABP.FullRoute[],
  name: string,
  newValue: Partial<ABP.FullRoute>,
  parentUrl: string = null,
): ABP.FullRoute[] {
  routes = routes.map(route => {
    if (route.name === name) {
      if (newValue.path) {
        newValue.url = `${parentUrl}/${newValue.path}`;
      }

      if (newValue.children && newValue.children.length) {
        newValue.children = newValue.children.map(child => ({
          ...child,
          url: `${parentUrl}/${route.path}/${child.path}`,
        }));
      }

      return { ...route, ...newValue };
    } else if (route.children && route.children.length) {
      route.children = patchRouteDeep(route.children, name, newValue, (parentUrl || '/') + route.path);
    }

    return route;
  });

  if (parentUrl) {
    // recursive block
    return routes;
  }

  return organizeRoutes(routes);
}
