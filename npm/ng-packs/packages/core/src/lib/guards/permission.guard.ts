import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';
import { HttpErrorReporterService } from '../services/http-error-reporter.service';
import { PermissionService } from '../services/permission.service';
import { RoutesService } from '../services/routes.service';
import { findRoute, getRoutePath } from '../utils/route-utils';

@Injectable({
  providedIn: 'root',
})
export class PermissionGuard implements CanActivate {
  constructor(
    private router: Router,
    private routesService: RoutesService,
    private permissionService: PermissionService,
    private httpErrorReporter: HttpErrorReporterService,
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    let { requiredPolicy } = route.data || {};

    if (!requiredPolicy) {
      const routeFound = findRoute(this.routesService, getRoutePath(this.router, state.url));
      requiredPolicy = routeFound?.requiredPolicy;
    }

    if (!requiredPolicy) return of(true);

    return this.permissionService.getGrantedPolicy$(requiredPolicy).pipe(
      tap(access => {
        if (!access) {
          this.httpErrorReporter.reportError({ status: 403 } as HttpErrorResponse);
        }
      }),
    );
  }
}
