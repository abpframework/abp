import { Injectable } from '@angular/core';
import { RestService, Rest } from '@abp/ng.core';
import { Books } from '../../store/models';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BooksService {
  constructor(private rest: RestService) {}

  get(): Observable<Books.Response> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: '/api/app/book',
    };

    return this.rest.request<null, Books.Response>(request);
  }

  create(body: Books.CreateUpdateBookInput): Observable<Books.Book> {
    const request: Rest.Request<Books.CreateUpdateBookInput> = {
      method: 'POST',
      url: '/api/app/book',
      body,
    };

    return this.rest.request<Books.CreateUpdateBookInput, Books.Book>(request);
  }

  getById(id: string): Observable<Books.Book> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: `/api/app/book/${id}`,
    };

    return this.rest.request<null, Books.Book>(request);
  }

  update(body: Books.CreateUpdateBookInput, id: string): Observable<Books.Book> {
    const request: Rest.Request<Books.CreateUpdateBookInput> = {
      method: 'PUT',
      url: `/api/app/book/${id}`,
      body,
    };

    return this.rest.request<Books.CreateUpdateBookInput, Books.Book>(request);
  }

  delete(id: string): Observable<null> {
    const request: Rest.Request<null> = {
      method: 'DELETE',
      url: `/api/app/book/${id}`,
    };

    return this.rest.request<null, null>(request);
  }
}
