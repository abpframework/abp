import { ABP } from '../models';
export declare class PatchRouteByName {
    name: string;
    newValue: Partial<ABP.Route>;
    static readonly type = "[Config] Patch Route By Name";
    constructor(name: string, newValue: Partial<ABP.Route>);
}
export declare class GetAppConfiguration {
    static readonly type = "[Config] Get App Configuration";
}
