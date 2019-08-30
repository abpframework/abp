import { Observable } from 'rxjs';
import { RestService } from './rest.service';
import { Profile } from '../models';
export declare class ProfileService {
    private rest;
    constructor(rest: RestService);
    get(): Observable<Profile.Response>;
    update(body: Profile.Response): Observable<Profile.Response>;
    changePassword(body: Profile.ChangePasswordRequest, skipHandleError?: boolean): Observable<null>;
}
