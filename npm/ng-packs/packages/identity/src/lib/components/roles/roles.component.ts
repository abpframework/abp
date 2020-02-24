import { ABP } from '@abp/ng.core';
import { ConfirmationService, Toaster } from '@abp/ng.theme.shared';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize, pluck } from 'rxjs/operators';
import {
  CreateRole,
  DeleteRole,
  GetRoleById,
  GetRoles,
  UpdateRole,
} from '../../actions/identity.actions';
import { Identity } from '../../models/identity';
import { IdentityState } from '../../states/identity.state';

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

  visiblePermissions = false;

  providerKey: string;

  pageQuery: ABP.PageQueryParams = { maxResultCount: 10 };

  loading = false;

  modalBusy = false;

  sortOrder = '';

  sortKey = '';

  @ViewChild('formRef', { static: false, read: ElementRef })
  formRef: ElementRef<HTMLFormElement>;

  onVisiblePermissionChange = event => {
    this.visiblePermissions = event;
  };

  constructor(
    private confirmationService: ConfirmationService,
    private fb: FormBuilder,
    private store: Store,
  ) {}

  ngOnInit() {
    this.get();
  }

  buildForm() {
    this.form = this.fb.group({
      name: new FormControl({ value: this.selected.name || '', disabled: this.selected.isStatic }, [
        Validators.required,
        Validators.maxLength(256),
      ]),
      isDefault: [this.selected.isDefault || false],
      isPublic: [this.selected.isPublic || false],
    });
  }

  openModal() {
    this.buildForm();
    this.isModalVisible = true;
  }

  add() {
    this.selected = {} as Identity.RoleItem;
    this.openModal();
  }

  edit(id: string) {
    this.store
      .dispatch(new GetRoleById(id))
      .pipe(pluck('IdentityState', 'selectedRole'))
      .subscribe(selectedRole => {
        this.selected = selectedRole;
        this.openModal();
      });
  }

  save() {
    if (!this.form.valid) return;
    this.modalBusy = true;

    this.store
      .dispatch(
        this.selected.id
          ? new UpdateRole({ ...this.selected, ...this.form.value, id: this.selected.id })
          : new CreateRole(this.form.value),
      )
      .pipe(finalize(() => (this.modalBusy = false)))
      .subscribe(() => {
        this.isModalVisible = false;
        this.get();
      });
  }

  delete(id: string, name: string) {
    this.confirmationService
      .warn('AbpIdentity::RoleDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
        messageLocalizationParams: [name],
      })
      .subscribe((status: Toaster.Status) => {
        if (status === Toaster.Status.confirm) {
          this.store.dispatch(new DeleteRole(id)).subscribe(() => this.get());
        }
      });
  }

  onPageChange(page: number) {
    this.pageQuery.skipCount = (page - 1) * this.pageQuery.maxResultCount;

    this.get();
  }

  get() {
    this.loading = true;
    this.store
      .dispatch(new GetRoles(this.pageQuery))
      .pipe(finalize(() => (this.loading = false)))
      .subscribe();
  }

  onClickSaveButton() {
    this.formRef.nativeElement.dispatchEvent(
      new Event('submit', { bubbles: true, cancelable: true }),
    );
  }
}
