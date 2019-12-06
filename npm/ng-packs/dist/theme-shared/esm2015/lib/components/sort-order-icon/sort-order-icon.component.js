/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/sort-order-icon/sort-order-icon.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, EventEmitter, Input, Output } from '@angular/core';
export class SortOrderIconComponent {
    constructor() {
        this.selectedKeyChange = new EventEmitter();
        this.selectedSortKeyChange = new EventEmitter();
        this.orderChange = new EventEmitter();
    }
    /**
     * @deprecated use selectedSortKey instead.
     * @param {?} value
     * @return {?}
     */
    set selectedKey(value) {
        this.selectedSortKey = value;
        this.selectedKeyChange.emit(value);
    }
    /**
     * @return {?}
     */
    get selectedKey() {
        return this._selectedSortKey;
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set selectedSortKey(value) {
        this._selectedSortKey = value;
        this.selectedSortKeyChange.emit(value);
    }
    /**
     * @return {?}
     */
    get selectedSortKey() {
        return this._selectedSortKey;
    }
    /**
     * @deprecated use sortKey instead.
     * @return {?}
     */
    get key() {
        return this.sortKey;
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set key(value) {
        this.sortKey = value;
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set order(value) {
        this._order = value;
        this.orderChange.emit(value);
    }
    /**
     * @return {?}
     */
    get order() {
        return this._order;
    }
    /**
     * @return {?}
     */
    get icon() {
        if (!this.selectedSortKey)
            return 'fa-sort';
        if (this.selectedSortKey === this.sortKey)
            return `fa-sort-${this.order}`;
        else
            return '';
    }
    /**
     * @param {?} key
     * @return {?}
     */
    sort(key) {
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
    }
}
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC1vcmRlci1pY29uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc29ydC1vcmRlci1pY29uL3NvcnQtb3JkZXItaWNvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLFlBQVksRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBTXZFLE1BQU0sT0FBTyxzQkFBc0I7SUFKbkM7UUE2QnFCLHNCQUFpQixHQUFHLElBQUksWUFBWSxFQUFVLENBQUM7UUFDL0MsMEJBQXFCLEdBQUcsSUFBSSxZQUFZLEVBQVUsQ0FBQztRQXlCbkQsZ0JBQVcsR0FBRyxJQUFJLFlBQVksRUFBVSxDQUFDO0lBOEI5RCxDQUFDOzs7Ozs7SUExRUMsSUFDSSxXQUFXLENBQUMsS0FBYTtRQUMzQixJQUFJLENBQUMsZUFBZSxHQUFHLEtBQUssQ0FBQztRQUM3QixJQUFJLENBQUMsaUJBQWlCLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO0lBQ3JDLENBQUM7Ozs7SUFDRCxJQUFJLFdBQVc7UUFDYixPQUFPLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQztJQUMvQixDQUFDOzs7OztJQUVELElBQ0ksZUFBZSxDQUFDLEtBQWE7UUFDL0IsSUFBSSxDQUFDLGdCQUFnQixHQUFHLEtBQUssQ0FBQztRQUM5QixJQUFJLENBQUMscUJBQXFCLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO0lBQ3pDLENBQUM7Ozs7SUFDRCxJQUFJLGVBQWU7UUFDakIsT0FBTyxJQUFJLENBQUMsZ0JBQWdCLENBQUM7SUFDL0IsQ0FBQzs7Ozs7SUFRRCxJQUNJLEdBQUc7UUFDTCxPQUFPLElBQUksQ0FBQyxPQUFPLENBQUM7SUFDdEIsQ0FBQzs7Ozs7SUFDRCxJQUFJLEdBQUcsQ0FBQyxLQUFhO1FBQ25CLElBQUksQ0FBQyxPQUFPLEdBQUcsS0FBSyxDQUFDO0lBQ3ZCLENBQUM7Ozs7O0lBS0QsSUFDSSxLQUFLLENBQUMsS0FBMEI7UUFDbEMsSUFBSSxDQUFDLE1BQU0sR0FBRyxLQUFLLENBQUM7UUFDcEIsSUFBSSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7SUFDL0IsQ0FBQzs7OztJQUNELElBQUksS0FBSztRQUNQLE9BQU8sSUFBSSxDQUFDLE1BQU0sQ0FBQztJQUNyQixDQUFDOzs7O0lBT0QsSUFBSSxJQUFJO1FBQ04sSUFBSSxDQUFDLElBQUksQ0FBQyxlQUFlO1lBQUUsT0FBTyxTQUFTLENBQUM7UUFDNUMsSUFBSSxJQUFJLENBQUMsZUFBZSxLQUFLLElBQUksQ0FBQyxPQUFPO1lBQUUsT0FBTyxXQUFXLElBQUksQ0FBQyxLQUFLLEVBQUUsQ0FBQzs7WUFDckUsT0FBTyxFQUFFLENBQUM7SUFDakIsQ0FBQzs7Ozs7SUFFRCxJQUFJLENBQUMsR0FBVztRQUNkLElBQUksQ0FBQyxXQUFXLEdBQUcsR0FBRyxDQUFDLENBQUMsc0JBQXNCO1FBQzlDLElBQUksQ0FBQyxlQUFlLEdBQUcsR0FBRyxDQUFDO1FBQzNCLFFBQVEsSUFBSSxDQUFDLEtBQUssRUFBRTtZQUNsQixLQUFLLEVBQUU7Z0JBQ0wsSUFBSSxDQUFDLEtBQUssR0FBRyxLQUFLLENBQUM7Z0JBQ25CLElBQUksQ0FBQyxXQUFXLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO2dCQUM3QixNQUFNO1lBQ1IsS0FBSyxLQUFLO2dCQUNSLElBQUksQ0FBQyxLQUFLLEdBQUcsTUFBTSxDQUFDO2dCQUNwQixJQUFJLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsQ0FBQztnQkFDOUIsTUFBTTtZQUNSLEtBQUssTUFBTTtnQkFDVCxJQUFJLENBQUMsS0FBSyxHQUFHLEVBQUUsQ0FBQztnQkFDaEIsSUFBSSxDQUFDLFdBQVcsR0FBRyxFQUFFLENBQUMsQ0FBQyxzQkFBc0I7Z0JBQzdDLElBQUksQ0FBQyxXQUFXLENBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxDQUFDO2dCQUMxQixNQUFNO1NBQ1Q7SUFDSCxDQUFDOzs7WUFwRkYsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxxQkFBcUI7Z0JBQy9CLHNHQUErQzthQUNoRDs7OzBCQVFFLEtBQUs7OEJBU0wsS0FBSztnQ0FTTCxNQUFNO29DQUNOLE1BQU07a0JBS04sS0FBSztzQkFRTCxLQUFLO29CQUdMLEtBQUs7MEJBU0wsTUFBTTt3QkFFTixLQUFLOzs7Ozs7O0lBcEROLHdDQUFvQzs7Ozs7SUFDcEMsa0RBQWlDOztJQXVCakMsbURBQWtFOztJQUNsRSx1REFBc0U7O0lBYXRFLHlDQUNnQjs7SUFXaEIsNkNBQTREOztJQUU1RCwyQ0FDa0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIEV2ZW50RW1pdHRlciwgSW5wdXQsIE91dHB1dCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtc29ydC1vcmRlci1pY29uJyxcbiAgdGVtcGxhdGVVcmw6ICcuL3NvcnQtb3JkZXItaWNvbi5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFNvcnRPcmRlckljb25Db21wb25lbnQge1xuICBwcml2YXRlIF9vcmRlcjogJ2FzYycgfCAnZGVzYycgfCAnJztcbiAgcHJpdmF0ZSBfc2VsZWN0ZWRTb3J0S2V5OiBzdHJpbmc7XG5cbiAgLyoqXG4gICAqIEBkZXByZWNhdGVkIHVzZSBzZWxlY3RlZFNvcnRLZXkgaW5zdGVhZC5cbiAgICovXG4gIEBJbnB1dCgpXG4gIHNldCBzZWxlY3RlZEtleSh2YWx1ZTogc3RyaW5nKSB7XG4gICAgdGhpcy5zZWxlY3RlZFNvcnRLZXkgPSB2YWx1ZTtcbiAgICB0aGlzLnNlbGVjdGVkS2V5Q2hhbmdlLmVtaXQodmFsdWUpO1xuICB9XG4gIGdldCBzZWxlY3RlZEtleSgpOiBzdHJpbmcge1xuICAgIHJldHVybiB0aGlzLl9zZWxlY3RlZFNvcnRLZXk7XG4gIH1cblxuICBASW5wdXQoKVxuICBzZXQgc2VsZWN0ZWRTb3J0S2V5KHZhbHVlOiBzdHJpbmcpIHtcbiAgICB0aGlzLl9zZWxlY3RlZFNvcnRLZXkgPSB2YWx1ZTtcbiAgICB0aGlzLnNlbGVjdGVkU29ydEtleUNoYW5nZS5lbWl0KHZhbHVlKTtcbiAgfVxuICBnZXQgc2VsZWN0ZWRTb3J0S2V5KCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHRoaXMuX3NlbGVjdGVkU29ydEtleTtcbiAgfVxuXG4gIEBPdXRwdXQoKSByZWFkb25seSBzZWxlY3RlZEtleUNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8c3RyaW5nPigpO1xuICBAT3V0cHV0KCkgcmVhZG9ubHkgc2VsZWN0ZWRTb3J0S2V5Q2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxzdHJpbmc+KCk7XG5cbiAgLyoqXG4gICAqIEBkZXByZWNhdGVkIHVzZSBzb3J0S2V5IGluc3RlYWQuXG4gICAqL1xuICBASW5wdXQoKVxuICBnZXQga2V5KCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHRoaXMuc29ydEtleTtcbiAgfVxuICBzZXQga2V5KHZhbHVlOiBzdHJpbmcpIHtcbiAgICB0aGlzLnNvcnRLZXkgPSB2YWx1ZTtcbiAgfVxuXG4gIEBJbnB1dCgpXG4gIHNvcnRLZXk6IHN0cmluZztcblxuICBASW5wdXQoKVxuICBzZXQgb3JkZXIodmFsdWU6ICdhc2MnIHwgJ2Rlc2MnIHwgJycpIHtcbiAgICB0aGlzLl9vcmRlciA9IHZhbHVlO1xuICAgIHRoaXMub3JkZXJDaGFuZ2UuZW1pdCh2YWx1ZSk7XG4gIH1cbiAgZ2V0IG9yZGVyKCk6ICdhc2MnIHwgJ2Rlc2MnIHwgJycge1xuICAgIHJldHVybiB0aGlzLl9vcmRlcjtcbiAgfVxuXG4gIEBPdXRwdXQoKSByZWFkb25seSBvcmRlckNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8c3RyaW5nPigpO1xuXG4gIEBJbnB1dCgpXG4gIGljb25DbGFzczogc3RyaW5nO1xuXG4gIGdldCBpY29uKCk6IHN0cmluZyB7XG4gICAgaWYgKCF0aGlzLnNlbGVjdGVkU29ydEtleSkgcmV0dXJuICdmYS1zb3J0JztcbiAgICBpZiAodGhpcy5zZWxlY3RlZFNvcnRLZXkgPT09IHRoaXMuc29ydEtleSkgcmV0dXJuIGBmYS1zb3J0LSR7dGhpcy5vcmRlcn1gO1xuICAgIGVsc2UgcmV0dXJuICcnO1xuICB9XG5cbiAgc29ydChrZXk6IHN0cmluZykge1xuICAgIHRoaXMuc2VsZWN0ZWRLZXkgPSBrZXk7IC8vIFRPRE86IFRvIGJlIHJlbW92ZWRcbiAgICB0aGlzLnNlbGVjdGVkU29ydEtleSA9IGtleTtcbiAgICBzd2l0Y2ggKHRoaXMub3JkZXIpIHtcbiAgICAgIGNhc2UgJyc6XG4gICAgICAgIHRoaXMub3JkZXIgPSAnYXNjJztcbiAgICAgICAgdGhpcy5vcmRlckNoYW5nZS5lbWl0KCdhc2MnKTtcbiAgICAgICAgYnJlYWs7XG4gICAgICBjYXNlICdhc2MnOlxuICAgICAgICB0aGlzLm9yZGVyID0gJ2Rlc2MnO1xuICAgICAgICB0aGlzLm9yZGVyQ2hhbmdlLmVtaXQoJ2Rlc2MnKTtcbiAgICAgICAgYnJlYWs7XG4gICAgICBjYXNlICdkZXNjJzpcbiAgICAgICAgdGhpcy5vcmRlciA9ICcnO1xuICAgICAgICB0aGlzLnNlbGVjdGVkS2V5ID0gJyc7IC8vIFRPRE86IFRvIGJlIHJlbW92ZWRcbiAgICAgICAgdGhpcy5vcmRlckNoYW5nZS5lbWl0KCcnKTtcbiAgICAgICAgYnJlYWs7XG4gICAgfVxuICB9XG59XG4iXX0=