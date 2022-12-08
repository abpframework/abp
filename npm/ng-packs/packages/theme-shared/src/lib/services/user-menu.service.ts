import { Injectable } from '@angular/core';
import { UserMenu } from '../models/user-menu';
import { AbstractMenuService } from './abstract-menu.service';

@Injectable({ providedIn: 'root' })
export class UserMenuService extends AbstractMenuService<UserMenu> {
  protected baseClass = UserMenu;
}
