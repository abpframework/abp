import { ABP, SessionSetTenantId } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Store } from '@ngxs/store';
import { throwError } from 'rxjs';
import { catchError, take } from 'rxjs/operators';
import snq from 'snq';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'abp-tenant-box',
  templateUrl: './tenant-box.component.html',
})
export class TenantBoxComponent {
  constructor(private store: Store, private toasterService: ToasterService, private accountService: AccountService) {}

  selected = {} as ABP.BasicItem;

  isModalVisible: boolean;

  @ViewChild('modalContent', { static: false })
  modalContent: TemplateRef<any>;

  onSwitch() {
    this.isModalVisible = true;
  }

  save() {
    this.selected.name = this.selected.name ? this.selected.name : '';

    this.accountService
      .findTenant(this.selected.name)
      .pipe(
        take(1),
        catchError(err => {
          this.toasterService.error(snq(() => err.error.error_description, 'An error occured.'), 'Error');
          return throwError(err);
        }),
      )
      .subscribe(({ success, tenantId }) => {
        if (success) {
          this.store.dispatch(new SessionSetTenantId(tenantId));
          this.isModalVisible = false;
        } else {
          this.toasterService.error(`Given tenant is not available: ${this.selected.name}`, 'Error');
          this.selected = {} as ABP.BasicItem;
          this.store.dispatch(new SessionSetTenantId(null));
        }
      });
  }
}
