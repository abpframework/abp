import { HttpClient, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable, throwError } from 'rxjs';
import { catchError, take } from 'rxjs/operators';
import { RestOccurError } from '../actions/rest.actions';
import { Rest } from '../models/rest';

@Injectable({
  providedIn: 'root',
})
export class RestService {
  constructor(private http: HttpClient, private store: Store) {}

  handleError(err: any): Observable<any> {
    this.store.dispatch(new RestOccurError(err));
    console.error(err);
    return throwError(err);
  }

  request<T, R>(request: HttpRequest<T> | Rest.Request<T>, config: Rest.Config = {}, api?: string): Observable<R> {
    const { observe = Rest.Observe.Body, skipHandleError } = config;
    const url = api || this.store.selectSnapshot(state => state.ConfigState).environment.apis.default.url + request.url;
    const { method, ...options } = request;
    return this.http.request<T>(method, url, { observe, ...options } as any).pipe(
      observe === Rest.Observe.Body ? take(1) : null,
      catchError(err => {
        if (skipHandleError) {
          return throwError(err);
        }

        return this.handleError(err);
      }),
    );
  }
}
