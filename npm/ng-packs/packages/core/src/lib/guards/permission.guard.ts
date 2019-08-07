import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate } from '@angular/router';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ConfigState } from '../states';

@Injectable({
  providedIn: 'root',
})
export class PermissionGuard implements CanActivate {
  constructor(private store: Store) {}

  canActivate({ data }: ActivatedRouteSnapshot): Observable<boolean> {
    const resource = data.requiredPolicy as string;
    return this.store.select(ConfigState.getGrantedPolicy(resource));
  }
}
