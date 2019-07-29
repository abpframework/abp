import { TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ABP } from '@abp/ng.core';
export declare class TenantBoxComponent {
    private modalService;
    private fb;
    constructor(modalService: NgbModal, fb: FormBuilder);
    form: FormGroup;
    selected: ABP.BasicItem;
    modalContent: TemplateRef<any>;
    createForm(): void;
    openModal(): void;
    onSwitch(): void;
    save(): void;
}
