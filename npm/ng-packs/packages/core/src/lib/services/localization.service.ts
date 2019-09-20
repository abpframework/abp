import { Injectable, Optional, SkipSelf, NgZone } from '@angular/core';
import { ActivatedRouteSnapshot, Router } from '@angular/router';
import { Actions, Store } from '@ngxs/store';
import { noop, Observable } from 'rxjs';
import { ConfigState } from '../states/config.state';
import { SessionState } from '../states/session.state';
import { registerLocale } from '../utils/initial-utils';

type ShouldReuseRoute = (future: ActivatedRouteSnapshot, curr: ActivatedRouteSnapshot) => boolean;

@Injectable({ providedIn: 'root' })
export class LocalizationService {
  get currentLang(): string {
    return this.store.selectSnapshot(SessionState.getLanguage);
  }

  constructor(
    private store: Store,
    private router: Router,
    private ngZone: NgZone,
    private actions: Actions,
    @Optional()
    @SkipSelf()
    otherInstance: LocalizationService,
  ) {
    if (otherInstance) throw new Error('LocaleService should have only one instance.');
  }

  private setRouteReuse(reuse: ShouldReuseRoute) {
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

  get(keys: string, ...interpolateParams: string[]): Observable<string> {
    return this.store.select(ConfigState.getCopy(keys, ...interpolateParams));
  }

  instant(keys: string, ...interpolateParams: string[]): string {
    return this.store.selectSnapshot(ConfigState.getCopy(keys, ...interpolateParams));
  }
}
