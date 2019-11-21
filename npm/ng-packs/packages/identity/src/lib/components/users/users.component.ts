import { ABP, ConfigState } from '@abp/ng.core';
import { ConfirmationService, Toaster } from '@abp/ng.theme.shared';
import { Component, TemplateRef, TrackByFunction, ViewChild, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
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
  GetRoles,
} from '../../actions/identity.actions';
import { Identity } from '../../models/identity';
import { IdentityState } from '../../states/identity.state';
import { PasswordRules, validatePassword } from '@ngx-validate/core';
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

  pageQuery: ABP.PageQueryParams = {};

  isModalVisible: boolean;

  loading = false;

  modalBusy = false;

  sortOrder = '';

  sortKey = '';

  passwordRulesArr = [] as PasswordRules;

  requiredPasswordLength = 1;

  trackByFn: TrackByFunction<AbstractControl> = (index, item) => Object.keys(item)[0] || index;

  get roleGroups(): FormGroup[] {
    return snq(() => (this.form.get('roleNames') as FormArray).controls as FormGroup[], []);
  }

  constructor(private confirmationService: ConfirmationService, private fb: FormBuilder, private store: Store) {}

  ngOnInit() {
    this.get();

    const passwordRules: ABP.Dictionary<string> = this.store.selectSnapshot(
      ConfigState.getSettings('Identity.Password'),
    );

    if ((passwordRules['Abp.Identity.Password.RequireDigit'] || '').toLowerCase() === 'true') {
      this.passwordRulesArr.push('number');
    }

    if ((passwordRules['Abp.Identity.Password.RequireLowercase'] || '').toLowerCase() === 'true') {
      this.passwordRulesArr.push('small');
    }

    if ((passwordRules['Abp.Identity.Password.RequireUppercase'] || '').toLowerCase() === 'true') {
      this.passwordRulesArr.push('capital');
    }

    if (+(passwordRules['Abp.Identity.Password.RequiredUniqueChars'] || 0) > 0) {
      this.passwordRulesArr.push('special');
    }

    if (Number.isInteger(+passwordRules['Abp.Identity.Password.RequiredLength'])) {
      this.requiredPasswordLength = +passwordRules['Abp.Identity.Password.RequiredLength'];
    }
  }

  onSearch(value) {
    this.pageQuery.filter = value;
    this.get();
  }

  buildForm() {
    this.store.dispatch(new GetRoles()).subscribe(() => {
      this.roles = this.store.selectSnapshot(IdentityState.getRoles);
      this.form = this.fb.group({
        userName: [this.selected.userName || '', [Validators.required, Validators.maxLength(256)]],
        email: [this.selected.email || '', [Validators.required, Validators.email, Validators.maxLength(256)]],
        name: [this.selected.name || '', [Validators.maxLength(64)]],
        surname: [this.selected.surname || '', [Validators.maxLength(64)]],
        phoneNumber: [this.selected.phoneNumber || '', [Validators.maxLength(16)]],
        lockoutEnabled: [this.selected.twoFactorEnabled || (this.selected.id ? false : true)],
        twoFactorEnabled: [this.selected.twoFactorEnabled || (this.selected.id ? false : true)],
        roleNames: this.fb.array(
          this.roles.map(role =>
            this.fb.group({
              [role.name]: [!!snq(() => this.selectedUserRoles.find(userRole => userRole.id === role.id))],
            }),
          ),
        ),
      });

      const passwordValidators = [
        validatePassword(this.passwordRulesArr),
        Validators.minLength(this.requiredPasswordLength),
        Validators.maxLength(32),
      ];

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
        this.selectedUserRoles = state.selectedUserRoles;
        this.openModal();
      });
  }

  save() {
    if (!this.form.valid || this.modalBusy) return;
    this.modalBusy = true;

    const { roleNames } = this.form.value;
    const mappedRoleNames = snq(
      () => roleNames.filter(role => !!role[Object.keys(role)[0]]).map(role => Object.keys(role)[0]),
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
      });
  }

  delete(id: string, userName: string) {
    this.confirmationService
      .warn('AbpIdentity::UserDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
        messageLocalizationParams: [userName],
      })
      .subscribe((status: Toaster.Status) => {
        if (status === Toaster.Status.confirm) {
          this.store.dispatch(new DeleteUser(id));
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
    this.store
      .dispatch(new GetUsers(this.pageQuery))
      .pipe(finalize(() => (this.loading = false)))
      .subscribe();
  }
}
