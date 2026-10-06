import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FormControl, FormsModule, NG_VALUE_ACCESSOR, ReactiveFormsModule } from '@angular/forms';
import { createHostFactory, SpectatorHost } from '@ngneat/spectator/vitest';
import { AbstractNgModelComponent } from '../abstracts';

@Component({
  selector: 'abp-test',
  template: '',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: TestComponent,
      multi: true,
    },
  ],
  imports: [FormsModule],
})
export class TestComponent extends AbstractNgModelComponent implements OnInit {
  @Input() override: boolean;

  ngOnInit() {
    setTimeout(() => {
      if (this.override) {
        this.value = 'test';
      }
    }, 0);
  }
}

describe('AbstractNgModelComponent', () => {
  let spectator: SpectatorHost<TestComponent, { val: any; override: boolean }>;

  const createHost = createHostFactory({
    component: TestComponent,
    imports: [AbstractNgModelComponent, FormsModule],
  });

  beforeEach(() => {
    spectator = createHost('<abp-test [(ngModel)]="val" [override]="override"></abp-test>', {
      hostProps: {
        val: '1',
        override: false,
      },
    });
  });

  test('should create component successfully', () => {
    expect(spectator.component).toBeTruthy();
  });
});

@Component({
  selector: 'abp-test-disabled',
  template: '<input [disabled]="disabled" />',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: TestDisabledComponent,
      multi: true,
    },
  ],
})
export class TestDisabledComponent extends AbstractNgModelComponent {}

describe('AbstractNgModelComponent setDisabledState', () => {
  let spectator: SpectatorHost<TestDisabledComponent, { control: FormControl }>;

  const createHost = createHostFactory({
    component: TestDisabledComponent,
    imports: [ReactiveFormsModule],
  });

  test('should refresh the view when the form control is disabled or enabled', () => {
    const control = new FormControl('');
    spectator = createHost('<abp-test-disabled [formControl]="control"></abp-test-disabled>', {
      hostProps: { control },
    });
    const input = () => spectator.query('input') as HTMLInputElement;

    expect(input().disabled).toBe(false);

    control.disable();
    spectator.detectChanges();
    expect(input().disabled).toBe(true);

    control.enable();
    spectator.detectChanges();
    expect(input().disabled).toBe(false);
  });
});
