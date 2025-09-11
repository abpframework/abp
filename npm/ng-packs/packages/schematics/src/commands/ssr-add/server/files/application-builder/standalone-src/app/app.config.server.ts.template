import {
  mergeApplicationConfig,
  ApplicationConfig,
  provideAppInitializer,
  inject,
  PLATFORM_ID,
  TransferState
} from '@angular/core';
import { isPlatformServer } from '@angular/common';
import { provideServerRendering, withRoutes } from '@angular/ssr';

import { appConfig } from './app.config';
import { serverRoutes } from './app.routes.server';
import { SSR_FLAG } from '@abp/ng.core';

const serverConfig: ApplicationConfig = {
  providers: [
    provideAppInitializer(() => {
      const platformId = inject(PLATFORM_ID);
      const transferState = inject<TransferState>(TransferState);
      if (isPlatformServer(platformId)) {
        transferState.set(SSR_FLAG, true);
      }
    }),
    provideServerRendering(withRoutes(serverRoutes)),
  ],
};

export const config = mergeApplicationConfig(appConfig, serverConfig);
