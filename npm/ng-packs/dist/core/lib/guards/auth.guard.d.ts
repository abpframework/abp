import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
export declare class AuthGuard implements CanActivate {
    private oauthService;
    private store;
    constructor(oauthService: OAuthService, store: Store);
    canActivate(_: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean | UrlTree;
}
