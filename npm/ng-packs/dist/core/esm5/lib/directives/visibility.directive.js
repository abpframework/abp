/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, Input, Optional, ElementRef, Renderer2 } from '@angular/core';
import { Subject } from 'rxjs';
import snq from 'snq';
var VisibilityDirective = /** @class */ (function() {
  function VisibilityDirective(elRef, renderer) {
    this.elRef = elRef;
    this.renderer = renderer;
    this.mutationObserverEnabled = true;
    this.completed$ = new Subject();
  }
  /**
   * @return {?}
   */
  VisibilityDirective.prototype.ngAfterViewInit
  /**
   * @return {?}
   */ = function() {
    var _this = this;
    /** @type {?} */
    var observer;
    if (this.mutationObserverEnabled) {
      observer = new MutationObserver
      /**
       * @param {?} mutations
       * @return {?}
       */(function(mutations) {
        mutations.forEach(
          /**
           * @param {?} mutation
           * @return {?}
           */
          function(mutation) {
            if (!mutation.target) return;
            /** @type {?} */
            var htmlNodes = snq(
              /**
               * @return {?}
               */
              (function() {
                return Array.from(mutation.target.childNodes).filter(
                  /**
                   * @param {?} node
                   * @return {?}
                   */
                  function(node) {
                    return node instanceof HTMLElement;
                  },
                );
              }),
              [],
            );
            if (!htmlNodes.length) {
              _this.removeFromDOM();
              _this.disconnect();
            } else {
              setTimeout(
                /**
                 * @return {?}
                 */
                function() {
                  _this.disconnect();
                },
                0,
              );
            }
          },
        );
      });
      observer.observe(this.focusedElement, {
        childList: true,
      });
    } else {
      setTimeout(
        /**
         * @return {?}
         */
        function() {
          /** @type {?} */
          var htmlNodes = snq(
            /**
             * @return {?}
             */
            (function() {
              return Array.from(_this.focusedElement.childNodes).filter(
                /**
                 * @param {?} node
                 * @return {?}
                 */
                function(node) {
                  return node instanceof HTMLElement;
                },
              );
            }),
            [],
          );
          if (!htmlNodes.length) _this.removeFromDOM();
        },
        0,
      );
    }
    this.completed$.subscribe(
      /**
       * @return {?}
       */
      function() {
        return observer.disconnect();
      },
    );
  };
  /**
   * @return {?}
   */
  VisibilityDirective.prototype.disconnect
  /**
   * @return {?}
   */ = function() {
    this.completed$.next();
    this.completed$.complete();
  };
  /**
   * @return {?}
   */
  VisibilityDirective.prototype.removeFromDOM
  /**
   * @return {?}
   */ = function() {
    this.renderer.removeChild(this.elRef.nativeElement.parentElement, this.elRef.nativeElement);
  };
  VisibilityDirective.decorators = [
    {
      type: Directive,
      args: [
        {
          selector: '[abpVisibility]',
        },
      ],
    },
  ];
  /** @nocollapse */
  VisibilityDirective.ctorParameters = function() {
    return [{ type: ElementRef, decorators: [{ type: Optional }] }, { type: Renderer2 }];
  };
  VisibilityDirective.propDecorators = {
    focusedElement: [{ type: Input, args: ['abpVisibility'] }],
    mutationObserverEnabled: [{ type: Input }],
  };
  return VisibilityDirective;
})();
export { VisibilityDirective };
if (false) {
  /** @type {?} */
  VisibilityDirective.prototype.focusedElement;
  /** @type {?} */
  VisibilityDirective.prototype.mutationObserverEnabled;
  /** @type {?} */
  VisibilityDirective.prototype.completed$;
  /**
   * @type {?}
   * @private
   */
  VisibilityDirective.prototype.elRef;
  /**
   * @type {?}
   * @private
   */
  VisibilityDirective.prototype.renderer;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidmlzaWJpbGl0eS5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy92aXNpYmlsaXR5LmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxLQUFLLEVBQUUsUUFBUSxFQUFFLFVBQVUsRUFBRSxTQUFTLEVBQWlCLE1BQU0sZUFBZSxDQUFDO0FBQ2pHLE9BQU8sRUFBRSxPQUFPLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDL0IsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBRXRCO0lBWUUsNkJBQWdDLEtBQWlCLEVBQVUsUUFBbUI7UUFBOUMsVUFBSyxHQUFMLEtBQUssQ0FBWTtRQUFVLGFBQVEsR0FBUixRQUFRLENBQVc7UUFKOUUsNEJBQXVCLEdBQUcsSUFBSSxDQUFDO1FBRS9CLGVBQVUsR0FBRyxJQUFJLE9BQU8sRUFBVyxDQUFDO0lBRTZDLENBQUM7Ozs7SUFFbEYsNkNBQWU7OztJQUFmO1FBQUEsaUJBc0NDOztZQXJDSyxRQUEwQjtRQUM5QixJQUFJLElBQUksQ0FBQyx1QkFBdUIsRUFBRTtZQUNoQyxRQUFRLEdBQUcsSUFBSSxnQkFBZ0I7Ozs7WUFBQyxVQUFBLFNBQVM7Z0JBQ3ZDLFNBQVMsQ0FBQyxPQUFPOzs7O2dCQUFDLFVBQUEsUUFBUTtvQkFDeEIsSUFBSSxDQUFDLFFBQVEsQ0FBQyxNQUFNO3dCQUFFLE9BQU87O3dCQUV2QixTQUFTLEdBQUcsR0FBRzs7O29CQUNuQixjQUFNLE9BQUEsS0FBSyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsTUFBTSxDQUFDLFVBQVUsQ0FBQyxDQUFDLE1BQU07Ozs7b0JBQUMsVUFBQSxJQUFJLElBQUksT0FBQSxJQUFJLFlBQVksV0FBVyxFQUEzQixDQUEyQixFQUFDLEVBQWxGLENBQWtGLEdBQ3hGLEVBQUUsQ0FDSDtvQkFFRCxJQUFJLENBQUMsU0FBUyxDQUFDLE1BQU0sRUFBRTt3QkFDckIsS0FBSSxDQUFDLGFBQWEsRUFBRSxDQUFDO3dCQUNyQixLQUFJLENBQUMsVUFBVSxFQUFFLENBQUM7cUJBQ25CO3lCQUFNO3dCQUNMLFVBQVU7Ozt3QkFBQzs0QkFDVCxLQUFJLENBQUMsVUFBVSxFQUFFLENBQUM7d0JBQ3BCLENBQUMsR0FBRSxDQUFDLENBQUMsQ0FBQztxQkFDUDtnQkFDSCxDQUFDLEVBQUMsQ0FBQztZQUNMLENBQUMsRUFBQyxDQUFDO1lBRUgsUUFBUSxDQUFDLE9BQU8sQ0FBQyxJQUFJLENBQUMsY0FBYyxFQUFFO2dCQUNwQyxTQUFTLEVBQUUsSUFBSTthQUNoQixDQUFDLENBQUM7U0FDSjthQUFNO1lBQ0wsVUFBVTs7O1lBQUM7O29CQUNILFNBQVMsR0FBRyxHQUFHOzs7Z0JBQ25CLGNBQU0sT0FBQSxLQUFLLENBQUMsSUFBSSxDQUFDLEtBQUksQ0FBQyxjQUFjLENBQUMsVUFBVSxDQUFDLENBQUMsTUFBTTs7OztnQkFBQyxVQUFBLElBQUksSUFBSSxPQUFBLElBQUksWUFBWSxXQUFXLEVBQTNCLENBQTJCLEVBQUMsRUFBdEYsQ0FBc0YsR0FDNUYsRUFBRSxDQUNIO2dCQUVELElBQUksQ0FBQyxTQUFTLENBQUMsTUFBTTtvQkFBRSxLQUFJLENBQUMsYUFBYSxFQUFFLENBQUM7WUFDOUMsQ0FBQyxHQUFFLENBQUMsQ0FBQyxDQUFDO1NBQ1A7UUFFRCxJQUFJLENBQUMsVUFBVSxDQUFDLFNBQVM7OztRQUFDLGNBQU0sT0FBQSxRQUFRLENBQUMsVUFBVSxFQUFFLEVBQXJCLENBQXFCLEVBQUMsQ0FBQztJQUN6RCxDQUFDOzs7O0lBRUQsd0NBQVU7OztJQUFWO1FBQ0UsSUFBSSxDQUFDLFVBQVUsQ0FBQyxJQUFJLEVBQUUsQ0FBQztRQUN2QixJQUFJLENBQUMsVUFBVSxDQUFDLFFBQVEsRUFBRSxDQUFDO0lBQzdCLENBQUM7Ozs7SUFFRCwyQ0FBYTs7O0lBQWI7UUFDRSxJQUFJLENBQUMsUUFBUSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsQ0FBQyxhQUFhLEVBQUUsSUFBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLENBQUMsQ0FBQztJQUM5RixDQUFDOztnQkE3REYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxpQkFBaUI7aUJBQzVCOzs7O2dCQU5vQyxVQUFVLHVCQWdCaEMsUUFBUTtnQkFoQjBCLFNBQVM7OztpQ0FRdkQsS0FBSyxTQUFDLGVBQWU7MENBR3JCLEtBQUs7O0lBdURSLDBCQUFDO0NBQUEsQUE5REQsSUE4REM7U0EzRFksbUJBQW1COzs7SUFDOUIsNkNBQzRCOztJQUU1QixzREFDK0I7O0lBRS9CLHlDQUFvQzs7Ozs7SUFFeEIsb0NBQXFDOzs7OztJQUFFLHVDQUEyQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IERpcmVjdGl2ZSwgSW5wdXQsIE9wdGlvbmFsLCBFbGVtZW50UmVmLCBSZW5kZXJlcjIsIEFmdGVyVmlld0luaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFN1YmplY3QgfSBmcm9tICdyeGpzJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcblxuQERpcmVjdGl2ZSh7XG4gIHNlbGVjdG9yOiAnW2FicFZpc2liaWxpdHldJ1xufSlcbmV4cG9ydCBjbGFzcyBWaXNpYmlsaXR5RGlyZWN0aXZlIGltcGxlbWVudHMgQWZ0ZXJWaWV3SW5pdCB7XG4gIEBJbnB1dCgnYWJwVmlzaWJpbGl0eScpXG4gIGZvY3VzZWRFbGVtZW50OiBIVE1MRWxlbWVudDtcblxuICBASW5wdXQoKVxuICBtdXRhdGlvbk9ic2VydmVyRW5hYmxlZCA9IHRydWU7XG5cbiAgY29tcGxldGVkJCA9IG5ldyBTdWJqZWN0PGJvb2xlYW4+KCk7XG5cbiAgY29uc3RydWN0b3IoQE9wdGlvbmFsKCkgcHJpdmF0ZSBlbFJlZjogRWxlbWVudFJlZiwgcHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyKSB7fVxuXG4gIG5nQWZ0ZXJWaWV3SW5pdCgpIHtcbiAgICBsZXQgb2JzZXJ2ZXI6IE11dGF0aW9uT2JzZXJ2ZXI7XG4gICAgaWYgKHRoaXMubXV0YXRpb25PYnNlcnZlckVuYWJsZWQpIHtcbiAgICAgIG9ic2VydmVyID0gbmV3IE11dGF0aW9uT2JzZXJ2ZXIobXV0YXRpb25zID0+IHtcbiAgICAgICAgbXV0YXRpb25zLmZvckVhY2gobXV0YXRpb24gPT4ge1xuICAgICAgICAgIGlmICghbXV0YXRpb24udGFyZ2V0KSByZXR1cm47XG5cbiAgICAgICAgICBjb25zdCBodG1sTm9kZXMgPSBzbnEoXG4gICAgICAgICAgICAoKSA9PiBBcnJheS5mcm9tKG11dGF0aW9uLnRhcmdldC5jaGlsZE5vZGVzKS5maWx0ZXIobm9kZSA9PiBub2RlIGluc3RhbmNlb2YgSFRNTEVsZW1lbnQpLFxuICAgICAgICAgICAgW11cbiAgICAgICAgICApO1xuXG4gICAgICAgICAgaWYgKCFodG1sTm9kZXMubGVuZ3RoKSB7XG4gICAgICAgICAgICB0aGlzLnJlbW92ZUZyb21ET00oKTtcbiAgICAgICAgICAgIHRoaXMuZGlzY29ubmVjdCgpO1xuICAgICAgICAgIH0gZWxzZSB7XG4gICAgICAgICAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgICAgICAgICAgdGhpcy5kaXNjb25uZWN0KCk7XG4gICAgICAgICAgICB9LCAwKTtcbiAgICAgICAgICB9XG4gICAgICAgIH0pO1xuICAgICAgfSk7XG5cbiAgICAgIG9ic2VydmVyLm9ic2VydmUodGhpcy5mb2N1c2VkRWxlbWVudCwge1xuICAgICAgICBjaGlsZExpc3Q6IHRydWVcbiAgICAgIH0pO1xuICAgIH0gZWxzZSB7XG4gICAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgICAgY29uc3QgaHRtbE5vZGVzID0gc25xKFxuICAgICAgICAgICgpID0+IEFycmF5LmZyb20odGhpcy5mb2N1c2VkRWxlbWVudC5jaGlsZE5vZGVzKS5maWx0ZXIobm9kZSA9PiBub2RlIGluc3RhbmNlb2YgSFRNTEVsZW1lbnQpLFxuICAgICAgICAgIFtdXG4gICAgICAgICk7XG5cbiAgICAgICAgaWYgKCFodG1sTm9kZXMubGVuZ3RoKSB0aGlzLnJlbW92ZUZyb21ET00oKTtcbiAgICAgIH0sIDApO1xuICAgIH1cblxuICAgIHRoaXMuY29tcGxldGVkJC5zdWJzY3JpYmUoKCkgPT4gb2JzZXJ2ZXIuZGlzY29ubmVjdCgpKTtcbiAgfVxuXG4gIGRpc2Nvbm5lY3QoKSB7XG4gICAgdGhpcy5jb21wbGV0ZWQkLm5leHQoKTtcbiAgICB0aGlzLmNvbXBsZXRlZCQuY29tcGxldGUoKTtcbiAgfVxuXG4gIHJlbW92ZUZyb21ET00oKSB7XG4gICAgdGhpcy5yZW5kZXJlci5yZW1vdmVDaGlsZCh0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQucGFyZW50RWxlbWVudCwgdGhpcy5lbFJlZi5uYXRpdmVFbGVtZW50KTtcbiAgfVxufVxuIl19
