/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxlLnByb3ZpZGVyLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3Byb3ZpZGVycy9sb2NhbGUucHJvdmlkZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFZLE1BQU0sZUFBZSxDQUFDO0FBQ3BELE9BQU8sY0FBYyxNQUFNLGdDQUFnQyxDQUFDO0FBQzVELE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLGtDQUFrQyxDQUFDO0FBRXZFO0lBQThCLG9DQUFNO0lBQ2xDLGtCQUFvQixtQkFBd0M7UUFBNUQsWUFDRSxpQkFBTyxTQUNSO1FBRm1CLHlCQUFtQixHQUFuQixtQkFBbUIsQ0FBcUI7O0lBRTVELENBQUM7Ozs7SUFFRCwyQkFBUTs7O0lBQVI7UUFDVSxJQUFBLGtEQUFXO1FBQ25CLE9BQU8sY0FBYyxDQUFDLFdBQVcsQ0FBQyxJQUFJLFdBQVcsQ0FBQztJQUNwRCxDQUFDOzs7O0lBRUQsMEJBQU87OztJQUFQO1FBQ0UsT0FBTyxJQUFJLENBQUMsUUFBUSxFQUFFLENBQUM7SUFDekIsQ0FBQztJQUNILGVBQUM7QUFBRCxDQUFDLEFBYkQsQ0FBOEIsTUFBTSxHQWFuQzs7Ozs7OztJQVphLHVDQUFnRDs7O0FBYzlELE1BQU0sS0FBTyxjQUFjLEdBQWE7SUFDdEMsT0FBTyxFQUFFLFNBQVM7SUFDbEIsUUFBUSxFQUFFLFFBQVE7SUFDbEIsSUFBSSxFQUFFLENBQUMsbUJBQW1CLENBQUM7Q0FDNUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBMT0NBTEVfSUQsIFByb3ZpZGVyIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCBsb2NhbGVzTWFwcGluZyBmcm9tICcuLi9jb25zdGFudHMvZGlmZmVyZW50LWxvY2FsZXMnO1xyXG5pbXBvcnQgeyBMb2NhbGl6YXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvbG9jYWxpemF0aW9uLnNlcnZpY2UnO1xyXG5cclxuZXhwb3J0IGNsYXNzIExvY2FsZUlkIGV4dGVuZHMgU3RyaW5nIHtcclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGxvY2FsaXphdGlvblNlcnZpY2U6IExvY2FsaXphdGlvblNlcnZpY2UpIHtcclxuICAgIHN1cGVyKCk7XHJcbiAgfVxyXG5cclxuICB0b1N0cmluZygpOiBzdHJpbmcge1xyXG4gICAgY29uc3QgeyBjdXJyZW50TGFuZyB9ID0gdGhpcy5sb2NhbGl6YXRpb25TZXJ2aWNlO1xyXG4gICAgcmV0dXJuIGxvY2FsZXNNYXBwaW5nW2N1cnJlbnRMYW5nXSB8fCBjdXJyZW50TGFuZztcclxuICB9XHJcblxyXG4gIHZhbHVlT2YoKTogc3RyaW5nIHtcclxuICAgIHJldHVybiB0aGlzLnRvU3RyaW5nKCk7XHJcbiAgfVxyXG59XHJcblxyXG5leHBvcnQgY29uc3QgTG9jYWxlUHJvdmlkZXI6IFByb3ZpZGVyID0ge1xyXG4gIHByb3ZpZGU6IExPQ0FMRV9JRCxcclxuICB1c2VDbGFzczogTG9jYWxlSWQsXHJcbiAgZGVwczogW0xvY2FsaXphdGlvblNlcnZpY2VdLFxyXG59O1xyXG4iXX0=