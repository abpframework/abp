/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZGF0ZS1leHRlbnNpb25zLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3V0aWxzL2RhdGUtZXh0ZW5zaW9ucy50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBUUEsSUFBSSxDQUFDLFNBQVMsQ0FBQyxnQkFBZ0I7Ozs7QUFBRzs7VUFDMUIsY0FBYyxHQUFHLElBQUksQ0FBQyxpQkFBaUIsRUFBRTtJQUUvQyxPQUFPLElBQUksSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLEVBQUUsR0FBRyxjQUFjLEdBQUcsS0FBSyxDQUFDLENBQUMsV0FBVyxFQUFFLENBQUM7QUFDekUsQ0FBQyxDQUFBLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJleHBvcnQge307XHJcblxyXG5kZWNsYXJlIGdsb2JhbCB7XHJcbiAgaW50ZXJmYWNlIERhdGUge1xyXG4gICAgdG9Mb2NhbElTT1N0cmluZygpOiBzdHJpbmc7XHJcbiAgfVxyXG59XHJcblxyXG5EYXRlLnByb3RvdHlwZS50b0xvY2FsSVNPU3RyaW5nID0gZnVuY3Rpb24odGhpczogRGF0ZSk6IHN0cmluZyB7XHJcbiAgY29uc3QgdGltZXpvbmVPZmZzZXQgPSB0aGlzLmdldFRpbWV6b25lT2Zmc2V0KCk7XHJcblxyXG4gIHJldHVybiBuZXcgRGF0ZSh0aGlzLmdldFRpbWUoKSAtIHRpbWV6b25lT2Zmc2V0ICogNjAwMDApLnRvSVNPU3RyaW5nKCk7XHJcbn07XHJcbiJdfQ==