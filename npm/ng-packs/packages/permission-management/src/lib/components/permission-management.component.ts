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
import {
  afterNextRender,
  Component,
  computed,
  DOCUMENT,
  effect,
  ElementRef,
  inject,
  Injector,
  input,
  output,
  signal,
  TrackByFunction,
  untracked,
  viewChildren,
} from '@angular/core';
import { of } from 'rxjs';
import { finalize, switchMap, tap } from 'rxjs/operators';
import { PermissionManagement } from '../models';

import { FormsModule } from '@angular/forms';

import { Tabs, TabList, Tab, TabPanel, TabContent } from '@angular/aria/tabs';

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
  private readonly injector = inject(Injector);
  private document = inject(DOCUMENT);


  readonly providerNameInput = input('', { alias: 'providerName' });
  readonly providerKeyInput = input('', { alias: 'providerKey' });
  readonly hideBadgesInput = input(false, { alias: 'hideBadges' });
  readonly entityDisplayName = input<string | undefined>(undefined);
  readonly visibleInput = input(false, { alias: 'visible' });

  // Output signals
  readonly visibleChange = output<boolean>();

  // Internal state
  protected readonly _visible = signal(false);

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

  selectAllInThisTabsRef = viewChildren<ElementRef<HTMLInputElement>>('selectAllInThisTabsRef');
  selectAllInAllTabsRef = viewChildren<ElementRef<HTMLInputElement>>('selectAllInAllTabsRef');

  data: GetPermissionListResultDto = { groups: [], entityDisplayName: '' };

  selectedGroup?: PermissionGroupDto | null;

  permissions: PermissionWithGroupName[] = [];

  selectThisTab = false;

  selectAllTab = false;

  disableSelectAllTab = false;

  disabledSelectAllInAllTabs = false;

  modalBusy = false;

  filter = signal<string>('');

  selectedGroupPermissions: PermissionWithStyle[] = [];

  permissionGroupSignal = signal<PermissionGroupDto[]>([]);

  permissionGroups = computed(() => {
    const search = this.filter().toLowerCase().trim();
    let groups = this.permissionGroupSignal();

    if (!search) {
      this.setSelectedGroup(groups[0]);
      return groups;
    }

    const includesSearch = text => text.toLowerCase().includes(search);
    groups = groups.filter(group =>
      group.permissions.some(
        permission => includesSearch(permission.displayName) || includesSearch(group.displayName),
      ),
    );

    if (groups.length) {
      this.setSelectedGroup(groups[0]);
    } else {
      this.selectedGroupPermissions = [];
    }

    return groups;
  });

  trackByFn: TrackByFunction<PermissionGroupDto> = (_, item) => item.name;

  // Getter/setter for visible - used by ReplaceableTemplateDirective and internal code
  get visible(): boolean {
    return this._visible();
  }

  set visible(value: boolean) {
    if (value === this._visible()) {
      return;
    }

    if (value) {
      this.openModal().subscribe(() => {
        this._visible.set(true);
        this.visibleChange.emit(true);
        afterNextRender(() => {
          this.initModal();
        }, { injector: this.injector });
      });
    } else {
      this.setSelectedGroup(null);
      this._visible.set(false);
      this.visibleChange.emit(false);
      this.filter.set('');
    }
  }

  constructor() {
    effect(() => {
      const inputValue = this.visibleInput();
      untracked(() => {
        if (this._visible() !== inputValue) {
          if (inputValue) {
            this.openModal().subscribe(() => {
              this._visible.set(true);
              afterNextRender(() => {
                this.initModal();
              }, { injector: this.injector });
            });
          } else {
            this.setSelectedGroup(null);
            this._visible.set(false);
            this.filter.set('');
          }
        }
      });
    });
  }

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
      (this.document.body?.dir as LocaleDirection) === 'rtl' ? 'right' : 'left'
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
    const providerName = this.providerName;
    const selectablePermissions = this.selectedGroupPermissions.filter(per =>
      per.grantedProviders.every(p => p.providerName === providerName),
    );

    const selectedPermissions = selectablePermissions.filter(per => per.isGranted);
    const element = this.document.querySelector('#select-all-in-this-tabs') as any;
    if (!element) {
      return;
    }

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
    const providerName = this.providerName;
    const selectablePermissions = this.permissions.filter(per =>
      per.grantedProviders.every(p => p.providerName === providerName),
    );
    const selectedAllPermissions = selectablePermissions.filter(per => per.isGranted);
    const checkboxElement = this.document.querySelector('#select-all-in-all-tabs') as any;

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
    if (this.filter()) {
      this.filter.set('');
    }

    this.permissions = this.permissions.map(permission => ({
      ...permission,
      isGranted:
        this.isGrantedByOtherProviderName(permission.grantedProviders) || !this.selectAllTab,
    }));

    if (!this.disableSelectAllTab) {
      this.selectThisTab = !this.selectAllTab;
      this.setTabCheckboxState();
      if (this.filter()) {
        this.setGrantCheckboxState();
      }
    }
    this.onChangeGroup(this.selectedGroup);
  }

  onTabChange(groupName: string) {
    const group = this.permissionGroups().find(g => g.name === groupName);
    if (group) {
      this.onChangeGroup(group);
    }
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
        this.permissions = getPermissions(groups);
        this.setSelectedGroup(groups[0]);

        this.disabledSelectAllInAllTabs = this.permissions.every(
          per =>
            per.isGranted &&
            per.grantedProviders.every(provider => provider.providerName !== providerName),
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
    const providerName = this.providerName;
    const providerKey = this.providerKey;

    if (providerName === 'R') return currentUser.roles.some(role => role === providerKey);

    if (providerName === 'U') return currentUser.id === providerKey;

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
