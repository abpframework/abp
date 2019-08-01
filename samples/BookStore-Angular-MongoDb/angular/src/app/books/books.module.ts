import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BooksRoutingModule } from './books-routing.module';
import { BooksComponent } from './books.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [BooksComponent],
  imports: [
    CommonModule,
    BooksRoutingModule,
    SharedModule
  ]
})
export class BooksModule { }
