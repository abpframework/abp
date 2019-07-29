import { ActivatedRouteSnapshot, CanActivate } from '@angular/router';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
export declare class PermissionGuard implements CanActivate {
    private store;
    constructor(store: Store);
    canActivate({ data }: ActivatedRouteSnapshot): Observable<boolean>;
}
