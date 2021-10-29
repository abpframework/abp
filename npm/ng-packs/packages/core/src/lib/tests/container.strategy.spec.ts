import { ViewContainerRef } from '@angular/core';
import {
  ClearContainerStrategy,
  CONTAINER_STRATEGY,
  InsertIntoContainerStrategy,
} from '../strategies';

describe('ClearContainerStrategy', () => {
  const containerRef = {
    clear: jest.fn(),
    length: 7,
  } as any as ViewContainerRef;

  describe('#getIndex', () => {
    it('should return 0', () => {
      const strategy = new ClearContainerStrategy(containerRef);
      expect(strategy.getIndex()).toBe(0);
    });
  });

  describe('#prepare', () => {
    it('should call clear method of containerRef once', () => {
      const strategy = new ClearContainerStrategy(containerRef);
      strategy.prepare();
      expect(strategy.getIndex()).toBe(0);
      expect(containerRef.clear).toHaveBeenCalledTimes(1);
    });
  });
});

describe('InsertIntoContainerStrategy', () => {
  const containerRef = {
    clear: jest.fn(),
    length: 7,
  } as any as ViewContainerRef;

  describe('#getIndex', () => {
    test.each`
      index       | expected
      ${0}        | ${0}
      ${4}        | ${4}
      ${9}        | ${7}
      ${-1}       | ${0}
      ${Infinity} | ${7}
    `(
      'should return $expected when index is given $index',
      ({ index, expected }: { index: number; expected: number }) => {
        const strategy = new InsertIntoContainerStrategy(containerRef, index);
        expect(strategy.getIndex()).toBe(expected);
      },
    );
  });

  describe('#prepare', () => {
    it('should not call clear method of containerRef', () => {
      const strategy = new InsertIntoContainerStrategy(containerRef, 0);
      strategy.prepare();
      expect(containerRef.clear).not.toHaveBeenCalled();
    });
  });
});

describe('CONTAINER_STRATEGY', () => {
  const containerRef = {
    clear: jest.fn(),
    length: 7,
  } as any as ViewContainerRef;

  test.each`
    name         | Strategy                       | index
    ${'Clear'}   | ${ClearContainerStrategy}      | ${undefined}
    ${'Append'}  | ${InsertIntoContainerStrategy} | ${containerRef.length}
    ${'Prepend'} | ${InsertIntoContainerStrategy} | ${0}
    ${'Insert'}  | ${InsertIntoContainerStrategy} | ${4}
  `('should successfully map $name to $Strategy.name', ({ name, Strategy, index }) => {
    expect(CONTAINER_STRATEGY[name](containerRef, index)).toEqual(
      new Strategy(containerRef, index),
    );
  });
});
