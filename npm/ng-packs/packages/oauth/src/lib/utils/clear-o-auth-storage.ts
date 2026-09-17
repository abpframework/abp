import { OAuthStorage } from 'angular-oauth2-oidc';
import { Injector } from '@angular/core';

export function clearOAuthStorage(injector: Injector) {
  const storage = injector.get(OAuthStorage);
  const keys = [
    'access_token',
    'id_token',
    'refresh_token',
    'nonce',
    'PKCE_verifier',
    'expires_at',
    'id_token_claims_obj',
    'id_token_expires_at',
    'id_token_stored_at',
    'access_token_stored_at',
    'granted_scopes',
    'session_state',
  ];

  keys.forEach(key => storage.removeItem(key));
}
