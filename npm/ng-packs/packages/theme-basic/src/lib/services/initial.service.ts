import { DomInsertionService, AddReplaceableComponent, CONTENT_STRATEGY } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import styles from '../constants/styles';
import { ApplicationLayoutComponent } from '../components/application-layout/application-layout.component';
import { AccountLayoutComponent } from '../components/account-layout/account-layout.component';
import { EmptyLayoutComponent } from '../components/empty-layout/empty-layout.component';
import { eThemeBasicComponents } from '../enums/components';
import { NavItemsService } from '@abp/ng.theme.shared';
import { LanguagesComponent } from '../components/nav-items/languages.component';
import { CurrentUserComponent } from '../components/nav-items/current-user.component';

@Injectable({ providedIn: 'root' })
export class InitialService {
  constructor(
    private domInsertion: DomInsertionService,
    private navItemsService: NavItemsService,
    private store: Store,
  ) {
    this.appendStyle();

    this.store.dispatch([
      new AddReplaceableComponent({
        key: eThemeBasicComponents.ApplicationLayout,
        component: ApplicationLayoutComponent,
      }),
      new AddReplaceableComponent({
        key: eThemeBasicComponents.AccountLayout,
        component: AccountLayoutComponent,
      }),
      new AddReplaceableComponent({
        key: eThemeBasicComponents.EmptyLayout,
        component: EmptyLayoutComponent,
      }),
    ]);

    this.navItemsService.addItems([
      { id: eThemeBasicComponents.CurrentUser, component: CurrentUserComponent },
      { id: eThemeBasicComponents.Languages, component: LanguagesComponent },
    ]);
  }

  appendStyle() {
    this.domInsertion.insertContent(CONTENT_STRATEGY.AppendStyleToHead(styles));
  }
}
