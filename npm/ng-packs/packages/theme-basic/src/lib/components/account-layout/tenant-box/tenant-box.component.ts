import { TenantBoxService } from '@abp/ng.account.core';
import { Component } from '@angular/core';

@Component({
  selector: 'abp-tenant-box',
  templateUrl: './tenant-box.component.html',
  providers: [TenantBoxService],
})
export class TenantBoxComponent {
  constructor(public service: TenantBoxService) {}
}
