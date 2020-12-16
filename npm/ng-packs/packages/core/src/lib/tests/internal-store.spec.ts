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
const deepPatchExpected1: MockState = clone(mockInitialState);
deepPatchExpected1.foo.bar.baz = deepPatch1.foo.bar.baz;

const deepPatch2: DeepPartial<MockState> = { foo: { bar: { qux: Promise.resolve() } } };
const deepPatchExpected2: MockState = clone(mockInitialState);
deepPatchExpected2.foo.bar.qux = deepPatch2.foo.bar.qux;

const deepPatch3: DeepPartial<MockState> = { foo: { n: 1 } };
const deepPatchExpected3: MockState = clone(mockInitialState);
deepPatchExpected3.foo.n = deepPatch3.foo.n;

const deepPatch4: DeepPartial<MockState> = { x: 'X' };
const deepPatchExpected4: MockState = clone(mockInitialState);
deepPatchExpected4.x = deepPatch4.x;

const deepPatch5: DeepPartial<MockState> = { a: true };
const deepPatchExpected5: MockState = clone(mockInitialState);
deepPatchExpected5.a = deepPatch5.a;

const patch1: Partial<MockState> = {
  foo: { bar: { baz: [() => {}] } } as typeof mockInitialState.foo,
};
const patchExpected1: MockState = clone(mockInitialState);
patchExpected1.foo = patch1.foo;

const patch2: Partial<MockState> = {
  foo: { bar: { qux: Promise.resolve() } } as typeof mockInitialState.foo,
};
const patchExpected2: MockState = clone(mockInitialState);
patchExpected2.foo = patch2.foo;

const patch3: Partial<MockState> = { foo: { n: 1 } as typeof mockInitialState.foo };
const patchExpected3: MockState = clone(mockInitialState);
patchExpected3.foo = patch3.foo;

const patch4: Partial<MockState> = { x: 'X' };
const patchExpected4: MockState = clone(mockInitialState);
patchExpected4.x = patch4.x;

const patch5: Partial<MockState> = { a: true };
const patchExpected5: MockState = clone(mockInitialState);
patchExpected5.a = patch5.a;

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

  describe('deepPatchState', () => {
    test.each`
      patch         | expected
      ${deepPatch1} | ${deepPatchExpected1}
      ${deepPatch2} | ${deepPatchExpected2}
      ${deepPatch3} | ${deepPatchExpected3}
      ${deepPatch4} | ${deepPatchExpected4}
      ${deepPatch5} | ${deepPatchExpected5}
    `('should set state as $expected when patch is $patch', ({ patch, expected }) => {
      const store = new InternalStore(mockInitialState);

      store.deepPatch(patch);

      expect(store.state).toEqual(expected);
    });
  });

  describe('patchState', () => {
    test.each`
      patch     | expected
      ${patch1} | ${patchExpected1}
      ${patch2} | ${patchExpected2}
      ${patch3} | ${patchExpected3}
      ${patch4} | ${patchExpected4}
      ${patch5} | ${patchExpected5}
    `('should set state as $expected when patch is $patch', ({ patch, expected }) => {
      const store = new InternalStore(mockInitialState);

      store.patch(patch);

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
