import { CONTENT_STRATEGY, DomInsertionService, ReplaceableComponentsService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';
import { AccountLayoutComponent } from '../components/account-layout/account-layout.component';
import { ApplicationLayoutComponent } from '../components/application-layout/application-layout.component';
import { EmptyLayoutComponent } from '../components/empty-layout/empty-layout.component';
import styles from '../constants/styles';
import { eThemeBasicComponents } from '../enums/components';

export const BASIC_THEME_STYLES_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: configureStyles,
    deps: [DomInsertionService, ReplaceableComponentsService],
    multi: true,
  },
];

export function configureStyles(
  domInsertion: DomInsertionService,
  replaceableComponents: ReplaceableComponentsService,
) {
  return () => {
    domInsertion.insertContent(CONTENT_STRATEGY.AppendStyleToHead(styles));

    initLayouts(replaceableComponents);
  };
}

function initLayouts(replaceableComponents: ReplaceableComponentsService) {
  replaceableComponents.add({
    key: eThemeBasicComponents.ApplicationLayout,
    component: ApplicationLayoutComponent,
  });
  replaceableComponents.add({
    key: eThemeBasicComponents.AccountLayout,
    component: AccountLayoutComponent,
  });
  replaceableComponents.add({
    key: eThemeBasicComponents.EmptyLayout,
    component: EmptyLayoutComponent,
  });
}
