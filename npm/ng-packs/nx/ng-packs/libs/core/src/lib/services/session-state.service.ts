import { Injectable } from '@angular/core';
import compare from 'just-compare';
import { filter, take } from 'rxjs/operators';
import { Session } from '../models/session';
import { CurrentTenantDto } from '../proxy/volo/abp/asp-net-core/mvc/multi-tenancy/models';
import { InternalStore } from '../utils/internal-store-utils';
import { ConfigStateService } from './config-state.service';

@Injectable({
  providedIn: 'root',
})
export class SessionStateService {
  private readonly store = new InternalStore({} as Session.State);

  private updateLocalStorage = () => {
    localStorage.setItem('abpSession', JSON.stringify(this.store.state));
  };

  constructor(private configState: ConfigStateService) {
    this.init();
    this.setInitialLanguage();
  }

  private init() {
    const session = localStorage.getItem('abpSession');
    if (session) {
      this.store.set(JSON.parse(session));
    }

    this.store.sliceUpdate(state => state).subscribe(this.updateLocalStorage);
  }

  private setInitialLanguage() {
    if (this.getLanguage()) return;

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

  setTenant(tenant: CurrentTenantDto) {
    if (compare(tenant, this.store.state.tenant)) return;

    this.store.set({ ...this.store.state, tenant });
  }

  setLanguage(language: string) {
    if (language === this.store.state.language) return;

    this.store.patch({ language });
    document.documentElement.setAttribute('lang', language);
  }
}
