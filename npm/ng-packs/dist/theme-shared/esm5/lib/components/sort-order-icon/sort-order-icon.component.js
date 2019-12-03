/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, EventEmitter, Input, Output } from '@angular/core';
var SortOrderIconComponent = /** @class */ (function () {
    function SortOrderIconComponent() {
        this.selectedKeyChange = new EventEmitter();
        this.selectedSortKeyChange = new EventEmitter();
        this.orderChange = new EventEmitter();
    }
    Object.defineProperty(SortOrderIconComponent.prototype, "selectedKey", {
        get: /**
         * @return {?}
         */
        function () {
            return this._selectedSortKey;
        },
        /**
         * @deprecated use selectedSortKey instead.
         */
        set: /**
         * @deprecated use selectedSortKey instead.
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this.selectedSortKey = value;
            this.selectedKeyChange.emit(value);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SortOrderIconComponent.prototype, "selectedSortKey", {
        get: /**
         * @return {?}
         */
        function () {
            return this._selectedSortKey;
        },
        set: /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this._selectedSortKey = value;
            this.selectedSortKeyChange.emit(value);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SortOrderIconComponent.prototype, "key", {
        /**
         * @deprecated use sortKey instead.
         */
        get: /**
         * @deprecated use sortKey instead.
         * @return {?}
         */
        function () {
            return this.sortKey;
        },
        set: /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this.sortKey = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SortOrderIconComponent.prototype, "order", {
        get: /**
         * @return {?}
         */
        function () {
            return this._order;
        },
        set: /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this._order = value;
            this.orderChange.emit(value);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SortOrderIconComponent.prototype, "icon", {
        get: /**
         * @return {?}
         */
        function () {
            if (!this.selectedSortKey)
                return 'fa-sort';
            if (this.selectedSortKey === this.sortKey)
                return "fa-sort-" + this.order;
            else
                return '';
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @param {?} key
     * @return {?}
     */
    SortOrderIconComponent.prototype.sort = /**
     * @param {?} key
     * @return {?}
     */
    function (key) {
        this.selectedKey = key; // TODO: To be removed
        this.selectedSortKey = key;
        switch (this.order) {
            case '':
                this.order = 'asc';
                this.orderChange.emit('asc');
                break;
            case 'asc':
                this.order = 'desc';
                this.orderChange.emit('desc');
                break;
            case 'desc':
                this.order = '';
                this.selectedKey = ''; // TODO: To be removed
                this.orderChange.emit('');
                break;
        }
    };
    SortOrderIconComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-sort-order-icon',
                    template: "<span class=\"float-right {{ iconClass }}\">\n  <i class=\"fa {{ icon }}\"></i>\n</span>\n"
                }] }
    ];
    SortOrderIconComponent.propDecorators = {
        selectedKey: [{ type: Input }],
        selectedSortKey: [{ type: Input }],
        selectedKeyChange: [{ type: Output }],
        selectedSortKeyChange: [{ type: Output }],
        key: [{ type: Input }],
        sortKey: [{ type: Input }],
        order: [{ type: Input }],
        orderChange: [{ type: Output }],
        iconClass: [{ type: Input }]
    };
    return SortOrderIconComponent;
}());
export { SortOrderIconComponent };
if (false) {
    /**
     * @type {?}
     * @private
     */
    SortOrderIconComponent.prototype._order;
    /**
     * @type {?}
     * @private
     */
    SortOrderIconComponent.prototype._selectedSortKey;
    /** @type {?} */
    SortOrderIconComponent.prototype.selectedKeyChange;
    /** @type {?} */
    SortOrderIconComponent.prototype.selectedSortKeyChange;
    /** @type {?} */
    SortOrderIconComponent.prototype.sortKey;
    /** @type {?} */
    SortOrderIconComponent.prototype.orderChange;
    /** @type {?} */
    SortOrderIconComponent.prototype.iconClass;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC1vcmRlci1pY29uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc29ydC1vcmRlci1pY29uL3NvcnQtb3JkZXItaWNvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFdkU7SUFBQTtRQTZCcUIsc0JBQWlCLEdBQUcsSUFBSSxZQUFZLEVBQVUsQ0FBQztRQUMvQywwQkFBcUIsR0FBRyxJQUFJLFlBQVksRUFBVSxDQUFDO1FBeUJuRCxnQkFBVyxHQUFHLElBQUksWUFBWSxFQUFVLENBQUM7SUE4QjlELENBQUM7SUExRUMsc0JBQ0ksK0NBQVc7Ozs7UUFJZjtZQUNFLE9BQU8sSUFBSSxDQUFDLGdCQUFnQixDQUFDO1FBQy9CLENBQUM7UUFWRDs7V0FFRzs7Ozs7O1FBQ0gsVUFDZ0IsS0FBYTtZQUMzQixJQUFJLENBQUMsZUFBZSxHQUFHLEtBQUssQ0FBQztZQUM3QixJQUFJLENBQUMsaUJBQWlCLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO1FBQ3JDLENBQUM7OztPQUFBO0lBS0Qsc0JBQ0ksbURBQWU7Ozs7UUFJbkI7WUFDRSxPQUFPLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQztRQUMvQixDQUFDOzs7OztRQVBELFVBQ29CLEtBQWE7WUFDL0IsSUFBSSxDQUFDLGdCQUFnQixHQUFHLEtBQUssQ0FBQztZQUM5QixJQUFJLENBQUMscUJBQXFCLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO1FBQ3pDLENBQUM7OztPQUFBO0lBV0Qsc0JBQ0ksdUNBQUc7UUFKUDs7V0FFRzs7Ozs7UUFDSDtZQUVFLE9BQU8sSUFBSSxDQUFDLE9BQU8sQ0FBQztRQUN0QixDQUFDOzs7OztRQUNELFVBQVEsS0FBYTtZQUNuQixJQUFJLENBQUMsT0FBTyxHQUFHLEtBQUssQ0FBQztRQUN2QixDQUFDOzs7T0FIQTtJQVFELHNCQUNJLHlDQUFLOzs7O1FBSVQ7WUFDRSxPQUFPLElBQUksQ0FBQyxNQUFNLENBQUM7UUFDckIsQ0FBQzs7Ozs7UUFQRCxVQUNVLEtBQTBCO1lBQ2xDLElBQUksQ0FBQyxNQUFNLEdBQUcsS0FBSyxDQUFDO1lBQ3BCLElBQUksQ0FBQyxXQUFXLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO1FBQy9CLENBQUM7OztPQUFBO0lBVUQsc0JBQUksd0NBQUk7Ozs7UUFBUjtZQUNFLElBQUksQ0FBQyxJQUFJLENBQUMsZUFBZTtnQkFBRSxPQUFPLFNBQVMsQ0FBQztZQUM1QyxJQUFJLElBQUksQ0FBQyxlQUFlLEtBQUssSUFBSSxDQUFDLE9BQU87Z0JBQUUsT0FBTyxhQUFXLElBQUksQ0FBQyxLQUFPLENBQUM7O2dCQUNyRSxPQUFPLEVBQUUsQ0FBQztRQUNqQixDQUFDOzs7T0FBQTs7Ozs7SUFFRCxxQ0FBSTs7OztJQUFKLFVBQUssR0FBVztRQUNkLElBQUksQ0FBQyxXQUFXLEdBQUcsR0FBRyxDQUFDLENBQUMsc0JBQXNCO1FBQzlDLElBQUksQ0FBQyxlQUFlLEdBQUcsR0FBRyxDQUFDO1FBQzNCLFFBQVEsSUFBSSxDQUFDLEtBQUssRUFBRTtZQUNsQixLQUFLLEVBQUU7Z0JBQ0wsSUFBSSxDQUFDLEtBQUssR0FBRyxLQUFLLENBQUM7Z0JBQ25CLElBQUksQ0FBQyxXQUFXLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO2dCQUM3QixNQUFNO1lBQ1IsS0FBSyxLQUFLO2dCQUNSLElBQUksQ0FBQyxLQUFLLEdBQUcsTUFBTSxDQUFDO2dCQUNwQixJQUFJLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsQ0FBQztnQkFDOUIsTUFBTTtZQUNSLEtBQUssTUFBTTtnQkFDVCxJQUFJLENBQUMsS0FBSyxHQUFHLEVBQUUsQ0FBQztnQkFDaEIsSUFBSSxDQUFDLFdBQVcsR0FBRyxFQUFFLENBQUMsQ0FBQyxzQkFBc0I7Z0JBQzdDLElBQUksQ0FBQyxXQUFXLENBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxDQUFDO2dCQUMxQixNQUFNO1NBQ1Q7SUFDSCxDQUFDOztnQkFwRkYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxxQkFBcUI7b0JBQy9CLHNHQUErQztpQkFDaEQ7Ozs4QkFRRSxLQUFLO2tDQVNMLEtBQUs7b0NBU0wsTUFBTTt3Q0FDTixNQUFNO3NCQUtOLEtBQUs7MEJBUUwsS0FBSzt3QkFHTCxLQUFLOzhCQVNMLE1BQU07NEJBRU4sS0FBSzs7SUE0QlIsNkJBQUM7Q0FBQSxBQXJGRCxJQXFGQztTQWpGWSxzQkFBc0I7Ozs7OztJQUNqQyx3Q0FBb0M7Ozs7O0lBQ3BDLGtEQUFpQzs7SUF1QmpDLG1EQUFrRTs7SUFDbEUsdURBQXNFOztJQWF0RSx5Q0FDZ0I7O0lBV2hCLDZDQUE0RDs7SUFFNUQsMkNBQ2tCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBFdmVudEVtaXR0ZXIsIElucHV0LCBPdXRwdXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLXNvcnQtb3JkZXItaWNvbicsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL3NvcnQtb3JkZXItaWNvbi5jb21wb25lbnQuaHRtbCcsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBTb3J0T3JkZXJJY29uQ29tcG9uZW50IHtcclxuICBwcml2YXRlIF9vcmRlcjogJ2FzYycgfCAnZGVzYycgfCAnJztcclxuICBwcml2YXRlIF9zZWxlY3RlZFNvcnRLZXk6IHN0cmluZztcclxuXHJcbiAgLyoqXHJcbiAgICogQGRlcHJlY2F0ZWQgdXNlIHNlbGVjdGVkU29ydEtleSBpbnN0ZWFkLlxyXG4gICAqL1xyXG4gIEBJbnB1dCgpXHJcbiAgc2V0IHNlbGVjdGVkS2V5KHZhbHVlOiBzdHJpbmcpIHtcclxuICAgIHRoaXMuc2VsZWN0ZWRTb3J0S2V5ID0gdmFsdWU7XHJcbiAgICB0aGlzLnNlbGVjdGVkS2V5Q2hhbmdlLmVtaXQodmFsdWUpO1xyXG4gIH1cclxuICBnZXQgc2VsZWN0ZWRLZXkoKTogc3RyaW5nIHtcclxuICAgIHJldHVybiB0aGlzLl9zZWxlY3RlZFNvcnRLZXk7XHJcbiAgfVxyXG5cclxuICBASW5wdXQoKVxyXG4gIHNldCBzZWxlY3RlZFNvcnRLZXkodmFsdWU6IHN0cmluZykge1xyXG4gICAgdGhpcy5fc2VsZWN0ZWRTb3J0S2V5ID0gdmFsdWU7XHJcbiAgICB0aGlzLnNlbGVjdGVkU29ydEtleUNoYW5nZS5lbWl0KHZhbHVlKTtcclxuICB9XHJcbiAgZ2V0IHNlbGVjdGVkU29ydEtleSgpOiBzdHJpbmcge1xyXG4gICAgcmV0dXJuIHRoaXMuX3NlbGVjdGVkU29ydEtleTtcclxuICB9XHJcblxyXG4gIEBPdXRwdXQoKSByZWFkb25seSBzZWxlY3RlZEtleUNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8c3RyaW5nPigpO1xyXG4gIEBPdXRwdXQoKSByZWFkb25seSBzZWxlY3RlZFNvcnRLZXlDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPHN0cmluZz4oKTtcclxuXHJcbiAgLyoqXHJcbiAgICogQGRlcHJlY2F0ZWQgdXNlIHNvcnRLZXkgaW5zdGVhZC5cclxuICAgKi9cclxuICBASW5wdXQoKVxyXG4gIGdldCBrZXkoKTogc3RyaW5nIHtcclxuICAgIHJldHVybiB0aGlzLnNvcnRLZXk7XHJcbiAgfVxyXG4gIHNldCBrZXkodmFsdWU6IHN0cmluZykge1xyXG4gICAgdGhpcy5zb3J0S2V5ID0gdmFsdWU7XHJcbiAgfVxyXG5cclxuICBASW5wdXQoKVxyXG4gIHNvcnRLZXk6IHN0cmluZztcclxuXHJcbiAgQElucHV0KClcclxuICBzZXQgb3JkZXIodmFsdWU6ICdhc2MnIHwgJ2Rlc2MnIHwgJycpIHtcclxuICAgIHRoaXMuX29yZGVyID0gdmFsdWU7XHJcbiAgICB0aGlzLm9yZGVyQ2hhbmdlLmVtaXQodmFsdWUpO1xyXG4gIH1cclxuICBnZXQgb3JkZXIoKTogJ2FzYycgfCAnZGVzYycgfCAnJyB7XHJcbiAgICByZXR1cm4gdGhpcy5fb3JkZXI7XHJcbiAgfVxyXG5cclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgb3JkZXJDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPHN0cmluZz4oKTtcclxuXHJcbiAgQElucHV0KClcclxuICBpY29uQ2xhc3M6IHN0cmluZztcclxuXHJcbiAgZ2V0IGljb24oKTogc3RyaW5nIHtcclxuICAgIGlmICghdGhpcy5zZWxlY3RlZFNvcnRLZXkpIHJldHVybiAnZmEtc29ydCc7XHJcbiAgICBpZiAodGhpcy5zZWxlY3RlZFNvcnRLZXkgPT09IHRoaXMuc29ydEtleSkgcmV0dXJuIGBmYS1zb3J0LSR7dGhpcy5vcmRlcn1gO1xyXG4gICAgZWxzZSByZXR1cm4gJyc7XHJcbiAgfVxyXG5cclxuICBzb3J0KGtleTogc3RyaW5nKSB7XHJcbiAgICB0aGlzLnNlbGVjdGVkS2V5ID0ga2V5OyAvLyBUT0RPOiBUbyBiZSByZW1vdmVkXHJcbiAgICB0aGlzLnNlbGVjdGVkU29ydEtleSA9IGtleTtcclxuICAgIHN3aXRjaCAodGhpcy5vcmRlcikge1xyXG4gICAgICBjYXNlICcnOlxyXG4gICAgICAgIHRoaXMub3JkZXIgPSAnYXNjJztcclxuICAgICAgICB0aGlzLm9yZGVyQ2hhbmdlLmVtaXQoJ2FzYycpO1xyXG4gICAgICAgIGJyZWFrO1xyXG4gICAgICBjYXNlICdhc2MnOlxyXG4gICAgICAgIHRoaXMub3JkZXIgPSAnZGVzYyc7XHJcbiAgICAgICAgdGhpcy5vcmRlckNoYW5nZS5lbWl0KCdkZXNjJyk7XHJcbiAgICAgICAgYnJlYWs7XHJcbiAgICAgIGNhc2UgJ2Rlc2MnOlxyXG4gICAgICAgIHRoaXMub3JkZXIgPSAnJztcclxuICAgICAgICB0aGlzLnNlbGVjdGVkS2V5ID0gJyc7IC8vIFRPRE86IFRvIGJlIHJlbW92ZWRcclxuICAgICAgICB0aGlzLm9yZGVyQ2hhbmdlLmVtaXQoJycpO1xyXG4gICAgICAgIGJyZWFrO1xyXG4gICAgfVxyXG4gIH1cclxufVxyXG4iXX0=