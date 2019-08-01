import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { BooksComponent } from './books.component';
import { LayoutApplicationComponent } from '@abp/ng.theme.basic';

const routes: Routes = [
  {
    path: '',
    component: LayoutApplicationComponent,
    children: [{ path: '', component: BooksComponent }],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BooksRoutingModule { }
