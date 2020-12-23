import compare from 'just-compare';
import { BehaviorSubject, Subject } from 'rxjs';
import { distinctUntilChanged, filter, map } from 'rxjs/operators';
import { DeepPartial } from '../models';
import { deepMerge } from './object-utils';

export class InternalStore<State> {
  private state$ = new BehaviorSubject<State>(this.initialState);

  private update$ = new Subject<DeepPartial<State>>();

  get state() {
    return this.state$.value;
  }

  sliceState = <Slice>(
    selector: (state: State) => Slice,
    compareFn: (s1: Slice, s2: Slice) => boolean = compare,
  ) => this.state$.pipe(map(selector), distinctUntilChanged(compareFn));

  sliceUpdate = <Slice>(
    selector: (state: DeepPartial<State>) => Slice,
    filterFn = (x: Slice) => x !== undefined,
  ) => this.update$.pipe(map(selector), filter(filterFn));

  constructor(private initialState: State) {}

  patch(state: DeepPartial<State>) {
    this.state$.next(deepMerge(this.state, state));
    this.update$.next(state);
  }

  set(state: State) {
    this.state$.next(state);
    this.update$.next(state);
  }

  reset() {
    this.set(this.initialState);
  }
}
