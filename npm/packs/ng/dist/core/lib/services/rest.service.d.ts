import { HttpClient, HttpRequest } from '@angular/common/http';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { Rest } from '../models/rest';
export declare class RestService {
    private http;
    private store;
    constructor(http: HttpClient, store: Store);
    handleError(err: any): Observable<any>;
    request<T, R>(request: HttpRequest<T> | Rest.Request<T>, config?: Rest.Config, api?: string): Observable<R>;
}
