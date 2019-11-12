/**
 * @fileoverview added by tsickle
 * Generated from: lib/tenant-management-routing.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { AuthGuard, DynamicLayoutComponent, PermissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TenantsComponent } from './components/tenants/tenants.component';
var ɵ0 = { requiredPolicy: 'AbpTenantManagement.Tenants' };
/** @type {?} */
var routes = [
    { path: '', redirectTo: 'tenants', pathMatch: 'full' },
    {
        path: 'tenants',
        component: DynamicLayoutComponent,
        canActivate: [AuthGuard, PermissionGuard],
        data: ɵ0,
        children: [{ path: '', component: TenantsComponent }],
    },
];
var TenantManagementRoutingModule = /** @class */ (function () {
    function TenantManagementRoutingModule() {
    }
    TenantManagementRoutingModule.decorators = [
        { type: NgModule, args: [{
                    imports: [RouterModule.forChild(routes)],
                    exports: [RouterModule],
                },] }
    ];
    return TenantManagementRoutingModule;
}());
export { TenantManagementRoutingModule };
export { ɵ0 };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQtcm91dGluZy5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3RlbmFudC1tYW5hZ2VtZW50LXJvdXRpbmcubW9kdWxlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxzQkFBc0IsRUFBRSxlQUFlLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDbEYsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN6QyxPQUFPLEVBQUUsWUFBWSxFQUFVLE1BQU0saUJBQWlCLENBQUM7QUFDdkQsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sd0NBQXdDLENBQUM7U0FRaEUsRUFBRSxjQUFjLEVBQUUsNkJBQTZCLEVBQUU7O0lBTnJELE1BQU0sR0FBVztJQUNyQixFQUFFLElBQUksRUFBRSxFQUFFLEVBQUUsVUFBVSxFQUFFLFNBQVMsRUFBRSxTQUFTLEVBQUUsTUFBTSxFQUFFO0lBQ3REO1FBQ0UsSUFBSSxFQUFFLFNBQVM7UUFDZixTQUFTLEVBQUUsc0JBQXNCO1FBQ2pDLFdBQVcsRUFBRSxDQUFDLFNBQVMsRUFBRSxlQUFlLENBQUM7UUFDekMsSUFBSSxJQUFtRDtRQUN2RCxRQUFRLEVBQUUsQ0FBQyxFQUFFLElBQUksRUFBRSxFQUFFLEVBQUUsU0FBUyxFQUFFLGdCQUFnQixFQUFFLENBQUM7S0FDdEQ7Q0FDRjtBQUVEO0lBQUE7SUFJNEMsQ0FBQzs7Z0JBSjVDLFFBQVEsU0FBQztvQkFDUixPQUFPLEVBQUUsQ0FBQyxZQUFZLENBQUMsUUFBUSxDQUFDLE1BQU0sQ0FBQyxDQUFDO29CQUN4QyxPQUFPLEVBQUUsQ0FBQyxZQUFZLENBQUM7aUJBQ3hCOztJQUMyQyxvQ0FBQztDQUFBLEFBSjdDLElBSTZDO1NBQWhDLDZCQUE2QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEF1dGhHdWFyZCwgRHluYW1pY0xheW91dENvbXBvbmVudCwgUGVybWlzc2lvbkd1YXJkIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgUm91dGVyTW9kdWxlLCBSb3V0ZXMgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xyXG5pbXBvcnQgeyBUZW5hbnRzQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3RlbmFudHMvdGVuYW50cy5jb21wb25lbnQnO1xyXG5cclxuY29uc3Qgcm91dGVzOiBSb3V0ZXMgPSBbXHJcbiAgeyBwYXRoOiAnJywgcmVkaXJlY3RUbzogJ3RlbmFudHMnLCBwYXRoTWF0Y2g6ICdmdWxsJyB9LFxyXG4gIHtcclxuICAgIHBhdGg6ICd0ZW5hbnRzJyxcclxuICAgIGNvbXBvbmVudDogRHluYW1pY0xheW91dENvbXBvbmVudCxcclxuICAgIGNhbkFjdGl2YXRlOiBbQXV0aEd1YXJkLCBQZXJtaXNzaW9uR3VhcmRdLFxyXG4gICAgZGF0YTogeyByZXF1aXJlZFBvbGljeTogJ0FicFRlbmFudE1hbmFnZW1lbnQuVGVuYW50cycgfSxcclxuICAgIGNoaWxkcmVuOiBbeyBwYXRoOiAnJywgY29tcG9uZW50OiBUZW5hbnRzQ29tcG9uZW50IH1dLFxyXG4gIH0sXHJcbl07XHJcblxyXG5ATmdNb2R1bGUoe1xyXG4gIGltcG9ydHM6IFtSb3V0ZXJNb2R1bGUuZm9yQ2hpbGQocm91dGVzKV0sXHJcbiAgZXhwb3J0czogW1JvdXRlck1vZHVsZV0sXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBUZW5hbnRNYW5hZ2VtZW50Um91dGluZ01vZHVsZSB7fVxyXG4iXX0=