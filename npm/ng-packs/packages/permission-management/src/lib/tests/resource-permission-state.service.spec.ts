import { ResourcePermissionDefinitionDto } from '@abp/ng.permission-management/proxy';
import { ResourcePermissionStateService } from '../services/resource-permission-state.service';

describe('ResourcePermissionStateService', () => {
  const definitions: ResourcePermissionDefinitionDto[] = [
    { name: 'Any', displayName: 'Any', allowedProviders: [] },
    { name: 'ClientOnly', displayName: 'Client only', allowedProviders: ['C'] },
    { name: 'UserOrRole', displayName: 'User or role', allowedProviders: ['U', 'R'] },
  ];

  let state: ResourcePermissionStateService;

  beforeEach(() => {
    state = new ResourcePermissionStateService();
    state.setProviders([{ name: 'U' }, { name: 'R' }, { name: 'C' }]);
    state.setDefinitions(definitions);
    state.goToAddMode();
  });

  const names = (items: { name?: string }[]) => items.map(x => x.name);

  it('should only list the permissions allowed for the selected provider in add mode', () => {
    expect(names(state.currentPermissionsList())).toEqual(['Any', 'UserOrRole']);

    state.onProviderChange('C');

    expect(names(state.currentPermissionsList())).toEqual(['Any', 'ClientOnly']);
  });

  it('should treat a permission without allowed providers as allowed for every provider', () => {
    state.setDefinitions([{ name: 'Legacy', displayName: 'Legacy' }]);

    expect(names(state.currentPermissionsList())).toEqual(['Legacy']);
  });

  it('should only select the permissions allowed for the selected provider when selecting all', () => {
    state.toggleAllPermissions(true);

    expect(state.selectedPermissions()).toEqual(['Any', 'UserOrRole']);
    expect(state.allPermissionsSelected()).toBe(true);
  });

  it('should drop the selected permissions the new provider is not allowed to hold', () => {
    state.toggleAllPermissions(true);

    state.onProviderChange('C');

    expect(state.selectedPermissions()).toEqual(['Any']);
    expect(state.allPermissionsSelected()).toBe(false);
  });

  it('should not filter the permissions in edit mode', () => {
    state.setEditModePermissions([
      { name: 'ClientOnly', displayName: 'Client only', providers: ['C'], isGranted: true },
    ]);

    expect(names(state.currentPermissionsList())).toEqual(['ClientOnly']);
  });
});
