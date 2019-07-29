import { Component, TemplateRef, ViewChild, TrackByFunction, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable, combineLatest, Subject } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators, FormArray, AbstractControl } from '@angular/forms';
import { IdentityState } from '../../states/identity.state';
import { Identity } from '../../models/identity';
import {
  IdentityUpdateUser,
  IdentityAddUser,
  IdentityDeleteUser,
  IdentityGetUserById,
  IdentityGetUsers,
} from '../../actions/identity.actions';
import { pluck, filter, map, take, debounceTime } from 'rxjs/operators';
import { ConfirmationService, Toaster } from '@abp/ng.theme.shared';
import snq from 'snq';
import { IdentityGetUserRoles } from '../../actions/identity.actions';
import { validatePassword } from '@ngx-validate/core';
import { ABP } from '@abp/ng.core';

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

  visiblePermissions: boolean = false;

  providerKey: string;

  pageQuery: ABP.PageQueryParams = {
    sorting: 'userName',
  };

  loading: boolean = false;

  search$ = new Subject<string>();

  trackByFn: TrackByFunction<AbstractControl> = (index, item) => Object.keys(item)[0] || index;

  get roleGroups(): FormGroup[] {
    return snq(() => (this.form.get('roleNames') as FormArray).controls as FormGroup[], []);
  }

  constructor(
    private confirmationService: ConfirmationService,
    private modalService: NgbModal,
    private fb: FormBuilder,
    private store: Store,
  ) {}

  ngOnInit() {
    this.search$.pipe(debounceTime(300)).subscribe(value => {
      this.pageQuery.filter = value;
      this.get();
    });
  }

  buildForm() {
    this.roles = this.store.selectSnapshot(IdentityState.getRoles);

    this.form = this.fb.group({
      password: [
        '',
        [
          Validators.required,
          Validators.maxLength(32),
          Validators.minLength(6),
          validatePassword(['small', 'capital', 'number', 'special']),
        ],
      ],
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
  }

  openModal() {
    this.buildForm();
    this.modalService.open(this.modalContent);
  }

  onAdd() {
    this.selected = {} as Identity.UserItem;
    this.selectedUserRoles = [] as Identity.RoleItem[];
    this.openModal();
  }

  onEdit(id: string) {
    combineLatest([this.store.dispatch(new IdentityGetUserById(id)), this.store.dispatch(new IdentityGetUserRoles(id))])
      .pipe(
        filter(([res1, res2]) => res1 && res2),
        map(([state, _]) => state),
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
    if (!this.form.valid) return;

    const { roleNames } = this.form.value;
    const mappedRoleNames = snq(
      () => roleNames.filter(role => !!role[Object.keys(role)[0]]).map(role => Object.keys(role)[0]),
      [],
    );

    this.store
      .dispatch(
        this.selected.id
          ? new IdentityUpdateUser({
              ...this.form.value,
              id: this.selected.id,
              roleNames: mappedRoleNames,
            })
          : new IdentityAddUser({ ...this.form.value, roleNames: mappedRoleNames }),
      )
      .subscribe(() => this.modalService.dismissAll());
  }

  delete(id: string, userName: string) {
    this.confirmationService
      .warn('AbpIdentity::UserDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
        messageLocalizationParams: [userName],
      })
      .subscribe((status: Toaster.Status) => {
        if (status === Toaster.Status.confirm) {
          this.store.dispatch(new IdentityDeleteUser(id));
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
    this.store.dispatch(new IdentityGetUsers(this.pageQuery)).subscribe(() => (this.loading = false));
  }
}
