/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { RolesComponent } from './components/roles/roles.component';
import { RoleResolver } from './resolvers/roles.resolver';
import { DynamicLayoutComponent, AuthGuard, PermissionGuard } from '@abp/ng.core';
import { UsersComponent } from './components/users/users.component';
import { UserResolver } from './resolvers/users.resolver';
var ɵ0 = { requiredPolicy: 'AbpIdentity.Roles' },
  ɵ1 = { requiredPolicy: 'AbpIdentity.Users' };
/** @type {?} */
var routes = [
  { path: '', redirectTo: 'roles', pathMatch: 'full' },
  {
    path: '',
    component: DynamicLayoutComponent,
    canActivate: [AuthGuard, PermissionGuard],
    children: [
      {
        path: 'roles',
        component: RolesComponent,
        resolve: [RoleResolver],
        data: ɵ0,
      },
      {
        path: 'users',
        component: UsersComponent,
        data: ɵ1,
        resolve: [RoleResolver, UserResolver],
      },
    ],
  },
];
var IdentityRoutingModule = /** @class */ (function() {
  function IdentityRoutingModule() {}
  IdentityRoutingModule.decorators = [
    {
      type: NgModule,
      args: [
        {
          imports: [RouterModule.forChild(routes)],
          exports: [RouterModule],
          providers: [RoleResolver, UserResolver],
        },
      ],
    },
  ];
  return IdentityRoutingModule;
})();
export { IdentityRoutingModule };
export { ɵ0, ɵ1 };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHktcm91dGluZy5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL2lkZW50aXR5LXJvdXRpbmcubW9kdWxlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3pDLE9BQU8sRUFBVSxZQUFZLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUN2RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDcEUsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBQzFELE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxTQUFTLEVBQUUsZUFBZSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ2xGLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztBQUNwRSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sNEJBQTRCLENBQUM7U0FhNUMsRUFBRSxjQUFjLEVBQUUsbUJBQW1CLEVBQUUsT0FLdkMsRUFBRSxjQUFjLEVBQUUsbUJBQW1CLEVBQUU7O0lBaEIvQyxNQUFNLEdBQVc7SUFDckIsRUFBRSxJQUFJLEVBQUUsRUFBRSxFQUFFLFVBQVUsRUFBRSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sRUFBRTtJQUNwRDtRQUNFLElBQUksRUFBRSxFQUFFO1FBQ1IsU0FBUyxFQUFFLHNCQUFzQjtRQUNqQyxXQUFXLEVBQUUsQ0FBQyxTQUFTLEVBQUUsZUFBZSxDQUFDO1FBQ3pDLFFBQVEsRUFBRTtZQUNSO2dCQUNFLElBQUksRUFBRSxPQUFPO2dCQUNiLFNBQVMsRUFBRSxjQUFjO2dCQUN6QixPQUFPLEVBQUUsQ0FBQyxZQUFZLENBQUM7Z0JBQ3ZCLElBQUksSUFBeUM7YUFDOUM7WUFDRDtnQkFDRSxJQUFJLEVBQUUsT0FBTztnQkFDYixTQUFTLEVBQUUsY0FBYztnQkFDekIsSUFBSSxJQUF5QztnQkFDN0MsT0FBTyxFQUFFLENBQUMsWUFBWSxFQUFFLFlBQVksQ0FBQzthQUN0QztTQUNGO0tBQ0Y7Q0FDRjtBQUVEO0lBQUE7SUFLb0MsQ0FBQzs7Z0JBTHBDLFFBQVEsU0FBQztvQkFDUixPQUFPLEVBQUUsQ0FBQyxZQUFZLENBQUMsUUFBUSxDQUFDLE1BQU0sQ0FBQyxDQUFDO29CQUN4QyxPQUFPLEVBQUUsQ0FBQyxZQUFZLENBQUM7b0JBQ3ZCLFNBQVMsRUFBRSxDQUFDLFlBQVksRUFBRSxZQUFZLENBQUM7aUJBQ3hDOztJQUNtQyw0QkFBQztDQUFBLEFBTHJDLElBS3FDO1NBQXhCLHFCQUFxQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IE5nTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBSb3V0ZXMsIFJvdXRlck1vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBSb2xlc0NvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9yb2xlcy9yb2xlcy5jb21wb25lbnQnO1xuaW1wb3J0IHsgUm9sZVJlc29sdmVyIH0gZnJvbSAnLi9yZXNvbHZlcnMvcm9sZXMucmVzb2x2ZXInO1xuaW1wb3J0IHsgRHluYW1pY0xheW91dENvbXBvbmVudCwgQXV0aEd1YXJkLCBQZXJtaXNzaW9uR3VhcmQgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgVXNlcnNDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvdXNlcnMvdXNlcnMuY29tcG9uZW50JztcbmltcG9ydCB7IFVzZXJSZXNvbHZlciB9IGZyb20gJy4vcmVzb2x2ZXJzL3VzZXJzLnJlc29sdmVyJztcblxuY29uc3Qgcm91dGVzOiBSb3V0ZXMgPSBbXG4gIHsgcGF0aDogJycsIHJlZGlyZWN0VG86ICdyb2xlcycsIHBhdGhNYXRjaDogJ2Z1bGwnIH0sXG4gIHtcbiAgICBwYXRoOiAnJyxcbiAgICBjb21wb25lbnQ6IER5bmFtaWNMYXlvdXRDb21wb25lbnQsXG4gICAgY2FuQWN0aXZhdGU6IFtBdXRoR3VhcmQsIFBlcm1pc3Npb25HdWFyZF0sXG4gICAgY2hpbGRyZW46IFtcbiAgICAgIHtcbiAgICAgICAgcGF0aDogJ3JvbGVzJyxcbiAgICAgICAgY29tcG9uZW50OiBSb2xlc0NvbXBvbmVudCxcbiAgICAgICAgcmVzb2x2ZTogW1JvbGVSZXNvbHZlcl0sXG4gICAgICAgIGRhdGE6IHsgcmVxdWlyZWRQb2xpY3k6ICdBYnBJZGVudGl0eS5Sb2xlcycgfSxcbiAgICAgIH0sXG4gICAgICB7XG4gICAgICAgIHBhdGg6ICd1c2VycycsXG4gICAgICAgIGNvbXBvbmVudDogVXNlcnNDb21wb25lbnQsXG4gICAgICAgIGRhdGE6IHsgcmVxdWlyZWRQb2xpY3k6ICdBYnBJZGVudGl0eS5Vc2VycycgfSxcbiAgICAgICAgcmVzb2x2ZTogW1JvbGVSZXNvbHZlciwgVXNlclJlc29sdmVyXSxcbiAgICAgIH0sXG4gICAgXSxcbiAgfSxcbl07XG5cbkBOZ01vZHVsZSh7XG4gIGltcG9ydHM6IFtSb3V0ZXJNb2R1bGUuZm9yQ2hpbGQocm91dGVzKV0sXG4gIGV4cG9ydHM6IFtSb3V0ZXJNb2R1bGVdLFxuICBwcm92aWRlcnM6IFtSb2xlUmVzb2x2ZXIsIFVzZXJSZXNvbHZlcl0sXG59KVxuZXhwb3J0IGNsYXNzIElkZW50aXR5Um91dGluZ01vZHVsZSB7fVxuIl19
