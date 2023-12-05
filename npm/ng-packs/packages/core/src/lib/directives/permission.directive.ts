import {
  AfterViewInit,
  ChangeDetectorRef,
  Directive,
  Inject,
  Input,
  OnChanges,
  OnDestroy,
  Optional,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { ReplaySubject, Subscription } from 'rxjs';
import { distinctUntilChanged, take } from 'rxjs/operators';
import { PermissionService } from '../services/permission.service';
import { QUEUE_MANAGER } from '../tokens/queue.token';
import { QueueManager } from '../utils/queue';

@Directive({
  standalone: true,
  selector: '[abpPermission]',
})
export class PermissionDirective implements OnDestroy, OnChanges, AfterViewInit {
  @Input('abpPermission') condition: string | undefined;

  @Input('abpPermissionRunChangeDetection') runChangeDetection = true;

  subscription!: Subscription;

  cdrSubject = new ReplaySubject<void>();

  rendered = false;

  constructor(
    @Optional() private templateRef: TemplateRef<any>,
    private vcRef: ViewContainerRef,
    private permissionService: PermissionService,
    private cdRef: ChangeDetectorRef,
    @Inject(QUEUE_MANAGER) public queue: QueueManager,
  ) {}

  private check() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    this.subscription = this.permissionService
      .getGrantedPolicy$(this.condition || '')
      .pipe(distinctUntilChanged())
      .subscribe(isGranted => {
        this.vcRef.clear();
        if (isGranted) this.vcRef.createEmbeddedView(this.templateRef);
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
