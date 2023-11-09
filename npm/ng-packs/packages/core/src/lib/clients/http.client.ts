import { HttpClient, HttpContext, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IS_EXTERNAL_REQUEST } from '../tokens/http-context.token';

// source : https://github.com/armanozak/demo-angular-server-specific-interceptors
@Injectable({
  providedIn: 'root',
})
export class ExternalHttpClient extends HttpClient {
  override request(
    first: string | HttpRequest<any>,
    url?: string,
    options: RequestOptions = {},
  ): Observable<any> {
    if (typeof first === 'string') {
      this.#setPlaceholderContext(options);
      return super.request(first, url || '', options);
    }

    this.#setPlaceholderContext(first);
    return super.request(first);
  }
  #setPlaceholderContext(optionsOrRequest: { context?: HttpContext }) {
    optionsOrRequest.context ??= new HttpContext();
    optionsOrRequest.context.set(IS_EXTERNAL_REQUEST, true);
  }
}

type RequestOptions = Parameters<HttpClient['request']>[2];
