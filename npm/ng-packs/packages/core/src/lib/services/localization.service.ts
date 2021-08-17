import { registerLocaleData } from '@angular/common';
import { Injectable, Injector, isDevMode, Optional, SkipSelf } from '@angular/core';
import { from, Observable, Subject } from 'rxjs';
import { filter, map, mapTo, switchMap } from 'rxjs/operators';
import { ABP } from '../models/common';
import { Config } from '../models/config';
import { ApplicationConfigurationDto } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/models';
import { CORE_OPTIONS } from '../tokens/options.token';
import { createLocalizer, createLocalizerWithFallback } from '../utils/localization-utils';
import { interpolate } from '../utils/string-utils';
import { ConfigStateService } from './config-state.service';
import { SessionStateService } from './session-state.service';

@Injectable({ providedIn: 'root' })
export class LocalizationService {
  private latestLang = this.sessionState.getLanguage();
  private _languageChange$ = new Subject<string>();

  /**
   * Returns currently selected language
   */
  get currentLang(): string {
    return this.latestLang || this.sessionState.getLanguage();
  }

  get languageChange$(): Observable<string> {
    return this._languageChange$.asObservable();
  }

  constructor(
    private sessionState: SessionStateService,
    private injector: Injector,
    @Optional()
    @SkipSelf()
    otherInstance: LocalizationService,
    private configState: ConfigStateService,
  ) {
    if (otherInstance) throw new Error('LocalizationService should have only one instance.');

    this.listenToSetLanguage();
  }

  private listenToSetLanguage() {
    this.sessionState
      .onLanguageChange$()
      .pipe(
        filter(
          lang => this.configState.getDeep('localization.currentCulture.cultureName') !== lang,
        ),
        switchMap(lang => this.configState.refreshAppState().pipe(mapTo(lang))),
        switchMap(lang => from(this.registerLocale(lang).then(() => lang))),
      )
      .subscribe(lang => this._languageChange$.next(lang));
  }

  registerLocale(locale: string) {
    const { registerLocaleFn }: ABP.Root = this.injector.get(CORE_OPTIONS);

    return registerLocaleFn(locale).then(module => {
      if (module?.default) registerLocaleData(module.default);
      this.latestLang = locale;
    });
  }

  /**
   * Returns an observable localized text with the given interpolation parameters in current language.
   * @param key Localizaton key to replace with localized text
   * @param interpolateParams Values to interpolate
   */
  get(
    key: string | Config.LocalizationWithDefault,
    ...interpolateParams: string[]
  ): Observable<string> {
    return this.configState
      .getAll$()
      .pipe(map(state => getLocalization(state, key, ...interpolateParams)));
  }

  getResource(resourceName: string) {
    return this.configState.getDeep(`localization.values.${resourceName}`);
  }

  getResource$(resourceName: string) {
    return this.configState.getDeep$(`localization.values.${resourceName}`);
  }

  /**
   * Returns localized text with the given interpolation parameters in current language.
   * @param key Localization key to replace with localized text
   * @param interpolateParams Values to intepolate.
   */
  instant(key: string | Config.LocalizationWithDefault, ...interpolateParams: string[]): string {
    return getLocalization(this.configState.getAll(), key, ...interpolateParams);
  }

  localize(resourceName: string, key: string, defaultValue: string): Observable<string> {
    return this.configState.getOne$('localization').pipe(
      map(createLocalizer),
      map(localize => localize(resourceName, key, defaultValue)),
    );
  }

  localizeSync(resourceName: string, key: string, defaultValue: string): string {
    const localization = this.configState.getOne('localization');
    return createLocalizer(localization)(resourceName, key, defaultValue);
  }

  localizeWithFallback(
    resourceNames: string[],
    keys: string[],
    defaultValue: string,
  ): Observable<string> {
    return this.configState.getOne$('localization').pipe(
      map(createLocalizerWithFallback),
      map(localizeWithFallback => localizeWithFallback(resourceNames, keys, defaultValue)),
    );
  }

  localizeWithFallbackSync(resourceNames: string[], keys: string[], defaultValue: string): string {
    const localization = this.configState.getOne('localization');
    return createLocalizerWithFallback(localization)(resourceNames, keys, defaultValue);
  }
}

function getLocalization(
  state: ApplicationConfigurationDto,
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
  const warn = (message: string) => {
    if (isDevMode) console.warn(message);
  };

  if (keys.length < 2) {
    warn('The localization source separator (::) not found.');
    return defaultValue || (key as string);
  }
  if (!state.localization) return defaultValue || keys[1];

  const sourceName = keys[0] || state.localization.defaultResourceName;
  const sourceKey = keys[1];

  if (sourceName === '_') {
    return defaultValue || sourceKey;
  }

  if (!sourceName) {
    warn('Localization source name is not specified and the defaultResourceName was not defined!');

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
}
