/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQtcm91dGluZy5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3RlbmFudC1tYW5hZ2VtZW50LXJvdXRpbmcubW9kdWxlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLHNCQUFzQixFQUFFLGVBQWUsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNsRixPQUFPLEVBQUUsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3pDLE9BQU8sRUFBRSxZQUFZLEVBQVUsTUFBTSxpQkFBaUIsQ0FBQztBQUN2RCxPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSx3Q0FBd0MsQ0FBQztXQVFoRSxFQUFFLGNBQWMsRUFBRSw2QkFBNkIsRUFBRTs7TUFOckQsTUFBTSxHQUFXO0lBQ3JCLEVBQUUsSUFBSSxFQUFFLEVBQUUsRUFBRSxVQUFVLEVBQUUsU0FBUyxFQUFFLFNBQVMsRUFBRSxNQUFNLEVBQUU7SUFDdEQ7UUFDRSxJQUFJLEVBQUUsU0FBUztRQUNmLFNBQVMsRUFBRSxzQkFBc0I7UUFDakMsV0FBVyxFQUFFLENBQUMsU0FBUyxFQUFFLGVBQWUsQ0FBQztRQUN6QyxJQUFJLElBQW1EO1FBQ3ZELFFBQVEsRUFBRSxDQUFDLEVBQUUsSUFBSSxFQUFFLEVBQUUsRUFBRSxTQUFTLEVBQUUsZ0JBQWdCLEVBQUUsQ0FBQztLQUN0RDtDQUNGO0FBTUQsTUFBTSxPQUFPLDZCQUE2Qjs7O1lBSnpDLFFBQVEsU0FBQztnQkFDUixPQUFPLEVBQUUsQ0FBQyxZQUFZLENBQUMsUUFBUSxDQUFDLE1BQU0sQ0FBQyxDQUFDO2dCQUN4QyxPQUFPLEVBQUUsQ0FBQyxZQUFZLENBQUM7YUFDeEIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBdXRoR3VhcmQsIER5bmFtaWNMYXlvdXRDb21wb25lbnQsIFBlcm1pc3Npb25HdWFyZCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcbmltcG9ydCB7IE5nTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFJvdXRlck1vZHVsZSwgUm91dGVzIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcclxuaW1wb3J0IHsgVGVuYW50c0NvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy90ZW5hbnRzL3RlbmFudHMuY29tcG9uZW50JztcclxuXHJcbmNvbnN0IHJvdXRlczogUm91dGVzID0gW1xyXG4gIHsgcGF0aDogJycsIHJlZGlyZWN0VG86ICd0ZW5hbnRzJywgcGF0aE1hdGNoOiAnZnVsbCcgfSxcclxuICB7XHJcbiAgICBwYXRoOiAndGVuYW50cycsXHJcbiAgICBjb21wb25lbnQ6IER5bmFtaWNMYXlvdXRDb21wb25lbnQsXHJcbiAgICBjYW5BY3RpdmF0ZTogW0F1dGhHdWFyZCwgUGVybWlzc2lvbkd1YXJkXSxcclxuICAgIGRhdGE6IHsgcmVxdWlyZWRQb2xpY3k6ICdBYnBUZW5hbnRNYW5hZ2VtZW50LlRlbmFudHMnIH0sXHJcbiAgICBjaGlsZHJlbjogW3sgcGF0aDogJycsIGNvbXBvbmVudDogVGVuYW50c0NvbXBvbmVudCB9XSxcclxuICB9LFxyXG5dO1xyXG5cclxuQE5nTW9kdWxlKHtcclxuICBpbXBvcnRzOiBbUm91dGVyTW9kdWxlLmZvckNoaWxkKHJvdXRlcyldLFxyXG4gIGV4cG9ydHM6IFtSb3V0ZXJNb2R1bGVdLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgVGVuYW50TWFuYWdlbWVudFJvdXRpbmdNb2R1bGUge31cclxuIl19