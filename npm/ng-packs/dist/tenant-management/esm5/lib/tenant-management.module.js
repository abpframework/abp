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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50ZW5hbnQtbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi90ZW5hbnQtbWFuYWdlbWVudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDMUMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDekQsT0FBTyxFQUFFLFFBQVEsRUFBWSxNQUFNLGVBQWUsQ0FBQztBQUNuRCxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUMvRCxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3pDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDNUMsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sd0NBQXdDLENBQUM7QUFDMUUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sa0NBQWtDLENBQUM7QUFDekUsT0FBTyxFQUFFLDZCQUE2QixFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDbkYsT0FBTyxFQUFFLHVCQUF1QixFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFDckUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFFM0Q7SUFBQTtJQWFxQyxDQUFDOztnQkFickMsUUFBUSxTQUFDO29CQUNSLFlBQVksRUFBRSxDQUFDLGdCQUFnQixDQUFDO29CQUNoQyxPQUFPLEVBQUU7d0JBQ1AsNkJBQTZCO3dCQUM3QixVQUFVLENBQUMsVUFBVSxDQUFDLENBQUMscUJBQXFCLENBQUMsQ0FBQzt3QkFDOUMscUJBQXFCO3dCQUNyQixVQUFVO3dCQUNWLFdBQVc7d0JBQ1gsaUJBQWlCO3dCQUNqQixpQkFBaUI7d0JBQ2pCLHVCQUF1QjtxQkFDeEI7aUJBQ0Y7O0lBQ29DLDZCQUFDO0NBQUEsQUFidEMsSUFhc0M7U0FBekIsc0JBQXNCOzs7Ozs7QUFNbkMsTUFBTSxVQUFVLHlCQUF5QjtJQUN2QyxPQUFPLEVBQUUsQ0FBQztBQUNaLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb3JlTW9kdWxlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgVGhlbWVTaGFyZWRNb2R1bGUgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XHJcbmltcG9ydCB7IE5nTW9kdWxlLCBQcm92aWRlciB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBOZ2JEcm9wZG93bk1vZHVsZSB9IGZyb20gJ0BuZy1ib290c3RyYXAvbmctYm9vdHN0cmFwJztcclxuaW1wb3J0IHsgTmd4c01vZHVsZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgVGFibGVNb2R1bGUgfSBmcm9tICdwcmltZW5nL3RhYmxlJztcclxuaW1wb3J0IHsgVGVuYW50c0NvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy90ZW5hbnRzL3RlbmFudHMuY29tcG9uZW50JztcclxuaW1wb3J0IHsgVGVuYW50TWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi9zdGF0ZXMvdGVuYW50LW1hbmFnZW1lbnQuc3RhdGUnO1xyXG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50Um91dGluZ01vZHVsZSB9IGZyb20gJy4vdGVuYW50LW1hbmFnZW1lbnQtcm91dGluZy5tb2R1bGUnO1xyXG5pbXBvcnQgeyBGZWF0dXJlTWFuYWdlbWVudE1vZHVsZSB9IGZyb20gJ0BhYnAvbmcuZmVhdHVyZS1tYW5hZ2VtZW50JztcclxuaW1wb3J0IHsgTmd4VmFsaWRhdGVDb3JlTW9kdWxlIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcclxuXHJcbkBOZ01vZHVsZSh7XHJcbiAgZGVjbGFyYXRpb25zOiBbVGVuYW50c0NvbXBvbmVudF0sXHJcbiAgaW1wb3J0czogW1xyXG4gICAgVGVuYW50TWFuYWdlbWVudFJvdXRpbmdNb2R1bGUsXHJcbiAgICBOZ3hzTW9kdWxlLmZvckZlYXR1cmUoW1RlbmFudE1hbmFnZW1lbnRTdGF0ZV0pLFxyXG4gICAgTmd4VmFsaWRhdGVDb3JlTW9kdWxlLFxyXG4gICAgQ29yZU1vZHVsZSxcclxuICAgIFRhYmxlTW9kdWxlLFxyXG4gICAgVGhlbWVTaGFyZWRNb2R1bGUsXHJcbiAgICBOZ2JEcm9wZG93bk1vZHVsZSxcclxuICAgIEZlYXR1cmVNYW5hZ2VtZW50TW9kdWxlLFxyXG4gIF0sXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBUZW5hbnRNYW5hZ2VtZW50TW9kdWxlIHt9XHJcblxyXG4vKipcclxuICpcclxuICogQGRlcHJlY2F0ZWQgc2luY2UgdmVyc2lvbiAwLjkuMFxyXG4gKi9cclxuZXhwb3J0IGZ1bmN0aW9uIFRlbmFudE1hbmFnZW1lbnRQcm92aWRlcnMoKTogUHJvdmlkZXJbXSB7XHJcbiAgcmV0dXJuIFtdO1xyXG59XHJcbiJdfQ==