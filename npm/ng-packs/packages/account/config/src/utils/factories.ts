import { Router } from '@angular/router';
import { EnvironmentService } from '@abp/ng.core';

export const navigateToManageProfileFactory = (
  router: Router,
  environment: EnvironmentService,
) => () => {
  if (router.config.findIndex(route => route.path === 'account') > -1) {
    router.navigateByUrl('/account/manage');
    return;
  }

  window.open(
    `${environment.getEnvironment().oAuthConfig.issuer}/Account/Manage?returnUrl=${
      window.location.href
    }`,
    '_self',
  );
};
