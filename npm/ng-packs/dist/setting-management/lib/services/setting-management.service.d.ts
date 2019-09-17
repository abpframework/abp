import { SettingTab } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
export declare class SettingManagementService {
    private router;
    private store;
    settings: SettingTab[];
    selected: SettingTab;
    private destroy$;
    constructor(router: Router, store: Store);
    ngOnDestroy(): void;
    setSettings(): void;
    checkSelected(): void;
    setSelected(selected: SettingTab): void;
}
