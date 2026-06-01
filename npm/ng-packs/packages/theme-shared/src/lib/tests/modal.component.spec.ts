import { ConfirmationService } from '@abp/ng.theme.shared';
import { CoreTestingModule } from '@abp/ng.core/testing';
import { Component, EventEmitter, Input } from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator/vitest';
import { Confirmation } from '@abp/ng.theme.shared';
import { firstValueFrom, Subject, timer } from 'rxjs';
import { ModalComponent } from '../components/modal/modal.component';
import { setupComponentResources } from './utils';

@Component({
  template: `
    <abp-modal
      [visible]="visible"
      [busy]="busy"
      (visibleChange)="visibleChange.emit($event)"
    >
      <ng-template #abpHeader>Header</ng-template>
      <ng-template #abpBody>Body</ng-template>
      <ng-template #abpFooter>Footer</ng-template>
    </abp-modal>
  `,
  imports: [ModalComponent]
})
class TestHostComponent {
  @Input() visible = false;
  @Input() busy = false;
  visibleChange = new EventEmitter<boolean>();
}

const mockConfirmation$ = new Subject<Confirmation.Status>();
const disappearFn = vi.fn();

describe('ModalComponent', () => {
  let spectator: Spectator<TestHostComponent>;
  let createComponent: ReturnType<typeof createComponentFactory<TestHostComponent>>;

  beforeAll(() => setupComponentResources('../components/modal', import.meta.url));

  beforeEach(() => {
    // Create component factory in beforeEach to ensure beforeAll has run
    if (!createComponent) {
      createComponent = createComponentFactory({
        component: TestHostComponent,
        imports: [
          CoreTestingModule.withConfig(),
          ModalComponent,
        ],
        providers: [
          {
            provide: ConfirmationService,
            useValue: {
              warn: vi.fn(() => mockConfirmation$),
            },
          },
        ],
      });
    }

    spectator = createComponent();
    disappearFn.mockClear();
  });

  it('should create component', () => {
    expect(spectator.component).toBeTruthy();
  });

  it('should handle visible input', () => {
    spectator.setInput('visible', true);
    spectator.detectChanges();
    expect(spectator.component.visible).toBe(true);
  });

  it('should handle busy input', () => {
    spectator.setInput('busy', true);
    spectator.detectChanges();
    expect(spectator.component.busy).toBe(true);
  });

  it('should have visibleChange emitter', () => {
    expect(spectator.component.visibleChange).toBeDefined();
  });
});

async function wait0ms() {  
  await firstValueFrom(timer(0));
}

async function wait300ms() {  
  await firstValueFrom(timer(300));
}
