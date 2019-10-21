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
const ɵ0 = { requiredPolicy: 'AbpIdentity.Roles' },
  ɵ1 = { requiredPolicy: 'AbpIdentity.Users' };
/** @type {?} */
const routes = [
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
export class IdentityRoutingModule {}
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
export { ɵ0, ɵ1 };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHktcm91dGluZy5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL2lkZW50aXR5LXJvdXRpbmcubW9kdWxlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3pDLE9BQU8sRUFBVSxZQUFZLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUN2RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDcEUsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBQzFELE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxTQUFTLEVBQUUsZUFBZSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ2xGLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztBQUNwRSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sNEJBQTRCLENBQUM7V0FhNUMsRUFBRSxjQUFjLEVBQUUsbUJBQW1CLEVBQUUsT0FLdkMsRUFBRSxjQUFjLEVBQUUsbUJBQW1CLEVBQUU7O01BaEIvQyxNQUFNLEdBQVc7SUFDckIsRUFBRSxJQUFJLEVBQUUsRUFBRSxFQUFFLFVBQVUsRUFBRSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sRUFBRTtJQUNwRDtRQUNFLElBQUksRUFBRSxFQUFFO1FBQ1IsU0FBUyxFQUFFLHNCQUFzQjtRQUNqQyxXQUFXLEVBQUUsQ0FBQyxTQUFTLEVBQUUsZUFBZSxDQUFDO1FBQ3pDLFFBQVEsRUFBRTtZQUNSO2dCQUNFLElBQUksRUFBRSxPQUFPO2dCQUNiLFNBQVMsRUFBRSxjQUFjO2dCQUN6QixPQUFPLEVBQUUsQ0FBQyxZQUFZLENBQUM7Z0JBQ3ZCLElBQUksSUFBeUM7YUFDOUM7WUFDRDtnQkFDRSxJQUFJLEVBQUUsT0FBTztnQkFDYixTQUFTLEVBQUUsY0FBYztnQkFDekIsSUFBSSxJQUF5QztnQkFDN0MsT0FBTyxFQUFFLENBQUMsWUFBWSxFQUFFLFlBQVksQ0FBQzthQUN0QztTQUNGO0tBQ0Y7Q0FDRjtBQU9ELE1BQU0sT0FBTyxxQkFBcUI7OztZQUxqQyxRQUFRLFNBQUM7Z0JBQ1IsT0FBTyxFQUFFLENBQUMsWUFBWSxDQUFDLFFBQVEsQ0FBQyxNQUFNLENBQUMsQ0FBQztnQkFDeEMsT0FBTyxFQUFFLENBQUMsWUFBWSxDQUFDO2dCQUN2QixTQUFTLEVBQUUsQ0FBQyxZQUFZLEVBQUUsWUFBWSxDQUFDO2FBQ3hDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJvdXRlcywgUm91dGVyTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IFJvbGVzQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3JvbGVzL3JvbGVzLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBSb2xlUmVzb2x2ZXIgfSBmcm9tICcuL3Jlc29sdmVycy9yb2xlcy5yZXNvbHZlcic7XG5pbXBvcnQgeyBEeW5hbWljTGF5b3V0Q29tcG9uZW50LCBBdXRoR3VhcmQsIFBlcm1pc3Npb25HdWFyZCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBVc2Vyc0NvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy91c2Vycy91c2Vycy5jb21wb25lbnQnO1xuaW1wb3J0IHsgVXNlclJlc29sdmVyIH0gZnJvbSAnLi9yZXNvbHZlcnMvdXNlcnMucmVzb2x2ZXInO1xuXG5jb25zdCByb3V0ZXM6IFJvdXRlcyA9IFtcbiAgeyBwYXRoOiAnJywgcmVkaXJlY3RUbzogJ3JvbGVzJywgcGF0aE1hdGNoOiAnZnVsbCcgfSxcbiAge1xuICAgIHBhdGg6ICcnLFxuICAgIGNvbXBvbmVudDogRHluYW1pY0xheW91dENvbXBvbmVudCxcbiAgICBjYW5BY3RpdmF0ZTogW0F1dGhHdWFyZCwgUGVybWlzc2lvbkd1YXJkXSxcbiAgICBjaGlsZHJlbjogW1xuICAgICAge1xuICAgICAgICBwYXRoOiAncm9sZXMnLFxuICAgICAgICBjb21wb25lbnQ6IFJvbGVzQ29tcG9uZW50LFxuICAgICAgICByZXNvbHZlOiBbUm9sZVJlc29sdmVyXSxcbiAgICAgICAgZGF0YTogeyByZXF1aXJlZFBvbGljeTogJ0FicElkZW50aXR5LlJvbGVzJyB9LFxuICAgICAgfSxcbiAgICAgIHtcbiAgICAgICAgcGF0aDogJ3VzZXJzJyxcbiAgICAgICAgY29tcG9uZW50OiBVc2Vyc0NvbXBvbmVudCxcbiAgICAgICAgZGF0YTogeyByZXF1aXJlZFBvbGljeTogJ0FicElkZW50aXR5LlVzZXJzJyB9LFxuICAgICAgICByZXNvbHZlOiBbUm9sZVJlc29sdmVyLCBVc2VyUmVzb2x2ZXJdLFxuICAgICAgfSxcbiAgICBdLFxuICB9LFxuXTtcblxuQE5nTW9kdWxlKHtcbiAgaW1wb3J0czogW1JvdXRlck1vZHVsZS5mb3JDaGlsZChyb3V0ZXMpXSxcbiAgZXhwb3J0czogW1JvdXRlck1vZHVsZV0sXG4gIHByb3ZpZGVyczogW1JvbGVSZXNvbHZlciwgVXNlclJlc29sdmVyXSxcbn0pXG5leHBvcnQgY2xhc3MgSWRlbnRpdHlSb3V0aW5nTW9kdWxlIHt9XG4iXX0=
