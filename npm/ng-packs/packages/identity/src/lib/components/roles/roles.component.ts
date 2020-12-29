import { ListService, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import { ePermissionManagementComponents } from '@abp/ng.permission-management';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import {
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.theme.shared/extensions';
import { Component, ElementRef, Injector, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
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
import { eIdentityComponents } from '../../enums/components';
import { IdentityRoleDto } from '../../proxy/identity/models';
import { IdentityState } from '../../states/identity.state';

@Component({
  selector: 'abp-roles',
  templateUrl: './roles.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eIdentityComponents.Roles,
    },
  ],
})
export class RolesComponent implements OnInit {
  @Select(IdentityState.getRoles)
  data$: Observable<IdentityRoleDto[]>;

  @Select(IdentityState.getRolesTotalCount)
  totalCount$: Observable<number>;

  form: FormGroup;

  selected: IdentityRoleDto;

  isModalVisible: boolean;

  visiblePermissions = false;

  providerKey: string;

  modalBusy = false;

  permissionManagementKey = ePermissionManagementComponents.PermissionManagement;

  @ViewChild('formRef', { static: false, read: ElementRef })
  formRef: ElementRef<HTMLFormElement>;

  onVisiblePermissionChange = event => {
    this.visiblePermissions = event;
  };

  constructor(
    public readonly list: ListService<PagedAndSortedResultRequestDto>,
    protected confirmationService: ConfirmationService,
    protected store: Store,
    protected injector: Injector,
  ) {}

  ngOnInit() {
    this.hookToQuery();
  }

  buildForm() {
    const data = new FormPropData(this.injector, this.selected);
    this.form = generateFormFromProps(data);
  }

  openModal() {
    this.buildForm();
    this.isModalVisible = true;
  }

  add() {
    this.selected = {} as IdentityRoleDto;
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
        this.list.get();
      });
  }

  delete(id: string, name: string) {
    this.confirmationService
      .warn('AbpIdentity::RoleDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
        messageLocalizationParams: [name],
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.store.dispatch(new DeleteRole(id)).subscribe(() => this.list.get());
        }
      });
  }

  private hookToQuery() {
    this.list.hookToQuery(query => this.store.dispatch(new GetRoles(query))).subscribe();
  }

  onClickSaveButton() {
    this.formRef.nativeElement.dispatchEvent(
      new Event('submit', { bubbles: true, cancelable: true }),
    );
  }

  openPermissionsModal(providerKey: string) {
    this.providerKey = providerKey;
    setTimeout(() => {
      this.visiblePermissions = true;
    }, 0);
  }

  sort(data) {
    const { prop, dir } = data.sorts[0];
    this.list.sortKey = prop;
    this.list.sortOrder = dir;
  }
}
