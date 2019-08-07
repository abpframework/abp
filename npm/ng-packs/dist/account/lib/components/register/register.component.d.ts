import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
export declare class RegisterComponent {
    private fb;
    private oauthService;
    private router;
    form: FormGroup;
    constructor(fb: FormBuilder, oauthService: OAuthService, router: Router);
    onSubmit(): void;
}
