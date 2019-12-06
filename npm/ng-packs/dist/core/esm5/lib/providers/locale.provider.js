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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxlLnByb3ZpZGVyLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3Byb3ZpZGVycy9sb2NhbGUucHJvdmlkZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBWSxNQUFNLGVBQWUsQ0FBQztBQUNwRCxPQUFPLGNBQWMsTUFBTSxnQ0FBZ0MsQ0FBQztBQUM1RCxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSxrQ0FBa0MsQ0FBQztBQUV2RTtJQUE4QixvQ0FBTTtJQUNsQyxrQkFBb0IsbUJBQXdDO1FBQTVELFlBQ0UsaUJBQU8sU0FDUjtRQUZtQix5QkFBbUIsR0FBbkIsbUJBQW1CLENBQXFCOztJQUU1RCxDQUFDOzs7O0lBRUQsMkJBQVE7OztJQUFSO1FBQ1UsSUFBQSxrREFBVztRQUNuQixPQUFPLGNBQWMsQ0FBQyxXQUFXLENBQUMsSUFBSSxXQUFXLENBQUM7SUFDcEQsQ0FBQzs7OztJQUVELDBCQUFPOzs7SUFBUDtRQUNFLE9BQU8sSUFBSSxDQUFDLFFBQVEsRUFBRSxDQUFDO0lBQ3pCLENBQUM7SUFDSCxlQUFDO0FBQUQsQ0FBQyxBQWJELENBQThCLE1BQU0sR0FhbkM7Ozs7Ozs7SUFaYSx1Q0FBZ0Q7OztBQWM5RCxNQUFNLEtBQU8sY0FBYyxHQUFhO0lBQ3RDLE9BQU8sRUFBRSxTQUFTO0lBQ2xCLFFBQVEsRUFBRSxRQUFRO0lBQ2xCLElBQUksRUFBRSxDQUFDLG1CQUFtQixDQUFDO0NBQzVCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgTE9DQUxFX0lELCBQcm92aWRlciB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IGxvY2FsZXNNYXBwaW5nIGZyb20gJy4uL2NvbnN0YW50cy9kaWZmZXJlbnQtbG9jYWxlcyc7XG5pbXBvcnQgeyBMb2NhbGl6YXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvbG9jYWxpemF0aW9uLnNlcnZpY2UnO1xuXG5leHBvcnQgY2xhc3MgTG9jYWxlSWQgZXh0ZW5kcyBTdHJpbmcge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGxvY2FsaXphdGlvblNlcnZpY2U6IExvY2FsaXphdGlvblNlcnZpY2UpIHtcbiAgICBzdXBlcigpO1xuICB9XG5cbiAgdG9TdHJpbmcoKTogc3RyaW5nIHtcbiAgICBjb25zdCB7IGN1cnJlbnRMYW5nIH0gPSB0aGlzLmxvY2FsaXphdGlvblNlcnZpY2U7XG4gICAgcmV0dXJuIGxvY2FsZXNNYXBwaW5nW2N1cnJlbnRMYW5nXSB8fCBjdXJyZW50TGFuZztcbiAgfVxuXG4gIHZhbHVlT2YoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy50b1N0cmluZygpO1xuICB9XG59XG5cbmV4cG9ydCBjb25zdCBMb2NhbGVQcm92aWRlcjogUHJvdmlkZXIgPSB7XG4gIHByb3ZpZGU6IExPQ0FMRV9JRCxcbiAgdXNlQ2xhc3M6IExvY2FsZUlkLFxuICBkZXBzOiBbTG9jYWxpemF0aW9uU2VydmljZV0sXG59O1xuIl19