import { Store } from '@ngxs/store';
export declare class ProfileStateService {
    private store;
    constructor(store: Store);
    getProfile(): import("../models").Profile.Response;
}
