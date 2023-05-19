import { OAuthStorage } from 'angular-oauth2-oidc';
import { oAuthStorage } from './oauth-storage';

export function storageFactory(): OAuthStorage {
  return oAuthStorage;
}
