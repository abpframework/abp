import { Injector } from '@angular/core';
import { RoutesService } from '@abp/ng.core';
import { Router } from '@angular/router';
import { eAccountRouteNames } from '../enums/route-names';

export function navigateToManageProfileFactory(injector: Injector) {
  return () => {
    const router = injector.get(Router);
    const routes = injector.get(RoutesService);
    const { path } = routes.find(item => item.name === eAccountRouteNames.ManageProfile);
    router.navigateByUrl(path);
  };
}
