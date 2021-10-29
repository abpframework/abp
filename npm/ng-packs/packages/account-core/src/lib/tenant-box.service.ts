import {
  AbpTenantService,
  ConfigStateService,
  CurrentTenantDto,
  SessionStateService,
} from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Injectable } from '@angular/core';
import { finalize } from 'rxjs/operators';

@Injectable()
export class TenantBoxService {
  currentTenant$ = this.sessionState.getTenant$();

  name: string;

  isModalVisible: boolean;

  modalBusy: boolean;

  constructor(
    private toasterService: ToasterService,
    private tenantService: AbpTenantService,
    private sessionState: SessionStateService,
    private configState: ConfigStateService,
  ) {}

  onSwitch() {
    const tenant = this.sessionState.getTenant();
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
    this.tenantService
      .findTenantByName(this.name, {})
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

  private setTenant(tenant: CurrentTenantDto) {
    this.sessionState.setTenant(tenant);
    this.configState.refreshAppState();
  }

  private showError() {
    this.toasterService.error('AbpUiMultiTenancy::GivenTenantIsNotAvailable', 'AbpUi::Error', {
      messageLocalizationParams: [this.name],
    });
  }
}
