/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
 * @return {?}
 */
export function TenantManagementProviders() {
    return [];
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50ZW5hbnQtbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi90ZW5hbnQtbWFuYWdlbWVudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDMUMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDekQsT0FBTyxFQUFFLFFBQVEsRUFBWSxNQUFNLGVBQWUsQ0FBQztBQUNuRCxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUMvRCxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3pDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDNUMsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sd0NBQXdDLENBQUM7QUFDMUUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sa0NBQWtDLENBQUM7QUFDekUsT0FBTyxFQUFFLDZCQUE2QixFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDbkYsT0FBTyxFQUFFLHVCQUF1QixFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFDckUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFlM0QsTUFBTSxPQUFPLHNCQUFzQjs7O1lBYmxDLFFBQVEsU0FBQztnQkFDUixZQUFZLEVBQUUsQ0FBQyxnQkFBZ0IsQ0FBQztnQkFDaEMsT0FBTyxFQUFFO29CQUNQLDZCQUE2QjtvQkFDN0IsVUFBVSxDQUFDLFVBQVUsQ0FBQyxDQUFDLHFCQUFxQixDQUFDLENBQUM7b0JBQzlDLHFCQUFxQjtvQkFDckIsVUFBVTtvQkFDVixXQUFXO29CQUNYLGlCQUFpQjtvQkFDakIsaUJBQWlCO29CQUNqQix1QkFBdUI7aUJBQ3hCO2FBQ0Y7Ozs7O0FBR0QsTUFBTSxVQUFVLHlCQUF5QjtJQUN2QyxPQUFPLEVBQUUsQ0FBQztBQUNaLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb3JlTW9kdWxlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IFRoZW1lU2hhcmVkTW9kdWxlIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgTmdNb2R1bGUsIFByb3ZpZGVyIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBOZ2JEcm9wZG93bk1vZHVsZSB9IGZyb20gJ0BuZy1ib290c3RyYXAvbmctYm9vdHN0cmFwJztcbmltcG9ydCB7IE5neHNNb2R1bGUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBUYWJsZU1vZHVsZSB9IGZyb20gJ3ByaW1lbmcvdGFibGUnO1xuaW1wb3J0IHsgVGVuYW50c0NvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy90ZW5hbnRzL3RlbmFudHMuY29tcG9uZW50JztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnRTdGF0ZSB9IGZyb20gJy4vc3RhdGVzL3RlbmFudC1tYW5hZ2VtZW50LnN0YXRlJztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnRSb3V0aW5nTW9kdWxlIH0gZnJvbSAnLi90ZW5hbnQtbWFuYWdlbWVudC1yb3V0aW5nLm1vZHVsZSc7XG5pbXBvcnQgeyBGZWF0dXJlTWFuYWdlbWVudE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcuZmVhdHVyZS1tYW5hZ2VtZW50JztcbmltcG9ydCB7IE5neFZhbGlkYXRlQ29yZU1vZHVsZSB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5cbkBOZ01vZHVsZSh7XG4gIGRlY2xhcmF0aW9uczogW1RlbmFudHNDb21wb25lbnRdLFxuICBpbXBvcnRzOiBbXG4gICAgVGVuYW50TWFuYWdlbWVudFJvdXRpbmdNb2R1bGUsXG4gICAgTmd4c01vZHVsZS5mb3JGZWF0dXJlKFtUZW5hbnRNYW5hZ2VtZW50U3RhdGVdKSxcbiAgICBOZ3hWYWxpZGF0ZUNvcmVNb2R1bGUsXG4gICAgQ29yZU1vZHVsZSxcbiAgICBUYWJsZU1vZHVsZSxcbiAgICBUaGVtZVNoYXJlZE1vZHVsZSxcbiAgICBOZ2JEcm9wZG93bk1vZHVsZSxcbiAgICBGZWF0dXJlTWFuYWdlbWVudE1vZHVsZSxcbiAgXSxcbn0pXG5leHBvcnQgY2xhc3MgVGVuYW50TWFuYWdlbWVudE1vZHVsZSB7fVxuXG5leHBvcnQgZnVuY3Rpb24gVGVuYW50TWFuYWdlbWVudFByb3ZpZGVycygpOiBQcm92aWRlcltdIHtcbiAgcmV0dXJuIFtdO1xufVxuIl19