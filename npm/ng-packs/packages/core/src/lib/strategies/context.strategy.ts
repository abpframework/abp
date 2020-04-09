import { ComponentRef, TemplateRef, Type } from '@angular/core';
import { InferedContextOf, InferedInstanceOf } from '../models';

export abstract class ContextStrategy<T = any> {
  constructor(public context: Partial<ContextType<T>>) {}

  /* tslint:disable-next-line:no-unused-variable */
  setContext(componentRef?: ComponentRef<InferedInstanceOf<T>>): Partial<ContextType<T>> {
    return this.context;
  }
}

export class NoContextStrategy<
  T extends Type<any> | TemplateRef<any> = any
> extends ContextStrategy<T> {
  constructor() {
    super(undefined);
  }
}

export class ComponentContextStrategy<T extends Type<any> = any> extends ContextStrategy<T> {
  setContext(componentRef: ComponentRef<InferedInstanceOf<T>>): Partial<InferedInstanceOf<T>> {
    Object.keys(this.context).forEach(key => (componentRef.instance[key] = this.context[key]));
    componentRef.changeDetectorRef.detectChanges();
    return this.context;
  }
}

export class TemplateContextStrategy<T extends TemplateRef<any> = any> extends ContextStrategy<T> {
  setContext(): Partial<InferedContextOf<T>> {
    return this.context;
  }
}

export const CONTEXT_STRATEGY = {
  None<T extends Type<any> | TemplateRef<any> = any>() {
    return new NoContextStrategy<T>();
  },
  Component<T extends Type<any> = any>(context: Partial<InferedInstanceOf<T>>) {
    return new ComponentContextStrategy<T>(context);
  },
  Template<T extends TemplateRef<any> = any>(context: Partial<InferedContextOf<T>>) {
    return new TemplateContextStrategy<T>(context);
  },
};

type ContextType<T> = T extends Type<infer U> | TemplateRef<infer U> ? U : never;
