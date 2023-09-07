import { Validators } from '@angular/forms';
import { map } from 'rxjs';
import { ePropType, FormProp, FormPropList } from '@abp/ng.theme.shared/extensions';
import {
  BookDto,
  BookStoreRentFormPropContributors,
  BooksService,
  DefaultOption,
  RentBookComponent,
  eBooksComponents,
} from '@book-store/books';

const { required, maxLength } = Validators;

const bookIdProp = new FormProp<BookDto>({
  type: ePropType.String,
  id: 'bookId',
  name: 'bookId',
  displayName: 'BookStore::Name',
  options: data => {
    const rentBook = data.getInjected(RentBookComponent);
    const { books } = data.getInjected(BooksService);
    const bookOptions = books().map(({ id, name }) => ({ value: id, key: name }));

    return rentBook.form.controls.genreId.valueChanges.pipe(
      map((value: string | undefined) =>
        value ? [DefaultOption, ...bookOptions] : [DefaultOption]
      )
    );
  },
  validators: () => [required, maxLength(255)],
});

export function bookIdPropContributor(propList: FormPropList<BookDto>) {
  propList.addByIndex(bookIdProp, 2);
}

export const bookStoreRentFormPropContributors: BookStoreRentFormPropContributors = {
  [eBooksComponents.RentBook]: [bookIdPropContributor],
};
