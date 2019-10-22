/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxlLnByb3ZpZGVyLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3Byb3ZpZGVycy9sb2NhbGUucHJvdmlkZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQVksTUFBTSxlQUFlLENBQUM7QUFDcEQsT0FBTyxjQUFjLE1BQU0sZ0NBQWdDLENBQUM7QUFDNUQsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0sa0NBQWtDLENBQUM7QUFFdkUsTUFBTSxPQUFPLFFBQVMsU0FBUSxNQUFNOzs7O0lBQ2xDLFlBQW9CLG1CQUF3QztRQUMxRCxLQUFLLEVBQUUsQ0FBQztRQURVLHdCQUFtQixHQUFuQixtQkFBbUIsQ0FBcUI7SUFFNUQsQ0FBQzs7OztJQUVELFFBQVE7Y0FDQSxFQUFFLFdBQVcsRUFBRSxHQUFHLElBQUksQ0FBQyxtQkFBbUI7UUFDaEQsT0FBTyxjQUFjLENBQUMsV0FBVyxDQUFDLElBQUksV0FBVyxDQUFDO0lBQ3BELENBQUM7Ozs7SUFFRCxPQUFPO1FBQ0wsT0FBTyxJQUFJLENBQUMsUUFBUSxFQUFFLENBQUM7SUFDekIsQ0FBQztDQUNGOzs7Ozs7SUFaYSx1Q0FBZ0Q7OztBQWM5RCxNQUFNLE9BQU8sY0FBYyxHQUFhO0lBQ3RDLE9BQU8sRUFBRSxTQUFTO0lBQ2xCLFFBQVEsRUFBRSxRQUFRO0lBQ2xCLElBQUksRUFBRSxDQUFDLG1CQUFtQixDQUFDO0NBQzVCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgTE9DQUxFX0lELCBQcm92aWRlciB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgbG9jYWxlc01hcHBpbmcgZnJvbSAnLi4vY29uc3RhbnRzL2RpZmZlcmVudC1sb2NhbGVzJztcclxuaW1wb3J0IHsgTG9jYWxpemF0aW9uU2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL2xvY2FsaXphdGlvbi5zZXJ2aWNlJztcclxuXHJcbmV4cG9ydCBjbGFzcyBMb2NhbGVJZCBleHRlbmRzIFN0cmluZyB7XHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBsb2NhbGl6YXRpb25TZXJ2aWNlOiBMb2NhbGl6YXRpb25TZXJ2aWNlKSB7XHJcbiAgICBzdXBlcigpO1xyXG4gIH1cclxuXHJcbiAgdG9TdHJpbmcoKTogc3RyaW5nIHtcclxuICAgIGNvbnN0IHsgY3VycmVudExhbmcgfSA9IHRoaXMubG9jYWxpemF0aW9uU2VydmljZTtcclxuICAgIHJldHVybiBsb2NhbGVzTWFwcGluZ1tjdXJyZW50TGFuZ10gfHwgY3VycmVudExhbmc7XHJcbiAgfVxyXG5cclxuICB2YWx1ZU9mKCk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gdGhpcy50b1N0cmluZygpO1xyXG4gIH1cclxufVxyXG5cclxuZXhwb3J0IGNvbnN0IExvY2FsZVByb3ZpZGVyOiBQcm92aWRlciA9IHtcclxuICBwcm92aWRlOiBMT0NBTEVfSUQsXHJcbiAgdXNlQ2xhc3M6IExvY2FsZUlkLFxyXG4gIGRlcHM6IFtMb2NhbGl6YXRpb25TZXJ2aWNlXSxcclxufTtcclxuIl19