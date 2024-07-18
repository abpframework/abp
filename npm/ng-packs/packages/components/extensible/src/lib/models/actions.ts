import { LinkedList } from '@abp/utils';
import { InjectFlags, InjectionToken, InjectOptions, Type } from '@angular/core';

export abstract class ActionList<R = any, A = Action<R>> extends LinkedList<A> {}

export abstract class ActionData<R = any> {
  abstract getInjected: <T>(
    token: Type<T> | InjectionToken<T>,
    notFoundValue?: T,
    flags?: InjectOptions | InjectFlags,
  ) => T;
  index?: number;
  abstract record: R;

  get data(): ReadonlyActionData<R> {
    return {
      getInjected: this.getInjected,
      index: this.index,
      record: this.record,
    };
  }
}

export type ReadonlyActionData<R = any> = Readonly<Omit<ActionData<R>, 'data'>>;

export abstract class Action<R = any> {
  constructor(
    public readonly permission: string,
    public readonly visible: ActionPredicate<R> = () => true,
    public readonly action: ActionCallback<R> = () => {},
    public readonly btnClass?: string,
    public readonly btnStyle?: string,
  ) {}
}

export type ActionCallback<T, R = any> = (data: Omit<ActionData<T>, 'data'>) => R;
export type ActionPredicate<T> = (data?: Omit<ActionData<T>, 'data'>) => boolean;

export abstract class ActionsFactory<C extends Actions<any>> {
  protected abstract _ctor: Type<C>;
  private contributorCallbacks: ActionContributorCallbacks<InferredActionList<C>> = {};

  get(name: string): C {
    this.contributorCallbacks[name] = this.contributorCallbacks[name] || [];

    return new this._ctor(this.contributorCallbacks[name]);
  }
}

export abstract class Actions<L extends ActionList<any, InferredAction<L>>> {
  protected abstract _ctor: Type<L>;

  get actions(): L {
    const actionList = new this._ctor();

    this.callbackList.forEach(callback => callback(actionList));

    return actionList;
  }

  constructor(private readonly callbackList: ActionContributorCallback<L>[]) {}

  addContributor(contributeCallback: ActionContributorCallback<L>) {
    this.callbackList.push(contributeCallback);
  }

  clearContributors() {
    while (this.callbackList.length) this.callbackList.pop();
  }
}

export type ActionContributorCallbacks<L extends ActionList<any, InferredAction<L>>> = Record<
  string,
  ActionContributorCallback<L>[]
>;

export type ActionContributorCallback<L extends ActionList<any, InferredAction<L>>> = (
  actionList: L,
) => any;

type InferredActionList<C> = C extends Actions<infer L> ? L : never;
export type InferredAction<T> = T extends ActionList<any, infer U> ? U : T;
