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
var TenantBoxComponent = /** @class */ (function () {
    function TenantBoxComponent(store, toasterService, accountService) {
        this.store = store;
        this.toasterService = toasterService;
        this.accountService = accountService;
        this.tenant = (/** @type {?} */ ({}));
    }
    /**
     * @return {?}
     */
    TenantBoxComponent.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        this.tenant =
            this.store.selectSnapshot(SessionState.getTenant) ||
                ((/** @type {?} */ ({})));
        this.tenantName = this.tenant.name || '';
    };
    /**
     * @return {?}
     */
    TenantBoxComponent.prototype.onSwitch = /**
     * @return {?}
     */
    function () {
        this.isModalVisible = true;
    };
    /**
     * @return {?}
     */
    TenantBoxComponent.prototype.save = /**
     * @return {?}
     */
    function () {
        var _this = this;
        if (this.tenant.name) {
            this.accountService
                .findTenant(this.tenant.name)
                .pipe(take(1), catchError((/**
             * @param {?} err
             * @return {?}
             */
            function (err) {
                _this.toasterService.error(snq((/**
                 * @return {?}
                 */
                function () { return err.error.error_description; }), 'AbpUi::DefaultErrorMessage'), 'AbpUi::Error');
                return throwError(err);
            })))
                .subscribe((/**
             * @param {?} __0
             * @return {?}
             */
            function (_a) {
                var success = _a.success, tenantId = _a.tenantId;
                if (success) {
                    _this.tenant = {
                        id: tenantId,
                        name: _this.tenant.name
                    };
                    _this.tenantName = _this.tenant.name;
                    _this.isModalVisible = false;
                }
                else {
                    _this.toasterService.error('AbpUiMultiTenancy::GivenTenantIsNotAvailable', 'AbpUi::Error', {
                        messageLocalizationParams: [_this.tenant.name]
                    });
                    _this.tenant = (/** @type {?} */ ({}));
                }
                _this.store.dispatch(new SetTenant(success ? _this.tenant : null));
            }));
        }
        else {
            this.store.dispatch(new SetTenant(null));
            this.tenantName = null;
            this.isModalVisible = false;
        }
    };
    TenantBoxComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-tenant-box',
                    template: "<div\r\n  class=\"tenant-switch-box\"\r\n  style=\"background-color: #eee; margin-bottom: 20px; color: #000; padding: 10px; text-align: center;\"\r\n>\r\n  <span style=\"color: #666;\">{{ 'AbpUiMultiTenancy::Tenant' | abpLocalization }}: </span>\r\n  <strong>\r\n    <i>{{ tenantName || ('AbpUiMultiTenancy::NotSelected' | abpLocalization) }}</i>\r\n  </strong>\r\n  (<a id=\"abp-tenant-switch-link\" style=\"color: #333; cursor: pointer\" (click)=\"onSwitch()\">{{\r\n    'AbpUiMultiTenancy::Switch' | abpLocalization\r\n  }}</a\r\n  >)\r\n</div>\r\n\r\n<abp-modal [(visible)]=\"isModalVisible\" size=\"md\">\r\n  <ng-template #abpHeader>\r\n    <h5>Switch Tenant</h5>\r\n  </ng-template>\r\n  <ng-template #abpBody>\r\n    <form (ngSubmit)=\"save()\">\r\n      <div class=\"mt-2\">\r\n        <div class=\"form-group\">\r\n          <label for=\"name\">{{ 'AbpUiMultiTenancy::Name' | abpLocalization }}</label>\r\n          <input [(ngModel)]=\"tenant.name\" type=\"text\" id=\"name\" name=\"tenant\" class=\"form-control\" autofocus />\r\n        </div>\r\n        <p>{{ 'AbpUiMultiTenancy::SwitchTenantHint' | abpLocalization }}</p>\r\n      </div>\r\n    </form>\r\n  </ng-template>\r\n  <ng-template #abpFooter>\r\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\r\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\r\n    </button>\r\n    <button type=\"button\" class=\"btn btn-primary\" (click)=\"save()\">\r\n      <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpTenantManagement::Save' | abpLocalization }}</span>\r\n    </button>\r\n  </ng-template>\r\n</abp-modal>\r\n"
                }] }
    ];
    /** @nocollapse */
    TenantBoxComponent.ctorParameters = function () { return [
        { type: Store },
        { type: ToasterService },
        { type: AccountService }
    ]; };
    return TenantBoxComponent;
}());
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LWJveC5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmFjY291bnQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy90ZW5hbnQtYm94L3RlbmFudC1ib3guY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQU8sU0FBUyxFQUFFLFlBQVksRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUM1RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdEQsT0FBTyxFQUFFLFNBQVMsRUFBVSxNQUFNLGVBQWUsQ0FBQztBQUNsRCxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDbEMsT0FBTyxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNsRCxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFDdEIsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLGdDQUFnQyxDQUFDO0FBRWhFO0lBS0UsNEJBQ1UsS0FBWSxFQUNaLGNBQThCLEVBQzlCLGNBQThCO1FBRjlCLFVBQUssR0FBTCxLQUFLLENBQU87UUFDWixtQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7UUFDOUIsbUJBQWMsR0FBZCxjQUFjLENBQWdCO1FBR3hDLFdBQU0sR0FBRyxtQkFBQSxFQUFFLEVBQWlCLENBQUM7SUFGMUIsQ0FBQzs7OztJQVFKLHFDQUFROzs7SUFBUjtRQUNFLElBQUksQ0FBQyxNQUFNO1lBQ1QsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsWUFBWSxDQUFDLFNBQVMsQ0FBQztnQkFDakQsQ0FBQyxtQkFBQSxFQUFFLEVBQWlCLENBQUMsQ0FBQztRQUN4QixJQUFJLENBQUMsVUFBVSxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxJQUFJLEVBQUUsQ0FBQztJQUMzQyxDQUFDOzs7O0lBRUQscUNBQVE7OztJQUFSO1FBQ0UsSUFBSSxDQUFDLGNBQWMsR0FBRyxJQUFJLENBQUM7SUFDN0IsQ0FBQzs7OztJQUVELGlDQUFJOzs7SUFBSjtRQUFBLGlCQTBDQztRQXpDQyxJQUFJLElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxFQUFFO1lBQ3BCLElBQUksQ0FBQyxjQUFjO2lCQUNoQixVQUFVLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUM7aUJBQzVCLElBQUksQ0FDSCxJQUFJLENBQUMsQ0FBQyxDQUFDLEVBQ1AsVUFBVTs7OztZQUFDLFVBQUEsR0FBRztnQkFDWixLQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FDdkIsR0FBRzs7O2dCQUNELGNBQU0sT0FBQSxHQUFHLENBQUMsS0FBSyxDQUFDLGlCQUFpQixFQUEzQixDQUEyQixHQUNqQyw0QkFBNEIsQ0FDN0IsRUFDRCxjQUFjLENBQ2YsQ0FBQztnQkFDRixPQUFPLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQztZQUN6QixDQUFDLEVBQUMsQ0FDSDtpQkFDQSxTQUFTOzs7O1lBQUMsVUFBQyxFQUFxQjtvQkFBbkIsb0JBQU8sRUFBRSxzQkFBUTtnQkFDN0IsSUFBSSxPQUFPLEVBQUU7b0JBQ1gsS0FBSSxDQUFDLE1BQU0sR0FBRzt3QkFDWixFQUFFLEVBQUUsUUFBUTt3QkFDWixJQUFJLEVBQUUsS0FBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJO3FCQUN2QixDQUFDO29CQUNGLEtBQUksQ0FBQyxVQUFVLEdBQUcsS0FBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUM7b0JBQ25DLEtBQUksQ0FBQyxjQUFjLEdBQUcsS0FBSyxDQUFDO2lCQUM3QjtxQkFBTTtvQkFDTCxLQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FDdkIsOENBQThDLEVBQzlDLGNBQWMsRUFDZDt3QkFDRSx5QkFBeUIsRUFBRSxDQUFDLEtBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxDQUFDO3FCQUM5QyxDQUNGLENBQUM7b0JBQ0YsS0FBSSxDQUFDLE1BQU0sR0FBRyxtQkFBQSxFQUFFLEVBQWlCLENBQUM7aUJBQ25DO2dCQUNELEtBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksU0FBUyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsS0FBSSxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQztZQUNuRSxDQUFDLEVBQUMsQ0FBQztTQUNOO2FBQU07WUFDTCxJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLFNBQVMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDO1lBQ3pDLElBQUksQ0FBQyxVQUFVLEdBQUcsSUFBSSxDQUFDO1lBQ3ZCLElBQUksQ0FBQyxjQUFjLEdBQUcsS0FBSyxDQUFDO1NBQzdCO0lBQ0gsQ0FBQzs7Z0JBdEVGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsZ0JBQWdCO29CQUMxQixzbERBQTBDO2lCQUMzQzs7OztnQkFUUSxLQUFLO2dCQUZMLGNBQWM7Z0JBTWQsY0FBYzs7SUF5RXZCLHlCQUFDO0NBQUEsQUF2RUQsSUF1RUM7U0FuRVksa0JBQWtCOzs7SUFPN0Isb0NBQTZCOztJQUU3Qix3Q0FBbUI7O0lBRW5CLDRDQUF3Qjs7Ozs7SUFUdEIsbUNBQW9COzs7OztJQUNwQiw0Q0FBc0M7Ozs7O0lBQ3RDLDRDQUFzQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCwgU2V0VGVuYW50LCBTZXNzaW9uU3RhdGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5pbXBvcnQgeyBUb2FzdGVyU2VydmljZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcclxuaW1wb3J0IHsgQ29tcG9uZW50LCBPbkluaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IHRocm93RXJyb3IgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgY2F0Y2hFcnJvciwgdGFrZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcclxuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xyXG5pbXBvcnQgeyBBY2NvdW50U2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzL2FjY291bnQuc2VydmljZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC10ZW5hbnQtYm94JyxcclxuICB0ZW1wbGF0ZVVybDogJy4vdGVuYW50LWJveC5jb21wb25lbnQuaHRtbCdcclxufSlcclxuZXhwb3J0IGNsYXNzIFRlbmFudEJveENvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XHJcbiAgY29uc3RydWN0b3IoXHJcbiAgICBwcml2YXRlIHN0b3JlOiBTdG9yZSxcclxuICAgIHByaXZhdGUgdG9hc3RlclNlcnZpY2U6IFRvYXN0ZXJTZXJ2aWNlLFxyXG4gICAgcHJpdmF0ZSBhY2NvdW50U2VydmljZTogQWNjb3VudFNlcnZpY2VcclxuICApIHt9XHJcblxyXG4gIHRlbmFudCA9IHt9IGFzIEFCUC5CYXNpY0l0ZW07XHJcblxyXG4gIHRlbmFudE5hbWU6IHN0cmluZztcclxuXHJcbiAgaXNNb2RhbFZpc2libGU6IGJvb2xlYW47XHJcblxyXG4gIG5nT25Jbml0KCkge1xyXG4gICAgdGhpcy50ZW5hbnQgPVxyXG4gICAgICB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFNlc3Npb25TdGF0ZS5nZXRUZW5hbnQpIHx8XHJcbiAgICAgICh7fSBhcyBBQlAuQmFzaWNJdGVtKTtcclxuICAgIHRoaXMudGVuYW50TmFtZSA9IHRoaXMudGVuYW50Lm5hbWUgfHwgJyc7XHJcbiAgfVxyXG5cclxuICBvblN3aXRjaCgpIHtcclxuICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSB0cnVlO1xyXG4gIH1cclxuXHJcbiAgc2F2ZSgpIHtcclxuICAgIGlmICh0aGlzLnRlbmFudC5uYW1lKSB7XHJcbiAgICAgIHRoaXMuYWNjb3VudFNlcnZpY2VcclxuICAgICAgICAuZmluZFRlbmFudCh0aGlzLnRlbmFudC5uYW1lKVxyXG4gICAgICAgIC5waXBlKFxyXG4gICAgICAgICAgdGFrZSgxKSxcclxuICAgICAgICAgIGNhdGNoRXJyb3IoZXJyID0+IHtcclxuICAgICAgICAgICAgdGhpcy50b2FzdGVyU2VydmljZS5lcnJvcihcclxuICAgICAgICAgICAgICBzbnEoXHJcbiAgICAgICAgICAgICAgICAoKSA9PiBlcnIuZXJyb3IuZXJyb3JfZGVzY3JpcHRpb24sXHJcbiAgICAgICAgICAgICAgICAnQWJwVWk6OkRlZmF1bHRFcnJvck1lc3NhZ2UnXHJcbiAgICAgICAgICAgICAgKSxcclxuICAgICAgICAgICAgICAnQWJwVWk6OkVycm9yJ1xyXG4gICAgICAgICAgICApO1xyXG4gICAgICAgICAgICByZXR1cm4gdGhyb3dFcnJvcihlcnIpO1xyXG4gICAgICAgICAgfSlcclxuICAgICAgICApXHJcbiAgICAgICAgLnN1YnNjcmliZSgoeyBzdWNjZXNzLCB0ZW5hbnRJZCB9KSA9PiB7XHJcbiAgICAgICAgICBpZiAoc3VjY2Vzcykge1xyXG4gICAgICAgICAgICB0aGlzLnRlbmFudCA9IHtcclxuICAgICAgICAgICAgICBpZDogdGVuYW50SWQsXHJcbiAgICAgICAgICAgICAgbmFtZTogdGhpcy50ZW5hbnQubmFtZVxyXG4gICAgICAgICAgICB9O1xyXG4gICAgICAgICAgICB0aGlzLnRlbmFudE5hbWUgPSB0aGlzLnRlbmFudC5uYW1lO1xyXG4gICAgICAgICAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gZmFsc2U7XHJcbiAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICB0aGlzLnRvYXN0ZXJTZXJ2aWNlLmVycm9yKFxyXG4gICAgICAgICAgICAgICdBYnBVaU11bHRpVGVuYW5jeTo6R2l2ZW5UZW5hbnRJc05vdEF2YWlsYWJsZScsXHJcbiAgICAgICAgICAgICAgJ0FicFVpOjpFcnJvcicsXHJcbiAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgbWVzc2FnZUxvY2FsaXphdGlvblBhcmFtczogW3RoaXMudGVuYW50Lm5hbWVdXHJcbiAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICApO1xyXG4gICAgICAgICAgICB0aGlzLnRlbmFudCA9IHt9IGFzIEFCUC5CYXNpY0l0ZW07XHJcbiAgICAgICAgICB9XHJcbiAgICAgICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBTZXRUZW5hbnQoc3VjY2VzcyA/IHRoaXMudGVuYW50IDogbnVsbCkpO1xyXG4gICAgICAgIH0pO1xyXG4gICAgfSBlbHNlIHtcclxuICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgU2V0VGVuYW50KG51bGwpKTtcclxuICAgICAgdGhpcy50ZW5hbnROYW1lID0gbnVsbDtcclxuICAgICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IGZhbHNlO1xyXG4gICAgfVxyXG4gIH1cclxufVxyXG4iXX0=