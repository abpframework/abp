import { ABP, GetAppConfiguration, SessionStateService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize, take } from 'rxjs/operators';
import { Account } from '../../models/account';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'abp-tenant-box',
  templateUrl: './tenant-box.component.html',
})
export class TenantBoxComponent
  implements Account.TenantBoxComponentInputs, Account.TenantBoxComponentOutputs {
  currentTenant$ = this.sessionState.getTenant$();

  name: string;

  isModalVisible: boolean;

  modalBusy: boolean;

  constructor(
    private store: Store,
    private toasterService: ToasterService,
    private accountService: AccountService,
    private sessionState: SessionStateService,
  ) {}

  onSwitch() {
    const tenant = this.sessionState.getTenant;
    this.name = tenant?.name;
    this.isModalVisible = true;
  }

  save() {
    if (!this.name) {
      this.setTenant(null);
      this.isModalVisible = false;
      return;
    }

    this.modalBusy = true;
    this.accountService
      .findTenant(this.name)
      .pipe(finalize(() => (this.modalBusy = false)))
      .subscribe(({ success, tenantId: id, name }) => {
        if (!success) {
          this.showError();
          return;
        }

        this.setTenant({ id, name });
        this.isModalVisible = false;
      });
  }

  private setTenant(tenant: ABP.BasicItem) {
    this.sessionState.setTenant(tenant);
    return this.store.dispatch(new GetAppConfiguration());
  }

  private showError() {
    this.toasterService.error('AbpUiMultiTenancy::GivenTenantIsNotAvailable', 'AbpUi::Error', {
      messageLocalizationParams: [this.name],
    });
  }
}
