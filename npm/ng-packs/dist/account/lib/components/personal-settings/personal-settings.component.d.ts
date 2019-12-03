import { Profile } from '@abp/ng.core';
import { OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ToasterService } from '@abp/ng.theme.shared';
export declare class PersonalSettingsComponent implements OnInit {
    private fb;
    private store;
    private toasterService;
    profile$: Observable<Profile.Response>;
    form: FormGroup;
    inProgress: boolean;
    constructor(fb: FormBuilder, store: Store, toasterService: ToasterService);
    ngOnInit(): void;
    buildForm(): void;
    submit(): void;
}
