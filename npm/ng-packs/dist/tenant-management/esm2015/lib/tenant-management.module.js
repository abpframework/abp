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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50ZW5hbnQtbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi90ZW5hbnQtbWFuYWdlbWVudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDMUMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDekQsT0FBTyxFQUFFLFFBQVEsRUFBWSxNQUFNLGVBQWUsQ0FBQztBQUNuRCxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUMvRCxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3pDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDNUMsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sd0NBQXdDLENBQUM7QUFDMUUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sa0NBQWtDLENBQUM7QUFDekUsT0FBTyxFQUFFLDZCQUE2QixFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDbkYsT0FBTyxFQUFFLHVCQUF1QixFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFDckUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFlM0QsTUFBTSxPQUFPLHNCQUFzQjs7O1lBYmxDLFFBQVEsU0FBQztnQkFDUixZQUFZLEVBQUUsQ0FBQyxnQkFBZ0IsQ0FBQztnQkFDaEMsT0FBTyxFQUFFO29CQUNQLDZCQUE2QjtvQkFDN0IsVUFBVSxDQUFDLFVBQVUsQ0FBQyxDQUFDLHFCQUFxQixDQUFDLENBQUM7b0JBQzlDLHFCQUFxQjtvQkFDckIsVUFBVTtvQkFDVixXQUFXO29CQUNYLGlCQUFpQjtvQkFDakIsaUJBQWlCO29CQUNqQix1QkFBdUI7aUJBQ3hCO2FBQ0Y7Ozs7Ozs7QUFPRCxNQUFNLFVBQVUseUJBQXlCO0lBQ3ZDLE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvcmVNb2R1bGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgVGhlbWVTaGFyZWRNb2R1bGUgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBOZ01vZHVsZSwgUHJvdmlkZXIgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5nYkRyb3Bkb3duTW9kdWxlIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xuaW1wb3J0IHsgTmd4c01vZHVsZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IFRhYmxlTW9kdWxlIH0gZnJvbSAncHJpbWVuZy90YWJsZSc7XG5pbXBvcnQgeyBUZW5hbnRzQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3RlbmFudHMvdGVuYW50cy5jb21wb25lbnQnO1xuaW1wb3J0IHsgVGVuYW50TWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi9zdGF0ZXMvdGVuYW50LW1hbmFnZW1lbnQuc3RhdGUnO1xuaW1wb3J0IHsgVGVuYW50TWFuYWdlbWVudFJvdXRpbmdNb2R1bGUgfSBmcm9tICcuL3RlbmFudC1tYW5hZ2VtZW50LXJvdXRpbmcubW9kdWxlJztcbmltcG9ydCB7IEZlYXR1cmVNYW5hZ2VtZW50TW9kdWxlIH0gZnJvbSAnQGFicC9uZy5mZWF0dXJlLW1hbmFnZW1lbnQnO1xuaW1wb3J0IHsgTmd4VmFsaWRhdGVDb3JlTW9kdWxlIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcblxuQE5nTW9kdWxlKHtcbiAgZGVjbGFyYXRpb25zOiBbVGVuYW50c0NvbXBvbmVudF0sXG4gIGltcG9ydHM6IFtcbiAgICBUZW5hbnRNYW5hZ2VtZW50Um91dGluZ01vZHVsZSxcbiAgICBOZ3hzTW9kdWxlLmZvckZlYXR1cmUoW1RlbmFudE1hbmFnZW1lbnRTdGF0ZV0pLFxuICAgIE5neFZhbGlkYXRlQ29yZU1vZHVsZSxcbiAgICBDb3JlTW9kdWxlLFxuICAgIFRhYmxlTW9kdWxlLFxuICAgIFRoZW1lU2hhcmVkTW9kdWxlLFxuICAgIE5nYkRyb3Bkb3duTW9kdWxlLFxuICAgIEZlYXR1cmVNYW5hZ2VtZW50TW9kdWxlLFxuICBdLFxufSlcbmV4cG9ydCBjbGFzcyBUZW5hbnRNYW5hZ2VtZW50TW9kdWxlIHt9XG5cbi8qKlxuICpcbiAqIEBkZXByZWNhdGVkIHNpbmNlIHZlcnNpb24gMC45LjBcbiAqL1xuZXhwb3J0IGZ1bmN0aW9uIFRlbmFudE1hbmFnZW1lbnRQcm92aWRlcnMoKTogUHJvdmlkZXJbXSB7XG4gIHJldHVybiBbXTtcbn1cbiJdfQ==