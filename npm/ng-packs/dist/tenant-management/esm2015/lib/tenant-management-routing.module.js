/**
 * @fileoverview added by tsickle
 * Generated from: lib/tenant-management-routing.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { AuthGuard, DynamicLayoutComponent, PermissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
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
        children: [{ path: '', component: TenantsComponent }],
    },
];
export class TenantManagementRoutingModule {
}
TenantManagementRoutingModule.decorators = [
    { type: NgModule, args: [{
                imports: [RouterModule.forChild(routes)],
                exports: [RouterModule],
            },] }
];
export { ɵ0 };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQtcm91dGluZy5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3RlbmFudC1tYW5hZ2VtZW50LXJvdXRpbmcubW9kdWxlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxzQkFBc0IsRUFBRSxlQUFlLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDbEYsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN6QyxPQUFPLEVBQUUsWUFBWSxFQUFVLE1BQU0saUJBQWlCLENBQUM7QUFDdkQsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sd0NBQXdDLENBQUM7V0FRaEUsRUFBRSxjQUFjLEVBQUUsNkJBQTZCLEVBQUU7O01BTnJELE1BQU0sR0FBVztJQUNyQixFQUFFLElBQUksRUFBRSxFQUFFLEVBQUUsVUFBVSxFQUFFLFNBQVMsRUFBRSxTQUFTLEVBQUUsTUFBTSxFQUFFO0lBQ3REO1FBQ0UsSUFBSSxFQUFFLFNBQVM7UUFDZixTQUFTLEVBQUUsc0JBQXNCO1FBQ2pDLFdBQVcsRUFBRSxDQUFDLFNBQVMsRUFBRSxlQUFlLENBQUM7UUFDekMsSUFBSSxJQUFtRDtRQUN2RCxRQUFRLEVBQUUsQ0FBQyxFQUFFLElBQUksRUFBRSxFQUFFLEVBQUUsU0FBUyxFQUFFLGdCQUFnQixFQUFFLENBQUM7S0FDdEQ7Q0FDRjtBQU1ELE1BQU0sT0FBTyw2QkFBNkI7OztZQUp6QyxRQUFRLFNBQUM7Z0JBQ1IsT0FBTyxFQUFFLENBQUMsWUFBWSxDQUFDLFFBQVEsQ0FBQyxNQUFNLENBQUMsQ0FBQztnQkFDeEMsT0FBTyxFQUFFLENBQUMsWUFBWSxDQUFDO2FBQ3hCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQXV0aEd1YXJkLCBEeW5hbWljTGF5b3V0Q29tcG9uZW50LCBQZXJtaXNzaW9uR3VhcmQgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJvdXRlck1vZHVsZSwgUm91dGVzIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IFRlbmFudHNDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvdGVuYW50cy90ZW5hbnRzLmNvbXBvbmVudCc7XG5cbmNvbnN0IHJvdXRlczogUm91dGVzID0gW1xuICB7IHBhdGg6ICcnLCByZWRpcmVjdFRvOiAndGVuYW50cycsIHBhdGhNYXRjaDogJ2Z1bGwnIH0sXG4gIHtcbiAgICBwYXRoOiAndGVuYW50cycsXG4gICAgY29tcG9uZW50OiBEeW5hbWljTGF5b3V0Q29tcG9uZW50LFxuICAgIGNhbkFjdGl2YXRlOiBbQXV0aEd1YXJkLCBQZXJtaXNzaW9uR3VhcmRdLFxuICAgIGRhdGE6IHsgcmVxdWlyZWRQb2xpY3k6ICdBYnBUZW5hbnRNYW5hZ2VtZW50LlRlbmFudHMnIH0sXG4gICAgY2hpbGRyZW46IFt7IHBhdGg6ICcnLCBjb21wb25lbnQ6IFRlbmFudHNDb21wb25lbnQgfV0sXG4gIH0sXG5dO1xuXG5ATmdNb2R1bGUoe1xuICBpbXBvcnRzOiBbUm91dGVyTW9kdWxlLmZvckNoaWxkKHJvdXRlcyldLFxuICBleHBvcnRzOiBbUm91dGVyTW9kdWxlXSxcbn0pXG5leHBvcnQgY2xhc3MgVGVuYW50TWFuYWdlbWVudFJvdXRpbmdNb2R1bGUge31cbiJdfQ==