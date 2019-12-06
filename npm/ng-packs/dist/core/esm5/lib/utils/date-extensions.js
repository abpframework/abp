/**
 * @fileoverview added by tsickle
 * Generated from: lib/utils/date-extensions.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
Date.prototype.toLocalISOString = (/**
 * @this {?}
 * @return {?}
 */
function () {
    /** @type {?} */
    var timezoneOffset = this.getTimezoneOffset();
    return new Date(this.getTime() - timezoneOffset * 60000).toISOString();
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZGF0ZS1leHRlbnNpb25zLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3V0aWxzL2RhdGUtZXh0ZW5zaW9ucy50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQVFBLElBQUksQ0FBQyxTQUFTLENBQUMsZ0JBQWdCOzs7O0FBQUc7O1FBQzFCLGNBQWMsR0FBRyxJQUFJLENBQUMsaUJBQWlCLEVBQUU7SUFFL0MsT0FBTyxJQUFJLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxFQUFFLEdBQUcsY0FBYyxHQUFHLEtBQUssQ0FBQyxDQUFDLFdBQVcsRUFBRSxDQUFDO0FBQ3pFLENBQUMsQ0FBQSxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiZXhwb3J0IHt9O1xyXG5cclxuZGVjbGFyZSBnbG9iYWwge1xyXG4gIGludGVyZmFjZSBEYXRlIHtcclxuICAgIHRvTG9jYWxJU09TdHJpbmcoKTogc3RyaW5nO1xyXG4gIH1cclxufVxyXG5cclxuRGF0ZS5wcm90b3R5cGUudG9Mb2NhbElTT1N0cmluZyA9IGZ1bmN0aW9uKHRoaXM6IERhdGUpOiBzdHJpbmcge1xyXG4gIGNvbnN0IHRpbWV6b25lT2Zmc2V0ID0gdGhpcy5nZXRUaW1lem9uZU9mZnNldCgpO1xyXG5cclxuICByZXR1cm4gbmV3IERhdGUodGhpcy5nZXRUaW1lKCkgLSB0aW1lem9uZU9mZnNldCAqIDYwMDAwKS50b0lTT1N0cmluZygpO1xyXG59O1xyXG4iXX0=