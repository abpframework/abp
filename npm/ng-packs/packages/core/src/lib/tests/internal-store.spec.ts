import clone from 'just-clone';
import { take } from 'rxjs/operators';
import { DeepPartial } from '../models';
import { InternalStore } from '../utils';

const mockInitialState = {
  foo: {
    bar: {
      baz: [() => {}],
      qux: null as Promise<any>,
    },
    n: 0,
  },
  x: '',
  a: false,
};

type MockState = typeof mockInitialState;

const deepPatch1: DeepPartial<MockState> = { foo: { bar: { baz: [() => {}] } } };
const expected1: MockState = clone(mockInitialState);
expected1.foo.bar.baz = deepPatch1.foo.bar.baz;

const deepPatch2: DeepPartial<MockState> = { foo: { bar: { qux: Promise.resolve() } } };
const expected2: MockState = clone(mockInitialState);
expected2.foo.bar.qux = deepPatch2.foo.bar.qux;

const deepPatch3: DeepPartial<MockState> = { foo: { n: 1 } };
const expected3: MockState = clone(mockInitialState);
expected3.foo.n = deepPatch3.foo.n;

const deepPatch4: DeepPartial<MockState> = { x: 'X' };
const expected4: MockState = clone(mockInitialState);
expected4.x = deepPatch4.x;

const deepPatch5: DeepPartial<MockState> = { a: true };
const expected5: MockState = clone(mockInitialState);
expected5.a = deepPatch5.a;

describe('Internal Store', () => {
  describe('sliceState', () => {
    test.each`
      selector                                   | expected
      ${(state: MockState) => state.a}           | ${mockInitialState.a}
      ${(state: MockState) => state.x}           | ${mockInitialState.x}
      ${(state: MockState) => state.foo.n}       | ${mockInitialState.foo.n}
      ${(state: MockState) => state.foo.bar}     | ${mockInitialState.foo.bar}
      ${(state: MockState) => state.foo.bar.baz} | ${mockInitialState.foo.bar.baz}
      ${(state: MockState) => state.foo.bar.qux} | ${mockInitialState.foo.bar.qux}
    `(
      'should return observable $expected when selector is $selector',
      async ({ selector, expected }) => {
        const store = new InternalStore(mockInitialState);

        const value = await store.sliceState(selector).pipe(take(1)).toPromise();

        expect(value).toEqual(expected);
      },
    );
  });

  describe('patchState', () => {
    test.each`
      patch         | expected
      ${deepPatch1} | ${expected1}
      ${deepPatch2} | ${expected2}
      ${deepPatch3} | ${expected3}
      ${deepPatch4} | ${expected4}
      ${deepPatch5} | ${expected5}
    `('should set state as $expected when patch is $patch', ({ patch, expected }) => {
      const store = new InternalStore(mockInitialState);

      store.deepPatch(patch);

      expect(store.state).toEqual(expected);
    });
  });

  describe('sliceUpdate', () => {
    it('should return slice of update$ based on selector', done => {
      const store = new InternalStore(mockInitialState);

      const onQux$ = store.sliceUpdate(state => state.foo.bar.qux);

      onQux$.pipe(take(1)).subscribe(value => {
        expect(value).toEqual(deepPatch2.foo.bar.qux);
        done();
      });

      store.deepPatch(deepPatch1);
      store.deepPatch(deepPatch2);
    });
  });

  describe('reset', () => {
    it('should reset state to initialState', () => {
      const store = new InternalStore(mockInitialState);

      store.deepPatch(deepPatch1);
      store.reset();

      expect(store.state).toEqual(mockInitialState);
    });
  });
});
