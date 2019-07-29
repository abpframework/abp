import { HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { OAuthService } from 'angular-oauth2-oidc';
import { Store } from '@ngxs/store';
export declare class ApiInterceptor implements HttpInterceptor {
    private oAuthService;
    private store;
    constructor(oAuthService: OAuthService, store: Store);
    intercept(request: HttpRequest<any>, next: HttpHandler): import("rxjs").Observable<import("@angular/common/http").HttpEvent<any>>;
}
