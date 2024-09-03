import { ConfigStateService, CurrentUserDto } from '@abp/ng.core';
import {
  GetPermissionListResultDto,
  PermissionGrantInfoDto,
  PermissionGroupDto,
  PermissionsService,
  ProviderInfoDto,
  UpdatePermissionDto,
} from '@abp/ng.permission-management/proxy';
import { LocaleDirection, ToasterService } from '@abp/ng.theme.shared';
import {
  Component,
  ElementRef,
  EventEmitter,
  inject,
  Input,
  Output,
  QueryList,
  TrackByFunction,
  ViewChildren,
} from '@angular/core';
import { concat, of } from 'rxjs';
import { finalize, switchMap, take, tap } from 'rxjs/operators';
import { PermissionManagement } from '../models/permission-management';

type PermissionWithStyle = PermissionGrantInfoDto & {
  style: string;
};

type PermissionWithGroupName = PermissionGrantInfoDto & {
  groupName: string;
};

@Component({
  selector: 'abp-permission-management',
  templateUrl: './permission-management.component.html',
  exportAs: 'abpPermissionManagement',
  styles: [
    `
      .overflow-scroll {
        max-height: 70vh;
        overflow-y: scroll;
      }
      .scroll-in-modal {
        overflow: auto;
        max-height: calc(100vh - 15rem);
      }
    `,
  ],
})
export class PermissionManagementComponent
  implements
    PermissionManagement.PermissionManagementComponentInputs,
    PermissionManagement.PermissionManagementComponentOutputs
{
  protected readonly service = inject(PermissionsService);
  protected readonly configState = inject(ConfigStateService);
  protected readonly toasterService = inject(ToasterService);

  @Input()
  readonly providerName!: string;

  @Input()
  readonly providerKey!: string;

  @Input()
  readonly hideBadges = false;

  protected _visible = false;

  @Input()
  entityDisplayName: string | undefined;

  @Input()
  get visible(): boolean {
    return this._visible;
  }

  set visible(value: boolean) {
    if (value === this._visible) return;

    if (value) {
      this.openModal().subscribe(() => {
        this._visible = true;
        this.visibleChange.emit(true);
        concat(this.selectAllInAllTabsRef.changes, this.selectAllInThisTabsRef.changes)
          .pipe(take(1))
          .subscribe(() => {
            this.initModal();
          });
      });
    } else {
      this.setSelectedGroup(null);
      this._visible = false;
      this.visibleChange.emit(false);
    }
  }

  @Output() readonly visibleChange = new EventEmitter<boolean>();

  @ViewChildren('selectAllInThisTabsRef')
  selectAllInThisTabsRef!: QueryList<ElementRef<HTMLInputElement>>;
  @ViewChildren('selectAllInAllTabsRef')
  selectAllInAllTabsRef!: QueryList<ElementRef<HTMLInputElement>>;

  data: GetPermissionListResultDto = { groups: [], entityDisplayName: '' };

  selectedGroup?: PermissionGroupDto | null;

  permissions: PermissionWithGroupName[] = [];

  selectThisTab = false;

  selectAllTab = false;

  disableSelectAllTab = false;

  disabledSelectAllInAllTabs = false;

  modalBusy = false;

  selectedGroupPermissions: PermissionWithStyle[] = [];

  trackByFn: TrackByFunction<PermissionGroupDto> = (_, item) => item.name;

  getChecked(name: string) {
    return (this.permissions.find(per => per.name === name) || { isGranted: false }).isGranted;
  }

  setSelectedGroup(group: PermissionGroupDto) {
    this.selectedGroup = group;
    if (!this.selectedGroup) {
      this.selectedGroupPermissions = [];
      return;
    }

    const margin = `margin-${
      (document.body.dir as LocaleDirection) === 'rtl' ? 'right' : 'left'
    }.px`;

    const permissions =
      (this.data.groups.find(group => group.name === this.selectedGroup?.name) || {}).permissions ||
      [];
    this.selectedGroupPermissions = permissions.map(
      permission =>
        ({
          ...permission,
          style: { [margin]: findMargin(permissions, permission) },
          isGranted: (this.permissions.find(per => per.name === permission.name) || {}).isGranted,
        }) as unknown as PermissionWithStyle,
    );
  }

  setDisabled(permissions: PermissionGrantInfoDto[]) {
    if (permissions.length) {
      this.disableSelectAllTab = permissions.every(
        permission =>
          permission.isGranted &&
          permission.grantedProviders?.every(p => p.providerName !== this.providerName),
      );
    } else {
      this.disableSelectAllTab = false;
    }
  }

  isGrantedByOtherProviderName(grantedProviders: ProviderInfoDto[]): boolean {
    if (grantedProviders.length) {
      return grantedProviders.findIndex(p => p.providerName !== this.providerName) > -1;
    }
    return false;
  }

  onClickCheckbox(clickedPermission: PermissionGrantInfoDto) {
    const { isGranted, grantedProviders } = clickedPermission;
    if (isGranted && this.isGrantedByOtherProviderName(grantedProviders)) {
      return;
    }

    this.setSelectedGroup(this.selectedGroup);
    setTimeout(() => {
      this.updatePermissionStatus(clickedPermission);
      this.updateSelectedGroupPermissions(clickedPermission);
      this.setParentClicked(clickedPermission);
      this.setTabCheckboxState();
      this.setGrantCheckboxState();
    }, 0);
  }

  updatePermissionStatus(clickedPermission: PermissionGrantInfoDto) {
    this.permissions = this.permissions.map(permission => {
      const isExactMatch = clickedPermission.name == permission.name;
      const isParentOfPermission = clickedPermission.parentName === permission.name;
      const isChildOfPermission = clickedPermission.name === permission.parentName;

      if (isExactMatch) {
        return { ...permission, isGranted: !permission.isGranted };
      }

      if (isChildOfPermission && permission.isGranted) {
        return { ...permission, isGranted: false };
      }

      if (isParentOfPermission && !permission.isGranted) {
        return { ...permission, isGranted: true };
      }

      return permission;
    });
  }

  setParentClicked(clickedPermission: PermissionGrantInfoDto) {
    if (clickedPermission.parentName) {
      const parentPermissions = findParentPermissions(this.permissions, clickedPermission);
      if (parentPermissions.length > 0) {
        const parentNames = new Set(parentPermissions.map(parent => parent.name));

        this.permissions = this.permissions.map(per => {
          let updatedIsGranted = per.isGranted;

          if (per.parentName === clickedPermission.name && !clickedPermission.isGranted) {
            updatedIsGranted = false;
          }

          if (parentNames.has(per.name)) {
            updatedIsGranted = true;
          }

          return { ...per, isGranted: updatedIsGranted };
        });
      }
      return;
    }

    this.permissions = this.permissions.map(per => {
      const parents = findParentPermissions(this.permissions, per);
      if (parents.length > 0) {
        const rootParent = parents[parents.length - 1];

        if (rootParent.name === clickedPermission.name && !rootParent.isGranted) {
          return { ...per, isGranted: false };
        }
      }
      return per;
    });
  }

  updateSelectedGroupPermissions(clickedPermissions: PermissionGrantInfoDto) {
    this.selectedGroupPermissions = this.selectedGroupPermissions.map(per => {
      if (per.name === clickedPermissions.name) {
        per.isGranted = !per.isGranted;
      }
      return per;
    });
  }

  setTabCheckboxState() {
    const selectablePermissions = this.permissions.filter(per =>
      per.grantedProviders.every(p => p.providerName === this.providerName),
    );
    const selectedPermissions = selectablePermissions.filter(per => per.isGranted);
    const element = document.querySelector('#select-all-in-this-tabs') as any;

    if (selectedPermissions.length === selectablePermissions.length) {
      element.indeterminate = false;
      this.selectThisTab = true;
    } else if (selectedPermissions.length === 0) {
      element.indeterminate = false;
      this.selectThisTab = false;
    } else {
      element.indeterminate = true;
    }
  }

  setGrantCheckboxState() {
    const selectablePermissions = this.permissions.filter(per =>
      per.grantedProviders.every(p => p.providerName === this.providerName),
    );
    const selectedAllPermissions = selectablePermissions.filter(per => per.isGranted);
    const checkboxElement = document.querySelector('#select-all-in-all-tabs') as any;

    if (selectedAllPermissions.length === selectablePermissions.length) {
      checkboxElement.indeterminate = false;
      this.selectAllTab = true;
    } else if (selectedAllPermissions.length === 0) {
      checkboxElement.indeterminate = false;
      this.selectAllTab = false;
    } else {
      checkboxElement.indeterminate = true;
    }
  }

  onClickSelectThisTab() {
    this.selectedGroupPermissions.forEach(permission => {
      if (permission.isGranted && this.isGrantedByOtherProviderName(permission.grantedProviders))
        return;

      const index = this.permissions.findIndex(per => per.name === permission.name);

      this.permissions = [
        ...this.permissions.slice(0, index),
        { ...this.permissions[index], isGranted: !this.selectThisTab },
        ...this.permissions.slice(index + 1),
      ];
    });

    this.setGrantCheckboxState();
  }

  onClickSelectAll() {
    this.permissions = this.permissions.map(permission => ({
      ...permission,
      isGranted:
        this.isGrantedByOtherProviderName(permission.grantedProviders) || !this.selectAllTab,
    }));
    if (!this.disableSelectAllTab) {
      this.selectThisTab = !this.selectAllTab;
      this.setTabCheckboxState();
    }
    this.onChangeGroup(this.selectedGroup);
  }

  onChangeGroup(group: PermissionGroupDto) {
    this.setDisabled(group.permissions);
    this.setSelectedGroup(group);
    this.setTabCheckboxState();
  }

  submit() {
    const unchangedPermissions = getPermissions(this.data.groups);

    const changedPermissions: UpdatePermissionDto[] = this.permissions
      .filter(per =>
        (unchangedPermissions.find(unchanged => unchanged.name === per.name) || {}).isGranted ===
        per.isGranted
          ? false
          : true,
      )
      .map(({ name, isGranted }) => ({ name, isGranted }));

    if (!changedPermissions.length) {
      this.visible = false;
      return;
    }

    this.modalBusy = true;
    this.service
      .update(this.providerName, this.providerKey, { permissions: changedPermissions })
      .pipe(
        switchMap(() =>
          this.shouldFetchAppConfig() ? this.configState.refreshAppState() : of(null),
        ),
        finalize(() => (this.modalBusy = false)),
      )
      .subscribe(() => {
        this.visible = false;
        this.toasterService.success('AbpUi::SavedSuccessfully');
      });
  }

  openModal() {
    if (!this.providerKey || !this.providerName) {
      throw new Error('Provider Key and Provider Name are required.');
    }

    return this.service.get(this.providerName, this.providerKey).pipe(
      tap((permissionRes: GetPermissionListResultDto) => {
        this.data = permissionRes;
        this.permissions = getPermissions(permissionRes.groups);
        this.setSelectedGroup(permissionRes.groups[0]);
        this.disabledSelectAllInAllTabs = this.permissions.every(
          per =>
            per.isGranted &&
            per.grantedProviders.every(provider => provider.providerName !== this.providerName),
        );
      }),
    );
  }

  initModal() {
    // TODO: Refactor
    setTimeout(() => {
      this.setDisabled(this.selectedGroup?.permissions || []);
      this.setTabCheckboxState();
      this.setGrantCheckboxState();
    });
  }

  getAssignedCount(groupName: string) {
    return this.permissions.reduce(
      (acc, val) => (val.groupName === groupName && val.isGranted ? acc + 1 : acc),
      0,
    );
  }

  shouldFetchAppConfig() {
    const currentUser = this.configState.getOne('currentUser') as CurrentUserDto;

    if (this.providerName === 'R') return currentUser.roles.some(role => role === this.providerKey);

    if (this.providerName === 'U') return currentUser.id === this.providerKey;

    return false;
  }
}

function findParentPermissions(
  permissions: PermissionGrantInfoDto[],
  permission: PermissionGrantInfoDto,
): PermissionGrantInfoDto[] {
  const permissionMap = new Map(permissions.map(p => [p.name, p]));
  let currentPermission = permissionMap.get(permission.name) ?? null;
  const parentPermissions: PermissionGrantInfoDto[] = [];

  while (currentPermission && currentPermission.parentName) {
    const parentPermission = permissionMap.get(currentPermission.parentName);
    if (!parentPermission) {
      break;
    }
    parentPermissions.push(parentPermission);
    currentPermission = parentPermission;
  }

  return parentPermissions;
}

function findMargin(
  permissions: PermissionGrantInfoDto[],
  permission: PermissionGrantInfoDto,
): number {
  const parentPermission = permissions.find(per => per.name === permission.parentName);

  if (parentPermission && parentPermission.parentName) {
    let margin = 20;
    return (margin += findMargin(permissions, parentPermission));
  }

  return parentPermission ? 20 : 0;
}

function getPermissions(groups: PermissionGroupDto[]): PermissionWithGroupName[] {
  return groups.reduce(
    (acc, val) => [
      ...acc,
      ...val.permissions.map<PermissionWithGroupName>(p => ({ ...p, groupName: val.name || '' })),
    ],
    [] as PermissionWithGroupName[],
  );
}
