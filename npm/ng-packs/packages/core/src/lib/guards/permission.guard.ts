import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate } from '@angular/router';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import snq from 'snq';
import { RestOccurError } from '../actions';
import { ConfigState } from '../states';

@Injectable({
  providedIn: 'root',
})
export class PermissionGuard implements CanActivate {
  constructor(private store: Store) {}

  canActivate({ data }: ActivatedRouteSnapshot): Observable<boolean> {
    const resource = snq(() => data.routes.requiredPolicy) || (data.requiredPolicy as string);
    return this.store.select(ConfigState.getGrantedPolicy(resource)).pipe(
      tap(access => {
        if (!access) {
          this.store.dispatch(new RestOccurError({ status: 403 }));
        }
      }),
    );
  }
}
