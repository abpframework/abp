import { EventEmitter, OnChanges, OnInit, SimpleChanges, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Validation } from '@ngx-validate/core';
import { Store } from '@ngxs/store';
import { ToasterService } from '../../services/toaster.service';
export declare class ChangePasswordComponent implements OnInit, OnChanges {
    private fb;
    private store;
    private toasterService;
    protected _visible: any;
    visible: boolean;
    readonly visibleChange: EventEmitter<boolean>;
    modalContent: TemplateRef<any>;
    form: FormGroup;
    modalBusy: boolean;
    mapErrorsFn: Validation.MapErrorsFn;
    constructor(fb: FormBuilder, store: Store, toasterService: ToasterService);
    ngOnInit(): void;
    onSubmit(): void;
    openModal(): void;
    ngOnChanges({ visible }: SimpleChanges): void;
}
