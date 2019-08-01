import { TemplateRef } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Identity } from '../../models/identity';
import { ConfirmationService } from '@abp/ng.theme.shared';
export declare class RolesComponent {
    private confirmationService;
    private fb;
    private store;
    roles$: Observable<Identity.RoleItem[]>;
    form: FormGroup;
    selected: Identity.RoleItem;
    isModalVisible: boolean;
    visiblePermissions: boolean;
    providerKey: string;
    modalContent: TemplateRef<any>;
    constructor(confirmationService: ConfirmationService, fb: FormBuilder, store: Store);
    createForm(): void;
    openModal(): void;
    onAdd(): void;
    onEdit(id: string): void;
    save(): void;
    delete(id: string, name: string): void;
}
