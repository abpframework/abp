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
    this.tenant = /** @type {?} */ ({});
  }
  /**
   * @return {?}
   */
  ngOnInit() {
    this.tenant = this.store.selectSnapshot(SessionState.getTenant) || /** @type {?} */ ({});
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
        .pipe(
          take(1),
          catchError(
            /**
             * @param {?} err
             * @return {?}
             */
            err => {
              this.toasterService.error(
                snq(
                  /**
                   * @return {?}
                   */
                  () => err.error.error_description,
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
          ({ success, tenantId }) => {
            if (success) {
              this.tenant = {
                id: tenantId,
                name: this.tenant.name,
              };
              this.tenantName = this.tenant.name;
              this.isModalVisible = false;
            } else {
              this.toasterService.error('AbpUiMultiTenancy::GivenTenantIsNotAvailable', 'AbpUi::Error', {
                messageLocalizationParams: [this.tenant.name],
              });
              this.tenant = /** @type {?} */ ({});
            }
            this.store.dispatch(new SetTenant(success ? this.tenant : null));
          },
        );
    } else {
      this.store.dispatch(new SetTenant(null));
      this.tenantName = null;
      this.isModalVisible = false;
    }
  }
}
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
TenantBoxComponent.ctorParameters = () => [{ type: Store }, { type: ToasterService }, { type: AccountService }];
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LWJveC5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmFjY291bnQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy90ZW5hbnQtYm94L3RlbmFudC1ib3guY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQU8sU0FBUyxFQUFFLFlBQVksRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUM1RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdEQsT0FBTyxFQUFFLFNBQVMsRUFBVSxNQUFNLGVBQWUsQ0FBQztBQUNsRCxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDbEMsT0FBTyxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNsRCxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFDdEIsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLGdDQUFnQyxDQUFDO0FBTWhFLE1BQU0sT0FBTyxrQkFBa0I7Ozs7OztJQUM3QixZQUNVLEtBQVksRUFDWixjQUE4QixFQUM5QixjQUE4QjtRQUY5QixVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQ1osbUJBQWMsR0FBZCxjQUFjLENBQWdCO1FBQzlCLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtRQUd4QyxXQUFNLEdBQUcsbUJBQUEsRUFBRSxFQUFpQixDQUFDO0lBRjFCLENBQUM7Ozs7SUFRSixRQUFRO1FBQ04sSUFBSSxDQUFDLE1BQU07WUFDVCxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxZQUFZLENBQUMsU0FBUyxDQUFDO2dCQUNqRCxDQUFDLG1CQUFBLEVBQUUsRUFBaUIsQ0FBQyxDQUFDO1FBQ3hCLElBQUksQ0FBQyxVQUFVLEdBQUcsSUFBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLElBQUksRUFBRSxDQUFDO0lBQzNDLENBQUM7Ozs7SUFFRCxRQUFRO1FBQ04sSUFBSSxDQUFDLGNBQWMsR0FBRyxJQUFJLENBQUM7SUFDN0IsQ0FBQzs7OztJQUVELElBQUk7UUFDRixJQUFJLElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxFQUFFO1lBQ3BCLElBQUksQ0FBQyxjQUFjO2lCQUNoQixVQUFVLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUM7aUJBQzVCLElBQUksQ0FDSCxJQUFJLENBQUMsQ0FBQyxDQUFDLEVBQ1AsVUFBVTs7OztZQUFDLEdBQUcsQ0FBQyxFQUFFO2dCQUNmLElBQUksQ0FBQyxjQUFjLENBQUMsS0FBSyxDQUN2QixHQUFHOzs7Z0JBQ0QsR0FBRyxFQUFFLENBQUMsR0FBRyxDQUFDLEtBQUssQ0FBQyxpQkFBaUIsR0FDakMsNEJBQTRCLENBQzdCLEVBQ0QsY0FBYyxDQUNmLENBQUM7Z0JBQ0YsT0FBTyxVQUFVLENBQUMsR0FBRyxDQUFDLENBQUM7WUFDekIsQ0FBQyxFQUFDLENBQ0g7aUJBQ0EsU0FBUzs7OztZQUFDLENBQUMsRUFBRSxPQUFPLEVBQUUsUUFBUSxFQUFFLEVBQUUsRUFBRTtnQkFDbkMsSUFBSSxPQUFPLEVBQUU7b0JBQ1gsSUFBSSxDQUFDLE1BQU0sR0FBRzt3QkFDWixFQUFFLEVBQUUsUUFBUTt3QkFDWixJQUFJLEVBQUUsSUFBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJO3FCQUN2QixDQUFDO29CQUNGLElBQUksQ0FBQyxVQUFVLEdBQUcsSUFBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUM7b0JBQ25DLElBQUksQ0FBQyxjQUFjLEdBQUcsS0FBSyxDQUFDO2lCQUM3QjtxQkFBTTtvQkFDTCxJQUFJLENBQUMsY0FBYyxDQUFDLEtBQUssQ0FDdkIsOENBQThDLEVBQzlDLGNBQWMsRUFDZDt3QkFDRSx5QkFBeUIsRUFBRSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsSUFBSSxDQUFDO3FCQUM5QyxDQUNGLENBQUM7b0JBQ0YsSUFBSSxDQUFDLE1BQU0sR0FBRyxtQkFBQSxFQUFFLEVBQWlCLENBQUM7aUJBQ25DO2dCQUNELElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksU0FBUyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQztZQUNuRSxDQUFDLEVBQUMsQ0FBQztTQUNOO2FBQU07WUFDTCxJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLFNBQVMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDO1lBQ3pDLElBQUksQ0FBQyxVQUFVLEdBQUcsSUFBSSxDQUFDO1lBQ3ZCLElBQUksQ0FBQyxjQUFjLEdBQUcsS0FBSyxDQUFDO1NBQzdCO0lBQ0gsQ0FBQzs7O1lBdEVGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsZ0JBQWdCO2dCQUMxQiwwZ0RBQTBDO2FBQzNDOzs7O1lBVFEsS0FBSztZQUZMLGNBQWM7WUFNZCxjQUFjOzs7O0lBYXJCLG9DQUE2Qjs7SUFFN0Isd0NBQW1COztJQUVuQiw0Q0FBd0I7Ozs7O0lBVHRCLG1DQUFvQjs7Ozs7SUFDcEIsNENBQXNDOzs7OztJQUN0Qyw0Q0FBc0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBQlAsIFNldFRlbmFudCwgU2Vzc2lvblN0YXRlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IFRvYXN0ZXJTZXJ2aWNlIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgQ29tcG9uZW50LCBPbkluaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgdGhyb3dFcnJvciB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgY2F0Y2hFcnJvciwgdGFrZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcbmltcG9ydCB7IEFjY291bnRTZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvYWNjb3VudC5zZXJ2aWNlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXRlbmFudC1ib3gnLFxuICB0ZW1wbGF0ZVVybDogJy4vdGVuYW50LWJveC5jb21wb25lbnQuaHRtbCdcbn0pXG5leHBvcnQgY2xhc3MgVGVuYW50Qm94Q29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0IHtcbiAgY29uc3RydWN0b3IoXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXG4gICAgcHJpdmF0ZSB0b2FzdGVyU2VydmljZTogVG9hc3RlclNlcnZpY2UsXG4gICAgcHJpdmF0ZSBhY2NvdW50U2VydmljZTogQWNjb3VudFNlcnZpY2VcbiAgKSB7fVxuXG4gIHRlbmFudCA9IHt9IGFzIEFCUC5CYXNpY0l0ZW07XG5cbiAgdGVuYW50TmFtZTogc3RyaW5nO1xuXG4gIGlzTW9kYWxWaXNpYmxlOiBib29sZWFuO1xuXG4gIG5nT25Jbml0KCkge1xuICAgIHRoaXMudGVuYW50ID1cbiAgICAgIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoU2Vzc2lvblN0YXRlLmdldFRlbmFudCkgfHxcbiAgICAgICh7fSBhcyBBQlAuQmFzaWNJdGVtKTtcbiAgICB0aGlzLnRlbmFudE5hbWUgPSB0aGlzLnRlbmFudC5uYW1lIHx8ICcnO1xuICB9XG5cbiAgb25Td2l0Y2goKSB7XG4gICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IHRydWU7XG4gIH1cblxuICBzYXZlKCkge1xuICAgIGlmICh0aGlzLnRlbmFudC5uYW1lKSB7XG4gICAgICB0aGlzLmFjY291bnRTZXJ2aWNlXG4gICAgICAgIC5maW5kVGVuYW50KHRoaXMudGVuYW50Lm5hbWUpXG4gICAgICAgIC5waXBlKFxuICAgICAgICAgIHRha2UoMSksXG4gICAgICAgICAgY2F0Y2hFcnJvcihlcnIgPT4ge1xuICAgICAgICAgICAgdGhpcy50b2FzdGVyU2VydmljZS5lcnJvcihcbiAgICAgICAgICAgICAgc25xKFxuICAgICAgICAgICAgICAgICgpID0+IGVyci5lcnJvci5lcnJvcl9kZXNjcmlwdGlvbixcbiAgICAgICAgICAgICAgICAnQWJwVWk6OkRlZmF1bHRFcnJvck1lc3NhZ2UnXG4gICAgICAgICAgICAgICksXG4gICAgICAgICAgICAgICdBYnBVaTo6RXJyb3InXG4gICAgICAgICAgICApO1xuICAgICAgICAgICAgcmV0dXJuIHRocm93RXJyb3IoZXJyKTtcbiAgICAgICAgICB9KVxuICAgICAgICApXG4gICAgICAgIC5zdWJzY3JpYmUoKHsgc3VjY2VzcywgdGVuYW50SWQgfSkgPT4ge1xuICAgICAgICAgIGlmIChzdWNjZXNzKSB7XG4gICAgICAgICAgICB0aGlzLnRlbmFudCA9IHtcbiAgICAgICAgICAgICAgaWQ6IHRlbmFudElkLFxuICAgICAgICAgICAgICBuYW1lOiB0aGlzLnRlbmFudC5uYW1lXG4gICAgICAgICAgICB9O1xuICAgICAgICAgICAgdGhpcy50ZW5hbnROYW1lID0gdGhpcy50ZW5hbnQubmFtZTtcbiAgICAgICAgICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSBmYWxzZTtcbiAgICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgICAgdGhpcy50b2FzdGVyU2VydmljZS5lcnJvcihcbiAgICAgICAgICAgICAgJ0FicFVpTXVsdGlUZW5hbmN5OjpHaXZlblRlbmFudElzTm90QXZhaWxhYmxlJyxcbiAgICAgICAgICAgICAgJ0FicFVpOjpFcnJvcicsXG4gICAgICAgICAgICAgIHtcbiAgICAgICAgICAgICAgICBtZXNzYWdlTG9jYWxpemF0aW9uUGFyYW1zOiBbdGhpcy50ZW5hbnQubmFtZV1cbiAgICAgICAgICAgICAgfVxuICAgICAgICAgICAgKTtcbiAgICAgICAgICAgIHRoaXMudGVuYW50ID0ge30gYXMgQUJQLkJhc2ljSXRlbTtcbiAgICAgICAgICB9XG4gICAgICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgU2V0VGVuYW50KHN1Y2Nlc3MgPyB0aGlzLnRlbmFudCA6IG51bGwpKTtcbiAgICAgICAgfSk7XG4gICAgfSBlbHNlIHtcbiAgICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFNldFRlbmFudChudWxsKSk7XG4gICAgICB0aGlzLnRlbmFudE5hbWUgPSBudWxsO1xuICAgICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IGZhbHNlO1xuICAgIH1cbiAgfVxufVxuIl19
