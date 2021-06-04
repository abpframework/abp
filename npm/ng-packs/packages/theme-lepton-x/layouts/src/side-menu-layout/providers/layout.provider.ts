import { APP_INITIALIZER, Injector, Provider } from '@angular/core';
import { ReplaceableComponentsService } from '@abp/ng.core';
import { eThemeLeptonXComponents } from '@abp/ng.theme.lepton-x';
import { SideMenuApplicationLayoutComponent } from '../side-menu-application-layout/side-menu-application-layout.component';

export const LPX_LAYOUT_PROVIDER: Provider = {
  provide: APP_INITIALIZER,
  useFactory: initLayouts,
  deps: [Injector],
  multi: true,
};

export function initLayouts(injector: Injector) {
  const replaceableComponents = injector.get(ReplaceableComponentsService);
  return () => {
    replaceableComponents.add({
      key: eThemeLeptonXComponents.ApplicationLayout,
      component: SideMenuApplicationLayoutComponent,
    });
  };
}
