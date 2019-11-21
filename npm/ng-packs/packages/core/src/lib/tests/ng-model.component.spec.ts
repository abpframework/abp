import { Component, forwardRef, Input, OnInit } from '@angular/core';
import { FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { AbstractNgModelComponent } from '../abstracts';
import { timer } from 'rxjs';

@Component({
  selector: 'abp-test',
  template: '',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      // tslint:disable-next-line: no-forward-ref
      useExisting: forwardRef(() => TestComponent),
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
  let spectator: SpectatorHost<TestComponent>;

  const createHost = createHostFactory({
    component: TestComponent,
    declarations: [AbstractNgModelComponent],
    imports: [FormsModule],
    detectChanges: false,
  });

  test('should pass the value with ngModel', done => {
    spectator = createHost('<abp-test [(ngModel)]="val"></abp-test>', {
      hostProps: {
        val: '1',
      },
    });

    spectator.detectChanges();

    timer(0).subscribe(() => {
      expect(spectator.component.value).toBe('1');
      done();
    });
  });

  test.skip('should set the value with ngModel', done => {
    spectator = createHost('<abp-test [(ngModel)]="val" [override]="true"></abp-test>', {
      hostProps: {
        val: '2',
      },
    });

    spectator.detectChanges();

    timer(0).subscribe(() => {
      expect(spectator.hostComponent['val']).toBe('test');
      done();
    });
  });

  test.skip('should not change value when disable is true', done => {
    spectator = createHost('<abp-test [(ngModel)]="val" [disabled]="true"></abp-test>', {
      hostProps: {
        val: '2',
      },
    });

    spectator.component['val'] = '3';
    spectator.detectChanges();

    timer(0).subscribe(() => {
      expect(spectator.hostComponent['val']).toBe('2');
      done();
    });
  });
});
