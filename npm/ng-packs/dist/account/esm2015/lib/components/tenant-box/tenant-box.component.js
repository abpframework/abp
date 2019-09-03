/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { SetTenant, SessionState } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { throwError } from 'rxjs';
import { catchError, take } from 'rxjs/operators';
import snq from 'snq';
import { AccountService } from '../../services/account.service';
export class TenantBoxComponent {
    /**
     * @param {?} store
     * @param {?} toasterService
     * @param {?} accountService
     */
    constructor(store, toasterService, accountService) {
        this.store = store;
        this.toasterService = toasterService;
        this.accountService = accountService;
        this.tenant = (/** @type {?} */ ({}));
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        this.tenant = this.store.selectSnapshot(SessionState.getTenant) || ((/** @type {?} */ ({})));
        this.tenantName = this.tenant.name || '';
    }
    /**
     * @return {?}
     */
    onSwitch() {
        this.isModalVisible = true;
    }
    /**
     * @return {?}
     */
    save() {
        if (this.tenant.name) {
            this.accountService
                .findTenant(this.tenant.name)
                .pipe(take(1), catchError((/**
             * @param {?} err
             * @return {?}
             */
            err => {
                this.toasterService.error(snq((/**
                 * @return {?}
                 */
                () => err.error.error_description), 'AbpUi::DefaultErrorMessage'), 'AbpUi::Error');
                return throwError(err);
            })))
                .subscribe((/**
             * @param {?} __0
             * @return {?}
             */
            ({ success, tenantId }) => {
                if (success) {
                    this.tenant = {
                        id: tenantId,
                        name: this.tenant.name,
                    };
                    this.tenantName = this.tenant.name;
                    this.isModalVisible = false;
                }
                else {
                    this.toasterService.error(`AbpUiMultiTenancy::GivenTenantIsNotAvailable`, 'AbpUi::Error', {
                        messageLocalizationParams: [this.tenant.name],
                    });
                    this.tenant = (/** @type {?} */ ({}));
                }
                this.store.dispatch(new SetTenant(success ? this.tenant : null));
            }));
        }
        else {
            this.store.dispatch(new SetTenant(null));
            this.tenantName = null;
            this.isModalVisible = false;
        }
    }
}
TenantBoxComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-tenant-box',
                template: "<div\n  class=\"tenant-switch-box\"\n  style=\"background-color: #eee; margin-bottom: 20px; color: #000; padding: 10px; text-align: center;\"\n>\n  <span style=\"color: #666;\">{{ 'AbpUiMultiTenancy::Tenant' | abpLocalization }}: </span>\n  <strong>\n    <i>{{ tenantName || ('AbpUiMultiTenancy::NotSelected' | abpLocalization) }}</i>\n  </strong>\n  (<a id=\"abp-tenant-switch-link\" style=\"color: #333; cursor: pointer\" (click)=\"onSwitch()\">{{\n    'AbpUiMultiTenancy::Switch' | abpLocalization\n  }}</a\n  >)\n</div>\n\n<abp-modal [(visible)]=\"isModalVisible\" size=\"md\">\n  <ng-template #abpHeader>\n    <h5>Switch Tenant</h5>\n  </ng-template>\n  <ng-template #abpBody>\n    <form (ngSubmit)=\"save()\">\n      <div class=\"mt-2\">\n        <div class=\"form-group\">\n          <label for=\"name\">{{ 'AbpUiMultiTenancy::Name' | abpLocalization }}</label>\n          <input [(ngModel)]=\"tenant.name\" type=\"text\" id=\"name\" name=\"tenant\" class=\"form-control\" autofocus />\n        </div>\n        <p>{{ 'AbpUiMultiTenancy::SwitchTenantHint' | abpLocalization }}</p>\n      </div>\n    </form>\n  </ng-template>\n  <ng-template #abpFooter>\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n    </button>\n    <button type=\"button\" class=\"btn btn-primary\" (click)=\"save()\">\n      <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpTenantManagement::Save' | abpLocalization }}</span>\n    </button>\n  </ng-template>\n</abp-modal>\n"
            }] }
];
/** @nocollapse */
TenantBoxComponent.ctorParameters = () => [
    { type: Store },
    { type: ToasterService },
    { type: AccountService }
];
if (false) {
    /** @type {?} */
    TenantBoxComponent.prototype.tenant;
    /** @type {?} */
    TenantBoxComponent.prototype.tenantName;
    /** @type {?} */
    TenantBoxComponent.prototype.isModalVisible;
    /**
     * @type {?}
     * @private
     */
    TenantBoxComponent.prototype.store;
    /**
     * @type {?}
     * @private
     */
    TenantBoxComponent.prototype.toasterService;
    /**
     * @type {?}
     * @private
     */
    TenantBoxComponent.prototype.accountService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LWJveC5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmFjY291bnQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy90ZW5hbnQtYm94L3RlbmFudC1ib3guY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQU8sU0FBUyxFQUFFLFlBQVksRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUM1RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdEQsT0FBTyxFQUFFLFNBQVMsRUFBVSxNQUFNLGVBQWUsQ0FBQztBQUNsRCxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDbEMsT0FBTyxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNsRCxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFDdEIsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLGdDQUFnQyxDQUFDO0FBTWhFLE1BQU0sT0FBTyxrQkFBa0I7Ozs7OztJQUM3QixZQUFvQixLQUFZLEVBQVUsY0FBOEIsRUFBVSxjQUE4QjtRQUE1RixVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQVUsbUJBQWMsR0FBZCxjQUFjLENBQWdCO1FBQVUsbUJBQWMsR0FBZCxjQUFjLENBQWdCO1FBRWhILFdBQU0sR0FBRyxtQkFBQSxFQUFFLEVBQWlCLENBQUM7SUFGc0YsQ0FBQzs7OztJQVFwSCxRQUFRO1FBQ04sSUFBSSxDQUFDLE1BQU0sR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxZQUFZLENBQUMsU0FBUyxDQUFDLElBQUksQ0FBQyxtQkFBQSxFQUFFLEVBQWlCLENBQUMsQ0FBQztRQUN6RixJQUFJLENBQUMsVUFBVSxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxJQUFJLEVBQUUsQ0FBQztJQUMzQyxDQUFDOzs7O0lBRUQsUUFBUTtRQUNOLElBQUksQ0FBQyxjQUFjLEdBQUcsSUFBSSxDQUFDO0lBQzdCLENBQUM7Ozs7SUFFRCxJQUFJO1FBQ0YsSUFBSSxJQUFJLENBQUMsTUFBTSxDQUFDLElBQUksRUFBRTtZQUNwQixJQUFJLENBQUMsY0FBYztpQkFDaEIsVUFBVSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxDQUFDO2lCQUM1QixJQUFJLENBQ0gsSUFBSSxDQUFDLENBQUMsQ0FBQyxFQUNQLFVBQVU7Ozs7WUFBQyxHQUFHLENBQUMsRUFBRTtnQkFDZixJQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FDdkIsR0FBRzs7O2dCQUFDLEdBQUcsRUFBRSxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUMsaUJBQWlCLEdBQUUsNEJBQTRCLENBQUMsRUFDcEUsY0FBYyxDQUNmLENBQUM7Z0JBQ0YsT0FBTyxVQUFVLENBQUMsR0FBRyxDQUFDLENBQUM7WUFDekIsQ0FBQyxFQUFDLENBQ0g7aUJBQ0EsU0FBUzs7OztZQUFDLENBQUMsRUFBRSxPQUFPLEVBQUUsUUFBUSxFQUFFLEVBQUUsRUFBRTtnQkFDbkMsSUFBSSxPQUFPLEVBQUU7b0JBQ1gsSUFBSSxDQUFDLE1BQU0sR0FBRzt3QkFDWixFQUFFLEVBQUUsUUFBUTt3QkFDWixJQUFJLEVBQUUsSUFBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJO3FCQUN2QixDQUFDO29CQUNGLElBQUksQ0FBQyxVQUFVLEdBQUcsSUFBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUM7b0JBQ25DLElBQUksQ0FBQyxjQUFjLEdBQUcsS0FBSyxDQUFDO2lCQUM3QjtxQkFBTTtvQkFDTCxJQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FBQyw4Q0FBOEMsRUFBRSxjQUFjLEVBQUU7d0JBQ3hGLHlCQUF5QixFQUFFLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUM7cUJBQzlDLENBQUMsQ0FBQztvQkFDSCxJQUFJLENBQUMsTUFBTSxHQUFHLG1CQUFBLEVBQUUsRUFBaUIsQ0FBQztpQkFDbkM7Z0JBQ0QsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxTQUFTLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDO1lBQ25FLENBQUMsRUFBQyxDQUFDO1NBQ047YUFBTTtZQUNMLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksU0FBUyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUM7WUFDekMsSUFBSSxDQUFDLFVBQVUsR0FBRyxJQUFJLENBQUM7WUFDdkIsSUFBSSxDQUFDLGNBQWMsR0FBRyxLQUFLLENBQUM7U0FDN0I7SUFDSCxDQUFDOzs7WUF6REYsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxnQkFBZ0I7Z0JBQzFCLDBnREFBMEM7YUFDM0M7Ozs7WUFUUSxLQUFLO1lBRkwsY0FBYztZQU1kLGNBQWM7Ozs7SUFTckIsb0NBQTZCOztJQUU3Qix3Q0FBbUI7O0lBRW5CLDRDQUF3Qjs7Ozs7SUFOWixtQ0FBb0I7Ozs7O0lBQUUsNENBQXNDOzs7OztJQUFFLDRDQUFzQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCwgU2V0VGVuYW50LCBTZXNzaW9uU3RhdGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgVG9hc3RlclNlcnZpY2UgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBDb21wb25lbnQsIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyB0aHJvd0Vycm9yIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBjYXRjaEVycm9yLCB0YWtlIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuaW1wb3J0IHsgQWNjb3VudFNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcy9hY2NvdW50LnNlcnZpY2UnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtdGVuYW50LWJveCcsXG4gIHRlbXBsYXRlVXJsOiAnLi90ZW5hbnQtYm94LmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgVGVuYW50Qm94Q29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0IHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUsIHByaXZhdGUgdG9hc3RlclNlcnZpY2U6IFRvYXN0ZXJTZXJ2aWNlLCBwcml2YXRlIGFjY291bnRTZXJ2aWNlOiBBY2NvdW50U2VydmljZSkge31cblxuICB0ZW5hbnQgPSB7fSBhcyBBQlAuQmFzaWNJdGVtO1xuXG4gIHRlbmFudE5hbWU6IHN0cmluZztcblxuICBpc01vZGFsVmlzaWJsZTogYm9vbGVhbjtcblxuICBuZ09uSW5pdCgpIHtcbiAgICB0aGlzLnRlbmFudCA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoU2Vzc2lvblN0YXRlLmdldFRlbmFudCkgfHwgKHt9IGFzIEFCUC5CYXNpY0l0ZW0pO1xuICAgIHRoaXMudGVuYW50TmFtZSA9IHRoaXMudGVuYW50Lm5hbWUgfHwgJyc7XG4gIH1cblxuICBvblN3aXRjaCgpIHtcbiAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gdHJ1ZTtcbiAgfVxuXG4gIHNhdmUoKSB7XG4gICAgaWYgKHRoaXMudGVuYW50Lm5hbWUpIHtcbiAgICAgIHRoaXMuYWNjb3VudFNlcnZpY2VcbiAgICAgICAgLmZpbmRUZW5hbnQodGhpcy50ZW5hbnQubmFtZSlcbiAgICAgICAgLnBpcGUoXG4gICAgICAgICAgdGFrZSgxKSxcbiAgICAgICAgICBjYXRjaEVycm9yKGVyciA9PiB7XG4gICAgICAgICAgICB0aGlzLnRvYXN0ZXJTZXJ2aWNlLmVycm9yKFxuICAgICAgICAgICAgICBzbnEoKCkgPT4gZXJyLmVycm9yLmVycm9yX2Rlc2NyaXB0aW9uLCAnQWJwVWk6OkRlZmF1bHRFcnJvck1lc3NhZ2UnKSxcbiAgICAgICAgICAgICAgJ0FicFVpOjpFcnJvcicsXG4gICAgICAgICAgICApO1xuICAgICAgICAgICAgcmV0dXJuIHRocm93RXJyb3IoZXJyKTtcbiAgICAgICAgICB9KSxcbiAgICAgICAgKVxuICAgICAgICAuc3Vic2NyaWJlKCh7IHN1Y2Nlc3MsIHRlbmFudElkIH0pID0+IHtcbiAgICAgICAgICBpZiAoc3VjY2Vzcykge1xuICAgICAgICAgICAgdGhpcy50ZW5hbnQgPSB7XG4gICAgICAgICAgICAgIGlkOiB0ZW5hbnRJZCxcbiAgICAgICAgICAgICAgbmFtZTogdGhpcy50ZW5hbnQubmFtZSxcbiAgICAgICAgICAgIH07XG4gICAgICAgICAgICB0aGlzLnRlbmFudE5hbWUgPSB0aGlzLnRlbmFudC5uYW1lO1xuICAgICAgICAgICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IGZhbHNlO1xuICAgICAgICAgIH0gZWxzZSB7XG4gICAgICAgICAgICB0aGlzLnRvYXN0ZXJTZXJ2aWNlLmVycm9yKGBBYnBVaU11bHRpVGVuYW5jeTo6R2l2ZW5UZW5hbnRJc05vdEF2YWlsYWJsZWAsICdBYnBVaTo6RXJyb3InLCB7XG4gICAgICAgICAgICAgIG1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXM6IFt0aGlzLnRlbmFudC5uYW1lXSxcbiAgICAgICAgICAgIH0pO1xuICAgICAgICAgICAgdGhpcy50ZW5hbnQgPSB7fSBhcyBBQlAuQmFzaWNJdGVtO1xuICAgICAgICAgIH1cbiAgICAgICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBTZXRUZW5hbnQoc3VjY2VzcyA/IHRoaXMudGVuYW50IDogbnVsbCkpO1xuICAgICAgICB9KTtcbiAgICB9IGVsc2Uge1xuICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgU2V0VGVuYW50KG51bGwpKTtcbiAgICAgIHRoaXMudGVuYW50TmFtZSA9IG51bGw7XG4gICAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gZmFsc2U7XG4gICAgfVxuICB9XG59XG4iXX0=