import { ConfigStateService, PermissionService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MockPermissionService extends PermissionService {
  constructor(protected configState: ConfigStateService) {
    super(configState);
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
    }, {});

    this.configState['store'].deepPatch({ auth: { grantedPolicies } });
  }
}
