import { inject, Provider } from '@angular/core';
import { EnvironmentService, NAVIGATE_TO_MANAGE_PROFILE } from '@abp/ng.core';

export const NavigateToManageProfileProvider: Provider = {
  provide: NAVIGATE_TO_MANAGE_PROFILE,
  useFactory: () => {
    const environment = inject(EnvironmentService);

    return () => {
      const env = environment.getEnvironment();
      if (!env.oAuthConfig) {
        console.warn('The oAuthConfig env is missing on environment.ts');
        return;
      }

      const { issuer } = env.oAuthConfig;
      const path = issuer.endsWith('/') ? issuer : `${issuer}/`;
      window.open(`${path}Account/Manage?returnUrl=${window.location.href}`, '_self');
    };
  },
};
