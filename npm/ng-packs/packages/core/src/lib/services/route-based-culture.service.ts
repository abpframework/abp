import { isPlatformBrowser } from '@angular/common';
import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';
import { LanguageInfo } from '../proxy/volo/abp/localization/models';
import { findMatchingCultureName, getFirstPathSegment } from '../utils/route-based-culture.utils';
import { ConfigStateService } from './config-state.service';
import { SessionStateService } from './session-state.service';

/**
 * When the backend enables URL-based localization (`localization.useRouteBasedCulture` from application configuration),
 * keeps session language in sync with the first URL path segment (e.g. /en/..., /tr-TR/...).
 * Works with nested routes because only the leading segment is interpreted as culture.
 */
@Injectable({
  providedIn: 'root',
})
export class RouteBasedCultureService {
  private readonly router = inject(Router);
  private readonly configState = inject(ConfigStateService);
  private readonly sessionState = inject(SessionStateService);
  private readonly platformId = inject(PLATFORM_ID);
  protected readonly localization = this.configState.getOne('localization');

  constructor() {
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    if (!this.localization?.useRouteBasedCulture) {
      return;
    }

    this.router.events
      .pipe(filter((e): e is NavigationEnd => e instanceof NavigationEnd))
      .subscribe(() => this.syncLanguageFromUrl());
  }

  /**
   * Reads the culture from the current URL and updates session language when it matches a configured language.
   * @param pathOverride Optional path (e.g. from `Location.path()` during app bootstrap before navigation settles).
   */
  syncLanguageFromUrl(pathOverride?: string): void {
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    const languages = this.localization?.languages as LanguageInfo[] | undefined;
    const path = pathOverride ?? this.router.url;
    const firstSegment = getFirstPathSegment(path);
    const cultureName = findMatchingCultureName(firstSegment, languages);

    if (!cultureName) {
      return;
    }

    this.sessionState.setLanguage(cultureName);
  }
}
