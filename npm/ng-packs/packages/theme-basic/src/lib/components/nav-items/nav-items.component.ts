import { AbpVisibleDirective, NavItem, NavItemsService } from '@abp/ng.theme.shared';
import { Component, TrackByFunction } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PermissionDirective, ToInjectorPipe } from '@abp/ng.core';

@Component({
  selector: 'abp-nav-items',
  templateUrl: 'nav-items.component.html',
  imports: [CommonModule, AbpVisibleDirective, PermissionDirective, ToInjectorPipe],
})
export class NavItemsComponent {
  trackByFn: TrackByFunction<NavItem> = (_, element) => element.id;

  constructor(public readonly navItems: NavItemsService) {}
}
