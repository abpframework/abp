import { HttpHeaders, HttpParameterCodec, HttpParams } from '@angular/common/http';

export namespace Rest {
  export type Config = Partial<{
    apiName: string;
    skipHandleError: boolean;
    observe: Observe;
    httpParamEncoder?: HttpParameterCodec;
  }>;

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

  export type Params = HttpParams | { [param: string]: any };

  export interface Request<T> {
    body?: T;
    headers?:
      | HttpHeaders
      | {
          [header: string]: string | string[];
        };
    method: string;
    params?: Params;
    reportProgress?: boolean;
    responseType?: 'arraybuffer' | 'blob' | 'json' | 'text';
    url: string;
    withCredentials?: boolean;
  }
}
