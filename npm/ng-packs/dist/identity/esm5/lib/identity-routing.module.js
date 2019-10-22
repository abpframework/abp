/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHktcm91dGluZy5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL2lkZW50aXR5LXJvdXRpbmcubW9kdWxlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLHNCQUFzQixFQUFFLGVBQWUsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNsRixPQUFPLEVBQUUsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3pDLE9BQU8sRUFBRSxZQUFZLEVBQVUsTUFBTSxpQkFBaUIsQ0FBQztBQUN2RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDcEUsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLG9DQUFvQyxDQUFDO1NBWXRELEVBQUUsY0FBYyxFQUFFLG1CQUFtQixFQUFFLE9BS3ZDLEVBQUUsY0FBYyxFQUFFLG1CQUFtQixFQUFFOztJQWYvQyxNQUFNLEdBQVc7SUFDckIsRUFBRSxJQUFJLEVBQUUsRUFBRSxFQUFFLFVBQVUsRUFBRSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sRUFBRTtJQUNwRDtRQUNFLElBQUksRUFBRSxFQUFFO1FBQ1IsU0FBUyxFQUFFLHNCQUFzQjtRQUNqQyxXQUFXLEVBQUUsQ0FBQyxTQUFTLEVBQUUsZUFBZSxDQUFDO1FBQ3pDLFFBQVEsRUFBRTtZQUNSO2dCQUNFLElBQUksRUFBRSxPQUFPO2dCQUNiLFNBQVMsRUFBRSxjQUFjO2dCQUN6QixJQUFJLElBQXlDO2FBQzlDO1lBQ0Q7Z0JBQ0UsSUFBSSxFQUFFLE9BQU87Z0JBQ2IsU0FBUyxFQUFFLGNBQWM7Z0JBQ3pCLElBQUksSUFBeUM7YUFDOUM7U0FDRjtLQUNGO0NBQ0Y7QUFFRDtJQUFBO0lBSW9DLENBQUM7O2dCQUpwQyxRQUFRLFNBQUM7b0JBQ1IsT0FBTyxFQUFFLENBQUMsWUFBWSxDQUFDLFFBQVEsQ0FBQyxNQUFNLENBQUMsQ0FBQztvQkFDeEMsT0FBTyxFQUFFLENBQUMsWUFBWSxDQUFDO2lCQUN4Qjs7SUFDbUMsNEJBQUM7Q0FBQSxBQUpyQyxJQUlxQztTQUF4QixxQkFBcUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBdXRoR3VhcmQsIER5bmFtaWNMYXlvdXRDb21wb25lbnQsIFBlcm1pc3Npb25HdWFyZCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcbmltcG9ydCB7IE5nTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFJvdXRlck1vZHVsZSwgUm91dGVzIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcclxuaW1wb3J0IHsgUm9sZXNDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvcm9sZXMvcm9sZXMuY29tcG9uZW50JztcclxuaW1wb3J0IHsgVXNlcnNDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvdXNlcnMvdXNlcnMuY29tcG9uZW50JztcclxuXHJcbmNvbnN0IHJvdXRlczogUm91dGVzID0gW1xyXG4gIHsgcGF0aDogJycsIHJlZGlyZWN0VG86ICdyb2xlcycsIHBhdGhNYXRjaDogJ2Z1bGwnIH0sXHJcbiAge1xyXG4gICAgcGF0aDogJycsXHJcbiAgICBjb21wb25lbnQ6IER5bmFtaWNMYXlvdXRDb21wb25lbnQsXHJcbiAgICBjYW5BY3RpdmF0ZTogW0F1dGhHdWFyZCwgUGVybWlzc2lvbkd1YXJkXSxcclxuICAgIGNoaWxkcmVuOiBbXHJcbiAgICAgIHtcclxuICAgICAgICBwYXRoOiAncm9sZXMnLFxyXG4gICAgICAgIGNvbXBvbmVudDogUm9sZXNDb21wb25lbnQsXHJcbiAgICAgICAgZGF0YTogeyByZXF1aXJlZFBvbGljeTogJ0FicElkZW50aXR5LlJvbGVzJyB9LFxyXG4gICAgICB9LFxyXG4gICAgICB7XHJcbiAgICAgICAgcGF0aDogJ3VzZXJzJyxcclxuICAgICAgICBjb21wb25lbnQ6IFVzZXJzQ29tcG9uZW50LFxyXG4gICAgICAgIGRhdGE6IHsgcmVxdWlyZWRQb2xpY3k6ICdBYnBJZGVudGl0eS5Vc2VycycgfSxcclxuICAgICAgfSxcclxuICAgIF0sXHJcbiAgfSxcclxuXTtcclxuXHJcbkBOZ01vZHVsZSh7XHJcbiAgaW1wb3J0czogW1JvdXRlck1vZHVsZS5mb3JDaGlsZChyb3V0ZXMpXSxcclxuICBleHBvcnRzOiBbUm91dGVyTW9kdWxlXSxcclxufSlcclxuZXhwb3J0IGNsYXNzIElkZW50aXR5Um91dGluZ01vZHVsZSB7fVxyXG4iXX0=