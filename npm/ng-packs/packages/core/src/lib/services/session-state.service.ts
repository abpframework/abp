import { Injectable, inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import compare from 'just-compare';
import { filter, take } from 'rxjs/operators';
import { Session } from '../models/session';
import { CurrentTenantDto } from '../proxy/volo/abp/asp-net-core/mvc/multi-tenancy/models';
import { InternalStore } from '../utils/internal-store-utils';
import { ConfigStateService } from './config-state.service';
import { AbpLocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root',
})
export class SessionStateService {
  private readonly store = new InternalStore({} as Session.State);
  protected readonly document = inject(DOCUMENT);

  private updateLocalStorage = () => {
    this.localStorageService.setItem('abpSession', JSON.stringify(this.store.state));
  };

  constructor(
    private configState: ConfigStateService,
    private localStorageService: AbpLocalStorageService,
  ) {
    this.init();
    this.setInitialLanguage();
  }

  private init() {
    const session = this.localStorageService.getItem('abpSession');
    if (session) {
      this.store.set(JSON.parse(session));
    }

    this.store.sliceUpdate(state => state).subscribe(this.updateLocalStorage);
  }

  private setInitialLanguage() {
    const appLanguage = this.getLanguage();

    this.configState
      .getDeep$('localization.currentCulture.cultureName')
      .pipe(
        filter(cultureName => !!cultureName),
        take(1),
      )
      .subscribe(lang => {
        if (lang.includes(';')) {
          lang = lang.split(';')[0];
        }
        
        this.setLanguage(lang);
      });
  }

  onLanguageChange$() {
    return this.store.sliceUpdate(state => state.language);
  }

  onTenantChange$() {
    return this.store.sliceUpdate(state => state.tenant);
  }

  getLanguage() {
    return this.store.state.language;
  }

  getLanguage$() {
    return this.store.sliceState(state => state.language);
  }

  getTenant() {
    return this.store.state.tenant;
  }

  getTenant$() {
    return this.store.sliceState(state => state.tenant);
  }

  setTenant(tenant: CurrentTenantDto | null) {
    if (compare(tenant, this.store.state.tenant)) return;

    this.store.set({ ...this.store.state, tenant });
  }

  setLanguage(language: string) {
    const currentLanguage = this.store.state.language;

    if (language !== currentLanguage) {
      this.store.patch({ language });
    }

    const currentAttribute = this.document.documentElement.getAttribute('lang');
    if (language !== currentAttribute) {
      this.document.documentElement.setAttribute('lang', language);
    }
  }
}
