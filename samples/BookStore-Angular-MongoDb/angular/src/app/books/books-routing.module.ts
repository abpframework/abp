import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutApplicationComponent } from '@abp/ng.theme.basic';
import { BooksComponent } from './books.component';

const routes: Routes = [
  { path: '', component: LayoutApplicationComponent, children: [{ path: '', component: BooksComponent }] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BooksRoutingModule {}
