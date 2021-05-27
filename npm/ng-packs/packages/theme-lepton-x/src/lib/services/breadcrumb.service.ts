import { Injectable } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { ABP, getRoutePath, LocalizationService, RoutesService, TreeNode } from '@abp/ng.core';
import { filter, map } from 'rxjs/operators';
import { BreadcrumbItem, BreadcrumbService } from '@volo/ngx-lepton-x.core';
import { eThemeSharedRouteNames } from '@abp/ng.theme.shared';

@Injectable({
  providedIn: 'root',
})
export class AbpBreadcrumbService {
  constructor(
    private router: Router,
    private routes: RoutesService,
    private breadcrumbService: BreadcrumbService,
    private localizationService: LocalizationService,
  ) {}

  listenRouter() {
    this.router.events
      .pipe(
        filter<NavigationEnd>(event => event instanceof NavigationEnd),
        map(() => this.routes.search({ path: getRoutePath(this.router) })),
      )
      .subscribe(route => {
        const segments = [];
        if (route) {
          let node = { parent: route } as TreeNode<ABP.Route>;

          while (node.parent) {
            node = node.parent;
            const { parent, isLeaf, ...segment } = node;
            segments.unshift(segment);
          }
        }
        const breadCrumbItems = [
          ...(this.router.url !== '/'
            ? [this.mapRouteToBreadcrumbItem(this.routes.search({ path: '/' }))]
            : []),
          ...segments
            .filter(segment => segment.name !== eThemeSharedRouteNames.Administration)
            .map(segment => this.mapRouteToBreadcrumbItem(segment, this.router.url)),
        ];
        this.breadcrumbService.setItems(breadCrumbItems);
      });
  }

  private getRouteChildrenAsBreadcrumbItems(
    children: TreeNode<ABP.Route>[],
    activeUrl,
  ): BreadcrumbItem[] {
    return children.map(child => this.mapRouteToBreadcrumbItem(child, activeUrl));
  }

  private mapRouteToBreadcrumbItem(route: TreeNode<ABP.Route>, activeUrl = ''): BreadcrumbItem {
    return {
      text: this.localizationService.instant(route.name),
      link: route.path,
      icon: route.iconClass,
      children: route.children?.length
        ? this.getRouteChildrenAsBreadcrumbItems(route.children, activeUrl)
        : undefined,
      active: activeUrl === route.path,
    };
  }
}
