/**
 * @fileoverview added by tsickle
 * Generated from: lib/pipes/localization.pipe.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Pipe, Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
export class LocalizationPipe {
    /**
     * @param {?} store
     */
    constructor(store) {
        this.store = store;
    }
    /**
     * @param {?=} value
     * @param {...?} interpolateParams
     * @return {?}
     */
    transform(value = '', ...interpolateParams) {
        return this.store.selectSnapshot(ConfigState.getLocalization(value, ...interpolateParams.reduce((/**
         * @param {?} acc
         * @param {?} val
         * @return {?}
         */
        (acc, val) => (Array.isArray(val) ? [...acc, ...val] : [...acc, val])), [])));
    }
}
LocalizationPipe.decorators = [
    { type: Injectable },
    { type: Pipe, args: [{
                name: 'abpLocalization',
            },] }
];
/** @nocollapse */
LocalizationPipe.ctorParameters = () => [
    { type: Store }
];
if (false) {
    /**
     * @type {?}
     * @private
     */
    LocalizationPipe.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnBpcGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvcGlwZXMvbG9jYWxpemF0aW9uLnBpcGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsSUFBSSxFQUFpQixVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDaEUsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUVwQyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sV0FBVyxDQUFDO0FBTXhDLE1BQU0sT0FBTyxnQkFBZ0I7Ozs7SUFDM0IsWUFBb0IsS0FBWTtRQUFaLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7Ozs7SUFFcEMsU0FBUyxDQUFDLFFBQWlELEVBQUUsRUFBRSxHQUFHLGlCQUEyQjtRQUMzRixPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUM5QixXQUFXLENBQUMsZUFBZSxDQUN6QixLQUFLLEVBQ0wsR0FBRyxpQkFBaUIsQ0FBQyxNQUFNOzs7OztRQUFDLENBQUMsR0FBRyxFQUFFLEdBQUcsRUFBRSxFQUFFLENBQUMsQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUcsR0FBRyxFQUFFLEdBQUcsR0FBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBRyxHQUFHLEVBQUUsR0FBRyxDQUFDLENBQUMsR0FBRSxFQUFFLENBQUMsQ0FDdkcsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7O1lBZEYsVUFBVTtZQUNWLElBQUksU0FBQztnQkFDSixJQUFJLEVBQUUsaUJBQWlCO2FBQ3hCOzs7O1lBUFEsS0FBSzs7Ozs7OztJQVNBLGlDQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFBpcGUsIFBpcGVUcmFuc2Zvcm0sIEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IENvbmZpZyB9IGZyb20gJy4uL21vZGVscyc7XHJcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzJztcclxuXHJcbkBJbmplY3RhYmxlKClcclxuQFBpcGUoe1xyXG4gIG5hbWU6ICdhYnBMb2NhbGl6YXRpb24nLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgTG9jYWxpemF0aW9uUGlwZSBpbXBsZW1lbnRzIFBpcGVUcmFuc2Zvcm0ge1xyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxyXG5cclxuICB0cmFuc2Zvcm0odmFsdWU6IHN0cmluZyB8IENvbmZpZy5Mb2NhbGl6YXRpb25XaXRoRGVmYXVsdCA9ICcnLCAuLi5pbnRlcnBvbGF0ZVBhcmFtczogc3RyaW5nW10pOiBzdHJpbmcge1xyXG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoXHJcbiAgICAgIENvbmZpZ1N0YXRlLmdldExvY2FsaXphdGlvbihcclxuICAgICAgICB2YWx1ZSxcclxuICAgICAgICAuLi5pbnRlcnBvbGF0ZVBhcmFtcy5yZWR1Y2UoKGFjYywgdmFsKSA9PiAoQXJyYXkuaXNBcnJheSh2YWwpID8gWy4uLmFjYywgLi4udmFsXSA6IFsuLi5hY2MsIHZhbF0pLCBbXSksXHJcbiAgICAgICksXHJcbiAgICApO1xyXG4gIH1cclxufVxyXG4iXX0=