import { HttpRequest } from '@angular/common/http';
export declare class LoaderStart {
    payload: HttpRequest<any>;
    static readonly type = "[Loader] Start";
    constructor(payload: HttpRequest<any>);
}
export declare class LoaderStop {
    payload: HttpRequest<any>;
    static readonly type = "[Loader] Stop";
    constructor(payload: HttpRequest<any>);
}
