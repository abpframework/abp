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

  getById(id: string): Observable<Books.Item> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: `/api/app/book/${id}`,
    };

    return this.rest.request<null, Books.Item>(request);
  }

  update(body: Books.SaveRequest, id: string): Observable<Books.Item> {
    const request: Rest.Request<Books.SaveRequest> = {
      method: 'PUT',
      url: `/api/app/book/${id}`,
      body,
    };

    return this.rest.request<Books.SaveRequest, Books.Item>(request);
  }

  add(body: Books.SaveRequest): Observable<Books.Item> {
    const request: Rest.Request<Books.SaveRequest> = {
      method: 'POST',
      url: '/api/app/book',
      body,
    };

    return this.rest.request<Books.SaveRequest, Books.Item>(request);
  }
}
