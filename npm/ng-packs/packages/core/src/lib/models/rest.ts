import { HttpHeaders, HttpParams } from '@angular/common/http';

export namespace Rest {
  export interface Config {
    skipHandleError?: boolean;
    observe?: Observe;
  }

  export const enum Observe {
    Body = 'body',
    Events = 'events',
    Response = 'response',
  }

  export const enum ResponseType {
    ArrayBuffer = 'arraybuffer',
    Blob = 'blob',
    JSON = 'json',
    Text = 'text',
  }

  export interface Request<T> {
    body?: T;
    headers?:
      | HttpHeaders
      | {
          [header: string]: string | string[];
        };
    method: string;
    params?:
      | HttpParams
      | {
          [param: string]: any;
        };
    reportProgress?: boolean;
    responseType?: ResponseType;
    url: string;
    withCredentials?: boolean;
  }
}
