import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Action, createSelector, Selector, State, StateContext, Store } from '@ngxs/store';
import { of, throwError } from 'rxjs';
import { catchError, switchMap, tap } from 'rxjs/operators';
import snq from 'snq';
import { GetAppConfiguration, SetEnvironment } from '../actions/config.actions';
import { RestOccurError } from '../actions/rest.actions';
import { SetLanguage } from '../actions/session.actions';
import { ApplicationConfiguration } from '../models/application-configuration';
import { Config } from '../models/config';
import { interpolate } from '../utils/string-utils';
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

  @Selector()
  static getEnvironment(state: Config.State): Config.Environment {
    return state.environment;
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

  static getApiUrl(key?: string) {
    const selector = createSelector([ConfigState], (state: Config.State): string => {
      return (state.environment.apis[key || 'default'] || state.environment.apis.default).url;
    });

    return selector;
  }

  static getFeature(key: string) {
    const selector = createSelector([ConfigState], (state: Config.State) => {
      return snq(() => state.features.values[key]);
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
      const getPolicy = (k: string) => snq(() => state.auth.grantedPolicies[k], false);

      const orRegexp = /\|\|/g;
      const andRegexp = /&&/g;

      // TODO: Allow combination of ANDs & ORs
      if (orRegexp.test(key)) {
        const keys = key.split('||').filter(Boolean);

        if (keys.length < 2) return false;

        return keys.some(k => getPolicy(k.trim()));
      } else if (andRegexp.test(key)) {
        const keys = key.split('&&').filter(Boolean);

        if (keys.length < 2) return false;

        return keys.every(k => getPolicy(k.trim()));
      }

      return getPolicy(key);
    });

    return selector;
  }

  static getLocalizationResource(resourceName: string) {
    const selector = createSelector([ConfigState], (state: Config.State): {
      [key: string]: string;
    } => {
      return state.localization.values[resourceName];
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

      // [TODO]: next line should be removed in v3.2, breaking change!!!
      interpolateParams = interpolateParams.filter(params => params != null);
      if (localization) localization = interpolate(localization, interpolateParams);

      if (typeof localization !== 'string') localization = '';

      return localization || defaultValue || (key as string);
    });

    return selector;
  }

  constructor(private http: HttpClient, private store: Store) {}

  @Action(GetAppConfiguration)
  addData({ patchState, dispatch }: StateContext<Config.State>) {
    const apiName = 'default';
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
          let lang = configuration.localization.currentCulture.cultureName;

          if (lang.includes(';')) {
            lang = lang.split(';')[0];
          }

          document.documentElement.setAttribute('lang', lang);

          return this.store.selectSnapshot(SessionState.getLanguage)
            ? of(null)
            : dispatch(new SetLanguage(lang, false));
        }),
        catchError((err: HttpErrorResponse) => {
          dispatch(new RestOccurError(err));
          return throwError(err);
        }),
      );
  }

  @Action(SetEnvironment)
  setEnvironment({ patchState }: StateContext<Config.State>, { environment }: SetEnvironment) {
    return patchState({ environment });
  }
}
