import { eLayoutType } from '@abp/ng.core';
import { SettingTab } from '@abp/ng.theme.shared';
import { TrackByFunction } from '@angular/core';
import { Router } from '@angular/router';
import { SettingManagementService } from '../services/setting-management.service';
export declare class SettingLayoutComponent {
    settingManagementService: SettingManagementService;
    private router;
    static type: eLayoutType;
    trackByFn: TrackByFunction<SettingTab>;
    constructor(settingManagementService: SettingManagementService, router: Router);
    ngOnDestroy(): void;
}
