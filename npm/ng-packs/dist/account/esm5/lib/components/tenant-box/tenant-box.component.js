/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { SetTenant, SessionState } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { throwError } from 'rxjs';
import { catchError, take } from 'rxjs/operators';
import snq from 'snq';
import { AccountService } from '../../services/account.service';
var TenantBoxComponent = /** @class */ (function() {
  function TenantBoxComponent(store, toasterService, accountService) {
    this.store = store;
    this.toasterService = toasterService;
    this.accountService = accountService;
    this.tenant = /** @type {?} */ ({});
  }
  /**
   * @return {?}
   */
  TenantBoxComponent.prototype.ngOnInit
  /**
   * @return {?}
   */ = function() {
    this.tenant = this.store.selectSnapshot(SessionState.getTenant) || /** @type {?} */ ({});
    this.tenantName = this.tenant.name || '';
  };
  /**
   * @return {?}
   */
  TenantBoxComponent.prototype.onSwitch
  /**
   * @return {?}
   */ = function() {
    this.isModalVisible = true;
  };
  /**
   * @return {?}
   */
  TenantBoxComponent.prototype.save
  /**
   * @return {?}
   */ = function() {
    var _this = this;
    if (this.tenant.name) {
      this.accountService
        .findTenant(this.tenant.name)
        .pipe(
          take(1),
          catchError(
            /**
             * @param {?} err
             * @return {?}
             */
            function(err) {
              _this.toasterService.error(
                snq(
                  /**
                   * @return {?}
                   */
                  function() {
                    return err.error.error_description;
                  },
                  'AbpUi::DefaultErrorMessage',
                ),
                'AbpUi::Error',
              );
              return throwError(err);
            },
          ),
        )
        .subscribe(
          /**
           * @param {?} __0
           * @return {?}
           */
          function(_a) {
            var success = _a.success,
              tenantId = _a.tenantId;
            if (success) {
              _this.tenant = {
                id: tenantId,
                name: _this.tenant.name,
              };
              _this.tenantName = _this.tenant.name;
              _this.isModalVisible = false;
            } else {
              _this.toasterService.error('AbpUiMultiTenancy::GivenTenantIsNotAvailable', 'AbpUi::Error', {
                messageLocalizationParams: [_this.tenant.name],
              });
              _this.tenant = /** @type {?} */ ({});
            }
            _this.store.dispatch(new SetTenant(success ? _this.tenant : null));
          },
        );
    } else {
      this.store.dispatch(new SetTenant(null));
      this.tenantName = null;
      this.isModalVisible = false;
    }
  };
  TenantBoxComponent.decorators = [
    {
      type: Component,
      args: [
        {
          selector: 'abp-tenant-box',
          template:
            '<div\n  class="tenant-switch-box"\n  style="background-color: #eee; margin-bottom: 20px; color: #000; padding: 10px; text-align: center;"\n>\n  <span style="color: #666;">{{ \'AbpUiMultiTenancy::Tenant\' | abpLocalization }}: </span>\n  <strong>\n    <i>{{ tenantName || (\'AbpUiMultiTenancy::NotSelected\' | abpLocalization) }}</i>\n  </strong>\n  (<a id="abp-tenant-switch-link" style="color: #333; cursor: pointer" (click)="onSwitch()">{{\n    \'AbpUiMultiTenancy::Switch\' | abpLocalization\n  }}</a\n  >)\n</div>\n\n<abp-modal [(visible)]="isModalVisible" size="md">\n  <ng-template #abpHeader>\n    <h5>Switch Tenant</h5>\n  </ng-template>\n  <ng-template #abpBody>\n    <form (ngSubmit)="save()">\n      <div class="mt-2">\n        <div class="form-group">\n          <label for="name">{{ \'AbpUiMultiTenancy::Name\' | abpLocalization }}</label>\n          <input [(ngModel)]="tenant.name" type="text" id="name" name="tenant" class="form-control" autofocus />\n        </div>\n        <p>{{ \'AbpUiMultiTenancy::SwitchTenantHint\' | abpLocalization }}</p>\n      </div>\n    </form>\n  </ng-template>\n  <ng-template #abpFooter>\n    <button #abpClose type="button" class="btn btn-secondary">\n      {{ \'AbpTenantManagement::Cancel\' | abpLocalization }}\n    </button>\n    <button type="button" class="btn btn-primary" (click)="save()">\n      <i class="fa fa-check mr-1"></i> <span>{{ \'AbpTenantManagement::Save\' | abpLocalization }}</span>\n    </button>\n  </ng-template>\n</abp-modal>\n',
        },
      ],
    },
  ];
  /** @nocollapse */
  TenantBoxComponent.ctorParameters = function() {
    return [{ type: Store }, { type: ToasterService }, { type: AccountService }];
  };
  return TenantBoxComponent;
})();
export { TenantBoxComponent };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LWJveC5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmFjY291bnQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy90ZW5hbnQtYm94L3RlbmFudC1ib3guY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQU8sU0FBUyxFQUFFLFlBQVksRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUM1RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdEQsT0FBTyxFQUFFLFNBQVMsRUFBVSxNQUFNLGVBQWUsQ0FBQztBQUNsRCxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDbEMsT0FBTyxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNsRCxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFDdEIsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLGdDQUFnQyxDQUFDO0FBRWhFO0lBS0UsNEJBQ1UsS0FBWSxFQUNaLGNBQThCLEVBQzlCLGNBQThCO1FBRjlCLFVBQUssR0FBTCxLQUFLLENBQU87UUFDWixtQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7UUFDOUIsbUJBQWMsR0FBZCxjQUFjLENBQWdCO1FBR3hDLFdBQU0sR0FBRyxtQkFBQSxFQUFFLEVBQWlCLENBQUM7SUFGMUIsQ0FBQzs7OztJQVFKLHFDQUFROzs7SUFBUjtRQUNFLElBQUksQ0FBQyxNQUFNO1lBQ1QsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsWUFBWSxDQUFDLFNBQVMsQ0FBQztnQkFDakQsQ0FBQyxtQkFBQSxFQUFFLEVBQWlCLENBQUMsQ0FBQztRQUN4QixJQUFJLENBQUMsVUFBVSxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxJQUFJLEVBQUUsQ0FBQztJQUMzQyxDQUFDOzs7O0lBRUQscUNBQVE7OztJQUFSO1FBQ0UsSUFBSSxDQUFDLGNBQWMsR0FBRyxJQUFJLENBQUM7SUFDN0IsQ0FBQzs7OztJQUVELGlDQUFJOzs7SUFBSjtRQUFBLGlCQTBDQztRQXpDQyxJQUFJLElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxFQUFFO1lBQ3BCLElBQUksQ0FBQyxjQUFjO2lCQUNoQixVQUFVLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUM7aUJBQzVCLElBQUksQ0FDSCxJQUFJLENBQUMsQ0FBQyxDQUFDLEVBQ1AsVUFBVTs7OztZQUFDLFVBQUEsR0FBRztnQkFDWixLQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FDdkIsR0FBRzs7O2dCQUNELGNBQU0sT0FBQSxHQUFHLENBQUMsS0FBSyxDQUFDLGlCQUFpQixFQUEzQixDQUEyQixHQUNqQyw0QkFBNEIsQ0FDN0IsRUFDRCxjQUFjLENBQ2YsQ0FBQztnQkFDRixPQUFPLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQztZQUN6QixDQUFDLEVBQUMsQ0FDSDtpQkFDQSxTQUFTOzs7O1lBQUMsVUFBQyxFQUFxQjtvQkFBbkIsb0JBQU8sRUFBRSxzQkFBUTtnQkFDN0IsSUFBSSxPQUFPLEVBQUU7b0JBQ1gsS0FBSSxDQUFDLE1BQU0sR0FBRzt3QkFDWixFQUFFLEVBQUUsUUFBUTt3QkFDWixJQUFJLEVBQUUsS0FBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJO3FCQUN2QixDQUFDO29CQUNGLEtBQUksQ0FBQyxVQUFVLEdBQUcsS0FBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUM7b0JBQ25DLEtBQUksQ0FBQyxjQUFjLEdBQUcsS0FBSyxDQUFDO2lCQUM3QjtxQkFBTTtvQkFDTCxLQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FDdkIsOENBQThDLEVBQzlDLGNBQWMsRUFDZDt3QkFDRSx5QkFBeUIsRUFBRSxDQUFDLEtBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxDQUFDO3FCQUM5QyxDQUNGLENBQUM7b0JBQ0YsS0FBSSxDQUFDLE1BQU0sR0FBRyxtQkFBQSxFQUFFLEVBQWlCLENBQUM7aUJBQ25DO2dCQUNELEtBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksU0FBUyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsS0FBSSxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQztZQUNuRSxDQUFDLEVBQUMsQ0FBQztTQUNOO2FBQU07WUFDTCxJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLFNBQVMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDO1lBQ3pDLElBQUksQ0FBQyxVQUFVLEdBQUcsSUFBSSxDQUFDO1lBQ3ZCLElBQUksQ0FBQyxjQUFjLEdBQUcsS0FBSyxDQUFDO1NBQzdCO0lBQ0gsQ0FBQzs7Z0JBdEVGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsZ0JBQWdCO29CQUMxQiwwZ0RBQTBDO2lCQUMzQzs7OztnQkFUUSxLQUFLO2dCQUZMLGNBQWM7Z0JBTWQsY0FBYzs7SUF5RXZCLHlCQUFDO0NBQUEsQUF2RUQsSUF1RUM7U0FuRVksa0JBQWtCOzs7SUFPN0Isb0NBQTZCOztJQUU3Qix3Q0FBbUI7O0lBRW5CLDRDQUF3Qjs7Ozs7SUFUdEIsbUNBQW9COzs7OztJQUNwQiw0Q0FBc0M7Ozs7O0lBQ3RDLDRDQUFzQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCwgU2V0VGVuYW50LCBTZXNzaW9uU3RhdGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgVG9hc3RlclNlcnZpY2UgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBDb21wb25lbnQsIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyB0aHJvd0Vycm9yIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBjYXRjaEVycm9yLCB0YWtlIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuaW1wb3J0IHsgQWNjb3VudFNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcy9hY2NvdW50LnNlcnZpY2UnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtdGVuYW50LWJveCcsXG4gIHRlbXBsYXRlVXJsOiAnLi90ZW5hbnQtYm94LmNvbXBvbmVudC5odG1sJ1xufSlcbmV4cG9ydCBjbGFzcyBUZW5hbnRCb3hDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBjb25zdHJ1Y3RvcihcbiAgICBwcml2YXRlIHN0b3JlOiBTdG9yZSxcbiAgICBwcml2YXRlIHRvYXN0ZXJTZXJ2aWNlOiBUb2FzdGVyU2VydmljZSxcbiAgICBwcml2YXRlIGFjY291bnRTZXJ2aWNlOiBBY2NvdW50U2VydmljZVxuICApIHt9XG5cbiAgdGVuYW50ID0ge30gYXMgQUJQLkJhc2ljSXRlbTtcblxuICB0ZW5hbnROYW1lOiBzdHJpbmc7XG5cbiAgaXNNb2RhbFZpc2libGU6IGJvb2xlYW47XG5cbiAgbmdPbkluaXQoKSB7XG4gICAgdGhpcy50ZW5hbnQgPVxuICAgICAgdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXNzaW9uU3RhdGUuZ2V0VGVuYW50KSB8fFxuICAgICAgKHt9IGFzIEFCUC5CYXNpY0l0ZW0pO1xuICAgIHRoaXMudGVuYW50TmFtZSA9IHRoaXMudGVuYW50Lm5hbWUgfHwgJyc7XG4gIH1cblxuICBvblN3aXRjaCgpIHtcbiAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gdHJ1ZTtcbiAgfVxuXG4gIHNhdmUoKSB7XG4gICAgaWYgKHRoaXMudGVuYW50Lm5hbWUpIHtcbiAgICAgIHRoaXMuYWNjb3VudFNlcnZpY2VcbiAgICAgICAgLmZpbmRUZW5hbnQodGhpcy50ZW5hbnQubmFtZSlcbiAgICAgICAgLnBpcGUoXG4gICAgICAgICAgdGFrZSgxKSxcbiAgICAgICAgICBjYXRjaEVycm9yKGVyciA9PiB7XG4gICAgICAgICAgICB0aGlzLnRvYXN0ZXJTZXJ2aWNlLmVycm9yKFxuICAgICAgICAgICAgICBzbnEoXG4gICAgICAgICAgICAgICAgKCkgPT4gZXJyLmVycm9yLmVycm9yX2Rlc2NyaXB0aW9uLFxuICAgICAgICAgICAgICAgICdBYnBVaTo6RGVmYXVsdEVycm9yTWVzc2FnZSdcbiAgICAgICAgICAgICAgKSxcbiAgICAgICAgICAgICAgJ0FicFVpOjpFcnJvcidcbiAgICAgICAgICAgICk7XG4gICAgICAgICAgICByZXR1cm4gdGhyb3dFcnJvcihlcnIpO1xuICAgICAgICAgIH0pXG4gICAgICAgIClcbiAgICAgICAgLnN1YnNjcmliZSgoeyBzdWNjZXNzLCB0ZW5hbnRJZCB9KSA9PiB7XG4gICAgICAgICAgaWYgKHN1Y2Nlc3MpIHtcbiAgICAgICAgICAgIHRoaXMudGVuYW50ID0ge1xuICAgICAgICAgICAgICBpZDogdGVuYW50SWQsXG4gICAgICAgICAgICAgIG5hbWU6IHRoaXMudGVuYW50Lm5hbWVcbiAgICAgICAgICAgIH07XG4gICAgICAgICAgICB0aGlzLnRlbmFudE5hbWUgPSB0aGlzLnRlbmFudC5uYW1lO1xuICAgICAgICAgICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IGZhbHNlO1xuICAgICAgICAgIH0gZWxzZSB7XG4gICAgICAgICAgICB0aGlzLnRvYXN0ZXJTZXJ2aWNlLmVycm9yKFxuICAgICAgICAgICAgICAnQWJwVWlNdWx0aVRlbmFuY3k6OkdpdmVuVGVuYW50SXNOb3RBdmFpbGFibGUnLFxuICAgICAgICAgICAgICAnQWJwVWk6OkVycm9yJyxcbiAgICAgICAgICAgICAge1xuICAgICAgICAgICAgICAgIG1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXM6IFt0aGlzLnRlbmFudC5uYW1lXVxuICAgICAgICAgICAgICB9XG4gICAgICAgICAgICApO1xuICAgICAgICAgICAgdGhpcy50ZW5hbnQgPSB7fSBhcyBBQlAuQmFzaWNJdGVtO1xuICAgICAgICAgIH1cbiAgICAgICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBTZXRUZW5hbnQoc3VjY2VzcyA/IHRoaXMudGVuYW50IDogbnVsbCkpO1xuICAgICAgICB9KTtcbiAgICB9IGVsc2Uge1xuICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgU2V0VGVuYW50KG51bGwpKTtcbiAgICAgIHRoaXMudGVuYW50TmFtZSA9IG51bGw7XG4gICAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gZmFsc2U7XG4gICAgfVxuICB9XG59XG4iXX0=
