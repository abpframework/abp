import { ConfirmationService } from '@abp/ng.theme.shared';
import { CoreTestingModule } from '@abp/ng.core/testing';
import { Component, EventEmitter, Input } from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { Confirmation } from '@abp/ng.theme.shared';
import { Subject, timer } from 'rxjs';
import { ModalComponent } from '../components/modal/modal.component';

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
const disappearFn = jest.fn();

describe('ModalComponent', () => {
  let spectator: Spectator<TestHostComponent>;

  const createComponent = createComponentFactory({
    component: TestHostComponent,
    imports: [CoreTestingModule.withConfig()],
    providers: [
      {
        provide: ConfirmationService,
        useValue: {
          warn: jest.fn(() => mockConfirmation$),
        },
      },
    ],
  });

  beforeEach(() => {
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
  await timer(0).toPromise();
}

async function wait300ms() {
  await timer(300).toPromise();
}
