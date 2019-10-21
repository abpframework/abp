import { Directive, ElementRef, Input, OnDestroy, OnInit, Optional, Renderer2 } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import { takeUntilDestroy } from '../utils';

@Directive({
  selector: '[abpPermission]',
})
export class PermissionDirective implements OnInit, OnDestroy {
  @Input('abpPermission') condition: string;

  constructor(@Optional() private elRef: ElementRef, private renderer: Renderer2, private store: Store) {}

  ngOnInit() {
    if (this.condition) {
      this.store
        .select(ConfigState.getGrantedPolicy(this.condition))
        .pipe(takeUntilDestroy(this))
        .subscribe(isGranted => {
          if (!isGranted) {
            this.renderer.removeChild(
              (this.elRef.nativeElement as HTMLElement).parentElement,
              this.elRef.nativeElement,
            );
          }
        });
    }
  }

  ngOnDestroy(): void {}
}
