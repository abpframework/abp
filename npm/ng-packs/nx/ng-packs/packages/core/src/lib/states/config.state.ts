import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Action, createSelector, Selector, State, StateContext, Store } from '@ngxs/store';
import { of, throwError } from 'rxjs';
import { catchError, distinctUntilChanged, switchMap, tap } from 'rxjs/operators';
import snq from 'snq';
import { GetAppConfiguration, PatchConfigState, SetEnvironment } from '../actions/config.actions';
import { RestOccurError } from '../actions/rest.actions';
import { ApplicationConfiguration } from '../models/application-configuration';
import { Config } from '../models/config';
import { ConfigStateService } from '../services/config-state.service';
import { EnvironmentService } from '../services/environment.service';
import { SessionStateService } from '../services/session-state.service';
import { interpolate } from '../utils/string-utils';
import compare from 'just-compare';
import { ApplicationConfigurationDto } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/models';

/**
 * @deprecated Use ConfigStateService instead. To be deleted in v5.0.
 */
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

  /**
   * @deprecated use PermissionService's getGrantedPolicyStream or getGrantedPolicy methods.
   */
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
    const selector = createSelector(
      [ConfigState],
      (
        state: Config.State,
      ): {
        [key: string]: string;
      } => {
        return state.localization.values[resourceName];
      },
    );

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
      if (localization) localization = interpolate(localization, interpolateParams);

      if (typeof localization !== 'string') localization = '';

      return localization || defaultValue || (key as string);
    });

    return selector;
  }

  constructor(
    private http: HttpClient,
    private store: Store,
    private sessionState: SessionStateService,
    private environmentService: EnvironmentService,
    private configState: ConfigStateService,
  ) {
    this.syncConfigState();
    this.syncEnvironment();
  }

  private syncConfigState() {
    this.configState
      .createOnUpdateStream(state => state)
      .pipe(distinctUntilChanged(compare))
      .subscribe(config => this.store.dispatch(new PatchConfigState(config as any)));
  }

  private syncEnvironment() {
    this.environmentService
      .createOnUpdateStream(state => state)
      .pipe(distinctUntilChanged(compare))
      .subscribe(env => this.store.dispatch(new PatchConfigState({ environment: env } as any)));
  }

  @Action(GetAppConfiguration)
  addData({ patchState, dispatch }: StateContext<Config.State>) {
    const apiName = 'default';
    const api = this.store.selectSnapshot(ConfigState.getApiUrl(apiName));
    return this.http
      .get<ApplicationConfigurationDto>(`${api}/api/abp/application-configuration`)
      .pipe(
        tap(configuration => this.configState.setState(configuration)),
        catchError((err: HttpErrorResponse) => {
          dispatch(new RestOccurError(err));
          return throwError(err);
        }),
      );
  }

  @Action(SetEnvironment)
  setEnvironment(_, { environment }: SetEnvironment) {
    return this.environmentService.setState(environment);
  }

  @Action(PatchConfigState)
  setConfig({ patchState, getState }: StateContext<Config.State>, { state }: PatchConfigState) {
    patchState({ ...getState(), ...state });
  }
}
