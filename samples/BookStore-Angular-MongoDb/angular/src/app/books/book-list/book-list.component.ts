import { Component, OnInit } from "@angular/core";
import { Store, Select } from "@ngxs/store";
import { BooksState } from "../../store/states";
import { Observable } from "rxjs";
import { Books } from "../../store/models";
import { GetBooks, CreateUpdateBook, DeleteBook } from "../../store/actions";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import {
  NgbDateNativeAdapter,
  NgbDateAdapter
} from "@ng-bootstrap/ng-bootstrap";
import { BooksService } from "../shared/books.service";
import { ConfirmationService, Confirmation } from "@abp/ng.theme.shared";

@Component({
  selector: "app-book-list",
  templateUrl: "./book-list.component.html",
  styleUrls: ["./book-list.component.scss"],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }]
})
export class BookListComponent implements OnInit {
  @Select(BooksState.getBooks)
  books$: Observable<Books.Book[]>;

  booksType = Books.BookType;

  bookTypeArr = Object.keys(Books.BookType).filter(
    bookType => typeof this.booksType[bookType] === "number"
  );

  loading = false;

  isModalOpen = false;

  form: FormGroup;

  selectedBook = {} as Books.Book;

  constructor(
    private store: Store,
    private fb: FormBuilder,
    private booksService: BooksService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit() {
    this.get();
  }

  get() {
    this.loading = true;
    this.store.dispatch(new GetBooks()).subscribe(() => {
      this.loading = false;
    });
  }

  createBook() {
    this.selectedBook = {} as Books.Book;
    this.buildForm();
    this.isModalOpen = true;
  }

  editBook(id: string) {
    this.booksService.getById(id).subscribe(book => {
      this.selectedBook = book;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  buildForm() {
    console.warn(this.selectedBook);
    this.form = this.fb.group({
      name: [this.selectedBook.name || "", Validators.required],
      type: [this.selectedBook.type || null, Validators.required],
      publishDate: [
        this.selectedBook.publishDate
          ? new Date(this.selectedBook.publishDate)
          : null,
        Validators.required
      ],
      price: [this.selectedBook.price || null, Validators.required]
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    this.store
      .dispatch(new CreateUpdateBook(this.form.value, this.selectedBook.id))
      .subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.get();
      });
  }

  delete(id: string, name: string) {
    this.confirmationService
      .warn("::AreYouSureToDelete", "AbpAccount::AreYouSure")
      .subscribe(status => {
        if (status === Confirmation.Status.confirm) {
          this.store.dispatch(new DeleteBook(id)).subscribe(() => this.get());
        }
      });
  }
}
