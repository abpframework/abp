import { InjectionToken } from '@angular/core';
import { ActionCallback, ReadonlyActionData as ActionData } from '../models/actions';
import { ExtensionsService } from '../services/extensions.service';
import { Observable } from 'rxjs';

export const EXTENSIONS_IDENTIFIER = new InjectionToken<string>('EXTENSIONS_IDENTIFIER');
export type ActionKeys = Extract<'entityActions' | 'toolbarActions', keyof ExtensionsService>;

export const EXTENSIONS_ACTION_TYPE = new InjectionToken<ActionKeys>('EXTENSIONS_ACTION_TYPE');

export const EXTENSIONS_ACTION_DATA = new InjectionToken<ActionData>('EXTENSIONS_ACTION_DATA');
export const EXTENSIONS_ACTION_CALLBACK = new InjectionToken<ActionCallback<unknown>>(
  'EXTENSIONS_ACTION_DATA',
);
export const PROP_DATA_STREAM = new InjectionToken<Observable<any>>('PROP_DATA_STREAM');
