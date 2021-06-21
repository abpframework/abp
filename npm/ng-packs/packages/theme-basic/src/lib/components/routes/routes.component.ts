import { ABP, RoutesService, TreeNode } from '@abp/ng.core';
import {
  Component,
  ElementRef,
  Input,
  QueryList,
  Renderer2,
  TrackByFunction,
  ViewChildren,
} from '@angular/core';

@Component({
  selector: 'abp-routes',
  templateUrl: 'routes.component.html',
})
export class RoutesComponent {
  @Input() smallScreen: boolean;

  @ViewChildren('childrenContainer') childrenContainers: QueryList<ElementRef<HTMLDivElement>>;

  trackByFn: TrackByFunction<TreeNode<ABP.Route>> = (_, item) => item.name;

  constructor(public readonly routesService: RoutesService, protected renderer: Renderer2) {}

  isDropdown(node: TreeNode<ABP.Route>) {
    return !node?.isLeaf || this.routesService.hasChildren(node.name);
  }

  closeDropdown() {
    this.childrenContainers.forEach(({ nativeElement }) => {
      this.renderer.addClass(nativeElement, 'd-none');
      setTimeout(() => this.renderer.removeClass(nativeElement, 'd-none'), 0);
    });
  }
}
