import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IdentityState } from '../../states/identity.state';
import { Identity } from '../../models/identity';
import {
  IdentityUpdateRole,
  IdentityAddRole,
  IdentityDeleteRole,
  IdentityGetRoleById,
} from '../../actions/identity.actions';
import { pluck } from 'rxjs/operators';
import { ConfirmationService, Toaster } from '@abp/ng.theme.shared';

@Component({
  selector: 'abp-roles',
  templateUrl: './roles.component.html',
})
export class RolesComponent {
  @Select(IdentityState.getRoles)
  roles$: Observable<Identity.RoleItem[]>;

  form: FormGroup;

  selected: Identity.RoleItem;

  isModalVisible: boolean;

  visiblePermissions: boolean = false;

  providerKey: string;

  @ViewChild('modalContent', { static: false })
  modalContent: TemplateRef<any>;

  constructor(private confirmationService: ConfirmationService, private fb: FormBuilder, private store: Store) {}

  createForm() {
    this.form = this.fb.group({
      name: [this.selected.name || '', [Validators.required, Validators.maxLength(256)]],
      isDefault: [this.selected.isDefault || false],
      isPublic: [this.selected.isPublic || false],
    });
  }

  openModal() {
    this.createForm();
    this.isModalVisible = true;
  }

  onAdd() {
    this.selected = {} as Identity.RoleItem;
    this.openModal();
  }

  onEdit(id: string) {
    this.store
      .dispatch(new IdentityGetRoleById(id))
      .pipe(pluck('IdentityState', 'selectedRole'))
      .subscribe(selectedRole => {
        this.selected = selectedRole;
        this.openModal();
      });
  }

  save() {
    if (!this.form.valid) return;

    this.store
      .dispatch(
        this.selected.id
          ? new IdentityUpdateRole({ ...this.form.value, id: this.selected.id })
          : new IdentityAddRole(this.form.value),
      )
      .subscribe(() => {
        this.isModalVisible = false;
      });
  }

  delete(id: string, name: string) {
    this.confirmationService
      .warn('AbpIdentity::RoleDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
        messageLocalizationParams: [name],
      })
      .subscribe((status: Toaster.Status) => {
        if (status === Toaster.Status.confirm) {
          this.store.dispatch(new IdentityDeleteRole(id));
        }
      });
  }
}
