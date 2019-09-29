import { ABP } from '../models';
export declare class SetLanguage {
    payload: string;
    static readonly type = "[Session] Set Language";
    constructor(payload: string);
}
export declare class SetTenant {
    payload: ABP.BasicItem;
    static readonly type = "[Session] Set Tenant";
    constructor(payload: ABP.BasicItem);
}
