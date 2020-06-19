import { ABP, RoutesService, TreeNode, LocalizationService, ConfigState } from '@abp/ng.core';
import { Component, Input, TrackByFunction } from '@angular/core';
import { Store } from '@ngxs/store';

@Component({
  selector: 'abp-routes',
  templateUrl: 'routes.component.html',
})
export class RoutesComponent {
  @Input() smallScreen: boolean;

  trackByFn: TrackByFunction<TreeNode<ABP.Route>> = (_, item) => item.name;

  constructor(public readonly routes: RoutesService, private store: Store) {}

  findRequiredPolicy(route: TreeNode<ABP.Route>): boolean {
    if (route.requiredPolicy || !route.children?.length) return true;

    const policies = getPolicies(route.children);

    let isVisible = true;
    for (let i = 0; i < policies.length; i++) {
      if (!this.store.selectSnapshot(ConfigState.getGrantedPolicy(policies[i]))) {
        isVisible = false;
        break;
      }
    }

    return isVisible;
  }
}

function getPolicies(routes: TreeNode<ABP.Route>[]): string[] {
  return routes.reduce((acc, val) => {
    if (val.requiredPolicy) acc.push(val.requiredPolicy);

    if (val.children?.length) {
      const childPolicies = getPolicies(val.children);

      if (childPolicies?.length) acc.push(...childPolicies);
    }

    return acc;
  }, []);
}
