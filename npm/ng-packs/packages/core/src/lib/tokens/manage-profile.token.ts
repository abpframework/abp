import { InjectionToken, inject } from '@angular/core';
import { EnvironmentService } from '../services/environment.service';

export const NAVIGATE_TO_MANAGE_PROFILE = new InjectionToken<() => void>(
  'NAVIGATE_TO_MANAGE_PROFILE',
  {
    providedIn: 'root',
    factory: () => {
      const environment = inject(EnvironmentService);

      return () => {
        window.open(
          `${environment.getEnvironment().oAuthConfig.issuer}/Account/Manage?returnUrl=${
            window.location.href
          }`,
          '_self',
        );
      };
    },
  },
);
