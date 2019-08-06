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
}
