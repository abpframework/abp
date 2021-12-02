import { ConfigStateService, CurrentUserDto } from '@abp/ng.core';
import {
  GetPermissionListResultDto,
  PermissionGrantInfoDto,
  PermissionGroupDto,
  PermissionsService,
  ProviderInfoDto,
  UpdatePermissionDto,
} from '@abp/ng.permission-management/proxy';
import { LocaleDirection } from '@abp/ng.theme.shared';
import {
  Component,
  ElementRef,
  EventEmitter,
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
    `,
  ],
})
export class PermissionManagementComponent
  implements
    PermissionManagement.PermissionManagementComponentInputs,
    PermissionManagement.PermissionManagementComponentOutputs
{
  @Input()
  readonly providerName: string;

  @Input()
  readonly providerKey: string;

  @Input()
  readonly hideBadges = false;

  protected _visible = false;

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
      this.selectedGroup = null;
      this._visible = false;
      this.visibleChange.emit(false);
    }
  }

  @Output() readonly visibleChange = new EventEmitter<boolean>();

  @ViewChildren('selectAllInThisTabsRef')
  selectAllInThisTabsRef: QueryList<ElementRef<HTMLInputElement>>;
  @ViewChildren('selectAllInAllTabsRef')
  selectAllInAllTabsRef: QueryList<ElementRef<HTMLInputElement>>;

  data: GetPermissionListResultDto = { groups: [], entityDisplayName: null };

  selectedGroup: PermissionGroupDto;

  permissions: PermissionWithGroupName[] = [];

  selectThisTab = false;

  selectAllTab = false;

  modalBusy = false;

  trackByFn: TrackByFunction<PermissionGroupDto> = (_, item) => item.name;

  get selectedGroupPermissions(): PermissionWithStyle[] {
    if (!this.selectedGroup) return [];

    const margin = `margin-${
      (document.body.dir as LocaleDirection) === 'rtl' ? 'right' : 'left'
    }.px`;

    const permissions = this.data.groups.find(
      group => group.name === this.selectedGroup.name,
    ).permissions;

    return permissions.map(
      permission =>
        ({
          ...permission,
          style: { [margin]: findMargin(permissions, permission) },
          isGranted: this.permissions.find(per => per.name === permission.name).isGranted,
        } as unknown as PermissionWithStyle),
    );
  }

  constructor(protected service: PermissionsService, protected configState: ConfigStateService) {}

  getChecked(name: string) {
    return (this.permissions.find(per => per.name === name) || { isGranted: false }).isGranted;
  }

  isGrantedByOtherProviderName(grantedProviders: ProviderInfoDto[]): boolean {
    if (grantedProviders.length) {
      return grantedProviders.findIndex(p => p.providerName !== this.providerName) > -1;
    }
    return false;
  }

  onClickCheckbox(clickedPermission: PermissionGrantInfoDto, value) {
    if (
      clickedPermission.isGranted &&
      this.isGrantedByOtherProviderName(clickedPermission.grantedProviders)
    )
      return;

    setTimeout(() => {
      this.permissions = this.permissions.map(per => {
        if (clickedPermission.name === per.name) {
          return { ...per, isGranted: !per.isGranted };
        } else if (clickedPermission.name === per.parentName && clickedPermission.isGranted) {
          return { ...per, isGranted: false };
        } else if (clickedPermission.parentName === per.name && !clickedPermission.isGranted) {
          return { ...per, isGranted: true };
        }

        return per;
      });

      this.setTabCheckboxState();
      this.setGrantCheckboxState();
    }, 0);
  }

  setTabCheckboxState() {
    const selectedPermissions = this.selectedGroupPermissions.filter(per => per.isGranted);
    const element = document.querySelector('#select-all-in-this-tabs') as any;

    if (selectedPermissions.length === this.selectedGroupPermissions.length) {
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
    const selectedAllPermissions = this.permissions.filter(per => per.isGranted);
    const checkboxElement = document.querySelector('#select-all-in-all-tabs') as any;

    if (selectedAllPermissions.length === this.permissions.length) {
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

    this.selectThisTab = !this.selectAllTab;
  }

  onChangeGroup(group: PermissionGroupDto) {
    this.selectedGroup = group;
    this.setTabCheckboxState();
  }

  submit() {
    const unchangedPermissions = getPermissions(this.data.groups);

    const changedPermissions: UpdatePermissionDto[] = this.permissions
      .filter(per =>
        unchangedPermissions.find(unchanged => unchanged.name === per.name).isGranted ===
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
      });
  }

  openModal() {
    if (!this.providerKey || !this.providerName) {
      throw new Error('Provider Key and Provider Name are required.');
    }

    return this.service.get(this.providerName, this.providerKey).pipe(
      tap((permissionRes: GetPermissionListResultDto) => {
        this.data = permissionRes;
        this.selectedGroup = permissionRes.groups[0];
        this.permissions = getPermissions(permissionRes.groups);
      }),
    );
  }

  initModal() {
    // TODO: Refactor
    setTimeout(() => {
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

function findMargin(permissions: PermissionGrantInfoDto[], permission: PermissionGrantInfoDto) {
  const parentPermission = permissions.find(per => per.name === permission.parentName);

  if (parentPermission && parentPermission.parentName) {
    let margin = 20;
    return (margin += findMargin(permissions, parentPermission));
  }

  return parentPermission ? 20 : 0;
}

function getPermissions(groups: PermissionGroupDto[]): PermissionWithGroupName[] {
  return groups.reduce(
    (acc, val) => [...acc, ...val.permissions.map(p => ({ ...p, groupName: val.name }))],
    [],
  );
}
