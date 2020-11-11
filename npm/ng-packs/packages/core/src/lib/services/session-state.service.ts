import { Injectable } from '@angular/core';
import { ApplicationConfiguration } from '../models/application-configuration';
import { Session } from '../models/session';
import { InternalStore } from '../utils/internal-store-utils';
import compare from 'just-compare';

export interface SessionDetail {
  openedTabCount: number;
  lastExitTime: number;
  remember: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class SessionStateService {
  private readonly store = new InternalStore({} as Session.State);

  private updateLocalStorage = () => {
    localStorage.setItem('abpSession', JSON.stringify(this.store.state));
  };

  constructor() {
    this.init();
  }

  private init() {
    const session = localStorage.getItem('abpSession');
    if (session) {
      this.store.patch(JSON.parse(session));
    }

    this.store.sliceUpdate(state => state).subscribe(this.updateLocalStorage);
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

  setTenant(tenant: ApplicationConfiguration.CurrentTenant) {
    if (compare(tenant, this.store.state.tenant)) return;

    this.store.patch({ tenant });
  }

  setLanguage(language: string) {
    if (language === this.store.state.language) return;

    this.store.patch({ language });
  }
}
