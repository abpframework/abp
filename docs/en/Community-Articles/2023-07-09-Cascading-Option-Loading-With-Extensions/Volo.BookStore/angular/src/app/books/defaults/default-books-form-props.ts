import { Validators } from '@angular/forms';
import { map, of } from 'rxjs';
import { ePropType, FormProp } from '@abp/ng.theme.shared/extensions';
import { BookDto, AuthorService, BooksService } from '../proxy';
import { RentBookComponent } from '../components';
import { DefaultOption } from '../utils';

const { required } = Validators;

export const DEFAULT_RENT_FORM_PROPS = FormProp.createMany<BookDto>([
  {
    type: ePropType.String,
    id: 'authorId',
    name: 'authorId',
    displayName: 'BookStore::Author',
    defaultValue: null,
    validators: () => [required],
    options: data => {
      const { authors } = data.getInjected(AuthorService);

      return of([
        DefaultOption,
        ...authors().map(author => ({ value: author.id, key: author.name })),
      ]);
    },
  },
  {
    type: ePropType.String,
    id: 'genreId',
    name: 'genreId',
    displayName: 'BookStore::Genre',
    defaultValue: null,
    validators: () => [required],
    options: data => {
      const rentBookComponent = data.getInjected(RentBookComponent);
      const { genres } = data.getInjected(BooksService);

      const genreOptions = genres().map(({ id, name }) => ({ value: id, key: name }));

      return rentBookComponent.form.controls.authorId.valueChanges.pipe(
        map((value: string | undefined) =>
          value ? [DefaultOption, ...genreOptions] : [DefaultOption]
        )
      );
    },
  },
  {
    type: ePropType.Date,
    id: 'returnDate',
    name: 'returnDate',
    displayName: 'BookStore::ReturnDate',
    defaultValue: null,
    validators: () => [required],
  },
]);
