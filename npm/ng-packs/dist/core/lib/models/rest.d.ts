import { HttpHeaders, HttpParams } from '@angular/common/http';
export declare namespace Rest {
    interface Config {
        skipHandleError?: boolean;
        observe?: Observe;
    }
    const enum Observe {
        Body = "body",
        Events = "events",
        Response = "response"
    }
    const enum ResponseType {
        ArrayBuffer = "arraybuffer",
        Blob = "blob",
        JSON = "json",
        Text = "text"
    }
    interface Request<T> {
        body?: T;
        headers?: HttpHeaders | {
            [header: string]: string | string[];
        };
        method: string;
        params?: HttpParams | {
            [param: string]: any;
        };
        reportProgress?: boolean;
        responseType?: ResponseType;
        url: string;
        withCredentials?: boolean;
    }
}
