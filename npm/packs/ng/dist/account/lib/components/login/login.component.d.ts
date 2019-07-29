import { FormBuilder, FormGroup } from '@angular/forms';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { Options } from '../../models/options';
export declare class LoginComponent {
    private fb;
    private oauthService;
    private store;
    private options;
    form: FormGroup;
    constructor(fb: FormBuilder, oauthService: OAuthService, store: Store, options: Options);
    onSubmit(): void;
}
