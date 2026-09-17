import { Component, inject, input, output } from '@angular/core';
import { Router } from '@angular/router';
import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/vitest';
import { BehaviorSubject } from 'rxjs';
import { ReplaceableTemplateDirective } from '../directives/replaceable-template.directive';
import { ReplaceableComponents } from '../models/replaceable-components';
import { ReplaceableComponentsService } from '../services/replaceable-components.service';
import { setInputSignal } from './utils';
@Component({
  selector: 'abp-default-component',
  template: ' <p>default</p> ',
  exportAs: 'abpDefaultComponent',
})
class DefaultComponent {
  onOneWay = input<any>();

  twoWay = input<boolean>();

  readonly twoWayChange = output<boolean>();

  readonly someOutput = output<string>();

  setTwoWay(value) {
    setInputSignal(this.twoWay, value);
    this.twoWayChange.emit(value);
  }
}

@Component({
  selector: 'abp-external-component',
  template: ' <p>external</p> ',
})
class ExternalComponent {
  data = inject<ReplaceableComponents.ReplaceableTemplateData<any, any>>('REPLACEABLE_DATA' as any)!;
}

describe('ReplaceableTemplateDirective', () => {
  let spectator: SpectatorDirective<ReplaceableTemplateDirective>;
  const get$Res = new BehaviorSubject(undefined);

  const createDirective = createDirectiveFactory({
    directive: ReplaceableTemplateDirective,
    imports: [DefaultComponent, ExternalComponent],
    mocks: [Router],
    providers: [{ provide: ReplaceableComponentsService, useValue: { get$: () => get$Res } }],
  });

  describe('without external component', () => {
    const twoWayChange = vi.fn(a => a);
    const someOutput = vi.fn(a => a);

    beforeEach(() => {
      spectator = createDirective(
        `
        <ng-template abpReplaceableTemplate let-initTemplate="initTemplate">
          <abp-default-component #defaultComponent="abpDefaultComponent"></abp-default-component>
        </ng-template>
        `,
        {
          detectChanges: false,
          hostProps: {
            oneWay: { label: 'Test' },
            twoWay: false,
            twoWayChange,
            someOutput,
          },
        },
      );
      setInputSignal(spectator.directive.data, {
        inputs: {
          oneWay: { value: { label: 'Test' } },
          twoWay: { value: false, twoWay: true },
        },
        outputs: { twoWayChange, someOutput },
        componentKey: 'TestModule.TestComponent',
      });
      spectator.detectChanges();
    });

    it('should create directive successfully', () => {
      expect(spectator.directive).toBeTruthy();
    });
  });

  describe('with external component', () => {
    it('should create directive successfully', () => {
      spectator = createDirective(
        `
        <ng-template abpReplaceableTemplate let-initTemplate="initTemplate">
          <abp-default-component #defaultComponent="abpDefaultComponent"></abp-default-component>
        </ng-template>
        `,
        {
          detectChanges: false,
        },
      );
      setInputSignal(spectator.directive.data, {
        inputs: {
          oneWay: { value: { label: 'Test' } },
          twoWay: { value: false, twoWay: true },
        },
        outputs: { twoWayChange: vi.fn(), someOutput: vi.fn() },
        componentKey: 'TestModule.TestComponent',
      });
      expect(spectator.directive).toBeTruthy();
    });
  });
});
