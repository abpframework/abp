/**
 * @fileoverview added by tsickle
 * Generated from: lib/providers/locale.provider.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { LOCALE_ID } from '@angular/core';
import localesMapping from '../constants/different-locales';
import { LocalizationService } from '../services/localization.service';
var LocaleId = /** @class */ (function (_super) {
    tslib_1.__extends(LocaleId, _super);
    function LocaleId(localizationService) {
        var _this = _super.call(this) || this;
        _this.localizationService = localizationService;
        return _this;
    }
    /**
     * @return {?}
     */
    LocaleId.prototype.toString = /**
     * @return {?}
     */
    function () {
        var currentLang = this.localizationService.currentLang;
        return localesMapping[currentLang] || currentLang;
    };
    /**
     * @return {?}
     */
    LocaleId.prototype.valueOf = /**
     * @return {?}
     */
    function () {
        return this.toString();
    };
    return LocaleId;
}(String));
export { LocaleId };
if (false) {
    /**
     * @type {?}
     * @private
     */
    LocaleId.prototype.localizationService;
}
/** @type {?} */
export var LocaleProvider = {
    provide: LOCALE_ID,
    useClass: LocaleId,
    deps: [LocalizationService],
};
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxlLnByb3ZpZGVyLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3Byb3ZpZGVycy9sb2NhbGUucHJvdmlkZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBWSxNQUFNLGVBQWUsQ0FBQztBQUNwRCxPQUFPLGNBQWMsTUFBTSxnQ0FBZ0MsQ0FBQztBQUM1RCxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSxrQ0FBa0MsQ0FBQztBQUV2RTtJQUE4QixvQ0FBTTtJQUNsQyxrQkFBb0IsbUJBQXdDO1FBQTVELFlBQ0UsaUJBQU8sU0FDUjtRQUZtQix5QkFBbUIsR0FBbkIsbUJBQW1CLENBQXFCOztJQUU1RCxDQUFDOzs7O0lBRUQsMkJBQVE7OztJQUFSO1FBQ1UsSUFBQSxrREFBVztRQUNuQixPQUFPLGNBQWMsQ0FBQyxXQUFXLENBQUMsSUFBSSxXQUFXLENBQUM7SUFDcEQsQ0FBQzs7OztJQUVELDBCQUFPOzs7SUFBUDtRQUNFLE9BQU8sSUFBSSxDQUFDLFFBQVEsRUFBRSxDQUFDO0lBQ3pCLENBQUM7SUFDSCxlQUFDO0FBQUQsQ0FBQyxBQWJELENBQThCLE1BQU0sR0FhbkM7Ozs7Ozs7SUFaYSx1Q0FBZ0Q7OztBQWM5RCxNQUFNLEtBQU8sY0FBYyxHQUFhO0lBQ3RDLE9BQU8sRUFBRSxTQUFTO0lBQ2xCLFFBQVEsRUFBRSxRQUFRO0lBQ2xCLElBQUksRUFBRSxDQUFDLG1CQUFtQixDQUFDO0NBQzVCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgTE9DQUxFX0lELCBQcm92aWRlciB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgbG9jYWxlc01hcHBpbmcgZnJvbSAnLi4vY29uc3RhbnRzL2RpZmZlcmVudC1sb2NhbGVzJztcclxuaW1wb3J0IHsgTG9jYWxpemF0aW9uU2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL2xvY2FsaXphdGlvbi5zZXJ2aWNlJztcclxuXHJcbmV4cG9ydCBjbGFzcyBMb2NhbGVJZCBleHRlbmRzIFN0cmluZyB7XHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBsb2NhbGl6YXRpb25TZXJ2aWNlOiBMb2NhbGl6YXRpb25TZXJ2aWNlKSB7XHJcbiAgICBzdXBlcigpO1xyXG4gIH1cclxuXHJcbiAgdG9TdHJpbmcoKTogc3RyaW5nIHtcclxuICAgIGNvbnN0IHsgY3VycmVudExhbmcgfSA9IHRoaXMubG9jYWxpemF0aW9uU2VydmljZTtcclxuICAgIHJldHVybiBsb2NhbGVzTWFwcGluZ1tjdXJyZW50TGFuZ10gfHwgY3VycmVudExhbmc7XHJcbiAgfVxyXG5cclxuICB2YWx1ZU9mKCk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gdGhpcy50b1N0cmluZygpO1xyXG4gIH1cclxufVxyXG5cclxuZXhwb3J0IGNvbnN0IExvY2FsZVByb3ZpZGVyOiBQcm92aWRlciA9IHtcclxuICBwcm92aWRlOiBMT0NBTEVfSUQsXHJcbiAgdXNlQ2xhc3M6IExvY2FsZUlkLFxyXG4gIGRlcHM6IFtMb2NhbGl6YXRpb25TZXJ2aWNlXSxcclxufTtcclxuIl19