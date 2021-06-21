import { eLayoutType, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';
import { eCmsKitRouteNames } from '../enums/route-names';

export const MY_PROJECT_NAME_ROUTE_PROVIDERS = [
    {
        provide: APP_INITIALIZER,
        useFactory: configureRoutes,
        deps: [RoutesService],
        multi: true,
    },
];

export function configureRoutes(routesService: RoutesService) {
    return () => {
        routesService.add([
            {
                path: '/cms-kit',
                name: eCmsKitRouteNames.CmsKit,
                iconClass: 'fas fa-book',
                layout: eLayoutType.application,
                order: 3,
            },
        ]);
    };
}
