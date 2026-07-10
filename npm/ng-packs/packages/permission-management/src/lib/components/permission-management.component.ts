import {
  ChangeDetectionStrategy,
  Component,
  computed,
  DOCUMENT,
  effect,
  inject,
  input,
  output,
  signal,
  TrackByFunction,
  untracked,
} from '@angular/core';
import { NgStyle } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Tabs, TabList, Tab, TabPanel, TabContent } from '@angular/aria/tabs';
import { of } from 'rxjs';
import { finalize, switchMap, tap } from 'rxjs/operators';

import { ConfigStateService, CurrentUserDto, LocalizationPipe } from '@abp/ng.core';
import {
  ButtonComponent,
  LocaleDirection,
  ModalCloseDirective,
  ModalComponent,
  ToasterService,
} from '@abp/ng.theme.shared';
import {
  GetPermissionListResultDto,
  PermissionGrantInfoDto,
  PermissionGroupDto,
  PermissionsService,
  ProviderInfoDto,
  UpdatePermissionDto,
} from '@abp/ng.permission-management/proxy';

type PermissionWithStyle = PermissionGrantInfoDto & {
  style: Record<string, number>;
};

type PermissionWithGroupName = PermissionGrantInfoDto & {
  groupName: string;
};

type SelectAllCheckboxState = {
  checked: boolean;
  indeterminate: boolean;
};

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-permission-management',
  templateUrl: './permission-management.component.html',
  exportAs: 'abpPermissionManagement',
  styles: [
    `
      .scroll-in-modal {
        overflow: auto;
        /*
        To maintain a 28px top margin and 28px bottom margin when the modal reaches full height, the scrollable area needs to be 100vh - 23.1rem
         */
        max-height: calc(100vh - 23.1rem);
      }

      .lpx-scroll-pills-container .nav-pills {
        display: block;
        overflow-y: auto;
      }

      /* Target mobile screens */
      @media (max-width: 768px) {
        .scroll-in-modal {
          max-height: calc(100vh - 15rem);
        }
        .lpx-scroll-pills-container .nav-pills {
          max-height: 500px;
        }
      }

      fieldset legend {
        float: none;
        width: auto;
      }

      .lpx-scroll-pills-container .tab-content {
        padding-top: 0 !important;
        padding-bottom: 0 !important;
      }

      .lpx-scroll-pills-container .nav-item {
        margin-bottom: 10px;
        border-radius: 10px;
      }

      .lpx-scroll-pills-container .nav-item .nav-link.active {
        color: #fff !important;
        border-color: #6c5dd3 !important;
        background-color: #6c5dd3 !important;
      }
    `,
  ],
  imports: [
    FormsModule,
    NgStyle,
    ModalComponent,
    LocalizationPipe,
    ButtonComponent,
    ModalCloseDirective,
    Tabs,
    TabList,
    Tab,
    TabPanel,
    TabContent,
  ],
})
export class PermissionManagementComponent {
  protected readonly service = inject(PermissionsService);
  protected readonly configState = inject(ConfigStateService);
  protected readonly toasterService = inject(ToasterService);
  private document = inject(DOCUMENT);

  readonly providerNameInput = input('', { alias: 'providerName' });
  readonly providerKeyInput = input('', { alias: 'providerKey' });
  readonly hideBadgesInput = input(false, { alias: 'hideBadges' });
  readonly entityDisplayName = input<string | undefined>(undefined);
  readonly visibleInput = input(false, { alias: 'visible' });

  readonly visibleChange = output<boolean>();

  protected readonly modalVisible = signal(false);
  protected readonly modalBusy = signal(false);
  protected readonly filter = signal<string>('');

  private isOpening = false;
  private isClosingModal = false;

  // Backward-compatible getters/setters for ReplaceableTemplateDirective.
  private _providerNameOverride?: string;
  get providerName(): string {
    return this._providerNameOverride ?? this.providerNameInput();
  }
  set providerName(value: string) {
    this._providerNameOverride = value;
  }

  private _providerKeyOverride?: string;
  get providerKey(): string {
    return this._providerKeyOverride ?? this.providerKeyInput();
  }
  set providerKey(value: string) {
    this._providerKeyOverride = value;
  }

  private _hideBadgesOverride?: boolean;
  get hideBadges(): boolean {
    return this._hideBadgesOverride ?? this.hideBadgesInput();
  }
  set hideBadges(value: boolean) {
    this._hideBadgesOverride = value;
  }

  data: GetPermissionListResultDto = { groups: [], entityDisplayName: '' };

  private readonly permissionsState = signal<PermissionWithGroupName[]>([]);
  private readonly selectedGroupState = signal<PermissionGroupDto | null>(null);
  private readonly permissionGroupSignal = signal<PermissionGroupDto[]>([]);

  protected readonly selectedGroupPermissions = computed(() => {
    const group = this.selectedGroupState();
    if (!group) {
      return [] as PermissionWithStyle[];
    }

    const margin = `margin-${
      (this.document.body?.dir as LocaleDirection) === 'rtl' ? 'right' : 'left'
    }.px`;

    const groupPermissions =
      this.permissionGroupSignal().find(item => item.name === group.name)?.permissions || [];
    const permissions = this.permissionsState();

    return groupPermissions.map(permission => ({
      ...permission,
      style: { [margin]: findMargin(groupPermissions, permission) },
      isGranted: (permissions.find(per => per.name === permission.name) || {}).isGranted,
    })) as PermissionWithStyle[];
  });

  protected readonly selectThisTabState = computed(() =>
    getSelectAllCheckboxState(this.selectedGroupPermissions(), this.providerName),
  );

  protected readonly selectAllTabState = computed(() =>
    getSelectAllCheckboxState(this.permissionsState(), this.providerName),
  );

  // Disabled state is based on the API snapshot (not live edits) so select-all stays toggleable.
  protected readonly disableSelectAllTab = computed(() =>
    isSelectAllDisabled(this.selectedGroupState()?.permissions, this.providerName),
  );

  private readonly disabledSelectAllInAllTabsState = signal(false);
  protected readonly disabledSelectAllInAllTabs = this.disabledSelectAllInAllTabsState.asReadonly();

  permissionGroups = computed(() => {
    const search = this.filter().toLowerCase().trim();
    const groups = this.permissionGroupSignal();

    if (!search) {
      return groups;
    }

    const includesSearch = (text: string) => text.toLowerCase().includes(search);
    return groups.filter(group =>
      group.permissions.some(
        permission => includesSearch(permission.displayName) || includesSearch(group.displayName),
      ),
    );
  });

  trackByFn: TrackByFunction<PermissionGroupDto> = (_, item) => item.name;

  // Backward-compatible getters/setters for ReplaceableTemplateDirective.
  get visible(): boolean {
    return this.modalVisible();
  }

  set visible(value: boolean) {
    this.setVisible(value);
  }

  private setVisible(value: boolean) {
    if (value && this.isClosingModal) {
      return;
    }

    if (value === this.modalVisible()) {
      return;
    }

    if (value) {
      this.showModal();
    } else {
      this.hideModal();
    }
  }

  get permissions(): PermissionWithGroupName[] {
    return this.permissionsState();
  }

  set permissions(value: PermissionWithGroupName[]) {
    this.permissionsState.set(value);
  }

  get selectedGroup(): PermissionGroupDto | null | undefined {
    return this.selectedGroupState();
  }

  get selectThisTab(): boolean {
    return this.selectThisTabState().checked;
  }

  get selectAllTab(): boolean {
    return this.selectAllTabState().checked;
  }

  onModalVisibleChange(value: boolean) {
    this.visibleChange.emit(value);
  }

  onModalDisappear() {
    setTimeout(() => {
      if (!this.modalVisible()) {
        this.resetModalState();
      }
      this.isClosingModal = false;
    });
  }

  private showModal() {
    if (this.isOpening || this.modalVisible()) {
      return;
    }

    this.isClosingModal = false;
    this.isOpening = true;
    this.openModal()
      .pipe(finalize(() => (this.isOpening = false)))
      .subscribe(() => {
        this.modalVisible.set(true);
        this.visibleChange.emit(true);
      });
  }

  private hideModal() {
    if (!this.modalVisible()) {
      return;
    }

    this.isClosingModal = true;
    this.visibleChange.emit(false);
    this.modalVisible.set(false);
  }

  private resetModalState() {
    this.setSelectedGroup(null);
    this.filter.set('');
    this.disabledSelectAllInAllTabsState.set(false);
  }

  // constructor() {
  //   effect(() => {
  //     this.setVisible(this.visibleInput());
  //   });
  // }

  constructor() {
    effect(() => {
      const visible = this.visibleInput();
      untracked(() => this.setVisible(visible));
    });
  }

  getChecked(name: string) {
    return (this.permissionsState().find(per => per.name === name) || { isGranted: false })
      .isGranted;
  }

  setSelectedGroup(group: PermissionGroupDto | null) {
    this.selectedGroupState.set(group);
  }

  isGrantedByOtherProviderName(grantedProviders?: ProviderInfoDto[]): boolean {
    if (grantedProviders?.length) {
      return grantedProviders.findIndex(p => p.providerName !== this.providerName) > -1;
    }
    return false;
  }

  onClickCheckbox(clickedPermission: PermissionGrantInfoDto) {
    const { isGranted, grantedProviders } = clickedPermission;
    if (isGranted && this.isGrantedByOtherProviderName(grantedProviders)) {
      return;
    }

    setTimeout(() => {
      this.updatePermissionStatus(clickedPermission);
      this.setParentClicked(clickedPermission);
    }, 0);
  }

  updatePermissionStatus(clickedPermission: PermissionGrantInfoDto) {
    this.permissionsState.update(permissions =>
      permissions.map(permission => {
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
      }),
    );
  }

  setParentClicked(clickedPermission: PermissionGrantInfoDto) {
    if (clickedPermission.parentName) {
      const parentPermissions = findParentPermissions(this.permissionsState(), clickedPermission);
      if (parentPermissions.length > 0) {
        const parentNames = new Set(parentPermissions.map(parent => parent.name));

        this.permissionsState.update(permissions =>
          permissions.map(per => {
            let updatedIsGranted = per.isGranted;

            if (per.parentName === clickedPermission.name && !clickedPermission.isGranted) {
              updatedIsGranted = false;
            }

            if (parentNames.has(per.name)) {
              updatedIsGranted = true;
            }

            return { ...per, isGranted: updatedIsGranted };
          }),
        );
      }
      return;
    }

    this.permissionsState.update(permissions =>
      permissions.map(per => {
        const parents = findParentPermissions(permissions, per);
        if (parents.length > 0) {
          const rootParent = parents[parents.length - 1];

          if (rootParent.name === clickedPermission.name && !rootParent.isGranted) {
            return { ...per, isGranted: false };
          }
        }
        return per;
      }),
    );
  }

  onSelectThisTabChange(event: Event) {
    if (this.disableSelectAllTab()) {
      return;
    }

    const state = this.selectThisTabState();
    const checked = !state.checked || state.indeterminate;

    this.permissionsState.update(permissions => {
      let updatedPermissions = [...permissions];

      this.selectedGroupPermissions().forEach(permission => {
        if (this.isGrantedByOtherProviderName(permission.grantedProviders)) {
          return;
        }

        const index = updatedPermissions.findIndex(per => per.name === permission.name);
        if (index < 0) {
          return;
        }

        updatedPermissions = [
          ...updatedPermissions.slice(0, index),
          { ...updatedPermissions[index], isGranted: checked },
          ...updatedPermissions.slice(index + 1),
        ];
      });

      return updatedPermissions;
    });
  }

  onSelectAllChange(event: Event) {
    if (this.disabledSelectAllInAllTabs()) {
      return;
    }

    const state = this.selectAllTabState();
    const checked = !state.checked || state.indeterminate;

    if (this.filter()) {
      this.onFilterChange('');
    }

    this.permissionsState.update(permissions =>
      permissions.map(permission => ({
        ...permission,
        isGranted: this.isGrantedByOtherProviderName(permission.grantedProviders) || checked,
      })),
    );
  }

  onFilterChange(value: string) {
    this.filter.set(value);
    this.syncSelectedGroupWithFilter();
  }

  private syncSelectedGroupWithFilter() {
    const groups = this.permissionGroups();

    if (!groups.length) {
      this.setSelectedGroup(null);
      return;
    }

    if (!groups.some(group => group.name === this.selectedGroupState()?.name)) {
      this.onChangeGroup(groups[0]);
    }
  }

  onTabChange(groupName: string) {
    const group = this.permissionGroups().find(g => g.name === groupName);
    if (group) {
      this.onChangeGroup(group);
    }
  }

  onChangeGroup(group: PermissionGroupDto) {
    this.setSelectedGroup(group);
  }

  submit() {
    if (this.modalBusy()) {
      return;
    }

    const unchangedPermissions = getPermissions(this.data.groups);

    const changedPermissions: UpdatePermissionDto[] = this.permissionsState()
      .filter(per =>
        (unchangedPermissions.find(unchanged => unchanged.name === per.name) || {}).isGranted ===
        per.isGranted
          ? false
          : true,
      )
      .map(({ name, isGranted }) => ({ name, isGranted }));

    if (!changedPermissions.length) {
      this.hideModal();
      return;
    }

    this.modalBusy.set(true);
    this.service
      .update(this.providerName, this.providerKey, { permissions: changedPermissions })
      .pipe(
        tap(() => this.hideModal()),
        switchMap(() =>
          this.shouldFetchAppConfig() ? this.configState.refreshAppState() : of(null),
        ),
        finalize(() => this.modalBusy.set(false)),
      )
      .subscribe(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully');
      });
  }

  openModal() {
    const providerName = this.providerName;
    const providerKey = this.providerKey;

    if (!providerKey || !providerName) {
      throw new Error('Provider Key and Provider Name are required.');
    }

    return this.service.get(providerName, providerKey).pipe(
      tap((permissionRes: GetPermissionListResultDto) => {
        const { groups } = permissionRes || {};

        this.data = permissionRes;
        this.permissionGroupSignal.set(groups);
        const permissions = getPermissions(groups);
        this.permissionsState.set(permissions);
        this.disabledSelectAllInAllTabsState.set(isSelectAllDisabled(permissions, providerName));
        this.setSelectedGroup(groups[0] ?? null);
      }),
    );
  }

  getAssignedCount(groupName: string) {
    return this.permissionsState().reduce(
      (acc, val) => (val.groupName === groupName && val.isGranted ? acc + 1 : acc),
      0,
    );
  }

  shouldFetchAppConfig() {
    const currentUser = this.configState.getOne('currentUser') as CurrentUserDto;
    const providerName = this.providerName;
    const providerKey = this.providerKey;

    if (providerName === 'R') return currentUser.roles.some(role => role === providerKey);

    if (providerName === 'U') return currentUser.id === providerKey;

    return false;
  }
}

function isSelectAllDisabled(
  permissions: PermissionGrantInfoDto[] | undefined,
  providerName: string,
): boolean {
  if (!permissions?.length) {
    return false;
  }

  return permissions.every(
    permission =>
      permission.isGranted &&
      permission.grantedProviders?.every(p => p.providerName !== providerName),
  );
}

function getSelectAllCheckboxState(
  permissions: PermissionGrantInfoDto[],
  providerName: string,
): SelectAllCheckboxState {
  const selectablePermissions = permissions.filter(permission =>
    (permission.grantedProviders ?? []).every(p => p.providerName === providerName),
  );
  const selectedPermissions = selectablePermissions.filter(permission => permission.isGranted);

  if (!selectablePermissions.length || selectedPermissions.length === 0) {
    return { checked: false, indeterminate: false };
  }

  if (selectedPermissions.length === selectablePermissions.length) {
    return { checked: true, indeterminate: false };
  }

  return { checked: false, indeterminate: true };
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
