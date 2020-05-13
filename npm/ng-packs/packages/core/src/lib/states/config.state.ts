import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Action, createSelector, Selector, State, StateContext, Store } from '@ngxs/store';
import { of, throwError } from 'rxjs';
import { catchError, switchMap, tap } from 'rxjs/operators';
import snq from 'snq';
import {
  AddRoute,
  GetAppConfiguration,
  PatchRouteByName,
  SetEnvironment,
} from '../actions/config.actions';
import { RestOccurError } from '../actions/rest.actions';
import { SetLanguage } from '../actions/session.actions';
import { ApplicationConfiguration } from '../models/application-configuration';
import { ABP } from '../models/common';
import { Config } from '../models/config';
import { organizeRoutes } from '../utils/route-utils';
import { SessionState } from './session.state';

@State<Config.State>({
  name: 'ConfigState',
  defaults: {} as Config.State,
})
@Injectable()
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
    const selector = createSelector([ConfigState], (state: Config.State) => {
      return state[key];
    });

    return selector;
  }

  static getDeep(keys: string[] | string) {
    if (typeof keys === 'string') {
      keys = keys.split('.');
    }

    if (!Array.isArray(keys)) {
      throw new Error('The argument must be a dot string or an string array.');
    }

    const selector = createSelector([ConfigState], (state: Config.State) => {
      return (keys as string[]).reduce((acc, val) => {
        if (acc) {
          return acc[val];
        }

        return undefined;
      }, state);
    });

    return selector;
  }

  static getRoute(path?: string, name?: string, url?: string) {
    const selector = createSelector([ConfigState], (state: Config.State) => {
      const { flattedRoutes } = state;
      return (flattedRoutes as ABP.FullRoute[]).find(route => {
        if (path && route.path === path) {
          return route;
        } else if (name && route.name === name) {
          return route;
        } else if (url && route.url === url) {
          return route;
        }
      });
    });

    return selector;
  }

  static getApiUrl(key?: string) {
    const selector = createSelector([ConfigState], (state: Config.State): string => {
      return (state.environment.apis[key || 'default'] || state.environment.apis.default).url;
    });

    return selector;
  }

  static getSetting(key: string) {
    const selector = createSelector([ConfigState], (state: Config.State) => {
      return snq(() => state.setting.values[key]);
    });

    return selector;
  }

  static getSettings(keyword?: string) {
    const selector = createSelector([ConfigState], (state: Config.State) => {
      const settings = snq(() => state.setting.values, {});

      if (!keyword) return settings;

      const keysFound = Object.keys(settings).filter(key => key.indexOf(keyword) > -1);

      return keysFound.reduce((acc, key) => {
        acc[key] = settings[key];
        return acc;
      }, {});
    });

    return selector;
  }

  static getGrantedPolicy(key: string) {
    const selector = createSelector([ConfigState], (state: Config.State): boolean => {
      if (!key) return true;
      const getPolicy = k => snq(() => state.auth.grantedPolicies[k], false);

      const orRegexp = /\|\|/g;
      const andRegexp = /&&/g;

      if (orRegexp.test(key)) {
        const keys = key.split('||').filter(k => !!k);

        if (keys.length !== 2) return false;

        return getPolicy(keys[0].trim()) || getPolicy(keys[1].trim());
      } else if (andRegexp.test(key)) {
        const keys = key.split('&&').filter(k => !!k);

        if (keys.length !== 2) return false;

        return getPolicy(keys[0].trim()) && getPolicy(keys[1].trim());
      }

      return getPolicy(key);
    });

    return selector;
  }

  static getLocalization(
    key: string | Config.LocalizationWithDefault,
    ...interpolateParams: string[]
  ) {
    if (!key) key = '';
    let defaultValue: string;

    if (typeof key !== 'string') {
      defaultValue = key.defaultValue;
      key = key.key;
    }

    const keys = key.split('::') as string[];
    const selector = createSelector([ConfigState], (state: Config.State): string => {
      const warn = (message: string) => {
        if (!state.environment.production) console.warn(message);
      };

      if (keys.length < 2) {
        warn('The localization source separator (::) not found.');
        return defaultValue || (key as string);
      }
      if (!state.localization) return defaultValue || keys[1];

      const sourceName =
        keys[0] ||
        snq(() => state.environment.localization.defaultResourceName) ||
        state.localization.defaultResourceName;
      const sourceKey = keys[1];

      if (sourceName === '_') {
        return defaultValue || sourceKey;
      }

      if (!sourceName) {
        warn(
          'Localization source name is not specified and the defaultResourceName was not defined!',
        );

        return defaultValue || sourceKey;
      }

      const source = state.localization.values[sourceName];
      if (!source) {
        warn('Could not find localization source: ' + sourceName);
        return defaultValue || sourceKey;
      }

      let localization = source[sourceKey];
      if (typeof localization === 'undefined') {
        return defaultValue || sourceKey;
      }

      interpolateParams = interpolateParams.filter(params => params != null);
      if (localization && interpolateParams && interpolateParams.length) {
        interpolateParams.forEach(param => {
          localization = localization.replace(/[\'\"]?\{[\d]+\}[\'\"]?/, param);
        });
      }

      if (typeof localization !== 'string') localization = '';
      return localization || defaultValue || (key as string);
    });

    return selector;
  }

  constructor(private http: HttpClient, private store: Store) {}

  @Action(GetAppConfiguration)
  addData({ patchState, dispatch }: StateContext<Config.State>) {
    const apiName = this.store.selectSnapshot(ConfigState.getDeep('environment.application.name'));
    const api = this.store.selectSnapshot(ConfigState.getApiUrl(apiName));
    return this.http
      .get<ApplicationConfiguration.Response>(`${api}/api/abp/application-configuration`)
      .pipe(
        tap(configuration =>
          patchState({
            ...configuration,
          }),
        ),
        switchMap(configuration => {
          let defaultLang: string =
            configuration.setting.values['Abp.Localization.DefaultLanguage'];

          if (defaultLang.includes(';')) {
            defaultLang = defaultLang.split(';')[0];
          }

          document.documentElement.setAttribute(
            'lang',
            configuration.localization.currentCulture.cultureName,
          );
          return this.store.selectSnapshot(SessionState.getLanguage)
            ? of(null)
            : dispatch(new SetLanguage(defaultLang, false));
        }),
        catchError(err => {
          dispatch(new RestOccurError(new HttpErrorResponse({ status: 0, error: err })));
          return throwError(err);
        }),
      );
  }

  @Action(PatchRouteByName)
  patchRoute(
    { patchState, getState }: StateContext<Config.State>,
    { name, newValue }: PatchRouteByName,
  ) {
    let routes: ABP.FullRoute[] = getState().routes;

    routes = patchRouteDeep(routes, name, newValue);

    const flattedRoutes = getState().flattedRoutes;
    const index = flattedRoutes.findIndex(route => route.name === name);

    if (index > -1) {
      flattedRoutes[index] = { ...flattedRoutes[index], ...newValue } as ABP.FullRoute;
    }

    return patchState({
      routes,
      flattedRoutes,
    });
  }

  @Action(AddRoute)
  addRoute({ patchState, getState }: StateContext<Config.State>, { payload }: AddRoute) {
    let routes: ABP.FullRoute[] = getState().routes;
    const flattedRoutes = getState().flattedRoutes;
    const route: ABP.FullRoute = { ...payload };

    if (route.parentName) {
      const index = flattedRoutes.findIndex(r => r.name === route.parentName);

      if (index < 0) return;

      const parent = flattedRoutes[index];
      if ((parent.url || '').replace('/', '')) {
        route.url = `${parent.url}/${route.path}`;
      } else {
        route.url = `/${route.path}`;
      }

      route.children = route.children || [];
      route.order = route.order || route.order === 0 ? route.order : (parent.children || []).length;
      parent.children = [...(parent.children || []), route].sort((a, b) => a.order - b.order);

      flattedRoutes[index] = parent;
      flattedRoutes.push(route);

      let parentName = parent.name;
      const parentNameArr = [parentName];

      while (parentName) {
        parentName = snq(() => flattedRoutes.find(r => r.name === parentName).parentName);

        if (parentName) {
          parentNameArr.unshift(parentName);
        }
      }

      routes = updateRouteDeep(routes, parentNameArr, parent);
    } else {
      route.url = `/${route.path}`;

      if (route.order || route.order === 0) {
        routes = [...routes, route].sort((a, b) => a.order - b.order);
      } else {
        route.order = routes.length;
        routes = [...routes, route];
      }

      flattedRoutes.push(route);
    }

    return patchState({
      routes,
      flattedRoutes,
    });
  }

  @Action(SetEnvironment)
  setEnvironment({ patchState }: StateContext<Config.State>, { environment }: SetEnvironment) {
    return patchState({
      environment,
    });
  }
}

function patchRouteDeep(
  routes: ABP.FullRoute[],
  name: string,
  newValue: Partial<ABP.FullRoute>,
  parentUrl: string = '',
): ABP.FullRoute[] {
  routes = routes.map(route => {
    if (route.name === name) {
      newValue.url = `${parentUrl}/${(!newValue.path || newValue.path === ''
        ? route.path
        : newValue.path) || ''}`;

      if (newValue.children && newValue.children.length) {
        newValue.children = newValue.children.map(child => ({
          ...child,
          url: `${newValue.url}/${child.path}`.replace('//', '/'),
        }));
      }

      return { ...route, ...newValue };
    } else if (route.children && route.children.length) {
      route.children = patchRouteDeep(
        route.children,
        name,
        newValue,
        (parentUrl || '/') + route.path,
      );
    }

    return route;
  });

  if (parentUrl) {
    // recursive block
    return routes;
  }

  return organizeRoutes(routes);
}

function updateRouteDeep(
  routes: ABP.FullRoute[],
  parentNameArr: string[],
  newValue: ABP.FullRoute,
  parentIndex = 0,
) {
  const index = routes.findIndex(route => route.name === parentNameArr[parentIndex]);

  if (parentIndex === parentNameArr.length - 1) {
    routes[index] = newValue;
  } else {
    routes[index].children = updateRouteDeep(
      routes[index].children,
      parentNameArr,
      newValue,
      parentIndex + 1,
    );
  }

  return routes;
}
