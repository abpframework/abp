import { Observable } from 'rxjs';
import { ApplicationConfiguration } from '../models';
import { RestService } from './rest.service';
export declare class ApplicationConfigurationService {
    private rest;
    constructor(rest: RestService);
    getConfiguration(): Observable<ApplicationConfiguration.Response>;
}
