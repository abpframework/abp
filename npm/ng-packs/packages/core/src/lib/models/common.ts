import { Config } from './config';
import { eLayoutType } from '../enums/common';

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
}
