import { HttpClient, HttpParameterCodec, HttpParams, HttpRequest } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ExternalHttpClient } from '../clients/http.client';
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
  protected options = inject<ABP.Root>(CORE_OPTIONS);
  protected http = inject(HttpClient);
  protected externalHttp = inject(ExternalHttpClient);
  protected environment = inject(EnvironmentService);
  protected httpErrorReporter = inject(HttpErrorReporterService);


  protected getApiFromStore(apiName: string | undefined): string {
    return this.environment.getApiUrl(apiName);
  }

  handleError(err: any): Observable<any> {
    this.httpErrorReporter.reportError(err);
    return throwError(() => err);
  }

  request<T, R>(
    request: HttpRequest<T> | Rest.Request<T>,
    config?: Rest.Config,
    api?: string,
  ): Observable<R> {
    config = config || ({} as Rest.Config);
    api = api || this.getApiFromStore(config.apiName);
    const { method, params, ...options } = request;
    const { observe = Rest.Observe.Body, skipHandleError, responseType = Rest.ResponseType.JSON } = config;
    const url = this.removeDuplicateSlashes(api + request.url);

    const httpClient: HttpClient = this.getHttpClient(config.skipAddingHeader);
    return httpClient
      .request<R>(method, url, {
        observe,
        responseType: responseType as any,
        ...(params && {
          params: this.getParams(params, config.httpParamEncoder),
        }),
        ...options,
      } as any)
      .pipe(catchError(err => (skipHandleError ? throwError(() => err) : this.handleError(err))));
  }
  private getHttpClient(isExternal: boolean) {
    return isExternal ? this.externalHttp : this.http;
  }

  private getParams(params: Rest.Params, encoder?: HttpParameterCodec): HttpParams {
    const filteredParams = Object.entries(params).reduce((acc, [key, value]) => {
      if (isUndefinedOrEmptyString(value)) return acc;
      if (value === null && !this.options.sendNullsAsQueryParam) return acc;
      acc[key] = value;
      return acc;
    }, {} as any);
    return encoder
      ? new HttpParams({ encoder, fromObject: filteredParams })
      : new HttpParams({ fromObject: filteredParams });
  }

  private removeDuplicateSlashes(url: string): string {
    return url.replace(/([^:]\/)\/+/g, '$1');
  }
}
