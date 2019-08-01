import { Profile } from '@abp/ng.core';
import { EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
export declare class ProfileComponent implements OnChanges {
    private fb;
    private store;
    protected _visible: any;
    visible: boolean;
    visibleChange: EventEmitter<boolean>;
    profile$: Observable<Profile.Response>;
    form: FormGroup;
    constructor(fb: FormBuilder, store: Store);
    buildForm(): void;
    onSubmit(): void;
    openModal(): void;
    ngOnChanges({ visible }: SimpleChanges): void;
}
