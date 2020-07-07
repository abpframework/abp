import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';
import snq from 'snq';
import { RestOccurError } from '../actions/rest.actions';
import { ConfigState } from '../states/config.state';
import { RoutesService } from '../services/routes.service';
import { findRoute, getRoutePath } from '../utils/route-utils';

@Injectable({
  providedIn: 'root',
})
export class PermissionGuard implements CanActivate {
  constructor(private router: Router, private routes: RoutesService, private store: Store) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): Observable<boolean> | boolean {
    let { requiredPolicy } = route.data || {};

    if (!requiredPolicy) {
      requiredPolicy = findRoute(this.routes, getRoutePath(this.router, state.url))?.requiredPolicy;

      if (!requiredPolicy) return true;
    }

    return this.store.select(ConfigState.getGrantedPolicy(requiredPolicy)).pipe(
      tap(access => {
        if (!access) {
          this.store.dispatch(new RestOccurError({ status: 403 }));
        }
      }),
    );
  }
}
