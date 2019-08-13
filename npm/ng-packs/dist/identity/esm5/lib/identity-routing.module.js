/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { RolesComponent } from './components/roles/roles.component';
import { RoleResolver } from './resolvers/roles.resolver';
import { DynamicLayoutComponent, AuthGuard, PermissionGuard } from '@abp/ng.core';
import { UsersComponent } from './components/users/users.component';
import { UserResolver } from './resolvers/users.resolver';
var ɵ0 = { requiredPolicy: 'AbpIdentity.Roles' }, ɵ1 = { requiredPolicy: 'AbpIdentity.Users' };
/** @type {?} */
var routes = [
    { path: '', redirectTo: 'roles', pathMatch: 'full' },
    {
        path: 'roles',
        component: DynamicLayoutComponent,
        canActivate: [AuthGuard, PermissionGuard],
        data: ɵ0,
        children: [{ path: '', component: RolesComponent, resolve: [RoleResolver] }],
    },
    {
        path: 'users',
        component: DynamicLayoutComponent,
        canActivate: [AuthGuard, PermissionGuard],
        data: ɵ1,
        children: [
            {
                path: '',
                component: UsersComponent,
                resolve: [RoleResolver, UserResolver],
            },
        ],
    },
];
var IdentityRoutingModule = /** @class */ (function () {
    function IdentityRoutingModule() {
    }
    IdentityRoutingModule.decorators = [
        { type: NgModule, args: [{
                    imports: [RouterModule.forChild(routes)],
                    exports: [RouterModule],
                    providers: [RoleResolver, UserResolver],
                },] }
    ];
    return IdentityRoutingModule;
}());
export { IdentityRoutingModule };
export { ɵ0, ɵ1 };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHktcm91dGluZy5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL2lkZW50aXR5LXJvdXRpbmcubW9kdWxlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3pDLE9BQU8sRUFBVSxZQUFZLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUN2RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDcEUsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBQzFELE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxTQUFTLEVBQUUsZUFBZSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ2xGLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztBQUNwRSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sNEJBQTRCLENBQUM7U0FRaEQsRUFBRSxjQUFjLEVBQUUsbUJBQW1CLEVBQUUsT0FPdkMsRUFBRSxjQUFjLEVBQUUsbUJBQW1CLEVBQUU7O0lBYjNDLE1BQU0sR0FBVztJQUNyQixFQUFFLElBQUksRUFBRSxFQUFFLEVBQUUsVUFBVSxFQUFFLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxFQUFFO0lBQ3BEO1FBQ0UsSUFBSSxFQUFFLE9BQU87UUFDYixTQUFTLEVBQUUsc0JBQXNCO1FBQ2pDLFdBQVcsRUFBRSxDQUFDLFNBQVMsRUFBRSxlQUFlLENBQUM7UUFDekMsSUFBSSxJQUF5QztRQUM3QyxRQUFRLEVBQUUsQ0FBQyxFQUFFLElBQUksRUFBRSxFQUFFLEVBQUUsU0FBUyxFQUFFLGNBQWMsRUFBRSxPQUFPLEVBQUUsQ0FBQyxZQUFZLENBQUMsRUFBRSxDQUFDO0tBQzdFO0lBQ0Q7UUFDRSxJQUFJLEVBQUUsT0FBTztRQUNiLFNBQVMsRUFBRSxzQkFBc0I7UUFDakMsV0FBVyxFQUFFLENBQUMsU0FBUyxFQUFFLGVBQWUsQ0FBQztRQUN6QyxJQUFJLElBQXlDO1FBQzdDLFFBQVEsRUFBRTtZQUNSO2dCQUNFLElBQUksRUFBRSxFQUFFO2dCQUNSLFNBQVMsRUFBRSxjQUFjO2dCQUN6QixPQUFPLEVBQUUsQ0FBQyxZQUFZLEVBQUUsWUFBWSxDQUFDO2FBQ3RDO1NBQ0Y7S0FDRjtDQUNGO0FBRUQ7SUFBQTtJQUtvQyxDQUFDOztnQkFMcEMsUUFBUSxTQUFDO29CQUNSLE9BQU8sRUFBRSxDQUFDLFlBQVksQ0FBQyxRQUFRLENBQUMsTUFBTSxDQUFDLENBQUM7b0JBQ3hDLE9BQU8sRUFBRSxDQUFDLFlBQVksQ0FBQztvQkFDdkIsU0FBUyxFQUFFLENBQUMsWUFBWSxFQUFFLFlBQVksQ0FBQztpQkFDeEM7O0lBQ21DLDRCQUFDO0NBQUEsQUFMckMsSUFLcUM7U0FBeEIscUJBQXFCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJvdXRlcywgUm91dGVyTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IFJvbGVzQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3JvbGVzL3JvbGVzLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBSb2xlUmVzb2x2ZXIgfSBmcm9tICcuL3Jlc29sdmVycy9yb2xlcy5yZXNvbHZlcic7XG5pbXBvcnQgeyBEeW5hbWljTGF5b3V0Q29tcG9uZW50LCBBdXRoR3VhcmQsIFBlcm1pc3Npb25HdWFyZCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBVc2Vyc0NvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy91c2Vycy91c2Vycy5jb21wb25lbnQnO1xuaW1wb3J0IHsgVXNlclJlc29sdmVyIH0gZnJvbSAnLi9yZXNvbHZlcnMvdXNlcnMucmVzb2x2ZXInO1xuXG5jb25zdCByb3V0ZXM6IFJvdXRlcyA9IFtcbiAgeyBwYXRoOiAnJywgcmVkaXJlY3RUbzogJ3JvbGVzJywgcGF0aE1hdGNoOiAnZnVsbCcgfSxcbiAge1xuICAgIHBhdGg6ICdyb2xlcycsXG4gICAgY29tcG9uZW50OiBEeW5hbWljTGF5b3V0Q29tcG9uZW50LFxuICAgIGNhbkFjdGl2YXRlOiBbQXV0aEd1YXJkLCBQZXJtaXNzaW9uR3VhcmRdLFxuICAgIGRhdGE6IHsgcmVxdWlyZWRQb2xpY3k6ICdBYnBJZGVudGl0eS5Sb2xlcycgfSxcbiAgICBjaGlsZHJlbjogW3sgcGF0aDogJycsIGNvbXBvbmVudDogUm9sZXNDb21wb25lbnQsIHJlc29sdmU6IFtSb2xlUmVzb2x2ZXJdIH1dLFxuICB9LFxuICB7XG4gICAgcGF0aDogJ3VzZXJzJyxcbiAgICBjb21wb25lbnQ6IER5bmFtaWNMYXlvdXRDb21wb25lbnQsXG4gICAgY2FuQWN0aXZhdGU6IFtBdXRoR3VhcmQsIFBlcm1pc3Npb25HdWFyZF0sXG4gICAgZGF0YTogeyByZXF1aXJlZFBvbGljeTogJ0FicElkZW50aXR5LlVzZXJzJyB9LFxuICAgIGNoaWxkcmVuOiBbXG4gICAgICB7XG4gICAgICAgIHBhdGg6ICcnLFxuICAgICAgICBjb21wb25lbnQ6IFVzZXJzQ29tcG9uZW50LFxuICAgICAgICByZXNvbHZlOiBbUm9sZVJlc29sdmVyLCBVc2VyUmVzb2x2ZXJdLFxuICAgICAgfSxcbiAgICBdLFxuICB9LFxuXTtcblxuQE5nTW9kdWxlKHtcbiAgaW1wb3J0czogW1JvdXRlck1vZHVsZS5mb3JDaGlsZChyb3V0ZXMpXSxcbiAgZXhwb3J0czogW1JvdXRlck1vZHVsZV0sXG4gIHByb3ZpZGVyczogW1JvbGVSZXNvbHZlciwgVXNlclJlc29sdmVyXSxcbn0pXG5leHBvcnQgY2xhc3MgSWRlbnRpdHlSb3V0aW5nTW9kdWxlIHt9XG4iXX0=