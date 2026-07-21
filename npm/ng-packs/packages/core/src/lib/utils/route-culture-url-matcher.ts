import { Route, Routes, UrlMatchResult, UrlSegment, UrlSegmentGroup } from '@angular/router';

/**
 * Heuristic: first path segment looks like a BCP 47-style culture code (e.g. en, tr, zh-Hans, pt-BR).
 * Used by the optional culture URL matcher so real routes like `identity` or `cms-kit` are not mistaken for cultures.
 */
export function isLikelyCultureSegment(segment: string): boolean {
  if (!segment) {
    return false;
  }

  return /^[a-z]{2}(-[a-zA-Z0-9]{2,8})?$/.test(segment);
}

/**
 * Matcher that consumes the first segment when it looks like a culture code.
 * Exposes it as the route param `culture` on matched routes.
 */
export function createRouteCultureUrlMatcher(): (
  segments: UrlSegment[],
  group: UrlSegmentGroup,
  route: Route,
) => UrlMatchResult | null {
  return (segments: UrlSegment[]) => {
    if (segments.length < 1) {
      return null;
    }

    const first = segments[0].path;
    if (!isLikelyCultureSegment(first)) {
      return null;
    }

    return {
      consumed: [segments[0]],
      posParams: { culture: segments[0] },
    };
  };
}

/**
 * Wraps your app routes so the same URLs work with or without a leading culture segment.
 *
 * Examples (same components for both shapes):
 * - `/` and `/en`
 * - `/home` and `/en/home`
 * - `/identity/users` and `/en/identity/users`
 *
 * The culture segment is matched only when it passes {@link isLikelyCultureSegment} (e.g. `en`, `tr-TR`, `zh-Hans`).
 * Session language from the URL is still applied only when `localization.useRouteBasedCulture` is true (`RouteBasedCultureService`).
 */
export function withOptionalRouteCulturePrefix(routes: Routes): Routes {
  const matcher = createRouteCultureUrlMatcher();

  return [
    { matcher, children: [...routes] },
    { path: '', children: [...routes] },
  ];
}
