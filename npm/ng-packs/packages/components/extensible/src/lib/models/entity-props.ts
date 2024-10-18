import { ABP, escapeHtmlChars } from '@abp/ng.core';
import { InjectFlags, InjectOptions, InjectionToken, Type } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ActionCallback } from './actions';
import {
  Prop,
  PropCallback,
  PropContributorCallback,
  PropContributorCallbacks,
  PropList,
  Props,
  PropsFactory,
} from './props';
import { FormPropTooltip } from './form-props';
import { FilteredWithOptions, PartialWithOptions } from '../utils/model.utils';

export class EntityPropList<R = any> extends PropList<R, EntityProp<R>> {}

export class EntityProps<R = any> extends Props<EntityPropList<R>> {
  protected _ctor: Type<EntityPropList<R>> = EntityPropList;
}

export class EntityPropsFactory<R = any> extends PropsFactory<EntityProps<R>> {
  protected _ctor: Type<EntityProps<R>> = EntityProps;
}

export class EntityProp<R = any> extends Prop<R> {
  readonly columnWidth: number | undefined;
  readonly sortable: boolean;
  readonly valueResolver: PropCallback<R, Observable<any>>;
  readonly action?: ActionCallback<R>;
  readonly component?: Type<any>;
  readonly enumList?: Array<ABP.Option<any>>;
  readonly tooltip?: FormPropTooltip;
  readonly columnVisible: ColumnPredicate;

  constructor(options: EntityPropOptions<R>) {
    super(
      options.type,
      options.name,
      options.displayName || '',
      options.permission || '',
      options.visible,
      options.isExtra,
    );

    this.columnVisible = options.columnVisible || (() => true);
    this.columnWidth = options.columnWidth;
    this.sortable = options.sortable || false;
    this.valueResolver =
      options.valueResolver ||
      (data => of(escapeHtmlChars((data.record as PropDataObject)[this.name])));
    if (options.action) {
      this.action = options.action;
    }
    if (options.component) {
      this.component = options.component;
    }
    if (options.enumList) {
      this.enumList = options.enumList;
    }
    this.tooltip = options.tooltip;
  }

  static create<R = any>(options: EntityPropOptions<R>) {
    return new EntityProp<R>(options);
  }

  static createMany<R = any>(arrayOfOptions: EntityPropOptions<R>[]) {
    return arrayOfOptions.map(EntityProp.create);
  }
}

type OptionalKeys =
  | 'permission'
  | 'visible'
  | 'columnVisible'
  | 'displayName'
  | 'isExtra'
  | 'columnWidth'
  | 'sortable'
  | 'valueResolver'
  | 'action'
  | 'component'
  | 'enumList';
export type EntityPropOptions<R = any> = PartialWithOptions<EntityProp<R>, OptionalKeys> &
  FilteredWithOptions<EntityProp<R>, OptionalKeys>;

export type EntityPropDefaults<R = any> = Record<string, EntityProp<R>[]>;
export type EntityPropContributorCallback<R = any> = PropContributorCallback<EntityPropList<R>>;
export type EntityPropContributorCallbacks<R = any> = PropContributorCallbacks<EntityPropList<R>>;
export type ColumnPredicate = (getInjected: GetInjected, auxData?: any) => boolean;
export type GetInjected = <T>(
  token: Type<T> | InjectionToken<T>,
  notFoundValue?: T,
  options?: InjectOptions | InjectFlags,
) => T;
type PropDataObject = { [key: string]: any };
