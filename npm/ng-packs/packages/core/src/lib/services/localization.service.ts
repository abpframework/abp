import { registerLocaleData } from '@angular/common';
import { Injectable, Injector, NgZone, Optional, SkipSelf } from '@angular/core';
import { ActivatedRouteSnapshot, Router } from '@angular/router';
import { Actions, ofActionSuccessful, Store } from '@ngxs/store';
import { noop, Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { SetLanguage } from '../actions/session.actions';
import { ABP } from '../models/common';
import { Config } from '../models/config';
import { ConfigState } from '../states/config.state';
import { CORE_OPTIONS } from '../tokens/options.token';
import { createLocalizer, createLocalizerWithFallback } from '../utils/localization-utils';

type ShouldReuseRoute = (future: ActivatedRouteSnapshot, curr: ActivatedRouteSnapshot) => boolean;

@Injectable({ providedIn: 'root' })
export class LocalizationService {
  /**
   * Returns currently selected language
   */
  get currentLang(): string {
    return this.store.selectSnapshot(state => state.SessionState.language);
  }

  get languageChange(): Observable<SetLanguage> {
    return this.actions.pipe(
      ofActionSuccessful(SetLanguage),
      filter((action: SetLanguage) => action.dispatchAppConfiguration !== false),
    );
  }

  constructor(
    private actions: Actions,
    private store: Store,
    private injector: Injector,
    private ngZone: NgZone,
    @Optional()
    @SkipSelf()
    otherInstance: LocalizationService,
  ) {
    if (otherInstance) throw new Error('LocalizationService should have only one instance.');

    this.listenToSetLanguage();
  }

  private listenToSetLanguage() {
    this.languageChange.subscribe(({ payload }) => this.registerLocale(payload));
  }

  registerLocale(locale: string) {
    const router = this.injector.get(Router);
    const { shouldReuseRoute } = router.routeReuseStrategy;
    router.routeReuseStrategy.shouldReuseRoute = () => false;
    router.navigated = false;

    const { registerLocaleFn }: ABP.Root = this.injector.get(CORE_OPTIONS);

    return registerLocaleFn(locale).then(module => {
      if (module?.default) registerLocaleData(module.default);

      this.ngZone.run(async () => {
        await router.navigateByUrl(router.url).catch(noop);
        router.routeReuseStrategy.shouldReuseRoute = shouldReuseRoute;
      });
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
    return this.store.select(ConfigState.getLocalization(key, ...interpolateParams));
  }

  getResource(resourceName: string) {
    return this.store.select(ConfigState.getLocalizationResource(resourceName));
  }

  /**
   * Returns localized text with the given interpolation parameters in current language.
   * @param key Localization key to replace with localized text
   * @param interpolateParams Values to intepolate.
   */
  instant(key: string | Config.LocalizationWithDefault, ...interpolateParams: string[]): string {
    return this.store.selectSnapshot(ConfigState.getLocalization(key, ...interpolateParams));
  }

  localize(resourceName: string, key: string, defaultValue: string): Observable<string> {
    return this.store.select(ConfigState.getOne('localization')).pipe(
      map(createLocalizer),
      map(localize => localize(resourceName, key, defaultValue)),
    );
  }

  localizeSync(resourceName: string, key: string, defaultValue: string): string {
    const localization = this.store.selectSnapshot(ConfigState.getOne('localization'));
    return createLocalizer(localization)(resourceName, key, defaultValue);
  }

  localizeWithFallback(
    resourceNames: string[],
    keys: string[],
    defaultValue: string,
  ): Observable<string> {
    return this.store.select(ConfigState.getOne('localization')).pipe(
      map(createLocalizerWithFallback),
      map(localizeWithFallback => localizeWithFallback(resourceNames, keys, defaultValue)),
    );
  }

  localizeWithFallbackSync(resourceNames: string[], keys: string[], defaultValue: string): string {
    const localization = this.store.selectSnapshot(ConfigState.getOne('localization'));
    return createLocalizerWithFallback(localization)(resourceNames, keys, defaultValue);
  }
}
