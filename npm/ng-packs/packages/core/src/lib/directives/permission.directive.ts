import {
  ChangeDetectorRef,
  Directive,
  ElementRef,
  Input,
  OnChanges,
  OnDestroy,
  Optional,
  Renderer2,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { distinctUntilChanged } from 'rxjs/operators';
import { PermissionService } from '../services/permission.service';

@Directive({
  selector: '[abpPermission]',
})
export class PermissionDirective implements OnDestroy, OnChanges {
  @Input('abpPermission') condition: string;

  subscription: Subscription;

  constructor(
    private elRef: ElementRef<HTMLElement>,
    private renderer: Renderer2,
    @Optional() private templateRef: TemplateRef<any>,
    private vcRef: ViewContainerRef,
    private permissionService: PermissionService,
    private cdRef: ChangeDetectorRef,
  ) {}

  private check() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    this.subscription = this.permissionService
      .getGrantedPolicy$(this.condition)
      .pipe(distinctUntilChanged())
      .subscribe(isGranted => {
        this.vcRef.clear();
        if (isGranted) this.vcRef.createEmbeddedView(this.templateRef);
        this.cdRef.detectChanges();
      });
  }

  ngOnDestroy(): void {
    if (this.subscription) this.subscription.unsubscribe();
  }

  ngOnChanges() {
    this.check();
  }
}
