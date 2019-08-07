import { HttpErrorResponse } from '@angular/common/http';
export declare class RestOccurError {
    payload: HttpErrorResponse | any;
    static readonly type = "[Rest] Error";
    constructor(payload: HttpErrorResponse | any);
}
