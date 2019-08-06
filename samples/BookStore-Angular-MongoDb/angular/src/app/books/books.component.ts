import { Component, OnInit } from '@angular/core';
import { Store, Select } from '@ngxs/store';
import { BooksState } from '../store/states';
import { Observable } from 'rxjs';
import { Books } from '../store/models';
import { BooksGet } from '../store/actions';

@Component({
  selector: 'app-books',
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.scss'],
})
export class BooksComponent implements OnInit {
  @Select(BooksState.getBooks)
  books$: Observable<Books.Item[]>;

  bookType = Books.Type;

  loading = false;

  constructor(private store: Store) {}

  ngOnInit() {
    this.loading = true;
    this.store.dispatch(new BooksGet()).subscribe(() => (this.loading = false));
  }
}
