import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  Renderer2,
  SimpleChanges,
  TrackByFunction,
} from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { from, Observable } from 'rxjs';
import { map, pluck, take, finalize } from 'rxjs/operators';
import { GetPermissions, UpdatePermissions } from '../actions/permission-management.actions';
import { PermissionManagement } from '../models/permission-management';
import { PermissionManagementState } from '../states/permission-management.state';

type PermissionWithMargin = PermissionManagement.Permission & {
  margin: number;
};

@Component({
  selector: 'abp-permission-management',
  templateUrl: './permission-management.component.html',
})
export class PermissionManagementComponent implements OnInit, OnChanges {
  @Input()
  providerName: string;

  @Input()
  providerKey: string;

  protected _visible;

  @Input()
  get visible(): boolean {
    return this._visible;
  }

  set visible(value: boolean) {
    if (!this.selectedGroup) return;

    this._visible = value;
    this.visibleChange.emit(value);

    if (!value) {
      this.selectedGroup = null;
    }
  }

  @Output() readonly visibleChange = new EventEmitter<boolean>();

  @Select(PermissionManagementState.getPermissionGroups)
  groups$: Observable<PermissionManagement.Group[]>;

  @Select(PermissionManagementState.getEntityDisplayName)
  entityName$: Observable<string>;

  selectedGroup: PermissionManagement.Group;

  permissions: PermissionManagement.Permission[] = [];

  selectThisTab = false;

  selectAllTab = false;

  modalBusy = false;

  trackByFn: TrackByFunction<PermissionManagement.Group> = (_, item) => item.name;

  get selectedGroupPermissions$(): Observable<PermissionWithMargin[]> {
    return this.groups$.pipe(
      map(groups =>
        this.selectedGroup ? groups.find(group => group.name === this.selectedGroup.name).permissions : [],
      ),
      map<PermissionManagement.Permission[], PermissionWithMargin[]>(permissions =>
        permissions.map(
          permission =>
            (({
              ...permission,
              margin: findMargin(permissions, permission),
              isGranted: this.permissions.find(per => per.name === permission.name).isGranted,
            } as any) as PermissionWithMargin),
        ),
      ),
    );
  }

  constructor(private store: Store, private renderer: Renderer2) {}

  ngOnInit(): void {}

  getChecked(name: string) {
    return (this.permissions.find(per => per.name === name) || { isGranted: false }).isGranted;
  }

  isGrantedByOtherProviderName(grantedProviders: PermissionManagement.GrantedProvider[]): boolean {
    if (grantedProviders.length) {
      return grantedProviders.findIndex(p => p.providerName !== this.providerName) > -1;
    }
    return false;
  }

  onClickCheckbox(clickedPermission: PermissionManagement.Permission, value) {
    if (clickedPermission.isGranted && this.isGrantedByOtherProviderName(clickedPermission.grantedProviders)) return;

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
        if (permission.isGranted && this.isGrantedByOtherProviderName(permission.grantedProviders)) return;

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
      isGranted: this.isGrantedByOtherProviderName(permission.grantedProviders) || !this.selectAllTab,
    }));

    this.selectThisTab = !this.selectAllTab;
  }

  onChangeGroup(group: PermissionManagement.Group) {
    this.selectedGroup = group;
    this.setTabCheckboxState();
  }

  submit() {
    this.modalBusy = true;
    const unchangedPermissions = getPermissions(
      this.store.selectSnapshot(PermissionManagementState.getPermissionGroups),
    );

    const changedPermissions: PermissionManagement.MinimumPermission[] = this.permissions
      .filter(per =>
        unchangedPermissions.find(unchanged => unchanged.name === per.name).isGranted === per.isGranted ? false : true,
      )
      .map(({ name, isGranted }) => ({ name, isGranted }));

    if (changedPermissions.length) {
      this.store
        .dispatch(
          new UpdatePermissions({
            providerKey: this.providerKey,
            providerName: this.providerName,
            permissions: changedPermissions,
          }),
        )
        .pipe(finalize(() => (this.modalBusy = false)))
        .subscribe(() => {
          this.visible = false;
        });
    } else {
      this.modalBusy = false;
      this.visible = false;
    }
  }

  openModal() {
    if (!this.providerKey || !this.providerName) {
      throw new Error('Provider Key and Provider Name are required.');
    }

    this.store
      .dispatch(
        new GetPermissions({
          providerKey: this.providerKey,
          providerName: this.providerName,
        }),
      )
      .pipe(pluck('PermissionManagementState', 'permissionRes'))
      .subscribe((permissionRes: PermissionManagement.Response) => {
        this.selectedGroup = permissionRes.groups[0];
        this.permissions = getPermissions(permissionRes.groups);

        this.visible = true;
      });
  }

  initModal() {
    this.setTabCheckboxState();
    this.setGrantCheckboxState();
  }

  ngOnChanges({ visible }: SimpleChanges): void {
    if (!visible) return;

    if (visible.currentValue) {
      this.openModal();
    } else if (visible.currentValue === false && this.visible) {
      this.visible = false;
    }
  }
}

function findMargin(permissions: PermissionManagement.Permission[], permission: PermissionManagement.Permission) {
  const parentPermission = permissions.find(per => per.name === permission.parentName);

  if (parentPermission && parentPermission.parentName) {
    let margin = 20;
    return (margin += findMargin(permissions, parentPermission));
  }

  return parentPermission ? 20 : 0;
}

function getPermissions(groups: PermissionManagement.Group[]): PermissionManagement.Permission[] {
  return groups.reduce((acc, val) => [...acc, ...val.permissions], []);
}
