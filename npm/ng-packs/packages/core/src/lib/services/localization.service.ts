import { Injectable, NgZone, Optional, SkipSelf } from '@angular/core';
import { ActivatedRouteSnapshot, Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { noop, Observable } from 'rxjs';
import { ConfigState } from '../states/config.state';
import { registerLocale } from '../utils/initial-utils';
import { Config } from '../models/config';

type ShouldReuseRoute = (future: ActivatedRouteSnapshot, curr: ActivatedRouteSnapshot) => boolean;

@Injectable({ providedIn: 'root' })
export class LocalizationService {
  get currentLang(): string {
    return this.store.selectSnapshot(state => state.SessionState.language);
  }

  constructor(
    private store: Store,
    private router: Router,
    private ngZone: NgZone,
    @Optional()
    @SkipSelf()
    otherInstance: LocalizationService,
  ) {
    if (otherInstance) throw new Error('LocalizationService should have only one instance.');
  }

  setRouteReuse(reuse: ShouldReuseRoute) {
    this.router.routeReuseStrategy.shouldReuseRoute = reuse;
  }

  registerLocale(locale: string) {
    const { shouldReuseRoute } = this.router.routeReuseStrategy;
    this.setRouteReuse(() => false);
    this.router.navigated = false;

    return registerLocale(locale).then(() => {
      this.ngZone.run(async () => {
        await this.router.navigateByUrl(this.router.url).catch(noop);
        this.setRouteReuse(shouldReuseRoute);
      });
    });
  }

  get(
    key: string | Config.LocalizationWithDefault,
    ...interpolateParams: string[]
  ): Observable<string> {
    return this.store.select(ConfigState.getLocalization(key, ...interpolateParams));
  }

  instant(key: string | Config.LocalizationWithDefault, ...interpolateParams: string[]): string {
    return this.store.selectSnapshot(ConfigState.getLocalization(key, ...interpolateParams));
  }
}
