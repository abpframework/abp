import { Type } from '@angular/core';
import { ReplaySubject } from 'rxjs';

export interface NavItem {
  component?: Type<any>;
  html?: string;
  action?: () => void;
  order?: number;
  permission?: string;
}

const navItems: NavItem[] = [];
const navItems$ = new ReplaySubject<NavItem[]>(1);

export function addNavItem(item: NavItem) {
  navItems.push(item);
  navItems$.next(navItems.sort((a, b) => (a.order ? a.order - b.order : 1)));
}

export function getNavItems() {
  return navItems$.asObservable();
}
