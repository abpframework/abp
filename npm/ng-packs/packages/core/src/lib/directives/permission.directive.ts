import {
  Directive,
  ElementRef,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Optional,
  Renderer2,
  SimpleChanges,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { PermissionService } from '../services/permission.service';

@Directive({
  selector: '[abpPermission]',
})
export class PermissionDirective implements OnInit, OnDestroy, OnChanges {
  @Input('abpPermission') condition: string;

  subscription: Subscription;

  constructor(
    private elRef: ElementRef,
    private renderer: Renderer2,
    @Optional() private templateRef: TemplateRef<any>,
    private vcRef: ViewContainerRef,
    private permissionService: PermissionService,
  ) {}

  private check() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
    this.subscription = this.permissionService
      .getGrantedPolicy$(this.condition)
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

  ngOnInit() {
    if (this.templateRef && !this.condition) {
      this.vcRef.createEmbeddedView(this.templateRef);
    }
  }

  ngOnDestroy(): void {
    if (this.subscription) this.subscription.unsubscribe();
  }

  ngOnChanges({ condition }: SimpleChanges) {
    if ((condition || { currentValue: null }).currentValue) {
      this.check();
    }
  }
}
