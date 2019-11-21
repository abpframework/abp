import { Store } from '@ngxs/store';
export declare class PermissionManagementStateService {
    private store;
    constructor(store: Store);
    getPermissionGroups(): import("../models").PermissionManagement.Group[];
    getEntityDisplayName(): string;
}
