/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { DynamicLayoutComponent } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { ManageProfileComponent } from './components/manage-profile/manage-profile.component';
import { RegisterComponent } from './components/register/register.component';
/** @type {?} */
var routes = [
    { path: '', pathMatch: 'full', redirectTo: 'login' },
    {
        path: '',
        component: DynamicLayoutComponent,
        children: [
            { path: 'login', component: LoginComponent },
            { path: 'register', component: RegisterComponent },
            {
                path: 'manage-profile',
                component: ManageProfileComponent,
            },
        ],
    },
];
var AccountRoutingModule = /** @class */ (function () {
    function AccountRoutingModule() {
    }
    AccountRoutingModule.decorators = [
        { type: NgModule, args: [{
                    imports: [RouterModule.forChild(routes)],
                    exports: [RouterModule],
                },] }
    ];
    return AccountRoutingModule;
}());
export { AccountRoutingModule };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC1yb3V0aW5nLm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuYWNjb3VudC8iLCJzb3VyY2VzIjpbImxpYi9hY2NvdW50LXJvdXRpbmcubW9kdWxlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDdEQsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN6QyxPQUFPLEVBQUUsWUFBWSxFQUFVLE1BQU0saUJBQWlCLENBQUM7QUFDdkQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLG9DQUFvQyxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxNQUFNLHNEQUFzRCxDQUFDO0FBQzlGLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLDBDQUEwQyxDQUFDOztJQUV2RSxNQUFNLEdBQVc7SUFDckIsRUFBRSxJQUFJLEVBQUUsRUFBRSxFQUFFLFNBQVMsRUFBRSxNQUFNLEVBQUUsVUFBVSxFQUFFLE9BQU8sRUFBRTtJQUNwRDtRQUNFLElBQUksRUFBRSxFQUFFO1FBQ1IsU0FBUyxFQUFFLHNCQUFzQjtRQUNqQyxRQUFRLEVBQUU7WUFDUixFQUFFLElBQUksRUFBRSxPQUFPLEVBQUUsU0FBUyxFQUFFLGNBQWMsRUFBRTtZQUM1QyxFQUFFLElBQUksRUFBRSxVQUFVLEVBQUUsU0FBUyxFQUFFLGlCQUFpQixFQUFFO1lBQ2xEO2dCQUNFLElBQUksRUFBRSxnQkFBZ0I7Z0JBQ3RCLFNBQVMsRUFBRSxzQkFBc0I7YUFDbEM7U0FDRjtLQUNGO0NBQ0Y7QUFFRDtJQUFBO0lBSW1DLENBQUM7O2dCQUpuQyxRQUFRLFNBQUM7b0JBQ1IsT0FBTyxFQUFFLENBQUMsWUFBWSxDQUFDLFFBQVEsQ0FBQyxNQUFNLENBQUMsQ0FBQztvQkFDeEMsT0FBTyxFQUFFLENBQUMsWUFBWSxDQUFDO2lCQUN4Qjs7SUFDa0MsMkJBQUM7Q0FBQSxBQUpwQyxJQUlvQztTQUF2QixvQkFBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBEeW5hbWljTGF5b3V0Q29tcG9uZW50IH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgTmdNb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgUm91dGVyTW9kdWxlLCBSb3V0ZXMgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xyXG5pbXBvcnQgeyBMb2dpbkNvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9sb2dpbi9sb2dpbi5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBNYW5hZ2VQcm9maWxlQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL21hbmFnZS1wcm9maWxlL21hbmFnZS1wcm9maWxlLmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IFJlZ2lzdGVyQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL3JlZ2lzdGVyL3JlZ2lzdGVyLmNvbXBvbmVudCc7XHJcblxyXG5jb25zdCByb3V0ZXM6IFJvdXRlcyA9IFtcclxuICB7IHBhdGg6ICcnLCBwYXRoTWF0Y2g6ICdmdWxsJywgcmVkaXJlY3RUbzogJ2xvZ2luJyB9LFxyXG4gIHtcclxuICAgIHBhdGg6ICcnLFxyXG4gICAgY29tcG9uZW50OiBEeW5hbWljTGF5b3V0Q29tcG9uZW50LFxyXG4gICAgY2hpbGRyZW46IFtcclxuICAgICAgeyBwYXRoOiAnbG9naW4nLCBjb21wb25lbnQ6IExvZ2luQ29tcG9uZW50IH0sXHJcbiAgICAgIHsgcGF0aDogJ3JlZ2lzdGVyJywgY29tcG9uZW50OiBSZWdpc3RlckNvbXBvbmVudCB9LFxyXG4gICAgICB7XHJcbiAgICAgICAgcGF0aDogJ21hbmFnZS1wcm9maWxlJyxcclxuICAgICAgICBjb21wb25lbnQ6IE1hbmFnZVByb2ZpbGVDb21wb25lbnQsXHJcbiAgICAgIH0sXHJcbiAgICBdLFxyXG4gIH0sXHJcbl07XHJcblxyXG5ATmdNb2R1bGUoe1xyXG4gIGltcG9ydHM6IFtSb3V0ZXJNb2R1bGUuZm9yQ2hpbGQocm91dGVzKV0sXHJcbiAgZXhwb3J0czogW1JvdXRlck1vZHVsZV0sXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBBY2NvdW50Um91dGluZ01vZHVsZSB7fVxyXG4iXX0=