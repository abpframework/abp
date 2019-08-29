import { OnInit } from '@angular/core';
import { SettingTab } from '@abp/ng.theme.shared';
import { InitialService } from '../services/initial.service';
export declare class SettingComponent implements OnInit {
    private initialService;
    settings: SettingTab[];
    selected: SettingTab;
    constructor(initialService: InitialService);
    ngOnInit(): void;
}
