import { Component, forwardRef, Input, OnInit } from '@angular/core';
import { FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import { createHostFactory, SpectatorHost } from '@ngneat/spectator';
import { AbstractNgModelComponent } from '../abstracts';

@Component({
  selector: 'abp-test',
  template: '',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
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
  const createHost = createHostFactory({
    component: TestComponent,
    declarations: [AbstractNgModelComponent],
    imports: [FormsModule],
  });

  it('should pass the value with ngModel', done => {
    const spectator = createHost('<abp-test [(ngModel)]="val"></abp-test>', {
      hostProps: {
        val: '1',
      },
    });

    setTimeout(() => {
      expect(spectator.component.value).toBe('1');
      done();
    }, 0);
  });

  it('should set the value with ngModel', done => {
    const spectator = createHost('<abp-test [(ngModel)]="val" [override]="override"></abp-test>', {
      hostProps: {
        val: '2',
        override: true,
      },
    });

    setTimeout(() => {
      expect(spectator.hostComponent['val']).toBe('test');
      done();
    }, 0);
  });

  it('should not change value when disable is true', done => {
    const spectator = createHost('<abp-test [(ngModel)]="val" [disabled]="true"></abp-test>', {
      hostProps: {
        val: '2',
      },
    });

    spectator.component['val'] = '3';

    setTimeout(() => {
      expect(spectator.hostComponent['val']).toBe('2');
      done();
    }, 0);
  });
});
