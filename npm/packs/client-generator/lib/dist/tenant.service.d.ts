import { RestService } from '@abp/ng.core';
import { Observable } from 'rxjs';
export declare class TenantService {
    private restService;
    constructor(restService: RestService);
    get(id: string): Observable<any>;
    getList(): Observable<any>;
    getDefaultConnectionString(id: string): Observable<any>;
}
