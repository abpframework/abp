import { HttpClient, HttpParameterCodec, HttpParams, HttpRequest } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ABP } from '../models/common';
import { Rest } from '../models/rest';
import { CORE_OPTIONS } from '../tokens/options.token';
import { isUndefinedOrEmptyString } from '../utils/common-utils';
import { EnvironmentService } from './environment.service';
import { HttpErrorReporterService } from './http-error-reporter.service';

@Injectable({
  providedIn: 'root',
})
export class RestService {
  constructor(
    @Inject(CORE_OPTIONS) protected options: ABP.Root,
    protected http: HttpClient,
    protected environment: EnvironmentService,
    protected httpErrorReporter: HttpErrorReporterService,
  ) {}

  protected getApiFromStore(apiName: string): string {
    return this.environment.getApiUrl(apiName);
  }

  handleError(err: any): Observable<any> {
    this.httpErrorReporter.reportError(err);
    return throwError(err);
  }

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
          params: this.getParams(params, config.httpParamEncoder),
        }),
        ...options,
      } as any)
      .pipe(catchError(err => (skipHandleError ? throwError(err) : this.handleError(err))));
  }

  private getParams(params: Rest.Params, encoder?: HttpParameterCodec): HttpParams {
    const filteredParams = Object.keys(params).reduce((acc, key) => {
      const value = params[key];
      if (isUndefinedOrEmptyString(value)) return acc;
      if (value === null && !this.options.sendNullsAsQueryParam) return acc;
      acc[key] = value;
      return acc;
    }, {});
    return encoder
      ? new HttpParams({ encoder, fromObject: filteredParams })
      : new HttpParams({ fromObject: filteredParams });
  }
}
