import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Store } from '@ngxs/store';
import { Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';
import { RestOccurError } from '../actions/rest.actions';
import { RoutesService } from '../services/routes.service';
import { findRoute, getRoutePath } from '../utils/route-utils';
import { PermissionService } from '../services/permission.service';

@Injectable({
  providedIn: 'root',
})
export class PermissionGuard implements CanActivate {
  constructor(
    private router: Router,
    private routesService: RoutesService,
    private store: Store,
    private permissionService: PermissionService,
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
          this.store.dispatch(new RestOccurError({ status: 403 }));
        }
      }),
    );
  }
}
