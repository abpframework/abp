import { Injectable } from '@angular/core';
import { EntityActionsFactory } from '../models/entity-actions';
import { EntityPropsFactory } from '../models/entity-props';
import { CreateFormPropsFactory, EditFormPropsFactory } from '../models/form-props';
import { ToolbarActionsFactory } from '../models/toolbar-actions';

@Injectable({
  providedIn: 'root',
})
export class ExtensionsService<R = any> {
  readonly entityActions = new EntityActionsFactory<R>();
  readonly toolbarActions = new ToolbarActionsFactory<R[]>();
  readonly entityProps = new EntityPropsFactory<R>();
  readonly createFormProps = new CreateFormPropsFactory<R>();
  readonly editFormProps = new EditFormPropsFactory<R>();
}
