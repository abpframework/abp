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
const routes = [
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
export class AccountRoutingModule {
}
AccountRoutingModule.decorators = [
    { type: NgModule, args: [{
                imports: [RouterModule.forChild(routes)],
                exports: [RouterModule],
            },] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC1yb3V0aW5nLm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuYWNjb3VudC8iLCJzb3VyY2VzIjpbImxpYi9hY2NvdW50LXJvdXRpbmcubW9kdWxlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDdEQsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN6QyxPQUFPLEVBQUUsWUFBWSxFQUFVLE1BQU0saUJBQWlCLENBQUM7QUFDdkQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLG9DQUFvQyxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxNQUFNLHNEQUFzRCxDQUFDO0FBQzlGLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxNQUFNLDBDQUEwQyxDQUFDOztNQUV2RSxNQUFNLEdBQVc7SUFDckIsRUFBRSxJQUFJLEVBQUUsRUFBRSxFQUFFLFNBQVMsRUFBRSxNQUFNLEVBQUUsVUFBVSxFQUFFLE9BQU8sRUFBRTtJQUNwRDtRQUNFLElBQUksRUFBRSxFQUFFO1FBQ1IsU0FBUyxFQUFFLHNCQUFzQjtRQUNqQyxRQUFRLEVBQUU7WUFDUixFQUFFLElBQUksRUFBRSxPQUFPLEVBQUUsU0FBUyxFQUFFLGNBQWMsRUFBRTtZQUM1QyxFQUFFLElBQUksRUFBRSxVQUFVLEVBQUUsU0FBUyxFQUFFLGlCQUFpQixFQUFFO1lBQ2xEO2dCQUNFLElBQUksRUFBRSxnQkFBZ0I7Z0JBQ3RCLFNBQVMsRUFBRSxzQkFBc0I7YUFDbEM7U0FDRjtLQUNGO0NBQ0Y7QUFNRCxNQUFNLE9BQU8sb0JBQW9COzs7WUFKaEMsUUFBUSxTQUFDO2dCQUNSLE9BQU8sRUFBRSxDQUFDLFlBQVksQ0FBQyxRQUFRLENBQUMsTUFBTSxDQUFDLENBQUM7Z0JBQ3hDLE9BQU8sRUFBRSxDQUFDLFlBQVksQ0FBQzthQUN4QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IER5bmFtaWNMYXlvdXRDb21wb25lbnQgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5pbXBvcnQgeyBOZ01vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBSb3V0ZXJNb2R1bGUsIFJvdXRlcyB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XHJcbmltcG9ydCB7IExvZ2luQ29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2xvZ2luL2xvZ2luLmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IE1hbmFnZVByb2ZpbGVDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvbWFuYWdlLXByb2ZpbGUvbWFuYWdlLXByb2ZpbGUuY29tcG9uZW50JztcclxuaW1wb3J0IHsgUmVnaXN0ZXJDb21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvcmVnaXN0ZXIvcmVnaXN0ZXIuY29tcG9uZW50JztcclxuXHJcbmNvbnN0IHJvdXRlczogUm91dGVzID0gW1xyXG4gIHsgcGF0aDogJycsIHBhdGhNYXRjaDogJ2Z1bGwnLCByZWRpcmVjdFRvOiAnbG9naW4nIH0sXHJcbiAge1xyXG4gICAgcGF0aDogJycsXHJcbiAgICBjb21wb25lbnQ6IER5bmFtaWNMYXlvdXRDb21wb25lbnQsXHJcbiAgICBjaGlsZHJlbjogW1xyXG4gICAgICB7IHBhdGg6ICdsb2dpbicsIGNvbXBvbmVudDogTG9naW5Db21wb25lbnQgfSxcclxuICAgICAgeyBwYXRoOiAncmVnaXN0ZXInLCBjb21wb25lbnQ6IFJlZ2lzdGVyQ29tcG9uZW50IH0sXHJcbiAgICAgIHtcclxuICAgICAgICBwYXRoOiAnbWFuYWdlLXByb2ZpbGUnLFxyXG4gICAgICAgIGNvbXBvbmVudDogTWFuYWdlUHJvZmlsZUNvbXBvbmVudCxcclxuICAgICAgfSxcclxuICAgIF0sXHJcbiAgfSxcclxuXTtcclxuXHJcbkBOZ01vZHVsZSh7XHJcbiAgaW1wb3J0czogW1JvdXRlck1vZHVsZS5mb3JDaGlsZChyb3V0ZXMpXSxcclxuICBleHBvcnRzOiBbUm91dGVyTW9kdWxlXSxcclxufSlcclxuZXhwb3J0IGNsYXNzIEFjY291bnRSb3V0aW5nTW9kdWxlIHt9XHJcbiJdfQ==