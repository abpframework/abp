import { EventEmitter, OnChanges, OnInit, SimpleChanges, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Store } from '@ngxs/store';
export declare class ChangePasswordComponent implements OnInit, OnChanges {
    private fb;
    private store;
    protected _visible: any;
    visible: boolean;
    visibleChange: EventEmitter<boolean>;
    modalContent: TemplateRef<any>;
    form: FormGroup;
    constructor(fb: FormBuilder, store: Store);
    ngOnInit(): void;
    onSubmit(): void;
    openModal(): void;
    ngOnChanges({ visible }: SimpleChanges): void;
}
