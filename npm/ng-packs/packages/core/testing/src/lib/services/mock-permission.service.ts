import { ConfigStateService, PermissionService } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MockPermissionService extends PermissionService {
  protected configState: ConfigStateService;

  constructor() {
    const configState = inject(ConfigStateService);
    super();
    this.configState = configState;

    this.grantAllPolicies();
  }

  grantAllPolicies() {
    const grantedPolicies = new Proxy(
      {},
      {
        get() {
          return true;
        },
      },
    );

    this.configState['store'].deepPatch({ auth: { grantedPolicies } });
  }

  grantPolicies(keys: string[]) {
    const grantedPolicies = keys.reduce((policies, key) => {
      policies[key] = true;
      return policies;
    }, {} as { [key: string]: boolean });

    this.configState['store'].deepPatch({ auth: { grantedPolicies } });
  }
}
