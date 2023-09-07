import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Injector,
  Output,
  inject,
} from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CoreModule, uuid } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import {
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  UiExtensionsModule,
  generateFormFromProps,
} from '@abp/ng.theme.shared/extensions';
import { AuthorService, BookDto, BooksService } from '../../proxy';
import { eBooksComponents } from '../../enums';

@Component({
  standalone: true,
  selector: 'app-rent-book',
  templateUrl: './rent-book.component.html',
  imports: [CoreModule, UiExtensionsModule, ThemeSharedModule],
  providers: [
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eBooksComponents.RentBook,
    },
    AuthorService,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RentBookComponent {
  protected readonly injector = inject(Injector);
  protected readonly authorService = inject(AuthorService);
  protected readonly booksService = inject(BooksService);

  //#region Just for demo
  readonly #authors = this.authorService.authors();
  readonly #genres = this.booksService.genres();
  readonly #books = this.booksService.books();
  //#endregion

  protected modalVisible = true;
  @Output() modalVisibleChange = new EventEmitter<boolean>();

  selected: BookDto;
  form: FormGroup;
  modalBusy = false;

  protected buildForm(): void {
    const data = new FormPropData(this.injector, this.selected);
    this.form = generateFormFromProps(data);
  }

  constructor() {
    this.buildForm();
  }

  save(): void {
    if (this.form.invalid) {
      return;
    }

    this.modalBusy = true;

    const { authorId, genreId, bookId, returnDate } = this.form.value;

    //#region Just for demo
    const authorName = this.#authors.find(({ id }) => id === authorId).name;
    const genreName = this.#genres.find(({ id }) => id === genreId).name;
    const bookName = this.#books.find(({ id }) => id === bookId).name;
    //#endregion

    this.booksService.rentedBooks.update(books => [
      {
        id: uuid(),
        name: bookName,
        author: authorName,
        genre: genreName,
        returnDate,
      },
      ...books,
    ]);

    this.modalBusy = false;
    this.modalVisible = false;
  }
}
