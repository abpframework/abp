import { Component, EventEmitter, Inject, Input, Optional, Output } from '@angular/core';
import { Router } from '@angular/router';
import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/jest';
import { BehaviorSubject } from 'rxjs';
import { ReplaceableTemplateDirective } from '../directives';
import { ReplaceableComponents } from '../models';
import { ReplaceableComponentsService } from '../services/replaceable-components.service';

@Component({
  selector: 'abp-default-component',
  template: ' <p>default</p> ',
  exportAs: 'abpDefaultComponent',
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
  template: ' <p>external</p> ',
})
class ExternalComponent {
  constructor(
    @Optional()
    @Inject('REPLACEABLE_DATA')
    public data: ReplaceableComponents.ReplaceableTemplateData<any, any>,
  ) {}
}

describe('ReplaceableTemplateDirective', () => {
  let spectator: SpectatorDirective<ReplaceableTemplateDirective>;
  const get$Res = new BehaviorSubject(undefined);

  const createDirective = createDirectiveFactory({
    directive: ReplaceableTemplateDirective,
    declarations: [DefaultComponent, ExternalComponent],
    entryComponents: [ExternalComponent],
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

      const component = spectator.query(DefaultComponent);
      spectator.directive.context.initTemplate(component);
      spectator.detectChanges();
    });

    afterEach(() => twoWayChange.mockClear());

    it('should display the default template when store response is undefined', () => {
      expect(spectator.query('abp-default-component')).toBeTruthy();
    });

    it('should be setted inputs and outputs', () => {
      const component = spectator.query(DefaultComponent);
      expect(component.oneWay).toEqual({ label: 'Test' });
      expect(component.twoWay).toEqual(false);
    });

    it('should change the component inputs', () => {
      const component = spectator.query(DefaultComponent);
      spectator.setHostInput({ oneWay: 'test' });
      component.setTwoWay(true);
      component.someOutput.emit('someOutput emitted');
      expect(component.oneWay).toBe('test');
      expect(twoWayChange).toHaveBeenCalledWith(true);
      expect(someOutput).toHaveBeenCalledWith('someOutput emitted');
    });
  });

  describe('with external component', () => {
    const twoWayChange = jest.fn(a => a);
    const someOutput = jest.fn(a => a);

    beforeEach(() => {
      spectator = createDirective(
        `
        <div *abpReplaceableTemplate="{inputs: {oneWay: {value: oneWay}, twoWay: {value: twoWay, twoWay: true}}, outputs: {twoWayChange: twoWayChange, someOutput: someOutput}, componentKey: 'TestModule.TestComponent'}; let initTemplate = initTemplate">
          <abp-default-component #defaultComponent="abpDefaultComponent"></abp-default-component>
        </div>
        `,
        { hostProps: { oneWay: { label: 'Test' }, twoWay: false, twoWayChange, someOutput } },
      );

      get$Res.next({ component: ExternalComponent, key: 'TestModule.TestComponent' });
    });

    afterEach(() => twoWayChange.mockClear());

    it('should display the external component', () => {
      expect(spectator.query('p')).toHaveText('external');
    });

    it('should be injected the data object', () => {
      const externalComponent = spectator.query(ExternalComponent);
      expect(externalComponent.data).toEqual({
        componentKey: 'TestModule.TestComponent',
        inputs: { oneWay: { label: 'Test' }, twoWay: false },
        outputs: { someOutput, twoWayChange },
      });
    });

    it('should be worked all data properties', () => {
      const externalComponent = spectator.query(ExternalComponent);
      spectator.setHostInput({ oneWay: 'test' });
      externalComponent.data.inputs.twoWay = true;
      externalComponent.data.outputs.someOutput('someOutput emitted');
      expect(externalComponent.data.inputs.oneWay).toBe('test');
      expect(twoWayChange).toHaveBeenCalledWith(true);
      expect(someOutput).toHaveBeenCalledWith('someOutput emitted');

      spectator.setHostInput({ twoWay: 'twoWay test' });
      expect(externalComponent.data.inputs.twoWay).toBe('twoWay test');
    });

    it('should be worked correctly the default component when the external component has been removed from store', () => {
      expect(spectator.query('p')).toHaveText('external');
      const externalComponent = spectator.query(ExternalComponent);
      spectator.setHostInput({ oneWay: 'test' });
      externalComponent.data.inputs.twoWay = true;
      get$Res.next({ component: null, key: 'TestModule.TestComponent' });
      spectator.detectChanges();
      const component = spectator.query(DefaultComponent);
      spectator.directive.context.initTemplate(component);
      expect(spectator.query('abp-default-component')).toBeTruthy();

      expect(component.oneWay).toEqual('test');
      expect(component.twoWay).toEqual(true);
    });

    it('should reset default component subscriptions', () => {
      get$Res.next({ component: null, key: 'TestModule.TestComponent' });
      const component = spectator.query(DefaultComponent);
      spectator.directive.context.initTemplate(component);
      spectator.detectChanges();
      const unsubscribe = jest.fn(() => {});
      spectator.directive.defaultComponentSubscriptions.twoWayChange.unsubscribe = unsubscribe;

      get$Res.next({ component: ExternalComponent, key: 'TestModule.TestComponent' });
      expect(unsubscribe).toHaveBeenCalled();
    });
  });
});
