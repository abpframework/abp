import { ListService, PagedResultDto } from "@abp/ng.core";
import { GetIdentityUsersInput, IdentityRoleDto, IdentityUserDto, IdentityUserService } from "@abp/ng.identity/proxy";
import { ePermissionManagementComponents } from "@abp/ng.permission-management";
import { Confirmation, ConfirmationService, ToasterService } from "@abp/ng.theme.shared";
import { EXTENSIONS_IDENTIFIER, FormPropData, generateFormFromProps } from "@abp/ng.theme.shared/extensions";
import { Component, Injector, OnInit, TemplateRef, TrackByFunction, ViewChild } from "@angular/core";
import { AbstractControl, UntypedFormArray, UntypedFormBuilder, UntypedFormGroup } from "@angular/forms";
import { finalize, switchMap, tap } from "rxjs/operators";
import { eIdentityComponents } from "../../enums/components";

@Component({
  selector: 'abp-users',
  templateUrl: './users.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eIdentityComponents.Users,
    },
  ],
})
export class UsersComponent implements OnInit {
  data: PagedResultDto<IdentityUserDto> = { items: [], totalCount: 0 };

  @ViewChild('modalContent', { static: false })
  modalContent!: TemplateRef<any>;

  form!: UntypedFormGroup;

  selected?: IdentityUserDto;

  selectedUserRoles?: IdentityRoleDto[];

  roles?: IdentityRoleDto[];

  visiblePermissions = false;

  providerKey?: string;

  isModalVisible?: boolean;

  modalBusy = false;

  permissionManagementKey = ePermissionManagementComponents.PermissionManagement;

  entityDisplayName?: string;

  trackByFn: TrackByFunction<AbstractControl> = (index, item) => Object.keys(item)[0] || index;

  onVisiblePermissionChange = (event: boolean) => {
    this.visiblePermissions = event;
  };

  get roleGroups(): UntypedFormGroup[] {
    return ((this.form.get('roleNames') as UntypedFormArray)?.controls as UntypedFormGroup[]) || [];
  }

  constructor(
    public readonly list: ListService<GetIdentityUsersInput>,
    protected confirmationService: ConfirmationService,
    protected service: IdentityUserService,
    private toasterService: ToasterService,
    protected fb: UntypedFormBuilder,
    protected injector: Injector,
  ) {}

  ngOnInit() {
    this.hookToQuery();
  }

  buildForm() {
    const data = new FormPropData(this.injector, this.selected);
    this.form = generateFormFromProps(data);

    this.service.getAssignableRoles().subscribe(({ items }) => {
      this.roles = items;
      if (this.roles) {
        this.form.addControl(
          'roleNames',
          this.fb.array(
            this.roles.map(role =>
              this.fb.group({
                [role.name as string]: [
                  this.selected?.id
                    ? !!this.selectedUserRoles?.find(userRole => userRole.id === role.id)
                    : role.isDefault,
                ],
              }),
            ),
          ),
        );
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
    this.service
      .get(id)
      .pipe(
        tap(user => (this.selected = user)),
        switchMap(() => this.service.getRoles(id)),
      )
      .subscribe(userRole => {
        this.selectedUserRoles = userRole.items || [];
        this.openModal();
      });
  }

  save() {
    if (!this.form.valid || this.modalBusy) return;
    this.modalBusy = true;

    const { roleNames = [] } = this.form.value;
    const mappedRoleNames =
      roleNames
        .filter((role: { [key: string]: any }) => !!role[Object.keys(role)[0]])
        .map((role: { [key: string]: any }) => Object.keys(role)[0]) || [];

    const { id } = this.selected || {};

    (id
      ? this.service.update(id, {
          ...this.selected,
          ...this.form.value,
          roleNames: mappedRoleNames,
        })
      : this.service.create({ ...this.form.value, roleNames: mappedRoleNames })
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
          this.service.delete(id).subscribe(() => {
            this.toasterService.success('AbpUi::SuccessfullyDeleted');
            this.list.get();
          });
        }
      });
  }

  sort(data: any) {
    const { prop, dir } = data.sorts[0];
    this.list.sortKey = prop;
    this.list.sortOrder = dir;
  }

  private hookToQuery() {
    this.list.hookToQuery(query => this.service.getList(query)).subscribe(res => (this.data = res));
  }

  openPermissionsModal(providerKey: string, entityDisplayName?: string) {
    this.providerKey = providerKey;
    this.entityDisplayName = entityDisplayName;
    setTimeout(() => {
      this.visiblePermissions = true;
    }, 0);
  }
}
