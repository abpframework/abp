import { uuid } from '@abp/ng.core';
import { Injectable, signal } from '@angular/core';

@Injectable()
export class AuthorService {
  readonly authors = signal([
    { id: uuid(), name: 'J.K. Rowling' },
    { id: uuid(), name: 'George R.R. Martin' },
    { id: uuid(), name: 'Stephen King' },
    { id: uuid(), name: 'J.R.R. Tolkien' },
  ]);
}
