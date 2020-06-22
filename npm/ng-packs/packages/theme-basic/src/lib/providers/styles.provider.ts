import { AddReplaceableComponent, CONTENT_STRATEGY, DomInsertionService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';
import { Store } from '@ngxs/store';
import { AccountLayoutComponent } from '../components/account-layout/account-layout.component';
import { ApplicationLayoutComponent } from '../components/application-layout/application-layout.component';
import { EmptyLayoutComponent } from '../components/empty-layout/empty-layout.component';
import styles from '../constants/styles';
import { eThemeBasicComponents } from '../enums/components';

export const BASIC_THEME_STYLES_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: configureStyles,
    deps: [DomInsertionService, Store],
    multi: true,
  },
];

export function configureStyles(domInsertion: DomInsertionService, store: Store) {
  return () => {
    domInsertion.insertContent(CONTENT_STRATEGY.AppendStyleToHead(styles));

    initLayouts(store);
  };
}

function initLayouts(store: Store) {
  store.dispatch([
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
