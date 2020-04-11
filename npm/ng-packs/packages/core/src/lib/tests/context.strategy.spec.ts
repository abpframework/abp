import { ComponentRef } from '@angular/core';
import {
  ComponentContextStrategy,
  CONTEXT_STRATEGY,
  NoContextStrategy,
  TemplateContextStrategy,
} from '../strategies';
import { uuid } from '../utils';

describe('ComponentContextStrategy', () => {
  describe('#setContext', () => {
    let componentRef: ComponentRef<any>;

    beforeEach(
      () =>
        (componentRef = {
          instance: {
            x: '',
            y: '',
            z: '',
          },
          changeDetectorRef: {
            detectChanges: jest.fn(),
          },
        } as any),
    );

    test.each`
      props              | values
      ${['x']}           | ${[uuid()]}
      ${['x', 'y']}      | ${[uuid(), uuid()]}
      ${['x', 'y', 'z']} | ${[uuid(), uuid(), uuid()]}
    `(
      'should set $props as $values and call detectChanges once',
      ({ props, values }: { props: string[]; values: string[] }) => {
        const context = {};
        props.forEach((prop, i) => {
          context[prop] = values[i];
        });

        const strategy = new ComponentContextStrategy(context);
        strategy.setContext(componentRef);

        expect(props.every(prop => componentRef.instance[prop] === context[prop])).toBe(true);
        expect(componentRef.changeDetectorRef.detectChanges).toHaveBeenCalledTimes(1);
      },
    );
  });
});

describe('NoContextStrategy', () => {
  describe('#setContext', () => {
    it('should return undefined', () => {
      const strategy = new NoContextStrategy();
      expect(strategy.setContext(null)).toBeUndefined();
    });
  });
});

describe('TemplateContextStrategy', () => {
  describe('#setContext', () => {
    it('should return context', () => {
      const context = { x: uuid() };
      const strategy = new TemplateContextStrategy(context);
      expect(strategy.setContext()).toEqual(context);
    });
  });
});

describe('CONTEXT_STRATEGY', () => {
  test.each`
    name           | Strategy
    ${'Component'} | ${ComponentContextStrategy}
    ${'None'}      | ${NoContextStrategy}
    ${'Template'}  | ${TemplateContextStrategy}
  `('should successfully map $name to $Strategy.name', ({ name, Strategy }) => {
    expect(CONTEXT_STRATEGY[name](undefined)).toEqual(new Strategy(undefined));
  });
});
