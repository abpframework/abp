import { Component, OnInit } from '@angular/core';
import { TenantBoxService } from '@abp/ng.account.core';

@Component({
  selector: 'abp-tenant-box',
  templateUrl: './tenant-box.component.html',
  providers: [TenantBoxService],
})
export class TenantBoxComponent implements OnInit {
  constructor(public service: TenantBoxService) {}

  ngOnInit(): void {}
}
