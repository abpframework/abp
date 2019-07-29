/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { AuthGuard, DynamicLayoutComponent, PermissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TenantsResolver } from './resolvers/tenants.resolver';
import { TenantsComponent } from './components/tenants/tenants.component';
const ɵ0 = { requiredPolicy: 'AbpTenantManagement.Tenants' };
/** @type {?} */
const routes = [
    { path: '', redirectTo: 'tenants', pathMatch: 'full' },
    {
        path: 'tenants',
        component: DynamicLayoutComponent,
        canActivate: [AuthGuard, PermissionGuard],
        data: ɵ0,
        children: [{ path: '', component: TenantsComponent, resolve: [TenantsResolver] }],
    },
];
export class TenantManagementRoutingModule {
}
TenantManagementRoutingModule.decorators = [
    { type: NgModule, args: [{
                imports: [RouterModule.forChild(routes)],
                exports: [RouterModule],
                providers: [TenantsResolver],
            },] }
];
export { ɵ0 };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQtcm91dGluZy5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3RlbmFudC1tYW5hZ2VtZW50LXJvdXRpbmcubW9kdWxlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLHNCQUFzQixFQUFFLGVBQWUsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNsRixPQUFPLEVBQUUsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3pDLE9BQU8sRUFBRSxZQUFZLEVBQVUsTUFBTSxpQkFBaUIsQ0FBQztBQUN2RCxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sOEJBQThCLENBQUM7QUFDL0QsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sd0NBQXdDLENBQUM7V0FRaEUsRUFBRSxjQUFjLEVBQUUsNkJBQTZCLEVBQUU7O01BTnJELE1BQU0sR0FBVztJQUNyQixFQUFFLElBQUksRUFBRSxFQUFFLEVBQUUsVUFBVSxFQUFFLFNBQVMsRUFBRSxTQUFTLEVBQUUsTUFBTSxFQUFFO0lBQ3REO1FBQ0UsSUFBSSxFQUFFLFNBQVM7UUFDZixTQUFTLEVBQUUsc0JBQXNCO1FBQ2pDLFdBQVcsRUFBRSxDQUFDLFNBQVMsRUFBRSxlQUFlLENBQUM7UUFDekMsSUFBSSxJQUFtRDtRQUN2RCxRQUFRLEVBQUUsQ0FBQyxFQUFFLElBQUksRUFBRSxFQUFFLEVBQUUsU0FBUyxFQUFFLGdCQUFnQixFQUFFLE9BQU8sRUFBRSxDQUFDLGVBQWUsQ0FBQyxFQUFFLENBQUM7S0FDbEY7Q0FDRjtBQU9ELE1BQU0sT0FBTyw2QkFBNkI7OztZQUx6QyxRQUFRLFNBQUM7Z0JBQ1IsT0FBTyxFQUFFLENBQUMsWUFBWSxDQUFDLFFBQVEsQ0FBQyxNQUFNLENBQUMsQ0FBQztnQkFDeEMsT0FBTyxFQUFFLENBQUMsWUFBWSxDQUFDO2dCQUN2QixTQUFTLEVBQUUsQ0FBQyxlQUFlLENBQUM7YUFDN0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBdXRoR3VhcmQsIER5bmFtaWNMYXlvdXRDb21wb25lbnQsIFBlcm1pc3Npb25HdWFyZCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBOZ01vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgUm91dGVyTW9kdWxlLCBSb3V0ZXMgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgVGVuYW50c1Jlc29sdmVyIH0gZnJvbSAnLi9yZXNvbHZlcnMvdGVuYW50cy5yZXNvbHZlcic7XG5pbXBvcnQgeyBUZW5hbnRzQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3RlbmFudHMvdGVuYW50cy5jb21wb25lbnQnO1xuXG5jb25zdCByb3V0ZXM6IFJvdXRlcyA9IFtcbiAgeyBwYXRoOiAnJywgcmVkaXJlY3RUbzogJ3RlbmFudHMnLCBwYXRoTWF0Y2g6ICdmdWxsJyB9LFxuICB7XG4gICAgcGF0aDogJ3RlbmFudHMnLFxuICAgIGNvbXBvbmVudDogRHluYW1pY0xheW91dENvbXBvbmVudCxcbiAgICBjYW5BY3RpdmF0ZTogW0F1dGhHdWFyZCwgUGVybWlzc2lvbkd1YXJkXSxcbiAgICBkYXRhOiB7IHJlcXVpcmVkUG9saWN5OiAnQWJwVGVuYW50TWFuYWdlbWVudC5UZW5hbnRzJyB9LFxuICAgIGNoaWxkcmVuOiBbeyBwYXRoOiAnJywgY29tcG9uZW50OiBUZW5hbnRzQ29tcG9uZW50LCByZXNvbHZlOiBbVGVuYW50c1Jlc29sdmVyXSB9XSxcbiAgfSxcbl07XG5cbkBOZ01vZHVsZSh7XG4gIGltcG9ydHM6IFtSb3V0ZXJNb2R1bGUuZm9yQ2hpbGQocm91dGVzKV0sXG4gIGV4cG9ydHM6IFtSb3V0ZXJNb2R1bGVdLFxuICBwcm92aWRlcnM6IFtUZW5hbnRzUmVzb2x2ZXJdLFxufSlcbmV4cG9ydCBjbGFzcyBUZW5hbnRNYW5hZ2VtZW50Um91dGluZ01vZHVsZSB7fVxuIl19