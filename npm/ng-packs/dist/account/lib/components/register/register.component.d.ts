import { ToasterService } from '@abp/ng.theme.shared';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { AccountService } from '../../services/account.service';
export declare class RegisterComponent {
    private fb;
    private accountService;
    private oauthService;
    private store;
    private toasterService;
    form: FormGroup;
    inProgress: boolean;
    constructor(fb: FormBuilder, accountService: AccountService, oauthService: OAuthService, store: Store, toasterService: ToasterService);
    onSubmit(): void;
}
