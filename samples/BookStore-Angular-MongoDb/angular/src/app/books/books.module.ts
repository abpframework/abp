import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BooksRoutingModule } from './books-routing.module';
import { BooksComponent } from './books.component';
import { SharedModule } from '../shared/shared.module';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { BookListComponent } from './book-list/book-list.component';

@NgModule({
  declarations: [BooksComponent, BookListComponent],
  imports: [CommonModule, BooksRoutingModule, SharedModule, NgbDatepickerModule],
})
export class BooksModule {}
