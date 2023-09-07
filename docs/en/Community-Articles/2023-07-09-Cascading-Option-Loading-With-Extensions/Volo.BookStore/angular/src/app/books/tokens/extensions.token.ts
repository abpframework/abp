import { CreateFormPropContributorCallback } from '@abp/ng.theme.shared/extensions';
import { InjectionToken } from '@angular/core';
import { BookDto } from '../proxy';
import { eBooksComponents } from '../enums';
import { DEFAULT_RENT_FORM_PROPS } from '../defaults';

export const DEFAULT_BOOK_STORE_CREATE_FORM_PROPS = {
  [eBooksComponents.RentBook]: DEFAULT_RENT_FORM_PROPS,
};

export const BOOK_STORE_RENT_FORM_PROP_CONTRIBUTORS =
  new InjectionToken<CreateFormPropContributors>('BOOK_STORE_RENT_FORM_PROP_CONTRIBUTORS');

type CreateFormPropContributors = Partial<{
  [eBooksComponents.RentBook]: CreateFormPropContributorCallback<BookDto>[];
  /**
   * Other creation form prop contributors...
   */
  // [eBooksComponents.CreateBook]: CreateFormPropContributorCallback<BookDto>[];
}>;
