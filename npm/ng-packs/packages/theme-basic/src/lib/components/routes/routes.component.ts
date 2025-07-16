import {
  ABP,
  LazyLocalizationPipe,
  LocalizationPipe,
  PermissionDirective,
  RoutesService,
  TreeNode,
} from '@abp/ng.core';
import {
  Component,
  ElementRef,
  inject,
  Input,
  QueryList,
  Renderer2,
  TrackByFunction,
  ViewChildren,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { EllipsisDirective } from '@abp/ng.theme.shared';

@Component({
  selector: 'abp-routes',
  templateUrl: 'routes.component.html',
  imports: [
    CommonModule,
    RouterModule,
    NgbDropdownModule,
    LazyLocalizationPipe,
    PermissionDirective,
    EllipsisDirective,
    LocalizationPipe,
  ],
})
export class RoutesComponent {
  public readonly routesService = inject(RoutesService);
  protected renderer = inject(Renderer2);

  @Input() smallScreen?: boolean;

  @ViewChildren('childrenContainer') childrenContainers!: QueryList<ElementRef<HTMLDivElement>>;

  rootDropdownExpand = {} as { [key: string]: boolean };

  trackByFn: TrackByFunction<TreeNode<ABP.Route>> = (_, item) => item.name;

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
