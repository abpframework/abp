/**
 * @fileoverview added by tsickle
 * Generated from: lib/identity-routing.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { AuthGuard, DynamicLayoutComponent, PermissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { RolesComponent } from './components/roles/roles.component';
import { UsersComponent } from './components/users/users.component';
var ɵ0 = { requiredPolicy: 'AbpIdentity.Roles' }, ɵ1 = { requiredPolicy: 'AbpIdentity.Users' };
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
                data: ɵ0,
            },
            {
                path: 'users',
                component: UsersComponent,
                data: ɵ1,
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
                },] }
    ];
    return IdentityRoutingModule;
}());
export { IdentityRoutingModule };
export { ɵ0, ɵ1 };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHktcm91dGluZy5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL2lkZW50aXR5LXJvdXRpbmcubW9kdWxlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxzQkFBc0IsRUFBRSxlQUFlLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDbEYsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN6QyxPQUFPLEVBQUUsWUFBWSxFQUFVLE1BQU0saUJBQWlCLENBQUM7QUFDdkQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLG9DQUFvQyxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxvQ0FBb0MsQ0FBQztTQVl0RCxFQUFFLGNBQWMsRUFBRSxtQkFBbUIsRUFBRSxPQUt2QyxFQUFFLGNBQWMsRUFBRSxtQkFBbUIsRUFBRTs7SUFmL0MsTUFBTSxHQUFXO0lBQ3JCLEVBQUUsSUFBSSxFQUFFLEVBQUUsRUFBRSxVQUFVLEVBQUUsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLEVBQUU7SUFDcEQ7UUFDRSxJQUFJLEVBQUUsRUFBRTtRQUNSLFNBQVMsRUFBRSxzQkFBc0I7UUFDakMsV0FBVyxFQUFFLENBQUMsU0FBUyxFQUFFLGVBQWUsQ0FBQztRQUN6QyxRQUFRLEVBQUU7WUFDUjtnQkFDRSxJQUFJLEVBQUUsT0FBTztnQkFDYixTQUFTLEVBQUUsY0FBYztnQkFDekIsSUFBSSxJQUF5QzthQUM5QztZQUNEO2dCQUNFLElBQUksRUFBRSxPQUFPO2dCQUNiLFNBQVMsRUFBRSxjQUFjO2dCQUN6QixJQUFJLElBQXlDO2FBQzlDO1NBQ0Y7S0FDRjtDQUNGO0FBRUQ7SUFBQTtJQUlvQyxDQUFDOztnQkFKcEMsUUFBUSxTQUFDO29CQUNSLE9BQU8sRUFBRSxDQUFDLFlBQVksQ0FBQyxRQUFRLENBQUMsTUFBTSxDQUFDLENBQUM7b0JBQ3hDLE9BQU8sRUFBRSxDQUFDLFlBQVksQ0FBQztpQkFDeEI7O0lBQ21DLDRCQUFDO0NBQUEsQUFKckMsSUFJcUM7U0FBeEIscUJBQXFCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQXV0aEd1YXJkLCBEeW5hbWljTGF5b3V0Q29tcG9uZW50LCBQZXJtaXNzaW9uR3VhcmQgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJvdXRlck1vZHVsZSwgUm91dGVzIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IFJvbGVzQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3JvbGVzL3JvbGVzLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBVc2Vyc0NvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy91c2Vycy91c2Vycy5jb21wb25lbnQnO1xuXG5jb25zdCByb3V0ZXM6IFJvdXRlcyA9IFtcbiAgeyBwYXRoOiAnJywgcmVkaXJlY3RUbzogJ3JvbGVzJywgcGF0aE1hdGNoOiAnZnVsbCcgfSxcbiAge1xuICAgIHBhdGg6ICcnLFxuICAgIGNvbXBvbmVudDogRHluYW1pY0xheW91dENvbXBvbmVudCxcbiAgICBjYW5BY3RpdmF0ZTogW0F1dGhHdWFyZCwgUGVybWlzc2lvbkd1YXJkXSxcbiAgICBjaGlsZHJlbjogW1xuICAgICAge1xuICAgICAgICBwYXRoOiAncm9sZXMnLFxuICAgICAgICBjb21wb25lbnQ6IFJvbGVzQ29tcG9uZW50LFxuICAgICAgICBkYXRhOiB7IHJlcXVpcmVkUG9saWN5OiAnQWJwSWRlbnRpdHkuUm9sZXMnIH0sXG4gICAgICB9LFxuICAgICAge1xuICAgICAgICBwYXRoOiAndXNlcnMnLFxuICAgICAgICBjb21wb25lbnQ6IFVzZXJzQ29tcG9uZW50LFxuICAgICAgICBkYXRhOiB7IHJlcXVpcmVkUG9saWN5OiAnQWJwSWRlbnRpdHkuVXNlcnMnIH0sXG4gICAgICB9LFxuICAgIF0sXG4gIH0sXG5dO1xuXG5ATmdNb2R1bGUoe1xuICBpbXBvcnRzOiBbUm91dGVyTW9kdWxlLmZvckNoaWxkKHJvdXRlcyldLFxuICBleHBvcnRzOiBbUm91dGVyTW9kdWxlXSxcbn0pXG5leHBvcnQgY2xhc3MgSWRlbnRpdHlSb3V0aW5nTW9kdWxlIHt9XG4iXX0=