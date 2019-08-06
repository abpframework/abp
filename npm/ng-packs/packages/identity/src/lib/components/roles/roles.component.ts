import { Component, TemplateRef, ViewChild, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable, Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IdentityState } from '../../states/identity.state';
import { Identity } from '../../models/identity';
import {
  IdentityUpdateRole,
  IdentityAddRole,
  IdentityDeleteRole,
  IdentityGetRoleById,
  IdentityGetRoles,
} from '../../actions/identity.actions';
import { pluck, debounceTime, finalize } from 'rxjs/operators';
import { ConfirmationService, Toaster } from '@abp/ng.theme.shared';
import { ABP } from '@abp/ng.core';

@Component({
  selector: 'abp-roles',
  templateUrl: './roles.component.html',
})
export class RolesComponent implements OnInit {
  @Select(IdentityState.getRoles)
  data$: Observable<Identity.RoleItem[]>;

  @Select(IdentityState.getRolesTotalCount)
  totalCount$: Observable<number>;

  form: FormGroup;

  selected: Identity.RoleItem;

  isModalVisible: boolean;

  visiblePermissions: boolean = false;

  providerKey: string;

  pageQuery: ABP.PageQueryParams = {
    sorting: 'name',
  };

  loading: boolean = false;

  search$ = new Subject<string>();

  @ViewChild('modalContent', { static: false })
  modalContent: TemplateRef<any>;

  constructor(private confirmationService: ConfirmationService, private fb: FormBuilder, private store: Store) {}

  ngOnInit() {
    this.search$.pipe(debounceTime(300)).subscribe(value => {
      this.pageQuery.filter = value;
      this.get();
    });
  }

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

  onPageChange(data) {
    this.pageQuery.skipCount = data.first;
    this.pageQuery.maxResultCount = data.rows;

    this.get();
  }

  get() {
    this.loading = true;
    console.warn(this.pageQuery);
    this.store
      .dispatch(new IdentityGetRoles(this.pageQuery))
      .pipe(finalize(() => (this.loading = false)))
      .subscribe();
  }
}
