import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate } from '@angular/router';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ConfigState } from '../states';
import { tap } from 'rxjs/operators';
import { RestOccurError } from '../actions';

@Injectable({
  providedIn: 'root',
})
export class PermissionGuard implements CanActivate {
  constructor(private store: Store) {}

  canActivate({ data }: ActivatedRouteSnapshot): Observable<boolean> {
    const resource = data.requiredPolicy as string;
    return this.store.select(ConfigState.getGrantedPolicy(resource)).pipe(
      tap(access => {
        if (!access) {
          this.store.dispatch(new RestOccurError({ status: 403 }));
        }
      }),
    );
  }
}
