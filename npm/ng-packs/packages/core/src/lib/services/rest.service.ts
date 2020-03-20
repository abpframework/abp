import { HttpClient, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { RestOccurError } from '../actions/rest.actions';
import { Rest } from '../models/rest';
import { ConfigState } from '../states/config.state';
import { isUndefinedOrEmptyString } from '../utils/common-utils';

@Injectable({
  providedIn: 'root',
})
export class RestService {
  constructor(private http: HttpClient, private store: Store) {}

  private getApiFromStore(apiName: string): string {
    return this.store.selectSnapshot(ConfigState.getApiUrl(apiName));
  }

  handleError(err: any): Observable<any> {
    this.store.dispatch(new RestOccurError(err));
    return throwError(err);
  }

  // TODO: Deprecate service or improve interface in v3.0
  request<T, R>(
    request: HttpRequest<T> | Rest.Request<T>,
    config?: Rest.Config,
    api?: string,
  ): Observable<R> {
    config = config || ({} as Rest.Config);
    api = api || this.getApiFromStore(config.apiName);
    const { method, params, ...options } = request;
    const { observe = Rest.Observe.Body, skipHandleError } = config;

    return this.http
      .request<R>(method, api + request.url, {
        observe,
        ...(params && {
          params: Object.keys(params).reduce((acc, key) => {
            const value = params[key];

            if (!isUndefinedOrEmptyString(value)) acc[key] = value;

            return acc;
          }, {}),
        }),
        ...options,
      } as any)
      .pipe(catchError(err => (skipHandleError ? throwError(err) : this.handleError(err))));
  }
}
