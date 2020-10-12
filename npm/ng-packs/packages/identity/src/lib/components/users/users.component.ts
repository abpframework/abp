import { ListService } from '@abp/ng.core';
import { ePermissionManagementComponents } from '@abp/ng.permission-management';
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
import { IdentityRoleService } from '../../proxy/identity/identity-role.service';
import { IdentityUserService } from '../../proxy/identity/identity-user.service';
import {
  GetIdentityUsersInput,
  IdentityRoleDto,
  IdentityUserDto,
} from '../../proxy/identity/models';
import { IdentityService } from '../../services/identity.service';
import { IdentityState } from '../../states/identity.state';

@Component({
  selector: 'abp-users',
  templateUrl: './users.component.html',
  providers: [ListService],
})
export class UsersComponent implements OnInit {
  @Select(IdentityState.getUsers)
  data$: Observable<IdentityUserDto[]>;

  @Select(IdentityState.getUsersTotalCount)
  totalCount$: Observable<number>;

  @ViewChild('modalContent', { static: false })
  modalContent: TemplateRef<any>;

  form: FormGroup;

  selected: IdentityUserDto;

  selectedUserRoles: IdentityRoleDto[];

  roles: IdentityRoleDto[];

  visiblePermissions = false;

  providerKey: string;

  isModalVisible: boolean;

  modalBusy = false;

  permissionManagementKey = ePermissionManagementComponents.PermissionManagement;

  trackByFn: TrackByFunction<AbstractControl> = (index, item) => Object.keys(item)[0] || index;

  onVisiblePermissionChange = event => {
    this.visiblePermissions = event;
  };

  get roleGroups(): FormGroup[] {
    return snq(() => (this.form.get('roleNames') as FormArray).controls as FormGroup[], []);
  }

  constructor(
    public readonly list: ListService<GetIdentityUsersInput>,
    private confirmationService: ConfirmationService,
    private fb: FormBuilder,
    private store: Store,
    private identityService: IdentityService,
    private identityUserService: IdentityUserService,
  ) {}

  ngOnInit() {
    this.hookToQuery();
  }

  buildForm() {
    this.identityUserService.getAssignableRoles().subscribe(({ items }) => {
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
        lockoutEnabled: [this.selected.id ? this.selected.lockoutEnabled : true],
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
    this.selected = {} as IdentityUserDto;
    this.selectedUserRoles = [] as IdentityRoleDto[];
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
        this.list.get();
      });
  }

  delete(id: string, userName: string) {
    this.confirmationService
      .warn('AbpIdentity::UserDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
        messageLocalizationParams: [userName],
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.store.dispatch(new DeleteUser(id)).subscribe(() => this.list.get());
        }
      });
  }

  sort(data) {
    const { prop, dir } = data.sorts[0];
    this.list.sortKey = prop;
    this.list.sortOrder = dir;
  }

  private hookToQuery() {
    this.list.hookToQuery(query => this.store.dispatch(new GetUsers(query))).subscribe();
  }

  openPermissionsModal(providerKey: string) {
    this.providerKey = providerKey;
    setTimeout(() => {
      this.visiblePermissions = true;
    }, 0);
  }
}
