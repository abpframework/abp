import { Injectable } from '@angular/core';
import { ABP, LocalizationService, RoutesService, TreeNode } from '@abp/ng.core';
import { NavbarItem, NavbarService as LeptonNavbarService } from '@lepton-x/common';

@Injectable({
  providedIn: 'root',
})
export class NavbarService {
  constructor(
    private routes: RoutesService,
    private navbarService: LeptonNavbarService,
    private localizationService: LocalizationService,
  ) {}

  private mapRouteToNavItem = (route: TreeNode<ABP.Route>, index: number): NavbarItem => {
    return {
      text: this.localizationService.instant(route.name),
      link: route.children && route.children.length ? null : route.path,
      icon: route.iconClass,
      children: this.getRouteChildrenAsNavItems(route.children || []),
      showOnMobileNavbar: index === 0 || index === 1,
    };
  };

  initRoutes() {
    this.routes.visible$.subscribe(routes => {
      this.navbarService.setNavbarItems(...routes.map(this.mapRouteToNavItem));
    });
  }

  private getRouteChildrenAsNavItems(children: TreeNode<ABP.Route>[]): NavbarItem[] {
    return children.map(this.mapRouteToNavItem);
  }
}
