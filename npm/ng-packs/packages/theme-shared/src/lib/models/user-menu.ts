import { NavItem } from './nav-item';

export class UserMenu extends NavItem {
  textTemplate?: UserMenuTextTemplate;
}

export interface UserMenuTextTemplate {
  text: string;
  icon?: string;
}
