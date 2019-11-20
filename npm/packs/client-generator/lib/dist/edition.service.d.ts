import { RestService } from '@abp/ng.core';
import { Observable } from 'rxjs';
export declare class EditionService {
    private restService;
    constructor(restService: RestService);
    get(id: string): Observable<any>;
    getList(): Observable<any>;
    getUsageStatistics(): Observable<any>;
}
