import { ChangeDetectorRef } from '@angular/core';
import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/vitest';
import { Subject } from 'rxjs';
import { PermissionDirective } from '../directives/permission.directive';
import { PermissionService } from '../services/permission.service';
import { QUEUE_MANAGER } from '../tokens/queue.token';

describe('PermissionDirective', () => {
  let spectator: SpectatorDirective<PermissionDirective>;
  let directive: PermissionDirective;
  const grantedPolicy$ = new Subject<boolean>();
  const createDirective = createDirectiveFactory({
    directive: PermissionDirective,
    providers: [
      { provide: PermissionService, useValue: { getGrantedPolicy$: () => grantedPolicy$ } },
      { provide: QUEUE_MANAGER, useValue: { add: vi.fn() } },
      { provide: ChangeDetectorRef, useValue: { detectChanges: vi.fn() } },
    ],
  });

  beforeEach(() => {
    spectator = createDirective(
      '<div [abpPermission]="permission" [abpPermissionRunChangeDetection]="runCD"></div>',
      {
        hostProps: { permission: 'test', runCD: false },
      },
    );
    directive = spectator.directive;
    grantedPolicy$.next(false);
    spectator.detectChanges();
  });

  afterEach(() => {
    // Clean up subscriptions to prevent errors after test completion
    if (directive?.subscription) {
      directive.subscription.unsubscribe();
    }
    grantedPolicy$.next(false);
  });

  it('should create directive', () => {
    expect(directive).toBeTruthy();
  });

  it('should handle permission input', () => {
    grantedPolicy$.next(false);
    directive.condition = 'new-permission';
    directive.ngOnChanges();
    grantedPolicy$.next(true);
    expect(directive).toBeTruthy();
    expect(directive.condition).toBe('new-permission');
  });

  it('should handle runChangeDetection input', () => {
    grantedPolicy$.next(false);
    directive.runChangeDetection = true;
    directive.ngOnChanges();
    grantedPolicy$.next(true);
    expect(directive).toBeTruthy();
    expect(directive.runChangeDetection).toBe(true);
  });
});
