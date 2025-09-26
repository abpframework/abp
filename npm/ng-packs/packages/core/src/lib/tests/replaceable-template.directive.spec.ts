import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { Router } from '@angular/router';
import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/jest';
import { BehaviorSubject } from 'rxjs';
import { ReplaceableTemplateDirective } from '../directives/replaceable-template.directive';
import { ReplaceableComponents } from '../models/replaceable-components';
import { ReplaceableComponentsService } from '../services/replaceable-components.service';

@Component({
  selector: 'abp-default-component',
  template: ' <p>default</p> ',
  exportAs: 'abpDefaultComponent'
})
class DefaultComponent {
  @Input()
  oneWay;

  @Input()
  twoWay: boolean;

  @Output()
  readonly twoWayChange = new EventEmitter<boolean>();

  @Output()
  readonly someOutput = new EventEmitter<string>();

  setTwoWay(value) {
    this.twoWay = value;
    this.twoWayChange.emit(value);
  }
}

@Component({
  selector: 'abp-external-component',
  template: ' <p>external</p> '
})
class ExternalComponent {
  data = inject<ReplaceableComponents.ReplaceableTemplateData<any, any>>('REPLACEABLE_DATA' as any, { optional: true })!;
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
    const twoWayChange = jest.fn(a => a);
    const someOutput = jest.fn(a => a);

    beforeEach(() => {
      spectator = createDirective(
        `
        <div *abpReplaceableTemplate="{inputs: {oneWay: {value: oneWay}, twoWay: {value: twoWay, twoWay: true}}, outputs: {twoWayChange: twoWayChange, someOutput: someOutput}, componentKey: 'TestModule.TestComponent'}; let initTemplate = initTemplate">
          <abp-default-component #defaultComponent="abpDefaultComponent"></abp-default-component>
        </div>
        `,
        {
          hostProps: {
            oneWay: { label: 'Test' },
            twoWay: false,
            twoWayChange,
            someOutput,
          },
        },
      );
    });

    it('should create directive successfully', () => {
      expect(spectator.directive).toBeTruthy();
    });
  });

  describe('with external component', () => {
    it('should create directive successfully', () => {
      spectator = createDirective(
        `
        <div *abpReplaceableTemplate="{inputs: {oneWay: {value: oneWay}, twoWay: {value: twoWay, twoWay: true}}, outputs: {twoWayChange: twoWayChange, someOutput: someOutput}, componentKey: 'TestModule.TestComponent'}; let initTemplate = initTemplate">
          <abp-default-component #defaultComponent="abpDefaultComponent"></abp-default-component>
        </div>
        `,
        {
          hostProps: {
            oneWay: { label: 'Test' },
            twoWay: false,
            twoWayChange: jest.fn(),
            someOutput: jest.fn(),
          },
        },
      );
      expect(spectator.directive).toBeTruthy();
    });
  });
});
