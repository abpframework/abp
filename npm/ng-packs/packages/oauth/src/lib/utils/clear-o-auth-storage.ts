import { OAuthStorage } from 'angular-oauth2-oidc';
import { oAuthStorage } from './oauth-storage';

export function clearOAuthStorage(storage: OAuthStorage = oAuthStorage) {
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
