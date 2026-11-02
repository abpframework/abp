import { UrlHelperService } from 'angular-oauth2-oidc';

/**
 * Returns the authorization response that the OAuth library handles on this page, if any. It is
 * read like the library does: from the query, or from the fragment when the query is empty.
 * Without the state, the library ignores a code, so it isn't a sign-in callback.
 */
export function getSignInCallback(
  location?: Pick<Location, 'search' | 'hash'> | null,
): { code?: string; error?: string } | null {
  const urlHelper = new UrlHelperService();
  let parts: Record<string, string> = {};
  if (location?.search) {
    parts = urlHelper.parseQueryString(location.search.substring(1)) as Record<string, string>;
  } else if (location?.hash) {
    parts = urlHelper.getHashFragmentParams(location.hash) as Record<string, string>;
  }

  if (!parts['state'] || !(parts['code'] || parts['error'])) {
    return null;
  }
  return { code: parts['code'], error: parts['error'] };
}
