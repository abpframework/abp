import { EventEmitter, OnChanges, SimpleChanges, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { Profile } from '@abp/ng.core';
export declare class ProfileComponent implements OnChanges {
    private fb;
    private modalService;
    private store;
    visible: boolean;
    visibleChange: EventEmitter<boolean>;
    modalContent: TemplateRef<any>;
    profile$: Observable<Profile.Response>;
    form: FormGroup;
    modalRef: NgbModalRef;
    constructor(fb: FormBuilder, modalService: NgbModal, store: Store);
    buildForm(): void;
    onSubmit(): void;
    openModal(): void;
    setVisible(value: boolean): void;
    ngOnChanges({ visible }: SimpleChanges): void;
}
