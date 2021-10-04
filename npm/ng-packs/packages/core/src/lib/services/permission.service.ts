import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { ABP } from '../models/common';
import { ApplicationConfigurationDto } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/models';
import { ConfigStateService } from './config-state.service';

@Injectable({ providedIn: 'root' })
export class PermissionService {
  constructor(protected configState: ConfigStateService) {}

  getGrantedPolicy$(key: string) {
    return this.getStream().pipe(
      map(grantedPolicies => this.isPolicyGranted(key, grantedPolicies)),
    );
  }

  getGrantedPolicy(key: string) {
    const policies = this.getSnapshot();
    return this.isPolicyGranted(key, policies);
  }

  filterItemsByPolicy<T extends ABP.HasPolicy>(items: Array<T>) {
    const policies = this.getSnapshot();
    return items.filter(
      item => !item.requiredPolicy || this.isPolicyGranted(item.requiredPolicy, policies),
    );
  }

  filterItemsByPolicy$<T extends ABP.HasPolicy>(items: Array<T>) {
    return this.getStream().pipe(
      map(policies =>
        items.filter(
          item => !item.requiredPolicy || this.isPolicyGranted(item.requiredPolicy, policies),
        ),
      ),
    );
  }

  protected isPolicyGranted(key: string, grantedPolicies: Record<string, boolean>) {
    if (!key) return true;

    const orRegexp = /\|\|/g;
    const andRegexp = /&&/g;

    // TODO: Allow combination of ANDs & ORs
    if (orRegexp.test(key)) {
      const keys = key.split('||').filter(Boolean);

      if (keys.length < 2) return false;

      return keys.some(k => this.getPolicy(k.trim(), grantedPolicies));
    } else if (andRegexp.test(key)) {
      const keys = key.split('&&').filter(Boolean);

      if (keys.length < 2) return false;

      return keys.every(k => this.getPolicy(k.trim(), grantedPolicies));
    }

    return this.getPolicy(key, grantedPolicies);
  }

  protected getStream() {
    return this.configState.getAll$().pipe(map(this.mapToPolicies));
  }

  protected getSnapshot() {
    return this.mapToPolicies(this.configState.getAll());
  }

  protected mapToPolicies(applicationConfiguration: ApplicationConfigurationDto) {
    return applicationConfiguration?.auth?.grantedPolicies || {};
  }

  protected getPolicy(key: string, grantedPolicies: Record<string, boolean>) {
    return grantedPolicies[key] || false;
  }
}
