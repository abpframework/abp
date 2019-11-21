/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { Pipe } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
var LocalizationPipe = /** @class */ (function() {
  function LocalizationPipe(store) {
    this.store = store;
  }
  /**
   * @param {?=} value
   * @param {...?} interpolateParams
   * @return {?}
   */
  LocalizationPipe.prototype.transform
  /**
   * @param {?=} value
   * @param {...?} interpolateParams
   * @return {?}
   */ = function(value) {
    if (value === void 0) {
      value = '';
    }
    var interpolateParams = [];
    for (var _i = 1; _i < arguments.length; _i++) {
      interpolateParams[_i - 1] = arguments[_i];
    }
    return this.store.selectSnapshot(
      ConfigState.getLocalization.apply(
        ConfigState,
        tslib_1.__spread(
          [value],
          interpolateParams.reduce(
            /**
             * @param {?} acc
             * @param {?} val
             * @return {?}
             */
            function(acc, val) {
              return Array.isArray(val) ? tslib_1.__spread(acc, val) : tslib_1.__spread(acc, [val]);
            },
            [],
          ),
        ),
      ),
    );
  };
  LocalizationPipe.decorators = [
    {
      type: Pipe,
      args: [
        {
          name: 'abpLocalization',
        },
      ],
    },
  ];
  /** @nocollapse */
  LocalizationPipe.ctorParameters = function() {
    return [{ type: Store }];
  };
  return LocalizationPipe;
})();
export { LocalizationPipe };
if (false) {
  /**
   * @type {?}
   * @private
   */
  LocalizationPipe.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnBpcGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvcGlwZXMvbG9jYWxpemF0aW9uLnBpcGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsSUFBSSxFQUFpQixNQUFNLGVBQWUsQ0FBQztBQUNwRCxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBRXBDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxXQUFXLENBQUM7QUFFeEM7SUFJRSwwQkFBb0IsS0FBWTtRQUFaLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7Ozs7SUFFcEMsb0NBQVM7Ozs7O0lBQVQsVUFBVSxLQUFtRDtRQUFuRCxzQkFBQSxFQUFBLFVBQW1EO1FBQUUsMkJBQThCO2FBQTlCLFVBQThCLEVBQTlCLHFCQUE4QixFQUE5QixJQUE4QjtZQUE5QiwwQ0FBOEI7O1FBQzNGLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQzlCLFdBQVcsQ0FBQyxlQUFlLE9BQTNCLFdBQVcsb0JBQ1QsS0FBSyxHQUNGLGlCQUFpQixDQUFDLE1BQU07Ozs7O1FBQUMsVUFBQyxHQUFHLEVBQUUsR0FBRyxJQUFLLE9BQUEsQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsa0JBQUssR0FBRyxFQUFLLEdBQUcsRUFBRSxDQUFDLGtCQUFLLEdBQUcsR0FBRSxHQUFHLEVBQUMsQ0FBQyxFQUF2RCxDQUF1RCxHQUFFLEVBQUUsQ0FBQyxHQUV6RyxDQUFDO0lBQ0osQ0FBQzs7Z0JBYkYsSUFBSSxTQUFDO29CQUNKLElBQUksRUFBRSxpQkFBaUI7aUJBQ3hCOzs7O2dCQU5RLEtBQUs7O0lBa0JkLHVCQUFDO0NBQUEsQUFkRCxJQWNDO1NBWFksZ0JBQWdCOzs7Ozs7SUFDZixpQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBQaXBlLCBQaXBlVHJhbnNmb3JtIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IENvbmZpZyB9IGZyb20gJy4uL21vZGVscyc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcyc7XG5cbkBQaXBlKHtcbiAgbmFtZTogJ2FicExvY2FsaXphdGlvbicsXG59KVxuZXhwb3J0IGNsYXNzIExvY2FsaXphdGlvblBpcGUgaW1wbGVtZW50cyBQaXBlVHJhbnNmb3JtIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgdHJhbnNmb3JtKHZhbHVlOiBzdHJpbmcgfCBDb25maWcuTG9jYWxpemF0aW9uV2l0aERlZmF1bHQgPSAnJywgLi4uaW50ZXJwb2xhdGVQYXJhbXM6IHN0cmluZ1tdKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChcbiAgICAgIENvbmZpZ1N0YXRlLmdldExvY2FsaXphdGlvbihcbiAgICAgICAgdmFsdWUsXG4gICAgICAgIC4uLmludGVycG9sYXRlUGFyYW1zLnJlZHVjZSgoYWNjLCB2YWwpID0+IChBcnJheS5pc0FycmF5KHZhbCkgPyBbLi4uYWNjLCAuLi52YWxdIDogWy4uLmFjYywgdmFsXSksIFtdKSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxufVxuIl19
