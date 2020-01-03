import { ABP, SetTenant, SessionState } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { throwError } from 'rxjs';
import { catchError, take, finalize } from 'rxjs/operators';
import snq from 'snq';
import { AccountService } from '../../services/account.service';
import { Account } from '../../models/account';

@Component({
  selector: 'abp-tenant-box',
  templateUrl: './tenant-box.component.html',
})
export class TenantBoxComponent
  implements OnInit, Account.TenantBoxComponentInputs, Account.TenantBoxComponentOutputs {
  tenant = {} as ABP.BasicItem;

  tenantName: string;

  isModalVisible: boolean;

  inProgress: boolean;

  constructor(
    private store: Store,
    private toasterService: ToasterService,
    private accountService: AccountService,
  ) {}

  ngOnInit() {
    this.tenant = this.store.selectSnapshot(SessionState.getTenant) || ({} as ABP.BasicItem);
    this.tenantName = this.tenant.name || '';
  }

  onSwitch() {
    this.isModalVisible = true;
  }

  save() {
    if (this.tenant.name && !this.inProgress) {
      this.inProgress = true;
      this.accountService
        .findTenant(this.tenant.name)
        .pipe(
          finalize(() => (this.inProgress = false)),
          take(1),
          catchError(err => {
            this.toasterService.error(
              snq(() => err.error.error_description, 'AbpUi::DefaultErrorMessage'),
              'AbpUi::Error',
            );
            return throwError(err);
          }),
        )
        .subscribe(({ success, tenantId }) => {
          if (success) {
            this.tenant = {
              id: tenantId,
              name: this.tenant.name,
            };
            this.tenantName = this.tenant.name;
            this.isModalVisible = false;
          } else {
            this.toasterService.error(
              'AbpUiMultiTenancy::GivenTenantIsNotAvailable',
              'AbpUi::Error',
              {
                messageLocalizationParams: [this.tenant.name],
              },
            );
            this.tenant = {} as ABP.BasicItem;
          }
          this.store.dispatch(new SetTenant(success ? this.tenant : null));
        });
    } else {
      this.store.dispatch(new SetTenant(null));
      this.tenantName = null;
      this.isModalVisible = false;
    }
  }
}
