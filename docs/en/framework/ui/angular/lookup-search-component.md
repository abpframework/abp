```json
//[doc-seo]
{
    "Description": "Learn how to use the generic ABP Angular lookup search component with remote searches, two-way values and custom templates."
}
```

# Lookup Search Component

`LookupSearchComponent` is a generic standalone search control exported by `@abp/ng.components/lookup`. A search function returns observable lookup items, and the component manages debouncing, loading, selection and clearing.

This component is part of the open-source `@abp/ng.components` package. It is different from the [commercial lookup component family](./lookup-components.md), which provides form-oriented typeahead, select and table controls.

The package is included in the Angular application templates. Install it if the application does not already reference it:

```bash
npm install @abp/ng.components
```

```ts
import { Component, inject, signal } from '@angular/core';
import {
  LookupItem,
  LookupSearchComponent,
  LookupSearchFn,
} from '@abp/ng.components/lookup';
import { map } from 'rxjs';

interface BookLookupItem extends LookupItem {
  authorName: string;
}

@Component({
  selector: 'app-book-lookup',
  templateUrl: './book-lookup.component.html',
  imports: [LookupSearchComponent],
})
export class BookLookupComponent {
  private readonly bookService = inject(BookService);

  readonly selectedBookId = signal('');
  readonly selectedBookName = signal('');

  readonly searchBooks: LookupSearchFn<BookLookupItem> = filter =>
    this.bookService.search(filter).pipe(
      map(books =>
        books.map(book => ({
          key: book.id,
          displayName: book.name,
          authorName: book.authorName,
        })),
      ),
    );

  onBookSelected(book: BookLookupItem) {
    console.log(book.key);
  }
}
```

{%{
```html
<abp-lookup-search
  label="Books::Book"
  placeholder="Books::SearchBooks"
  [searchFn]="searchBooks"
  [minSearchLength]="2"
  [selectedValue]="selectedBookId()"
  (selectedValueChange)="selectedBookId.set($event)"
  [displayValue]="selectedBookName()"
  (displayValueChange)="selectedBookName.set($event)"
  (itemSelected)="onBookSelected($event)"
>
  <ng-template #itemTemplate let-book>
    <strong>{{ book.displayName }}</strong>
    <small>{{ book.authorName }}</small>
  </ng-template>
</abp-lookup-search>
```
}%}

Each lookup item uses `key` as its selected value and `displayName` as its displayed value by default. Set `valueKey` or `displayKey` to use another item property.

Searches use a 300 millisecond debounce by default. Configure `debounceTime` and `minSearchLength` when the remote endpoint needs different behavior. `label` and `placeholder` values are passed through ABP localization.

`selectedValue` and `displayValue` are model inputs and support two-way binding. The component also emits `searchChanged` for input changes and `itemSelected` after selection.

Add an `#itemTemplate` template to customize each result, as in the previous example. Add a `#noResultsTemplate` template to replace the default no-results content:

```html
<ng-template #noResultsTemplate>
  <div class="list-group-item text-muted">No matching books</div>
</ng-template>
```
