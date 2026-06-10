import { ResourcePermissionGrantInfoDto } from '@abp/ng.permission-management/proxy';
import { EntityPropContributorCallback } from '@abp/ng.components/extensible';
import { InjectionToken } from '@angular/core';
import { DEFAULT_RESOURCE_PERMISSION_ENTITY_PROPS } from '../defaults/default-resource-permission-entity-props';
import { ePermissionManagementComponents } from '../enums/components';

export const DEFAULT_RESOURCE_PERMISSION_ENTITY_PROPS_MAP = {
    [ePermissionManagementComponents.ResourcePermissions]: DEFAULT_RESOURCE_PERMISSION_ENTITY_PROPS,
};

export const RESOURCE_PERMISSION_ENTITY_PROP_CONTRIBUTORS = new InjectionToken<EntityPropContributors>(
    'RESOURCE_PERMISSION_ENTITY_PROP_CONTRIBUTORS',
);

type EntityPropContributors = Partial<{
    [ePermissionManagementComponents.ResourcePermissions]: EntityPropContributorCallback<ResourcePermissionGrantInfoDto>[];
}>;
