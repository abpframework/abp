import { EventEmitter, OnChanges, OnInit, SimpleChanges, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngxs/store';
export declare class ChangePasswordComponent implements OnInit, OnChanges {
    private fb;
    private modalService;
    private store;
    visible: boolean;
    visibleChange: EventEmitter<boolean>;
    modalContent: TemplateRef<any>;
    form: FormGroup;
    modalRef: NgbModalRef;
    constructor(fb: FormBuilder, modalService: NgbModal, store: Store);
    ngOnInit(): void;
    onSubmit(): void;
    openModal(): void;
    setVisible(value: boolean): void;
    ngOnChanges({ visible }: SimpleChanges): void;
}
