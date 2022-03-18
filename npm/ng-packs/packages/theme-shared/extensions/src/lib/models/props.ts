import { LinkedList } from '@abp/utils';
import { InjectFlags, InjectionToken, Type } from '@angular/core';
import { O } from 'ts-toolbelt';
import { ePropType } from '../enums/props.enum';

export abstract class PropList<R = any, A = Prop<R>> extends LinkedList<A> {}

export abstract class PropData<R = any> {
  abstract getInjected: <T>(
    token: Type<T> | InjectionToken<T>,
    notFoundValue?: T,
    flags?: InjectFlags,
  ) => T;
  index?: number;
  abstract record: R;

  get data(): ReadonlyPropData<R> {
    return {
      getInjected: this.getInjected,
      index: this.index,
      record: this.record,
    };
  }
}

export type ReadonlyPropData<R = any> = O.Readonly<Omit<PropData<R>, 'data'>>;

export abstract class Prop<R = any> {
  constructor(
    public readonly type: ePropType,
    public readonly name: string,
    public readonly displayName: string,
    public readonly permission: string,
    public readonly visible: PropPredicate<R> = _ => true,
    public readonly isExtra = false,
  ) {
    this.displayName = this.displayName || this.name;
  }
}

export type PropCallback<T, R = any> = (data: Omit<PropData<T>, 'data'>, auxData?: any) => R;
export type PropPredicate<T> = (data?: Omit<PropData<T>, 'data'>, auxData?: any) => boolean;

export abstract class PropsFactory<C extends Props<any>> {
  protected abstract _ctor: Type<C>;
  private contributorCallbacks: PropContributorCallbacks<InferredPropList<C>> = {};

  get(name: string): C {
    this.contributorCallbacks[name] = this.contributorCallbacks[name] || [];

    return new this._ctor(this.contributorCallbacks[name]);
  }
}

export abstract class Props<L extends PropList> {
  protected abstract _ctor: Type<L>;

  get props(): L {
    const propList = new this._ctor();

    this.callbackList.forEach(callback => callback(propList));

    return propList;
  }

  constructor(private readonly callbackList: PropContributorCallback<L>[]) {}

  addContributor(contributeCallback: PropContributorCallback<L>) {
    this.callbackList.push(contributeCallback);
  }

  clearContributors() {
    while (this.callbackList.length) this.callbackList.pop();
  }
}

export type PropContributorCallbacks<L extends PropList<any>> = Record<
  string,
  PropContributorCallback<L>[]
>;

export type PropContributorCallback<L extends PropList<any>> = (propList: L) => any;

type InferredPropList<C> = C extends Props<infer L> ? L : never;
