import { TrackByFunction, OnInit } from '@angular/core';
import { SettingTab } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
export declare class SettingManagementComponent implements OnInit {
    private router;
    private store;
    settings: SettingTab[];
    selected: SettingTab;
    trackByFn: TrackByFunction<SettingTab>;
    constructor(router: Router, store: Store);
    ngOnInit(): void;
}
