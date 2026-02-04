import {
  AfterViewInit,
  ChangeDetectorRef,
  Directive,
  Input,
  OnChanges,
  OnDestroy,
  TemplateRef,
  ViewContainerRef,
  inject,
} from '@angular/core';
import { ReplaySubject, Subscription } from 'rxjs';
import { distinctUntilChanged, take } from 'rxjs/operators';
import { PermissionService } from '../services/permission.service';
import { QUEUE_MANAGER } from '../tokens/queue.token';
import { QueueManager } from '../utils/queue';

@Directive({
  selector: '[abpPermission]',
})
export class PermissionDirective implements OnDestroy, OnChanges, AfterViewInit {
  private templateRef = inject<TemplateRef<any>>(TemplateRef, { optional: true });
  private vcRef = inject(ViewContainerRef);
  private permissionService = inject(PermissionService);
  private cdRef = inject(ChangeDetectorRef);
  queue = inject<QueueManager>(QUEUE_MANAGER);

  @Input('abpPermission') condition: string | undefined;

  @Input('abpPermissionRunChangeDetection') runChangeDetection = true;

  subscription!: Subscription;

  cdrSubject = new ReplaySubject<void>();

  rendered = false;

  private check() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    this.subscription = this.permissionService
      .getGrantedPolicy$(this.condition || '')
      .pipe(distinctUntilChanged())
      .subscribe(isGranted => {
        this.vcRef.clear();
        if (isGranted && this.templateRef) {
          this.vcRef.createEmbeddedView(this.templateRef);
        }
        if (this.runChangeDetection) {
          if (!this.rendered) {
            this.cdrSubject.next();
          } else {
            this.cdRef.detectChanges();
          }
        } else {
          this.cdRef.markForCheck();
        }
      });
  }

  ngOnDestroy(): void {
    if (this.subscription) this.subscription.unsubscribe();
  }

  ngOnChanges() {
    this.check();
  }

  ngAfterViewInit() {
    this.cdrSubject.pipe(take(1)).subscribe(() => this.queue.add(() => this.cdRef.detectChanges()));
    this.rendered = true;
  }
}
