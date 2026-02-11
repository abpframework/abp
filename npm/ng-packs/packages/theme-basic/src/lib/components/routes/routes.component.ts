import {
  ABP,
  AsyncLocalizationPipe,
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
  Renderer2,
  TrackByFunction,
  viewChildren
} from '@angular/core';
import { NgTemplateOutlet, AsyncPipe } from '@angular/common';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterLink } from '@angular/router';
import { EllipsisDirective } from '@abp/ng.theme.shared';

@Component({
  selector: 'abp-routes',
  templateUrl: 'routes.component.html',
  imports: [
    NgTemplateOutlet,
    AsyncPipe,
    RouterLink,
    NgbDropdownModule,
    AsyncLocalizationPipe,
    PermissionDirective,
    EllipsisDirective,
    LocalizationPipe,
  ],
})
export class RoutesComponent {
  public readonly routesService = inject(RoutesService);
  protected renderer = inject(Renderer2);

  @Input() smallScreen?: boolean;

  readonly childrenContainers = viewChildren<ElementRef<HTMLDivElement>>('childrenContainer');

  rootDropdownExpand = {} as { [key: string]: boolean };

  trackByFn: TrackByFunction<TreeNode<ABP.Route>> = (_, item) => item.name;

  isDropdown(node: TreeNode<ABP.Route>) {
    return !node?.isLeaf || this.routesService.hasChildren(node.name);
  }

  closeDropdown() {
    this.childrenContainers().forEach(({ nativeElement }) => {
      this.renderer.addClass(nativeElement, 'd-none');
      setTimeout(() => this.renderer.removeClass(nativeElement, 'd-none'), 0);
    });
  }
}
