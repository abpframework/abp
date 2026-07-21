import { Routes } from '@angular/router';
import { Provider } from '@angular/core';
import { RouterOutletComponent } from '@abp/ng.core';

export interface CmsKitPublicConfigOptions {
  // Extension point contributors
}

export function createRoutes(config: CmsKitPublicConfigOptions = {}): Routes {
  return [
    {
      path: '',
      component: RouterOutletComponent,
      providers: provideCmsKitPublicContributors(config),
      children: [
        // Routes will be added here
      ],
    },
  ];
}

function provideCmsKitPublicContributors(options: CmsKitPublicConfigOptions = {}): Provider[] {
  return [
    // Contributors will be added here
  ];
}
