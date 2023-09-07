import { NgModule } from '@angular/core';
import { RouterModule, Routes, mapToCanActivate } from '@angular/router';
import {
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';
import { eBooksComponents } from './enums';
import { BooksExtensionsGuard } from './guards';
import BooksComponent from './books.component';

const routes: Routes = [
  {
    path: '',
    component: RouterOutletComponent,
    canActivate: mapToCanActivate([BooksExtensionsGuard]),
    children: [
      {
        path: '',
        component: ReplaceableRouteContainerComponent,
        data: {
          replaceableComponent: {
            key: eBooksComponents.Books,
            defaultComponent: BooksComponent,
          } as ReplaceableComponents.RouteData<BooksComponent>,
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BooksRoutingModule {}
