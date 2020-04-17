import { ABP } from '@abp/ng.core';
import { Confirmation, ConfirmationService, getPasswordValidators } from '@abp/ng.theme.shared';
import { Component, OnInit, TemplateRef, TrackByFunction, ViewChild } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize, pluck, switchMap, take } from 'rxjs/operators';
import snq from 'snq';
import {
  CreateUser,
  DeleteUser,
  GetUserById,
  GetUserRoles,
  GetUsers,
  UpdateUser,
} from '../../actions/identity.actions';
import { Identity } from '../../models/identity';
import { IdentityService } from '../../services/identity.service';
import { IdentityState } from '../../states/identity.state';
import { ePermissionManagementComponents } from '@abp/ng.permission-management';
@Component({
  selector: 'abp-users',
  templateUrl: './users.component.html',
})
export class UsersComponent implements OnInit {
  @Select(IdentityState.getUsers)
  data$: Observable<Identity.UserItem[]>;

  @Select(IdentityState.getUsersTotalCount)
  totalCount$: Observable<number>;

  @ViewChild('modalContent', { static: false })
  modalContent: TemplateRef<any>;

  form: FormGroup;

  selected: Identity.UserItem;

  selectedUserRoles: Identity.RoleItem[];

  roles: Identity.RoleItem[];

  visiblePermissions = false;

  providerKey: string;

  pageQuery: ABP.PageQueryParams = { maxResultCount: 10 };

  isModalVisible: boolean;

  loading = false;

  modalBusy = false;

  sortOrder = '';

  sortKey = '';

  permissionManagementKey = ePermissionManagementComponents.PermissionManagement;

  trackByFn: TrackByFunction<AbstractControl> = (index, item) => Object.keys(item)[0] || index;

  onVisiblePermissionChange = event => {
    this.visiblePermissions = event;
  };

  get roleGroups(): FormGroup[] {
    return snq(() => (this.form.get('roleNames') as FormArray).controls as FormGroup[], []);
  }

  constructor(
    private confirmationService: ConfirmationService,
    private fb: FormBuilder,
    private store: Store,
    private identityService: IdentityService,
  ) {}

  ngOnInit() {
    this.get();
  }

  onSearch(value: string) {
    this.pageQuery.filter = value;
    this.get();
  }

  buildForm() {
    this.identityService.getAllRoles().subscribe(({ items }) => {
      this.roles = items;
      this.form = this.fb.group({
        userName: [this.selected.userName || '', [Validators.required, Validators.maxLength(256)]],
        email: [
          this.selected.email || '',
          [Validators.required, Validators.email, Validators.maxLength(256)],
        ],
        name: [this.selected.name || '', [Validators.maxLength(64)]],
        surname: [this.selected.surname || '', [Validators.maxLength(64)]],
        phoneNumber: [this.selected.phoneNumber || '', [Validators.maxLength(16)]],
        lockoutEnabled: [this.selected.lockoutEnabled || (this.selected.id ? false : true)],
        twoFactorEnabled: [this.selected.twoFactorEnabled || (this.selected.id ? false : true)],
        roleNames: this.fb.array(
          this.roles.map(role =>
            this.fb.group({
              [role.name]: [
                this.selected.id
                  ? !!snq(() => this.selectedUserRoles.find(userRole => userRole.id === role.id))
                  : role.isDefault,
              ],
            }),
          ),
        ),
      });

      const passwordValidators = getPasswordValidators(this.store);

      this.form.addControl('password', new FormControl('', [...passwordValidators]));

      if (!this.selected.userName) {
        this.form.get('password').setValidators([...passwordValidators, Validators.required]);
        this.form.get('password').updateValueAndValidity();
      }
    });
  }

  openModal() {
    this.buildForm();
    this.isModalVisible = true;
  }

  add() {
    this.selected = {} as Identity.UserItem;
    this.selectedUserRoles = [] as Identity.RoleItem[];
    this.openModal();
  }

  edit(id: string) {
    this.store
      .dispatch(new GetUserById(id))
      .pipe(
        switchMap(() => this.store.dispatch(new GetUserRoles(id))),
        pluck('IdentityState'),
        take(1),
      )
      .subscribe((state: Identity.State) => {
        this.selected = state.selectedUser;
        this.selectedUserRoles = state.selectedUserRoles || [];
        this.openModal();
      });
  }

  save() {
    if (!this.form.valid || this.modalBusy) return;
    this.modalBusy = true;

    const { roleNames } = this.form.value;
    const mappedRoleNames = snq(
      () =>
        roleNames.filter(role => !!role[Object.keys(role)[0]]).map(role => Object.keys(role)[0]),
      [],
    );

    this.store
      .dispatch(
        this.selected.id
          ? new UpdateUser({
              ...this.selected,
              ...this.form.value,
              id: this.selected.id,
              roleNames: mappedRoleNames,
            })
          : new CreateUser({
              ...this.form.value,
              roleNames: mappedRoleNames,
            }),
      )
      .pipe(finalize(() => (this.modalBusy = false)))
      .subscribe(() => {
        this.isModalVisible = false;
        this.get();
      });
  }

  delete(id: string, userName: string) {
    this.confirmationService
      .warn('AbpIdentity::UserDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
        messageLocalizationParams: [userName],
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.store.dispatch(new DeleteUser(id)).subscribe(() => this.get());
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
      .dispatch(new GetUsers(this.pageQuery))
      .pipe(finalize(() => (this.loading = false)))
      .subscribe();
  }

  openPermissionsModal(providerKey: string) {
    this.providerKey = providerKey;
    setTimeout(() => {
      this.visiblePermissions = true;
    }, 0);
  }
}
