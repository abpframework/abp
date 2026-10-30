import { DOCUMENT, Injector, PLATFORM_ID } from '@angular/core';
import { APP_STARTED_WITH_SSR, noop } from '@abp/ng.core';
import { Params } from '@angular/router';
import { filter, firstValueFrom, from, of, take, tap } from 'rxjs';
import { AuthConfig, OAuthService } from 'angular-oauth2-oidc';
import { AuthFlowStrategy } from './auth-flow-strategy';
import { isTokenExpired } from '../utils';
import { isPlatformBrowser } from '@angular/common';

const DEFAULT_OPEN_URI = new AuthConfig().openUri?.toString();

export class AuthCodeFlowStrategy extends AuthFlowStrategy {
  readonly isInternalAuth = false;
  private platformId: object;
  protected appStartedWithSSR: boolean;
  protected document: Document;

  constructor(protected injector: Injector) {
    super(injector);
    this.platformId = injector.get(PLATFORM_ID);
    this.appStartedWithSSR = injector.get(APP_STARTED_WITH_SSR);
    this.document = injector.get(DOCUMENT);
  }

  async init() {
    this.checkRememberMeOption();
    this.listenToTokenReceived();
    if (!this.appStartedWithSSR && isPlatformBrowser(this.platformId)) {
      return super
        .init()
        .then(() => this.oAuthService.tryLogin().catch(noop))
        .then(() => {
          this.oAuthService.setupAutomaticSilentRefresh();
        });
    }
  }

  private checkRememberMeOption() {
    const accessToken = this.oAuthService.getAccessToken();
    const isTokenExpire = isTokenExpired(this.oAuthService.getAccessTokenExpiration());
    let rememberMe = this.rememberMeService.get();
    if (accessToken && !rememberMe) {
      const rememberMeValue = this.rememberMeService.getFromToken(accessToken);

      this.rememberMeService.set(!!rememberMeValue);
    }

    rememberMe = this.rememberMeService.get();
    // The expired tokens of other tabs aren't signed out from here: that would end their session
    // and remove the sign-in values of a callback on this page.
    if (accessToken && isTokenExpire && !rememberMe && !this.hasOnlySharedTokens()) {
      this.rememberMeService.remove();
      this.oAuthService.logOut();
    }
  }

  private getCultureParams(queryParams?: Params) {
    const lang = this.sessionState.getLanguage();
    const culture = { culture: lang, 'ui-culture': lang };
    return { ...(lang && culture), ...queryParams };
  }

  protected setUICulture() {
    if (isPlatformBrowser(this.platformId)) {
      const urlParams = new URLSearchParams(window.location.search);
      this.configState.uiCultureFromAuthCodeFlow = urlParams.get('ui-culture');
    }
  }

  protected replaceURLParams() {
    if (isPlatformBrowser(this.platformId)) {
      const location = this.windowService.window.location;
      const history = this.windowService.window.history;

      const query = location.search
        .replace(/([?&])iss=[^&]*&?/, '$1')
        .replace(/([?&])culture=[^&]*&?/, '$1')
        .replace(/([?&])ui-culture=[^&]*&?/, '$1')
        .replace(/[?&]+$/, '');

      const href = location.origin + location.pathname + query + location.hash;

      history.replaceState(null, '', href);
    }
  }

  protected listenToTokenReceived() {
    if (isPlatformBrowser(this.platformId) && !this.appStartedWithSSR) {
      this.oAuthService.events
        .pipe(
          filter(event => event.type === 'token_received'),
          tap(() => {
            //TODO: Investigate how to handle with ssr
            this.setUICulture();
            this.replaceURLParams();
          }),
          take(1),
        )
        .subscribe();
    }
  }

  private redirect?: Promise<void>;

  override startLogin(queryParams?: Params): Promise<void> | undefined {
    if (!this.usesDefaultRedirect()) {
      return super.startLogin(queryParams);
    }

    return this.redirectToLogin(queryParams?.returnUrl || '', this.getCultureParams(queryParams));
  }

  // Each redirect stores its own nonce and PKCE verifier, so requests that come while the page
  // leaves for the login page share the first redirect. The user can stay on the page (a
  // beforeunload prompt) or come back to it (the back-forward cache); a new redirect starts then.
  private redirectToLogin(state: string, params: object): Promise<void> {
    // Another version of the library can lack the members used below; the public API still opens
    // the login page then, without sharing it.
    if (!canCreateLoginUrl(this.oAuthService)) {
      this.oAuthService.initCodeFlow(state, params);
      return Promise.resolve();
    }

    if (!this.redirect) {
      // Watches the page before it opens the login page, as beforeunload runs at that moment.
      const leaving = watchLeaving(this.document.defaultView);
      this.redirect = startCodeFlow(this.oAuthService, state, params)
        .then(
          () => leaving.stayed,
          error => {
            leaving.stop();
            throw error;
          },
        )
        .finally(() => (this.redirect = undefined));
    }
    return this.redirect;
  }

  // Other ways to open the login page (a subclass, a custom openUri or initCodeFlow, SSR) can keep
  // this page or have their own rules, so they get every request, as before.
  override usesDefaultRedirect(): boolean {
    return (
      isPlatformBrowser(this.platformId) &&
      !this.appStartedWithSSR &&
      this.navigateToLogin === AuthCodeFlowStrategy.prototype.navigateToLogin &&
      this.oAuthService.initCodeFlow === OAuthService.prototype.initCodeFlow &&
      // Each configuration gets a new default openUri, so it is compared by its code.
      this.oAuthService['config']?.openUri?.toString() === DEFAULT_OPEN_URI
    );
  }

  navigateToLogin(queryParams?: Params) {
    if (isPlatformBrowser(this.platformId)) {
      if (this.appStartedWithSSR) {
        if (this.document.defaultView) {
          const returnUrl = queryParams?.returnUrl;
          this.document.defaultView.location.replace(
            returnUrl ? `/authorize?returnUrl=${encodeURIComponent(returnUrl)}` : '/authorize',
          );
        }
      } else {
        let additionalState = '';
        if (queryParams?.returnUrl) {
          additionalState = queryParams.returnUrl;
        }

        const cultureParams = this.getCultureParams(queryParams);
        this.oAuthService.initCodeFlow(additionalState, cultureParams);
      }
    }
  }

  checkIfInternalAuth(queryParams?: Params) {
    if (isPlatformBrowser(this.platformId)) {
      this.oAuthService.initCodeFlow('', this.getCultureParams(queryParams));
      return false;
    }
  }

  logout(queryParams?: Params) {
    this.rememberMeService.remove();

    if (this.appStartedWithSSR) {
      if (this.document.defaultView) {
        this.document.defaultView.location.replace('/logout');
      }
    } else {
      if (queryParams?.noRedirectToLogoutUrl) {
        this.router.navigate(['/']);
        return from(this.oAuthService.revokeTokenAndLogout(true));
      }
      return from(this.oAuthService.revokeTokenAndLogout(this.getCultureParams(queryParams)));
    }
  }

  login(queryParams?: Params) {
    if (isPlatformBrowser(this.platformId)) {
      if (this.usesDefaultRedirect()) {
        // Shares the redirect that a route guard or a 401 may have started. While it runs, tokens
        // from other tabs don't reload this page.
        const storage = this.getMemoryStorage();
        storage?.markSignInStarted();
        this.redirectToLogin('', this.getCultureParams(queryParams))
          .catch(error => console.error('Could not start the sign-in', error))
          .finally(() => storage?.markSignInFinished());
      } else {
        this.oAuthService.initCodeFlow('', this.getCultureParams(queryParams));
      }
      return of(null);
    }
  }
}

// `stayed` resolves when the page comes back from the back-forward cache, or when the user uses the
// page after a beforeunload prompt (the user canceled the navigation). Input on a page that is
// waiting for a slow login page keeps the redirect shared.
function watchLeaving(view: Window | null): { stayed: Promise<void>; stop: () => void } {
  const beforeUnloads: BeforeUnloadEvent[] = [];
  // Read later, as listeners that run after this one can cancel the event.
  const prompted = () =>
    beforeUnloads.some(
      ({ defaultPrevented, returnValue }) =>
        defaultPrevented || (typeof returnValue === 'string' && returnValue !== ''),
    );
  let resolveStayed!: () => void;
  const stayed = new Promise<void>(resolve => (resolveStayed = resolve));
  const listener = (event: Event) => {
    if (event.type === 'beforeunload') {
      beforeUnloads.push(event as BeforeUnloadEvent);
    } else if (event.type === 'pageshow' ? (event as PageTransitionEvent).persisted : prompted()) {
      stop();
      resolveStayed();
    }
  };
  const events = ['beforeunload', 'pageshow', 'pointerdown', 'keydown'];
  const stop = () => events.forEach(type => view?.removeEventListener(type, listener));
  events.forEach(type => view?.addEventListener(type, listener));
  return { stayed, stop };
}

function canCreateLoginUrl(oAuthService: OAuthService): boolean {
  return (
    typeof oAuthService['createLoginUrl'] === 'function' &&
    typeof oAuthService['validateUrlForHttps'] === 'function'
  );
}

/**
 * Same as `OAuthService.initCodeFlow`, but the returned promise rejects when the login page can't
 * be opened; `initCodeFlow` only logs that error.
 */
async function startCodeFlow(
  oAuthService: OAuthService,
  additionalState: string,
  params: object,
): Promise<void> {
  if (oAuthService.loginUrl === '') {
    await firstValueFrom(
      oAuthService.events.pipe(filter(event => event.type === 'discovery_document_loaded')),
    );
  }

  if (!oAuthService['validateUrlForHttps'](oAuthService.loginUrl)) {
    throw new Error(
      "loginUrl must use HTTPS (with TLS), or config value for property 'requireHttps' must be set to 'false' and allow HTTP (without TLS).",
    );
  }

  const url = await oAuthService['createLoginUrl'](additionalState, '', '', false, params);
  await oAuthService['config'].openUri(url);
}
