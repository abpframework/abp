/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { FeatureManagementComponent } from './components/feature-management/feature-management.component';
import { NgxsModule } from '@ngxs/store';
import { FeatureManagementState } from './states/feature-management.state';
var FeatureManagementModule = /** @class */ (function () {
    function FeatureManagementModule() {
    }
    FeatureManagementModule.decorators = [
        { type: NgModule, args: [{
                    declarations: [FeatureManagementComponent],
                    imports: [CoreModule, ThemeSharedModule, NgxsModule.forFeature([FeatureManagementState])],
                    exports: [FeatureManagementComponent],
                },] }
    ];
    return FeatureManagementModule;
}());
export { FeatureManagementModule };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZmVhdHVyZS1tYW5hZ2VtZW50Lm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuZmVhdHVyZS1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2ZlYXR1cmUtbWFuYWdlbWVudC5tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDMUMsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDekQsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN6QyxPQUFPLEVBQUUsMEJBQTBCLEVBQUUsTUFBTSw4REFBOEQsQ0FBQztBQUMxRyxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3pDLE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxNQUFNLG1DQUFtQyxDQUFDO0FBRTNFO0lBQUE7SUFLc0MsQ0FBQzs7Z0JBTHRDLFFBQVEsU0FBQztvQkFDUixZQUFZLEVBQUUsQ0FBQywwQkFBMEIsQ0FBQztvQkFDMUMsT0FBTyxFQUFFLENBQUMsVUFBVSxFQUFFLGlCQUFpQixFQUFFLFVBQVUsQ0FBQyxVQUFVLENBQUMsQ0FBQyxzQkFBc0IsQ0FBQyxDQUFDLENBQUM7b0JBQ3pGLE9BQU8sRUFBRSxDQUFDLDBCQUEwQixDQUFDO2lCQUN0Qzs7SUFDcUMsOEJBQUM7Q0FBQSxBQUx2QyxJQUt1QztTQUExQix1QkFBdUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb3JlTW9kdWxlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgVGhlbWVTaGFyZWRNb2R1bGUgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XHJcbmltcG9ydCB7IE5nTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IEZlYXR1cmVNYW5hZ2VtZW50Q29tcG9uZW50IH0gZnJvbSAnLi9jb21wb25lbnRzL2ZlYXR1cmUtbWFuYWdlbWVudC9mZWF0dXJlLW1hbmFnZW1lbnQuY29tcG9uZW50JztcclxuaW1wb3J0IHsgTmd4c01vZHVsZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgRmVhdHVyZU1hbmFnZW1lbnRTdGF0ZSB9IGZyb20gJy4vc3RhdGVzL2ZlYXR1cmUtbWFuYWdlbWVudC5zdGF0ZSc7XHJcblxyXG5ATmdNb2R1bGUoe1xyXG4gIGRlY2xhcmF0aW9uczogW0ZlYXR1cmVNYW5hZ2VtZW50Q29tcG9uZW50XSxcclxuICBpbXBvcnRzOiBbQ29yZU1vZHVsZSwgVGhlbWVTaGFyZWRNb2R1bGUsIE5neHNNb2R1bGUuZm9yRmVhdHVyZShbRmVhdHVyZU1hbmFnZW1lbnRTdGF0ZV0pXSxcclxuICBleHBvcnRzOiBbRmVhdHVyZU1hbmFnZW1lbnRDb21wb25lbnRdLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgRmVhdHVyZU1hbmFnZW1lbnRNb2R1bGUge31cclxuIl19