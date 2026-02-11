import { AbpVisibleDirective, NavItem, NavItemsService } from '@abp/ng.theme.shared';
import { Component, TrackByFunction, inject, PLATFORM_ID } from '@angular/core';
import { NgComponentOutlet, AsyncPipe, isPlatformBrowser } from '@angular/common';
import { PermissionDirective, ToInjectorPipe } from '@abp/ng.core';

@Component({
  selector: 'abp-nav-items',
  templateUrl: 'nav-items.component.html',
  imports: [NgComponentOutlet, AsyncPipe, AbpVisibleDirective, PermissionDirective, ToInjectorPipe],
})
export class NavItemsComponent {
  readonly navItems = inject(NavItemsService);
  private platformId = inject(PLATFORM_ID);
  readonly isBrowser = isPlatformBrowser(this.platformId);

  trackByFn: TrackByFunction<NavItem> = (_, element) => element.id;
}
