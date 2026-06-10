import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { filter, map, switchMap, take } from 'rxjs/operators';
import { AuthService, IAbpGuard } from '../abstracts';
import { findRoute, getRoutePath } from '../utils/route-utils';
import { RoutesService, PermissionService, HttpErrorReporterService, ConfigStateService } from '../services';
import { isPlatformServer } from '@angular/common';
/**
 * @deprecated Use `permissionGuard` *function* instead.
 */
@Injectable({
  providedIn: 'root',
})
export class PermissionGuard implements IAbpGuard {
  protected readonly router = inject(Router);
  protected readonly routesService = inject(RoutesService);
  protected readonly authService = inject(AuthService);
  protected readonly permissionService = inject(PermissionService);
  protected readonly httpErrorReporter = inject(HttpErrorReporterService);
  protected readonly configStateService = inject(ConfigStateService);

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> {
    let { requiredPolicy } = route.data || {};

    if (!requiredPolicy) {
      const routeFound = findRoute(this.routesService, getRoutePath(this.router, state.url));
      requiredPolicy = routeFound?.requiredPolicy;
    }

    if (!requiredPolicy) {
      return of(true);
    }

    return this.configStateService.getAll$().pipe(
      filter(config => !!config?.auth?.grantedPolicies),
      take(1),
      switchMap(() => this.permissionService.getGrantedPolicy$(requiredPolicy)),
      take(1),
      map(access => {
        if (access) return true;

        if (route.data?.['redirectUrl']) {
          return this.router.parseUrl(route.data['redirectUrl']);
        }

        if (this.authService.isAuthenticated) {
          this.httpErrorReporter.reportError({ status: 403 } as HttpErrorResponse);
        }
        return false;
      }),
    );
  }
}

export const permissionGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot,
) => {
  const router = inject(Router);
  const routesService = inject(RoutesService);
  const authService = inject(AuthService);
  const permissionService = inject(PermissionService);
  const httpErrorReporter = inject(HttpErrorReporterService);
  const configStateService = inject(ConfigStateService);
  const platformId = inject(PLATFORM_ID);

  let { requiredPolicy } = route.data || {};

  if (!requiredPolicy) {
    const routeFound = findRoute(routesService, getRoutePath(router, state.url));
    requiredPolicy = routeFound?.requiredPolicy;
  }

  if (!requiredPolicy) {
    return of(true);
  }

  //TODO enable permission check on ssr
  if (isPlatformServer(platformId)) {
    return of(true);
  }

  return configStateService.getAll$().pipe(
    filter(config => !!config?.auth?.grantedPolicies),
    take(1),
    switchMap(() => permissionService.getGrantedPolicy$(requiredPolicy)),
    take(1),
    map(access => {
      if (access) return true;

      if (route.data?.['redirectUrl']) {
        return router.parseUrl(route.data['redirectUrl']);
      }

      if (authService.isAuthenticated) {
        httpErrorReporter.reportError({ status: 403 } as HttpErrorResponse);
      }

      return false;
    }),
  );
};
