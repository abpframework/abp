import type { LanguageInfo } from '../proxy/volo/abp/localization/models';

/**
 * Returns the first path segment of a URL path (leading slashes are ignored).
 * Strips query string and fragment first (e.g. "/zh-Hans/account?x=1" → "zh-Hans").
 */
export function getFirstPathSegment(urlPath: string): string {
  if (!urlPath) {
    return '';
  }

  const suffixIndex = urlPath.search(/[?#]/);
  const pathPart = suffixIndex >= 0 ? urlPath.slice(0, suffixIndex) : urlPath;
  const segments = pathPart.split('/').filter(s => s.length > 0);

  return segments[0] ?? '';
}

/**
 * If the first segment matches a configured language culture name (case-insensitive),
 * returns the canonical culture name from configuration; otherwise undefined.
 */
export function findMatchingCultureName(
  firstSegment: string,
  languages: LanguageInfo[] | undefined,
): string | undefined {
  if (!firstSegment || !languages?.length) {
    return undefined;
  }

  const normalized = firstSegment.toLowerCase();
  const match = languages.find(
    lang => lang.cultureName && lang.cultureName.toLowerCase() === normalized,
  );

  return match?.cultureName;
}

/**
 * Removes the first path segment when it matches a configured UI culture (e.g. `/en/identity/users` → `/identity/users`).
 * Use when comparing the **browser URL** to **menu links** that omit the culture segment.
 */
export function stripCultureSegmentFromPath(
  path: string,
  languages: LanguageInfo[] | undefined,
): string {
  if (!path || !languages?.length) {
    return path;
  }

  const pathEnd = path.search(/[?#]/);
  const pathOnly = pathEnd >= 0 ? path.slice(0, pathEnd) : path;
  const suffix = pathEnd >= 0 ? path.slice(pathEnd) : '';

  const first = getFirstPathSegment(pathOnly);
  if (!first || !findMatchingCultureName(first, languages)) {
    return path;
  }

  const segments = pathOnly.split('/').filter(s => s.length > 0);
  if (segments.length === 0) {
    return path;
  }

  const rest = segments.slice(1);
  const normalized = rest.length ? '/' + rest.join('/') : '/';
  return normalized + suffix;
}

/**
 * When route-based culture is enabled, normalizes URLs for comparison with menu routes that do not include a culture prefix.
 */
export function normalizeUrlForRouteCultureMatch(
  path: string,
  useRouteBasedCulture: boolean,
  languages: LanguageInfo[] | undefined,
): string {
  if (!useRouteBasedCulture || !path) {
    return path;
  }
  return stripCultureSegmentFromPath(path, languages);
}
