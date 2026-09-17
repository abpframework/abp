import { Injectable, signal, computed } from '@angular/core';
import {
    ResourcePermissionGrantInfoDto,
    ResourceProviderDto,
    ResourcePermissionDefinitionDto,
    SearchProviderKeyInfo,
    ResourcePermissionWithProdiverGrantInfoDto,
} from '@abp/ng.permission-management/proxy';
import { eResourcePermissionViewModes } from '../enums/view-modes';

@Injectable()
export class ResourcePermissionStateService {
    // View state
    readonly viewMode = signal<eResourcePermissionViewModes>(eResourcePermissionViewModes.List);
    readonly modalBusy = signal(false);
    readonly hasResourcePermission = signal(false);
    readonly hasProviderKeyLookupService = signal(false);

    // Resource data
    readonly resourceName = signal('');
    readonly resourceKey = signal('');
    readonly resourceDisplayName = signal<string | undefined>(undefined);

    // Permissions data
    readonly allResourcePermissions = signal<ResourcePermissionGrantInfoDto[]>([]);
    readonly resourcePermissions = signal<ResourcePermissionGrantInfoDto[]>([]);
    readonly totalCount = signal(0);
    readonly permissionDefinitions = signal<ResourcePermissionDefinitionDto[]>([]);
    readonly permissionsWithProvider = signal<ResourcePermissionWithProdiverGrantInfoDto[]>([]);
    readonly selectedPermissions = signal<string[]>([]);

    // Provider data
    readonly providers = signal<ResourceProviderDto[]>([]);
    readonly selectedProviderName = signal('');
    readonly selectedProviderKey = signal('');

    // Edit mode specific
    readonly editProviderName = signal('');
    readonly editProviderKey = signal('');

    // Search state
    readonly searchFilter = signal('');
    readonly searchResults = signal<SearchProviderKeyInfo[]>([]);
    readonly showDropdown = signal(false);

    // Computed properties
    readonly isAddMode = computed(() => this.viewMode() === eResourcePermissionViewModes.Add);
    readonly isEditMode = computed(() => this.viewMode() === eResourcePermissionViewModes.Edit);
    readonly isListMode = computed(() => this.viewMode() === eResourcePermissionViewModes.List);

    readonly currentPermissionsList = computed(() =>
        this.isAddMode() ? this.permissionDefinitions() : this.permissionsWithProvider()
    );

    readonly allPermissionsSelected = computed(() => {
        const definitions = this.currentPermissionsList();
        return definitions.length > 0 &&
            definitions.every(p => this.selectedPermissions().includes(p.name || ''));
    });

    readonly canSave = computed(() => {
        if (this.isAddMode()) {
            return !!this.selectedProviderKey() && this.selectedPermissions().length > 0;
        }
        return this.selectedPermissions().length >= 0;
    });

    // State transition methods
    goToListMode() {
        this.viewMode.set(eResourcePermissionViewModes.List);
        this.selectedPermissions.set([]);
    }

    goToAddMode() {
        this.viewMode.set(eResourcePermissionViewModes.Add);
        this.selectedPermissions.set([]);
        this.selectedProviderKey.set('');
        this.searchResults.set([]);
        this.searchFilter.set('');
    }

    prepareEditMode(grant: ResourcePermissionGrantInfoDto) {
        this.editProviderName.set(grant.providerName || '');
        this.editProviderKey.set(grant.providerKey || '');
    }

    setEditModePermissions(permissions: ResourcePermissionWithProdiverGrantInfoDto[]) {
        this.permissionsWithProvider.set(permissions);
        this.selectedPermissions.set(
            permissions.filter(p => p.isGranted).map(p => p.name || '')
        );
        this.viewMode.set(eResourcePermissionViewModes.Edit);
    }

    // Permission selection methods
    togglePermission(permissionName: string) {
        const current = this.selectedPermissions();
        if (current.includes(permissionName)) {
            this.selectedPermissions.set(current.filter(p => p !== permissionName));
        } else {
            this.selectedPermissions.set([...current, permissionName]);
        }
    }

    toggleAllPermissions(selectAll: boolean) {
        const permissions = this.currentPermissionsList();
        this.selectedPermissions.set(
            selectAll ? permissions.map(p => p.name || '') : []
        );
    }

    isPermissionSelected(permissionName: string): boolean {
        return this.selectedPermissions().includes(permissionName);
    }

    // Provider search methods
    onProviderChange(providerName: string) {
        this.selectedProviderName.set(providerName);
        this.selectedProviderKey.set('');
        this.searchResults.set([]);
        this.searchFilter.set('');
    }

    selectProviderKey(key: SearchProviderKeyInfo) {
        this.selectedProviderKey.set(key.providerKey || '');
        this.searchFilter.set(key.providerDisplayName || key.providerKey || '');
        this.searchResults.set([]);
        this.showDropdown.set(false);
    }

    // Reset all state
    reset() {
        this.viewMode.set(eResourcePermissionViewModes.List);
        this.allResourcePermissions.set([]);
        this.resourcePermissions.set([]);
        this.totalCount.set(0);
        this.selectedProviderName.set('');
        this.selectedProviderKey.set('');
        this.searchFilter.set('');
        this.selectedPermissions.set([]);
        this.searchResults.set([]);
        this.editProviderName.set('');
        this.editProviderKey.set('');
    }

    // Data loading helpers
    setResourceData(permissions: ResourcePermissionGrantInfoDto[]) {
        this.allResourcePermissions.set(permissions);
        this.totalCount.set(permissions.length);
    }

    setProviders(providers: ResourceProviderDto[]) {
        this.providers.set(providers);
        this.hasProviderKeyLookupService.set(providers.length > 0);
        if (providers.length) {
            this.selectedProviderName.set(providers[0].name || '');
        }
    }

    setDefinitions(permissions: ResourcePermissionDefinitionDto[]) {
        this.permissionDefinitions.set(permissions);
        this.hasResourcePermission.set(permissions.length > 0);
    }
}
