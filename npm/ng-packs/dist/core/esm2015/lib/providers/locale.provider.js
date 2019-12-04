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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxlLnByb3ZpZGVyLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3Byb3ZpZGVycy9sb2NhbGUucHJvdmlkZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFZLE1BQU0sZUFBZSxDQUFDO0FBQ3BELE9BQU8sY0FBYyxNQUFNLGdDQUFnQyxDQUFDO0FBQzVELE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLGtDQUFrQyxDQUFDO0FBRXZFLE1BQU0sT0FBTyxRQUFTLFNBQVEsTUFBTTs7OztJQUNsQyxZQUFvQixtQkFBd0M7UUFDMUQsS0FBSyxFQUFFLENBQUM7UUFEVSx3QkFBbUIsR0FBbkIsbUJBQW1CLENBQXFCO0lBRTVELENBQUM7Ozs7SUFFRCxRQUFRO2NBQ0EsRUFBRSxXQUFXLEVBQUUsR0FBRyxJQUFJLENBQUMsbUJBQW1CO1FBQ2hELE9BQU8sY0FBYyxDQUFDLFdBQVcsQ0FBQyxJQUFJLFdBQVcsQ0FBQztJQUNwRCxDQUFDOzs7O0lBRUQsT0FBTztRQUNMLE9BQU8sSUFBSSxDQUFDLFFBQVEsRUFBRSxDQUFDO0lBQ3pCLENBQUM7Q0FDRjs7Ozs7O0lBWmEsdUNBQWdEOzs7QUFjOUQsTUFBTSxPQUFPLGNBQWMsR0FBYTtJQUN0QyxPQUFPLEVBQUUsU0FBUztJQUNsQixRQUFRLEVBQUUsUUFBUTtJQUNsQixJQUFJLEVBQUUsQ0FBQyxtQkFBbUIsQ0FBQztDQUM1QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IExPQ0FMRV9JRCwgUHJvdmlkZXIgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCBsb2NhbGVzTWFwcGluZyBmcm9tICcuLi9jb25zdGFudHMvZGlmZmVyZW50LWxvY2FsZXMnO1xuaW1wb3J0IHsgTG9jYWxpemF0aW9uU2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL2xvY2FsaXphdGlvbi5zZXJ2aWNlJztcblxuZXhwb3J0IGNsYXNzIExvY2FsZUlkIGV4dGVuZHMgU3RyaW5nIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBsb2NhbGl6YXRpb25TZXJ2aWNlOiBMb2NhbGl6YXRpb25TZXJ2aWNlKSB7XG4gICAgc3VwZXIoKTtcbiAgfVxuXG4gIHRvU3RyaW5nKCk6IHN0cmluZyB7XG4gICAgY29uc3QgeyBjdXJyZW50TGFuZyB9ID0gdGhpcy5sb2NhbGl6YXRpb25TZXJ2aWNlO1xuICAgIHJldHVybiBsb2NhbGVzTWFwcGluZ1tjdXJyZW50TGFuZ10gfHwgY3VycmVudExhbmc7XG4gIH1cblxuICB2YWx1ZU9mKCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHRoaXMudG9TdHJpbmcoKTtcbiAgfVxufVxuXG5leHBvcnQgY29uc3QgTG9jYWxlUHJvdmlkZXI6IFByb3ZpZGVyID0ge1xuICBwcm92aWRlOiBMT0NBTEVfSUQsXG4gIHVzZUNsYXNzOiBMb2NhbGVJZCxcbiAgZGVwczogW0xvY2FsaXphdGlvblNlcnZpY2VdLFxufTtcbiJdfQ==