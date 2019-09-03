import { ABP } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { AccountService } from '../../services/account.service';
export declare class TenantBoxComponent implements OnInit {
    private store;
    private toasterService;
    private accountService;
    constructor(store: Store, toasterService: ToasterService, accountService: AccountService);
    tenant: ABP.BasicItem;
    tenantName: string;
    isModalVisible: boolean;
    ngOnInit(): void;
    onSwitch(): void;
    save(): void;
}
