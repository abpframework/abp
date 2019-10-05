import { SettingTab } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { Store, Actions } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
export declare class SettingManagementService {
    private actions;
    private router;
    private store;
    private oAuthService;
    settings: SettingTab[];
    selected: SettingTab;
    private destroy$;
    constructor(actions: Actions, router: Router, store: Store, oAuthService: OAuthService);
    ngOnDestroy(): void;
    setSettings(): void;
    checkSelected(): void;
    setSelected(selected: SettingTab): void;
}
