/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { DynamicLayoutComponent } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SettingComponent } from './components/setting/setting.component';
/** @type {?} */
const routes = [
    {
        path: '',
        component: DynamicLayoutComponent,
        children: [{ path: '', component: SettingComponent }],
    },
];
export class SettingManagementRoutingModule {
}
SettingManagementRoutingModule.decorators = [
    { type: NgModule, args: [{
                imports: [RouterModule.forChild(routes)],
                exports: [RouterModule],
            },] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LXJvdXRpbmcubW9kdWxlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc2V0dGluZy1tYW5hZ2VtZW50LXJvdXRpbmcubW9kdWxlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsc0JBQXNCLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDdEQsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN6QyxPQUFPLEVBQUUsWUFBWSxFQUFVLE1BQU0saUJBQWlCLENBQUM7QUFDdkQsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sd0NBQXdDLENBQUM7O01BRXBFLE1BQU0sR0FBVztJQUNyQjtRQUNFLElBQUksRUFBRSxFQUFFO1FBQ1IsU0FBUyxFQUFFLHNCQUFzQjtRQUNqQyxRQUFRLEVBQUUsQ0FBQyxFQUFFLElBQUksRUFBRSxFQUFFLEVBQUUsU0FBUyxFQUFFLGdCQUFnQixFQUFFLENBQUM7S0FDdEQ7Q0FDRjtBQU1ELE1BQU0sT0FBTyw4QkFBOEI7OztZQUoxQyxRQUFRLFNBQUM7Z0JBQ1IsT0FBTyxFQUFFLENBQUMsWUFBWSxDQUFDLFFBQVEsQ0FBQyxNQUFNLENBQUMsQ0FBQztnQkFDeEMsT0FBTyxFQUFFLENBQUMsWUFBWSxDQUFDO2FBQ3hCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgRHluYW1pY0xheW91dENvbXBvbmVudCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBOZ01vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgUm91dGVyTW9kdWxlLCBSb3V0ZXMgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgU2V0dGluZ0NvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9zZXR0aW5nL3NldHRpbmcuY29tcG9uZW50JztcblxuY29uc3Qgcm91dGVzOiBSb3V0ZXMgPSBbXG4gIHtcbiAgICBwYXRoOiAnJyxcbiAgICBjb21wb25lbnQ6IER5bmFtaWNMYXlvdXRDb21wb25lbnQsXG4gICAgY2hpbGRyZW46IFt7IHBhdGg6ICcnLCBjb21wb25lbnQ6IFNldHRpbmdDb21wb25lbnQgfV0sXG4gIH0sXG5dO1xuXG5ATmdNb2R1bGUoe1xuICBpbXBvcnRzOiBbUm91dGVyTW9kdWxlLmZvckNoaWxkKHJvdXRlcyldLFxuICBleHBvcnRzOiBbUm91dGVyTW9kdWxlXSxcbn0pXG5leHBvcnQgY2xhc3MgU2V0dGluZ01hbmFnZW1lbnRSb3V0aW5nTW9kdWxlIHt9XG4iXX0=