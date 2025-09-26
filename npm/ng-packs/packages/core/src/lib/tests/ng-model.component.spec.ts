import { Component, Input, OnInit } from '@angular/core';
import { FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
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
