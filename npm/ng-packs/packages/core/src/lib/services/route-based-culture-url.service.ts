import { Location } from '@angular/common';
import { isPlatformBrowser } from '@angular/common';
import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { Router } from '@angular/router';
import { LanguageInfo } from '../proxy/volo/abp/localization/models';
import {
  findMatchingCultureName,
  getFirstPathSegment,
  stripCultureSegmentFromPath,
} from '../utils/route-based-culture.utils';
import { getRoutePath } from '../utils/route-utils';
import { ConfigStateService } from './config-state.service';
import { SessionStateService } from './session-state.service';

/**
 * URL helpers for route-based culture: prefix menu links, strip culture for route matching,
 * and navigate when the user picks a language so the URL stays in sync with the session.
 */
@Injectable({
  providedIn: 'root',
})
export class RouteBasedCultureUrlService {
  private readonly router = inject(Router);
  private readonly location = inject(Location);
  private readonly configState = inject(ConfigStateService);
  private readonly sessionState = inject(SessionStateService);
  private readonly platformId = inject(PLATFORM_ID);

  /** Cached from localization config; refreshed when application configuration updates. */
  private useRouteBasedCulture = false;
  private languages: LanguageInfo[] | undefined;

  constructor() {
    this.refreshRouteCultureCache();
    this.configState.getAll$().subscribe(() => this.refreshRouteCultureCache());
  }

  private refreshRouteCultureCache(): void {
    const loc = this.configState.getOne('localization');
    if (!loc) {
      this.useRouteBasedCulture = false;
      this.languages = undefined;
      return;
    }
    this.useRouteBasedCulture = !!loc.useRouteBasedCulture;
    this.languages = loc.languages as LanguageInfo[] | undefined;
  }

  private isRouteBasedCultureEnabled(): boolean {
    return this.useRouteBasedCulture;
  }

  /**
   * Same as {@link getRoutePath} but removes the leading culture segment when route-based culture is enabled,
   * so paths match `RoutesService` entries (e.g. `/identity/users`).
   */
  getRoutePathForMatching(router: Router, url = router.url): string {
    const raw = getRoutePath(router, url);
    return this.stripCulturePrefixIfEnabled(raw);
  }

  /**
   * Strips the leading culture segment when it matches a configured language and route-based culture is enabled.
   * Use for menu active state, breadcrumbs, and any comparison between `router.url` and `RoutesService` paths.
   */
  stripCulturePrefixIfEnabled(path: string): string {
    if (!this.isRouteBasedCultureEnabled() || !path) {
      return path;
    }
    return this.stripCulturePrefix(path);
  }

  /**
   * Alias for {@link stripCulturePrefixIfEnabled}: normalizes the current URL for **menu highlighting**
   * when menu `link` values omit the culture segment (e.g. Lepton `NavbarRoutesComponent`, `NavbarService.getRouteItem`).
   */
  normalizeForMenuMatch(path: string): string {
    return this.stripCulturePrefixIfEnabled(path);
  }

  /**
   * Removes the first segment when it is a known UI culture (for matching and normalization).
   */
  stripCulturePrefix(path: string): string {
    return stripCultureSegmentFromPath(path, this.languages);
  }

  /**
   * Prefixes an app path with the current session culture when route-based culture is enabled
   * (e.g. `/identity/users` → `/en/identity/users`). Use for `routerLink`, `navigateByUrl`, etc.
   */
  prefixPathWithCulture(path: string | undefined | null): string | undefined | null {
    if (path == null || path === '') {
      return path;
    }

    if (!this.isRouteBasedCultureEnabled()) {
      return path;
    }

    if (/^https?:\/\//i.test(path)) {
      return path;
    }

    const lang = this.sessionState.getLanguage();
    if (!lang) {
      return path;
    }

    const stripped = this.stripCulturePrefix(path);
    const normalized = stripped.startsWith('/') ? stripped : '/' + stripped;
    const suffix = normalized === '/' ? '' : normalized;
    return `/${lang}${suffix}`;
  }

  /**
   * Rewrites the current URL so the first segment is {@link cultureName} (or prepends it).
   * Call this when the user selects a language in the UI instead of only {@link SessionStateService.setLanguage}
   * so the address bar stays aligned with the session.
   */
  navigateToUrlWithCulture(cultureName: string): Promise<boolean> | void {
    if (!cultureName || !isPlatformBrowser(this.platformId)) {
      return Promise.resolve(false);
    }

    if (!this.isRouteBasedCultureEnabled()) {
      this.sessionState.setLanguage(cultureName);
      return Promise.resolve(true);
    }

    const path = this.location.path();
    const newPath = this.rewritePathToCulture(path, cultureName);
    return this.router.navigateByUrl(newPath);
  }

  /**
   * Preferred entry point for language pickers (e.g. Lepton toolbar): same as {@link navigateToUrlWithCulture}.
   */
  applyLanguageSelection(cultureName: string): Promise<boolean> | void {
    return this.navigateToUrlWithCulture(cultureName);
  }

  /**
   * Builds a new path (including query/hash) with the given culture as the first segment,
   * replacing the culture segment when one is already present.
   */
  rewritePathToCulture(urlPath: string, newCulture: string): string {
    const languages = this.languages;
    const pathEnd = urlPath.search(/[?#]/);
    const pathOnly = pathEnd >= 0 ? urlPath.slice(0, pathEnd) : urlPath;
    const suffix = pathEnd >= 0 ? urlPath.slice(pathEnd) : '';

    const first = getFirstPathSegment(pathOnly);
    const firstIsCulture = !!findMatchingCultureName(first, languages);

    if (firstIsCulture) {
      const segments = pathOnly.split('/').filter(s => s.length > 0);
      if (segments.length === 0) {
        return `/${newCulture}${suffix}`;
      }
      segments[0] = newCulture;
      const joined = '/' + segments.join('/');
      return joined + suffix;
    }

    const normalized = pathOnly.startsWith('/') ? pathOnly : '/' + pathOnly;
    const rest = normalized === '/' ? '' : normalized;
    return `/${newCulture}${rest}${suffix}`;
  }
}
