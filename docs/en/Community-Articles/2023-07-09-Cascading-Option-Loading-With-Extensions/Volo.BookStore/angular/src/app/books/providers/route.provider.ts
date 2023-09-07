import { RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER, inject } from '@angular/core';

export const BOOK_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: provideBookStoreRoutes, multi: true },
];

function provideBookStoreRoutes() {
  const routesService = inject(RoutesService);

  return () => {
    routesService.add([
      {
        path: '/books',
        name: '::Menu:Books',
        iconClass: 'fas fa-book',
        order: 1,
        layout: eLayoutType.application,
      },
    ]);
  };
}
