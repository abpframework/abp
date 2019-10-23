/**
 * @fileoverview added by tsickle
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
export class TenantManagementModule {
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
/**
 *
 * @deprecated since version 0.9.0
 * @return {?}
 */
export function TenantManagementProviders() {
    return [];
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50ZW5hbnQtbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi90ZW5hbnQtbWFuYWdlbWVudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDMUMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDekQsT0FBTyxFQUFFLFFBQVEsRUFBWSxNQUFNLGVBQWUsQ0FBQztBQUNuRCxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUMvRCxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3pDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDNUMsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sd0NBQXdDLENBQUM7QUFDMUUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sa0NBQWtDLENBQUM7QUFDekUsT0FBTyxFQUFFLDZCQUE2QixFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDbkYsT0FBTyxFQUFFLHVCQUF1QixFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFDckUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFlM0QsTUFBTSxPQUFPLHNCQUFzQjs7O1lBYmxDLFFBQVEsU0FBQztnQkFDUixZQUFZLEVBQUUsQ0FBQyxnQkFBZ0IsQ0FBQztnQkFDaEMsT0FBTyxFQUFFO29CQUNQLDZCQUE2QjtvQkFDN0IsVUFBVSxDQUFDLFVBQVUsQ0FBQyxDQUFDLHFCQUFxQixDQUFDLENBQUM7b0JBQzlDLHFCQUFxQjtvQkFDckIsVUFBVTtvQkFDVixXQUFXO29CQUNYLGlCQUFpQjtvQkFDakIsaUJBQWlCO29CQUNqQix1QkFBdUI7aUJBQ3hCO2FBQ0Y7Ozs7Ozs7QUFPRCxNQUFNLFVBQVUseUJBQXlCO0lBQ3ZDLE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5pbXBvcnQgeyBUaGVtZVNoYXJlZE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcclxuaW1wb3J0IHsgTmdNb2R1bGUsIFByb3ZpZGVyIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IE5nYkRyb3Bkb3duTW9kdWxlIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xyXG5pbXBvcnQgeyBOZ3hzTW9kdWxlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBUYWJsZU1vZHVsZSB9IGZyb20gJ3ByaW1lbmcvdGFibGUnO1xyXG5pbXBvcnQgeyBUZW5hbnRzQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3RlbmFudHMvdGVuYW50cy5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuL3N0YXRlcy90ZW5hbnQtbWFuYWdlbWVudC5zdGF0ZSc7XHJcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnRSb3V0aW5nTW9kdWxlIH0gZnJvbSAnLi90ZW5hbnQtbWFuYWdlbWVudC1yb3V0aW5nLm1vZHVsZSc7XHJcbmltcG9ydCB7IEZlYXR1cmVNYW5hZ2VtZW50TW9kdWxlIH0gZnJvbSAnQGFicC9uZy5mZWF0dXJlLW1hbmFnZW1lbnQnO1xyXG5pbXBvcnQgeyBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xyXG5cclxuQE5nTW9kdWxlKHtcclxuICBkZWNsYXJhdGlvbnM6IFtUZW5hbnRzQ29tcG9uZW50XSxcclxuICBpbXBvcnRzOiBbXHJcbiAgICBUZW5hbnRNYW5hZ2VtZW50Um91dGluZ01vZHVsZSxcclxuICAgIE5neHNNb2R1bGUuZm9yRmVhdHVyZShbVGVuYW50TWFuYWdlbWVudFN0YXRlXSksXHJcbiAgICBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUsXHJcbiAgICBDb3JlTW9kdWxlLFxyXG4gICAgVGFibGVNb2R1bGUsXHJcbiAgICBUaGVtZVNoYXJlZE1vZHVsZSxcclxuICAgIE5nYkRyb3Bkb3duTW9kdWxlLFxyXG4gICAgRmVhdHVyZU1hbmFnZW1lbnRNb2R1bGUsXHJcbiAgXSxcclxufSlcclxuZXhwb3J0IGNsYXNzIFRlbmFudE1hbmFnZW1lbnRNb2R1bGUge31cclxuXHJcbi8qKlxyXG4gKlxyXG4gKiBAZGVwcmVjYXRlZCBzaW5jZSB2ZXJzaW9uIDAuOS4wXHJcbiAqL1xyXG5leHBvcnQgZnVuY3Rpb24gVGVuYW50TWFuYWdlbWVudFByb3ZpZGVycygpOiBQcm92aWRlcltdIHtcclxuICByZXR1cm4gW107XHJcbn1cclxuIl19