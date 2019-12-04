/**
 * @fileoverview added by tsickle
 * Generated from: lib/tenant-management.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxsModule } from '@ngxs/store';
import { TableModule } from 'primeng/table';
import { TenantsComponent } from './components/tenants/tenants.component';
import { TenantManagementState } from './states/tenant-management.state';
import { TenantManagementRoutingModule } from './tenant-management-routing.module';
import { FeatureManagementModule } from '@abp/ng.feature-management';
import { NgxValidateCoreModule } from '@ngx-validate/core';
var TenantManagementModule = /** @class */ (function () {
    function TenantManagementModule() {
    }
    TenantManagementModule.decorators = [
        { type: NgModule, args: [{
                    declarations: [TenantsComponent],
                    imports: [
                        TenantManagementRoutingModule,
                        NgxsModule.forFeature([TenantManagementState]),
                        NgxValidateCoreModule,
                        CoreModule,
                        TableModule,
                        ThemeSharedModule,
                        NgbDropdownModule,
                        FeatureManagementModule,
                    ],
                },] }
    ];
    return TenantManagementModule;
}());
export { TenantManagementModule };
/**
 *
 * @deprecated since version 0.9.0
 * @return {?}
 */
export function TenantManagementProviders() {
    return [];
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50ZW5hbnQtbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi90ZW5hbnQtbWFuYWdlbWVudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzFDLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3pELE9BQU8sRUFBRSxRQUFRLEVBQVksTUFBTSxlQUFlLENBQUM7QUFDbkQsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFDL0QsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUN6QyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLHdDQUF3QyxDQUFDO0FBQzFFLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLGtDQUFrQyxDQUFDO0FBQ3pFLE9BQU8sRUFBRSw2QkFBNkIsRUFBRSxNQUFNLG9DQUFvQyxDQUFDO0FBQ25GLE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBQ3JFLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBRTNEO0lBQUE7SUFhcUMsQ0FBQzs7Z0JBYnJDLFFBQVEsU0FBQztvQkFDUixZQUFZLEVBQUUsQ0FBQyxnQkFBZ0IsQ0FBQztvQkFDaEMsT0FBTyxFQUFFO3dCQUNQLDZCQUE2Qjt3QkFDN0IsVUFBVSxDQUFDLFVBQVUsQ0FBQyxDQUFDLHFCQUFxQixDQUFDLENBQUM7d0JBQzlDLHFCQUFxQjt3QkFDckIsVUFBVTt3QkFDVixXQUFXO3dCQUNYLGlCQUFpQjt3QkFDakIsaUJBQWlCO3dCQUNqQix1QkFBdUI7cUJBQ3hCO2lCQUNGOztJQUNvQyw2QkFBQztDQUFBLEFBYnRDLElBYXNDO1NBQXpCLHNCQUFzQjs7Ozs7O0FBTW5DLE1BQU0sVUFBVSx5QkFBeUI7SUFDdkMsT0FBTyxFQUFFLENBQUM7QUFDWixDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29yZU1vZHVsZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBUaGVtZVNoYXJlZE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IE5nTW9kdWxlLCBQcm92aWRlciB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmdiRHJvcGRvd25Nb2R1bGUgfSBmcm9tICdAbmctYm9vdHN0cmFwL25nLWJvb3RzdHJhcCc7XG5pbXBvcnQgeyBOZ3hzTW9kdWxlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgVGFibGVNb2R1bGUgfSBmcm9tICdwcmltZW5nL3RhYmxlJztcbmltcG9ydCB7IFRlbmFudHNDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvdGVuYW50cy90ZW5hbnRzLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuL3N0YXRlcy90ZW5hbnQtbWFuYWdlbWVudC5zdGF0ZSc7XG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50Um91dGluZ01vZHVsZSB9IGZyb20gJy4vdGVuYW50LW1hbmFnZW1lbnQtcm91dGluZy5tb2R1bGUnO1xuaW1wb3J0IHsgRmVhdHVyZU1hbmFnZW1lbnRNb2R1bGUgfSBmcm9tICdAYWJwL25nLmZlYXR1cmUtbWFuYWdlbWVudCc7XG5pbXBvcnQgeyBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xuXG5ATmdNb2R1bGUoe1xuICBkZWNsYXJhdGlvbnM6IFtUZW5hbnRzQ29tcG9uZW50XSxcbiAgaW1wb3J0czogW1xuICAgIFRlbmFudE1hbmFnZW1lbnRSb3V0aW5nTW9kdWxlLFxuICAgIE5neHNNb2R1bGUuZm9yRmVhdHVyZShbVGVuYW50TWFuYWdlbWVudFN0YXRlXSksXG4gICAgTmd4VmFsaWRhdGVDb3JlTW9kdWxlLFxuICAgIENvcmVNb2R1bGUsXG4gICAgVGFibGVNb2R1bGUsXG4gICAgVGhlbWVTaGFyZWRNb2R1bGUsXG4gICAgTmdiRHJvcGRvd25Nb2R1bGUsXG4gICAgRmVhdHVyZU1hbmFnZW1lbnRNb2R1bGUsXG4gIF0sXG59KVxuZXhwb3J0IGNsYXNzIFRlbmFudE1hbmFnZW1lbnRNb2R1bGUge31cblxuLyoqXG4gKlxuICogQGRlcHJlY2F0ZWQgc2luY2UgdmVyc2lvbiAwLjkuMFxuICovXG5leHBvcnQgZnVuY3Rpb24gVGVuYW50TWFuYWdlbWVudFByb3ZpZGVycygpOiBQcm92aWRlcltdIHtcbiAgcmV0dXJuIFtdO1xufVxuIl19