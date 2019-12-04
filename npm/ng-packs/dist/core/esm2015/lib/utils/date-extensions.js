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
    const timezoneOffset = this.getTimezoneOffset();
    return new Date(this.getTime() - timezoneOffset * 60000).toISOString();
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZGF0ZS1leHRlbnNpb25zLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3V0aWxzL2RhdGUtZXh0ZW5zaW9ucy50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQVFBLElBQUksQ0FBQyxTQUFTLENBQUMsZ0JBQWdCOzs7O0FBQUc7O1VBQzFCLGNBQWMsR0FBRyxJQUFJLENBQUMsaUJBQWlCLEVBQUU7SUFFL0MsT0FBTyxJQUFJLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxFQUFFLEdBQUcsY0FBYyxHQUFHLEtBQUssQ0FBQyxDQUFDLFdBQVcsRUFBRSxDQUFDO0FBQ3pFLENBQUMsQ0FBQSxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiZXhwb3J0IHt9O1xuXG5kZWNsYXJlIGdsb2JhbCB7XG4gIGludGVyZmFjZSBEYXRlIHtcbiAgICB0b0xvY2FsSVNPU3RyaW5nKCk6IHN0cmluZztcbiAgfVxufVxuXG5EYXRlLnByb3RvdHlwZS50b0xvY2FsSVNPU3RyaW5nID0gZnVuY3Rpb24odGhpczogRGF0ZSk6IHN0cmluZyB7XG4gIGNvbnN0IHRpbWV6b25lT2Zmc2V0ID0gdGhpcy5nZXRUaW1lem9uZU9mZnNldCgpO1xuXG4gIHJldHVybiBuZXcgRGF0ZSh0aGlzLmdldFRpbWUoKSAtIHRpbWV6b25lT2Zmc2V0ICogNjAwMDApLnRvSVNPU3RyaW5nKCk7XG59O1xuIl19