import { ToasterService } from '@abp/ng.theme.shared';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AccountService } from '../../services/account.service';
export declare class RegisterComponent {
    private fb;
    private accountService;
    private toasterService;
    form: FormGroup;
    inProgress: boolean;
    constructor(fb: FormBuilder, accountService: AccountService, toasterService: ToasterService);
    onSubmit(): void;
}
