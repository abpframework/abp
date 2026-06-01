import {
  AbpTenantService,
  ConfigStateService,
  CurrentTenantDto,
  SessionStateService,
} from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Injectable, inject } from '@angular/core';
import { finalize } from 'rxjs/operators';

@Injectable()
export class TenantBoxService {
  private toasterService = inject(ToasterService);
  private tenantService = inject(AbpTenantService);
  private sessionState = inject(SessionStateService);
  private configState = inject(ConfigStateService);

  currentTenant$ = this.sessionState.getTenant$();

  name?: string;

  isModalVisible!: boolean;

  modalBusy!: boolean;

  onSwitch() {
    const tenant = this.sessionState.getTenant();
    this.name = tenant?.name || '';
    this.isModalVisible = true;
  }

  save() {
    if (!this.name) {
      this.setTenant(null);
      this.isModalVisible = false;
      return;
    }

    this.modalBusy = true;
    this.tenantService
      .findTenantByName(this.name)
      .pipe(finalize(() => (this.modalBusy = false)))
      .subscribe(({ success, tenantId: id, ...tenant }) => {
        if (!success) {
          this.showError();
          return;
        }

        this.setTenant({ ...tenant, id, isAvailable: true });
        this.isModalVisible = false;
      });
  }

  private setTenant(tenant: CurrentTenantDto | null) {
    this.sessionState.setTenant(tenant);
    this.configState.refreshAppState();
  }

  private showError() {
    this.toasterService.error('AbpUiMultiTenancy::GivenTenantIsNotAvailable', 'AbpUi::Error', {
      messageLocalizationParams: [this.name || ''],
    });
  }
}
