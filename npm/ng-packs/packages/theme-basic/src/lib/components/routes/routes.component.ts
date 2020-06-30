import { ABP, RoutesService, TreeNode } from '@abp/ng.core';
import { Component, Input, TrackByFunction } from '@angular/core';

@Component({
  selector: 'abp-routes',
  templateUrl: 'routes.component.html',
})
export class RoutesComponent {
  @Input() smallScreen: boolean;

  trackByFn: TrackByFunction<TreeNode<ABP.Route>> = (_, item) => item.name;

  constructor(public readonly routes: RoutesService) {}

  isDropdown(node: TreeNode<ABP.Route>) {
    return !node.isLeaf || this.routes.hasInvisibleChild(node.name);
  }
}
