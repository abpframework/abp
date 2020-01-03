import { eLayoutType } from '../enums/common';
import { Config } from './config';
import { EventEmitter } from '@angular/core';
import { Subject } from 'rxjs';

export namespace ABP {
  export interface Root {
    environment: Partial<Config.Environment>;
    requirements: Config.Requirements;
  }

  export type PagedResponse<T> = {
    totalCount: number;
  } & PagedItemsResponse<T>;

  export interface PagedItemsResponse<T> {
    items: T[];
  }

  export interface PageQueryParams {
    filter?: string;
    sorting?: string;
    skipCount?: number;
    maxResultCount?: number;
  }

  export interface Route {
    children?: Route[];
    invisible?: boolean;
    layout?: eLayoutType;
    name: string;
    order?: number;
    parentName?: string;
    path: string;
    requiredPolicy?: string;
    iconClass?: string;
  }

  export interface FullRoute extends Route {
    url?: string;
    wrapper?: boolean;
  }

  export interface BasicItem {
    id: string;
    name: string;
  }

  export interface Dictionary<T = any> {
    [key: string]: T;
  }

  export type ExtractFromOutput<
    T extends EventEmitter<any> | Subject<any>
  > = T extends EventEmitter<infer X> ? X : T extends Subject<infer Y> ? Y : never;
}
