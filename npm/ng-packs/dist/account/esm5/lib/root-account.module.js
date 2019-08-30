/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { NgModule } from '@angular/core';
import { ACCOUNT_OPTIONS, optionsFactory } from './tokens/options.token';
var RootAccountModule = /** @class */ (function () {
    function RootAccountModule() {
    }
    /**
     * @param {?=} options
     * @return {?}
     */
    RootAccountModule.forRoot = /**
     * @param {?=} options
     * @return {?}
     */
    function (options) {
        if (options === void 0) { options = (/** @type {?} */ ({})); }
        return {
            ngModule: RootAccountModule,
            providers: [
                { provide: ACCOUNT_OPTIONS, useValue: options },
                {
                    provide: 'ACCOUNT_OPTIONS',
                    useFactory: optionsFactory,
                    deps: [ACCOUNT_OPTIONS],
                },
            ],
        };
    };
    RootAccountModule.decorators = [
        { type: NgModule, args: [{},] }
    ];
    return RootAccountModule;
}());
export { RootAccountModule };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm9vdC1hY2NvdW50Lm1vZHVsZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuYWNjb3VudC8iLCJzb3VyY2VzIjpbImxpYi9yb290LWFjY291bnQubW9kdWxlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQXVCLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUU5RCxPQUFPLEVBQUUsZUFBZSxFQUFFLGNBQWMsRUFBRSxNQUFNLHdCQUF3QixDQUFDO0FBRXpFO0lBQUE7SUFlQSxDQUFDOzs7OztJQWJRLHlCQUFPOzs7O0lBQWQsVUFBZSxPQUF1QjtRQUF2Qix3QkFBQSxFQUFBLDZCQUFVLEVBQUUsRUFBVztRQUNwQyxPQUFPO1lBQ0wsUUFBUSxFQUFFLGlCQUFpQjtZQUMzQixTQUFTLEVBQUU7Z0JBQ1QsRUFBRSxPQUFPLEVBQUUsZUFBZSxFQUFFLFFBQVEsRUFBRSxPQUFPLEVBQUU7Z0JBQy9DO29CQUNFLE9BQU8sRUFBRSxpQkFBaUI7b0JBQzFCLFVBQVUsRUFBRSxjQUFjO29CQUMxQixJQUFJLEVBQUUsQ0FBQyxlQUFlLENBQUM7aUJBQ3hCO2FBQ0Y7U0FDRixDQUFDO0lBQ0osQ0FBQzs7Z0JBZEYsUUFBUSxTQUFDLEVBQUU7O0lBZVosd0JBQUM7Q0FBQSxBQWZELElBZUM7U0FkWSxpQkFBaUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBNb2R1bGVXaXRoUHJvdmlkZXJzLCBOZ01vZHVsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgT3B0aW9ucyB9IGZyb20gJy4vbW9kZWxzL29wdGlvbnMnO1xuaW1wb3J0IHsgQUNDT1VOVF9PUFRJT05TLCBvcHRpb25zRmFjdG9yeSB9IGZyb20gJy4vdG9rZW5zL29wdGlvbnMudG9rZW4nO1xuXG5ATmdNb2R1bGUoe30pXG5leHBvcnQgY2xhc3MgUm9vdEFjY291bnRNb2R1bGUge1xuICBzdGF0aWMgZm9yUm9vdChvcHRpb25zID0ge30gYXMgT3B0aW9ucyk6IE1vZHVsZVdpdGhQcm92aWRlcnMge1xuICAgIHJldHVybiB7XG4gICAgICBuZ01vZHVsZTogUm9vdEFjY291bnRNb2R1bGUsXG4gICAgICBwcm92aWRlcnM6IFtcbiAgICAgICAgeyBwcm92aWRlOiBBQ0NPVU5UX09QVElPTlMsIHVzZVZhbHVlOiBvcHRpb25zIH0sXG4gICAgICAgIHtcbiAgICAgICAgICBwcm92aWRlOiAnQUNDT1VOVF9PUFRJT05TJyxcbiAgICAgICAgICB1c2VGYWN0b3J5OiBvcHRpb25zRmFjdG9yeSxcbiAgICAgICAgICBkZXBzOiBbQUNDT1VOVF9PUFRJT05TXSxcbiAgICAgICAgfSxcbiAgICAgIF0sXG4gICAgfTtcbiAgfVxufVxuIl19