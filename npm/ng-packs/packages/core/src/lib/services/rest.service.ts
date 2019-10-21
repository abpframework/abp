import { HttpClient, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable, throwError } from 'rxjs';
import { catchError, take, tap } from 'rxjs/operators';
import { RestOccurError } from '../actions/rest.actions';
import { Rest } from '../models/rest';
import { ConfigState } from '../states/config.state';

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

  request<T, R>(request: HttpRequest<T> | Rest.Request<T>, config?: Rest.Config, api?: string): Observable<R> {
    config = config || ({} as Rest.Config);
    const { observe = Rest.Observe.Body, skipHandleError } = config;
    const url = (api || this.store.selectSnapshot(ConfigState.getApiUrl())) + request.url;
    const { method, ...options } = request;

    return this.http.request<T>(method, url, { observe, ...options } as any).pipe(
      observe === Rest.Observe.Body ? take(1) : tap(),
      catchError(err => {
        if (skipHandleError) {
          return throwError(err);
        }

        return this.handleError(err);
      }),
    );
  }
}
