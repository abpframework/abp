import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

import { appRoutes } from './app.routes';
import { APP_ROUTE_PROVIDER } from './route.provider';
import { provideAbpCore, withOptions } from '@abp/ng.core';
import { environment } from '../environments/environment';
import { registerLocale } from '@abp/ng.core/locale';
import { provideAbpOAuth } from '@abp/ng.oauth';
import { provideAbpThemeShared } from '@abp/ng.theme.shared';
import { provideSettingManagementConfig } from '@abp/ng.setting-management/config';
import { provideAccountConfig } from '@abp/ng.account/config';
import { provideIdentityConfig } from '@abp/ng.identity/config';
import { provideTenantManagementConfig } from '@abp/ng.tenant-management/config';
import { provideFeatureManagementConfig } from '@abp/ng.feature-management';
import { provideThemeLeptonX } from '@abp/ng.theme.lepton-x';
import { provideSideMenuLayout } from '@abp/ng.theme.lepton-x/layouts';
import { provideLogo, withEnvironmentOptions } from '@volo/ngx-lepton-x.core';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(appRoutes),
    APP_ROUTE_PROVIDER,
    provideAbpCore(
      withOptions({
        environment,
        registerLocaleFn: registerLocale(),
      })
    ),
    provideThemeLeptonX(),
    provideSideMenuLayout(),
    provideAbpOAuth(),
    provideAbpThemeShared(),
    provideSettingManagementConfig(),
    provideAccountConfig(),
    provideIdentityConfig(),
    provideTenantManagementConfig(),
    provideFeatureManagementConfig(),
    provideAnimations(),
    provideLogo(withEnvironmentOptions(environment)),
  ],
};
