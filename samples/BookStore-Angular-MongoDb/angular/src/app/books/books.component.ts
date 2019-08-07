import { Component, OnInit } from '@angular/core';
import { Store, Select } from '@ngxs/store';
import { BooksState } from '../store/states';
import { Observable } from 'rxjs';
import { Books } from '../store/models';
import { BooksGet, BooksSave } from '../store/actions';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbDateAdapter, NgbDateNativeAdapter } from '@ng-bootstrap/ng-bootstrap';
import { BooksService } from './services/books.service';

@Component({
  selector: 'app-books',
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }],
})
export class BooksComponent implements OnInit {
  @Select(BooksState.getBooks)
  books$: Observable<Books.Item[]>;

  bookType = Books.Type;

  bookTypes = Object.keys(Books.Type).filter(bookType => typeof this.bookType[bookType] === 'number');

  loading = false;

  isModalOpen = false;

  form: FormGroup;

  selectedBook = {} as Books.Item;

  constructor(private store: Store, private fb: FormBuilder, private booksService: BooksService) {}

  ngOnInit() {
    this.loading = true;
    this.store.dispatch(new BooksGet()).subscribe(() => (this.loading = false));
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.selectedBook.name || '', Validators.required],
      type: this.selectedBook.type || null,
      publishDate: this.selectedBook.publishDate ? new Date(this.selectedBook.publishDate) : null,
      price: this.selectedBook.price || null,
    });
  }

  onAdd() {
    this.selectedBook = {} as Books.Item;
    this.buildForm();
    this.isModalOpen = true;
  }

  onEdit(id: string) {
    this.booksService.getById(id).subscribe(book => {
      this.selectedBook = book;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    this.store.dispatch(new BooksSave(this.form.value, this.selectedBook.id)).subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
    });
  }
}
