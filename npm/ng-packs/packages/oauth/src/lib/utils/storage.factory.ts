import { AbpLocalStorageService } from '@abp/ng.core';
import { inject } from '@angular/core';
import { OAuthStorage } from 'angular-oauth2-oidc';
 
export function storageFactory(): OAuthStorage {
  const storage = inject(AbpLocalStorageService)
  return storage;
}
