import { CONTENT_STRATEGY, DomInsertionService, ReplaceableComponentsService } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';
import { AccountLayoutComponent } from '../components/account-layout/account-layout.component';
import { ApplicationLayoutComponent } from '../components/application-layout/application-layout.component';
import { EmptyLayoutComponent } from '../components/empty-layout/empty-layout.component';
import styles from '../constants/styles';
import { eThemeBasicComponents } from '../enums/components';

export const BASIC_THEME_STYLES_PROVIDERS = [
  provideAppInitializer(() => {
    const initializerFn = configureStyles(
      inject(DomInsertionService),
      inject(ReplaceableComponentsService),
    );
    return initializerFn();
  }),
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
