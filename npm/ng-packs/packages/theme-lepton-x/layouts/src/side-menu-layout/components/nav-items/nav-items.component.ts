import { NavItem, NavItemsService } from '@abp/ng.theme.shared';
import { Component, TrackByFunction } from '@angular/core';

@Component({
  selector: 'abp-nav-items',
  templateUrl: 'nav-items.component.html',
  styles: [':host{ all: inherit }'],
})
export class NavItemsComponent {
  trackByFn: TrackByFunction<NavItem> = (_, element) => element.id;

  constructor(public readonly navItems: NavItemsService) {}
}
