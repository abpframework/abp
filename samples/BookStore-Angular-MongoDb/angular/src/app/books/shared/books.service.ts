import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import { Books } from '../../store/models';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BooksService {
  constructor(private restService: RestService) {}

  get(): Observable<Books.Response> {
    return this.restService.request<void, Books.Response>({
      method: 'GET',
      url: '/api/app/book?MaxResultCount=100',
    });
  }

  create(createBookInput: Books.CreateUpdateBookInput): Observable<Books.Book> {
    return this.restService.request<Books.CreateUpdateBookInput, Books.Book>({
      method: 'POST',
      url: '/api/app/book',
      body: createBookInput,
    });
  }

  getById(id: string): Observable<Books.Book> {
    return this.restService.request<void, Books.Book>({
      method: 'GET',
      url: `/api/app/book/${id}`,
    });
  }

  update(updateBookInput: Books.CreateUpdateBookInput, id: string): Observable<Books.Book> {
    return this.restService.request<Books.CreateUpdateBookInput, Books.Book>({
      method: 'PUT',
      url: `/api/app/book/${id}`,
      body: updateBookInput,
    });
  }

  delete(id: string): Observable<void> {
    return this.restService.request<void, void>({
      method: 'DELETE',
      url: `/api/app/book/${id}`,
    });
  }
}
