import {
    ExtensionsService,
    mergeWithDefaultProps,
} from '@abp/ng.components/extensible';
import { inject } from '@angular/core';
import {
    RESOURCE_PERMISSION_ENTITY_PROP_CONTRIBUTORS,
    DEFAULT_RESOURCE_PERMISSION_ENTITY_PROPS_MAP,
} from '../tokens';

export function configureResourcePermissionExtensions() {
    const extensions = inject(ExtensionsService);

    const config = { optional: true };

    const propContributors = inject(RESOURCE_PERMISSION_ENTITY_PROP_CONTRIBUTORS, config) || {};

    mergeWithDefaultProps(
        extensions.entityProps,
        DEFAULT_RESOURCE_PERMISSION_ENTITY_PROPS_MAP,
        propContributors,
    );
}
