import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { Store } from '@ngxs/store';
import { Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';
import snq from 'snq';
import { RestOccurError } from '../actions';
import { ConfigState } from '../states';

@Injectable({
  providedIn: 'root',
})
export class PermissionGuard implements CanActivate {
  constructor(private store: Store) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    let resource =
      snq(() => route.data.routes.requiredPolicy) || snq(() => route.data.requiredPolicy as string);
    if (!resource) {
      resource = snq(
        () =>
          route.routeConfig.children.find(child => state.url.indexOf(child.path) > -1).data
            .requiredPolicy,
      );

      if (!resource) {
        return of(true);
      }
    }

    return this.store.select(ConfigState.getGrantedPolicy(resource)).pipe(
      tap(access => {
        if (!access) {
          this.store.dispatch(new RestOccurError({ status: 403 }));
        }
      }),
    );
  }
}
