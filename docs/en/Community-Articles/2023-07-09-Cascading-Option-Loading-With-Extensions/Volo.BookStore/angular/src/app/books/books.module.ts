import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { BooksConfigOptions } from './models';
import { BooksExtensionsGuard } from './guards';
import { BOOK_STORE_RENT_FORM_PROP_CONTRIBUTORS } from './tokens';
import { BooksRoutingModule } from './books-routing.module';

@NgModule({
  imports: [
    BooksRoutingModule,
    CoreModule.forChild({
      localizations: [
        {
          culture: 'en',
          resources: [
            {
              resourceName: 'BookStore',
              texts: {
                RentedBooks: 'Rented books',
                RentABook: 'Rent a book',
                Author: 'Author',
              },
            },
          ],
        },
      ],
    }),
  ],
})
export class BooksModule {
  static forChild(options: BooksConfigOptions = {}): ModuleWithProviders<BooksModule> {
    return {
      ngModule: BooksModule,
      providers: [
        {
          provide: BOOK_STORE_RENT_FORM_PROP_CONTRIBUTORS,
          useValue: options.rentFormPropContributors,
        },
        BooksExtensionsGuard,
      ],
    };
  }

  static forLazy(options: BooksConfigOptions = {}): NgModuleFactory<BooksConfigOptions> {
    return new LazyModuleFactory(BooksModule.forChild(options));
  }
}
