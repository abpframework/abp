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
        this.tenant = this.store.selectSnapshot(SessionState.getTenant) || ((/** @type {?} */ ({})));
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
                        name: _this.tenant.name,
                    };
                    _this.tenantName = _this.tenant.name;
                    _this.isModalVisible = false;
                }
                else {
                    _this.toasterService.error("AbpUiMultiTenancy::GivenTenantIsNotAvailable", 'AbpUi::Error', {
                        messageLocalizationParams: [_this.tenant.name],
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
                    template: "<div\n  class=\"tenant-switch-box\"\n  style=\"background-color: #eee; margin-bottom: 20px; color: #000; padding: 10px; text-align: center;\"\n>\n  <span style=\"color: #666;\">{{ 'AbpUiMultiTenancy::Tenant' | abpLocalization }}: </span>\n  <strong>\n    <i>{{ tenantName || ('AbpUiMultiTenancy::NotSelected' | abpLocalization) }}</i>\n  </strong>\n  (<a id=\"abp-tenant-switch-link\" style=\"color: #333; cursor: pointer\" (click)=\"onSwitch()\">{{\n    'AbpUiMultiTenancy::Switch' | abpLocalization\n  }}</a\n  >)\n</div>\n\n<abp-modal [(visible)]=\"isModalVisible\" size=\"md\">\n  <ng-template #abpHeader>\n    <h5>Switch Tenant</h5>\n  </ng-template>\n  <ng-template #abpBody>\n    <form (ngSubmit)=\"save()\">\n      <div class=\"mt-2\">\n        <div class=\"form-group\">\n          <label for=\"name\">{{ 'AbpUiMultiTenancy::Name' | abpLocalization }}</label>\n          <input [(ngModel)]=\"tenant.name\" type=\"text\" id=\"name\" name=\"tenant\" class=\"form-control\" autofocus />\n        </div>\n        <p>{{ 'AbpUiMultiTenancy::SwitchTenantHint' | abpLocalization }}</p>\n      </div>\n    </form>\n  </ng-template>\n  <ng-template #abpFooter>\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n    </button>\n    <button type=\"button\" class=\"btn btn-primary\" (click)=\"save()\">\n      <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpTenantManagement::Save' | abpLocalization }}</span>\n    </button>\n  </ng-template>\n</abp-modal>\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LWJveC5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmFjY291bnQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy90ZW5hbnQtYm94L3RlbmFudC1ib3guY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQU8sU0FBUyxFQUFFLFlBQVksRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUM1RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdEQsT0FBTyxFQUFFLFNBQVMsRUFBVSxNQUFNLGVBQWUsQ0FBQztBQUNsRCxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDbEMsT0FBTyxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNsRCxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFDdEIsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLGdDQUFnQyxDQUFDO0FBRWhFO0lBS0UsNEJBQW9CLEtBQVksRUFBVSxjQUE4QixFQUFVLGNBQThCO1FBQTVGLFVBQUssR0FBTCxLQUFLLENBQU87UUFBVSxtQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7UUFBVSxtQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7UUFFaEgsV0FBTSxHQUFHLG1CQUFBLEVBQUUsRUFBaUIsQ0FBQztJQUZzRixDQUFDOzs7O0lBUXBILHFDQUFROzs7SUFBUjtRQUNFLElBQUksQ0FBQyxNQUFNLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsWUFBWSxDQUFDLFNBQVMsQ0FBQyxJQUFJLENBQUMsbUJBQUEsRUFBRSxFQUFpQixDQUFDLENBQUM7UUFDekYsSUFBSSxDQUFDLFVBQVUsR0FBRyxJQUFJLENBQUMsTUFBTSxDQUFDLElBQUksSUFBSSxFQUFFLENBQUM7SUFDM0MsQ0FBQzs7OztJQUVELHFDQUFROzs7SUFBUjtRQUNFLElBQUksQ0FBQyxjQUFjLEdBQUcsSUFBSSxDQUFDO0lBQzdCLENBQUM7Ozs7SUFFRCxpQ0FBSTs7O0lBQUo7UUFBQSxpQkFtQ0M7UUFsQ0MsSUFBSSxJQUFJLENBQUMsTUFBTSxDQUFDLElBQUksRUFBRTtZQUNwQixJQUFJLENBQUMsY0FBYztpQkFDaEIsVUFBVSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxDQUFDO2lCQUM1QixJQUFJLENBQ0gsSUFBSSxDQUFDLENBQUMsQ0FBQyxFQUNQLFVBQVU7Ozs7WUFBQyxVQUFBLEdBQUc7Z0JBQ1osS0FBSSxDQUFDLGNBQWMsQ0FBQyxLQUFLLENBQ3ZCLEdBQUc7OztnQkFBQyxjQUFNLE9BQUEsR0FBRyxDQUFDLEtBQUssQ0FBQyxpQkFBaUIsRUFBM0IsQ0FBMkIsR0FBRSw0QkFBNEIsQ0FBQyxFQUNwRSxjQUFjLENBQ2YsQ0FBQztnQkFDRixPQUFPLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQztZQUN6QixDQUFDLEVBQUMsQ0FDSDtpQkFDQSxTQUFTOzs7O1lBQUMsVUFBQyxFQUFxQjtvQkFBbkIsb0JBQU8sRUFBRSxzQkFBUTtnQkFDN0IsSUFBSSxPQUFPLEVBQUU7b0JBQ1gsS0FBSSxDQUFDLE1BQU0sR0FBRzt3QkFDWixFQUFFLEVBQUUsUUFBUTt3QkFDWixJQUFJLEVBQUUsS0FBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJO3FCQUN2QixDQUFDO29CQUNGLEtBQUksQ0FBQyxVQUFVLEdBQUcsS0FBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUM7b0JBQ25DLEtBQUksQ0FBQyxjQUFjLEdBQUcsS0FBSyxDQUFDO2lCQUM3QjtxQkFBTTtvQkFDTCxLQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FBQyw4Q0FBOEMsRUFBRSxjQUFjLEVBQUU7d0JBQ3hGLHlCQUF5QixFQUFFLENBQUMsS0FBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUM7cUJBQzlDLENBQUMsQ0FBQztvQkFDSCxLQUFJLENBQUMsTUFBTSxHQUFHLG1CQUFBLEVBQUUsRUFBaUIsQ0FBQztpQkFDbkM7Z0JBQ0QsS0FBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxTQUFTLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxLQUFJLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDO1lBQ25FLENBQUMsRUFBQyxDQUFDO1NBQ047YUFBTTtZQUNMLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksU0FBUyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUM7WUFDekMsSUFBSSxDQUFDLFVBQVUsR0FBRyxJQUFJLENBQUM7WUFDdkIsSUFBSSxDQUFDLGNBQWMsR0FBRyxLQUFLLENBQUM7U0FDN0I7SUFDSCxDQUFDOztnQkF6REYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxnQkFBZ0I7b0JBQzFCLDBnREFBMEM7aUJBQzNDOzs7O2dCQVRRLEtBQUs7Z0JBRkwsY0FBYztnQkFNZCxjQUFjOztJQTREdkIseUJBQUM7Q0FBQSxBQTFERCxJQTBEQztTQXREWSxrQkFBa0I7OztJQUc3QixvQ0FBNkI7O0lBRTdCLHdDQUFtQjs7SUFFbkIsNENBQXdCOzs7OztJQU5aLG1DQUFvQjs7Ozs7SUFBRSw0Q0FBc0M7Ozs7O0lBQUUsNENBQXNDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQUJQLCBTZXRUZW5hbnQsIFNlc3Npb25TdGF0ZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBUb2FzdGVyU2VydmljZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IENvbXBvbmVudCwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IHRocm93RXJyb3IgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGNhdGNoRXJyb3IsIHRha2UgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQgeyBBY2NvdW50U2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzL2FjY291bnQuc2VydmljZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC10ZW5hbnQtYm94JyxcbiAgdGVtcGxhdGVVcmw6ICcuL3RlbmFudC1ib3guY29tcG9uZW50Lmh0bWwnLFxufSlcbmV4cG9ydCBjbGFzcyBUZW5hbnRCb3hDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSwgcHJpdmF0ZSB0b2FzdGVyU2VydmljZTogVG9hc3RlclNlcnZpY2UsIHByaXZhdGUgYWNjb3VudFNlcnZpY2U6IEFjY291bnRTZXJ2aWNlKSB7fVxuXG4gIHRlbmFudCA9IHt9IGFzIEFCUC5CYXNpY0l0ZW07XG5cbiAgdGVuYW50TmFtZTogc3RyaW5nO1xuXG4gIGlzTW9kYWxWaXNpYmxlOiBib29sZWFuO1xuXG4gIG5nT25Jbml0KCkge1xuICAgIHRoaXMudGVuYW50ID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXNzaW9uU3RhdGUuZ2V0VGVuYW50KSB8fCAoe30gYXMgQUJQLkJhc2ljSXRlbSk7XG4gICAgdGhpcy50ZW5hbnROYW1lID0gdGhpcy50ZW5hbnQubmFtZSB8fCAnJztcbiAgfVxuXG4gIG9uU3dpdGNoKCkge1xuICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSB0cnVlO1xuICB9XG5cbiAgc2F2ZSgpIHtcbiAgICBpZiAodGhpcy50ZW5hbnQubmFtZSkge1xuICAgICAgdGhpcy5hY2NvdW50U2VydmljZVxuICAgICAgICAuZmluZFRlbmFudCh0aGlzLnRlbmFudC5uYW1lKVxuICAgICAgICAucGlwZShcbiAgICAgICAgICB0YWtlKDEpLFxuICAgICAgICAgIGNhdGNoRXJyb3IoZXJyID0+IHtcbiAgICAgICAgICAgIHRoaXMudG9hc3RlclNlcnZpY2UuZXJyb3IoXG4gICAgICAgICAgICAgIHNucSgoKSA9PiBlcnIuZXJyb3IuZXJyb3JfZGVzY3JpcHRpb24sICdBYnBVaTo6RGVmYXVsdEVycm9yTWVzc2FnZScpLFxuICAgICAgICAgICAgICAnQWJwVWk6OkVycm9yJyxcbiAgICAgICAgICAgICk7XG4gICAgICAgICAgICByZXR1cm4gdGhyb3dFcnJvcihlcnIpO1xuICAgICAgICAgIH0pLFxuICAgICAgICApXG4gICAgICAgIC5zdWJzY3JpYmUoKHsgc3VjY2VzcywgdGVuYW50SWQgfSkgPT4ge1xuICAgICAgICAgIGlmIChzdWNjZXNzKSB7XG4gICAgICAgICAgICB0aGlzLnRlbmFudCA9IHtcbiAgICAgICAgICAgICAgaWQ6IHRlbmFudElkLFxuICAgICAgICAgICAgICBuYW1lOiB0aGlzLnRlbmFudC5uYW1lLFxuICAgICAgICAgICAgfTtcbiAgICAgICAgICAgIHRoaXMudGVuYW50TmFtZSA9IHRoaXMudGVuYW50Lm5hbWU7XG4gICAgICAgICAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gZmFsc2U7XG4gICAgICAgICAgfSBlbHNlIHtcbiAgICAgICAgICAgIHRoaXMudG9hc3RlclNlcnZpY2UuZXJyb3IoYEFicFVpTXVsdGlUZW5hbmN5OjpHaXZlblRlbmFudElzTm90QXZhaWxhYmxlYCwgJ0FicFVpOjpFcnJvcicsIHtcbiAgICAgICAgICAgICAgbWVzc2FnZUxvY2FsaXphdGlvblBhcmFtczogW3RoaXMudGVuYW50Lm5hbWVdLFxuICAgICAgICAgICAgfSk7XG4gICAgICAgICAgICB0aGlzLnRlbmFudCA9IHt9IGFzIEFCUC5CYXNpY0l0ZW07XG4gICAgICAgICAgfVxuICAgICAgICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFNldFRlbmFudChzdWNjZXNzID8gdGhpcy50ZW5hbnQgOiBudWxsKSk7XG4gICAgICAgIH0pO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBTZXRUZW5hbnQobnVsbCkpO1xuICAgICAgdGhpcy50ZW5hbnROYW1lID0gbnVsbDtcbiAgICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSBmYWxzZTtcbiAgICB9XG4gIH1cbn1cbiJdfQ==