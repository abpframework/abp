/**
 * @fileoverview added by tsickle
 * Generated from: lib/providers/locale.provider.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { LOCALE_ID } from '@angular/core';
import localesMapping from '../constants/different-locales';
import { LocalizationService } from '../services/localization.service';
export class LocaleId extends String {
    /**
     * @param {?} localizationService
     */
    constructor(localizationService) {
        super();
        this.localizationService = localizationService;
    }
    /**
     * @return {?}
     */
    toString() {
        const { currentLang } = this.localizationService;
        return localesMapping[currentLang] || currentLang;
    }
    /**
     * @return {?}
     */
    valueOf() {
        return this.toString();
    }
}
if (false) {
    /**
     * @type {?}
     * @private
     */
    LocaleId.prototype.localizationService;
}
/** @type {?} */
export const LocaleProvider = {
    provide: LOCALE_ID,
    useClass: LocaleId,
    deps: [LocalizationService],
};
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxlLnByb3ZpZGVyLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3Byb3ZpZGVycy9sb2NhbGUucHJvdmlkZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFZLE1BQU0sZUFBZSxDQUFDO0FBQ3BELE9BQU8sY0FBYyxNQUFNLGdDQUFnQyxDQUFDO0FBQzVELE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLGtDQUFrQyxDQUFDO0FBRXZFLE1BQU0sT0FBTyxRQUFTLFNBQVEsTUFBTTs7OztJQUNsQyxZQUFvQixtQkFBd0M7UUFDMUQsS0FBSyxFQUFFLENBQUM7UUFEVSx3QkFBbUIsR0FBbkIsbUJBQW1CLENBQXFCO0lBRTVELENBQUM7Ozs7SUFFRCxRQUFRO2NBQ0EsRUFBRSxXQUFXLEVBQUUsR0FBRyxJQUFJLENBQUMsbUJBQW1CO1FBQ2hELE9BQU8sY0FBYyxDQUFDLFdBQVcsQ0FBQyxJQUFJLFdBQVcsQ0FBQztJQUNwRCxDQUFDOzs7O0lBRUQsT0FBTztRQUNMLE9BQU8sSUFBSSxDQUFDLFFBQVEsRUFBRSxDQUFDO0lBQ3pCLENBQUM7Q0FDRjs7Ozs7O0lBWmEsdUNBQWdEOzs7QUFjOUQsTUFBTSxPQUFPLGNBQWMsR0FBYTtJQUN0QyxPQUFPLEVBQUUsU0FBUztJQUNsQixRQUFRLEVBQUUsUUFBUTtJQUNsQixJQUFJLEVBQUUsQ0FBQyxtQkFBbUIsQ0FBQztDQUM1QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IExPQ0FMRV9JRCwgUHJvdmlkZXIgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IGxvY2FsZXNNYXBwaW5nIGZyb20gJy4uL2NvbnN0YW50cy9kaWZmZXJlbnQtbG9jYWxlcyc7XHJcbmltcG9ydCB7IExvY2FsaXphdGlvblNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9sb2NhbGl6YXRpb24uc2VydmljZSc7XHJcblxyXG5leHBvcnQgY2xhc3MgTG9jYWxlSWQgZXh0ZW5kcyBTdHJpbmcge1xyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgbG9jYWxpemF0aW9uU2VydmljZTogTG9jYWxpemF0aW9uU2VydmljZSkge1xyXG4gICAgc3VwZXIoKTtcclxuICB9XHJcblxyXG4gIHRvU3RyaW5nKCk6IHN0cmluZyB7XHJcbiAgICBjb25zdCB7IGN1cnJlbnRMYW5nIH0gPSB0aGlzLmxvY2FsaXphdGlvblNlcnZpY2U7XHJcbiAgICByZXR1cm4gbG9jYWxlc01hcHBpbmdbY3VycmVudExhbmddIHx8IGN1cnJlbnRMYW5nO1xyXG4gIH1cclxuXHJcbiAgdmFsdWVPZigpOiBzdHJpbmcge1xyXG4gICAgcmV0dXJuIHRoaXMudG9TdHJpbmcoKTtcclxuICB9XHJcbn1cclxuXHJcbmV4cG9ydCBjb25zdCBMb2NhbGVQcm92aWRlcjogUHJvdmlkZXIgPSB7XHJcbiAgcHJvdmlkZTogTE9DQUxFX0lELFxyXG4gIHVzZUNsYXNzOiBMb2NhbGVJZCxcclxuICBkZXBzOiBbTG9jYWxpemF0aW9uU2VydmljZV0sXHJcbn07XHJcbiJdfQ==