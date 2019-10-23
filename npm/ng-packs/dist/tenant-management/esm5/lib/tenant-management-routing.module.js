/**
 * @fileoverview added by tsickle
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
var TenantManagementRoutingModule = /** @class */ (function() {
  function TenantManagementRoutingModule() {}
  TenantManagementRoutingModule.decorators = [
    {
      type: NgModule,
      args: [
        {
          imports: [RouterModule.forChild(routes)],
          exports: [RouterModule],
        },
      ],
    },
  ];
  return TenantManagementRoutingModule;
})();
export { TenantManagementRoutingModule };
export { ɵ0 };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQtcm91dGluZy5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3RlbmFudC1tYW5hZ2VtZW50LXJvdXRpbmcubW9kdWxlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLHNCQUFzQixFQUFFLGVBQWUsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNsRixPQUFPLEVBQUUsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3pDLE9BQU8sRUFBRSxZQUFZLEVBQVUsTUFBTSxpQkFBaUIsQ0FBQztBQUN2RCxPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSx3Q0FBd0MsQ0FBQztTQVFoRSxFQUFFLGNBQWMsRUFBRSw2QkFBNkIsRUFBRTs7SUFOckQsTUFBTSxHQUFXO0lBQ3JCLEVBQUUsSUFBSSxFQUFFLEVBQUUsRUFBRSxVQUFVLEVBQUUsU0FBUyxFQUFFLFNBQVMsRUFBRSxNQUFNLEVBQUU7SUFDdEQ7UUFDRSxJQUFJLEVBQUUsU0FBUztRQUNmLFNBQVMsRUFBRSxzQkFBc0I7UUFDakMsV0FBVyxFQUFFLENBQUMsU0FBUyxFQUFFLGVBQWUsQ0FBQztRQUN6QyxJQUFJLElBQW1EO1FBQ3ZELFFBQVEsRUFBRSxDQUFDLEVBQUUsSUFBSSxFQUFFLEVBQUUsRUFBRSxTQUFTLEVBQUUsZ0JBQWdCLEVBQUUsQ0FBQztLQUN0RDtDQUNGO0FBRUQ7SUFBQTtJQUk0QyxDQUFDOztnQkFKNUMsUUFBUSxTQUFDO29CQUNSLE9BQU8sRUFBRSxDQUFDLFlBQVksQ0FBQyxRQUFRLENBQUMsTUFBTSxDQUFDLENBQUM7b0JBQ3hDLE9BQU8sRUFBRSxDQUFDLFlBQVksQ0FBQztpQkFDeEI7O0lBQzJDLG9DQUFDO0NBQUEsQUFKN0MsSUFJNkM7U0FBaEMsNkJBQTZCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQXV0aEd1YXJkLCBEeW5hbWljTGF5b3V0Q29tcG9uZW50LCBQZXJtaXNzaW9uR3VhcmQgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJvdXRlck1vZHVsZSwgUm91dGVzIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IFRlbmFudHNDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvdGVuYW50cy90ZW5hbnRzLmNvbXBvbmVudCc7XG5cbmNvbnN0IHJvdXRlczogUm91dGVzID0gW1xuICB7IHBhdGg6ICcnLCByZWRpcmVjdFRvOiAndGVuYW50cycsIHBhdGhNYXRjaDogJ2Z1bGwnIH0sXG4gIHtcbiAgICBwYXRoOiAndGVuYW50cycsXG4gICAgY29tcG9uZW50OiBEeW5hbWljTGF5b3V0Q29tcG9uZW50LFxuICAgIGNhbkFjdGl2YXRlOiBbQXV0aEd1YXJkLCBQZXJtaXNzaW9uR3VhcmRdLFxuICAgIGRhdGE6IHsgcmVxdWlyZWRQb2xpY3k6ICdBYnBUZW5hbnRNYW5hZ2VtZW50LlRlbmFudHMnIH0sXG4gICAgY2hpbGRyZW46IFt7IHBhdGg6ICcnLCBjb21wb25lbnQ6IFRlbmFudHNDb21wb25lbnQgfV0sXG4gIH0sXG5dO1xuXG5ATmdNb2R1bGUoe1xuICBpbXBvcnRzOiBbUm91dGVyTW9kdWxlLmZvckNoaWxkKHJvdXRlcyldLFxuICBleHBvcnRzOiBbUm91dGVyTW9kdWxlXSxcbn0pXG5leHBvcnQgY2xhc3MgVGVuYW50TWFuYWdlbWVudFJvdXRpbmdNb2R1bGUge31cbiJdfQ==
