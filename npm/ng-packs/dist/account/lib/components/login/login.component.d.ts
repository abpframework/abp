import { FormBuilder, FormGroup } from '@angular/forms';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { Options } from '../../models/options';
import { ToasterService } from '@abp/ng.theme.shared';
export declare class LoginComponent {
    private fb;
    private oauthService;
    private store;
    private toasterService;
    private options;
    form: FormGroup;
    inProgress: boolean;
    constructor(fb: FormBuilder, oauthService: OAuthService, store: Store, toasterService: ToasterService, options: Options);
    onSubmit(): void;
}
