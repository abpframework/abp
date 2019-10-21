/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, Input, Optional, ElementRef, Renderer2 } from '@angular/core';
import { Subject } from 'rxjs';
import snq from 'snq';
export class VisibilityDirective {
  /**
   * @param {?} elRef
   * @param {?} renderer
   */
  constructor(elRef, renderer) {
    this.elRef = elRef;
    this.renderer = renderer;
    this.mutationObserverEnabled = true;
    this.completed$ = new Subject();
  }
  /**
   * @return {?}
   */
  ngAfterViewInit() {
    /** @type {?} */
    let observer;
    if (this.mutationObserverEnabled) {
      observer = new MutationObserver
      /**
       * @param {?} mutations
       * @return {?}
       */(mutations => {
        mutations.forEach(
          /**
           * @param {?} mutation
           * @return {?}
           */
          mutation => {
            if (!mutation.target) return;
            /** @type {?} */
            const htmlNodes = snq(
              /**
               * @return {?}
               */
              (() =>
                Array.from(mutation.target.childNodes).filter(
                  /**
                   * @param {?} node
                   * @return {?}
                   */
                  node => node instanceof HTMLElement,
                )),
              [],
            );
            if (!htmlNodes.length) {
              this.removeFromDOM();
              this.disconnect();
            } else {
              setTimeout(
                /**
                 * @return {?}
                 */
                () => {
                  this.disconnect();
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
        () => {
          /** @type {?} */
          const htmlNodes = snq(
            /**
             * @return {?}
             */
            (() =>
              Array.from(this.focusedElement.childNodes).filter(
                /**
                 * @param {?} node
                 * @return {?}
                 */
                node => node instanceof HTMLElement,
              )),
            [],
          );
          if (!htmlNodes.length) this.removeFromDOM();
        },
        0,
      );
    }
    this.completed$.subscribe(
      /**
       * @return {?}
       */
      () => observer.disconnect(),
    );
  }
  /**
   * @return {?}
   */
  disconnect() {
    this.completed$.next();
    this.completed$.complete();
  }
  /**
   * @return {?}
   */
  removeFromDOM() {
    this.renderer.removeChild(this.elRef.nativeElement.parentElement, this.elRef.nativeElement);
  }
}
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
VisibilityDirective.ctorParameters = () => [
  { type: ElementRef, decorators: [{ type: Optional }] },
  { type: Renderer2 },
];
VisibilityDirective.propDecorators = {
  focusedElement: [{ type: Input, args: ['abpVisibility'] }],
  mutationObserverEnabled: [{ type: Input }],
};
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidmlzaWJpbGl0eS5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvZGlyZWN0aXZlcy92aXNpYmlsaXR5LmRpcmVjdGl2ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxLQUFLLEVBQUUsUUFBUSxFQUFFLFVBQVUsRUFBRSxTQUFTLEVBQWlCLE1BQU0sZUFBZSxDQUFDO0FBQ2pHLE9BQU8sRUFBRSxPQUFPLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDL0IsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBS3RCLE1BQU0sT0FBTyxtQkFBbUI7Ozs7O0lBUzlCLFlBQWdDLEtBQWlCLEVBQVUsUUFBbUI7UUFBOUMsVUFBSyxHQUFMLEtBQUssQ0FBWTtRQUFVLGFBQVEsR0FBUixRQUFRLENBQVc7UUFKOUUsNEJBQXVCLEdBQUcsSUFBSSxDQUFDO1FBRS9CLGVBQVUsR0FBRyxJQUFJLE9BQU8sRUFBVyxDQUFDO0lBRTZDLENBQUM7Ozs7SUFFbEYsZUFBZTs7WUFDVCxRQUEwQjtRQUM5QixJQUFJLElBQUksQ0FBQyx1QkFBdUIsRUFBRTtZQUNoQyxRQUFRLEdBQUcsSUFBSSxnQkFBZ0I7Ozs7WUFBQyxTQUFTLENBQUMsRUFBRTtnQkFDMUMsU0FBUyxDQUFDLE9BQU87Ozs7Z0JBQUMsUUFBUSxDQUFDLEVBQUU7b0JBQzNCLElBQUksQ0FBQyxRQUFRLENBQUMsTUFBTTt3QkFBRSxPQUFPOzswQkFFdkIsU0FBUyxHQUFHLEdBQUc7OztvQkFDbkIsR0FBRyxFQUFFLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsTUFBTSxDQUFDLFVBQVUsQ0FBQyxDQUFDLE1BQU07Ozs7b0JBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxJQUFJLFlBQVksV0FBVyxFQUFDLEdBQ3hGLEVBQUUsQ0FDSDtvQkFFRCxJQUFJLENBQUMsU0FBUyxDQUFDLE1BQU0sRUFBRTt3QkFDckIsSUFBSSxDQUFDLGFBQWEsRUFBRSxDQUFDO3dCQUNyQixJQUFJLENBQUMsVUFBVSxFQUFFLENBQUM7cUJBQ25CO3lCQUFNO3dCQUNMLFVBQVU7Ozt3QkFBQyxHQUFHLEVBQUU7NEJBQ2QsSUFBSSxDQUFDLFVBQVUsRUFBRSxDQUFDO3dCQUNwQixDQUFDLEdBQUUsQ0FBQyxDQUFDLENBQUM7cUJBQ1A7Z0JBQ0gsQ0FBQyxFQUFDLENBQUM7WUFDTCxDQUFDLEVBQUMsQ0FBQztZQUVILFFBQVEsQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUFDLGNBQWMsRUFBRTtnQkFDcEMsU0FBUyxFQUFFLElBQUk7YUFDaEIsQ0FBQyxDQUFDO1NBQ0o7YUFBTTtZQUNMLFVBQVU7OztZQUFDLEdBQUcsRUFBRTs7c0JBQ1IsU0FBUyxHQUFHLEdBQUc7OztnQkFDbkIsR0FBRyxFQUFFLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsY0FBYyxDQUFDLFVBQVUsQ0FBQyxDQUFDLE1BQU07Ozs7Z0JBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxJQUFJLFlBQVksV0FBVyxFQUFDLEdBQzVGLEVBQUUsQ0FDSDtnQkFFRCxJQUFJLENBQUMsU0FBUyxDQUFDLE1BQU07b0JBQUUsSUFBSSxDQUFDLGFBQWEsRUFBRSxDQUFDO1lBQzlDLENBQUMsR0FBRSxDQUFDLENBQUMsQ0FBQztTQUNQO1FBRUQsSUFBSSxDQUFDLFVBQVUsQ0FBQyxTQUFTOzs7UUFBQyxHQUFHLEVBQUUsQ0FBQyxRQUFRLENBQUMsVUFBVSxFQUFFLEVBQUMsQ0FBQztJQUN6RCxDQUFDOzs7O0lBRUQsVUFBVTtRQUNSLElBQUksQ0FBQyxVQUFVLENBQUMsSUFBSSxFQUFFLENBQUM7UUFDdkIsSUFBSSxDQUFDLFVBQVUsQ0FBQyxRQUFRLEVBQUUsQ0FBQztJQUM3QixDQUFDOzs7O0lBRUQsYUFBYTtRQUNYLElBQUksQ0FBQyxRQUFRLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxDQUFDLGFBQWEsRUFBRSxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsQ0FBQyxDQUFDO0lBQzlGLENBQUM7OztZQTdERixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGlCQUFpQjthQUM1Qjs7OztZQU5vQyxVQUFVLHVCQWdCaEMsUUFBUTtZQWhCMEIsU0FBUzs7OzZCQVF2RCxLQUFLLFNBQUMsZUFBZTtzQ0FHckIsS0FBSzs7OztJQUhOLDZDQUM0Qjs7SUFFNUIsc0RBQytCOztJQUUvQix5Q0FBb0M7Ozs7O0lBRXhCLG9DQUFxQzs7Ozs7SUFBRSx1Q0FBMkIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBEaXJlY3RpdmUsIElucHV0LCBPcHRpb25hbCwgRWxlbWVudFJlZiwgUmVuZGVyZXIyLCBBZnRlclZpZXdJbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTdWJqZWN0IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5cbkBEaXJlY3RpdmUoe1xuICBzZWxlY3RvcjogJ1thYnBWaXNpYmlsaXR5XSdcbn0pXG5leHBvcnQgY2xhc3MgVmlzaWJpbGl0eURpcmVjdGl2ZSBpbXBsZW1lbnRzIEFmdGVyVmlld0luaXQge1xuICBASW5wdXQoJ2FicFZpc2liaWxpdHknKVxuICBmb2N1c2VkRWxlbWVudDogSFRNTEVsZW1lbnQ7XG5cbiAgQElucHV0KClcbiAgbXV0YXRpb25PYnNlcnZlckVuYWJsZWQgPSB0cnVlO1xuXG4gIGNvbXBsZXRlZCQgPSBuZXcgU3ViamVjdDxib29sZWFuPigpO1xuXG4gIGNvbnN0cnVjdG9yKEBPcHRpb25hbCgpIHByaXZhdGUgZWxSZWY6IEVsZW1lbnRSZWYsIHByaXZhdGUgcmVuZGVyZXI6IFJlbmRlcmVyMikge31cblxuICBuZ0FmdGVyVmlld0luaXQoKSB7XG4gICAgbGV0IG9ic2VydmVyOiBNdXRhdGlvbk9ic2VydmVyO1xuICAgIGlmICh0aGlzLm11dGF0aW9uT2JzZXJ2ZXJFbmFibGVkKSB7XG4gICAgICBvYnNlcnZlciA9IG5ldyBNdXRhdGlvbk9ic2VydmVyKG11dGF0aW9ucyA9PiB7XG4gICAgICAgIG11dGF0aW9ucy5mb3JFYWNoKG11dGF0aW9uID0+IHtcbiAgICAgICAgICBpZiAoIW11dGF0aW9uLnRhcmdldCkgcmV0dXJuO1xuXG4gICAgICAgICAgY29uc3QgaHRtbE5vZGVzID0gc25xKFxuICAgICAgICAgICAgKCkgPT4gQXJyYXkuZnJvbShtdXRhdGlvbi50YXJnZXQuY2hpbGROb2RlcykuZmlsdGVyKG5vZGUgPT4gbm9kZSBpbnN0YW5jZW9mIEhUTUxFbGVtZW50KSxcbiAgICAgICAgICAgIFtdXG4gICAgICAgICAgKTtcblxuICAgICAgICAgIGlmICghaHRtbE5vZGVzLmxlbmd0aCkge1xuICAgICAgICAgICAgdGhpcy5yZW1vdmVGcm9tRE9NKCk7XG4gICAgICAgICAgICB0aGlzLmRpc2Nvbm5lY3QoKTtcbiAgICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICAgICAgICAgIHRoaXMuZGlzY29ubmVjdCgpO1xuICAgICAgICAgICAgfSwgMCk7XG4gICAgICAgICAgfVxuICAgICAgICB9KTtcbiAgICAgIH0pO1xuXG4gICAgICBvYnNlcnZlci5vYnNlcnZlKHRoaXMuZm9jdXNlZEVsZW1lbnQsIHtcbiAgICAgICAgY2hpbGRMaXN0OiB0cnVlXG4gICAgICB9KTtcbiAgICB9IGVsc2Uge1xuICAgICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICAgIGNvbnN0IGh0bWxOb2RlcyA9IHNucShcbiAgICAgICAgICAoKSA9PiBBcnJheS5mcm9tKHRoaXMuZm9jdXNlZEVsZW1lbnQuY2hpbGROb2RlcykuZmlsdGVyKG5vZGUgPT4gbm9kZSBpbnN0YW5jZW9mIEhUTUxFbGVtZW50KSxcbiAgICAgICAgICBbXVxuICAgICAgICApO1xuXG4gICAgICAgIGlmICghaHRtbE5vZGVzLmxlbmd0aCkgdGhpcy5yZW1vdmVGcm9tRE9NKCk7XG4gICAgICB9LCAwKTtcbiAgICB9XG5cbiAgICB0aGlzLmNvbXBsZXRlZCQuc3Vic2NyaWJlKCgpID0+IG9ic2VydmVyLmRpc2Nvbm5lY3QoKSk7XG4gIH1cblxuICBkaXNjb25uZWN0KCkge1xuICAgIHRoaXMuY29tcGxldGVkJC5uZXh0KCk7XG4gICAgdGhpcy5jb21wbGV0ZWQkLmNvbXBsZXRlKCk7XG4gIH1cblxuICByZW1vdmVGcm9tRE9NKCkge1xuICAgIHRoaXMucmVuZGVyZXIucmVtb3ZlQ2hpbGQodGhpcy5lbFJlZi5uYXRpdmVFbGVtZW50LnBhcmVudEVsZW1lbnQsIHRoaXMuZWxSZWYubmF0aXZlRWxlbWVudCk7XG4gIH1cbn1cbiJdfQ==
