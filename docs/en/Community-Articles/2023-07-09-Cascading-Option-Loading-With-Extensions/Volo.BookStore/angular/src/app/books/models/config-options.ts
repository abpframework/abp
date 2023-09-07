import { CreateFormPropContributorCallback } from '@abp/ng.theme.shared/extensions';
import { BookDto } from '../proxy';
import { eBooksComponents } from '../enums';

export type BookStoreRentFormPropContributors = Partial<{
  [eBooksComponents.RentBook]: CreateFormPropContributorCallback<BookDto>[];
}>;

export interface BooksConfigOptions {
  rentFormPropContributors?: BookStoreRentFormPropContributors;
}
