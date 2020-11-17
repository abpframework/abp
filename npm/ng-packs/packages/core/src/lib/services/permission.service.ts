import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import snq from 'snq';
import { ApplicationConfiguration } from '../models/application-configuration';
import { ConfigStateService } from './config-state.service';

@Injectable({ providedIn: 'root' })
export class PermissionService {
  constructor(private configState: ConfigStateService) {}

  getGrantedPolicy$(key: string) {
    return this.getStream().pipe(map(policies => this.isPolicyGranted(key, policies)));
  }

  getGrantedPolicy(key: string) {
    const policies = this.getSnapshot();
    return this.isPolicyGranted(key, policies);
  }

  private isPolicyGranted(key: string, policies: ApplicationConfiguration.Policy) {
    if (!key) return true;

    const orRegexp = /\|\|/g;
    const andRegexp = /&&/g;

    // TODO: Allow combination of ANDs & ORs
    if (orRegexp.test(key)) {
      const keys = key.split('||').filter(Boolean);

      if (keys.length < 2) return false;

      return keys.some(k => this.getPolicy(k.trim(), policies));
    } else if (andRegexp.test(key)) {
      const keys = key.split('&&').filter(Boolean);

      if (keys.length < 2) return false;

      return keys.every(k => this.getPolicy(k.trim(), policies));
    }

    return this.getPolicy(key, policies);
  }

  private getStream() {
    return this.configState.getAll$().pipe(map(this.mapToPolicies));
  }

  private getSnapshot() {
    return this.mapToPolicies(this.configState.getAll());
  }

  private mapToPolicies(applicationConfiguration: ApplicationConfiguration.Response) {
    return snq(() => applicationConfiguration.auth.grantedPolicies);
  }

  private getPolicy(policy: string, policies: ApplicationConfiguration.Policy) {
    return snq(() => policies[policy], false);
  }
}
