import { uuid } from '@abp/ng.core';
import { Injectable, signal } from '@angular/core';

@Injectable()
export class BooksService {
  readonly genres = signal([
    { id: uuid(), name: 'Fantasy' },
    { id: uuid(), name: 'Science Fiction' },
    { id: uuid(), name: 'Romance' },
  ]);

  readonly books = signal([
    { id: uuid(), name: 'The Shining', author: 'Stephen King', genre: 'Science Fiction' },
    { id: uuid(), name: 'The Notebook', author: 'Nicholas Sparks', genre: 'Romance' },
    { id: uuid(), name: 'The Last Song', author: 'Nicholas Sparks', genre: 'Romance' },
  ]);

  readonly rentedBooks = signal([
    {
      id: uuid(),
      name: 'The Lord of the Rings',
      author: 'J. R. R. Tolkien',
      genre: 'Fantasy',
      returnDate: new Date('2024-01-01'),
    },
    {
      id: uuid(),
      name: 'The Hobbit',
      author: 'J. R. R. Tolkien',
      genre: 'Fantasy',
      returnDate: new Date('2024-02-01'),
    },
    {
      id: uuid(),
      name: 'The Stand',
      author: 'Stephen King',
      genre: 'Science Fiction',
      returnDate: new Date('2024-03-01'),
    },
  ]);
}
