import { Component, OnInit } from '@angular/core';
import { eThemeLeptonXComponents } from '@abp/ng.theme.lepton-x';

@Component({
  selector: 'abp-side-menu-application-layout',
  templateUrl: './side-menu-application-layout.component.html',
})
export class SideMenuApplicationLayoutComponent implements OnInit {
  toolbarKey = eThemeLeptonXComponents.Navbar;
  navbarKey = eThemeLeptonXComponents.Sidebar;
  routesKey = eThemeLeptonXComponents.Routes;
  toolbarItemsKey = eThemeLeptonXComponents.NavItems;

  constructor() {}

  ngOnInit(): void {}
}
