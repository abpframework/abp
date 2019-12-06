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
                    template: "<div class=\"card shadow-sm rounded mb-3\">\n  <div class=\"card-body px-5\">\n    <div class=\"row\">\n      <div class=\"col\">\n        <span style=\"font-size: 0.8em;\" class=\"text-uppercase text-muted\">{{\n          'AbpUiMultiTenancy::Tenant' | abpLocalization\n        }}</span\n        ><br />\n        <h6 class=\"m-0 d-inline-block\">\n          <span>\n            {{ tenantName || ('AbpUiMultiTenancy::NotSelected' | abpLocalization) }}\n          </span>\n        </h6>\n      </div>\n      <div class=\"col-auto\">\n        <a\n          id=\"AbpTenantSwitchLink\"\n          href=\"javascript:void(0);\"\n          class=\"btn btn-sm mt-3 btn-outline-primary\"\n          (click)=\"onSwitch()\"\n          >{{ 'AbpUiMultiTenancy::Switch' | abpLocalization }}</a\n        >\n      </div>\n    </div>\n  </div>\n</div>\n\n<abp-modal size=\"md\" [(visible)]=\"isModalVisible\" [busy]=\"inProgress\">\n  <ng-template #abpHeader>\n    <h5>Switch Tenant</h5>\n  </ng-template>\n  <ng-template #abpBody>\n    <form (ngSubmit)=\"save()\">\n      <div class=\"mt-2\">\n        <div class=\"form-group\">\n          <label for=\"name\">{{ 'AbpUiMultiTenancy::Name' | abpLocalization }}</label>\n          <input [(ngModel)]=\"tenant.name\" type=\"text\" id=\"name\" name=\"tenant\" class=\"form-control\" autofocus />\n        </div>\n        <p>{{ 'AbpUiMultiTenancy::SwitchTenantHint' | abpLocalization }}</p>\n      </div>\n    </form>\n  </ng-template>\n  <ng-template #abpFooter>\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n    </button>\n    <abp-button buttonType=\"button\" buttonClass=\"btn btn-primary\" (click)=\"save()\">\n      <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpTenantManagement::Save' | abpLocalization }}</span>\n    </abp-button>\n  </ng-template>\n</abp-modal>\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LWJveC5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmFjY291bnQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy90ZW5hbnQtYm94L3RlbmFudC1ib3guY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFPLFNBQVMsRUFBRSxZQUFZLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDNUQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3RELE9BQU8sRUFBRSxTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFDbEQsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2xDLE9BQU8sRUFBRSxVQUFVLEVBQUUsSUFBSSxFQUFFLFFBQVEsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQzVELE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUN0QixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sZ0NBQWdDLENBQUM7QUFFaEU7SUFhRSw0QkFBb0IsS0FBWSxFQUFVLGNBQThCLEVBQVUsY0FBOEI7UUFBNUYsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUFVLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtRQUFVLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtRQVJoSCxXQUFNLEdBQUcsbUJBQUEsRUFBRSxFQUFpQixDQUFDO0lBUXNGLENBQUM7Ozs7SUFFcEgscUNBQVE7OztJQUFSO1FBQ0UsSUFBSSxDQUFDLE1BQU0sR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxZQUFZLENBQUMsU0FBUyxDQUFDLElBQUksQ0FBQyxtQkFBQSxFQUFFLEVBQWlCLENBQUMsQ0FBQztRQUN6RixJQUFJLENBQUMsVUFBVSxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxJQUFJLEVBQUUsQ0FBQztJQUMzQyxDQUFDOzs7O0lBRUQscUNBQVE7OztJQUFSO1FBQ0UsSUFBSSxDQUFDLGNBQWMsR0FBRyxJQUFJLENBQUM7SUFDN0IsQ0FBQzs7OztJQUVELGlDQUFJOzs7SUFBSjtRQUFBLGlCQXFDQztRQXBDQyxJQUFJLElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxJQUFJLENBQUMsSUFBSSxDQUFDLFVBQVUsRUFBRTtZQUN4QyxJQUFJLENBQUMsVUFBVSxHQUFHLElBQUksQ0FBQztZQUN2QixJQUFJLENBQUMsY0FBYztpQkFDaEIsVUFBVSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxDQUFDO2lCQUM1QixJQUFJLENBQ0gsUUFBUTs7O1lBQUMsY0FBTSxPQUFBLENBQUMsS0FBSSxDQUFDLFVBQVUsR0FBRyxLQUFLLENBQUMsRUFBekIsQ0FBeUIsRUFBQyxFQUN6QyxJQUFJLENBQUMsQ0FBQyxDQUFDLEVBQ1AsVUFBVTs7OztZQUFDLFVBQUEsR0FBRztnQkFDWixLQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FDdkIsR0FBRzs7O2dCQUFDLGNBQU0sT0FBQSxHQUFHLENBQUMsS0FBSyxDQUFDLGlCQUFpQixFQUEzQixDQUEyQixHQUFFLDRCQUE0QixDQUFDLEVBQ3BFLGNBQWMsQ0FDZixDQUFDO2dCQUNGLE9BQU8sVUFBVSxDQUFDLEdBQUcsQ0FBQyxDQUFDO1lBQ3pCLENBQUMsRUFBQyxDQUNIO2lCQUNBLFNBQVM7Ozs7WUFBQyxVQUFDLEVBQXFCO29CQUFuQixvQkFBTyxFQUFFLHNCQUFRO2dCQUM3QixJQUFJLE9BQU8sRUFBRTtvQkFDWCxLQUFJLENBQUMsTUFBTSxHQUFHO3dCQUNaLEVBQUUsRUFBRSxRQUFRO3dCQUNaLElBQUksRUFBRSxLQUFJLENBQUMsTUFBTSxDQUFDLElBQUk7cUJBQ3ZCLENBQUM7b0JBQ0YsS0FBSSxDQUFDLFVBQVUsR0FBRyxLQUFJLENBQUMsTUFBTSxDQUFDLElBQUksQ0FBQztvQkFDbkMsS0FBSSxDQUFDLGNBQWMsR0FBRyxLQUFLLENBQUM7aUJBQzdCO3FCQUFNO29CQUNMLEtBQUksQ0FBQyxjQUFjLENBQUMsS0FBSyxDQUFDLDhDQUE4QyxFQUFFLGNBQWMsRUFBRTt3QkFDeEYseUJBQXlCLEVBQUUsQ0FBQyxLQUFJLENBQUMsTUFBTSxDQUFDLElBQUksQ0FBQztxQkFDOUMsQ0FBQyxDQUFDO29CQUNILEtBQUksQ0FBQyxNQUFNLEdBQUcsbUJBQUEsRUFBRSxFQUFpQixDQUFDO2lCQUNuQztnQkFDRCxLQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLFNBQVMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLEtBQUksQ0FBQyxNQUFNLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUM7WUFDbkUsQ0FBQyxFQUFDLENBQUM7U0FDTjthQUFNO1lBQ0wsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxTQUFTLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQztZQUN6QyxJQUFJLENBQUMsVUFBVSxHQUFHLElBQUksQ0FBQztZQUN2QixJQUFJLENBQUMsY0FBYyxHQUFHLEtBQUssQ0FBQztTQUM3QjtJQUNILENBQUM7O2dCQTdERixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGdCQUFnQjtvQkFDMUIsMDJEQUEwQztpQkFDM0M7Ozs7Z0JBVFEsS0FBSztnQkFGTCxjQUFjO2dCQU1kLGNBQWM7O0lBZ0V2Qix5QkFBQztDQUFBLEFBOURELElBOERDO1NBMURZLGtCQUFrQjs7O0lBQzdCLG9DQUE2Qjs7SUFFN0Isd0NBQW1COztJQUVuQiw0Q0FBd0I7O0lBRXhCLHdDQUFvQjs7Ozs7SUFFUixtQ0FBb0I7Ozs7O0lBQUUsNENBQXNDOzs7OztJQUFFLDRDQUFzQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCwgU2V0VGVuYW50LCBTZXNzaW9uU3RhdGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgVG9hc3RlclNlcnZpY2UgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBDb21wb25lbnQsIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyB0aHJvd0Vycm9yIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBjYXRjaEVycm9yLCB0YWtlLCBmaW5hbGl6ZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcbmltcG9ydCB7IEFjY291bnRTZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvYWNjb3VudC5zZXJ2aWNlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXRlbmFudC1ib3gnLFxuICB0ZW1wbGF0ZVVybDogJy4vdGVuYW50LWJveC5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFRlbmFudEJveENvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XG4gIHRlbmFudCA9IHt9IGFzIEFCUC5CYXNpY0l0ZW07XG5cbiAgdGVuYW50TmFtZTogc3RyaW5nO1xuXG4gIGlzTW9kYWxWaXNpYmxlOiBib29sZWFuO1xuXG4gIGluUHJvZ3Jlc3M6IGJvb2xlYW47XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUsIHByaXZhdGUgdG9hc3RlclNlcnZpY2U6IFRvYXN0ZXJTZXJ2aWNlLCBwcml2YXRlIGFjY291bnRTZXJ2aWNlOiBBY2NvdW50U2VydmljZSkge31cblxuICBuZ09uSW5pdCgpIHtcbiAgICB0aGlzLnRlbmFudCA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoU2Vzc2lvblN0YXRlLmdldFRlbmFudCkgfHwgKHt9IGFzIEFCUC5CYXNpY0l0ZW0pO1xuICAgIHRoaXMudGVuYW50TmFtZSA9IHRoaXMudGVuYW50Lm5hbWUgfHwgJyc7XG4gIH1cblxuICBvblN3aXRjaCgpIHtcbiAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gdHJ1ZTtcbiAgfVxuXG4gIHNhdmUoKSB7XG4gICAgaWYgKHRoaXMudGVuYW50Lm5hbWUgJiYgIXRoaXMuaW5Qcm9ncmVzcykge1xuICAgICAgdGhpcy5pblByb2dyZXNzID0gdHJ1ZTtcbiAgICAgIHRoaXMuYWNjb3VudFNlcnZpY2VcbiAgICAgICAgLmZpbmRUZW5hbnQodGhpcy50ZW5hbnQubmFtZSlcbiAgICAgICAgLnBpcGUoXG4gICAgICAgICAgZmluYWxpemUoKCkgPT4gKHRoaXMuaW5Qcm9ncmVzcyA9IGZhbHNlKSksXG4gICAgICAgICAgdGFrZSgxKSxcbiAgICAgICAgICBjYXRjaEVycm9yKGVyciA9PiB7XG4gICAgICAgICAgICB0aGlzLnRvYXN0ZXJTZXJ2aWNlLmVycm9yKFxuICAgICAgICAgICAgICBzbnEoKCkgPT4gZXJyLmVycm9yLmVycm9yX2Rlc2NyaXB0aW9uLCAnQWJwVWk6OkRlZmF1bHRFcnJvck1lc3NhZ2UnKSxcbiAgICAgICAgICAgICAgJ0FicFVpOjpFcnJvcicsXG4gICAgICAgICAgICApO1xuICAgICAgICAgICAgcmV0dXJuIHRocm93RXJyb3IoZXJyKTtcbiAgICAgICAgICB9KSxcbiAgICAgICAgKVxuICAgICAgICAuc3Vic2NyaWJlKCh7IHN1Y2Nlc3MsIHRlbmFudElkIH0pID0+IHtcbiAgICAgICAgICBpZiAoc3VjY2Vzcykge1xuICAgICAgICAgICAgdGhpcy50ZW5hbnQgPSB7XG4gICAgICAgICAgICAgIGlkOiB0ZW5hbnRJZCxcbiAgICAgICAgICAgICAgbmFtZTogdGhpcy50ZW5hbnQubmFtZSxcbiAgICAgICAgICAgIH07XG4gICAgICAgICAgICB0aGlzLnRlbmFudE5hbWUgPSB0aGlzLnRlbmFudC5uYW1lO1xuICAgICAgICAgICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IGZhbHNlO1xuICAgICAgICAgIH0gZWxzZSB7XG4gICAgICAgICAgICB0aGlzLnRvYXN0ZXJTZXJ2aWNlLmVycm9yKCdBYnBVaU11bHRpVGVuYW5jeTo6R2l2ZW5UZW5hbnRJc05vdEF2YWlsYWJsZScsICdBYnBVaTo6RXJyb3InLCB7XG4gICAgICAgICAgICAgIG1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXM6IFt0aGlzLnRlbmFudC5uYW1lXSxcbiAgICAgICAgICAgIH0pO1xuICAgICAgICAgICAgdGhpcy50ZW5hbnQgPSB7fSBhcyBBQlAuQmFzaWNJdGVtO1xuICAgICAgICAgIH1cbiAgICAgICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBTZXRUZW5hbnQoc3VjY2VzcyA/IHRoaXMudGVuYW50IDogbnVsbCkpO1xuICAgICAgICB9KTtcbiAgICB9IGVsc2Uge1xuICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgU2V0VGVuYW50KG51bGwpKTtcbiAgICAgIHRoaXMudGVuYW50TmFtZSA9IG51bGw7XG4gICAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gZmFsc2U7XG4gICAgfVxuICB9XG59XG4iXX0=