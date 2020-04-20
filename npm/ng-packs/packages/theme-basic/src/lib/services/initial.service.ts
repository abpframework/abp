import { DomInsertionService, AddReplaceableComponent, CONTENT_STRATEGY } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import styles from '../constants/styles';
import { ApplicationLayoutComponent } from '../components/application-layout/application-layout.component';
import { AccountLayoutComponent } from '../components/account-layout/account-layout.component';
import { EmptyLayoutComponent } from '../components/empty-layout/empty-layout.component';
import { eThemeBasicComponents } from '../enums/components';

@Injectable({ providedIn: 'root' })
export class InitialService {
  constructor(private domInsertion: DomInsertionService, private store: Store) {
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
  }

  appendStyle() {
    this.domInsertion.insertContent(CONTENT_STRATEGY.AppendStyleToHead(styles));
  }
}
