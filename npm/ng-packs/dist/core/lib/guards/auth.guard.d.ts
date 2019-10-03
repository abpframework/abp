import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
export declare class AuthGuard implements CanActivate {
    private oauthService;
    private store;
    private router;
    constructor(oauthService: OAuthService, store: Store, router: Router);
    canActivate(_: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean | UrlTree;
}
