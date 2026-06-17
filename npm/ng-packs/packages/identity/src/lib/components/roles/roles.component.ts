import { Component, inject, Injector, signal, ChangeDetectionStrategy } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { ReactiveFormsModule, UntypedFormGroup } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import {
  InitDirective,
  ListService,
  LocalizationPipe,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
  ReplaceableTemplateDirective,
} from '@abp/ng.core';
import { IdentityRoleDto, IdentityRoleService } from '@abp/ng.identity/proxy';
import {
  ePermissionManagementComponents,
  PermissionManagementComponent,
} from '@abp/ng.permission-management';
import {
  ButtonComponent,
  Confirmation,
  ConfirmationService,
  ModalCloseDirective,
  ModalComponent,
  ToasterService,
} from '@abp/ng.theme.shared';
import {
  ExtensibleFormComponent,
  ExtensibleTableComponent,
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.components/extensible';
import { PageComponent } from '@abp/ng.components/page';
import { eIdentityComponents } from '../../enums/components';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-roles',
  templateUrl: './roles.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eIdentityComponents.Roles,
    },
  ],
  imports: [
    ReactiveFormsModule,
    LocalizationPipe,
    ExtensibleTableComponent,
    ModalComponent,
    ButtonComponent,
    PageComponent,
    ExtensibleFormComponent,
    ModalCloseDirective,
    PermissionManagementComponent,
    ReplaceableTemplateDirective,
    NgxValidateCoreModule,
    InitDirective,
  ],
})
export class RolesComponent {
  protected readonly list = inject(ListService<PagedAndSortedResultRequestDto>);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly toasterService = inject(ToasterService);
  private readonly injector = inject(Injector);
  protected readonly service = inject(IdentityRoleService);

  readonly data = toSignal(
    this.list.hookToQuery(query => this.service.getList(query)),
    { initialValue: { items: [], totalCount: 0 } as PagedResultDto<IdentityRoleDto> },
  );

  form!: UntypedFormGroup;
  selected?: IdentityRoleDto;

  readonly isModalVisible = signal(false);
  readonly visiblePermissions = signal(false);

  providerKey?: string;
  readonly modalBusy = signal(false);

  permissionManagementKey = ePermissionManagementComponents.PermissionManagement;

  onVisiblePermissionChange = (event: boolean) => {
    this.visiblePermissions.set(event);
  };

  buildForm() {
    const data = new FormPropData(this.injector, this.selected);
    this.form = generateFormFromProps(data);
  }

  openModal() {
    this.buildForm();
    this.isModalVisible.set(true);
  }

  add() {
    this.selected = {} as IdentityRoleDto;
    this.openModal();
  }

  edit(id: string) {
    this.service.get(id).subscribe(res => {
      this.selected = res;
      this.openModal();
    });
  }

  save() {
    if (!this.form.valid) return;
    this.modalBusy.set(true);

    const { id } = this.selected || {};
    (id
      ? this.service.update(id, { ...this.selected, ...this.form.value })
      : this.service.create(this.form.value)
    )
      .pipe(finalize(() => this.modalBusy.set(false)))
      .subscribe(() => {
        this.isModalVisible.set(false);
        this.toasterService.success('AbpUi::SavedSuccessfully');
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
          this.service.delete(id).subscribe(() => {
            this.toasterService.success('AbpUi::DeletedSuccessfully');
            this.list.get();
          });
        }
      });
  }

  openPermissionsModal(providerKey: string) {
    this.providerKey = providerKey;
    setTimeout(() => {
      this.visiblePermissions.set(true);
    }, 0);
  }

  sort(data: any) {
    const { prop, dir } = data.sorts[0];
    this.list.sortKey = prop;
    this.list.sortOrder = dir;
  }
}
