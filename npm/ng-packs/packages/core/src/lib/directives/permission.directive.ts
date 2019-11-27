import {
  Directive,
  ElementRef,
  Input,
  OnDestroy,
  OnInit,
  Renderer2,
  ViewContainerRef,
  TemplateRef,
  Optional,
} from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import { takeUntilDestroy } from '../utils';

@Directive({
  selector: '[abpPermission]',
})
export class PermissionDirective implements OnInit, OnDestroy {
  @Input('abpPermission') condition: string;

  constructor(
    private elRef: ElementRef,
    private renderer: Renderer2,
    private store: Store,
    @Optional() private templateRef: TemplateRef<any>,
    private vcRef: ViewContainerRef,
  ) {}

  ngOnInit() {
    if (this.condition) {
      this.store
        .select(ConfigState.getGrantedPolicy(this.condition))
        .pipe(takeUntilDestroy(this))
        .subscribe(isGranted => {
          if (this.templateRef && isGranted) {
            this.vcRef.clear();
            this.vcRef.createEmbeddedView(this.templateRef);
          } else if (this.templateRef && !isGranted) {
            this.vcRef.clear();
          } else if (!isGranted && !this.templateRef) {
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
