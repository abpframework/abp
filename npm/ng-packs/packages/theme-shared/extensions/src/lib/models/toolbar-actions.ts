import { Type } from '@angular/core';
import { O } from 'ts-toolbelt';
import {
  Action,
  ActionContributorCallback,
  ActionContributorCallbacks,
  ActionList,
  Actions,
  ActionsFactory,
} from './actions';

export class ToolbarActionList<R = any> extends ActionList<
  R,
  ToolbarAction<R> | ToolbarComponent<R>
> {}

export class ToolbarActions<R = any> extends Actions<ToolbarActionList<R>> {
  protected _ctor: Type<ToolbarActionList<R>> = ToolbarActionList;
}

export class ToolbarActionsFactory<R = any> extends ActionsFactory<ToolbarActions<R>> {
  protected _ctor: Type<ToolbarActions<R>> = ToolbarActions;
}

export class ToolbarAction<R = any> extends Action<R> {
  readonly text: string;
  readonly icon: string;

  constructor(options: ToolbarActionOptions<R>) {
    super(options.permission || '', options.visible, options.action);
    this.text = options.text;
    this.icon = options.icon || '';
  }

  static create<R = any>(options: ToolbarActionOptions<R>) {
    return new ToolbarAction<R>(options);
  }

  static createMany<R = any>(arrayOfOptions: ToolbarActionOptions<R>[]) {
    return arrayOfOptions.map(ToolbarAction.create);
  }
}

export class ToolbarComponent<R = any> extends Action<R> {
  readonly component: Type<any>;

  constructor(options: ToolbarComponentOptions<R>) {
    super(options.permission || '', options.visible, options.action);
    this.component = options.component;
  }

  static create<R = any>(options: ToolbarComponentOptions<R>) {
    return new ToolbarComponent<R>(options);
  }

  static createMany<R = any>(arrayOfOptions: ToolbarComponentOptions<R>[]) {
    return arrayOfOptions.map(ToolbarComponent.create);
  }
}

export type ToolbarActionOptions<R = any> = O.Optional<
  O.Writable<ToolbarAction<R>>,
  'permission' | 'visible' | 'icon'
>;

export type ToolbarComponentOptions<R = any> = O.Optional<
  O.Writable<ToolbarComponent<R>>,
  'permission' | 'visible' | 'action'
>;

export type ToolbarActionDefaults<R = any> = Record<string, ToolbarAction<R>[]>;
export type ToolbarActionContributorCallback<R = any> = ActionContributorCallback<
  ToolbarActionList<R>
>;
export type ToolbarActionContributorCallbacks<R = any> = ActionContributorCallbacks<
  ToolbarActionList<R>
>;
