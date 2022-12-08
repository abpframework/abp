import { Injectable } from '@angular/core';
import { NavItem } from '../models/nav-item';
import { AbstractMenuService } from './abstract-menu.service';

@Injectable({ providedIn: 'root' })
export class NavItemsService extends AbstractMenuService<NavItem> {
  protected baseClass = NavItem;
}
