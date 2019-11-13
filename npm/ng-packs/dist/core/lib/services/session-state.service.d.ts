import { Store } from '@ngxs/store';
export declare class SessionStateService {
    private store;
    constructor(store: Store);
    getLanguage(): string;
    getTenant(): import("../models").ABP.BasicItem;
}
