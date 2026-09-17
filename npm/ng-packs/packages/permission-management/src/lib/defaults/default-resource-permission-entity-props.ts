import { EntityProp, ePropType } from '@abp/ng.components/extensible';
import { ResourcePermissionGrantInfoDto } from '@abp/ng.permission-management/proxy';
import { of } from 'rxjs';

export const DEFAULT_RESOURCE_PERMISSION_ENTITY_PROPS = EntityProp.createMany<ResourcePermissionGrantInfoDto>([
    {
        type: ePropType.String,
        name: 'providerWithKey',
        displayName: 'AbpPermissionManagement::Provider',
        sortable: false,
        valueResolver: data => {
            const providerName = data.record.providerName || '';
            const providerDisplayName = data.record.providerDisplayName || data.record.providerKey || '';
            // Get first letter of provider name for abbreviation
            const abbr = providerName.charAt(0).toUpperCase();
            return of(
                `<span class="d-inline-block bg-light rounded-pill px-2 me-1 ms-1 mb-1" title="${data.record.providerNameDisplayName || providerName}">${abbr}</span>${providerDisplayName}`
            );
        },
    },
    {
        type: ePropType.String,
        name: 'permissions',
        displayName: 'AbpPermissionManagement::Permissions',
        sortable: false,
        valueResolver: data => {
            const permissions = data.record.permissions || [];
            const pills = permissions
                .map(p => `<span class="d-inline-block bg-light rounded-pill px-2 me-1 mb-1">${p.displayName}</span>`)
                .join('');
            return of(pills);
        },
    },
]);
