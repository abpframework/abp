import {
  AbpApplicationConfigurationService,
  ConfigStateService,
  CurrentUserDto,
} from '@abp/ng.core';
import { LocaleDirection } from '@abp/ng.theme.shared';
import { Component, EventEmitter, Input, Output, TrackByFunction } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable, of } from 'rxjs';
import { finalize, map, pluck, switchMap, take, tap } from 'rxjs/operators';
import { GetPermissions, UpdatePermissions } from '../actions/permission-management.actions';
import { PermissionManagement } from '../models/permission-management';
import {
  GetPermissionListResultDto,
  PermissionGrantInfoDto,
  PermissionGroupDto,
  ProviderInfoDto,
  UpdatePermissionDto,
} from '../proxy/models';
import { PermissionManagementState } from '../states/permission-management.state';

type PermissionWithStyle = PermissionGrantInfoDto & {
  style: string;
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
    PermissionManagement.PermissionManagementComponentOutputs {
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
      });
    } else {
      this.selectedGroup = null;
      this._visible = false;
      this.visibleChange.emit(false);
    }
  }

  @Output() readonly visibleChange = new EventEmitter<boolean>();

  @Select(PermissionManagementState.getPermissionGroups)
  groups$: Observable<PermissionGroupDto[]>;

  @Select(PermissionManagementState.getEntityDisplayName)
  entityName$: Observable<string>;

  selectedGroup: PermissionGroupDto;

  permissions: PermissionGrantInfoDto[] = [];

  selectThisTab = false;

  selectAllTab = false;

  modalBusy = false;

  trackByFn: TrackByFunction<PermissionGroupDto> = (_, item) => item.name;

  get selectedGroupPermissions$(): Observable<PermissionWithStyle[]> {
    const margin = `margin-${
      (document.body.dir as LocaleDirection) === 'rtl' ? 'right' : 'left'
    }.px`;

    return this.groups$.pipe(
      map(groups =>
        this.selectedGroup
          ? groups.find(group => group.name === this.selectedGroup.name).permissions
          : [],
      ),
      map<PermissionGrantInfoDto[], PermissionWithStyle[]>(permissions =>
        permissions.map(
          permission =>
            (({
              ...permission,
              style: { [margin]: findMargin(permissions, permission) },
              isGranted: this.permissions.find(per => per.name === permission.name).isGranted,
            } as any) as PermissionWithStyle),
        ),
      ),
    );
  }

  constructor(
    protected store: Store,
    protected configState: ConfigStateService,
    protected appConfigService: AbpApplicationConfigurationService,
  ) {}

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
    this.selectedGroupPermissions$.pipe(take(1)).subscribe(permissions => {
      const selectedPermissions = permissions.filter(per => per.isGranted);
      const element = document.querySelector('#select-all-in-this-tabs') as any;

      if (selectedPermissions.length === permissions.length) {
        element.indeterminate = false;
        this.selectThisTab = true;
      } else if (selectedPermissions.length === 0) {
        element.indeterminate = false;
        this.selectThisTab = false;
      } else {
        element.indeterminate = true;
      }
    });
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
    this.selectedGroupPermissions$.pipe(take(1)).subscribe(permissions => {
      permissions.forEach(permission => {
        if (permission.isGranted && this.isGrantedByOtherProviderName(permission.grantedProviders))
          return;

        const index = this.permissions.findIndex(per => per.name === permission.name);

        this.permissions = [
          ...this.permissions.slice(0, index),
          { ...this.permissions[index], isGranted: !this.selectThisTab },
          ...this.permissions.slice(index + 1),
        ];
      });
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
    const unchangedPermissions = getPermissions(
      this.store.selectSnapshot(PermissionManagementState.getPermissionGroups),
    );

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
    this.store
      .dispatch(
        new UpdatePermissions({
          providerKey: this.providerKey,
          providerName: this.providerName,
          permissions: changedPermissions,
        }),
      )
      .pipe(
        switchMap(() =>
          this.shouldFetchAppConfig()
            ? this.appConfigService.get().pipe(tap(res => this.configState.setState(res)))
            : of(null),
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

    return this.store
      .dispatch(
        new GetPermissions({
          providerKey: this.providerKey,
          providerName: this.providerName,
        }),
      )
      .pipe(
        pluck('PermissionManagementState', 'permissionRes'),
        tap((permissionRes: GetPermissionListResultDto) => {
          this.selectedGroup = permissionRes.groups[0];
          this.permissions = getPermissions(permissionRes.groups);
        }),
      );
  }

  initModal() {
    this.setTabCheckboxState();
    this.setGrantCheckboxState();
  }

  getAssignedCount(groupName: string) {
    return this.permissions.reduce(
      (acc, val) => (val.name.split('.')[0] === groupName && val.isGranted ? acc + 1 : acc),
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

function getPermissions(groups: PermissionGroupDto[]): PermissionGrantInfoDto[] {
  return groups.reduce((acc, val) => [...acc, ...val.permissions], []);
}
