import { Component, Input, OnInit } from '@angular/core';
import { FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { timer } from 'rxjs';
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
    declarations: [AbstractNgModelComponent],
    imports: [FormsModule],
  });

  beforeEach(() => {
    spectator = createHost('<abp-test [(ngModel)]="val" [override]="override"></abp-test>', {
      hostProps: {
        val: '1',
        override: false,
      },
    });
  });

  test('should pass the value with ngModel', done => {
    timer(0).subscribe(() => {
      expect(spectator.component.value).toBe('1');
      done();
    });
  });

  test('should set the value with ngModel', done => {
    spectator.setHostInput({ val: '2', override: true });

    timer(0).subscribe(() => {
      expect(spectator.hostComponent.val).toBe('test');
      done();
    });
  });
});
