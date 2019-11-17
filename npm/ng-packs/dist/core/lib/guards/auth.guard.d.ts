import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
export declare class AuthGuard implements CanActivate {
    private oauthService;
    private router;
    constructor(oauthService: OAuthService, router: Router);
    canActivate(_: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean | UrlTree;
}
