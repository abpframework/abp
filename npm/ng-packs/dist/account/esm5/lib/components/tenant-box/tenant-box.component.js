/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/tenant-box/tenant-box.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { SetTenant, SessionState } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { throwError } from 'rxjs';
import { catchError, take, finalize } from 'rxjs/operators';
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
        if (this.tenant.name && !this.inProgress) {
            this.inProgress = true;
            this.accountService
                .findTenant(this.tenant.name)
                .pipe(finalize((/**
             * @return {?}
             */
            function () { return (_this.inProgress = false); })), take(1), catchError((/**
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
                    _this.toasterService.error('AbpUiMultiTenancy::GivenTenantIsNotAvailable', 'AbpUi::Error', {
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
                    template: "<div class=\"card shadow-sm rounded mb-3\">\r\n  <div class=\"card-body px-5\">\r\n    <div class=\"row\">\r\n      <div class=\"col\">\r\n        <span style=\"font-size: 0.8em;\" class=\"text-uppercase text-muted\">{{\r\n          'AbpUiMultiTenancy::Tenant' | abpLocalization\r\n        }}</span\r\n        ><br />\r\n        <h6 class=\"m-0 d-inline-block\">\r\n          <span>\r\n            {{ tenantName || ('AbpUiMultiTenancy::NotSelected' | abpLocalization) }}\r\n          </span>\r\n        </h6>\r\n      </div>\r\n      <div class=\"col-auto\">\r\n        <a\r\n          id=\"AbpTenantSwitchLink\"\r\n          href=\"javascript:void(0);\"\r\n          class=\"btn btn-sm mt-3 btn-outline-primary\"\r\n          (click)=\"onSwitch()\"\r\n          >{{ 'AbpUiMultiTenancy::Switch' | abpLocalization }}</a\r\n        >\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<abp-modal size=\"md\" [(visible)]=\"isModalVisible\" [busy]=\"inProgress\">\r\n  <ng-template #abpHeader>\r\n    <h5>Switch Tenant</h5>\r\n  </ng-template>\r\n  <ng-template #abpBody>\r\n    <form (ngSubmit)=\"save()\">\r\n      <div class=\"mt-2\">\r\n        <div class=\"form-group\">\r\n          <label for=\"name\">{{ 'AbpUiMultiTenancy::Name' | abpLocalization }}</label>\r\n          <input [(ngModel)]=\"tenant.name\" type=\"text\" id=\"name\" name=\"tenant\" class=\"form-control\" autofocus />\r\n        </div>\r\n        <p>{{ 'AbpUiMultiTenancy::SwitchTenantHint' | abpLocalization }}</p>\r\n      </div>\r\n    </form>\r\n  </ng-template>\r\n  <ng-template #abpFooter>\r\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\r\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\r\n    </button>\r\n    <abp-button buttonType=\"button\" buttonClass=\"btn btn-primary\" (click)=\"save()\">\r\n      <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpTenantManagement::Save' | abpLocalization }}</span>\r\n    </abp-button>\r\n  </ng-template>\r\n</abp-modal>\r\n"
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
    /** @type {?} */
    TenantBoxComponent.prototype.inProgress;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LWJveC5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmFjY291bnQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy90ZW5hbnQtYm94L3RlbmFudC1ib3guY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFPLFNBQVMsRUFBRSxZQUFZLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDNUQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3RELE9BQU8sRUFBRSxTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFDbEQsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2xDLE9BQU8sRUFBRSxVQUFVLEVBQUUsSUFBSSxFQUFFLFFBQVEsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQzVELE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUN0QixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sZ0NBQWdDLENBQUM7QUFFaEU7SUFhRSw0QkFBb0IsS0FBWSxFQUFVLGNBQThCLEVBQVUsY0FBOEI7UUFBNUYsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUFVLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtRQUFVLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtRQVJoSCxXQUFNLEdBQUcsbUJBQUEsRUFBRSxFQUFpQixDQUFDO0lBUXNGLENBQUM7Ozs7SUFFcEgscUNBQVE7OztJQUFSO1FBQ0UsSUFBSSxDQUFDLE1BQU0sR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxZQUFZLENBQUMsU0FBUyxDQUFDLElBQUksQ0FBQyxtQkFBQSxFQUFFLEVBQWlCLENBQUMsQ0FBQztRQUN6RixJQUFJLENBQUMsVUFBVSxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxJQUFJLEVBQUUsQ0FBQztJQUMzQyxDQUFDOzs7O0lBRUQscUNBQVE7OztJQUFSO1FBQ0UsSUFBSSxDQUFDLGNBQWMsR0FBRyxJQUFJLENBQUM7SUFDN0IsQ0FBQzs7OztJQUVELGlDQUFJOzs7SUFBSjtRQUFBLGlCQXFDQztRQXBDQyxJQUFJLElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxJQUFJLENBQUMsSUFBSSxDQUFDLFVBQVUsRUFBRTtZQUN4QyxJQUFJLENBQUMsVUFBVSxHQUFHLElBQUksQ0FBQztZQUN2QixJQUFJLENBQUMsY0FBYztpQkFDaEIsVUFBVSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxDQUFDO2lCQUM1QixJQUFJLENBQ0gsUUFBUTs7O1lBQUMsY0FBTSxPQUFBLENBQUMsS0FBSSxDQUFDLFVBQVUsR0FBRyxLQUFLLENBQUMsRUFBekIsQ0FBeUIsRUFBQyxFQUN6QyxJQUFJLENBQUMsQ0FBQyxDQUFDLEVBQ1AsVUFBVTs7OztZQUFDLFVBQUEsR0FBRztnQkFDWixLQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FDdkIsR0FBRzs7O2dCQUFDLGNBQU0sT0FBQSxHQUFHLENBQUMsS0FBSyxDQUFDLGlCQUFpQixFQUEzQixDQUEyQixHQUFFLDRCQUE0QixDQUFDLEVBQ3BFLGNBQWMsQ0FDZixDQUFDO2dCQUNGLE9BQU8sVUFBVSxDQUFDLEdBQUcsQ0FBQyxDQUFDO1lBQ3pCLENBQUMsRUFBQyxDQUNIO2lCQUNBLFNBQVM7Ozs7WUFBQyxVQUFDLEVBQXFCO29CQUFuQixvQkFBTyxFQUFFLHNCQUFRO2dCQUM3QixJQUFJLE9BQU8sRUFBRTtvQkFDWCxLQUFJLENBQUMsTUFBTSxHQUFHO3dCQUNaLEVBQUUsRUFBRSxRQUFRO3dCQUNaLElBQUksRUFBRSxLQUFJLENBQUMsTUFBTSxDQUFDLElBQUk7cUJBQ3ZCLENBQUM7b0JBQ0YsS0FBSSxDQUFDLFVBQVUsR0FBRyxLQUFJLENBQUMsTUFBTSxDQUFDLElBQUksQ0FBQztvQkFDbkMsS0FBSSxDQUFDLGNBQWMsR0FBRyxLQUFLLENBQUM7aUJBQzdCO3FCQUFNO29CQUNMLEtBQUksQ0FBQyxjQUFjLENBQUMsS0FBSyxDQUFDLDhDQUE4QyxFQUFFLGNBQWMsRUFBRTt3QkFDeEYseUJBQXlCLEVBQUUsQ0FBQyxLQUFJLENBQUMsTUFBTSxDQUFDLElBQUksQ0FBQztxQkFDOUMsQ0FBQyxDQUFDO29CQUNILEtBQUksQ0FBQyxNQUFNLEdBQUcsbUJBQUEsRUFBRSxFQUFpQixDQUFDO2lCQUNuQztnQkFDRCxLQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLFNBQVMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLEtBQUksQ0FBQyxNQUFNLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUM7WUFDbkUsQ0FBQyxFQUFDLENBQUM7U0FDTjthQUFNO1lBQ0wsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxTQUFTLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQztZQUN6QyxJQUFJLENBQUMsVUFBVSxHQUFHLElBQUksQ0FBQztZQUN2QixJQUFJLENBQUMsY0FBYyxHQUFHLEtBQUssQ0FBQztTQUM3QjtJQUNILENBQUM7O2dCQTdERixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGdCQUFnQjtvQkFDMUIsZzlEQUEwQztpQkFDM0M7Ozs7Z0JBVFEsS0FBSztnQkFGTCxjQUFjO2dCQU1kLGNBQWM7O0lBZ0V2Qix5QkFBQztDQUFBLEFBOURELElBOERDO1NBMURZLGtCQUFrQjs7O0lBQzdCLG9DQUE2Qjs7SUFFN0Isd0NBQW1COztJQUVuQiw0Q0FBd0I7O0lBRXhCLHdDQUFvQjs7Ozs7SUFFUixtQ0FBb0I7Ozs7O0lBQUUsNENBQXNDOzs7OztJQUFFLDRDQUFzQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCwgU2V0VGVuYW50LCBTZXNzaW9uU3RhdGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5pbXBvcnQgeyBUb2FzdGVyU2VydmljZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcclxuaW1wb3J0IHsgQ29tcG9uZW50LCBPbkluaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IHRocm93RXJyb3IgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgY2F0Y2hFcnJvciwgdGFrZSwgZmluYWxpemUgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XHJcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcclxuaW1wb3J0IHsgQWNjb3VudFNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcy9hY2NvdW50LnNlcnZpY2UnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdhYnAtdGVuYW50LWJveCcsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL3RlbmFudC1ib3guY29tcG9uZW50Lmh0bWwnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgVGVuYW50Qm94Q29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0IHtcclxuICB0ZW5hbnQgPSB7fSBhcyBBQlAuQmFzaWNJdGVtO1xyXG5cclxuICB0ZW5hbnROYW1lOiBzdHJpbmc7XHJcblxyXG4gIGlzTW9kYWxWaXNpYmxlOiBib29sZWFuO1xyXG5cclxuICBpblByb2dyZXNzOiBib29sZWFuO1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSwgcHJpdmF0ZSB0b2FzdGVyU2VydmljZTogVG9hc3RlclNlcnZpY2UsIHByaXZhdGUgYWNjb3VudFNlcnZpY2U6IEFjY291bnRTZXJ2aWNlKSB7fVxyXG5cclxuICBuZ09uSW5pdCgpIHtcclxuICAgIHRoaXMudGVuYW50ID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXNzaW9uU3RhdGUuZ2V0VGVuYW50KSB8fCAoe30gYXMgQUJQLkJhc2ljSXRlbSk7XHJcbiAgICB0aGlzLnRlbmFudE5hbWUgPSB0aGlzLnRlbmFudC5uYW1lIHx8ICcnO1xyXG4gIH1cclxuXHJcbiAgb25Td2l0Y2goKSB7XHJcbiAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gdHJ1ZTtcclxuICB9XHJcblxyXG4gIHNhdmUoKSB7XHJcbiAgICBpZiAodGhpcy50ZW5hbnQubmFtZSAmJiAhdGhpcy5pblByb2dyZXNzKSB7XHJcbiAgICAgIHRoaXMuaW5Qcm9ncmVzcyA9IHRydWU7XHJcbiAgICAgIHRoaXMuYWNjb3VudFNlcnZpY2VcclxuICAgICAgICAuZmluZFRlbmFudCh0aGlzLnRlbmFudC5uYW1lKVxyXG4gICAgICAgIC5waXBlKFxyXG4gICAgICAgICAgZmluYWxpemUoKCkgPT4gKHRoaXMuaW5Qcm9ncmVzcyA9IGZhbHNlKSksXHJcbiAgICAgICAgICB0YWtlKDEpLFxyXG4gICAgICAgICAgY2F0Y2hFcnJvcihlcnIgPT4ge1xyXG4gICAgICAgICAgICB0aGlzLnRvYXN0ZXJTZXJ2aWNlLmVycm9yKFxyXG4gICAgICAgICAgICAgIHNucSgoKSA9PiBlcnIuZXJyb3IuZXJyb3JfZGVzY3JpcHRpb24sICdBYnBVaTo6RGVmYXVsdEVycm9yTWVzc2FnZScpLFxyXG4gICAgICAgICAgICAgICdBYnBVaTo6RXJyb3InLFxyXG4gICAgICAgICAgICApO1xyXG4gICAgICAgICAgICByZXR1cm4gdGhyb3dFcnJvcihlcnIpO1xyXG4gICAgICAgICAgfSksXHJcbiAgICAgICAgKVxyXG4gICAgICAgIC5zdWJzY3JpYmUoKHsgc3VjY2VzcywgdGVuYW50SWQgfSkgPT4ge1xyXG4gICAgICAgICAgaWYgKHN1Y2Nlc3MpIHtcclxuICAgICAgICAgICAgdGhpcy50ZW5hbnQgPSB7XHJcbiAgICAgICAgICAgICAgaWQ6IHRlbmFudElkLFxyXG4gICAgICAgICAgICAgIG5hbWU6IHRoaXMudGVuYW50Lm5hbWUsXHJcbiAgICAgICAgICAgIH07XHJcbiAgICAgICAgICAgIHRoaXMudGVuYW50TmFtZSA9IHRoaXMudGVuYW50Lm5hbWU7XHJcbiAgICAgICAgICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSBmYWxzZTtcclxuICAgICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIHRoaXMudG9hc3RlclNlcnZpY2UuZXJyb3IoJ0FicFVpTXVsdGlUZW5hbmN5OjpHaXZlblRlbmFudElzTm90QXZhaWxhYmxlJywgJ0FicFVpOjpFcnJvcicsIHtcclxuICAgICAgICAgICAgICBtZXNzYWdlTG9jYWxpemF0aW9uUGFyYW1zOiBbdGhpcy50ZW5hbnQubmFtZV0sXHJcbiAgICAgICAgICAgIH0pO1xyXG4gICAgICAgICAgICB0aGlzLnRlbmFudCA9IHt9IGFzIEFCUC5CYXNpY0l0ZW07XHJcbiAgICAgICAgICB9XHJcbiAgICAgICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBTZXRUZW5hbnQoc3VjY2VzcyA/IHRoaXMudGVuYW50IDogbnVsbCkpO1xyXG4gICAgICAgIH0pO1xyXG4gICAgfSBlbHNlIHtcclxuICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgU2V0VGVuYW50KG51bGwpKTtcclxuICAgICAgdGhpcy50ZW5hbnROYW1lID0gbnVsbDtcclxuICAgICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IGZhbHNlO1xyXG4gICAgfVxyXG4gIH1cclxufVxyXG4iXX0=