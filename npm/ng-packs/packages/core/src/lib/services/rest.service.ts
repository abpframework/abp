import { HttpClient, HttpHeaders, HttpParameterCodec, HttpParams, HttpRequest } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, from, of, throwError } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
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
    const effectiveResponseType =
      ((request as Rest.Request<T>).responseType as Rest.ResponseType | undefined) ?? responseType;
    const url = this.removeDuplicateSlashes(api + request.url);
    const headers = this.ensureAcceptHeader(options.headers, effectiveResponseType);

    const httpClient: HttpClient = this.getHttpClient(config.skipAddingHeader);
    return httpClient
      .request<R>(method, url, {
        observe,
        responseType: responseType as any,
        ...(params && {
          params: this.getParams(params, config.httpParamEncoder),
        }),
        ...options,
        ...(headers && { headers }),
      } as any)
      .pipe(
        catchError(err => {
          if (skipHandleError) {
            return throwError(() => err);
          }
          return this.normalizeErrorBody(err, effectiveResponseType).pipe(
            switchMap(normalizedErr => this.handleError(normalizedErr)),
          );
        }),
      );
  }
  private getHttpClient(isExternal: boolean) {
    return isExternal ? this.externalHttp : this.http;
  }

  private ensureAcceptHeader(
    headers: Rest.Request<unknown>['headers'],
    responseType: Rest.ResponseType,
  ): Rest.Request<unknown>['headers'] | undefined {
    const accept = this.getAcceptForResponseType(responseType);
    if (!accept) return headers;
    if (this.hasAcceptHeader(headers)) return headers;
    if (headers instanceof HttpHeaders) {
      return headers.set('Accept', accept);
    }
    return { ...(headers || {}), Accept: accept };
  }

  private hasAcceptHeader(headers: Rest.Request<unknown>['headers']): boolean {
    if (!headers) return false;
    if (headers instanceof HttpHeaders) return headers.has('Accept');
    return Object.keys(headers).some(key => key.toLowerCase() === 'accept');
  }

  private getAcceptForResponseType(responseType: Rest.ResponseType): string | null {
    switch (responseType) {
      case Rest.ResponseType.Blob:
      case Rest.ResponseType.ArrayBuffer:
        return 'application/octet-stream';
      default:
        return null;
    }
  }

  private normalizeErrorBody(err: any, responseType: Rest.ResponseType): Observable<any> {
    if (!err || responseType === Rest.ResponseType.JSON) {
      return of(err);
    }

    if (typeof err.error === 'string') {
      this.tryParseJsonErrorText(err, err.error);
      return of(err);
    }

    if (typeof Blob !== 'undefined' && err.error instanceof Blob) {
      return from(this.readBlobAsText(err.error)).pipe(
        map((text: string) => {
          this.tryParseJsonErrorText(err, text);
          return err;
        }),
        catchError(() => of(err)),
      );
    }

    return of(err);
  }

  private readBlobAsText(blob: Blob): Promise<string> {
    if (typeof (blob as any).text === 'function') {
      return (blob as any).text();
    }
    return new Promise<string>((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => resolve(typeof reader.result === 'string' ? reader.result : '');
      reader.onerror = () => reject(reader.error);
      reader.readAsText(blob);
    });
  }

  private tryParseJsonErrorText(err: any, text: string): void {
    if (!text || text.length === 0) return;
    let parsed: any;
    try {
      parsed = JSON.parse(text);
    } catch {
      return;
    }
    if (this.looksLikeAbpErrorEnvelope(parsed)) {
      err.error = parsed;
    }
  }

  private looksLikeAbpErrorEnvelope(parsed: any): boolean {
    if (!parsed || typeof parsed !== 'object' || Array.isArray(parsed)) return false;
    const inner = parsed.error;
    if (!inner || typeof inner !== 'object') return false;
    return (
      typeof inner.code === 'string' ||
      typeof inner.message === 'string' ||
      Array.isArray(inner.validationErrors)
    );
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
