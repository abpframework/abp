/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxlLnByb3ZpZGVyLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3Byb3ZpZGVycy9sb2NhbGUucHJvdmlkZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFZLE1BQU0sZUFBZSxDQUFDO0FBQ3BELE9BQU8sY0FBYyxNQUFNLGdDQUFnQyxDQUFDO0FBQzVELE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLGtDQUFrQyxDQUFDO0FBRXZFO0lBQThCLG9DQUFNO0lBQ2xDLGtCQUFvQixtQkFBd0M7UUFBNUQsWUFDRSxpQkFBTyxTQUNSO1FBRm1CLHlCQUFtQixHQUFuQixtQkFBbUIsQ0FBcUI7O0lBRTVELENBQUM7Ozs7SUFFRCwyQkFBUTs7O0lBQVI7UUFDVSxJQUFBLGtEQUFXO1FBQ25CLE9BQU8sY0FBYyxDQUFDLFdBQVcsQ0FBQyxJQUFJLFdBQVcsQ0FBQztJQUNwRCxDQUFDOzs7O0lBRUQsMEJBQU87OztJQUFQO1FBQ0UsT0FBTyxJQUFJLENBQUMsUUFBUSxFQUFFLENBQUM7SUFDekIsQ0FBQztJQUNILGVBQUM7QUFBRCxDQUFDLEFBYkQsQ0FBOEIsTUFBTSxHQWFuQzs7Ozs7OztJQVphLHVDQUFnRDs7O0FBYzlELE1BQU0sS0FBTyxjQUFjLEdBQWE7SUFDdEMsT0FBTyxFQUFFLFNBQVM7SUFDbEIsUUFBUSxFQUFFLFFBQVE7SUFDbEIsSUFBSSxFQUFFLENBQUMsbUJBQW1CLENBQUM7Q0FDNUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBMT0NBTEVfSUQsIFByb3ZpZGVyIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgbG9jYWxlc01hcHBpbmcgZnJvbSAnLi4vY29uc3RhbnRzL2RpZmZlcmVudC1sb2NhbGVzJztcbmltcG9ydCB7IExvY2FsaXphdGlvblNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9sb2NhbGl6YXRpb24uc2VydmljZSc7XG5cbmV4cG9ydCBjbGFzcyBMb2NhbGVJZCBleHRlbmRzIFN0cmluZyB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgbG9jYWxpemF0aW9uU2VydmljZTogTG9jYWxpemF0aW9uU2VydmljZSkge1xuICAgIHN1cGVyKCk7XG4gIH1cblxuICB0b1N0cmluZygpOiBzdHJpbmcge1xuICAgIGNvbnN0IHsgY3VycmVudExhbmcgfSA9IHRoaXMubG9jYWxpemF0aW9uU2VydmljZTtcbiAgICByZXR1cm4gbG9jYWxlc01hcHBpbmdbY3VycmVudExhbmddIHx8IGN1cnJlbnRMYW5nO1xuICB9XG5cbiAgdmFsdWVPZigpOiBzdHJpbmcge1xuICAgIHJldHVybiB0aGlzLnRvU3RyaW5nKCk7XG4gIH1cbn1cblxuZXhwb3J0IGNvbnN0IExvY2FsZVByb3ZpZGVyOiBQcm92aWRlciA9IHtcbiAgcHJvdmlkZTogTE9DQUxFX0lELFxuICB1c2VDbGFzczogTG9jYWxlSWQsXG4gIGRlcHM6IFtMb2NhbGl6YXRpb25TZXJ2aWNlXSxcbn07XG4iXX0=